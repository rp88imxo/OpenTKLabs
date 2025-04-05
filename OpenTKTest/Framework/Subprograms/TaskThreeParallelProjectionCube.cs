using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Framework.Subprograms;

public class TaskThreeParallelProjectionCube : BaseSubprogram
{
    private DefaultShader _currentShader;
    private RenderOperation _renderOperationCube;

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
    
    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        GL.Enable(EnableCap.DepthTest);
        

        Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(25f)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45f));

        Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 2.5f), 
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f));
        
        Matrix4 projection =
            Matrix4.CreateOrthographicOffCenter(-0.7f, 0.7f, -0.85f, 0.85f, 3, 10);
        
        _currentShader.Use();
        
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref model);
        
        var projectionLocation = GL.GetUniformLocation(_currentShader.Handle, "projection");
        GL.UniformMatrix4(projectionLocation, true, ref projection);
        
        var viewLocation = GL.GetUniformLocation(_currentShader.Handle, "view");
        GL.UniformMatrix4(viewLocation, false, ref view);
        
        _renderOperationCube.Render(renderArgumentsData);
    }
}