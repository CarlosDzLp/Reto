using System.Globalization;

namespace Reto.Converts
{
    public class ImageConvert : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            try
            {
                if (value is byte[] file)
                {
                    if(file != null)
                    {
                        var result = ImageSource.FromStream(() => new MemoryStream(file));
                        return result;
                    }
                    else
                        return "camera.png";
                }
                else
                    return "camera.png";
            }
            catch(Exception ex)
            {
                return "camera.png";
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
