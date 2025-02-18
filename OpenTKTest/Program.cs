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
            new TaskOnePointsInPolygonSubprogram(new TaskOneSubprogramParams(24), baseWindow),
            new TaskTwoLinesInPolygonSubprogram(new TaskTwoSubprogramParams(24), baseWindow),
            new TaskThreeFigureWithLinesSubprogram(baseWindow),
            new TaskFourFigureWithLineLoopSubprogram(baseWindow),
            new TaskFiveFigureWithTrianglesSubprogram(baseWindow),
            new TaskSixNPolygonWithTriangleFanSubprogram(new TaskOneSubprogramParams(8), baseWindow),
            new TaskSevenFigureWithTriangles(baseWindow)
        }, baseWindow);
        
        baseWindow.InitRunner(programRunner);
        baseWindow.Run();
    }
}