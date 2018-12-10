using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace Mail_24.ViewModels
{
    public class KontoViewModel
    {
        private Models.KontoModel model;
        //własności:
        public string AdresSerwera
        {
            get
            {
                return model.AdresSerwera;
            }
        }
        public string Port
        {
            get
            {
                return model.Port;
            }
        }
        public string ImieNazwisko
        {
            get
            {
                return model.ImieNazwisko;
            }
        }
        public string AdresEmail
        {
            get
            {
                return model.AdresEmail;
            }
        }
        public string Haslo
        {
            get
            {
                return model.Haslo;
            }
        }
        public KontoViewModel(Models.KontoModel konto)
        {
            model = konto;
        }
        public KontoViewModel(string adresSerwera, string port, string imieNazwisko, string adresEmail, string haslo)
        {
            model = new Models.KontoModel(adresSerwera, port, imieNazwisko, adresEmail, haslo);
        }
        public Models.KontoModel GetModel()
        {
            return model;
        }
    }

    public class Konta : INotifyPropertyChanged
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
        private string adres;
        public string Adres
        {
            get
            {
                return adres;
            }
            set
            {
                adres = value;
                OnPropertyChanged("Adres");
            }

        }
        private string port;
        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                OnPropertyChanged("Port");
            }

        }
        private string imie;
        public string Imie
        {
            get
            {
                return imie;
            }
            set
            {
                imie = value;
                OnPropertyChanged("Imie");
            }

        }
        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }

        }
        private string haslo;
        public string Haslo
        {
            get
            {
                return haslo;
            }
            set
            {
                haslo = value;
                OnPropertyChanged("Haslo");
            }
        }
        private Models.Konta model;
        private const string savePathXml = "konta.xml";
        public static ObservableCollection<KontoViewModel> ListaKont { get; } = new ObservableCollection<KontoViewModel>();

        private void KopiujKonta()
        {
            ListaKont.CollectionChanged -= ModelSynchro;
            ListaKont.Clear();
            foreach (Models.KontoModel konto in model)
                ListaKont.Add(new KontoViewModel(konto));
            ListaKont.CollectionChanged += ModelSynchro;
        }
        private void ModelSynchro(object sender, NotifyCollectionChangedEventArgs e) //wywołuje akcję, wchodzimy przez CopyPersons()
        {
            switch (e.Action)
            {
                //nazwy metod jak w modelu: DelPerson i NewPerson
                //NotifyCollectionChangedAction opisuje akcje które spowodowały CollectionChanged event
                case NotifyCollectionChangedAction.Add: //opisuje akcję która powoduje System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
                    KontoViewModel newTask = (KontoViewModel)e.NewItems[0]; //NewItems otrzymuje listę nowych rzeczy powodujących zmianę.
                    if (newTask != null) //jeśli wciąż są nowe itmey...
                        model.DodajKonto(newTask.GetModel()); //w kolekcji jest tworzone nowe wywołanie modelu
                    break;
                case NotifyCollectionChangedAction.Remove: //j.w. tylko dla usuwania
                    KontoViewModel removeTask = (KontoViewModel)e.OldItems[0];
                    if (removeTask != null)
                        model.UsunKonto(removeTask.GetModel());
                    break;
            }
        }
        public Konta()
        {
            if (System.IO.File.Exists(savePathXml)) //jeśli plik istnieje to...
                model = Models.ZapisXML.WczytajKonta(savePathXml); //wołanie modelem(kolekcją) metody Load w SavingXML
            else model = new Models.Konta(); //w przeciwnym wypadku utwórz nową kolekcję
            KopiujKonta(); //wołanie synchronizacji modelu i NotifyCollectionChangedAction.Add
        }
        private ICommand usunKonto;
        public ICommand UsunKonto
        {
            get
            {
                if (usunKonto == null) //jak delPerson jeszcze nie było
                    usunKonto = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu
                        KontoViewModel zadanie = ListaKont[indeksZadania]; //przypisanie zmiennej zadanie obiekt z kolekcji
                        ListaKont.Remove(zadanie); //z kolekcji usuwamy przypisany zmiennej obiekt
                        Models.ZapisXML.ZapiszKonta(savePathXml, model); //...metodę Save z klasy SavingXML
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        if (o == null) return false; //jeśli nie ma obiektu...
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu (ale po co?)
                        return indeksZadania >= 0; //zwracamy indeksZadania >= 0; (nie może być chyba <0)
                    });
                return usunKonto; //wykonujemy RelayCommand
            }
        }
        private ICommand dodajKonto;
        public ICommand DodajKonto //zapisuje tez konta
        {
            get
            {
                if (dodajKonto == null)
                    dodajKonto = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        var openDialog = new Powiadomienie();
                        KontoViewModel zadanie = o as KontoViewModel;
                        if (zadanie != null)
                        {
                            ListaKont.Add(zadanie);
                            Models.ZapisXML.ZapiszKonta(savePathXml, model); //...metodę Save z klasy SavingXML
                            openDialog.ShowPowiadomienie("Dodano nowe konto.", "Powiadomienie");
                            Adres = "";
                            Port = "";
                            Imie = "";
                            Email = "";
                            Haslo = "";
                        }
                        else
                        {
                            openDialog.ShowPowiadomienie("Źle wpisano dane. Spróbuj ponownie", "Powiadomienie");
                        }

                    });
                return dodajKonto;
            }
        }
    }
}
