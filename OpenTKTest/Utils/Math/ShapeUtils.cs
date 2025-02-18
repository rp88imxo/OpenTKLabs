namespace OpenTKTest.Utils.Math;

public static class ShapeUtils
{
    public readonly struct NPolygonData
    {
        public NPolygonData(float[] vertices, float[] colors)
        {
            Vertices = vertices;
            Colors = colors;
        }

        public float[] Vertices { get; }
        public float[] Colors { get; }
    }
    
    public static NPolygonData CreateNPolygon(int sides)
    {
        float[] vertices = new float[sides * 3];
        float[] colors = new float[sides * 3];
        
        for (int i = 0; i < sides; i++)
        {
            var angle = 2.0f * MathF.PI * i / sides;
            vertices[i * 3] = MathF.Cos(angle);
            vertices[i * 3 + 1] = MathF.Sin(angle);
            vertices[i * 3 + 2] = 0.0f;
            
            colors[i * 3] = 0.0f;
            colors[i * 3 + 1] = 1.0f;
            colors[i * 3 + 2] = 0.0f;
        }
        
        var data = new NPolygonData(vertices, colors);
        return data;
    }
}