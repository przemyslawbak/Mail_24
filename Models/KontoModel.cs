using System.Collections;
using System.Collections.Generic;

namespace Mail_24.Models
{
    public class KontoModel
    {
        public string AdresSerwera { get; private set; }
        public string Port { get; private set; }
        public string ImieNazwisko { get; private set; }
        public string AdresEmail { get; private set; }
        public string Haslo { get; private set; }
        public KontoModel(string adresSerwera, string port, string imieNazwisko, string adresEmail, string haslo)
        {
            AdresSerwera = adresSerwera;
            Port = port;
            ImieNazwisko = imieNazwisko;
            AdresEmail = adresEmail;
            Haslo = haslo;
        }
    }
    public class Konta : IEnumerable<KontoModel>//dla adresów, które będą zebrane w kolekcję
    {
        private List<KontoModel> listaKont = new List<KontoModel>(); //instancja kolekcji kont
        public IEnumerator<KontoModel> GetEnumerator() //interfejs, korzysta datacontext vm z xaml
        {
            return listaKont.GetEnumerator(); //zwraca enumerator który iteruje przez całą kolekcję
        }
        IEnumerator IEnumerable.GetEnumerator() //interfejs
        {
            return GetEnumerator();
        }
        public void DodajKonto(KontoModel konto)  //metoda, dodawanie nowego konta
        {
            listaKont.Add(konto);
        }
        public void UsunKonto(KontoModel konto) //metoda, usuwanie konta
        {
            listaKont.Remove(konto);
        }
    }
}
