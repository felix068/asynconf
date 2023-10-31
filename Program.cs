using System;
using System.Collections.Generic;

class TUI
{
    private string[] options;
    private int cursorPosition;
    private string title;

    public TUI(string title, string[] options)
    {
        this.title = title;
        this.options = options;
        this.cursorPosition = 0;
    }

    public void Display()
    { // Affiche le menu
        Console.Clear();
        DisplayTitle(title);

        int maxOptionLength = options.Max(option => option.Length);
        int frameWidth = maxOptionLength + 4;

        // Affichage des options
        for (int i = 0; i < options.Length; i++)
        {
            if (i == cursorPosition)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Affichage des ║ et des options
            Console.SetCursorPosition((Console.WindowWidth - frameWidth) / 2, i + 2);
            Console.Write("║"); 
            Console.Write(" " + options[i].PadRight(maxOptionLength) + "   ");
            Console.Write("║");

            Console.ResetColor();
        }

        // Affichage du cadre
        Console.SetCursorPosition((Console.WindowWidth - frameWidth) / 2, 1);
        Console.Write("╔");
        Console.Write(new string('═', frameWidth));
        Console.Write("╗");

        Console.SetCursorPosition((Console.WindowWidth - frameWidth) / 2, options.Length + 2);
        Console.Write("╚");
        Console.Write(new string('═', frameWidth));
        Console.Write("╝");
    }

    public void DisplayTitle(string title)
    {
        int consoleWidth = Console.WindowWidth;
        Console.SetCursorPosition((consoleWidth - title.Length) / 2, 0);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    public string Run()
    {
        Display();

        // Boucle infinie pour la sélection des options
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Si l'utilisateur appuie sur une touche
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (cursorPosition > 0)
                    {
                        cursorPosition--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (cursorPosition < options.Length - 1)
                    {
                        cursorPosition++;
                    }
                    break;
                case ConsoleKey.Enter:
                    return options[cursorPosition];
                default:
                    break;
            }

            Display();
        }
    }
}


class Program
{
    public static void Main(string[] args)
    {
        // Parse JSON
        string json = System.IO.File.ReadAllText("data.json");
        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

        // Création des tableaux d'options
        string[] typeOptions = new string[data.Type.Count];
        string[] kilometrageOptions = new string[data.Kilometrage.Count];
        string[] anneeOptions = new string[data.Annee.Count];
        string[] energieOptions = new string[data.Energie.Count];
        string[] passagersOptions = new string[data.Passagers.Count];

        // Remplissage des tableaux d'options
        for (int i = 0; i < data.Type.Count; i++)
        {
            typeOptions[i] = data.Type[i].Nom;
        }

        for (int i = 0; i < data.Kilometrage.Count; i++)
        {
            kilometrageOptions[i] = data.Kilometrage[i].Kilometrage;
        }

        for (int i = 0; i < data.Annee.Count; i++)
        {
            anneeOptions[i] = data.Annee[i].Annee;
        }

        for (int i = 0; i < data.Energie.Count; i++)
        {
            energieOptions[i] = data.Energie[i].Energie;
        }

        for (int i = 0; i < data.Passagers.Count; i++)
        {
            passagersOptions[i] = data.Passagers[i].Passagers;
        }

        // Demande des informations à l'utilisateur
        string title = "Choisissez le type de voiture";
        string[] options = typeOptions;
        TUI tui = new TUI(title, options);
        string voiture = tui.Run();

        title = "Choisissez l'énergie";
        options = energieOptions;
        tui = new TUI(title, options);
        string energie = tui.Run();

        title = "Choisissez le kilométrage";
        options = kilometrageOptions;
        tui = new TUI(title, options);
        string kilometrage = tui.Run();

        title = "Choisissez l'année";
        options = anneeOptions;
        tui = new TUI(title, options);
        string annee = tui.Run();

        title = "Choisissez le nombre de passagers";
        options = passagersOptions;
        tui = new TUI(title, options);
        string passagers = tui.Run();

        Console.Clear();

        int typeIndex = Array.IndexOf(typeOptions, voiture);
        int kilometrageIndex = Array.IndexOf(kilometrageOptions, kilometrage);
        int anneeIndex = Array.IndexOf(anneeOptions, annee);
        int energieIndex = Array.IndexOf(energieOptions, energie);

        // Calcul du score
        string noteType = data.Type[typeIndex].Note;
        string noteKilometrage = data.Kilometrage[kilometrageIndex].Note;
        string noteAnnee = data.Annee[anneeIndex].Note;
        string noteEnergie = data.Energie[energieIndex].Note;

        // On enlève les "/10" pour pouvoir convertir en int
        noteType = noteType.Substring(0, noteType.IndexOf("/"));
        noteKilometrage = noteKilometrage.Substring(0, noteKilometrage.IndexOf("/"));
        noteAnnee = noteAnnee.Substring(0, noteAnnee.IndexOf("/"));
        noteEnergie = noteEnergie.Substring(0, noteEnergie.IndexOf("/"));

        // On convertit en int et on additionne les notes
        int scoreVehicule = Convert.ToInt32(noteType) + Convert.ToInt32(noteKilometrage) + Convert.ToInt32(noteAnnee) + Convert.ToInt32(noteEnergie);

        // Calcul du taux d'emprunt
        string tauxEmprunt = "";
        if (scoreVehicule >= 0 && scoreVehicule <= 10)
        {
            tauxEmprunt = data["Taux d’emprunt"][0].Taux;
        }
        else if (scoreVehicule >= 11 && scoreVehicule <= 15)
        {
            tauxEmprunt = data["Taux d’emprunt"][1].Taux;
        }
        else if (scoreVehicule >= 16 && scoreVehicule <= 25)
        {
            tauxEmprunt = data["Taux d’emprunt"][2].Taux;
        }
        else if (scoreVehicule >= 26 && scoreVehicule <= 33)
        {
            tauxEmprunt = data["Taux d’emprunt"][3].Taux;
        }
        else if (scoreVehicule >= 34 && scoreVehicule <= 40)
        {
            tauxEmprunt = data["Taux d’emprunt"][4].Taux;
        }

        // On ajoute le taux d'emprunt en fonction du nombre de passagers
        if (passagers == "1")
        {
            tauxEmprunt = Convert.ToString(Convert.ToDouble(tauxEmprunt) + Convert.ToDouble(data.Passagers[0].Taux));
        }
        else if (passagers == "2")
        {
            tauxEmprunt = Convert.ToString(Convert.ToDouble(tauxEmprunt) + Convert.ToDouble(data.Passagers[1].Taux));
        }
        else if (passagers == "3")
        {
            tauxEmprunt = Convert.ToString(Convert.ToDouble(tauxEmprunt) + Convert.ToDouble(data.Passagers[2].Taux));
        }
        else if (passagers == "4")
        {
            tauxEmprunt = Convert.ToString(Convert.ToDouble(tauxEmprunt) + Convert.ToDouble(data.Passagers[3].Taux));
        }

        // Affichage des informations saisies par l'utilisateur
        Console.WriteLine("╔══════════════════╦═════════════════╗");
        Console.WriteLine("║ Type de voiture  ║ " + voiture.PadRight(15) + " ║");
        Console.WriteLine("║ Energie          ║ " + energie.PadRight(15) + " ║");
        Console.WriteLine("║ Kilométrage      ║ " + kilometrage.PadRight(15) + " ║");
        Console.WriteLine("║ Année            ║ " + annee.PadRight(15) + " ║");
        Console.WriteLine("║ Passagers        ║ " + passagers.PadRight(15) + " ║");
        Console.WriteLine("╠══════════════════╬═════════════════╣");
        
        // Affichage du total score du véhicule
        Console.WriteLine("║ Total score      ║ " + scoreVehicule.ToString().PadRight(15) + " ║");
        Console.WriteLine("╚══════════════════╩═════════════════╝");

        // Affichage des notes
        Console.WriteLine("╔══════════════════╦═════════════════╗");
        Console.WriteLine("║ Note type        ║ " + noteType.PadRight(15) + " ║");
        Console.WriteLine("║ Note kilométrage ║ " + noteKilometrage.PadRight(15) + " ║");
        Console.WriteLine("║ Note année       ║ " + noteAnnee.PadRight(15) + " ║");
        Console.WriteLine("║ Note énergie     ║ " + noteEnergie.PadRight(15) + " ║");
        Console.WriteLine("╚══════════════════╩═════════════════╝");

        // Affichage du taux d'emprunt en rouge 
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("╔══════════════════╦═════════════════╗");
        Console.WriteLine("║ Taux d'emprunt   ║ " + tauxEmprunt.PadRight(14) + "% ║");
        Console.WriteLine("╚══════════════════╩═════════════════╝");
        Console.ResetColor();
        //La plupart des commentaires sont faits par Copilot
    }
}
