using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace FluentCss.Core;

public class ClassRegistry : IEnumerable<UtilityClass>
{
    public event EventHandler<UtilityClass>? OnClassAdded;
    private readonly ConcurrentSet<UtilityClass> _classes = new();

    // Rules are emitted in buckets so that variants always come after the rules
    // they are meant to override. Without this the shared registry could emit a
    // media variant (e.g. min-md:flex) before the base utility it overrides
    // (e.g. hidden), because classes are deduplicated in the order they were
    // first registered across the whole process.
    //
    // Only the media/non-media split is required: a pseudo (\:hover) or theme
    // (.theme-dark .x) selector already has a higher specificity than the base
    // rule, so it wins regardless of source order. Every media combination can
    // therefore share one final bucket, leaving five buckets:
    //
    //   0 base, 1 pseudo, 2 theme, 3 theme + pseudo, 4 any media
    private const int BucketCount = 5;
    private readonly StringBuilder[] _buckets;
    private readonly object _cssBuilderLock = new();
    private readonly ConcurrentLazyObject<string> _cssObject;
    private readonly ConcurrentLazyObject<byte[]> _bytesObject;
    private readonly ConcurrentLazyObject<byte[]> _sha256Object;
    private readonly ConcurrentLazyObject<string> _sha256HexObject;

    public string Css => _cssObject.Get();

    public byte[] CssUtf8 => _bytesObject.Get();
    public byte[] Sha256 => _sha256Object.Get();
    public string Sha256Hex => _sha256HexObject.Get();

    public static ClassRegistry DefaultRegistry { get; } = new();

    public ClassRegistry()
    {
        _buckets = new StringBuilder[BucketCount];

        for (int i = 0; i < BucketCount; i++)
        {
            _buckets[i] = new StringBuilder();
        }

        _cssObject = new(() =>
        {
            lock (_cssBuilderLock)
            {
                StringBuilder result = StringBuilderBag.TakeOrNew();

                for (int i = 0; i < BucketCount; i++)
                {
                    result.Append(_buckets[i]);
                }

                string css = result.ToString();
                StringBuilderBag.ClearAndReturn(result);

                return css;
            }
        });
        _bytesObject = new(() => Encoding.UTF8.GetBytes(Css));
        _sha256Object = new(() => ComputeSha256(CssUtf8));
        _sha256HexObject = new(() => ToHex(Sha256));
    }

    public bool Add(UtilityClass c)
    {
        if (!_classes.TryAdd(c))
        {
            return false;
        }

        int bucket = BucketIndex(c);

        lock (_cssBuilderLock)
        {
            _buckets[bucket].Append('\n');
            _buckets[bucket].Append(c.CssString);
        }

        _cssObject.Invalidate();
        _bytesObject.Invalidate();
        _sha256Object.Invalidate();
        _sha256HexObject.Invalidate();

        EventHandler<UtilityClass>? handler = OnClassAdded;
        handler?.Invoke(this, c);

        return true;
    }

    private static byte[] ComputeSha256(byte[] bytes)
    {
        using SHA256 sha = SHA256.Create();
        return sha.ComputeHash(bytes);
    }

    private static string ToHex(byte[] bytes) =>
        BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();

    private static int BucketIndex(UtilityClass c)
    {
        // Media variants always go last, in a single bucket. Their extra selectors
        // (theme class and/or pseudo-class) give them enough specificity to win
        // over the base and media-only rules they need to override.
        if (!string.IsNullOrWhiteSpace(c.MediaQuery))
        {
            return 4;
        }

        bool themed = !string.IsNullOrWhiteSpace(c.Theme);
        bool pseudo = !string.IsNullOrWhiteSpace(c.Pseudo);

        if (themed)
        {
            return pseudo ? 3 : 2;
        }

        return pseudo ? 1 : 0;
    }

    public IEnumerator<UtilityClass> GetEnumerator() => _classes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
