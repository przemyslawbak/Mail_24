using System;

namespace Mail_24.Models
{
    public class LimityModel
    {
        public string PerHour { get; set; }
        public string PerDay { get; set; }
        public string HowOften { get; set; }
        public DateTime RozpoczecieGodzinowe { get; set; }
        public DateTime RozpoczecieDobowe { get; set; }
        public int IloscGodzinowa { get; set; }
        public int IloscDobowa { get; set; }
        public LimityModel(string perHour, string perDay, string howOften,
            DateTime rozpoczecieGodzinowe, DateTime rozpoczecieDobowe, int iloscGodzinowa, int iloscDobowa)
        {
            PerHour = perHour;
            PerDay = perDay;
            HowOften = howOften;
            RozpoczecieGodzinowe = rozpoczecieGodzinowe;
            RozpoczecieDobowe = rozpoczecieDobowe;
            IloscGodzinowa = iloscGodzinowa;
            IloscDobowa = iloscDobowa;
        }
    }
}
