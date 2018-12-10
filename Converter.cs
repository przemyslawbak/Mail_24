using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Mail_24
{
    public class KontaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Regex regex = new Regex("[^a-zA-Z]+");

            string adresSerwera = (string)values[0];
            string port = (string)values[1];
            string imieNazwisko = (string)values[2];
            string adresEmail = (string)values[3];
            string haslo = (string)values[4];

            if ((new[] { adresSerwera, port, imieNazwisko, adresEmail, haslo }).All(c => !string.IsNullOrWhiteSpace(c))
            && (new[] { "@", "." }).Contains(adresEmail) && regex.IsMatch(port))
            {
                return new ViewModels.KontoViewModel(adresSerwera, port, imieNazwisko, adresEmail, haslo);
            }
            else return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AktywnyToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return bvalue ? "Do wysłania" : "Nieaktywny";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AktywnyKolor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return bvalue ? Brushes.Green : Brushes.LightGray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BooleanStart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.StatusWysylania sendingStatus = (Models.StatusWysylania)value;
            if (sendingStatus == Models.StatusWysylania.Wysylanie || sendingStatus == Models.StatusWysylania.Oczekiwanie)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BooleanStop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.StatusWysylania sendingStatus = (Models.StatusWysylania)value;
            if (sendingStatus == Models.StatusWysylania.Wysylanie || sendingStatus == Models.StatusWysylania.Oczekiwanie)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToString : IValueConverter //text decoration
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.StatusWysylania sendingStatus = (Models.StatusWysylania)value;
            if (sendingStatus == Models.StatusWysylania.Oczekiwanie)
            {
                return "Oczekiwanie";
            }
            if (sendingStatus == Models.StatusWysylania.Wstrzymanie)
            {
                return "Wysyłanie wiadomości zatrzymane";
            }
            else
            {
                return "Wysyłanie";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToBrush : IValueConverter //text decoration
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.StatusWysylania sendingStatus = (Models.StatusWysylania)value;
            if (sendingStatus == Models.StatusWysylania.Oczekiwanie)
            {
                return Brushes.Green;
            }
            if (sendingStatus == Models.StatusWysylania.Wstrzymanie)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.Green;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolButon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.StatusWysylania sendingStatus = (Models.StatusWysylania)value;
            if (sendingStatus == Models.StatusWysylania.Wysylanie || sendingStatus == Models.StatusWysylania.Oczekiwanie)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
