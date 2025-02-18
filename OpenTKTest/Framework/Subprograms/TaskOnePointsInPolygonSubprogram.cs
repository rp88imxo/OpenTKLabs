using OpenTK.Graphics.OpenGL;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public readonly struct TaskOneSubprogramParams
{
    public TaskOneSubprogramParams(int sidesCount)
    {
        SidesCount = sidesCount;
    }

    public int SidesCount { get; }
}

public class TaskOnePointsInPolygonSubprogram : BaseSubprogram
{
    private readonly TaskOneSubprogramParams _taskOneSubprogramParams;
    private readonly BaseWindow _baseWindow;
    private RenderOperation _polygonRenderOperation;
    private ShapeUtils.NPolygonData _polygonPoints;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;

    public TaskOnePointsInPolygonSubprogram(TaskOneSubprogramParams taskOneSubprogramParams, BaseWindow baseWindow)
    {
        _taskOneSubprogramParams = taskOneSubprogramParams;
        _baseWindow = baseWindow;
    }
    
    public override void Init()
    {
        _polygonPoints = ShapeUtils.CreateNPolygon(_taskOneSubprogramParams.SidesCount);

        _shaderToUse = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        
        _renderOperationData = new RenderOperationData(PrimitiveType.Points);
        _polygonRenderOperation = new RenderOperation(_polygonPoints.Vertices, _polygonPoints.Colors, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();
        
        _baseWindow.Title = "RML Labs | Task 1";
    }
    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        GL.Enable(EnableCap.PointSmooth);
        GL.PointSize(10);
        
        _polygonRenderOperation.Render(renderArgumentsData);
    }
}