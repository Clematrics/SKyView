using System;
using System.Drawing;

namespace SkyView.Image {

    public class Image {
        int width;
        int height;
        public Color[] data;

        public Image(int height, int width) {
            this.height = height;
            this.width = width;
        }
        public Image(string path) {
            Bitmap image;
            try {
                image = new Bitmap(path);
            }
            catch (Exception e) {
                throw e;
            }

            height = image.Height;
            width = image.Width;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    data[i * width + j] = image.GetPixel(j, i);
        }

        public Color Getcolor(int x, int y, int methode) {
            if (x < 0 || y < 0 || x > width || y > height)
                switch (methode) {
                    case 2:
                        break;
                    case 1:
                        return Color.FromArgb(255, 255, 255, 255);

                    case 0:
                    default:
                        return Color.FromArgb(0, 0, 0, 0);
                }
            if (x > width)
                x = width - 1;
            if (y > height)
                y = height - 1;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            return data[y * width + x];
        }
    }
}
