using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core;
using OpenTKTest.Framework.Subprograms;

namespace OpenTKTest.Framework.SubprogramRunner;

public class BaseSubprogramRunner : IBaseSubprogramRunner
{
    private readonly IList<IBaseSubprogram> _baseSubprograms;
    private readonly BaseWindow _baseWindow;
    
    private Queue<IBaseSubprogram> _subprogramsQueue;
    private IBaseSubprogram _activeSubprogram;

    public BaseSubprogramRunner(IList<IBaseSubprogram> baseSubprograms, BaseWindow baseWindow)
    {
        _baseSubprograms = baseSubprograms;
        _baseWindow = baseWindow;
    }

    public virtual void Init(BaseWindow baseWindow)
    {
        _subprogramsQueue = new Queue<IBaseSubprogram>(_baseSubprograms);
        if (_subprogramsQueue.Count > 0)
        {
            _activeSubprogram = _subprogramsQueue.Dequeue();
            _activeSubprogram.Init();
        }
    }

    public void Update(FrameEventArgs args)
    {
        if ( _baseWindow.KeyboardState.IsKeyPressed(Keys.Space))
        {
            if (!TryMoveToNextSubprogram())
            {
                _baseWindow.Close();
            }
        }
        
        _activeSubprogram.Update(args);
    }
    
    public virtual void Render(RenderArgumentsData renderArgumentsData)
    {
        _activeSubprogram.Render(renderArgumentsData);
    }
    
    private bool TryMoveToNextSubprogram()
    {
        if (_subprogramsQueue.Count > 0)
        {
            _activeSubprogram.Dispose();
            _activeSubprogram = _subprogramsQueue.Dequeue();
            _activeSubprogram.Init();
            return true;
        }

        if (_activeSubprogram != null)
        {
            _activeSubprogram.Dispose();
        }
       
        return false;
    }
}