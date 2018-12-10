using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Mail_24.ViewModels
{
    public class LimityViewModel : INotifyPropertyChanged
    {
        private Models.LimityModel model;
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
        public string PerHour
        {
            get
            {
                return model.PerHour;
            }
            set
            {
                model.PerHour = value;
                OnPropertyChanged("PerHour");
            }
        }
        public string PerDay
        {
            get
            {
                return model.PerDay;
            }
            set
            {
                model.PerDay = value;
                OnPropertyChanged("PerDay");
            }
        }
        public string HowOften
        {
            get
            {
                return model.HowOften;
            }
            set
            {
                model.HowOften = value;
                OnPropertyChanged("HowOften");
            }
        }
        public DateTime RozpoczecieGodzinowe
        {
            get
            {
                return model.RozpoczecieGodzinowe;
            }
            set
            {
                model.RozpoczecieGodzinowe = value;
                OnPropertyChanged("RozpoczecieGodzinowe");
            }
        }
        public DateTime RozpoczecieDobowe
        {
            get
            {
                return model.RozpoczecieDobowe;
            }
            set
            {
                model.RozpoczecieDobowe = value;
                OnPropertyChanged("RozpoczecieDobowe");
            }
        }
        public int IloscGodzinowa
        {
            get
            {
                return model.IloscGodzinowa;
            }
            set
            {
                model.IloscGodzinowa = value;
                OnPropertyChanged("IloscGodzinowa");
            }
        }
        public int IloscDobowa
        {
            get
            {
                return model.IloscDobowa;
            }
            set
            {
                model.IloscDobowa = value;
                OnPropertyChanged("IloscDobowa");
            }
        }
        public LimityViewModel(string perHour, string perDay, string howOften,
            DateTime rozpoczecieGodzinowe, DateTime rozpoczecieDobowe, int iloscGodzinowa, int iloscDobowa)
        {
            model = new Models.LimityModel(perHour, perDay, howOften, rozpoczecieGodzinowe, rozpoczecieDobowe, iloscGodzinowa, iloscDobowa);
        }

        public Models.LimityModel GetModel()
        {
            return model;
        }
    }
    public class Limity : INotifyPropertyChanged
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
        public static LimityViewModel limity { get; set; } = new LimityViewModel("", "", "", DateTime.Now, DateTime.Now, 0, 0);
        private const string saveLimityXml = "limity.xml";
        public Limity()
        {
            WczytajLimity();
        }
        public void WczytajLimity()
        {
            if (System.IO.File.Exists(saveLimityXml))
            {
                Models.ZapisXML.WczytajLimity(saveLimityXml, limity);
            }
            else
            {
                ResetDoDomyslnych();
            }
        }
        public void ResetDoDomyslnych()
        {
            limity.PerHour = "99";
            limity.PerDay = "499";
            limity.HowOften = "3";
            limity.RozpoczecieGodzinowe = DateTime.Now;
            limity.RozpoczecieDobowe = DateTime.Now;
            limity.IloscGodzinowa = 0;
            limity.IloscDobowa = 0;
        }
        public void Zapisz() //dla zapisu z innych VM
        {
            Models.ZapisXML.ZapiszLimity(saveLimityXml, limity.PerHour, limity.PerDay, limity.HowOften,
                                limity.RozpoczecieGodzinowe, limity.RozpoczecieDobowe, limity.IloscGodzinowa, limity.IloscDobowa);
        }
        private ICommand zapisanieLimitow;
        public ICommand ZapisanieLimitow
        {
            get
            {
                if (zapisanieLimitow == null)
                    zapisanieLimitow = new RelayCommand(
                        o =>
                        {
                            var openDialog = new Powiadomienie();
                            Zapisz();
                            WczytajLimity();
                            openDialog.ShowPowiadomienie("Zapisano ustawienia", "Powiadomienie");

                        });
                return zapisanieLimitow;
            }
        }
        private ICommand resetLimitow;
        public ICommand ResetLimitow
        {
            get
            {
                if (resetLimitow == null)
                    resetLimitow = new RelayCommand(
                        o =>
                        {
                            var openDialog = new Powiadomienie();
                            ResetDoDomyslnych();
                            Zapisz();
                            openDialog.ShowPowiadomienie("Zapisano domyślne ustawienia", "Powiadomienie");
                        });
                return resetLimitow;
            }
        }
    }
}
