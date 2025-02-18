using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Core.Shaders;
using OpenTKTest.Framework.SubprogramRunner;

namespace OpenTKTest.Core
{
    public class BaseWindow : GameWindow
    {
        private IBaseSubprogramRunner _baseSubprogramRunner;
        
        private Stopwatch _timer;
        
        public BaseWindow(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                ClientSize = (width, height), Title = title
            })
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        public void InitRunner(IBaseSubprogramRunner baseSubprogramRunner)
        {
            _baseSubprogramRunner = baseSubprogramRunner;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            
            _baseSubprogramRunner.Update(args);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            _baseSubprogramRunner.Init(this);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            var data = new RenderArgumentsData
            {
                TotalTimePassed = _timer.Elapsed.TotalSeconds
            };

            #region RENDER_RUNNER

            _baseSubprogramRunner.Render(data);

            #endregion

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}