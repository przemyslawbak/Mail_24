using System.ComponentModel;
using System.Windows.Input;

namespace Mail_24.ViewModels
{
    public class DodajAdresViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] nazwyWłasności) //metoda pomocnicza
                                                                       //aktualizowanie kontrolek powiązanych
                                                                       //z własnościami tej klasy.
                                                                       //string[] nazwyWłasności-tablica nazw własności
                                                                       //dzięki "params" nie tworzymy tablicy
        {
            if (PropertyChanged != null) //jeśli zmiana własności...
            {
                foreach (string nazwaWłasności in nazwyWłasności) //dla każdej nazwy własności z nazwyWłasności
                    PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
            }
        }
        private string dodawanyAdres;
        public string DodawanyAdres
        {
            get
            {
                return dodawanyAdres;
            }
            set
            {
                dodawanyAdres = value;
                OnPropertyChanged("DodawanyAdres");
            }
        }
        public DodajAdresViewModel()
        {

        }
        private ICommand dodawanieAdresu;
        public ICommand DodawanieAdresu
        {
            get
            {
                if (dodawanieAdresu == null)
                    dodawanieAdresu = new RelayCommand(
                        o =>
                        {
                            ViewModels.Adres.ListaAdresow.Add(new ViewModels.AdresViewModel(DodawanyAdres, true, false, true));

                            var openDialog = new Powiadomienie();
                            openDialog.ShowPowiadomienie("Dodano nowy adres", "Powiadomienie");
                            DodawanyAdres = "";
                        });
                return dodawanieAdresu;
            }
        }
    }
}
