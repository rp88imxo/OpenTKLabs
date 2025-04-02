using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp; // Используем ImageSharp
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

    // Загрузка текстуры из файла
    public static Texture LoadFromFile(string path)
    {
        // Генерируем хэндл текстуры
        int handle = GL.GenTexture();

        // Биндим текстуру как 2D текстуру
        GL.ActiveTexture(TextureUnit.Texture0); // Активируем текстурный юнит (0 по умолчанию)
        GL.BindTexture(TextureTarget.Texture2D, handle);

        // Загружаем изображение с помощью ImageSharp
        // ImageSharp автоматически определяет формат
        // .Load<Rgba32>() загружает изображение в формате RGBA (по 8 бит на канал)
        // Using для автоматического освобождения ресурсов ImageSharp
        using (var image = Image.Load<Rgba32>(path))
        {
            // ImageSharp загружает изображения "вверх ногами" относительно OpenGL
            // поэтому переворачиваем его
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            // Получаем массив байтов пикселей.
            // Важно: используем GetPixelRowSpan для эффективного доступа
            var pixels = new byte[4 * image.Width * image.Height]; // 4 байта на пиксель (RGBA)
            image.CopyPixelDataTo(pixels);

            // Устанавливаем параметры текстуры
            // Уменьшение (Minification) - Простая (Nearest Neighbor) - начальное значение
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            // Увеличение (Magnification) - Билинейная
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Повторение текстуры (Wrapping)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Загружаем данные текстуры в OpenGL
            GL.TexImage2D(TextureTarget.Texture2D,
                          0, // Уровень Mipmap
                          PixelInternalFormat.Rgba, // Формат хранения в OpenGL
                          image.Width,
                          image.Height,
                          0, // Граница (должна быть 0)
                          PixelFormat.Rgba, // Формат исходных данных
                          PixelType.UnsignedByte, // Тип данных исходных данных
                          pixels); // Данные пикселей
        }

        // Генерируем Mipmap'ы - важно для качественного уменьшения и некоторых режимов фильтрации
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        // Отвязываем текстуру
        GL.BindTexture(TextureTarget.Texture2D, 0);

        // Возвращаем созданный объект Texture
        return new Texture(handle);
    }

    // Биндит текстуру к активному текстурному юниту
    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    // Освобождение ресурсов
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