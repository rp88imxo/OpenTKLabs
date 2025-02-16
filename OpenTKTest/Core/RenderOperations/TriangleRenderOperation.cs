using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core;

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

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _defaultShaderToUse.Init();
    }

    public void Render()
    {
        _defaultShaderToUse.Use();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}