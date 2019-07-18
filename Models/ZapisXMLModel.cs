using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Mail_24.Models
{
    public static class ZapisXML
    {
        public static void ZapiszKonta(string filePath, Konta konta)
        {
            try
            {
                XDocument xml =
                    new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Konta", from KontoModel konto in konta
                                          select new XElement("Konto",
                    new XElement("AdresSerwera", konto.AdresSerwera),
                    new XElement("Port", konto.Port),
                    new XElement("ImieNazwisko", konto.ImieNazwisko),
                    new XElement("AdresEmail", konto.AdresEmail),
                    new XElement("Haslo", konto.Haslo))));
                xml.Save(filePath);
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy zapisie", exc);
            }
        }
        public static Konta WczytajKonta(string filePath)
        {
            try
            {
                XDocument xml = XDocument.Load(filePath);
                IEnumerable<KontoModel> dane =
                from konto in xml.Root.Descendants("Konto")
                select new KontoModel
                (
                    konto.Element("AdresSerwera").Value,
                    konto.Element("Port").Value,
                    konto.Element("ImieNazwisko").Value,
                    konto.Element("AdresEmail").Value,
                    konto.Element("Haslo").Value);

                Konta konta = new Konta();
                foreach (KontoModel konto in dane) konta.DodajKonto(konto);
                return konta;
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy odczycie", exc);
            }
        }
        //adresy email
        public static void ZapiszAdresy(string filePath, AdresyEmail adresy)
        {
            try
            {
                XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Adresy", from AdresModel adres in adresy
                                           select new XElement("Adres",
                                           new XElement("AdresMailowy", adres.AdresMailowy),
                                           new XElement("AktywnyEmail", (adres.AktywnyEmail).ToString()),
                                           new XElement("ZaznaczenieEmail", false),
                                           new XElement("WyswietlenieEmail", true)))); //domyślnie zapisuje prawdę
                xml.Save(filePath);
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy zapisie", exc);
            }
        }
        public static AdresyEmail WczytajAdresy(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    XDocument xml = XDocument.Load(fs);
                    IEnumerable<AdresModel> dane =
                    from adres in xml.Root.Descendants("Adres")
                    select new AdresModel
                    (
                        adres.Element("AdresMailowy").Value,
                        bool.Parse(adres.Element("AktywnyEmail").Value),
                        bool.Parse(adres.Element("ZaznaczenieEmail").Value),
                        bool.Parse(adres.Element("WyswietlenieEmail").Value));

                    AdresyEmail adresy = new AdresyEmail();
                    foreach (AdresModel adres in dane) adresy.DodajAdres(adres);
                    return adresy;
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy odczycie", exc);
            }
        }
        public static void ZapiszLimity(string filePath, string perHour, string perDay, string howOften,
            DateTime rozpoczecieGodzinowe, DateTime rozpoczecieDobowe, int iloscGodzinowa, int iloscDobowa)
        {
            XDocument document = new XDocument();
            document.Add(new XElement("Limity",
                            new XElement("PerHour", perHour),
                            new XElement("PerDay", perDay),
                            new XElement("HowOften", howOften),
                            new XElement("RozpoczecieGodzinowe", rozpoczecieGodzinowe.ToString()),
                            new XElement("RozpoczecieDobowe", rozpoczecieDobowe.ToString()),
                            new XElement("IloscGodzinowa", iloscGodzinowa.ToString()),
                            new XElement("IloscDobowa", iloscDobowa.ToString())));
            document.Save(filePath);
        }
        public static void WczytajLimity(string filePath, ViewModels.LimityViewModel vmodel)
        {
            try
            {
                XDocument document = XDocument.Load(filePath);
                foreach (XElement element in document.Descendants("Limity"))
                {
                    vmodel.PerHour = element.Element("PerHour").Value;
                    vmodel.PerDay = element.Element("PerDay").Value;
                    vmodel.HowOften = element.Element("HowOften").Value;
                    vmodel.RozpoczecieGodzinowe = DateTime.Parse(element.Element("RozpoczecieGodzinowe").Value);
                    vmodel.RozpoczecieDobowe = DateTime.Parse(element.Element("RozpoczecieDobowe").Value);
                    vmodel.IloscGodzinowa = int.Parse(element.Element("IloscGodzinowa").Value);
                    vmodel.IloscDobowa = int.Parse(element.Element("IloscDobowa").Value);
                    break;
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy odczycie", exc);
            }
        }
    }
}
