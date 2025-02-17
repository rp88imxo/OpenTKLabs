using OpenTK.Graphics.OpenGL4;
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
        
        // 0 is positionVertexAttribute
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // 1 is colorVertexAttribute
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 12);
        GL.EnableVertexAttribArray(1);

        _defaultShaderToUse.Init();
    }

    public void Render(RenderArgumentsData renderArgumentsData)
    {
        // var colorLocation = GL.GetUniformLocation(_defaultShaderToUse.Handle, "ourColor");
        //
        // var timeValue = (float)renderArgumentsData.TotalTimePassed;
        // var greenValue = (MathF.Sin(timeValue) / 2.0f) + 0.5f;
        //
        
       // var bounceValue = MathF.Sin((float)(renderArgumentsData.TotalTimePassed)) * 0.1f;
        _defaultShaderToUse.Use();
        //_defaultShaderToUse.SetFloat("offsetValue", bounceValue);
        //
        // GL.Uniform4(colorLocation, 0.0f, greenValue, 0.0f, 1.0f);
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}