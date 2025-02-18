using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TaskFourFigureWithLineLoopSubprogram : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private RenderOperation _polygonRenderOperation;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;
    private ShapeUtils.VerticesData _linesPoints;

    public TaskFourFigureWithLineLoopSubprogram( BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
    }
    
    public override void Init()
    {
        _linesPoints = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(540, 440),
            new Vector2(960, 1080),
            new Vector2(1300, 880),
            new Vector2(1132, 624),
            new Vector2(1472, 424),
            new Vector2(1220, 40),
        }, ShapeUtils.DefaultColorCallback);

        _shaderToUse = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        
        _renderOperationData = new RenderOperationData(PrimitiveType.LineLoop);
        _polygonRenderOperation = new RenderOperation(_linesPoints.Vertices, _linesPoints.Colors, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();
        
        _baseWindow.Title = "RML Labs | Task 4";
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        //GL.Enable(EnableCap.LineSmooth);
        //GL.LineWidth(_lineWidth);
        
        _polygonRenderOperation.Render(renderArgumentsData);
    }
}