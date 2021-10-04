using Godot;

public static class ColorExtensions
{
    public static Color Lerp(this Color a, Color b, float w)
    {
        Color c = new()
        {
            r = Mathf.Lerp(a.r, b.r, w),
            g = Mathf.Lerp(a.g, b.g, w),
            b = Mathf.Lerp(a.b, b.b, w),
            a = Mathf.Lerp(a.a, b.a, w)
        };
        return c;
    }
}

public static class Helper
{
    private static readonly Color TextA = Color.Color8(225, 165, 15);
    private static readonly Color TextB = Color.Color8(203, 32, 42);

    public static Color GetPointsColor(float w) => TextA.Lerp(TextB, Mathf.Max(1, w));
}
