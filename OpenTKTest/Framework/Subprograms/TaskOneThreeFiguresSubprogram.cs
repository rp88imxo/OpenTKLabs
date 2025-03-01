using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TaskOneThreeFiguresSubprogram : BaseSubprogram
{
    private DefaultShader _currentShader;
    private RenderOperation _renderOperationTriangleOne;
    private RenderOperation _renderOperationRectangle;
    private RenderOperation _renderOperationTriangleTwo;

    public override void Init()
    {
        base.Init();
        
        _currentShader = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        InitRenderOperationTriangleOne();
        InitRenderOperationRectangle();
        InitRenderOperationTriangleTwo();
    }

    private void InitRenderOperationTriangleOne()
    {
        var vertices = new float[]
        {
            0f, 0.3f, 0.2f,
            0.3f, 0.3f, 0f,
            0f, 0f, 0.7f
        };
        
        var colors = new float[]
        {
            1f, 0f, 0f,
            1f, 0f, 0f,
            1f, 0f, 0f,
        };
        
        _renderOperationTriangleOne = new RenderOperation(vertices, colors, _currentShader,
            new RenderOperationData(PrimitiveType.Triangles));
        _renderOperationTriangleOne.Init();
    }
    
    private void InitRenderOperationRectangle()
    {
        var vertices = new float[]
        {
           0.5f, 0.5f, 0.3f,
           -0.5f, -0.5f, 0.3f,
           0.5f, -0.5f, 0.3f,
           0.5f, 0.5f, 0.3f
        };
        
        var colors = new float[]
        {
            0f, 1f, 0f,
            0f, 1f, 0f,
            0f, 1f, 0f,
            0f, 1f, 0f,
        };
        
        _renderOperationRectangle = new RenderOperation(vertices, colors, _currentShader,
            new RenderOperationData(PrimitiveType.Triangles));
        _renderOperationRectangle.Init();
    }
    
    private void InitRenderOperationTriangleTwo()
    {
        var vertices = new float[]
        {
            0f, 3f, 0.3f,
            0.4f, 0f, 0.5f,
            -0.5f, 0.5f, -1f
        };
        
        var colors = new float[]
        {
            0f, 0f, 1f,
            0f, 0f, 1f,
            0f, 0f, 1f,
            0f, 0f, 1f,
        };
        
        _renderOperationTriangleTwo = new RenderOperation(vertices, colors, _currentShader,
            new RenderOperationData(PrimitiveType.Triangles));
        _renderOperationTriangleTwo.Init();
    }
    
    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        Matrix4 model = Matrix4.Identity;
        
        Matrix4 projection = Matrix4.Identity;
        
        _currentShader.Use();
        
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref model);
        
        var projectionLocation = GL.GetUniformLocation(_currentShader.Handle, "projection");
        GL.UniformMatrix4(projectionLocation, true, ref projection);
        
        _renderOperationTriangleOne.Render(renderArgumentsData);
        _renderOperationRectangle.Render(renderArgumentsData);
        _renderOperationTriangleTwo.Render(renderArgumentsData);
    }
}