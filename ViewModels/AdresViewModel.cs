using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Mail_24.ViewModels
{
    public class AdresViewModel : INotifyPropertyChanged
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
        private Models.AdresModel model;
        public string AdresMailowy
        {
            get
            {
                return model.AdresMailowy;
            }
        }
        public bool AktywnyEmail
        {
            get
            {
                return model.AktywnyEmail;
            }
            set
            {
                model.AktywnyEmail = value;
                OnPropertyChanged("AktywnyEmail");
            }
        }
        public event EventHandler InformacjaCheck;
        public bool ZaznaczenieEmail
        {
            get
            {
                return model.ZaznaczenieEmail;
            }
            set
            {
                model.ZaznaczenieEmail = value;
                if (InformacjaCheck != null)
                {
                    InformacjaCheck(this, new EventArgs());
                }
                OnPropertyChanged("ZaznaczenieEmail");
            }
        }
        public bool WyswietlenieEmail
        {
            get
            {
                return model.WyswietlenieEmail;
            }
            set
            {
                model.WyswietlenieEmail = value;
                OnPropertyChanged("ZaznaczenieEmail");
            }
        }
        public AdresViewModel(Models.AdresModel adres)
        {
            model = adres;
        }
        public AdresViewModel(string adresEmail, bool aktywnyEmail, bool zaznaczenieEmail, bool wyswietlenieEmail)
        {
            model = new Models.AdresModel(adresEmail, aktywnyEmail, zaznaczenieEmail, wyswietlenieEmail);
        }

        public Models.AdresModel GetModel()
        {
            return model;
        }
    }
    public class Adres : INotifyPropertyChanged
    {
        public ObservableCollection<AdresViewModel> Adresy //instancja kolekcji, która zwraca ListeAdresow
        {
            get { return ListaAdresow; }
        }
        private void Odpalamy(AdresViewModel adres) // EventHandler
        {
            adres.InformacjaCheck += JeszczeRaz;
        }
        private void JeszczeRaz(object sender, EventArgs e) //metoda EventHandler
        {
            if (Adresy.Count(i => i.ZaznaczenieEmail) != 0)
            {
                IsLaunchEnabled = true;
            }
            else
            {
                IsLaunchEnabled = false;
            }
        }
        private bool m_IsLaunchEnabled;
        public bool IsLaunchEnabled //wiązanie dla przycisków Usuń i Status
        {
            get
            {
                return m_IsLaunchEnabled;
            }
            set
            {
                m_IsLaunchEnabled = value;
                OnPropertyChanged("IsLaunchEnabled");
            }
        }
        //dodane przed tym
        public Adres()
        {
            WczytajWszystko();
            KopiujAdresy(); //wołanie synchronizacji modelu i NotifyCollectionChangedAction.Add
            FiltrowanieWidoku();
            CheckWydarzenie();

        }
        public void CheckWydarzenie()
        {
            foreach (var i in Adresy)
            {
                Odpalamy(i); //też odpalamy w kilku komendach dla EventHandler
            }
        }
        public void FiltrowanieWidoku()
        {
            FiltrowanaKolekcja = new CollectionViewSource();
            FiltrowanaKolekcja.Source = ListaAdresow;
        }
        public void WczytajWszystko()
        {
            if (System.IO.File.Exists(saveAdresPathXml)) //jeśli plik istnieje to...
                model = Models.ZapisXML.WczytajAdresy(saveAdresPathXml); //wołanie modelem(kolekcją) metody Load w SavingXML
            else model = new Models.AdresyEmail(); //w przeciwnym wypadku utwórz nową kolekcję
        }
        public void ZapiszAdresy() //dla wywołania z innego VM
        {
            Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
        }
        public void KopiujAdresy()
        {
            ListaAdresow.CollectionChanged -= ModelSynchro;
            ListaAdresow.Clear();
            foreach (Models.AdresModel adres in model)
                ListaAdresow.Add(new AdresViewModel(adres));
            ListaAdresow.CollectionChanged += ModelSynchro;
        }
        private void ModelSynchro(object sender, NotifyCollectionChangedEventArgs e) //wywołuje akcję, wchodzimy przez CopyPersons()
        {
            switch (e.Action)
            {
                //nazwy metod jak w modelu: DelPerson i NewPerson
                //NotifyCollectionChangedAction opisuje akcje które spowodowały CollectionChanged event
                case NotifyCollectionChangedAction.Add: //Opisuje akcję która powoduje System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
                    AdresViewModel newTask = (AdresViewModel)e.NewItems[0]; //NewItems otrzymuje listę nowych rzeczy powodujących zmianę.
                    if (newTask != null) //jeśli wciąż są nowe itmey...
                        model.DodajAdres(newTask.GetModel()); //w kolekcji jest tworzone nowe wywołanie modelu
                    break;
                case NotifyCollectionChangedAction.Remove: //j.w. tylko dla usuwania
                    AdresViewModel removeTask = (AdresViewModel)e.OldItems[0];
                    if (removeTask != null)
                        model.UsunAdres(removeTask.GetModel());
                    break;
            }
        }
        private bool caic; //prop dla checkbox "zaznacz wszystko"
        public bool CheckAllIsChecked
        {
            get
            {
                return caic;
            }
            set
            {
                caic = value;
                OnPropertyChanged("CheckAllIsChecked");
            }
        }
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
        private const string saveAdresPathXml = "adresy.xml";
        private Models.AdresyEmail model;
        public static ObservableCollection<AdresViewModel> ListaAdresow { get; set; } = new ObservableCollection<AdresViewModel>();
        public CollectionViewSource FiltrowanaKolekcja { get; set; } //kolekcja wyszukiwania/filtru
        private string szukanie = ""; //zm
        public string FiltrSzukania //własność
        {
            get
            {
                return szukanie;
            }

            set
            {
                szukanie = value;
                if (!string.IsNullOrEmpty(FiltrSzukania)) AddFilter();
                FiltrowanaKolekcja.View.Refresh();

            }
        }
        private void AddFilter() //odpalanie metody
        {
            FiltrowanaKolekcja.Filter -= new FilterEventHandler(Filter);
            FiltrowanaKolekcja.Filter += new FilterEventHandler(Filter);

        }
        private void Filter(object seder, FilterEventArgs e) //metoda
        {
            var src = e.Item as ViewModels.AdresViewModel;
            if (src != null)
                src.WyswietlenieEmail = true; //dodaje w źródle wyswietlenie
            if (src == null)
                e.Accepted = false;
            else if (src.AdresMailowy != null && !src.AdresMailowy.Contains(FiltrSzukania))
            {
                src.WyswietlenieEmail = false; //usuwa w źródle wyswietlenie
                e.Accepted = false; //usuwa w widoku wyswietlenie
            }

        }
        //koniec filtrowania
        public string fileFilter { get; set; } //własność filtra open/save dialog box
        private ICommand otworzPlikAdresy;
        public ICommand OtworzPlikAdresy
        {
            get
            {
                if (otworzPlikAdresy == null)
                    otworzPlikAdresy = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        DodajAdresFileDialog otworz = new DodajAdresFileDialog();
                        otworz.PokazOpenDialog(fileFilter);
                        Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
                        foreach (var i in Adresy)
                        {
                            Odpalamy(i);
                        }
                    });
                return otworzPlikAdresy;
            }
        }
        private ICommand zmienStatus;
        public ICommand ZmienStatus
        {
            get
            {
                if (zmienStatus == null)
                    zmienStatus = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        foreach (var rzecz in ListaAdresow)
                        {
                            if (rzecz.ZaznaczenieEmail == true)
                            {
                                if (rzecz.AktywnyEmail == true)
                                {
                                    rzecz.AktywnyEmail = false;

                                }
                                else
                                {
                                    rzecz.AktywnyEmail = true;

                                }
                            }
                            rzecz.ZaznaczenieEmail = false;
                        }

                        Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
                    });
                return zmienStatus;
            }
        }
        private ICommand usunAdresy;
        public ICommand UsunAdresy
        {
            get
            {
                if (usunAdresy == null)
                    usunAdresy = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        for (int rzecz = ListaAdresow.Count - 1; rzecz >= 0; rzecz--)
                        {
                            if (ListaAdresow[rzecz].ZaznaczenieEmail == true)
                            {
                                ListaAdresow[rzecz].ZaznaczenieEmail = false;
                                ListaAdresow.RemoveAt(rzecz);
                            }

                        }
                        Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
                        foreach (var i in Adresy)
                        {
                            Odpalamy(i);
                        }
                    });
                return usunAdresy;
            }
        }
        private ICommand zapiszDoPliku;
        public ICommand EksportujDoPliku
        {
            get
            {
                if (zapiszDoPliku == null)
                    zapiszDoPliku = new RelayCommand(
                        o =>
                        {
                            EksportAdresFileDialog eksport = new EksportAdresFileDialog();
                            eksport.PokazSaveDialog(fileFilter);
                        });
                return zapiszDoPliku;
            }
        }
        private ICommand sortujAdresy;
        public ICommand SortujAdresy
        {
            get
            {
                if (sortujAdresy == null)
                    sortujAdresy = new RelayCommand(
                        o =>
                        {
                            model.SortowniaAdresow();
                            Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
                            KopiujAdresy();
                            CheckWydarzenie();
                        });
                return sortujAdresy;
            }
        }
        private ICommand checkAll;
        public ICommand CheckAll
        {
            get
            {
                if (checkAll == null)
                    checkAll = new RelayCommand(
                        o =>
                        {

                            foreach (var rzecz in ListaAdresow)
                            {
                                if (rzecz.WyswietlenieEmail == true)
                                    rzecz.ZaznaczenieEmail = true;
                            }
                        });
                return checkAll;
            }
        }
        private ICommand uncheckAll;
        public ICommand UncheckAll
        {
            get
            {
                if (uncheckAll == null)
                    uncheckAll = new RelayCommand(
                        o =>
                        {
                            foreach (var rzecz in ListaAdresow)
                            {
                                if (rzecz.WyswietlenieEmail == true)
                                    rzecz.ZaznaczenieEmail = false;
                            }
                        });
                return uncheckAll;
            }
        }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public string Caption { get; set; }
        private ICommand dodajAdres;
        public ICommand DodajAdres
        {
            get
            {
                if (dodajAdres == null)
                    dodajAdres = new RelayCommand(
                        o =>
                        {
                            var openWindow = new WindowDialog();
                            openWindow.ShowWindow(new DodajAdresViewModel(), WindowHeight, WindowWidth, Caption);
                            Models.ZapisXML.ZapiszAdresy(saveAdresPathXml, model);
                            KopiujAdresy();
                            CheckWydarzenie();
                        });
                return dodajAdres;
            }
        }
    }
}
