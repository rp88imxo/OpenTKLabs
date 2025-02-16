using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest;

class Program
{
    static void Main(string[] args)
    {
        float[] vertices =
        {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f, 0.5f, 0.0f //Top vertex
        };
        
        float[] vertices2 =
        {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };
        
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        using (var baseWindow = new BaseWindow(800, 600, "OpenGL", new List<IRenderOperation>()
               {
                   new TriangleRenderOperation(vertices,
                       new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader")),
                   new ElementBufferRenderOperation(vertices2,indices,
                       new DefaultShader("Shaders/vertex.shader", "Shaders/fragment.shader")),
               }))
        {
            baseWindow.Run();
        }
    }
}