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
    
     public readonly struct MeshData
    {
        public MeshData(float[] vertices, float[] normals, uint[] indices)
        {
            Vertices = vertices;
            Normals = normals;
            Indices = indices;
        }

        public float[] Vertices { get; }
        public float[] Normals { get; }
        public uint[] Indices { get; }
    }

    public static MeshData CreateSphere(float radius, int sectorCount, int stackCount)
    {
        List<float> vertices = new List<float>();
        List<float> normals = new List<float>();
        // List<float> texCoords = new List<float>(); // Пока не используем текстуры
        List<uint> indices = new List<uint>();

        float x, y, z, xy;                              // vertex position
        float nx, ny, nz, lengthInv = 1.0f / radius;    // vertex normal
        // float s, t;                                     // vertex texCoord

        float sectorStep = 2 * MathF.PI / sectorCount;
        float stackStep = MathF.PI / stackCount;
        float sectorAngle, stackAngle;

        for (int i = 0; i <= stackCount; ++i)
        {
            stackAngle = MathF.PI / 2 - i * stackStep; // starting from pi/2 to -pi/2
            xy = radius * MathF.Cos(stackAngle);       // r * cos(u)
            z = radius * MathF.Sin(stackAngle);        // r * sin(u)

            for (int j = 0; j <= sectorCount; ++j)
            {
                sectorAngle = j * sectorStep;           // starting from 0 to 2pi

                // vertex position (x, y, z)
                x = xy * MathF.Cos(sectorAngle);       // r * cos(u) * cos(v)
                y = xy * MathF.Sin(sectorAngle);       // r * cos(u) * sin(v)
                vertices.Add(x);
                vertices.Add(y);
                vertices.Add(z);

                // normalized vertex normal (nx, ny, nz)
                nx = x * lengthInv;
                ny = y * lengthInv;
                nz = z * lengthInv;
                normals.Add(nx);
                normals.Add(ny);
                normals.Add(nz);

                // vertex tex coord (s, t) range between [0, 1]
                // s = (float)j / sectorCount;
                // t = (float)i / stackCount;
                // texCoords.Add(s);
                // texCoords.Add(t);
            }
        }

        // generate indices for triangles
        uint k1, k2;
        for (int i = 0; i < stackCount; ++i)
        {
            k1 = (uint)(i * (sectorCount + 1)); // beginning of current stack
            k2 = (uint)(k1 + sectorCount + 1);  // beginning of next stack

            for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
            {
                // 2 triangles per sector excluding first and last stacks
                // k1 => k2 => k1+1
                if (i != 0)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);
                }

                // k1+1 => k2 => k2+1
                if (i != (stackCount - 1))
                {
                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }
        }

        return new MeshData(vertices.ToArray(), normals.ToArray(), indices.ToArray());
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
    
    public static VerticesData CreateNPolygon(int sides)
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
        
        var data = new VerticesData(vertices, colors);
        return data;
    }
}