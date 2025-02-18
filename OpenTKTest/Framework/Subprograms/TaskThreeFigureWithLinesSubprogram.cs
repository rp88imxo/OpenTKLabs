using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TaskThreeFigureWithLinesSubprogram : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private RenderOperation _polygonRenderOperation;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;
    private ShapeUtils.VerticesData _linesPoints;

    public TaskThreeFigureWithLinesSubprogram( BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
    }
    
    public override void Init()
    {
        _linesPoints = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(100, 700),
            new Vector2(700, 850),
            new Vector2(900, 540),
            new Vector2(400, 200),
            new Vector2(1280, 200),
            new Vector2(1700, 540),
        }, ShapeUtils.DefaultColorCallback);

        _shaderToUse = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        
        _renderOperationData = new RenderOperationData(PrimitiveType.LineStrip);
        _polygonRenderOperation = new RenderOperation(_linesPoints.Vertices, _linesPoints.Colors, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();
        
        _baseWindow.Title = "RML Labs | Task 3";
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