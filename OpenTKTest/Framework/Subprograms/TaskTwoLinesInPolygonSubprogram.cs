using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public readonly struct TaskTwoSubprogramParams
{
    public TaskTwoSubprogramParams(int sidesCount)
    {
        SidesCount = sidesCount;
    }

    public int SidesCount { get; }
}

public class TaskTwoLinesInPolygonSubprogram : BaseSubprogram
{
    private readonly TaskTwoSubprogramParams _taskOneSubprogramParams;
    private readonly BaseWindow _baseWindow;
    private RenderOperation _polygonRenderOperation;
    private ShapeUtils.VerticesData _polygonPoints;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;
    
    private float _lineWidth;
    private const float LineWidthMax = 50f;
    private const float LineWidthMin = 0.1f;

    public TaskTwoLinesInPolygonSubprogram(TaskTwoSubprogramParams taskOneSubprogramParams, BaseWindow baseWindow)
    {
        _taskOneSubprogramParams = taskOneSubprogramParams;
        _baseWindow = baseWindow;
    }
    
    public override void Init()
    {
        _polygonPoints = ShapeUtils.CreateNPolygonLines(_taskOneSubprogramParams.SidesCount);

        _shaderToUse = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        
        _renderOperationData = new RenderOperationData(PrimitiveType.Lines);
        _polygonRenderOperation = new RenderOperation(_polygonPoints.Vertices, _polygonPoints.Colors, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();
        
        _baseWindow.Title = "RML Labs | Task 2";

        _lineWidth = 1.0f;
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        _baseWindow.Title = $"RML Labs | Task 2 | Line Width: {_lineWidth}";
        
        if (_baseWindow.KeyboardState.IsKeyDown(Keys.Up))
        {
            _lineWidth += 0.01f;
            if (_lineWidth > LineWidthMax)
            {
                _lineWidth = LineWidthMax;
            }
        }

        if (_baseWindow.KeyboardState.IsKeyDown(Keys.Down))
        {
            _lineWidth -= 0.01f;
            if (_lineWidth < LineWidthMin)
            {
                _lineWidth = LineWidthMin;
            }
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        //GL.Enable(EnableCap.LineSmooth);
        GL.LineWidth(_lineWidth);
        
        _polygonRenderOperation.Render(renderArgumentsData);
    }
}