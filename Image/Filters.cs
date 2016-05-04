using SkyView.Nodes;
using System;
using System.Drawing;

namespace SkyView.Image {

    public delegate Image Filter(int height, int width, Image[] inputImages, NodeProperty[] parameters);

    public static class Filters {

        public static Image LoadImage(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            return new Image(parameters[0].value);
        }

        public static Image AddFilter(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);
                    Color colorB = inputImages[1].Getcolor(x, y, 0);

                    int A = (colorA.A + colorB.A > 255) ? 255 : colorA.A + colorB.A;
                    int R = (colorA.R + colorB.R > 255) ? 255 : colorA.R + colorB.R;
                    int G = (colorA.G + colorB.G > 255) ? 255 : colorA.G + colorB.G;
                    int B = (colorA.B + colorB.B > 255) ? 255 : colorA.B + colorB.B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B) ;
                }
            return finalImage;
        }

        public static Image SubstractFilter(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);
                    Color colorB = inputImages[1].Getcolor(x, y, 0);

                    int A = (colorA.A - colorB.A < 0) ? 255 : colorA.A - colorB.A;
                    int R = (colorA.R - colorB.R < 0) ? 255 : colorA.R - colorB.R;
                    int G = (colorA.G - colorB.G < 0) ? 255 : colorA.G - colorB.G;
                    int B = (colorA.B - colorB.B < 0) ? 255 : colorA.B - colorB.B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image MultiplyFilter(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);
                    Color colorB = inputImages[1].Getcolor(x, y, 1);

                    int A = colorA.A / 255 * colorB.A;
                    int R = colorA.R / 255 * colorB.R;
                    int G = colorA.G / 255 * colorB.G;
                    int B = colorA.B / 255 * colorB.B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image DivideFilter(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);
                    Color colorB = inputImages[1].Getcolor(x, y, 1);

                    int A = (colorB.A == 0) ? 255 : colorA.A / colorB.A;
                    int R = (colorB.R == 0) ? 255 : colorA.R / colorB.R;
                    int G = (colorB.G == 0) ? 255 : colorA.G / colorB.G;
                    int B = (colorB.B == 0) ? 255 : colorA.B / colorB.B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image ConstantFilter(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            int A, R, G, B;
            try {
                A = int.Parse(parameters[0].value);
                R = int.Parse(parameters[1].value);
                G = int.Parse(parameters[2].value);
                B = int.Parse(parameters[3].value);
            }
            catch (Exception e) {
                throw e;
            }
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
            return finalImage;
        }

        public static Image GetAlphaChannel(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.A, 0, 0, 0);
                }
            return finalImage;
        }
        public static Image GetRedChannel(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(0, colorA.R, 0, 0);
                }
            return finalImage;
        }
        public static Image GetGreenChannel(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(0, 0, colorA.G, 0);
                }
            return finalImage;
        }
        public static Image GetBlueChannel(int height, int width, Image[] inputImages, NodeProperty[] parameters) {
            Image finalImage = new Image(height, width);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(0, 0, 0, colorA.B);
                }
            return finalImage;
        }

    }
}
