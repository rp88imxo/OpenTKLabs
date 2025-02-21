using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public interface IPreRenderContext
{
    public void Init();

    public void Update(FrameEventArgs args);

    public void OnPreRender(RenderArgumentsData renderArgumentsData, DefaultShader currentShader);
}

public class PreRenderContext : IPreRenderContext
{
    protected readonly BaseWindow BaseWindow;

    public PreRenderContext(BaseWindow baseWindow)
    {
        BaseWindow = baseWindow;
    }

    public virtual void Init()
    {
        
    }
    
    public virtual void Update(FrameEventArgs args)
    {
        
    }

    public virtual void OnPreRender(RenderArgumentsData renderArgumentsData, DefaultShader currentShader)
    {
       
    }
}

public class TrianglePreRenderContext : PreRenderContext
{
    private float _interpolator;
    private float _scaleValueX;
    private float _scaleValueY;
    private float _transCalculatedX;
    private float _transCalculatedY;

    public TrianglePreRenderContext(BaseWindow baseWindow) : base(baseWindow)
    {
        _interpolator = 0f;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Right))
        {
            _interpolator += 1.0f * (float)args.Time;
            if (_interpolator > 1.0f)
            {
                _interpolator = 1.0f;
            }
        }
        
        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Left))
        {
            _interpolator -= 1.0f * (float)args.Time;
            if (_interpolator < 0.0f)
            {
                _interpolator = 0.0f;
            }
        }

        _scaleValueX = MathHelper.Lerp(1.0f, 2f, _interpolator);
        _scaleValueY = MathHelper.Lerp(1.0f, 1.5f, _interpolator);
        
        _transCalculatedX = MathHelper.Lerp(0.0f, 0.2f, _interpolator);
        _transCalculatedY = MathHelper.Lerp(0.0f, 0.5f, _interpolator);
    }

    public override void OnPreRender(RenderArgumentsData renderArgumentsData, DefaultShader currentShader)
    {
        base.OnPreRender(renderArgumentsData, currentShader);
        
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        
        Matrix4 scale = Matrix4.CreateScale(_scaleValueX, _scaleValueY, 1.0f);

        var transX = MathUtils.Remap(320, 0, 1920, 1f, -1f);
        var transY = MathUtils.Remap(540, 0, 1080, 1f, -1f);

        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);
        var translationCalculated = Matrix4.CreateTranslation(_transCalculatedX, _transCalculatedY, 0.0f);
        
        // var currentRotation = (float)renderArgumentsData.TotalTimePassed * 25;
        // var scaleValue = ((MathF.Sin((float)renderArgumentsData.TotalTimePassed * 2) + 1) / 2);
        //
        // Matrix4 rotationMatrix =
        //     Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation));
        // Matrix4 scale = Matrix4.CreateScale(scaleValue, scaleValue, scaleValue);
        Matrix4 trs = scale * translationDefault * translationCalculated;

        var transformLocation = GL.GetUniformLocation(currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }
}

public class RectanglePreRenderContext : PreRenderContext
{
    private float _interpolator;
    private float _angleValue;

    private float _interpolationSpeed;
    
    public RectanglePreRenderContext(BaseWindow baseWindow) : base(baseWindow)
    {
        _interpolator = 0f;
        _interpolationSpeed = 1.0f;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Right))
        {
            _interpolator += _interpolationSpeed * (float)args.Time;
            if (_interpolator > 1.0f)
            {
                _interpolator = 1.0f;
            }
        }
        
        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Left))
        {
            _interpolator -= _interpolationSpeed * (float)args.Time;
            if (_interpolator < 0.0f)
            {
                _interpolator = 0.0f;
            }
        }


        _angleValue = MathHelper.Lerp(0f, 15f, _interpolator);
    }

    public override void OnPreRender(RenderArgumentsData renderArgumentsData, DefaultShader currentShader)
    {
        base.OnPreRender(renderArgumentsData, currentShader);
        
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        
        //Matrix4 scale = Matrix4.CreateScale(_scaleValueX, _scaleValueY, 1.0f);

        // var transX = MathUtils.Remap(320, 0, 1920, 1f, -1f);
        // var transY = MathUtils.Remap(540, 0, 1080, 1f, -1f);

        var translationDefault = Matrix4.CreateTranslation(-3f, -3f, 0.0f);

        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_angleValue));

        var calculatedMatrix = translationDefault * rotationMatrix;
        
        //var translationCalculated = Matrix4.CreateTranslation(_transCalculatedX, _transCalculatedY, 0.0f);
        
        // var currentRotation = (float)renderArgumentsData.TotalTimePassed * 25;
        // var scaleValue = ((MathF.Sin((float)renderArgumentsData.TotalTimePassed * 2) + 1) / 2);
        //
        // Matrix4 rotationMatrix =
        //     Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation));
        // Matrix4 scale = Matrix4.CreateScale(scaleValue, scaleValue, scaleValue);
        Matrix4 trs =  calculatedMatrix * Matrix4.Invert(translationDefault);

        var transformLocation = GL.GetUniformLocation(currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }
}

public class LinePreRenderContext : PreRenderContext
{
    private float _interpolator;
    private float _angleValue;
    
    public LinePreRenderContext(BaseWindow baseWindow) : base(baseWindow)
    {
        _interpolator = 0f;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Right))
        {
            _interpolator += 1.0f * (float)args.Time;
            if (_interpolator > 1.0f)
            {
                _interpolator = 1.0f;
            }
        }
        
        if (BaseWindow.KeyboardState.IsKeyDown(Keys.Left))
        {
            _interpolator -= 1.0f * (float)args.Time;
            if (_interpolator < 0.0f)
            {
                _interpolator = 0.0f;
            }
        }

        _angleValue = MathHelper.Lerp(0f, 30f, _interpolator);
    }

    public override void OnPreRender(RenderArgumentsData renderArgumentsData, DefaultShader currentShader)
    {
        base.OnPreRender(renderArgumentsData, currentShader);
        
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        
        //Matrix4 scale = Matrix4.CreateScale(_scaleValueX, _scaleValueY, 1.0f);

        var transX = MathUtils.Remap(1800, 0, 1920, 1f, -1f);
        var transY = MathUtils.Remap(540, 0, 1080, 1f, -1f);

        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_angleValue));

        var calculatedMatrix = translationDefault * rotationMatrix;
        
        //var translationCalculated = Matrix4.CreateTranslation(_transCalculatedX, _transCalculatedY, 0.0f);
        
        // var currentRotation = (float)renderArgumentsData.TotalTimePassed * 25;
        // var scaleValue = ((MathF.Sin((float)renderArgumentsData.TotalTimePassed * 2) + 1) / 2);
        //
        // Matrix4 rotationMatrix =
        //     Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation));
        // Matrix4 scale = Matrix4.CreateScale(scaleValue, scaleValue, scaleValue);
        Matrix4 trs =  calculatedMatrix;

        var transformLocation = GL.GetUniformLocation(currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }
}

public class TaskTwoAffineTransformSubprogram : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private readonly Queue<FigureOutputContextBase> _allModesQueue;
    private FigureOutputContextBase _currentMode;
    private readonly DefaultShader _shaderToUse;
    private readonly IList<IPreRenderContext> _preRenderContexts;
    
    public TaskTwoAffineTransformSubprogram(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
        
        _shaderToUse = new DefaultShader("Shaders/vertex_transform.shader", "Shaders/fragment.shader");
        
        _preRenderContexts = new List<IPreRenderContext>()
        {
            new TrianglePreRenderContext(baseWindow),
            new RectanglePreRenderContext(baseWindow),
            new LinePreRenderContext(baseWindow)
        };
        
        List<FigureOutputContextBase> allModes = new()
        {
            new OutputContextAggregator(new List<FigureOutputContextBase>()
            {
                CreateTriangleOutputContextBeforeTransform(),
                CreateRectangleOutputContextBeforeTransform(),
                CreateLineOutputContextBeforeTransform()
            })
        };

        _allModesQueue = new Queue<FigureOutputContextBase>(allModes);
    }

    public override void Init()
    {
        base.Init();

        _baseWindow.Title = "RML Labs | Task 2";

        TryMoveToNextMode();
        
        foreach (var preRenderContext in _preRenderContexts)
        {
            preRenderContext.Init();
        }
    }

    private FigureOutputContextBase CreateTriangleOutputContextBeforeTransform()
    {
        var triangleData = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(960 - 100, 540), // left one
            new Vector2(960 + 100, 540), // right one
            new Vector2(960, 540 + 100), // top one
        }, ShapeUtils.RandomColorPerTriangleCallback);
        
        var dataSubtaskOne = new TriangleFigureOutputContextData()
        {
            WindowTitleName = "RML Labs | Task 2",
            LinesPoints = triangleData,
            Indices = new uint[] { 0, 1, 2 },
            PrimitiveType = PrimitiveType.Triangles,
            OnPreRender = _preRenderContexts[0].OnPreRender,
            VertexShaderPath = "Shaders/vertex_transform.shader"
        };

        var outputContextTriangleBeforeTransformed = new TriangleFigureOutputContext(_baseWindow, dataSubtaskOne);

        return outputContextTriangleBeforeTransformed;
    }
    
    private FigureOutputContextBase CreateRectangleOutputContextBeforeTransform()
    {
        var triangleData = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(960 - 100, 540), // left bottom
            new Vector2(960 + 100, 540), // right bottom
            new Vector2(960 - 100, 540 + 100), // left top
            new Vector2(960 + 100, 540 + 100), // right top
        }, ShapeUtils.RandomColorPerTriangleCallback);
        
        var dataSubtaskOne = new TriangleFigureOutputContextData()
        {
            WindowTitleName = "RML Labs | Task 2",
            LinesPoints = triangleData,
            Indices = new uint[] { 0, 1, 2, 2,1,3 },
            PrimitiveType = PrimitiveType.Triangles,
            VertexShaderPath = "Shaders/vertex_transform.shader",
            OnPreRender = _preRenderContexts[1].OnPreRender,
        };

        var beforeTransformed = new TriangleFigureOutputContext(_baseWindow, dataSubtaskOne);

        return beforeTransformed;
    }
    
    private FigureOutputContextBase CreateLineOutputContextBeforeTransform()
    {
        var triangleData = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(960 - 100, 540), // left
            new Vector2(540 + 100, 540), // right
          
        }, ShapeUtils.RandomColorPerTriangleCallback);

        var dataSubtaskOne = new TriangleFigureOutputContextData()
        {
            WindowTitleName = "RML Labs | Task 2",
            LinesPoints = triangleData,
            Indices = new uint[] { 0, 1},
            PrimitiveType = PrimitiveType.Lines,
            VertexShaderPath = "Shaders/vertex_transform.shader",
            OnPreRender = _preRenderContexts[2].OnPreRender,
        };

        var beforeTransformed = new TriangleFigureOutputContext(_baseWindow, dataSubtaskOne);

        return beforeTransformed;
    }
    
    private bool TryMoveToNextMode()
    {
        if (_allModesQueue.Count > 0)
        {
            var oldMode = _currentMode;
            _currentMode = _allModesQueue.Dequeue();
            _currentMode.Init();
            if (oldMode != null)
            {
                _allModesQueue.Enqueue(oldMode);
            }
            return true;
        }

        return false;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (_baseWindow.KeyboardState.IsKeyPressed(Keys.Up))
        {
            TryMoveToNextMode();
        }
        
        foreach (var preRenderContext in _preRenderContexts)
        {
            preRenderContext.Update(args);
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        _currentMode.Render(renderArgumentsData);
    }

    private List<IRenderOperation> CreateRenderOperationsBeforeTransform()
    {
        var renderOperations = new List<IRenderOperation>();

        // Triangle
        var triangleData = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(320 - 100, 540), // left one
            new Vector2(320 + 100, 540), // right one
            new Vector2(320, 540 + 100), // top one
        }, ShapeUtils.RandomColorPerTriangleCallback);

        var renderOperationDataTriangle = new RenderOperationData(PrimitiveType.Triangles);

        IRenderOperation renderOperationTriangle = new ElementBufferRenderOperation(
            triangleData.Vertices,
            triangleData.Colors, 
            new uint[] { 0, 1, 2 },
            _shaderToUse, 
            renderOperationDataTriangle);

        renderOperations.Add(renderOperationTriangle);
        
        return renderOperations;
    }
}