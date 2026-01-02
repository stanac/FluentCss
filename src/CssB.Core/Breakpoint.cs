namespace CssB.Core;

public class Breakpoint
{
    public Breakpoint(string value, string name)
    {
        Value = value;
        Name = name;
    }

    public string Value { get; }
    public string Name { get; }

    public static Breakpoint Sm { get; } = new("40rem", "sm");
    public static Breakpoint Md { get; } = new("48rem", "md");
    public static Breakpoint Lg { get; } = new("64rem", "lg");
    public static Breakpoint Xl { get; } = new("80rem", "xl");
    public static Breakpoint _2Xl { get; } = new("96rem", "2l");
    public static Breakpoint _3Xl { get; } = new("96rem", "3l");
}