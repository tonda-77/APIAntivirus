using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class Program
{
    // DPI funkce pro ostré dialogy
    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    // Proměnné z druhé části kódu
    static readonly string apiKey = "e25b72fccd3414fd7e66de6ac46c94e4da211414a4504d678e9a7c07aa62edc1"; // API Key - old: 80e71974b3ea745f99c7c8e0afa28ef345718011332ccca7cb1d8a44608a0609

    // Všechny cesty k souborům a složkám v projektu (AV = Anti-Virus)
    static readonly string avLogsFolderPath = Path.Combine("..", "..", "..", "AV_Logs");
    static readonly string accountsFolderPath = Path.Combine(avLogsFolderPath, "accounts");
    static readonly string logFolderPath = Path.Combine(avLogsFolderPath, "logs");
    static readonly string accountsFilePath = Path.Combine(accountsFolderPath, "Accounts.txt");

    static int selectedOption = 1;
    static bool blinkState = false;

    // Do proměnné se ulozi velikost okna a pred ukoncenim programu se znovu pouzije
    static int originalWidth = Console.WindowWidth; //uprava pormene na static aby se dala pouzit v main - David
    static int originalHeight = Console.WindowHeight;

    [STAThread]
    static async Task Main() // Login system
    {
        // DPI pro ostry dialogy
        if (Environment.OSVersion.Version.Major >= 6) 
        {
            SetProcessDPIAware();
        }
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Nastavení pro zobrazení emoji
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        // Ensure AV_Logs folder, accounts folder and logs folder exist
        Directory.CreateDirectory(avLogsFolderPath);
        Directory.CreateDirectory(accountsFolderPath);
        Directory.CreateDirectory(logFolderPath);

        // Lists for usernames and passwords - Login lists
        List<string> usernames = new List<string>();
        List<string> passwords = new List<string>();

        // Load existing accounts from individual files
        if (Directory.Exists(accountsFolderPath))
        {
            try
            {
                var accountFiles = Directory.GetFiles(accountsFolderPath, "Account_*.txt");
                foreach (var file in accountFiles)
                {
                    var lines = File.ReadAllLines(file);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Username: "))
                        {
                            usernames.Add(lines[i].Substring("Username: ".Length));
                        }
                        else if (lines[i].StartsWith("Password: "))
                        {
                            passwords.Add(lines[i].Substring("Password: ".Length));
                        }
                    }
                }
            }
            catch
            {
                // If error reading accounts, just continue with empty lists
            }
        }

        Console.WriteLine("                             y                        ");
        Console.WriteLine("                                                     ");
        Console.Clear();
        Console.WriteLine("Cuus, Ja jsem antivirus pro kontrolovani souboru, slozek, disku, a vseho mozneho 😁🙃");
        KlavesaProPokracovani();

        // Forloop pro login - dokud se uživatel nepřihlásí správně, tak to loopuje
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
            Console.WriteLine("                                                     ");

            int loginOptionInput;
            int.TryParse(ReadLineWithQuit(), out loginOptionInput);

            switch (loginOptionInput)
            {
                case 1:
                    Console.WriteLine("Zadej sve username pro prihlaseni: 👇");
                    string loginUsernameInput = ReadLineWithQuit();
                    Console.WriteLine("Zadej sve heslo pro prihlaseni: ");
                    string loginPasswordInput = ReadLineWithQuit();

                    // Hodnota urcuje jestli se naslo zadane username v listu "usernames"
                    bool usernameFound = false;
                    int index = 0;  // Index pro hesla, aby to bralo index stejny jako zadal user u username

                    foreach (string username in usernames)
                    {
                        if (username == loginUsernameInput)
                        {
                            if (passwords[index] == loginPasswordInput)
                            {
                                Console.WriteLine("                                                     ");
                                Console.WriteLine("Prihlaseni uspesne! 😁");
                                Thread.Sleep(1000);
                                correctLogin = true;
                            }
                            else
                            {
                                Console.WriteLine("                                                     ");
                                Console.WriteLine("Spatne heslo, zkus to znovu.. 🤔😂😂");
                            }

                            usernameFound = true;
                            break;
                        }
                        index++;
                    }

                    // Pokud nenajde username v listu tak se to vrati na case 1
                    if (!usernameFound)
                    {
                        Console.WriteLine("Tvuj username neni v seznamu, zkus to znova.. 😂😂");
                        Console.WriteLine("                                                     ");
                        Thread.Sleep(2000);
                        Console.Clear();
                        goto case 1;
                    }
                    break;

                case 2:
                    bool registrationComplete = false;
                    while (!registrationComplete)
                    {
                        Console.WriteLine("                                                     ");
                        Console.WriteLine("Oukeeej 😉😂");
                        Console.WriteLine("                                                     ");
                        Thread.Sleep(500);

                        Console.WriteLine("Zadej sve username pro vytvoreni uctu: ");
                        string registrationUsernameInput = ReadLineWithQuit();

                        Console.WriteLine("Zadej sve heslo pro vytvoreni uctu: ");
                        string registrationPasswordInput = ReadLineWithQuit();

                        // Before adding, check if username already exists
                        if (usernames.Contains(registrationUsernameInput))
                        {
                            Console.WriteLine("Toto username jiz existuje, zkus to jine. 😊");
                            Thread.Sleep(1000);
                            Console.Clear();
                            continue; // Restart registration loop
                        }

                        usernames.Add(registrationUsernameInput);
                        passwords.Add(registrationPasswordInput);
                        Thread.Sleep(700);

                    chechIfUserAcceptRegistration:

                        Console.Clear();
                        Console.WriteLine("                                                     ");
                        Console.WriteLine("Taakze, toto je tve username: " + registrationUsernameInput + "  😊"); // Vypise jmeno a heslo ktere user zadal
                        Thread.Sleep(500);
                        Console.WriteLine("A toto je tve heslo: " + registrationPasswordInput + "  😊");
                        Thread.Sleep(500);

                        Console.WriteLine("                                                     ");
                        Console.WriteLine("Je takhle vse v poradku (y/n)? 🤔");
                        Thread.Sleep(200);
                        Console.WriteLine("Ano (y)");
                        Console.WriteLine("Ne (n)");
                        char confirmRegistrationInput;

                        Console.WriteLine("                                                     ");
                        char.TryParse(ReadLineWithQuit(), out confirmRegistrationInput);
                        if (confirmRegistrationInput == 'y')
                        {
                            Thread.Sleep(1000);
                            Console.Clear();
                            Console.WriteLine("                                                     ");
                            Console.WriteLine("Ukladam.. 🙄");
                            Thread.Sleep(2000);
                            // Kontrola s aktuálním indexem (zkracene poslední přidaný účet)
                            int currentIndex = usernames.Count - 1;
                            if (registrationUsernameInput == usernames[currentIndex] && registrationPasswordInput == passwords[currentIndex])
                            {
                                Console.WriteLine("                                                     ");
                                Console.WriteLine("Vse je v poradku, takze se muzes prihlasit 🤔");
                                Thread.Sleep(800);

                                // Save accounts to text files
                                SaveAccountsToFiles(usernames, passwords);
                            }
                            registrationComplete = true; // Ukončí forloop registrace

                            // Přepne na přihlášení
                            Console.WriteLine("Přepínám na přihlášení.. 🔄");
                            Thread.Sleep(500);
                            Console.Clear();
                            Console.WriteLine("                                                     ");
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
                            goto case 2;
                        }
                        else
                        {
                            Console.WriteLine("Zadana hodnota neni validni, zkus to znovu.. 🤔🤬🤬");
                            Thread.Sleep(1000);
                            Console.Clear();
                            goto chechIfUserAcceptRegistration;
                        }
                    }
                    break;

                case 3:
                    quitApp();
                    break;
                default:
                    Console.WriteLine("Zadana hodnota neni validni, zkus to znovu.. 🤔🤬🤬");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }
        }

        Console.Clear();
        Console.WriteLine("Vitej v antivirovem skeneru! 🦠🔍");
        Console.WriteLine("                                                     ");
        Console.WriteLine("                                                     ");
        Thread.Sleep(1000);

        Directory.CreateDirectory(logFolderPath);
        // Nastavíme velikost konzole pro pěkné zobrazení menu
        Console.SetWindowSize(50, 20);
        Console.SetBufferSize(50, 20);
        Console.CursorVisible = false;

        await RunMainLoopAsync();
    }

    // Save each account to separate file (1 account = 1 file)
    static void SaveAccountsToFiles(List<string> usernames, List<string> passwords)
    {
        for (int i = 0; i < usernames.Count; i++)
        {
            string individualFilePath = Path.Combine(accountsFolderPath, $"Account_{i + 1}.txt");
            var accountSb = new StringBuilder();
            accountSb.AppendLine($"Account {i + 1}.");
            accountSb.AppendLine($"Username: {usernames[i]}");
            accountSb.AppendLine($"Password: {passwords[i]}");
            File.WriteAllText(individualFilePath, accountSb.ToString());
        }
    }

    static void KlavesaProPokracovani()
    {
        Thread.Sleep(500);
        Console.WriteLine("===== Pro pokracovani stiskni libovolnou klavesu... 🎮 =====");
        Console.ReadKey();
    }

    static string ReadLineWithQuit()
    {
        // Tato funkce čte vstup od uživatele a zároveň kontroluje jestli nechce odejít
        string input = Console.ReadLine();
        if (input != null)
        {
            string lowerInput = input.ToLower();
            if (lowerInput == "quit" || lowerInput == "exit")
            {
                Console.WriteLine("Ukončuji program.. 👋");
                Thread.Sleep(1000);
                Console.Clear();
                Console.SetWindowSize(originalWidth, originalHeight);
                Console.SetBufferSize(originalWidth, originalHeight);
                Environment.Exit(0);
            }
        }
        return input;
    }

    static void quitApp() // NOO NOO QUIT APP TUNG TUNG TUNG SAHUR..
    {
        Console.Clear();
        Console.WriteLine("                                                     ");
        Console.WriteLine("                                                     ");
        Console.WriteLine("Neeeeeeeeeeeeee 😖😭");
        Thread.Sleep(1500);
        Console.WriteLine("                                                     ");
        Console.WriteLine("Doopravdy chces ukoncit appku (y/n)?!? 😖");

        Thread.Sleep(500);
        Console.WriteLine("Ano (y)");
        Console.WriteLine("Ne (n)");
        Console.WriteLine("                                                     ");
        char quitGameInput;
        char.TryParse(ReadLineWithQuit(), out quitGameInput);
        switch (quitGameInput)
        {
            case 'y':
                Console.WriteLine("Ukoncuji program.. 👋");
                Thread.Sleep(1000);
                Console.Clear();
                Console.SetWindowSize(originalWidth, originalHeight);
                Console.SetBufferSize(originalWidth, originalHeight);
                Environment.Exit(0);
                break;
            case 'n':
                Console.WriteLine("Skvelee rozhodnuti! 😊");
                Thread.Sleep(1000);
                break;
            default:
                Console.WriteLine("Zadana hodnota neni validni, zkus to znovu.. 🤔🤬🤬");
                Thread.Sleep(1000);
                Console.Clear();
                quitApp();
                break;
        }
    }

    // Funkce z druhé části kódu (Tohle je ten hlavní anti virus haha)
    static async Task RunMainLoopAsync()
    {
        string frame1option1 = @"
API Antivirus: 
┌────────────────────────┐
│ 1. Skenovat soubor     │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame2option1 = @"
API Antivirus: 
┌────────────────────────┐
│ 1. Skenovat soubor◄    │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame1option2 = @"
API Antivirus: 
┌────────────────────────┐
│ 1. Skenovat soubor     │
│ 2. Exit                │
│                        │
│                        │
│                        │
│                        │
│                        │
└────────────────────────┘";

        string frame2option2 = @"
API Antivirus: 
┌────────────────────────┐
│ 1. Skenovat soubor     │
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
                    Console.Clear();

                    if (selectedOption == 2)
                    {
                        Console.CursorVisible = true;
                        Console.WriteLine("Ukončuji program.. 👋");
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.SetWindowSize(originalWidth, originalHeight);
                        Console.SetBufferSize(originalWidth, originalHeight);
                        return;
                    }
                    else if (selectedOption == 1)
                    {
                        Console.CursorVisible = true;
                        await ScanFileAsync();
                        Console.CursorVisible = false;
                        Console.Clear();
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
            Thread.Sleep(100);
        }
    }

    // Otevře Windows dialog pro výběr souboru
    static string OpenFileDialog()
    {
        string result = string.Empty;
        
        Thread thread = new Thread(() =>
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Vyber soubor pro skenování";
                dialog.Filter = "Všechny soubory (*.*)|*.*";
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                
                if (dialog.ShowDialog() == DialogResult.OK)
                    result = dialog.FileName;
            }
        });
        
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        
        return result;
    }

    static async Task ScanFileAsync()
    {
        Console.Clear();
        Console.WriteLine("===== VÝBĚR SOUBORU PRO SKENOVÁNÍ =====");
        Console.WriteLine();
        Console.WriteLine("Otevirám Windows průzkumník... 📁");
        
        // Otevře dialog pro výběr souboru
        string path = OpenFileDialog();
        
        // Počká a vyčistí obrazovku
        Thread.Sleep(500);
        Console.Clear();
        
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("❌ Žádný soubor nebyl vybrán.");
            Console.WriteLine("Stiskni libovolnou klávesu pro návrat do menu..");
            Console.ReadKey(true);
            return;
        }

        // Kontrola existence souboru

        if (!File.Exists(path))
        {
            Console.WriteLine("❌ Soubor nenalezen!");
            Console.WriteLine($"Cesta: {path}");
            Console.WriteLine("Stiskni libovolnou klávesu pro návrat do menu..");
            Console.ReadKey(true);
            return;
        }
        
        Console.WriteLine($"✅ Soubor vybrán: {Path.GetFileName(path)}");

        Console.WriteLine("Nahravani souboru do VirusTotal..");

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

            Console.WriteLine("Soubor nahran. Cekani na analyzu..");

            int progressCounter = 1;
            bool done = false;

            int progressLine = Console.CursorTop;
            string initialProgressText = $"Probiha analyza.. ({progressCounter}x)";
            Console.WriteLine(initialProgressText);

            int maxProgressLength = initialProgressText.Length;

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
                    break;
                }

                progressCounter++;
                string progressText = progressCounter <= 10
                    ? $"Probiha analyza.. ({progressCounter}x)"
                    : $"Probiha analyza.. ({progressCounter}x. Omlouvame se pokud to trva uz moc dlouho, API na tom pracuje ;))";

                int padLength = Math.Max(progressText.Length, maxProgressLength);
                maxProgressLength = padLength;

                Console.SetCursorPosition(0, progressLine);
                Console.Write(progressText.PadRight(padLength));
            }

            var stats = JsonDocument.Parse(await client.GetStringAsync($"https://www.virustotal.com/api/v3/analyses/{analysisId}"))
                .RootElement.GetProperty("data").GetProperty("attributes").GetProperty("stats");

            int malicious = stats.GetProperty("malicious").GetInt32();
            int suspicious = stats.GetProperty("suspicious").GetInt32();
            int harmless = stats.GetProperty("harmless").GetInt32();
            int undetected = stats.GetProperty("undetected").GetInt32();

            string resultText = $@"
Sken dokoncen:
Soubor: {Path.GetFileName(path)}
Skodlive: {malicious}
Podezrele: {suspicious}
Neskodne: {harmless}
Nedetekovano: {undetected}
Cas skenu: {DateTime.Now}
";

            Console.SetCursorPosition(0, progressLine + 2);
            Console.WriteLine(resultText);
            Console.WriteLine("Chcete ulozit vysledky do logu? (y/n)");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
            {
                string fileName = $"scan_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string fullPath = Path.Combine(logFolderPath, fileName);
                File.WriteAllText(fullPath, resultText);

                Console.WriteLine($"\nVysledky logu v: {fullPath}");
            }
            else
            {
                Console.WriteLine("\nVysledky nebyly logovany.");
            }

            Console.WriteLine("\nStiskni libovolnou klavesu pro navraceni do menu..");
            Console.ReadKey(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
            Console.WriteLine("Stiskni libovolnou klavesu pro navraceni..");
            Console.ReadKey(true);
        }
    }
}
