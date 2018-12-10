using System.ComponentModel;

namespace Mail_24.ViewModels
{
    public class ZalacznikViewModel : INotifyPropertyChanged
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
        private Models.ZalacznikModel model;
        public string NazwaZalacznika
        {
            get
            {
                return model.NazwaPliku;
            }
        }
        public string AdresPliku
        {
            get
            {
                return model.AdresPliku;
            }
        }
        public ZalacznikViewModel(Models.ZalacznikModel zalacznik)
        {
            model = zalacznik;
        }
        public ZalacznikViewModel(string nazwaPLiku, string adresPliku)
        {
            model = new Models.ZalacznikModel(nazwaPLiku, adresPliku);
        }
        public Models.ZalacznikModel GetModel()
        {
            return model;
        }
    }
}
