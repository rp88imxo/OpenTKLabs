using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TaskThreeFigureWithAffineTransforms : BaseSubprogram
{
    private readonly BaseWindow _baseWindow;
    private readonly DefaultShader _currentShader;
    private readonly List<IRenderOperation> _renderOperations;

    private float _globalOffset = 5f;
    
    public TaskThreeFigureWithAffineTransforms(BaseWindow baseWindow)
    {
        _baseWindow = baseWindow;
        _renderOperations = new List<IRenderOperation>();

        var line = ShapeUtils.CreatePoints(new List<Vector2>()
        {
            new Vector2(1920f / 2f - 100f, 1080 / 2f),
            new Vector2(1920f / 2f + 100f, 1080 / 2f),

        }, ShapeUtils.RandomColorPerTriangleCallback);

        _currentShader = new DefaultShader("Shaders/vertex_transform.shader", "Shaders/fragment.shader");

        var renderOperationTopLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderTopLine));
        
        var renderOperationBottomLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderBottomLine));
        
        var renderOperationLeftLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderLeftLine));
        
        var renderOperationRightLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderRightLine));
        
        var renderOperationHorizontalRightTopLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderHorizontalRightTopLine));
        
        var renderOperationHorizontalRightBottomLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderHorizontalRightBottomLine));
        
        var renderOperationHorizontalLeftTopLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderHorizontalLeftTopLine));
        
        var renderOperationHorizontalLeftBottomLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderHorizontalLeftBottomLine));
        
        var renderOperationVerticalRightTopLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderVerticalRightTopLine));
        
        var renderOperationVerticalRightBottomLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderVerticalRightBottomLine));
        
        var renderOperationVerticalLeftTopLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderVerticalLeftTopLine));
        
        var renderOperationVerticalLeftBottomLine = new ElementBufferRenderOperation(line.Vertices, line.Colors, new uint[] { 0, 1 },
            _currentShader, new RenderOperationData(PrimitiveType.Lines, OnPreRenderVerticalLeftBottomLine));

        _renderOperations.Add(renderOperationTopLine);
        _renderOperations.Add(renderOperationBottomLine);
        _renderOperations.Add(renderOperationLeftLine);
        _renderOperations.Add(renderOperationRightLine);
        _renderOperations.Add(renderOperationHorizontalRightTopLine);
        _renderOperations.Add(renderOperationHorizontalRightBottomLine);
        _renderOperations.Add(renderOperationHorizontalLeftTopLine);
        _renderOperations.Add(renderOperationHorizontalLeftBottomLine);
        _renderOperations.Add(renderOperationVerticalRightTopLine);
        _renderOperations.Add(renderOperationVerticalRightBottomLine);
        _renderOperations.Add(renderOperationVerticalLeftTopLine);
        _renderOperations.Add(renderOperationVerticalLeftBottomLine);
    }

    private void OnPreRenderVerticalLeftBottomLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(-100f, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(-200f - _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderVerticalLeftTopLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(-100f, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(200f + _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderVerticalRightBottomLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(100f, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(-200f - _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderVerticalRightTopLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(100f, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(200f + _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderHorizontalLeftBottomLine(RenderArgumentsData obj)
    {
        //var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(-200f - _globalOffset, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(-100f - _globalOffset, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderHorizontalLeftTopLine(RenderArgumentsData obj)
    {
        //var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(-200f - _globalOffset, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(100f + _globalOffset, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderHorizontalRightBottomLine(RenderArgumentsData obj)
    {
        var transX = MathUtils.Remap(200f + _globalOffset, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(-100f - _globalOffset, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderHorizontalRightTopLine(RenderArgumentsData obj)
    {
        //var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(200f + _globalOffset, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(100f + _globalOffset, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderRightLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(300f + _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(0f, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderLeftLine(RenderArgumentsData obj)
    {
        var rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
        
        var transX = MathUtils.Remap(-300f - _globalOffset * 2, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(0f, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = rotationMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderBottomLine(RenderArgumentsData obj)
    {
        var scaleMatrix = Matrix4.CreateScale(0.95f, 1f, 1f);
        
        var transX = MathUtils.Remap(0, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(-300f - 15f, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = scaleMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }

    private void OnPreRenderTopLine(RenderArgumentsData obj)
    {
        var scaleMatrix = Matrix4.CreateScale(0.95f, 1f, 1f);
        
        var transX = MathUtils.Remap(0, -1000f, 1000f, 1f, -1f);
        var transY = MathUtils.Remap(300f + 15f, -1000f, 1000f, 1f, -1f);
        
        var translationDefault = Matrix4.CreateTranslation(transX, transY, 0.0f);

        Matrix4 trs = scaleMatrix * translationDefault;

        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trs);
    }
    
    public override void Init()
    {
        base.Init();
        _baseWindow.Title = "RML Labs | Task 3";
        foreach (var renderOperation in _renderOperations)
        {
            renderOperation.Init();
        }
    }

    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        GL.LineWidth(5);
        GL.Enable(EnableCap.LineSmooth);
        
        foreach (var renderOperation in _renderOperations)
        {
            renderOperation.Render(renderArgumentsData);
        }
    }
}