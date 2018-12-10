using System.Collections.Generic;
using System.Text;
using System.Windows;
using Mail_24.ViewModels;
using System.IO;

namespace Mail_24
{
    class WindowDialog : FrameworkElement
    {
        public void ShowWindow(object viewModel, double height, double width, string caption)
        {
            var win = new Window();
            win.Content = viewModel;
            win.Width = width;
            win.Height = height;
            win.Title = caption;
            win.ShowDialog();
        }
    }
    public class DodajAdresFileDialog
    {
        protected Microsoft.Win32.FileDialog fileDialog = null;
        public void PokazOpenDialog(string fileFilter)
        {
            fileDialog = new Microsoft.Win32.OpenFileDialog(); //utworzenie instancji klasy OpenFileDialog
            fileDialog.Filter = fileFilter;
            if (fileDialog.ShowDialog() == true)
            {
                string tekst = "";
                FileStream fileStream = new FileStream(fileDialog.FileName,
                                               FileMode.Open);
                StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
                while ((tekst = streamReader.ReadLine()) != null)
                {
                    ViewModels.Adres.ListaAdresow.Add(new AdresViewModel(tekst, true, false, true));
                }
                fileStream.Close();
            }
        }
    }
    public class DodajZalacznikFileDialog
    {
        protected Microsoft.Win32.FileDialog fileDialog = null;
        public void ZalacznikOpenDialog()
        {
            fileDialog = new Microsoft.Win32.OpenFileDialog(); //utworzenie instancji klasy OpenFileDialog
            if (fileDialog.ShowDialog() == true)
            {
                string nazwa = "";
                string adres = "";

                FileStream fileStream = new FileStream(fileDialog.FileName,
                                               FileMode.Open);
                StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
                nazwa = fileDialog.SafeFileName; ///do zmiany
                adres = fileDialog.FileName;

                ViewModels.MainViewModel.ListaZalacznikow.Add(new ZalacznikViewModel(nazwa, adres));
                fileStream.Close();
            }
        }
    }
    public class EksportAdresFileDialog
    {
        protected Microsoft.Win32.FileDialog fileDialog = null;
        public void PokazSaveDialog(string fileFilter)
        {
            fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.Filter = fileFilter;
            if (fileDialog.ShowDialog() == true)
            {
                List<string> adresyMailowe = new List<string>();
                foreach (var adres in ViewModels.Adres.ListaAdresow)
                {
                    adresyMailowe.Add((adres.AdresMailowy).ToString());
                    System.IO.File.WriteAllLines(fileDialog.FileName, adresyMailowe.ToArray());
                }
            }
        }
    }
    public class Powiadomienie
    {
        public void ShowPowiadomienie(string wiadomosc, string caption)
        {
            MessageBox.Show(wiadomosc, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}