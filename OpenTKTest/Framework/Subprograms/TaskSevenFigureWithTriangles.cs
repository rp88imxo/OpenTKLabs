using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TaskSevenFigureWithTriangles : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private readonly Queue<FigureOutputContextBase> _allModesQueue;
    private FigureOutputContextBase _currentMode;

    public TaskSevenFigureWithTriangles( BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;

        var points = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(300, 100),
            new Vector2(300, 980),
            new Vector2(400, 560),
            
            new Vector2(300, 980),
            new Vector2(400, 560),
            new Vector2(1050, 980),
            
            new Vector2(300, 100),
            new Vector2(900, 300),
            new Vector2(400, 560),
            
            new Vector2(300, 100),
            new Vector2(900, 300),
            new Vector2(1655, 100),
            
            new Vector2(900, 300),
            new Vector2(1655, 100),
            new Vector2(1355, 594),
            
            new Vector2(1355, 594),
            new Vector2(1655, 100),
            new Vector2(1655, 733),
            
            new Vector2(1355, 594),
            new Vector2(1655, 733),
            new Vector2(1355, 833),
            
            // new Vector2(300, 980),
            // new Vector2(1050, 980),
            // new Vector2(400, 560),
            // new Vector2(900, 300),
            // new Vector2(1355, 594),
            // new Vector2(1355, 833),
            // new Vector2(1655, 733),
            // new Vector2(1655, 100),
          
        }, ShapeUtils.RandomColorPerTriangleCallback);
        
        List<FigureOutputContextBase> allModes = new()
        {
            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = points,
                PrimitiveType = PrimitiveType.Triangles,
                OnPreRender = OnPreRenderFaceVertex
            }),
            
            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = points,
                PrimitiveType = PrimitiveType.Triangles,
                OnPreRender = OnPreRenderFaceFillBackLines
            }),
            
            new TriangleFigureOutputContext(new TriangleFigureOutputContextData
            {
                LinesPoints = points,
                PrimitiveType = PrimitiveType.Triangles,
                OnPreRender = OnPreRenderFrontBackLines
            }),
        };

        _allModesQueue = new Queue<FigureOutputContextBase>(allModes);
    }

    private void OnPreRenderFrontBackLines()
    {
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
    }

    private void OnPreRenderFaceFillBackLines()
    {
        GL.PolygonMode(TriangleFace.Front, PolygonMode.Fill);
        GL.PolygonMode(TriangleFace.Back, PolygonMode.Line);
    }

    private void OnPreRenderFaceVertex()
    {
        GL.PolygonMode(TriangleFace.Front, PolygonMode.Point);
    }

    public override void Init()
    {
        _baseWindow.Title = "RML Labs | Task 7";

        TryMoveToNextMode();
    }
    
    public override void Update(FrameEventArgs args)
    {
        base.Update(args);
        
        if ( _baseWindow.KeyboardState.IsKeyPressed(Keys.Up))
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