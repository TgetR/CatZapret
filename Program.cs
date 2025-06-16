using System.Net;
using System.IO.Compression;
using System.Diagnostics;
class Program()
{
    static string zipUrl = "https://github.com/Flowseal/zapret-discord-youtube/archive/refs/heads/main.zip";
    static string localZipPath = Path.Combine(Environment.CurrentDirectory, "repo.zip");
    static string extractPath = Path.Combine(Environment.CurrentDirectory, "Zapret_Files");

    public static void Main(string[] args)
    {
        //Start. Check directory.
        Console.WriteLine("WARNING!");
        Console.WriteLine("This application creates some files in the folder it is in. Please put it in a separate folder before you start.");
        Console.WriteLine("For start press any button...");
        Console.ReadLine();
        if (Directory.Exists(extractPath))
        {
            Console.WriteLine("Directory alredy exist. Deleting...");

            try
            {
                ForceUnlockFiles(extractPath);
                Directory.Delete(extractPath, true);
                Console.WriteLine("Directory successfuly deleted!");
            }

            catch (Exception ex)
            {
                //Print error message
                Console.WriteLine($"Some problems: {ex.Message}");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        //Clone repository from GitHub
        Console.WriteLine("Start downloading zip...");
        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(zipUrl, localZipPath);
            }
            Console.WriteLine("ZIP succesfully downloaded. Extarcting...");
            ZipFile.ExtractToDirectory(localZipPath, extractPath);
            Console.WriteLine("Succesfully extracted.");
        }
        catch (Exception ex)
        {
            //Print error message
            Console.WriteLine($"Error: {ex.Message}");
            Console.ReadLine();
            Environment.Exit(1);
        }

        //Starting bat file on zapret
        Console.WriteLine("Starting service.bat...");
        try
        {
            string batPath = Path.Combine(extractPath + @"\zapret-discord-youtube-main", "service.bat");

            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = batPath,
                WorkingDirectory = extractPath + @"\zapret-discord-youtube-main",
                UseShellExecute = true
            };
            process.Start();

            Console.WriteLine("Start successful! Goodbye!");
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
    static void ForceUnlockFiles(string dir)
    {
    foreach (string file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
    {
        try
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        catch { }
    }
    }
}

