using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpenTKTest.Core;

public class Texture : IDisposable
{
    public readonly int Handle;
    private bool disposedValue;

    public Texture(int glHandle)
    {
        Handle = glHandle;
    }


    public static Texture LoadFromFile(string path)
    {
        int handle = GL.GenTexture();


        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);


        using (var image = Image.Load<Rgba32>(path))
        {
            image.Mutate(x => x.Flip(FlipMode.Vertical));


            var pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(pixels);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);


            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                pixels);
        }


        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


        GL.BindTexture(TextureTarget.Texture2D, 0);


        return new Texture(handle);
    }


    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteTexture(Handle);
            disposedValue = true;
        }
    }

    ~Texture()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}