using OpenTK.Graphics.OpenGL;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core;

public class RenderOperationData
{
    public RenderOperationData(PrimitiveType primitiveType)
    {
        PrimitiveType = primitiveType;
    }

    public PrimitiveType PrimitiveType { get; }
}

public class RenderOperation : IRenderOperation
{
    private readonly float[] _vertices;
    private readonly float[] _colors;
    private readonly DefaultShader _defaultShaderToUse;
    private readonly RenderOperationData _renderOperationData;

    int _vertexBufferObject;
    int _colorBufferObject;
    int _vertexArrayObject;
    private int _vertexCountToRender;

    public RenderOperation(float[] vertices,float[] colors, DefaultShader defaultShaderToUse,RenderOperationData renderOperationData )
    {
        _vertices = vertices;
        _colors = colors;
        _defaultShaderToUse = defaultShaderToUse;
        _renderOperationData = renderOperationData;
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

        _colorBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _colors.Length * sizeof(float), _colors,
            BufferUsageHint.StaticDraw);
        
        
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);

        _defaultShaderToUse.Init();

        _vertexCountToRender = _vertices.Length / 3;
    }

    public void Render(RenderArgumentsData renderArgumentsData)
    {
        
        
        
        
        
        
        
        _defaultShaderToUse.Use();
        
        
        
        
        GL.BindVertexArray(_vertexArrayObject);
        
        
        
       
        
        
        
        
        
        
        
        
        
        
        
        
        

        GL.DrawArrays(_renderOperationData.PrimitiveType, 0, _vertexCountToRender);
    }
}