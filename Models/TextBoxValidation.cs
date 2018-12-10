using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Mail_24.Models
{
    public class TextBoxValidation : ValidationRule
    {
        public Type ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) //zwracanie wyniku walidacji
        {
            string strValue = Convert.ToString(value); //zmienna = dostarczny string
            if (string.IsNullOrEmpty(strValue)) //jeśli null, to
                return new ValidationResult(false, ""); //fałsz
            bool canConvert = false; //domyślnie canConvert = fałsz
            switch (ValidationType.Name) //dla różnych typów walidacji (podawane w widoku)
            {
                case "Boolean": //dla przypadku bool
                    bool boolVal = false; //domyślnie , do końca nie wiem po ch*j
                    canConvert = bool.TryParse(strValue, out boolVal); //spr czy da się konwertować wartość na bool...
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean"); //...jeśli tak i nie
                //kolejne przypadki podobnie j.w.
                case "Int32":
                    int intVal = 0;
                    canConvert = int.TryParse(strValue, out intVal);
                    if (canConvert)
                    {
                        if (int.Parse(strValue) <= 0)
                        {
                            canConvert = false;
                        }
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Proszę wpisać liczbę całkowitą większą od 0");
                case "Double":
                    double doubleVal = 0;
                    canConvert = double.TryParse(strValue, out doubleVal);
                    if (canConvert)
                    {
                        if (double.Parse(strValue) <= 0)
                        {
                            canConvert = false;
                        }
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Proszę wpisać liczbę większą od 0");

                case "String":
                    canConvert = true;
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Proszę wpisać ciąg znaków");
                case "Int64": //dla e-mail
                    if (strValue.Contains('@') && strValue.Contains('.')) canConvert = true;
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Proszę wpisać adres e-mail");
                default:
                    throw new InvalidCastException($"{ValidationType.Name} is not supported");
            }
        }
    }
}
