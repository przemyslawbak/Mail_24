using System;
using System.Collections;
using System.Collections.Generic;

namespace Mail_24.Models
{
    public class AdresModel
    {
        public string AdresMailowy { get; private set; }
        public bool AktywnyEmail { get; set; }
        public bool ZaznaczenieEmail { get; set; }
        public bool WyswietlenieEmail { get; set; }
        public AdresModel(string adresEmail, bool aktywnyEmail, bool zaznaczenieEmail, bool wyswietlenieEmail)
        {
            AdresMailowy = adresEmail;
            AktywnyEmail = aktywnyEmail;
            ZaznaczenieEmail = zaznaczenieEmail;
            WyswietlenieEmail = wyswietlenieEmail;
        }
    }
    public class AdresyEmail : IEnumerable<AdresModel> //dla modeli, które będą zebrane w kolekcję
    {
        private List<AdresModel> listaAdresow = new List<AdresModel>(); //inst kolekcji adresów
        public IEnumerator<AdresModel> GetEnumerator()
        {
            return listaAdresow.GetEnumerator(); //zwraca enumerator który iteruje przez całą kolekcję
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void DodajAdres(AdresModel adres) //metoda, dodawanie nowego adresu
        {
            listaAdresow.Add(adres);
        }
        public void UsunAdres(AdresModel adres) //metoda, usuwanie adresu
        {
            listaAdresow.Remove(adres);
        }
        private Comparison<AdresModel> porownywanieAdresow = new Comparison<AdresModel>( //metoda, porównywarka adresów
            (AdresModel adres1, AdresModel adres2) => //dla adresu 1 i 2
            {
                int wynik = -adres1.AdresMailowy.CompareTo(adres2.AdresMailowy); //zwracamy wynik, gdzie adres1 porównujemy do zdres2
                return wynik;
            });
        public void SortowniaAdresow() //metoda, sortujemy porównane adresy
        {
            listaAdresow.Sort(porownywanieAdresow); //Sort sortuje elementy w całej kolekcji
            listaAdresow.Reverse(); //odwraca wynik
        }
    }
}
