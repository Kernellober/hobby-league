using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mannschaften
{
    class Mannschaft
    {
        private string name = "";
        private int punkte = 0;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Punkte
        {
            get { return punkte; }
            set { punkte = value; }
        }

        public Mannschaft(string n, int p)
        {
            name = n;
            punkte = p;
        }

    }

    class Menue
    {
        private List<Mannschaft> teams = new List<Mannschaft>();

        private bool erfassungAbgeschlossen = false;

        public void Anzeigen()
        {
            int auswahl = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("*** Hobbyligaverwaltung *** Wählen Sie aus:");
                Console.WriteLine("1 Mannschaften erfassen");
                Console.WriteLine("2 Tabelle ausgeben");
                Console.WriteLine("3 Punkte erfassen");
                Console.WriteLine("4 Tabelle in Datei speichern");
                Console.WriteLine("5 Tabelle aus Datei Laden");
                Console.WriteLine("9 Programmende");
                Console.Write("Auswahl: ");

                auswahl = Convert.ToInt32(Console.ReadLine());

                switch (auswahl)
                {
                    case 1:
                        {
                            if (erfassungAbgeschlossen == false)
                                MannschaftenAnlegen();
                            else
                            {
                                Console.WriteLine("Keine weiteren Mannschaften möglich! Weiter mit Enter!");
                                Console.ReadLine();
                            }
                        }
                        break;
                    case 2:
                        TabelleAusgeben();
                        break;
                    case 3:
                        PunkteErfassen();
                        break;
                    case 4:
                        TabelleSpeichern();
                        break;
                    case 5:
                        TabelleAuslesen();
                        break;
                    case 9: break;
                    default:
                        {
                            Console.WriteLine("Keine gültige Zahl! Weiter mit Enter!");
                            Console.ReadLine();
                        }
                        break;
                }

            } while (auswahl != 9);
        }

        public void TabelleAuslesen()
        {
            bool exisitiertDatei = false;
            do
            {
                Console.Write("Dateiname eingeben: ");
                string dateiName = Console.ReadLine();
                string path = @"C:\Users\gaedtkej\OneDrive - Berufliches Schulzentrum Wiesau\WIT12B\AEuP_C#\Hobbyliga_Vorlage\Mannschaften\bin\Debug\" + dateiName + ".csv";

                if (!File.Exists(path))
                {
                    Console.WriteLine("Datei nicht vorhanden. Eingabe wiederholen!");
                }
                else
                {
                    exisitiertDatei = true;
                    teams = new List<Mannschaft>();

                    using (StreamReader sw = new StreamReader(path))
                    {
                        string zeile = sw.ReadLine();

                        while (zeile != null)
                        {
                            string[] getrennteZeile = new string[2];
                            getrennteZeile = zeile.Split(';');

                            if (int.TryParse(getrennteZeile[1], out int punke))
                            {
                                teams.Add(new Mannschaft(getrennteZeile[0], punke));
                            }

                            zeile = sw.ReadLine();

                        }
                    }

                    Console.Write("Daten aus Datei geladen. Zurück zum Hauptmenü mit Enter.");
                    Console.ReadLine();
                }

            } while (!exisitiertDatei);
        }

        public void TabelleSpeichern()
        {
            bool exisitiertDatei = true;
            do
            {
                Console.Write("Dateiname eingeben: ");
                string dateiName = Console.ReadLine();
                string path = @"C:\Users\gaedtkej\OneDrive - Berufliches Schulzentrum Wiesau\WIT12B\AEuP_C#\Hobbyliga_Vorlage\Mannschaften\bin\Debug\" + dateiName + ".csv";

                if (File.Exists(path))
                {
                    Console.WriteLine("Datei bereits vorhanden. Eingabe wiederholen!");
                }
                else
                {
                    exisitiertDatei = false;

                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        var orderedTeams = teams.OrderBy(x => x.Punkte).ToList();

                        for (int i = orderedTeams.Count() - 1; i >= 0; i--)
                        {
                            sw.WriteLine(
                                    orderedTeams[i].Name + ";" + orderedTeams[i].Punkte.ToString()
                                );
                        }
                    }

                    Console.Write("Speichern erledigt. Zurück zum Hauptmenü mit Enter.");
                    Console.ReadLine();
                }

            } while (exisitiertDatei);
        }

        public void MannschaftenAnlegen()
        {
            string beenden = "9";
            string name = "";

            do
            {
                Console.Write("Name der Mannschaft: ");
                name = Console.ReadLine();
                Mannschaft team = new Mannschaft(name, 0);
                teams.Add(team);
                Console.Write("Weiter mit Enter oder Ende der Erfassung mit 9: ");
                beenden = Console.ReadLine();
            } while (beenden != "9");
            erfassungAbgeschlossen = true;
        }

        public void TabelleAusgeben()
        {
            int feldbreite = 0;
            foreach (Mannschaft m in teams)
            {
                if (m.Name.Length > feldbreite)
                    feldbreite = m.Name.Length;
            }

            int zaehler = 1;
            foreach (Mannschaft m in teams)
            {
                Console.WriteLine($"{zaehler} {m.Name.ToString().PadRight(feldbreite)} {m.Punkte}");
                zaehler++;
            }
            Console.WriteLine("Weiter mit Enter");
            Console.ReadLine();
        }

        public void PunkteErfassen()
        {
            TabelleAusgeben();
            int platz = -1;
            int punkte = 0;
            int indexMerker = 0;
            Console.Write("Für welche Mannschaft gibt es Punkte? (Platzierung eingeben:) ");
            platz = Convert.ToInt32(Console.ReadLine());
            Mannschaft aend = teams[platz - 1];
            Console.Write("Punkte: ");
            punkte = Convert.ToInt32(Console.ReadLine());
            aend.Punkte += punkte;

            teams.RemoveAt(platz - 1);
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].Punkte < aend.Punkte)
                {
                    indexMerker = i;
                    break;
                }
            }
            teams.Insert(indexMerker, aend);
            Console.WriteLine("Neue Tabelle:");
            TabelleAusgeben();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Menue menue = new Menue();
            menue.Anzeigen();
        }
    }
}