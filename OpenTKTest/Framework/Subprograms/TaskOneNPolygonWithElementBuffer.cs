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
    public uint[] Indices { get; set; }
    public PrimitiveType PrimitiveType { get; set; }
    
    public Action<RenderArgumentsData, DefaultShader>? OnPreRender { get; set; }
    public string WindowTitleName { get; set; }
    public string VertexShaderPath { get; set; } = "Shaders/vertex.shader";
    public string FragmentShaderPath { get; set; } = "Shaders/fragment.shader";
}

public class TriangleFigureOutputContext : FigureOutputContextBase
{
    private readonly BaseWindow _baseWindow;
    private IRenderOperation _polygonRenderOperation;
    private DefaultShader _shaderToUse;
    private RenderOperationData _renderOperationData;
    private ShapeUtils.VerticesData _linesPoints;
    private readonly PrimitiveType _primitiveType;
    private readonly Action<RenderArgumentsData, DefaultShader>? _onPreRender;
    private readonly uint[] _indices;
    private readonly string _windowTitle;
    private readonly string _vertexShaderPath;
    private readonly string _fragmentShaderPath;

    public TriangleFigureOutputContext(BaseWindow baseWindow, TriangleFigureOutputContextData triangleFigureOutputContextData)
    {
        _linesPoints = triangleFigureOutputContextData.LinesPoints;
        _primitiveType = triangleFigureOutputContextData.PrimitiveType;
        _onPreRender = triangleFigureOutputContextData.OnPreRender;
        _indices = triangleFigureOutputContextData.Indices;
        _windowTitle = triangleFigureOutputContextData.WindowTitleName;
        _baseWindow = baseWindow;
        _vertexShaderPath = triangleFigureOutputContextData.VertexShaderPath;
        _fragmentShaderPath = triangleFigureOutputContextData.FragmentShaderPath;
    }

    public override void Init()
    {
        _shaderToUse = new DefaultShader(_vertexShaderPath, _fragmentShaderPath);
        
        _renderOperationData = new RenderOperationData(_primitiveType);
        _polygonRenderOperation =
            new ElementBufferRenderOperation(_linesPoints.Vertices, _linesPoints.Colors, _indices, _shaderToUse, _renderOperationData);
        _polygonRenderOperation.Init();

        if (!string.IsNullOrEmpty(_windowTitle))
        {
            _baseWindow.Title = _windowTitle;
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        if (!string.IsNullOrEmpty(_windowTitle))
        {
            _baseWindow.Title = _windowTitle;
        }
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        _shaderToUse.Use();
        _onPreRender?.Invoke(renderArgumentsData, _shaderToUse);
        _polygonRenderOperation.Render(renderArgumentsData);
    }
}

public class OutputContextAggregator : FigureOutputContextBase
{
    private readonly IList<FigureOutputContextBase> _figureOutputContextBases;

    public OutputContextAggregator(IList<FigureOutputContextBase> figureOutputContextBases)
    {
        _figureOutputContextBases = figureOutputContextBases;
    }
    
    public override void Init()
    {
        foreach (var figureOutputContextBase in _figureOutputContextBases)
        {
            figureOutputContextBase.Init();
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        foreach (var figureOutputContextBase in _figureOutputContextBases)
        {
            figureOutputContextBase.Render(renderArgumentsData);
        }
    }
}

public class TaskOneNPolygonWithElementBuffer : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private readonly Queue<FigureOutputContextBase> _allModesQueue;
    private FigureOutputContextBase _currentMode;

    public TaskOneNPolygonWithElementBuffer(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;

        #region SUBTASK_ONE

        var verticesDataTaskOne = ShapeUtils.CreateNPolygon(24, ShapeUtils.RandomColorPerTriangleCallback);
        var dataSubtaskOne = new TriangleFigureOutputContextData
        {
            LinesPoints = verticesDataTaskOne,
            Indices = CalculateIndicesForTaskSubtaskOne(verticesDataTaskOne),
            PrimitiveType = PrimitiveType.Triangles,
            OnPreRender = OnPreRenderSubTaskOne,
            WindowTitleName = "RML Labs | Task 1 | Subtask 1"
        };

        #endregion


        #region SUBTASK_TWO

        var verticesDataTaskTwo = ShapeUtils.CreateNPolygon(24, ShapeUtils.RandomColorPerTriangleCallback);
        var dataSubtaskTwo = new TriangleFigureOutputContextData
        {
            LinesPoints = verticesDataTaskTwo,
            Indices = CalculateIndicesForTaskSubtaskTwo(verticesDataTaskTwo),
            PrimitiveType = PrimitiveType.Lines,
            OnPreRender = OnPreRenderSubTaskOne,
            WindowTitleName = "RML Labs | Task 1 | Subtask 2"
        };

        #endregion
        
        #region SUBTASK_THREE

        var verticesDataTaskThree = ShapeUtils.CreateNPolygon(24, ShapeUtils.RandomColorPerTriangleCallback);
        var dataSubtaskThree = new TriangleFigureOutputContextData
        {
            LinesPoints = verticesDataTaskThree,
            Indices = CalculateIndicesForTaskSubtaskThree(verticesDataTaskThree),
            PrimitiveType = PrimitiveType.LineLoop,
            OnPreRender = OnPreRenderSubTaskOne,
            WindowTitleName = "RML Labs | Task 1 | Subtask 3"
        };

        #endregion
        
        #region SUBTASK_FOUR

        var verticesDataTaskFour= ShapeUtils.CreateNPolygon(24, ShapeUtils.RandomColorPerTriangleCallback);
        var dataSubtaskFour = new TriangleFigureOutputContextData
        {
            LinesPoints = verticesDataTaskFour,
            Indices = CalculateIndicesForTaskSubtaskFour(verticesDataTaskFour),
            PrimitiveType = PrimitiveType.LineLoop,
            OnPreRender = OnPreRenderSubTaskOne,
            WindowTitleName = "RML Labs | Task 1 | Subtask 4"
        };

        #endregion
        
        List<FigureOutputContextBase> allModes = new()
        {
            new TriangleFigureOutputContext(_baseWindow,dataSubtaskOne),
            new TriangleFigureOutputContext(_baseWindow,dataSubtaskTwo),
            new TriangleFigureOutputContext(_baseWindow,dataSubtaskThree),
            new TriangleFigureOutputContext(_baseWindow,dataSubtaskFour),
        };

        _allModesQueue = new Queue<FigureOutputContextBase>(allModes);
    }

    private void OnPreRenderSubTaskOne(RenderArgumentsData renderArgumentsData, DefaultShader currentShader)
    {
        GL.PointSize(5f);
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
    }

    public override void Init()
    {
        _baseWindow.Title = "RML Labs | Task 1";

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

    private uint[] CalculateIndicesForTaskSubtaskOne(ShapeUtils.VerticesData verticesData)
    {
        var pointsCount = verticesData.Vertices.Length / 3;

        var indicesToAdd = new List<uint>();
        for (uint i = 0; i < pointsCount; i++)
        {
            indicesToAdd.Add(0);
            indicesToAdd.Add(i - 1);
            indicesToAdd.Add(i);
        }

        return indicesToAdd.ToArray();
    }

    private uint[] CalculateIndicesForTaskSubtaskTwo(ShapeUtils.VerticesData verticesData)
    {
        var pointsCount = verticesData.Vertices.Length / 3;

        var indicesToAdd = new List<uint>();
        for (uint i = 0; i < pointsCount; i++)
        {
            for (uint j = 0; j < pointsCount; j++)
            {
                if (j != i && FindDifference(pointsCount, (int)i, (int)j) > 1)
                {
                    indicesToAdd.Add(i);
                    indicesToAdd.Add(j);
                }
            }
        }

        int FindDifference(int count, int index1, int index2)
        {
            int lastIndex = count - 1;
            
            // from left
            int startIndex = index1;
            int distanceFromLeft = 0;
            while (startIndex != index2)
            {
                distanceFromLeft++;
                startIndex--;
                if (startIndex == -1)
                {
                    startIndex = lastIndex;
                }
            }
            
            //from right
            startIndex = index1;
            int distanceFromRight = 0;
            while (startIndex != index2)
            {
                distanceFromRight++;
                startIndex++;
                if (startIndex == lastIndex + 1)
                {
                    startIndex = 0;
                }
            }

            return Math.Min(distanceFromRight, distanceFromLeft);
        }

    return indicesToAdd.ToArray();
    }
    
    private uint[] CalculateIndicesForTaskSubtaskThree(ShapeUtils.VerticesData verticesData)
    {
        var pointsCount = verticesData.Vertices.Length / 3;

        var indicesToAdd = new List<uint>();
        for (uint i = 0; i < pointsCount; i++)
        {
            indicesToAdd.Add(i);
        }

        return indicesToAdd.ToArray();
    }
    
    private uint[] CalculateIndicesForTaskSubtaskFour(ShapeUtils.VerticesData verticesData)
    {
        var pointsCount = verticesData.Vertices.Length / 3;

        var indicesToAdd = new List<uint>();
        for (uint i = 0; i < pointsCount; i++)
        {
            if (i % 2 == 0)
            {
                indicesToAdd.Add(i);
            }
        }

        return indicesToAdd.ToArray();
    }
}