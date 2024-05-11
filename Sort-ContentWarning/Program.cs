using System.Globalization;
using System.Reflection;

internal class Program
{
    public static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public static readonly string ContentFolder = Directory.CreateDirectory(Path.Combine(Desktop, "Content Warning")).FullName;
    public static readonly string ConWarnFileKind = "content_warning_*.webm";
    public static FileSystemWatcher? FsWatcher = null;

    private static void Main(string[] args)
    {
        WriteHeader();
        if (!args.Any())
        {
            IEnumerable<string> DesktopEnum = Directory.EnumerateFiles(Desktop, ConWarnFileKind);
            if (!DesktopEnum.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No footage file finded in \"{0}\"!", Desktop);
                Console.WriteLine("Exiting...");
                Console.ResetColor();
                return;
            }

            foreach (string ConWarnVid in DesktopEnum)
            {
                FileInfo ConWarnFootage = new FileInfo(ConWarnVid);
                MoveVideoFile(ConWarnFootage);
            }

            return;
        }

        switch (args[0].ToLower())
        {
            case "-a":
            case "--auto":
                {
                    WriteAutoModeNotify();
                    FsWatcher = CreateWatcher();
                    AppDomain.CurrentDomain.ProcessExit += (o, e) => FsWatcher.Dispose();
                    Thread.Sleep(-1);
                    break;
                }

            case "-h":
            case "--help":
                {
                    WriteHelp();
                    break;
                }

            default:
                Console.WriteLine("ERROR : Incorect arguments");
                Console.WriteLine("Type [-h, --help] for showing program information");
                break;
        }
    }

    public static void WriteHeader()
    {
        Console.WriteLine("ContentWarning footage sorter {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        Console.WriteLine("ContentFolder : \"{0}\"\n", ContentFolder);
    }

    public static void WriteAutoModeNotify()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\rAutomatic file watcher mode\n");
        Console.ResetColor();
    }

    public static void WriteHelp()
    {
        Console.WriteLine("Moves all footage made in the game \"Content Warning\" to a separate folder instead of littering the desktop");
        Console.WriteLine("Can be launched with [-a, --auto] flag for automaic moving on footage saving");
        Console.WriteLine("And remember, Snails Sucks!\n");
    }

    public static FileSystemWatcher CreateWatcher()
    {
        FileSystemWatcher watcher = new FileSystemWatcher(Desktop, ConWarnFileKind)
        {
            NotifyFilter = NotifyFilters.FileName,
            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };

        watcher.Created += (o, e) => MoveVideoFile(new FileInfo(e.FullPath));
        return watcher;
    }

    public static void MoveVideoFile(FileInfo Info)
    {
        try
        {
            Info.MoveTo(Path.Combine(ContentFolder, Info.Name), true);
            Console.WriteLine("Moved \"{0}\" to ContentFolder", Info.Name);
        }
        catch (Exception Exc)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to move \"{0}\" to ContentFolder - {1}", Info.Name, Exc.Message);
            Console.ResetColor();
        }
    }
}