
using OpenTK.Graphics.OpenGL;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core;

public struct RenderArgumentsData
{
    public double TotalTimePassed;
}

public class TriangleRenderOperation : IRenderOperation
{
    private readonly float[] _vertices;
    private readonly DefaultShader _defaultShaderToUse;

    int _vertexBufferObject;
    int _vertexArrayObject;
    
    public TriangleRenderOperation(float[] vertices, DefaultShader defaultShaderToUse)
    {
        _vertices = vertices;
        _defaultShaderToUse = defaultShaderToUse;
    }

    public void Init()
    {
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);
        
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 12);
        GL.EnableVertexAttribArray(1);

        _defaultShaderToUse.Init();
    }

    public void Render(RenderArgumentsData renderArgumentsData)
    {
        
        
        
        
        
        
       
        _defaultShaderToUse.Use();
        
        
        
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.BindVertexArray(_vertexArrayObject);
        
        
        
        GL.PointSize(5);
        GL.Enable(EnableCap.PointSmooth);
        
        
        
        
        
        
        
        
        
        
        
        
        
        GL.DrawArrays(PrimitiveType.Points, 0, 3);
    }
}