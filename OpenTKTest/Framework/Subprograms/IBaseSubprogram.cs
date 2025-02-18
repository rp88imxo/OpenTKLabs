using OpenTKTest.Core;

namespace OpenTKTest.Framework.Subprograms;

public interface IBaseSubprogram
{
    void Init();
    void Dispose();
    void Render(RenderArgumentsData renderArgumentsData);
}