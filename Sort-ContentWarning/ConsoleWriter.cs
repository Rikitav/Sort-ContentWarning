using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SortContentWarning
{
    public static class ConsoleWrite
    {
        public static void Header(string ContentFolder)
        {
            Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("ContentWarning footage sorter {0}", AssemblyVersion.ToString());
            Console.WriteLine("ContentFolder : \"{0}\"\n", ContentFolder);
        }

        public static void Notify(string Description)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Description);
            Console.ResetColor();
        }

        public static void Error(string Name, string Description)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR : {0}", Name);
            Console.WriteLine(Description);
            Console.ResetColor();
        }

        public static void Help()
        {
            Console.WriteLine("Moves all footage made in the game \"Content Warning\" to a separate folder instead of littering the desktop.");
            Console.WriteLine();
            Console.WriteLine("\t[-a, --auto]\t\t- automaic moving on footage saving.");
            Console.WriteLine("\t[-d, --dir] \t\t- change saving direcotry. You need to specify the path to the new directory as the second parameter.");
            Console.WriteLine();
            Console.WriteLine("And remember, Snails Sucks!\n");
        }
    }
}
