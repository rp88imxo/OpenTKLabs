namespace OpenTKTest.Utils.Math;

public static class MathUtils
{
    public static float Remap(float value, float low1, float high1, float high2, float low2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}