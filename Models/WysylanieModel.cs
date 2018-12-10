using System.ComponentModel;

namespace Mail_24.Models
{
    public enum StatusWysylania : byte { Wysylanie, Oczekiwanie, Wstrzymanie }
    public class WysylanieModel : INotifyPropertyChanged
    {
        private string tematWiadomosc;
        public string TematWiadomosc
        {
            get
            {
                return tematWiadomosc;
            }
            set
            {
                tematWiadomosc = value;
                OnPropertyChanged("TematWiadomosc");
            }
        }
        private string trescWiadomosci;
        public string TrescWiadomosci
        {
            get
            {
                return trescWiadomosci;
            }
            set
            {
                trescWiadomosci = value;
                OnPropertyChanged("TrescWiadomosci");
            }
        }
        private StatusWysylania status;
        public StatusWysylania Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        private string terazWysylanyAdres;
        public string TerazWysylanyAdres
        {
            get
            {
                return terazWysylanyAdres;
            }
            set
            {
                terazWysylanyAdres = value;
                OnPropertyChanged("TerazWysylanyAdres");
            }
        }
        private string terazWysylaneKonto;
        public string TerazWysylaneKonto
        {
            get
            {
                return terazWysylaneKonto;
            }
            set
            {
                terazWysylaneKonto = value;
                OnPropertyChanged("TerazWysylaneKonto");
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
    }
}
