using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public enum TaskFiveOutputMode
{
    Triangle,
    TriangleStrip,
    TriangleFan
}

public abstract class FigureOutputContextBase
{
    public abstract void Init();
    public abstract void Render(RenderArgumentsData renderArgumentsData);
}

public class TriangleFigureOutputContextData
{
    public ShapeUtils.VerticesData LinesPoints { get; set; }
    public PrimitiveType PrimitiveType { get; set; }
    
    public Action? OnPreRender { get; set; }
}

public class TriangleFigureOutputContext : FigureOutputContextBase
{
    private readonly BaseWindow _baseWindow;
    private RenderOperation _polygonRenderOperation;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;
    private ShapeUtils.VerticesData _linesPoints;
    private readonly PrimitiveType _primitiveType;
    private readonly Action? _onPreRender;

    public TriangleFigureOutputContext(TriangleFigureOutputContextData triangleFigureOutputContextData)
    {
        _linesPoints = triangleFigureOutputContextData.LinesPoints;
        _primitiveType = triangleFigureOutputContextData.PrimitiveType;
        _onPreRender = triangleFigureOutputContextData.OnPreRender;
    }

    public override void Init()
    {
        _shaderToUse = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");

        _renderOperationData = new RenderOperationData(_primitiveType);
        _polygonRenderOperation =
            new RenderOperation(_linesPoints.Vertices, _linesPoints.Colors, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        _onPreRender?.Invoke();
        _polygonRenderOperation.Render(renderArgumentsData);
    }
}


public class TaskFiveFigureWithTrianglesSubprogram : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private readonly Queue<FigureOutputContextBase> _allModesQueue;
    private FigureOutputContextBase _currentMode;

    public TaskFiveFigureWithTrianglesSubprogram(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
        List<FigureOutputContextBase> allModes = new()
        {
            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = ShapeUtils.CreatePoints(new List<Vector2>()
                {
                    new Vector2(540, 440),
                    new Vector2(1132, 624),
                    new Vector2(960, 1080),

                    new Vector2(960, 1080),
                    new Vector2(1132, 624),
                    new Vector2(1300, 880),

                    new Vector2(540, 440),
                    new Vector2(1220, 40),
                    new Vector2(1132, 624),

                    new Vector2(1132, 624),
                    new Vector2(1220, 40),
                    new Vector2(1472, 424),
                }, ShapeUtils.RandomColorPerTriangleCallback),
                PrimitiveType = PrimitiveType.Triangles
            }),

            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = ShapeUtils.CreatePoints(new List<Vector2>()
                {
                    new Vector2(1300, 880),// 5
                    new Vector2(960, 1080),// 4
                    new Vector2(1132, 624),// 2
                    new Vector2(540, 440),// 3
                    new Vector2(1472, 424), // 0
                    new Vector2(1220, 40),// 1
                }, ShapeUtils.RandomColorPerTriangleCallback),
                PrimitiveType = PrimitiveType.TriangleStrip
            }),
            
            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = ShapeUtils.CreatePoints(new List<Vector2>()
                {
                    new Vector2(1132, 624),
                    new Vector2(1472, 424),
                    new Vector2(1220, 40),
                    new Vector2(540, 440),
                    new Vector2(960, 1080),
                    new Vector2(1300, 880),
                }, ShapeUtils.RandomColorPerTriangleCallback),
                PrimitiveType = PrimitiveType.TriangleFan
            }),
        };

        _allModesQueue = new Queue<FigureOutputContextBase>(allModes);
    }

    public override void Init()
    {
        _baseWindow.Title = "RML Labs | Task 5";

        TryMoveToNextMode();
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        if (_baseWindow.KeyboardState.IsKeyPressed(Keys.Up))
        {
            TryMoveToNextMode();
        }
    }

    private bool TryMoveToNextMode()
    {
        if (_allModesQueue.Count > 0)
        {
            _currentMode = _allModesQueue.Dequeue();
            _currentMode.Init();
            return true;
        }

        return false;
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
       
        _currentMode.Render(renderArgumentsData);
    }
}