using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core;

public class ElementBufferRenderOperation : IRenderOperation
{
    private readonly float[] _vertices;
    private readonly DefaultShader _defaultShaderToUse;

    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private int _elementBufferObject;
    private uint[] _indices;

    public ElementBufferRenderOperation(float[] vertices,uint[] indices, DefaultShader defaultShaderToUse)
    {
        _vertices = vertices;
        _defaultShaderToUse = defaultShaderToUse;
        _indices = indices;
    }
    
    public void Init()
    {
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _defaultShaderToUse.Init();
    }

    public void Render(RenderArgumentsData renderArgumentsData)
    {
        _defaultShaderToUse.Use();
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}