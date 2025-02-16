using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest;

class Program
{
    static void Main(string[] args)
    {
        using (var baseWindow = new BaseWindow(800, 600, "OpenGL", new List<IRenderOperation>()
               {
                   new TriangleRenderOperation(TrianglesData.VerticesTriangleRed,
                       new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader")),
                   new TriangleRenderOperation(TrianglesData.VerticesTriangleGreen,
               }))
        {
            baseWindow.Run();
        }
    }
}