namespace Reto.Converts
{
    public class TextToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string text)
            {
                switch (text)
                {
                    case "Pendiente":
                        return Color.Parse("#ff0000");//
                    case "Rechazado":
                        return Color.Parse("#a18262");//
                    case "Aceptado":
                        return Color.Parse("#00913f");//
                    case "Enviado":
                        return Color.Parse("#0000ff");//
                    case "Recibido":
                        return Color.Parse("#FFA500");
                    default:
                        return Color.Parse("#FFFFFF");
                }
            }
            return Color.Parse("#FFFFFF");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null; 
        }
    }
}
