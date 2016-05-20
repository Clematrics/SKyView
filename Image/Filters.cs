using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SkyView.Image {

    [Serializable]
    public delegate Image Filter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters);

    public static class Filters {

        public static Image NoFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            return new Image(width, height);
        }

        public static Image LoadImage( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            return new Image(parameters[0].Value);
        }

        public static Image AddFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
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

        public static Image SubstractFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
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

        public static Image MultiplyFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
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

        public static Image DivideFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
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

        public static Image InvertFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    int A = 255 - colorA.A;
                    int R = 255 - colorA.R;
                    int G = 255 - colorA.G;
                    int B = 255 - colorA.B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image ConstantFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            int A, R, G, B;
            try {
                R = int.Parse(parameters[0].Value);
                G = int.Parse(parameters[1].Value);
                B = int.Parse(parameters[2].Value);
                A = int.Parse(parameters[3].Value);
            }
            catch (Exception e) {
                throw e;
            }
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
            return finalImage;
        }

        public static Image LinearRampFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            int x1, y1, x0, y0;
            try {
                x1 = int.Parse(parameters[0].Value);
                y1 = int.Parse(parameters[1].Value);
                x0 = int.Parse(parameters[2].Value);
                y0 = int.Parse(parameters[3].Value);
            }
            catch (Exception e) {
                throw e;
            }
            double norm = Math.Sqrt(Math.Pow((x0 - x1), 2) + Math.Pow((y0 - y1), 2));
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {

                    double scalaire = x*(x0 - x1) + y*(y0 - y1);
                    int color = (int)(255 - (255 * scalaire / (norm * norm)) );
                    color = (color > 255) ? 255 : color;
                    color = (color < 0) ? 0 : color;

                    finalImage.data[y * width + x] = Color.FromArgb(color, color, color, color);
                }
            return finalImage;
        }

        public static Image RadialRampFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            int x1, y1, x0, y0;
            try {
                x1 = int.Parse(parameters[0].Value);
                y1 = int.Parse(parameters[1].Value);
                x0 = int.Parse(parameters[2].Value);
                y0 = int.Parse(parameters[3].Value); 
            }
            catch (Exception e) {
                throw e;
            }
            double norm = Math.Sqrt( Math.Pow((x0 - x1), 2) + Math.Pow((y0 - y1), 2));
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {

                    double normPixel = Math.Sqrt(Math.Pow((x - x1), 2) + Math.Pow((y - y1), 2));
                    int color = (int)( 255 - (255*normPixel/norm) );
                    color = (color < 0) ? 0 : color;

                    finalImage.data[y * width + x] = Color.FromArgb(color, color, color, color);
                }
            return finalImage;
        }

        public static Image GetAlphaChannel( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.A, colorA.A, colorA.A, colorA.A);
                }
            return finalImage;
        }

        public static Image GetRedChannel( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.R, colorA.R, colorA.R, colorA.R);
                }
            return finalImage;
        }

        public static Image GetGreenChannel( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.G, colorA.G, colorA.G, colorA.G);
                }
            return finalImage;
        }

        public static Image GetBlueChannel( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 1);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.B, colorA.B, colorA.B, colorA.B);
                }
            return finalImage;
        }

        public static Image CombineChannels( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);
                    Color colorR = inputImages[0].Getcolor(x, y, 0);
                    Color colorG = inputImages[0].Getcolor(x, y, 0);
                    Color colorB = inputImages[0].Getcolor(x, y, 0);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.A, colorR.R, colorG.G, colorB.B);
                }
            return finalImage;
        }

        public static Image GrayScaleFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);
                    int grey = (int)(.299 * colorA.R + .587 * colorA.G + .114 * colorA.B);

                    finalImage.data[y * width + x] = Color.FromArgb(colorA.A, grey, grey, grey);
                }
            return finalImage;
        }

        public static Image LuminosityFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);

            int brightness;
            try {
                brightness = int.Parse(parameters[0].Value);
            }
            catch (Exception e) {
                throw e;
            }
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);

                    int A = (colorA.A + brightness > 255) ? 255 : colorA.A + brightness;
                    int R = (colorA.R + brightness > 255) ? 255 : colorA.R + brightness;
                    int G = (colorA.G + brightness > 255) ? 255 : colorA.G + brightness;
                    int B = (colorA.B + brightness > 255) ? 255 : colorA.B + brightness;

                    A = (A < 0) ? 0 : A;
                    R = (R < 0) ? 0 : R;
                    G = (G < 0) ? 0 : G;
                    B = (B < 0) ? 0 : B;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image ThresholdFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);

            int threshold;
            try {
                threshold = int.Parse(parameters[0].Value);
            }
            catch (Exception e) {
                throw e;
            }
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);

                    int A = (colorA.A > threshold) ? 255 : 0;
                    int R = (colorA.R > threshold) ? 255 : 0;
                    int G = (colorA.G > threshold) ? 255 : 0;
                    int B = (colorA.B > threshold) ? 255 : 0;

                    finalImage.data[y * width + x] = Color.FromArgb(A, R, G, B);
                }
            return finalImage;
        }

        public static Image ColorSelectionFilter( int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color colorA = inputImages[0].Getcolor(x, y, 0);
                    Color colorB = inputImages[1].Getcolor(x, y, 0);

                    int A = (colorA.A - colorB.A < 0) ? 255 : colorA.A - colorB.A;
                    int R = (colorA.R - colorB.R < 0) ? 255 : colorA.R - colorB.R;
                    int G = (colorA.G - colorB.G < 0) ? 255 : colorA.G - colorB.G;
                    int B = (colorA.B - colorB.B < 0) ? 255 : colorA.B - colorB.B;

                    int dist = (int)Math.Sqrt(A * A + R * R + G * G + B * B);

                    finalImage.data[y * width + x] = Color.FromArgb(255 - dist / 2, 255 - dist / 2, 255 - dist / 2, 255 - dist / 2);
                }
            return finalImage;
        }

        public static Image SetAlpha(int width, int height, List<Image> inputImages, Collection<NodeProperty> parameters) {
            Image finalImage = new Image(width, height);
            int Alpha;
            try {
                Alpha = int.Parse(parameters[0].Value);
            }
            catch (Exception e) {
                throw e;
            }
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    Color color = inputImages[0].Getcolor(x, y, 0);
                    Color newColor = Color.FromArgb(Alpha, color.R, color.G, color.B);
                    finalImage.data[y * width + x] = newColor;
                }
            return finalImage;
        }
    }
}
