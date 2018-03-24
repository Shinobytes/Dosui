using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Shinobytes.Console.Forms.Graphics
{
    public class ConsoleImage : IDisposable
    {
        private bool isDisposed;
        private Color[] spritePixels;

        public ConsoleImage(Color[] imageData, int imageWidth, int imageHeight)
        {
            Width = imageWidth;
            Height = imageHeight;
            spritePixels = imageData;
        }

        public ConsoleImage(Bitmap image)
        {
            Image = image;
            Width = Image.Width;
            Height = Image.Height;
            LoadImageDataUnsafe(image);
            var pixels = GetPixels();
        }

        public Color[] GetPixels(Rectangle? source = null)
        {
            var x = 0;
            var y = 0;
            var w = Width;
            var h = Height;
            if (source != null)
            {
                x = source.Value.X;
                y = source.Value.Y;
                w = source.Value.Width;
                h = source.Value.Height;
            }
            var pixels = new Color[w * h];
            var startX = x;
            var destY = y + h;
            var destX = x + w;
            for (var outY = 0; y < destY; ++y, ++outY)
            {
                for (var outX = 0; x < destX; ++x, ++outX)
                {
                    var srcIndex = outY * w + outX;
                    var dstIndex = y * Width + x;
                    pixels[srcIndex] = spritePixels[dstIndex];
                }
                x = startX;
            }
            return pixels;
        }

        private unsafe void LoadImageDataUnsafe(Bitmap image)
        {
            var bitmapData = Image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                image.PixelFormat);

            int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize(image.PixelFormat) / 8;
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;
            int width = bitmapData.Width;
            byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

            spritePixels = new Color[image.Width * image.Height];

            Parallel.For(0, heightInPixels, y =>
            {
                byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                var idx = y * width;
                var x2 = 0;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    spritePixels[idx + x2++] = Color.FromArgb(
                        currentLine[x + 3],
                        currentLine[x + 2],
                        currentLine[x + 1],
                        currentLine[x + 0]);
                }
            });

            Image.UnlockBits(bitmapData);
        }

        public Bitmap Image { get; set; }

        public int Width { get; }

        public int Height { get; }

        public static ConsoleImage FromFile(string fileName)
        {
            return new ConsoleImage((Bitmap)System.Drawing.Image.FromFile(fileName));
        }

        public void Dispose()
        {
            if (isDisposed) return;
            Image.Dispose();
            isDisposed = true;
        }

        public ColorPalette GetColorPalette()
        {
            if (Image != null) return Image.Palette;

            var pixels = spritePixels.Where(x => x.A > 0).Distinct();

            var colorPalette = FormatterServices.GetUninitializedObject(typeof(ColorPalette));
            var entriesField = typeof(ColorPalette).GetField("entries", BindingFlags.Instance | BindingFlags.NonPublic);
            entriesField.SetValue(colorPalette, pixels.ToArray());
            return (ColorPalette)colorPalette;
        }
    }
}