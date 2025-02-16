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

        public BaseWindow(int width, int height, string title, IList<IRenderOperation> renderOperations) : base(GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                ClientSize = (width, height), Title = title
            })
        {
            _renderOperations = renderOperations;
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

            foreach (var renderOperation in _renderOperations)
            {
                renderOperation.Render();
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