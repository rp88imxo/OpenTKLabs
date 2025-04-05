using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math; 

namespace OpenTKTest.Framework.Subprograms;

public class PhongLightingTask : BaseSubprogram, IDisposable
{
    private readonly BaseWindow _baseWindow;
    private DefaultShader _phongShader;
    private LitMeshRenderOperation _renderOpSphere1;
    private LitMeshRenderOperation _renderOpSphere2;
    
    private float _sphereRadius = 3.0f;
    private float _sphereSeparationDistance; 
    private Vector3 _sphere1Pos;
    private Vector3 _sphere2Pos;

    float speed = 5.0f;
    Vector3 position = new Vector3(0.0f, 0.0f, 15.0f); 
    Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
    Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
    private Vector2 lastPos;
    private float yaw = -90.0f; 
    private float pitch = 0.0f;
    private bool firstMove = true;

    
    private Material _emeraldMaterial;
    private Material _redPlasticMaterial;

    
    private DirLight _dirLight;
    private PointLight[] _pointLights = new PointLight[3]; 
    private SpotLight _spotLight;

    
    
    private struct Material
    {
        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public float Shininess;
    }

    private struct DirLight
    {
        public Vector3 Direction;
        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }

    private struct PointLight
    {
        public Vector3 Position;
        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public float Constant;
        public float Linear;
        public float Quadratic;
    }
    
    private struct SpotLight 
    {
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public float CutOff;       
        public float OuterCutOff;  
        public float Constant;
        public float Linear;
        public float Quadratic;
    }

    private bool disposedValue;


    public PhongLightingTask(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow ?? throw new ArgumentNullException(nameof(baseWindow));
    }

    public override void Init()
    {
        base.Init();

        
        _sphereSeparationDistance = _sphereRadius * 4.0f;
        _sphere1Pos = new Vector3(-_sphereSeparationDistance / 2.0f, 0.0f, 0.0f);
        _sphere2Pos = new Vector3(_sphereSeparationDistance / 2.0f, 0.0f, 0.0f);

        
        _phongShader = new DefaultShader("Shaders/phong_vert.shader", "Shaders/phong_frag.shader");
        _phongShader.Init();

        
        
        var sphereMeshData = ShapeUtils.CreateSphere(_sphereRadius, 36, 18);

        
        _renderOpSphere1 = new LitMeshRenderOperation(sphereMeshData.Vertices, sphereMeshData.Normals, sphereMeshData.Indices, _phongShader);
        _renderOpSphere1.Init();
        _renderOpSphere2 = new LitMeshRenderOperation(sphereMeshData.Vertices, sphereMeshData.Normals, sphereMeshData.Indices, _phongShader);
        _renderOpSphere2.Init();

        
        
        _emeraldMaterial = new Material
        {
            Ambient = new Vector3(0.0215f, 0.1745f, 0.0215f),
            Diffuse = new Vector3(0.07568f, 0.61424f, 0.07568f),
            Specular = new Vector3(0.633f, 0.727811f, 0.633f),
            Shininess = 0.6f * 128.0f 
        };

        
        _redPlasticMaterial = new Material
        {
            Ambient = new Vector3(0.0f, 0.0f, 0.0f),
            Diffuse = new Vector3(0.5f, 0.0f, 0.0f),
            Specular = new Vector3(0.1f, 0.1f, 0.1f),
            Shininess = 0.25f * 128.0f
        };

        
        
        _dirLight = new DirLight
        {
            Direction = new Vector3(0.2f, 0.2f, 0.3f),
            Ambient = new Vector3(0.15f, 0.15f, 0.15f),
            Diffuse = new Vector3(0.6f, 0.6f, 0.6f),
            Specular = new Vector3(0.7f, 0.7f, 0.7f)
        };

        
        _pointLights[0] = new PointLight
        {
            Position = new Vector3(0.0f, 5.0f, 5.0f),
            Ambient = new Vector3(0.05f, 0.05f, 0.05f),
            Diffuse = new Vector3(0.8f, 0.8f, 0.8f),
            Specular = new Vector3(1.0f, 1.0f, 1.0f),
            Constant = 1.0f,
            Linear = 0.09f,
            Quadratic = 0.032f
        };
        _pointLights[1] = new PointLight
        {
            Position = new Vector3(-4.0f, -3.0f, -3.0f),
            Ambient = new Vector3(0.05f, 0.05f, 0.05f),
            Diffuse = new Vector3(0.5f, 0.5f, 0.9f), 
            Specular = new Vector3(0.8f, 0.8f, 1.0f),
            Constant = 1.0f,
            Linear = 0.14f,
            Quadratic = 0.07f
        };
        _pointLights[2] = new PointLight
        {
            Position = new Vector3(5.0f, 2.0f, -6.0f),
            Ambient = new Vector3(0.05f, 0.05f, 0.05f),
            Diffuse = new Vector3(0.9f, 0.5f, 0.5f), 
            Specular = new Vector3(1.0f, 0.8f, 0.8f),
            Constant = 1.0f,
            Linear = 0.07f,
            Quadratic = 0.017f
        };
        
        _spotLight = new SpotLight
        {
            
            Position = position, 
            Direction = front,   
            Ambient = new Vector3(0.0f, 0.0f, 0.0f),
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f),   
            Specular = new Vector3(1.0f, 1.0f, 1.0f),
            Constant = 1.0f,
            Linear = 0.09f,
            Quadratic = 0.032f,
            CutOff = MathF.Cos(MathHelper.DegreesToRadians(12.5f)), 
            OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(17.5f)) 
        };

        
        if (_baseWindow.IsFocused)
        {
            lastPos = new Vector2(_baseWindow.MouseState.X, _baseWindow.MouseState.Y);
        }
        
         _baseWindow.CursorState = CursorState.Grabbed;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (!_baseWindow.IsFocused) 
        {
            return;
        }

        
        KeyboardState input = _baseWindow.KeyboardState;
        float cameraSpeed = speed * (float)args.Time;

        if (input.IsKeyDown(Keys.W)) position += front * cameraSpeed; 
        if (input.IsKeyDown(Keys.S)) position -= front * cameraSpeed; 
        if (input.IsKeyDown(Keys.A)) position -= Vector3.Normalize(Vector3.Cross(front, up)) * cameraSpeed; 
        if (input.IsKeyDown(Keys.D)) position += Vector3.Normalize(Vector3.Cross(front, up)) * cameraSpeed; 
        if (input.IsKeyDown(Keys.LeftShift)) position += up * cameraSpeed; 
        if (input.IsKeyDown(Keys.LeftControl)) position -= up * cameraSpeed; 

        MouseState mouse = _baseWindow.MouseState;

        if (firstMove)
        {
            lastPos = new Vector2(mouse.X, mouse.Y);
            firstMove = false;
        }
        else
        {
            float deltaX = mouse.X - lastPos.X;
            float deltaY = mouse.Y - lastPos.Y; 
            lastPos = new Vector2(mouse.X, mouse.Y);

            float sensitivity = 0.1f; 
            yaw += deltaX * sensitivity;
            pitch -= deltaY * sensitivity; 

            
            if (pitch > 89.0f) pitch = 89.0f;
            if (pitch < -89.0f) pitch = -89.0f;

            
            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
        }
        
        
        _spotLight.Position = position;
        _spotLight.Direction = front;
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); 
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _phongShader.Use();

        
        
        Matrix4 view = Matrix4.LookAt(position, position + front, up);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            (float)_baseWindow.ClientSize.X / _baseWindow.ClientSize.Y,
            0.1f,
            100.0f);

        _phongShader.SetMatrix4("view", view);
        _phongShader.SetMatrix4("projection", projection);

        
        _phongShader.SetVector3("viewPos", position);

        
        _phongShader.SetVector3("dirLight.direction", _dirLight.Direction);
        _phongShader.SetVector3("dirLight.ambient", _dirLight.Ambient);
        _phongShader.SetVector3("dirLight.diffuse", _dirLight.Diffuse);
        _phongShader.SetVector3("dirLight.specular", _dirLight.Specular);

        
        for (int i = 0; i < _pointLights.Length; i++)
        {
            _phongShader.SetVector3($"pointLights[{i}].position", _pointLights[i].Position);
            _phongShader.SetVector3($"pointLights[{i}].ambient", _pointLights[i].Ambient);
            _phongShader.SetVector3($"pointLights[{i}].diffuse", _pointLights[i].Diffuse);
            _phongShader.SetVector3($"pointLights[{i}].specular", _pointLights[i].Specular);
            _phongShader.SetFloat($"pointLights[{i}].constant", _pointLights[i].Constant);
            _phongShader.SetFloat($"pointLights[{i}].linear", _pointLights[i].Linear);
            _phongShader.SetFloat($"pointLights[{i}].quadratic", _pointLights[i].Quadratic);
        }
        
        
        _phongShader.SetVector3("spotLight.position", _spotLight.Position);
        _phongShader.SetVector3("spotLight.direction", _spotLight.Direction);
        _phongShader.SetVector3("spotLight.ambient", _spotLight.Ambient);
        _phongShader.SetVector3("spotLight.diffuse", _spotLight.Diffuse);
        _phongShader.SetVector3("spotLight.specular", _spotLight.Specular);
        _phongShader.SetFloat("spotLight.cutOff", _spotLight.CutOff);
        _phongShader.SetFloat("spotLight.outerCutOff", _spotLight.OuterCutOff);
        _phongShader.SetFloat("spotLight.constant", _spotLight.Constant);
        _phongShader.SetFloat("spotLight.linear", _spotLight.Linear);
        _phongShader.SetFloat("spotLight.quadratic", _spotLight.Quadratic);

        
        Matrix4 model1 = Matrix4.CreateTranslation(_sphere1Pos);
        _phongShader.SetMatrix4("model", model1);
        SetMaterialUniforms(_emeraldMaterial);
        _renderOpSphere1.Render(renderArgumentsData);

        
        Matrix4 model2 = Matrix4.CreateTranslation(_sphere2Pos);
        _phongShader.SetMatrix4("model", model2);
        SetMaterialUniforms(_redPlasticMaterial);
        _renderOpSphere2.Render(renderArgumentsData);

        GL.Disable(EnableCap.DepthTest); 
    }

    
    private void SetMaterialUniforms(Material mat)
    {
        _phongShader.SetVector3("material.ambient", mat.Ambient);
        _phongShader.SetVector3("material.diffuse", mat.Diffuse);
        _phongShader.SetVector3("material.specular", mat.Specular);
        _phongShader.SetFloat("material.shininess", mat.Shininess);
    }

    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                
                _renderOpSphere1?.Dispose();
                _renderOpSphere2?.Dispose();
            }

            
            
            _phongShader?.Dispose(); 

            
            if (_baseWindow != null && _baseWindow.CursorState == CursorState.Grabbed)
            {
                 _baseWindow.CursorState = CursorState.Normal;
            }

            disposedValue = true;
        }
    }

    public override void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}


public static class ShaderExtensions
{
    public static void SetMatrix4(this DefaultShader shader, string name, Matrix4 data)
    {
        GL.UseProgram(shader.Handle); 
        int location = GL.GetUniformLocation(shader.Handle, name);
        if (location != -1)
        {
            GL.UniformMatrix4(location, false, ref data); 
        }
        else
        {
             System.Diagnostics.Debug.WriteLine($"Warning: Uniform '{name}' not found in shader.");
        }
    }

    public static void SetVector3(this DefaultShader shader, string name, Vector3 data)
    {
        GL.UseProgram(shader.Handle);
        int location = GL.GetUniformLocation(shader.Handle, name);
        if (location != -1)
        {
            GL.Uniform3(location, data);
        }
         else
        {
             System.Diagnostics.Debug.WriteLine($"Warning: Uniform '{name}' not found in shader.");
        }
    }

    
    public static void SetFloat(this DefaultShader shader, string name, float value)
    {
       GL.UseProgram(shader.Handle);
       var location = GL.GetUniformLocation(shader.Handle, name);
       if (location != -1)
       {
           GL.Uniform1(location, value);
       }
        else
        {
             System.Diagnostics.Debug.WriteLine($"Warning: Uniform '{name}' not found in shader.");
        }
    }
}