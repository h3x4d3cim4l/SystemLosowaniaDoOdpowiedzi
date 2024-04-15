using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SystemLosowaniaDoOdpowiedzi.Resources.Models;

namespace SystemLosowaniaDoOdpowiedzi
{
    

    public partial class MainPage : ContentPage
    {
        private List<Klasa> listaKlas = new List<Klasa>();
        private Klasa wybranaKlasa;
        private string sciezkaPliku;


        public MainPage()
        {
            InitializeComponent();
            sciezkaPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dane.json");
            OdczytajDaneZPliku();
        }


        private void ZapiszDaneDoPliku()
        {
            var daneDoZapisu = JsonSerializer.Serialize(listaKlas);
            File.WriteAllText(sciezkaPliku, daneDoZapisu);
        }

        private void OdczytajDaneZPliku()
        {
            if (File.Exists(sciezkaPliku))
            {
                var daneZPliku = File.ReadAllText(sciezkaPliku);
                listaKlas = JsonSerializer.Deserialize<List<Klasa>>(daneZPliku);
                // Aktualizuj etykiety lub inne kontrole, jeśli to konieczne
            }
        }

        private void DodajKlase_Clicked(object sender, EventArgs e)
        {
            string nazwaKlasy = nazwaKlasyEntry.Text;
            if (listaKlas.Any(klasa => klasa.Nazwa == nazwaKlasy))
            {
                DisplayAlert("Błąd", "Klasa o tej nazwie już istnieje.", "OK");
                return;
            }

            listaKlas.Add(new Klasa(nazwaKlasy));
            DisplayAlert("Sukces", $"Klasa {nazwaKlasy} została dodana pomyślnie.", "OK");
            nazwaKlasyEntry.Text = "";
            ZapiszDaneDoPliku();
        }

        private void UsunKlase_Clicked(object sender, EventArgs e)
        {
            string nazwaKlasy = nazwaKlasyEntry.Text;
            Klasa klasaDoUsuniecia = listaKlas.FirstOrDefault(klasa => klasa.Nazwa == nazwaKlasy);
            if (klasaDoUsuniecia == null)
            {
                DisplayAlert("Błąd", "Nie ma klasy o podanej nazwie.", "OK");
                return;
            }

            listaKlas.Remove(klasaDoUsuniecia);
            DisplayAlert("Sukces", $"Klasa {nazwaKlasy} została usunięta pomyślnie.", "OK");
            ZapiszDaneDoPliku();
        }

        private async void WybierzKlase_Clicked(object sender, EventArgs e)
        {
            var listaNazwKlas = listaKlas.Select(klasa => klasa.Nazwa).ToList();
            string wybranaKlasa = await DisplayActionSheet("Wybierz klasę", "Anuluj", null, listaNazwKlas.ToArray());
            if (wybranaKlasa != "Anuluj")
            {
                wybranaKlasa = listaKlas.FirstOrDefault(k => k.Nazwa == wybranaKlasa).Nazwa;
                if (wybranaKlasa != null)
                {
                    wybranaKlasaLabel.Text = $"Wybrana klasa: {wybranaKlasa}";
                    this.wybranaKlasa = listaKlas.FirstOrDefault(k => k.Nazwa == wybranaKlasa);
                    AktualizujListeUczniow();
                }
                else
                {
                    await DisplayAlert("Błąd", "Nie można znaleźć wybranej klasy.", "OK");
                }
            }
        }

        private void DodajUcznia_Clicked(object sender, EventArgs e)
        {
            if (wybranaKlasa == null)
            {
                DisplayAlert("Błąd", "Najpierw wybierz klasę.", "OK");
                return;
            }

            string imie = imieEntry.Text;
            wybranaKlasa.Uczniowie.Add(new Uczen(imie, wybranaKlasa.Nazwa));
            imieEntry.Text = "";
            ZapiszDaneDoPliku();
            AktualizujListeUczniow();
        }

        private async void UsunUcznia_Clicked(object sender, EventArgs e)
        {
            if (wybranaKlasa == null)
            {
                DisplayAlert("Błąd", "Najpierw wybierz klasę.", "OK");
                return;
            }

            // Usuń ucznia z wybranej klasy
            // AktualizujListeUczniow();
            var listaUczniow = wybranaKlasa.Uczniowie.Select(uczen => uczen.Imie).ToArray();
            string usunietyUczen = await DisplayActionSheet("Wybierz ucznia do usunięcia", "Anuluj", null, listaUczniow);
            if (usunietyUczen != "Anuluj")
            {
                Uczen uczenDoUsuniecia = wybranaKlasa.Uczniowie.FirstOrDefault(uczen => uczen.Imie == usunietyUczen);
                if (uczenDoUsuniecia != null)
                {
                    wybranaKlasa.Uczniowie.Remove(uczenDoUsuniecia);
                    AktualizujListeUczniow();
                    ZapiszDaneDoPliku();
                    DisplayAlert("Sukces", $"Uczeń {usunietyUczen} został usunięty pomyślnie.", "OK");
                }
                else
                {
                    DisplayAlert("Błąd", "Wybrany uczeń nie istnieje.", "OK");
                }
            }
        }

        

        private void WylosujOsobe_Clicked(object sender, EventArgs e)
        {
            if (wybranaKlasa == null)
            {
                DisplayAlert("Błąd", "Najpierw wybierz klasę.", "OK");
                return;
            }
            if(wybranaKlasa.Uczniowie.Count ==0) 
            {
                DisplayAlert("Błąd", "Lista uczniow jest pusta, najpierw dodaj uczniów", "OK");
                return;
            }

            Random random = new Random();
            int index = random.Next(wybranaKlasa.Uczniowie.Count);
            string osobaWylosowana = wybranaKlasa.Uczniowie[index].Imie;
            DisplayAlert("Wylosowana osoba", osobaWylosowana, "OK");
        }

        private void AktualizujListeUczniow()
        {
            if (wybranaKlasa == null)
            {
                listaUczniowLabel.Text = "Nie wybrano klasy.";
                return;
            }

            string listaUczniow = string.Join("\n", wybranaKlasa.Uczniowie.Select((uczen, index) => $"{index + 1}. {uczen.Imie}"));
            listaUczniowLabel.Text = listaUczniow;

            if(wybranaKlasa.Uczniowie.Count == 0)
            {
                listaUczniowLabel.Text = "";
            }
        }
    }
}
