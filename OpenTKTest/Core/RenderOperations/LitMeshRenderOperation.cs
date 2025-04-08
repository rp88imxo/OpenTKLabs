using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core;

public class LitMeshRenderOperation : IRenderOperation, IDisposable
{
    private readonly float[] _vertices;
    private readonly float[] _normals;
    private readonly float[] _texCoords;
    private readonly uint[] _indices;
    private readonly DefaultShader _shader;

    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _normalBufferObject;
    private int _texCoordBufferObject;
    private int _elementBufferObject;

    private bool disposedValue;

   
    public LitMeshRenderOperation(float[] vertices, float[] normals, float[] texCoords, uint[] indices, DefaultShader shader)
    {
        _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
        _normals = normals ?? throw new ArgumentNullException(nameof(normals));
        _texCoords = texCoords; 
        _indices = indices ?? throw new ArgumentNullException(nameof(indices));
        _shader = shader ?? throw new ArgumentNullException(nameof(shader));
    }

     public void Init()
    {
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        
        _normalBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _normals.Length * sizeof(float), _normals, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);

        
        if (_texCoords != null && _texCoords.Length > 0) 
        {
            _texCoordBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _texCoords.Length * sizeof(float), _texCoords, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(2);
        }

        
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public void Render(RenderArgumentsData renderArgumentsData)
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_normalBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            if (_texCoordBufferObject > 0) 
            {
                GL.DeleteBuffer(_texCoordBufferObject);
            }
            GL.DeleteVertexArray(_vertexArrayObject);

            disposedValue = true;
        }
    }
    
     ~LitMeshRenderOperation()
     {
         Dispose(disposing: false);
     }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}