using System.Globalization;

namespace Reto.Converts
{
    public class ImagePathConvert : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value != null)
            {
                string imagePath = value.ToString();
                return ImageSource.FromFile(imagePath);
            }
            else
            {
                return ImageSource.FromFile("image_not_found.png");
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
