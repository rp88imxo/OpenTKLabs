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
        
        _currentShader = new DefaultShader("Shaders/vertex_mvp.shader", "Shaders/fragment.shader");
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
        
        

        var rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30f * (float)renderArgumentsData.TotalTimePassed));
        var rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(70f* (float)renderArgumentsData.TotalTimePassed));
        Matrix4 model = rotationX * rotationY;

        Matrix4 projection = 
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(125f) , 800f / 600f, 0.1f, 100f);

        Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 2.5f), 
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f));
        
        _currentShader.Use();
        
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, false, ref model);
        
        var projectionLocation = GL.GetUniformLocation(_currentShader.Handle, "projection");
        GL.UniformMatrix4(projectionLocation, false, ref projection);
        
        var viewLocation = GL.GetUniformLocation(_currentShader.Handle, "view");
        GL.UniformMatrix4(viewLocation, false, ref view);
        
        
        
        _renderOperationCube.Render(renderArgumentsData);
    }
}