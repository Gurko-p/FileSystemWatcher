using System;
using System.IO;
using System.Security.Permissions;

namespace ConsoleApp9
{
    class Program
    {
        static void Main(string[] args)
        {
            Watch(@"E:\Test", "*.txt", true);
            Console.Read();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Watch(string path, string filter, bool includeSuDirectories)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path, filter);
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.Error += new ErrorEventHandler(OnError);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            watcher.IncludeSubdirectories = includeSuDirectories;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.Name + " was " + e.ChangeType);
            using (TextWriter writer = File.AppendText("E:\\text.txt"))
            {
                writer.WriteLine("File: " + e.Name + " was " + e.ChangeType);
            }
            Console.Out.WriteLine("Информация записана в файл!");
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldName, e.Name);
            using (TextWriter writer = File.AppendText("E:\\text.txt"))
            {
                writer.WriteLine(e.OldName + " was " + e.ChangeType + " to " + e.Name);
            }
            Console.Out.WriteLine("Информация записана в файл!");
        }
        private static void OnError(object source, ErrorEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("Error {0}", e.GetException().Message);
            using (TextWriter writer = File.AppendText("E:\\text.txt"))
            {
                writer.WriteLine("Возникла ошибка {0}", e.GetException().Message);
            }
            Console.Out.WriteLine("Информация записана в файл!");
        }
    }
}
