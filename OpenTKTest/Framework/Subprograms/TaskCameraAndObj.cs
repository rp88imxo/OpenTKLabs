using ObjLoader.Loader.Loaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Framework.Subprograms;

public class TaskCameraAndObj : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private DefaultShader _currentShader;
    private RenderOperation _renderOperationShip;
    private bool _currentModeFlag = false;
    
    float speed = 5.0f;

    Vector3 position = new Vector3(0.0f, 0.0f,  5.0f);
    Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
    Vector3 up = new Vector3(0.0f, 1.0f,  0.0f);

    private Vector2 lastPos;
    private float yaw;
    private float pitch;

    public TaskCameraAndObj(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
        _currentModeFlag = false;
    }
    
    
    public override void Init()
    {
        base.Init();
        
        
       
        _currentShader = new DefaultShader("Shaders/vertex_mvp.shader", "Shaders/fragment.shader");
        InitRenderOperationShip();
    }

    private void InitRenderOperationShip()
    {
        var objLoaderFactory = new ObjLoaderFactory();
        var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
        
        var fileStream = new FileStream("Data/Models/ship.obj", FileMode.Open);
        var loadResult = objLoader.Load(fileStream);
        
        var vertices = loadResult.Vertices.SelectMany(v => new float[] { v.X, v.Y, v.Z }).ToArray();
        var colors = vertices.ToArray();
        
        _renderOperationShip = new RenderOperation(vertices, colors, _currentShader,
            new RenderOperationData(PrimitiveType.TriangleFan));
        _renderOperationShip.Init();
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);
        if ( _baseWindow.KeyboardState.IsKeyReleased(Keys.Up))
        {
            SwitchMode();
        }
        
        KeyboardState input = _baseWindow.KeyboardState;
        
        if (input.IsKeyDown(Keys.W))
        {
            position += front * speed * (float)args.Time; //Forward 
        }

        if (input.IsKeyDown(Keys.S))
        {
            position -= front * speed* (float)args.Time; //Backwards
        }

        if (input.IsKeyDown(Keys.A))
        {
            position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed* (float)args.Time; //Left
        }

        if (input.IsKeyDown(Keys.D))
        {
            position += Vector3.Normalize(Vector3.Cross(front, up)) * speed* (float)args.Time; //Right
        }

        if (input.IsKeyDown(Keys.Space))
        {
            position += up * speed* (float)args.Time; //Up 
        }

        if (input.IsKeyDown(Keys.LeftShift))
        {
            position -= up * speed* (float)args.Time; //Down
        }

        #region MOUSE_INPUT

        if (FirstMove)
        {
            lastPos = new Vector2( _baseWindow.MouseState.X, _baseWindow.MouseState.Y);
            FirstMove = false;
        }
        else
        {
            float deltaX = _baseWindow.MouseState.X - lastPos.X;
            float deltaY = _baseWindow.MouseState.Y - lastPos.Y;
            lastPos = new Vector2(_baseWindow.MouseState.X, _baseWindow.MouseState.Y);

            yaw += deltaX * 1f;
            if(pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            else if(pitch < -89.0f)
            {
                pitch = -89.0f;
            }
            else
            {
                pitch -= deltaY * 1f;
            }
        }
    
        front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
        front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
        front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
        front = Vector3.Normalize(front);
        
        #endregion
    }

    public bool FirstMove { get; set; } = true;

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

        var rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30f * (float)renderArgumentsData.TotalTimePassed));
        var rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-70f* (float)renderArgumentsData.TotalTimePassed));

        var translation = Matrix4.CreateTranslation(MathF.Sin((float)renderArgumentsData.TotalTimePassed * 0.1f) * 2f, 0f, 0f);
        
        Matrix4 model = translation  * rotationY;

        Matrix4 projection = //Matrix4.Identity; //Matrix4.CreatePerspectiveOffCenter(-10f, 10f, -10f, 10f, 1f, 100);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f) , 800f / 600f, 0.1f, 10f);

        Matrix4 view = Matrix4.LookAt(position, position + front, up);
        
        _currentShader.Use();
        
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, false, ref model);
        
        var projectionLocation = GL.GetUniformLocation(_currentShader.Handle, "projection");
        GL.UniformMatrix4(projectionLocation, false, ref projection);
        
        var viewLocation = GL.GetUniformLocation(_currentShader.Handle, "view");
        GL.UniformMatrix4(viewLocation, false, ref view);
        
        //GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        
        _renderOperationShip.Render(renderArgumentsData);
    }
}