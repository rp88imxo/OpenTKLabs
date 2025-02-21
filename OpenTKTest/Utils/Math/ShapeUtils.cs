using OpenTK.Mathematics;

namespace OpenTKTest.Utils.Math;

public static class ShapeUtils
{
    public readonly struct VerticesData
    {
        public VerticesData(float[] vertices, float[] colors)
        {
            Vertices = vertices;
            Colors = colors;
        }

        public float[] Vertices { get; }
        public float[] Colors { get; }
    }

    public static void DefaultColorCallback(float[] colors, int i)
    {
        colors[i * 3] = 0.0f;
        colors[i * 3 + 1] = 1.0f;
        colors[i * 3 + 2] = 0.0f;
    }
    
    public static void RandomColorPerTriangleCallback(float[] colors, int i)
    {
        var colorR = Random.Shared.NextSingle();
        var colorG = Random.Shared.NextSingle();
        var colorB = Random.Shared.NextSingle();
        
        colors[i * 3] = colorR;
        colors[i * 3 + 1] = colorG;
        colors[i * 3 + 2] = colorB;
    }

    public static VerticesData CreatePoints(List<Vector2> positions,Action<float[], int> colorCallback, int xDimension = 1920, int yDimension = 1080)
    {
        float[] vertices = new float[positions.Count * 3];
        float[] colors = new float[positions.Count * 3];
        
        for (var i = 0; i < positions.Count; i++)
        {
            var position = positions[i];
            
            vertices[i * 3] = MathUtils.Remap(position.X, 0f, 1920f, 1.0f, -1.0f);
            vertices[i * 3 + 1] = MathUtils.Remap(position.Y, 0f, 1080f, 1.0f, -1.0f);
            vertices[i * 3 + 2] = 0.0f;

            colorCallback.Invoke(colors, i);
        }

        return new VerticesData(vertices, colors);
    }
    
    public static VerticesData CreateNPolygonLines(int sides)
    {
        float[] vertices = new float[sides * 6];
        float[] colors = new float[sides * 6];
        
        for (int i = 0; i < sides; i++)
        {
            var angle1 = 2.0f * MathF.PI * i / sides;
            var angle2 = 2.0f * MathF.PI * ((i + 1) % sides) / sides;
            
            vertices[i * 6] = MathF.Cos(angle1);
            vertices[i * 6 + 1] = MathF.Sin(angle1);
            vertices[i * 6 + 2] = 0.0f;
            
            vertices[i * 6 + 3] = MathF.Cos(angle2);
            vertices[i * 6 + 4] = MathF.Sin(angle2);
            vertices[i * 6 + 5] = 0.0f;
            
            colors[i * 6] = 0.0f;
            colors[i * 6 + 1] = 1.0f;
            colors[i * 6 + 2] = 0.0f;
            
            colors[i * 6 + 3] = 0.0f;
            colors[i * 6 + 4] = 1.0f;
            colors[i * 6 + 5] = 0.0f;
        }
        
        var data = new VerticesData(vertices, colors);
        return data;
    }
    
    public static VerticesData CreateNPolygon(int sides, Action<float[], int> colorCallback)
    {
        float[] vertices = new float[sides * 3];
        float[] colors = new float[sides * 3];
        
        for (int i = 0; i < sides; i++)
        {
            var angle = 2.0f * MathF.PI * i / sides;
            vertices[i * 3] = MathF.Cos(angle);
            vertices[i * 3 + 1] = MathF.Sin(angle);
            vertices[i * 3 + 2] = 0.0f;
            
            colorCallback.Invoke(colors, i);
        }
        
        var data = new VerticesData(vertices, colors);
        return data;
    }
}