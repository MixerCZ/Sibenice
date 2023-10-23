using System;
using System.Collections.Generic;

class HraSibenice
{
    static void Main()
    {
        Start();
    }

    public static List<char> uhadnuto = new List<char>();
    public static List<char> pouziteChars = new List<char>();
    public static List<char> useless = new List<char>();
    public static int pokusy = 0;
    public static bool vyhra = false;

    // Nastavení
    public static string veta = "Čichnova";
    public static int sibeniceOffset = 2;
    public static int allOffsetX = 0;
    public static int allOffsetY = 0;

    // Definice pozic pro vykreslení šibenice
    public static String[] sibenice = { "/\\", "||", "||", "||", "|/", "__", "__", "__", "__", "|", "\\ o /", "/x\\", "|   |" };
    public static int[] sibeniceX = { 0, 0, 0, 0, 0, 0, 2, 4, 6, 7, 5, 6, 5 };
    public static int[] sibeniceY = { 5, 4, 3, 2, 1, 0, 0, 0, 0, 1, 2, 3, 4 };

    // Vykreslí obrázek šibenice - jen pro testovací účely, nepoužívá se
    static void Test()
    {
        for (int i = 0; i < sibenice.Length; i++)
        {
            Console.SetCursorPosition(sibeniceX[i], sibeniceY[i]);
            Console.Write(sibenice[i]);
        }

        Console.ReadLine();
    }

    // Vytvoří úvodní obrazovku a zapne rekurzivní funkci
    static void Start()
    {
        Console.Title = "Hra oběšenec";
        Console.BackgroundColor = System.ConsoleColor.Blue;

        // Nastaví vstupní i výstupní kódování na unicode
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        // Nastaví na střed obrazovky
        allOffsetY = (Console.WindowHeight / 2) - 6;
        allOffsetX = (Console.WindowWidth / 2) - ((17 + veta.Length) / 2);

        // Přidá mezeru, čárku, tečku, vykřičník, otazník mezi zbytečné znaky, aby nebylo potřeba zadávat tyto znaky při hádání vět.
        useless.Add(' ');
        useless.Add('.');
        useless.Add('!');
        useless.Add(',');
        useless.Add('?');
        Console.SetCursorPosition(allOffsetX, allOffsetY);
        Console.Write("Věta k uhádnutí: ");
        for (int i = 0; i < veta.Length; i++)
        {
            Console.Write("_");
        }

        SibeniceGuess(false);
    }


    // Funkce pro vykreslení šibenice a věty na uhádnutí
    static void Vykresleni(bool spatne)
    {
        // Zkontroluje nastavení centrování a případně opraví
        allOffsetY = (Console.WindowHeight / 2) - 6;
        allOffsetX = (Console.WindowWidth / 2) - ((17 + veta.Length) / 2);

        Console.Clear();
        Console.SetCursorPosition(allOffsetX, allOffsetY);
        Console.Write("Věta k uhádnutí: ");
        for (int i = 0; i < veta.Length; i++)
        {
            if (uhadnuto.Contains(veta.ToLower()[i]))
                Console.Write(veta[i]);

            // Vypsání znaků zbytečných na hádání
            else if (useless.Contains(veta.ToLower()[i]) || i == 0 || i == veta.Length - 1)
                Console.Write(veta[i]);

            else
                Console.Write("_");
        }

        // Vykreslí část šibenice podle počtu špatných pokusů
        for (int i = 0; i < pokusy; i++)
        {
            Console.SetCursorPosition(sibeniceX[i] + sibeniceOffset + allOffsetX, sibeniceY[i] + sibeniceOffset + allOffsetY);
            Console.Write(sibenice[i]);
        }

        Console.SetCursorPosition(sibeniceX[0] + allOffsetX, sibeniceY[0] + sibeniceOffset + 2 + allOffsetY);
        Console.Write("Vyzkoušené znaky: ");
        for (int i = 0; i < pouziteChars.Count; i++)
        {
            Console.Write(pouziteChars[i]);
            if (i < pouziteChars.Count - 1) Console.Write(", ");
        }

        if (spatne)
        {
            Console.SetCursorPosition(sibeniceX[0] + allOffsetX, sibeniceY[0] + sibeniceOffset + 4 + allOffsetY);
            Console.WriteLine("Tento znak si už hádal");
        }

        // Prohra - oznámí prohru a ukončí program
        if (pokusy >= sibenice.Length)
        {
            Console.SetCursorPosition(sibeniceX[pokusy - 1] + allOffsetX + sibeniceOffset, sibeniceY[pokusy - 1] + sibeniceOffset + allOffsetY);
            Console.Write(sibenice[pokusy - 1]);
            Console.SetCursorPosition(sibeniceX[0] + allOffsetX, sibeniceY[0] + sibeniceOffset + 3 + allOffsetY);
            Console.WriteLine("Prohrál jsi! Věta zněla: " + veta);
            Console.Read();
            Environment.Exit(0);
        }

        // Oznámí j)výhru a ukončí program
        if (vyhra)
        {
            Console.SetCursorPosition(allOffsetX, allOffsetY + 12);
            Console.WriteLine("Vyhrál jsi!");
            Console.Read();
            Environment.Exit(0);
        }
    }

    // Načítání znaku a zpracování výsledku
    static void SibeniceGuess(bool spatne)
    {
        Vykresleni(spatne);
        Console.SetCursorPosition(allOffsetX, sibeniceY[0] + sibeniceOffset + 3 + allOffsetY);
        char znak = ReadChar("Zadej znak k uhádnutí: ");
        znak = char.ToLower(znak);

        if (uhadnuto.Contains(znak) || pouziteChars.Contains(znak))
        {
            SibeniceGuess(true);
        }

        if (SibeniceTest(znak))
            uhadnuto.Add(znak);
        else
        {
            pouziteChars.Add(znak);
            pokusy++;
        }

        SibeniceGuess(false);
    }

    // Detekce výhry a správného tipu
    static bool SibeniceTest(char znak)
    {
        bool ret = false;
        int remain = 0;

        for (int i = 0; i < veta.Length; i++)
        {
            if (veta.ToLower()[i] == znak)
                ret = true;

            else if (i == 0 || i == veta.Length - 1)
            { }

            else if ((!uhadnuto.Contains(veta.ToLower()[i]) && !useless.Contains(veta.ToLower()[i])))
                remain++;
        }

        // Detekuje výhru
        if (remain == 0)
        {
            vyhra = true;
        }
        return ret;
    }

    static char ReadChar(string text)
    {
        if (text != "") Console.Write(text);
        char charakter;
        while (!char.TryParse(Console.ReadLine(), out charakter))
            if (text != "") Console.Write("Zadaná hodnota není znak typu Char!\n" + text);
            else Console.Write("Zadaná hodnota není znak typu Char!\n" + "Zadej znovu: ");
        return charakter;
    }
}