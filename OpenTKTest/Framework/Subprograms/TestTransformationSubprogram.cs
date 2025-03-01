using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Utils.Math;

namespace OpenTKTest.Framework.Subprograms;

public class TestTransformationSubprogram : BaseSubprogram
{
    private ShapeUtils.VerticesData _points;
    private DefaultShader _currentShader;
    private RenderOperation _renderOperation;

    public override void Init()
    {
        base.Init();

        _points = ShapeUtils.CreateNPolygon(12);
        _currentShader = new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader");
        _renderOperation = new RenderOperation(_points.Vertices, _points.Colors, _currentShader,
            new RenderOperationData(PrimitiveType.TriangleFan));
        _renderOperation.Init();
    }
    
    public override void Render(RenderArgumentsData renderArgumentsData)
    {
        base.Render(renderArgumentsData);
        
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        var currentRotation = (float)renderArgumentsData.TotalTimePassed * 25;
        var scaleValue = ((MathF.Sin((float)renderArgumentsData.TotalTimePassed * 2) + 1) / 2);
        
        Matrix4 rotationMatrix =
            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation));
        Matrix4 scale = Matrix4.CreateScale(scaleValue, scaleValue, scaleValue);
        Matrix4 trans = rotationMatrix * scale;
        
        _currentShader.Use();
        var transformLocation = GL.GetUniformLocation(_currentShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref trans);
        
        _renderOperation.Render(renderArgumentsData);
    }
}