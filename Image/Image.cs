using System;
using System.Drawing;

namespace SkyView.Image {

    public class Image {
        public int Width;
        public int Height;
        public Color[] data;

        public Image(int width, int height) {
            Width = width;
            Height = height;
            data = new Color[Width * Height];
        }
        public Image(string path) {
            Bitmap image;
            try {
                image = new Bitmap(path);
            }
            catch (Exception e) {
                throw e;
            }

            Width = image.Width;
            Height = image.Height;
            data = new Color[Width * Height];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    data[i * Width + j] = image.GetPixel(j, i);
        }

        public Color Getcolor(int x, int y, int methode) {
            if (x < 0 || y < 0 || x > Width || y > Height)
                switch (methode) {
                    case 2:
                        break;
                    case 1:
                        return Color.FromArgb(255, 255, 255, 255);

                    case 0:
                    default:
                        return Color.FromArgb(0, 0, 0, 0);
                }
            if (x > Width)
                x = Width - 1;
            if (y > Height)
                y = Height - 1;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            return data[y * Width + x];
        }

        public int Size() {
            return Width * Height;
        }
    }
}
