using BenchmarkDotNet.Attributes;
using FluentCss.Core;

namespace FluentCss.Benchmarks;

/// <summary>
/// Adds batches of <see cref="ClassCount"/> utility classes to a <see cref="ClassRegistry"/>.
/// <see cref="MemoryDiagnoserAttribute"/> reports allocated bytes alongside the timing, so a
/// single benchmark method yields both metrics. Results are reported per 250-class batch
/// (see <see cref="BenchmarkAttribute.OperationsPerInvoke"/>).
/// </summary>
[MemoryDiagnoser]
public class ClassRegistryBenchmarks
{
    private const int ClassCount = 500;

    // Each invocation repeats the [ClassCount]-class batch enough times to clear BenchmarkDotNet's
    // 100 ms minimum iteration time. OperationsPerInvoke is the batch count, so the
    // reported time/allocations are per [ClassCount]-class batch.
    private const int NewBatches = 25_000;
    private const int DuplicateBatches = 55_000;

    private ClassRegistry _registry = null!;

    // A fresh, pre-warmed registry per iteration (outside the measured region) so repeated
    // iterations don't get deduplicated.
    [IterationSetup]
    public void IterationSetup()
    {
        _registry = new ClassRegistry();

        // Seed the "p-" classes so Add_DuplicateClasses has something to collide with.
        for (int i = 0; i < ClassCount; i++)
        {
            _registry.Add(new UtilityClass($"p-{i}", "padding", $"{i * 0.25}rem"));
        }
    }

    /// <summary>
    /// Adds 250 not-yet-registered classes per batch: full insert path (bucket append,
    /// lazy invalidation, event dispatch).
    /// </summary>
    [Benchmark(Baseline = true, OperationsPerInvoke = NewBatches)]
    public void Add_NewClasses()
    {
        for (int batch = 0; batch < NewBatches; batch++)
        {
            ClassRegistry registry = new ClassRegistry();

            for (int i = 0; i < ClassCount; i++)
            {
                registry.Add(new UtilityClass($"q-{i}", "padding", $"{i * 0.25}rem"));
            }
        }
    }

    /// <summary>
    /// Adds 250 already-present classes per batch: the steady-state path where
    /// <c>TryAdd</c> fails and the insert work is skipped.
    /// </summary>
    [Benchmark(OperationsPerInvoke = DuplicateBatches)]
    public void Add_DuplicateClasses()
    {
        for (int batch = 0; batch < DuplicateBatches; batch++)
        {
            for (int i = 0; i < ClassCount; i++)
            {
                _registry.Add(new UtilityClass($"p-{i}", "padding", $"{i * 0.25}rem"));
            }
        }
    }
}

/*

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9457)
AMD Ryzen 5 5600X 3.70GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  Job-CNUJVU : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3

 Initial results:

| Method               | Mean      | Error    | StdDev   | Ratio | Gen0    | Allocated | Alloc Ratio |
|--------------------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
| Add_NewClasses       | 123.73 us | 1.121 us | 0.936 us |  1.00 | 15.3600 | 251.39 KB |        1.00 |
| Add_DuplicateClasses |  70.69 us | 0.154 us | 0.128 us |  0.57 |  5.6909 |  92.97 KB |        0.37 |

 */