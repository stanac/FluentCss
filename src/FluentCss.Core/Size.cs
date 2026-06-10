namespace FluentCss.Core;

public class Size
{
    public Size(string value, string name)
    {
        Value = value;
        Name = name;
    }

    public string Value { get; }
    public string Name { get; }

    public static Size Sm { get; } = new("40rem", "sm");
    public static Size Md { get; } = new("48rem", "md");
    public static Size Lg { get; } = new("64rem", "lg");
    public static Size Xl { get; } = new("80rem", "xl");
    public static Size _2Xl { get; } = new("96rem", "2l");
    public static Size _3Xl { get; } = new("96rem", "3l");

    public static Size Full { get; } = new("100%", "full");
    public static Size ScreenH { get; } = new("100vh", "screen");
    public static Size ScreenW { get; } = new("100vw", "screen");
    public static Size Zero { get; } = new("0px", "0");

    public static Size Min { get; } = new("min-content", "min");
    public static Size Max { get; } = new("max-content", "max");
    public static Size Fit { get; } = new("fit-content", "fit");
}