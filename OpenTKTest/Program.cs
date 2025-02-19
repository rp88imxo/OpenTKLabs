using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Framework.SubprogramRunner;
using OpenTKTest.Framework.Subprograms;

namespace OpenTKTest;

class Program
{
    static void Main(string[] args)
    {
        using var baseWindow = new BaseWindow(800, 600, "Labs RML");

        var programRunner = new BaseSubprogramRunner(new List<IBaseSubprogram>()
        {
            new TaskOneNPolygonWithElementBuffer(baseWindow)
        }, baseWindow);
        
        baseWindow.InitRunner(programRunner);
        baseWindow.Run();
    }
}