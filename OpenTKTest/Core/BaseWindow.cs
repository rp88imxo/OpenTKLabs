using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core.Shaders;

namespace OpenTKTest.Core
{
    public class BaseWindow : GameWindow
    {
        private readonly IList<IRenderOperation> _renderOperations;

        private Stopwatch _timer;
        
        public BaseWindow(int width, int height, string title, IList<IRenderOperation> renderOperations) : base(GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                ClientSize = (width, height), Title = title
            })
        {
            _renderOperations = renderOperations;
            _timer = new Stopwatch();
            _timer.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            foreach (var renderOperation in _renderOperations)
            {
                renderOperation.Init();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            var data = new RenderArgumentsData
            {
                TotalTimePassed = _timer.Elapsed.TotalSeconds
            };
            
            foreach (var renderOperation in _renderOperations)
            {
                renderOperation.Render(data);
            }
            
            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}