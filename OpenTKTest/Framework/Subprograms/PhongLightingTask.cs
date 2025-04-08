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

    
    private Texture _rectTexture;
    private LitMeshRenderOperation _renderOpRect;
    private Material _rectMaterial;
    private float _rectRotationX = 0.0f;
    private TextureMinFilter _currentMinFilter = TextureMinFilter.Nearest; 
    private bool _minFilterChanged = false; 
    
    
    private struct Material
    {
        public Vector3 Specular;
        public float Shininess;
        public bool UseTexture;
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

        
        _sphereSeparationDistance = _sphereRadius * 2.0f;
        _sphere1Pos = new Vector3(-_sphereSeparationDistance / 2.0f, 0.0f, 0.0f);
        _sphere2Pos = new Vector3(_sphereSeparationDistance / 2.0f, 0.0f, 0.0f);

        
        _phongShader = new DefaultShader("Shaders/phong_vert.shader", "Shaders/phong_frag.shader");
        _phongShader.Init();

        
        var sphereMeshData = ShapeUtils.CreateSphere(_sphereRadius, 36, 18);

        
        _renderOpSphere1 = new LitMeshRenderOperation(sphereMeshData.Vertices, sphereMeshData.Normals, null, sphereMeshData.Indices, _phongShader);
        _renderOpSphere1.Init();
        _renderOpSphere2 = new LitMeshRenderOperation(sphereMeshData.Vertices, sphereMeshData.Normals, null, sphereMeshData.Indices, _phongShader);
        _renderOpSphere2.Init();

        
        _emeraldMaterial = new Material
        {
            
            Specular = new Vector3(0.633f, 0.727811f, 0.633f),
            Shininess = 0.6f * 128.0f,
            UseTexture = false 
        };
        _redPlasticMaterial = new Material
        {
            Specular = new Vector3(0.7f, 0.6f, 0.6f),
            Shininess = 0.25f * 128.0f,
            UseTexture = false 
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

        
        
        string texturePath = Path.Combine("Data", "Textures", "wall.jpg"); 
        if (!File.Exists(texturePath))
        {
            
            Console.WriteLine($"Error: Texture not found at {Path.GetFullPath(texturePath)}");
            
             _rectTexture = null; 
        }
        else
        {
            _rectTexture = Texture.LoadFromFile(texturePath);
        }

        
        float rectSize = _sphereSeparationDistance * 1.5f;
        float[] rectVertices = {
             rectSize / 2f,  rectSize / 2f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f, 
             rectSize / 2f, -rectSize / 2f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f, 
            -rectSize / 2f, -rectSize / 2f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f, 
            -rectSize / 2f,  rectSize / 2f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f  
        };
        uint[] rectIndices = {
            0, 1, 3, 
            1, 2, 3  
        };

        
        float[] rVerts = new float[4 * 3];
        float[] rNorms = new float[4 * 3];
        float[] rTexCoords = new float[4 * 2];
        int vIdx = 0, nIdx = 0, tIdx = 0;
        for (int i = 0; i < 4; ++i) {
            rVerts[vIdx++] = rectVertices[i*8 + 0];
            rVerts[vIdx++] = rectVertices[i*8 + 1];
            rVerts[vIdx++] = rectVertices[i*8 + 2];
            rNorms[nIdx++] = rectVertices[i*8 + 3];
            rNorms[nIdx++] = rectVertices[i*8 + 4];
            rNorms[nIdx++] = rectVertices[i*8 + 5];
            rTexCoords[tIdx++] = rectVertices[i*8 + 6];
            rTexCoords[tIdx++] = rectVertices[i*8 + 7];
        }


        
        _renderOpRect = new LitMeshRenderOperation(rVerts, rNorms, rTexCoords, rectIndices, _phongShader);
        _renderOpRect.Init();

        
        _rectMaterial = new Material
        {
            Specular = new Vector3(0.1f, 0.1f, 0.1f), 
            Shininess = 0.1f * 128.0f,
            UseTexture = true 
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
        
        
        float rotationSpeed = 90.0f * (float)args.Time; 

        
        if (input.IsKeyDown(Keys.Right))
        {
            _rectRotationX += MathHelper.DegreesToRadians(rotationSpeed);
        }
        if (input.IsKeyDown(Keys.Left))
        {
            _rectRotationX -= MathHelper.DegreesToRadians(rotationSpeed);
        }

        
        if (input.IsKeyReleased(Keys.D1))
        {
            if (_currentMinFilter != TextureMinFilter.Nearest)
            {
                _currentMinFilter = TextureMinFilter.Nearest;
                _minFilterChanged = true;
                Console.WriteLine("Min Filter: Nearest");
            }
        }
        if (input.IsKeyReleased(Keys.D2))
        {
            if (_currentMinFilter != TextureMinFilter.Linear)
            {
                _currentMinFilter = TextureMinFilter.Linear;
                _minFilterChanged = true;
                Console.WriteLine("Min Filter: Bilinear (LinearMipmapLinear)");
            }
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

        SetupLightUniforms();

        
        if (_minFilterChanged && _rectTexture != null)
        {
            GL.BindTexture(TextureTarget.Texture2D, _rectTexture.Handle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_currentMinFilter);
            GL.BindTexture(TextureTarget.Texture2D, 0); 
            _minFilterChanged = false;
        }

        
        if (_renderOpRect != null && _rectTexture != null) 
        {
            
            _rectTexture.Use(TextureUnit.Texture0);
            _phongShader.SetInt("texture0", 0); 

            
            SetMaterialUniforms(_rectMaterial); 

            
            Matrix4 rectModel = Matrix4.CreateRotationX(_rectRotationX)
                                * Matrix4.CreateTranslation(0.0f, -_sphereRadius - 5.0f, 0.0f); 
            _phongShader.SetMatrix4("model", rectModel);

            
            _renderOpRect.Render(renderArgumentsData);

            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        
        
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
        _phongShader.SetVector3("material.specular", mat.Specular);
        _phongShader.SetFloat("material.shininess", mat.Shininess);
        _phongShader.SetBool("material.useTexture", mat.UseTexture); 
    }
    
    
    private void SetupLightUniforms()
    {
        
        _phongShader.SetVector3("dirLight.direction", _dirLight.Direction);
        _phongShader.SetVector3("dirLight.ambient", _dirLight.Ambient);
        _phongShader.SetVector3("dirLight.diffuse", _dirLight.Diffuse);
        _phongShader.SetVector3("dirLight.specular", _dirLight.Specular);

        
        for (int i = 0; i < _pointLights.Length; i++)
        {
            string prefix = $"pointLights[{i}].";
            _phongShader.SetVector3(prefix + "position", _pointLights[i].Position);
            _phongShader.SetVector3(prefix + "ambient", _pointLights[i].Ambient);
            _phongShader.SetVector3(prefix + "diffuse", _pointLights[i].Diffuse);
            _phongShader.SetVector3(prefix + "specular", _pointLights[i].Specular);
            _phongShader.SetFloat(prefix + "constant", _pointLights[i].Constant);
            _phongShader.SetFloat(prefix + "linear", _pointLights[i].Linear);
            _phongShader.SetFloat(prefix + "quadratic", _pointLights[i].Quadratic);
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
    }

    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                
                _renderOpSphere1?.Dispose();
                _renderOpSphere2?.Dispose();
                _renderOpRect?.Dispose(); 
                _rectTexture?.Dispose(); 
            }

            
            _phongShader?.Dispose();

            
            if (_baseWindow != null && _baseWindow.CursorState == CursorState.Grabbed) { /* ... */ }

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
    
    public static void SetInt(this DefaultShader shader, string name, int value)
    {
        GL.UseProgram(shader.Handle);
        int location = GL.GetUniformLocation(shader.Handle, name);
        if (location != -1) GL.Uniform1(location, value);
        else System.Diagnostics.Debug.WriteLine($"Warning: Uniform '{name}' not found.");
    }

    public static void SetBool(this DefaultShader shader, string name, bool value)
    {
        GL.UseProgram(shader.Handle);
        int location = GL.GetUniformLocation(shader.Handle, name);
        if (location != -1) GL.Uniform1(location, value ? 1 : 0); 
        else System.Diagnostics.Debug.WriteLine($"Warning: Uniform '{name}' not found.");
    }
}

