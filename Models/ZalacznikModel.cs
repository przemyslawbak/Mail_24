using System.Collections;
using System.Collections.Generic;

namespace Mail_24.Models
{
    public class ZalacznikModel
    {
        public string NazwaPliku { get; set; }
        public string AdresPliku { get; set; }
        public ZalacznikModel(string nazwaPliku, string adresPliku)
        {
            NazwaPliku = nazwaPliku;
            AdresPliku = adresPliku;
        }
    }
    public class ZalacznikiModel : IEnumerable<ZalacznikModel>
    {
        public IEnumerator<ZalacznikModel> GetEnumerator()
        {
            return listaZalacznikow.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private List<ZalacznikModel> listaZalacznikow = new List<ZalacznikModel>(); //inst kolekcji
        public void DodajZalacznik(ZalacznikModel adres) //dodawanie
        {
            listaZalacznikow.Add(adres);
        }
        public void UsunZalacznik(ZalacznikModel adres) //usuwanie
        {
            listaZalacznikow.Remove(adres);
        }
    }
}
