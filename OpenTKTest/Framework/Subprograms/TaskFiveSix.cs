using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Framework.Subprograms;

public class TaskFiveSix : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private DefaultShader _currentShader;
    private RenderOperation _renderOperationCube;
    private bool _currentModeFlag;

    public TaskFiveSix(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
        _currentModeFlag = true;
    }
    
    public override void Init()
    {
        base.Init();
        
        _currentShader = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        InitRenderOperationCube();
    }

    private void InitRenderOperationCube()
    {
        float[] vertices = {
            -0.5f, -0.5f, -0.5f, 
            0.5f, -0.5f, -0.5f,  
            0.5f,  0.5f, -0.5f,  
            0.5f,  0.5f, -0.5f,  
            -0.5f,  0.5f, -0.5f, 
            -0.5f, -0.5f, -0.5f, 

            -0.5f, -0.5f,  0.5f, 
            0.5f, -0.5f,  0.5f,  
            0.5f,  0.5f,  0.5f,  
            0.5f,  0.5f,  0.5f,  
            -0.5f,  0.5f,  0.5f, 
            -0.5f, -0.5f,  0.5f, 

            -0.5f,  0.5f,  0.5f, 
            -0.5f,  0.5f, -0.5f, 
            -0.5f, -0.5f, -0.5f, 
            -0.5f, -0.5f, -0.5f, 
            -0.5f, -0.5f,  0.5f, 
            -0.5f,  0.5f,  0.5f, 

            0.5f,  0.5f,  0.5f,  
            0.5f,  0.5f, -0.5f,  
            0.5f, -0.5f, -0.5f,  
            0.5f, -0.5f, -0.5f,  
            0.5f, -0.5f,  0.5f,  
            0.5f,  0.5f,  0.5f,  

            -0.5f, -0.5f, -0.5f, 
            0.5f, -0.5f, -0.5f,  
            0.5f, -0.5f,  0.5f,  
            0.5f, -0.5f,  0.5f,  
            -0.5f, -0.5f,  0.5f, 
            -0.5f, -0.5f, -0.5f, 

            -0.5f,  0.5f, -0.5f, 
            0.5f,  0.5f, -0.5f,  
            0.5f,  0.5f,  0.5f,  
            0.5f,  0.5f,  0.5f,  
            -0.5f,  0.5f,  0.5f, 
            -0.5f,  0.5f, -0.5f,
            // -1f, -1f, -1f, 
            // 1f, -1f, -1f,  
            // 1f,  1f, -1f,  
            // 1f,  1f, -1f,  
            // -1f,  1f, -1f, 
            // -1f, -1f, -1f, 
            //
            // -1f, -1f,  1f, 
            // 1f, -1f,  1f,  
            // 1f,  1f,  1f,  
            // 1f,  1f,  1f,  
            // -1f,  1f,  1f, 
            // -1f, -1f,  1f, 
            //
            // -1f,  1f,  1f, 
            // -1f,  1f, -1f, 
            // -1f, -1f, -1f, 
            // -1f, -1f, -1f, 
            // -1f, -1f,  1f, 
            // -1f,  1f,  1f, 
            //
            // 1f,  1f,  1f,  
            // 1f,  1f, -1f,  
            // 1f, -1f, -1f,  
            // 1f, -1f, -1f,  
            // 1f, -1f,  1f,  
            // 1f,  1f,  1f,  
            //
            // -1f, -1f, -1f, 
            // 1f, -1f, -1f,  
            // 1f, -1f,  1f,  
            // 1f, -1f,  1f,  
            // -1f, -1f,  1f, 
            // -1f, -1f, -1f, 
            //
            // -1f,  1f, -1f, 
            // 1f,  1f, -1f,  
            // 1f,  1f,  1f,  
            // 1f,  1f,  1f,  
            // -1f,  1f,  1f, 
            // -1f,  1f, -1f, 
        };
        
        var colors = new float[]
        {
            1f, 0f, 0f, 
            1f, 0f, 0f,  
            1f, 0f, 0f,  
            1f, 0f, 0f,  
            1f, 0f, 0f, 
            1f, 0f, 0f, 

            0f, 1f, 0f, 
            0f, 1f, 0f,  
            0f, 1f, 0f,  
            0f, 1f, 0f,  
            0f, 1f, 0f, 
            0f, 1f, 0f, 

            0f, 0f, 1f, 
            0f, 0f, 1f,  
            0f, 0f, 1f,  
            0f, 0f, 1f,  
            0f, 0f, 1f, 
            0f, 0f, 1f, 

            0.5f, 0f, 0f, 
            0.5f, 0f, 0f,  
            0.5f, 0f, 0f,  
            0.5f, 0f, 0f,  
            0.5f, 0f, 0f, 
            0.5f, 0f, 0f,   

            0f, 0.5f, 0f, 
            0f, 0.5f, 0f,  
            0f, 0.5f, 0f,  
            0f, 0.5f, 0f,  
            0f, 0.5f, 0f, 
            0f, 0.5f, 0f,  

            0f, 0f, 0.5f, 
            0f, 0f, 0.5f,  
            0f, 0f, 0.5f,  
            0f, 0f, 0.5f,  
            0f, 0f, 0.5f, 
            0f, 0f, 0.5f, 
        };
        
        _renderOperationCube = new RenderOperation(vertices, colors, _currentShader,
            new RenderOperationData(PrimitiveType.Triangles));
        _renderOperationCube.Init();
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);
        if ( _baseWindow.KeyboardState.IsKeyReleased(Keys.Up))
        {
            SwitchMode();
        }
    }

    private void SwitchMode()
    {
        _currentModeFlag = !_currentModeFlag;
        UpdateMode();
    }

    private void UpdateMode()
    {
        if (_currentModeFlag)
        {
            GL.CullFace(TriangleFace.Back);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        }
        else
        {
            GL.CullFace(TriangleFace.Back);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        UpdateMode();
        
        //GL.DepthFunc(DepthFunction.Less);

        var rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30f));
        var rotationY = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(70f));
        Matrix4 model = rotationX * rotationY * Matrix4.CreateTranslation(0f,0f, 0f * (float)renderArgumentsData.TotalTimePassed);

        Matrix4 projection = //Matrix4.Identity; //Matrix4.CreatePerspectiveOffCenter(-10f, 10f, -10f, 10f, 1f, 100);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(125f) , 800f / 600f, 1f, 100f);
        
        _currentShader.Use();
        
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref model);
        
        var projectionLocation = GL.GetUniformLocation(_currentShader.Handle, "projection");
        GL.UniformMatrix4(projectionLocation, true, ref projection);
        
        //GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        
        _renderOperationCube.Render(renderArgumentsData);
    }
}