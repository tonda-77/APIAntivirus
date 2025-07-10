using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Proměnné z druhé části kódu
    static readonly string apiKey = "80e71974b3ea745f99c7c8e0afa28ef345718011332ccca7cb1d8a44608a0609";
    static int selectedOption = 1;
    static bool blinkState = false;

    static async Task Main()
    {

        List<string> usernames = new List<string>();
        List<string> passwords = new List<string>();

        Console.WriteLine("Cuuus, Ja jsem antivirus pro kontrolovani souboru, slozek, disku, a vseho mozneho 😁🙃");
        KlavesaProPokracovani();

        for (bool correctLogin = false; !correctLogin;)
        {
            Console.WriteLine("                                                     ");
            Console.WriteLine("                                                     ");
            Console.WriteLine("Login: 🙄🙄");
            Thread.Sleep(200);

            Console.WriteLine("                                                     ");
            Console.WriteLine("===== Moznosti prihlaseni: =====");
            Console.WriteLine("1.       Prihlasit se ");
            Console.WriteLine("2.       Vytvorit si novy ucet ");
            Console.WriteLine("3.       Ukoncit nejlepsi antivirus vsech dob?!?! 😲😖😭");

            int loginOptionInput;
            int.TryParse(ReadLineWithQuit(), out loginOptionInput);


            switch (loginOptionInput)
            {
                case 1:
                    Console.WriteLine("Zadej sve username pro prihlaseni: ");
                    string loginUsernameInput = ReadLineWithQuit();
                    Console.WriteLine("Zadej sve heslo pro prihlaseni: ");
                    string loginPasswordInput = ReadLineWithQuit();

                    bool usernameFound = false;
                    int index = 0;

                    foreach (string username in usernames)
                    {
                        if (username == loginUsernameInput)
                        {
                            if (passwords[index] == loginPasswordInput)
                            {

                                Console.WriteLine("Prihlaseni uspesne! 😁");
                                Thread.Sleep(1000);
                                correctLogin = true;
                            }
                            else
                            {
                                Console.WriteLine("Spatne heslo, zkus to znovu.. 🤔😂😂");
                            }

                            usernameFound = true;
                            break;
                        }
                        index++;
                    }

                    if (!usernameFound)
                    {
                        Console.WriteLine("Tvuj username neni v seznamu, zkus to znova.. 😂😂");
                        Thread.Sleep(1500);
                        goto case 1;
                    }
                    break;

                case 2:
                    bool registrationComplete = false;
                    while (!registrationComplete)
                    {
                        Console.WriteLine("Oukeeej 😉😂");
                        Thread.Sleep(500);

                        Console.WriteLine("Zadej sve username pro vytvoreni uctu: ");
                        string registrationUsernameInput = ReadLineWithQuit();
                        usernames.Add(registrationUsernameInput);

                        Console.WriteLine("Zadej sve heslo pro vytvoreni uctu: ");
                        string registrationPasswordInput = ReadLineWithQuit();
                        passwords.Add(registrationPasswordInput);
                        Thread.Sleep(700);

                        Console.WriteLine("Taakze, toto je tve username: " + registrationUsernameInput + "  😊");
                        Thread.Sleep(500);
                        Console.WriteLine("A toto je tve heslo: " + registrationPasswordInput + "  😊");
                        Thread.Sleep(500);

                        Console.WriteLine("                                                     ");
                        Console.WriteLine("                                                     ");
                        Console.WriteLine("Je takhle vse v poradku (y/n)? 🤔");
                        Thread.Sleep(200);
                        Console.WriteLine("Ano (y)");
                        Console.WriteLine("Ne (n)");
                        char confirmRegistrationInput;
                        char.TryParse(ReadLineWithQuit(), out confirmRegistrationInput);
                        if (confirmRegistrationInput == 'y')
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("Ukladam.. 🙄");
                            Thread.Sleep(2500);
                            // Kontrola s aktuálním indexem (zkracene poslední přidaný účet)
                            int currentIndex = usernames.Count - 1;
                            if (registrationUsernameInput == usernames[currentIndex] && registrationPasswordInput == passwords[currentIndex])
                            {
                                Console.WriteLine("Vse je v poradku, takze se muzes prihlasit 🤔");
                                Thread.Sleep(1500);
                            }
                            registrationComplete = true; // Ukončí smyčku registrace

                            // Přepne na přihlášení
                            Console.WriteLine("Přepínám na přihlášení.. 🔄");
                            Thread.Sleep(1000);
                            goto case 1;
                        }
                        else if (confirmRegistrationInput == 'n')
                        {
                            Console.WriteLine("Odstranuji neúspěšný účet.. 🗑️");
                            Thread.Sleep(1000);
                            // Smaže neuspesny přidaný účet
                            usernames.RemoveAt(usernames.Count - 1);
                            passwords.RemoveAt(passwords.Count - 1);
                            Console.WriteLine("Ok, zkus to znovu.. 😊");
                            Thread.Sleep(500);
                        }
                    }
                    break;

                case 3:
                    quitApp();
                    break;
                default:
                    Console.WriteLine("Zadana hodnota neni validni, zkus to znovu.. 🤔🤬🤬");
                    Thread.Sleep(1000);
                    goto case 1;
                    break;
            }
        }

        // Po úspěšném přihlášení spustíme VirusTotal scanner
        Console.Clear();
        Console.WriteLine("Vitej v antivirovem skeneru! 🦠🔍");
        Thread.Sleep(1000);
        Console.SetWindowSize(50, 20);
        Console.SetBufferSize(50, 20);
        Console.CursorVisible = false;

        await RunMainLoopAsync();
    }

    static void KlavesaProPokracovani()
    {
        Thread.Sleep(500);
        Console.WriteLine("===== Pro pokracovani stiskni libovolnou klavesu... 🎮 =====");
        Console.ReadKey();
    }

    static string ReadLineWithQuit()
    {
        string input = Console.ReadLine();
        if (input != null)
        {
            string lowerInput = input.ToLower();
            if (lowerInput == "quit" || lowerInput == "exit")
            {
                Console.WriteLine("Ukončuji program.. 👋");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
        return input;
    }

    static void quitApp()
    {
        Console.WriteLine("Neeeeeeeeeeeeee 😖😭😭");
        Thread.Sleep(1500);
        Console.WriteLine("Doopravdy chces ukoncit appku (y/n)?!? 😖😭");

        Thread.Sleep(500);
        Console.WriteLine("Ano (y)");
        Console.WriteLine("Ne (n)");
        char quitGameInput;
        char.TryParse(ReadLineWithQuit(), out quitGameInput);
        switch (quitGameInput)
        {
            case 'y':
                Console.WriteLine("Ukoncuji program.. 😢😭");
                Thread.Sleep(1000);
                Environment.Exit(0);
                break;
            case 'n':
                Console.WriteLine("Skvelee rozhodnuti! 😊");
                Thread.Sleep(1000);
                break;
            default:
                Console.WriteLine("Zadana hodnota neni validni, zkus to znovu.. 🤔🤬🤬");
                Thread.Sleep(1000);
                quitApp();
                break;
        }
    }

    // Funkce z druhé části kódu (VirusTotal scanner)
    static async Task RunMainLoopAsync()
    {
        string frame1option1 = @"
Simple Anti-Virus
┌────────────────────────┐
│ 1. Scan File           │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame2option1 = @"
Simple Anti-Virus
┌────────────────────────┐
│ 1. Scan File◄          │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame1option2 = @"
Simple Anti-Virus
┌────────────────────────┐
│ 1. Scan File           │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame2option2 = @"
Simple Anti-Virus
┌────────────────────────┐
│ 1. Scan File           │
│ 2. Exit◄               │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        while (true)
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow)
                {
                    selectedOption++;
                    if (selectedOption > 2)
                        selectedOption = 1;
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selectedOption--;
                    if (selectedOption < 1)
                        selectedOption = 2;
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (selectedOption == 2)
                    {
                        Console.CursorVisible = true;
                        return;
                    }
                    else if (selectedOption == 1)
                    {
                        Console.CursorVisible = true;
                        await ScanFileAsync();
                        Console.CursorVisible = false;
                    }
                }
            }

            string frameToDraw = selectedOption switch
            {
                1 => blinkState ? frame1option1 : frame2option1,
                2 => blinkState ? frame1option2 : frame2option2,
                _ => frame1option1
            };

            Console.SetCursorPosition(0, 0);
            Console.Write(frameToDraw);

            blinkState = !blinkState;
            Thread.Sleep(100); // To je rychlost te animace lalala 
        }
    }

    static async Task ScanFileAsync()
    {
        Console.Clear();
        Console.WriteLine("Enter full path of file to scan:");
        string path = Console.ReadLine()?.Trim('"');

        if (!File.Exists(path))
        {
            Console.WriteLine("File not found! Press any key to return...");
            Console.ReadKey(true);
            return;
        }

        Console.WriteLine("Uploading file to VirusTotal...");

        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("x-apikey", apiKey);

            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(File.ReadAllBytes(path));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Add(fileContent, "file", Path.GetFileName(path));


            HttpResponseMessage uploadResponse = await client.PostAsync("https://www.virustotal.com/api/v3/files", content);
            uploadResponse.EnsureSuccessStatusCode();

            var json = await uploadResponse.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            string analysisId = doc.RootElement.GetProperty("data").GetProperty("id").GetString();

            Console.WriteLine("File uploaded. Waiting for analysis...");

            bool done = false;
            while (!done)
            {
                await Task.Delay(5000);

                HttpResponseMessage analysisResponse = await client.GetAsync($"https://www.virustotal.com/api/v3/analyses/{analysisId}");
                analysisResponse.EnsureSuccessStatusCode();

                string analysisJson = await analysisResponse.Content.ReadAsStringAsync();
                using JsonDocument analysisDoc = JsonDocument.Parse(analysisJson);

                var status = analysisDoc.RootElement.GetProperty("data").GetProperty("attributes").GetProperty("status").GetString();

                if (status == "completed")
                {
                    done = true;

                    var stats = analysisDoc.RootElement.GetProperty("data").GetProperty("attributes").GetProperty("stats");
                    int malicious = stats.GetProperty("malicious").GetInt32();
                    int suspicious = stats.GetProperty("suspicious").GetInt32();
                    int harmless = stats.GetProperty("harmless").GetInt32();
                    int undetected = stats.GetProperty("undetected").GetInt32();

                    Console.WriteLine("Scan complete:");
                    Console.WriteLine("Malicious: " + malicious);
                    Console.WriteLine("Suspicious: " + suspicious);
                    Console.WriteLine("Harmless: " + harmless);
                    Console.WriteLine("Undetected: " + undetected);
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey(true);
                }
                else
                {
                    Console.WriteLine("Analysis in progress...");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey(true);
        }
    }
}