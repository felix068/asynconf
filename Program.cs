using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class TUI
{
    private string[] options;
    private int cursorPosition;
    private string title;

    public TUI(string title, string[] options)
    { // Constructeur de la classe
        this.title = title;
        this.options = options;
        this.cursorPosition = 0;
    }

    public void Display()
    { // Affiche le menu
        Console.Clear();
        int consoleWidth = Console.WindowWidth;
        int consoleHeight = Console.WindowHeight;

        DisplayTitle(title);

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
            int padding = (consoleWidth - options[i].Length) / 2;
            Console.SetCursorPosition(padding, i + 2);
            Console.Write(options[i]);
            if (i == cursorPosition)
            {
                Console.SetCursorPosition(padding - 2, i + 2);
                Console.Write(">");
            }
            Console.ResetColor();
        }
    }

    public void DisplayTitle(string title)
    { // Affiche le titre
        int consoleWidth = Console.WindowWidth;
        Console.SetCursorPosition((consoleWidth - title.Length) / 2, 0);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    public string Run()
    { // Lance le menu et retourne l'option choisie
        Display();

        // Boucle infinie pour gérer les touches
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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

        // Affichage des informations saisies par l'utilisateur
        Console.WriteLine("Type de voiture : " + voiture);
        Console.WriteLine("Energie : " + energie);
        Console.WriteLine("Kilométrage : " + kilometrage);
        Console.WriteLine("Année : " + annee);
        Console.WriteLine("Nombre de passagers : " + passagers);
        Console.WriteLine("---------------------------------");

        int typeIndex = Array.IndexOf(typeOptions, voiture);
        int kilometrageIndex = Array.IndexOf(kilometrageOptions, kilometrage);
        int anneeIndex = Array.IndexOf(anneeOptions, annee);
        int energieIndex = Array.IndexOf(energieOptions, energie);

        // Affichage des notes
        Console.WriteLine("Note type : " + data.Type[typeIndex].Note);
        Console.WriteLine("Note energie : " + data.Energie[energieIndex].Note);
        Console.WriteLine("Note kilometrage : " + data.Kilometrage[kilometrageIndex].Note);
        Console.WriteLine("Note annee : " + data.Annee[anneeIndex].Note);

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

        // Affichage du score du véhicule
        Console.WriteLine("Score du véhicule : " + scoreVehicule + "/40");

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

        // Affichage du taux d'emprunt en rouge 
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Taux d'emprunt : " + tauxEmprunt + "%");
        Console.ResetColor();
    }
}
