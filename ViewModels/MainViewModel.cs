using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mail_24.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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
        public MainViewModel()
        {
            Status = Models.StatusWysylania.Wstrzymanie;
        }
        public static ObservableCollection<ZalacznikViewModel> ListaZalacznikow { get; } = new ObservableCollection<ZalacznikViewModel>();
        private Models.WysylanieModel wiadomoscModel = new Models.WysylanieModel();
        public string TematWiadomosc
        {
            get
            {
                return wiadomoscModel.TematWiadomosc;
            }
            set
            {
                wiadomoscModel.TematWiadomosc = value;
            }
        }
        public string TrescWiadomosc
        {
            get
            {
                return wiadomoscModel.TrescWiadomosci;
            }
            set
            {
                wiadomoscModel.TrescWiadomosci = value;
            }
        }
        public Models.StatusWysylania Status
        {
            get
            {
                return wiadomoscModel.Status;
            }
            set
            {
                wiadomoscModel.Status = value;
            }
        }
        public string TerazWysylanyAdres
        {
            get
            {
                return wiadomoscModel.TerazWysylanyAdres;
            }
            set
            {
                wiadomoscModel.TerazWysylanyAdres = value;
            }
        }
        public string TerazWysylaneKonto
        {
            get
            {
                return wiadomoscModel.TerazWysylaneKonto;
            }
            set
            {
                wiadomoscModel.TerazWysylaneKonto = value;
            }
        }
        private string godzinowoWidok;
        public string GodzinowoWidok
        {
            get
            {
                return godzinowoWidok;
            }
            set
            {
                godzinowoWidok = value;
                OnPropertyChanged("GodzinowoWidok");
            }
        }
        private string godzinowoModel;
        public string GodzinowoModel
        {
            get
            {
                return godzinowoModel;
            }
            set
            {
                godzinowoModel = value;
                OnPropertyChanged("GodzinowoModel");
            }
        }
        private string dobowoWidok;
        public string DobowoWidok
        {
            get
            {
                return dobowoWidok;
            }
            set
            {
                dobowoWidok = value;
                OnPropertyChanged("DobowoWidok");
            }
        }
        private string dobowoModel;
        public string DobowoModel
        {
            get
            {
                return dobowoModel;
            }
            set
            {
                dobowoModel = value;
                OnPropertyChanged("DobowoModel");
            }
        }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public string Caption { get; set; }
        private ICommand openWindowKonto;
        public ICommand OpenWindowKonto
        {
            get
            {
                if (openWindowKonto == null) //jeśli nie jest uruchomiony...
                    openWindowKonto = new RelayCommand( //przekazujemy do RC...
                    argument =>
                    {
                        var openWindow = new WindowDialog();
                        openWindow.ShowWindow(new Konta(), WindowHeight, WindowWidth, Caption);
                    });
                return openWindowKonto;
            }
        }
        private ICommand openWindowUstawienia;
        public ICommand OpenWindowUstawienia
        {
            get
            {
                if (openWindowUstawienia == null) //jeśli nie jest uruchomiony...
                    openWindowUstawienia = new RelayCommand( //przekazujemy do RC...
                    argument =>
                    {
                        var openWindow = new WindowDialog();
                        openWindow.ShowWindow(new Limity(), WindowHeight, WindowWidth, Caption);
                    });
                return openWindowUstawienia;
            }
        }
        private ICommand openWindowAdresEmail;
        public ICommand OpenWindowAdresEmail
        {
            get
            {
                if (openWindowAdresEmail == null) //jeśli nie jest uruchomiony...
                    openWindowAdresEmail = new RelayCommand( //przekazujemy do RC...
                    argument =>
                    {
                        var openWindow = new WindowDialog();
                        openWindow.ShowWindow(new Adres(), WindowHeight, WindowWidth, Caption);
                    });
                return openWindowAdresEmail;
            }
        }
        private ICommand dodajZalacznik;
        public ICommand DodajZalacznik
        {
            get
            {
                if (dodajZalacznik == null)
                    dodajZalacznik = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        DodajZalacznikFileDialog otworz = new DodajZalacznikFileDialog();
                        otworz.ZalacznikOpenDialog();
                    });
                return dodajZalacznik;
            }
        }
        private ICommand usunZalacznik;
        public ICommand UsunZalacznik
        {
            get
            {
                if (usunZalacznik == null) //jak delPerson jeszcze nie było
                    usunZalacznik = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu
                        ZalacznikViewModel zalacznik = ListaZalacznikow[indeksZadania]; //przypisanie zmiennej zadanie obiekt z kolekcji
                        ListaZalacznikow.Remove(zalacznik); //z kolekcji usuwamy przypisany zmiennej obiekt
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        if (o == null) return false; //jeśli nie ma obiektu...
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu (ale po co?)
                        return indeksZadania >= 0; //zwracamy indeksZadania >= 0; (nie może być chyba <0)
                    });
                return usunZalacznik; //wykonujemy RelayCommand
            }
        }
        private ICommand startSending;
        public ICommand StartSending
        {
            get
            {
                if (startSending == null)
                    startSending = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        WysylanieWiadomosci();//odpalenie metody wysyłania
                    });
                return startSending;
            }
        }
        private ICommand stopSending;
        public ICommand StopSending
        {
            get
            {
                if (stopSending == null)
                    stopSending = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        Status = Models.StatusWysylania.Wstrzymanie;
                        OnPropertyChanged("Status");
                    });
                return stopSending;
            }
        }
        //metoda i algorytm wysylania wiadomosci
        public async void WysylanieWiadomosci()
        {
            Adres adresy = new Adres();
            Konta konta = new Konta();
            Limity limity = new Limity();
            var openDialog = new Powiadomienie();
            int taskDelayValue = Convert.ToInt32(decimal.Parse(Limity.limity.HowOften) / Konta.ListaKont.Count * 1000);
            if (Adres.ListaAdresow.Count > 0 && Konta.ListaKont.Count > 0 && !string.IsNullOrWhiteSpace(TematWiadomosc) && !string.IsNullOrWhiteSpace(TrescWiadomosc))
            {
                int kontaLicznik = 0;
                int adresyLicznik = 0;
                do
                {
                    GodzinowoModel = Limity.limity.PerHour;
                    DobowoModel = Limity.limity.PerDay;
                    GodzinowoWidok = Limity.limity.IloscGodzinowa.ToString();
                    DobowoWidok = Limity.limity.IloscDobowa.ToString();
                    if ((DateTime.Now - Limity.limity.RozpoczecieGodzinowe) > TimeSpan.FromHours(1)) //reset rozpoczęcia godzinowego
                    {
                        Limity.limity.RozpoczecieGodzinowe = DateTime.Now;
                        Limity.limity.IloscGodzinowa = 0;
                        limity.Zapisz();
                    }
                    if ((DateTime.Now - Limity.limity.RozpoczecieDobowe) > TimeSpan.FromHours(24)) //rezet rozpoczęcia dobowego
                    {
                        Limity.limity.RozpoczecieDobowe = DateTime.Now;
                        Limity.limity.IloscDobowa = 0;
                        limity.Zapisz();
                    }
                    if (Limity.limity.IloscGodzinowa > int.Parse(Limity.limity.PerHour) || Limity.limity.IloscDobowa > int.Parse(Limity.limity.PerDay)) //jeśli mamy limit godzinowy lub dniowy
                    {
                        Status = Models.StatusWysylania.Oczekiwanie;
                        OnPropertyChanged("Status");
                        //openDialog.ShowPowiadomienie("Limit godzinowy/dobowy", "Powiadomienie");
                        await Task.Delay(1000);
                        if (Status == Models.StatusWysylania.Wstrzymanie) break;
                        continue;
                    }
                    if (adresyLicznik < Adres.ListaAdresow.Count) //wysyłka
                    {
                        Status = Models.StatusWysylania.Wysylanie;
                        OnPropertyChanged("Status");
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                        });
                        if (Adres.ListaAdresow[adresyLicznik].AktywnyEmail == true)
                        {
                            TerazWysylanyAdres = Adres.ListaAdresow[adresyLicznik].AdresMailowy;
                            OnPropertyChanged("TerazWysylanyAdres");
                            TerazWysylaneKonto = Konta.ListaKont[kontaLicznik].AdresEmail; //email konta
                            string TerazWysylaneHaslo = Konta.ListaKont[kontaLicznik].Haslo; //haslo konta
                            string TerazWysylanySerwer = Konta.ListaKont[kontaLicznik].AdresSerwera; //smtp adres
                            string TerazWysylaneNazwisko = Konta.ListaKont[kontaLicznik].ImieNazwisko; //nazwisko konta
                            int TerazWysylanyPort = int.Parse(Konta.ListaKont[kontaLicznik].Port);
                            OnPropertyChanged("TerazWysylaneKonto");
                            //tutaj metoda wysyłania wiadomości
                            WyslijWiadomosc(TerazWysylanyAdres,
                                TerazWysylaneKonto,
                                TerazWysylaneNazwisko,
                                wiadomoscModel.TematWiadomosc,
                                wiadomoscModel.TrescWiadomosci,
                                TerazWysylaneHaslo,
                                TerazWysylanySerwer,
                                TerazWysylanyPort);
                            Adres.ListaAdresow[adresyLicznik].AktywnyEmail = false;
                            kontaLicznik++;
                            adresy.ZapiszAdresy();
                            await Task.Delay(taskDelayValue); //wpisana ilość przez ilość kont
                        }
                        adresyLicznik++;
                    }
                    if (kontaLicznik >= Konta.ListaKont.Count) //jeśli dojdziemy do końca listy kont
                    {
                        kontaLicznik = 0;
                        Limity.limity.IloscGodzinowa++;
                        Limity.limity.IloscDobowa++;
                        limity.Zapisz();
                    }
                    if (adresyLicznik == Adres.ListaAdresow.Count) //jeśli dojdziemy do końca listy adresów
                    {
                        Status = Models.StatusWysylania.Wstrzymanie;
                        OnPropertyChanged("Status");
                        openDialog.ShowPowiadomienie("Wysłano wszystkie wiadomości", "Powiadomienie");
                        break;
                    }
                    if (Status == Models.StatusWysylania.Wstrzymanie) break;
                } while (kontaLicznik < Konta.ListaKont.Count);
            }
            else
            {
                //info w razie nie spełnienia warunków
                if (Adres.ListaAdresow.Count < 1)
                    openDialog.ShowPowiadomienie("Nie można rozpocząć wysyłania, adresy nie zostały wczytane", "Powiadomienie");
                if (Konta.ListaKont.Count < 1)
                    openDialog.ShowPowiadomienie("Nie można rozpocząć wysyłania, konta nie zostały zapisane", "Powiadomienie");
                if (string.IsNullOrWhiteSpace(TematWiadomosc) || string.IsNullOrWhiteSpace(TrescWiadomosc))
                    openDialog.ShowPowiadomienie("Nie można rozpocząć wysyłania, brak tematu lub treści wiadomości", "Powiadomienie");

            }
            void WyslijWiadomosc(string to, string from, string name, string subject, string body, string pass, string smtp, int port) //wysyla wiadomosc
            {
                //dane wiadomosci
                MailMessage msg = new MailMessage();
                msg.To.Add(to);//Mail recipient account
                msg.From = new MailAddress(from, name, System.Text.Encoding.UTF8);//Mail account and displays the name and the character encoding
                msg.Subject = subject;//Message header
                msg.SubjectEncoding = System.Text.Encoding.UTF8;//Mail title code
                msg.Body = body;//The message content
                msg.BodyEncoding = System.Text.Encoding.UTF8;//Message encoding
                msg.IsBodyHtml = false;//Whether the HTML mail
                msg.Priority = MailPriority.Normal;//Priority mail
                if (ListaZalacznikow.Count > 0)
                {
                    foreach (var item in ListaZalacznikow)
                    {
                        msg.Attachments.Add(new Attachment(item.AdresPliku));
                    }
                }
                //dane konta
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(from, pass);//The registered email address and password, the QQ mailbox, if you set a password to use independent, independent password instead of the password
                client.Host = smtp;//QQ mailbox corresponding to the SMTP server
                client.Port = port;
                object userState = msg;
                try
                {
                    client.SendAsync(msg, userState);
                }
                catch (Exception ex)
                {
                }
            }
        }

    }
}

