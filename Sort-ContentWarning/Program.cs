using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SortContentWarning
{
    internal class Program
    {
        public static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string ConWarnFileKind = "content_warning_*.webm";

        public static string ContentFolder;
        public static readonly string ContentFolderVar = "ContentWarning_FootageDir";

        private static void Main(string[] args)
        {
            ContentFolder = Environment.GetEnvironmentVariable(ContentFolderVar, EnvironmentVariableTarget.User);
            if (ContentFolder == null)
                ContentFolder = Directory.CreateDirectory(Path.Combine(Desktop, "Content Warning")).FullName;

            ConsoleWrite.Header(ContentFolder);
            if (args.Length == 0)
            {
                if (!Directory.EnumerateFiles(Desktop, ConWarnFileKind).Any())
                {
                    ConsoleWrite.Error("No footage files finded", "Exiting...");
                    return;
                }

                foreach (string ConWarnVid in Directory.EnumerateFiles(Desktop, ConWarnFileKind))
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
                        ConsoleWrite.Notify("\rAutomatic file watcher mode\n");
                        FileSystemWatcher FsWatcher = CreateWatcher();
                        AppDomain.CurrentDomain.ProcessExit += (o, e) => FsWatcher.Dispose();
                        Thread.Sleep(-1);
                        return;
                    }

                case "-d":
                case "--dir":
                    {
                        if (args.Length == 1)
                        {
                            ConsoleWrite.Error("No path", "You need to specify the path to the new directory as the second parameter.");
                            return;
                        }

                        string NewPath = Regex.Replace(args[1], "%(.+?)%", (x) =>
                        {
                            string? NewValue = Environment.GetEnvironmentVariable(x.Groups[1].Value);
                            return NewValue ?? x.Value;
                        });

                        if (!Directory.Exists(NewPath))
                        {
                            ConsoleWrite.Error("Incorect path format", "The path must point to an existing directory");
                            return;
                        }

                        if (!Path.IsPathFullyQualified(NewPath))
                        {
                            ConsoleWrite.Error("Incorect path format", "The path must not be relative");
                            return;
                        }

                        ConsoleWrite.Notify(string.Format("New saving path is \"{0}\"", NewPath));
                        Environment.SetEnvironmentVariable(ContentFolderVar, NewPath, EnvironmentVariableTarget.User);
                        return;
                    }

                case "-h":
                case "--help":
                    {
                        ConsoleWrite.Help();
                        return;
                    }

                default:
                    ConsoleWrite.Error("Incorect arguments", "Type [-h, --help] for showing program information");
                    return;
            }
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
                ConsoleWrite.Error("Failed to move", string.Format("\"{0}\". Exception - \"{1}\"", Info.Name, Exc.Message));
                Console.ResetColor();
            }
        }
    }
}