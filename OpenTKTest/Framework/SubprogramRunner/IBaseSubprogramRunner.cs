using OpenTK.Windowing.Common;
using OpenTKTest.Core;

namespace OpenTKTest.Framework.SubprogramRunner;

public interface IBaseSubprogramRunner
{
    void Init(BaseWindow baseWindow);
    void Update(FrameEventArgs args);
    void Render(RenderArgumentsData renderArgumentsData);
}