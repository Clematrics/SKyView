using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SkyView.Utils {
    public class ColorArrayToImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Image.Image && (value as Image.Image).Size() != 0) {
                Image.Image image = value as Image.Image;
                byte[] bytes = new byte[image.Width * image.Height * 4];
                for (int i = 0; i < image.Width * image.Height; i++) {
                    byte r = image.data[i].R;
                    byte g = image.data[i].G;
                    byte b = image.data[i].B;
                    byte a = image.data[i].A;

                    bytes[4 * i + 0] = b;
                    bytes[4 * i + 1] = g;
                    bytes[4 * i + 2] = r;
                    bytes[4 * i + 3] = a;
                }

                BitmapSource bitmap = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Bgra32, null, bytes, ((image.Width * 32 + 31) & ~31) / 8);

                return bitmap;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
