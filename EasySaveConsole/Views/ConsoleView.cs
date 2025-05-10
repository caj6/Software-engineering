// Views/ConsoleView.cs
using System;
using System.IO;
using System.Text.Json.Nodes;
using BackupApp.Models;
using BackupApp.Services;

namespace BackupApp.Views
{
    public class ConsoleView
    {
        private readonly LanguageService _lang;

        public ConsoleView(LanguageService lang)
        {
            _lang = lang;
        }

        public void ShowMenu()
        {
            Console.WriteLine("------Welcome to easy save software---------");
            Console.WriteLine("======= MENU ======");
            Console.WriteLine("1. " + _lang.Get("menu_add"));
            Console.WriteLine("2. " + _lang.Get("menu_execute"));
            Console.WriteLine("3. " + _lang.Get("menu_delete"));
            Console.WriteLine("4. " + _lang.Get("menu_edit"));
            Console.WriteLine("5. " + _lang.Get("menu_list"));
            Console.WriteLine("6. " + _lang.Get("menu_lang"));
            Console.WriteLine("7. " + _lang.Get("menu_clear"));
            Console.WriteLine("8. " + _lang.Get("menu_exit"));
        }

        public int GetMenuChoice()
        {
            Console.Write(_lang.Get("choose_option"));
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : -1;
        }
        public string GetBackupName()
        {
            Console.Write("Enter a name for this backup job: ");
            return Console.ReadLine()!;
        }

        public string GetSourcePath()
        {
            Console.Write(_lang.Get("enter_source"));
            return Console.ReadLine()!;
        }

        public string GetDestinationPath()
        {
            Console.Write(_lang.Get("enter_destination"));
            return Console.ReadLine()!;
        }
        public void ClearScreen()
        {
            Console.Clear();
        }

        public string GetBackupMode()
        {
            Console.Write(_lang.Get("choose_mode"));
            return Console.ReadLine()!;
        }

        public int GetJobId()
        {
            Console.Write(_lang.Get("enter_id"));
            return int.TryParse(Console.ReadLine(), out int id) ? id : -1;
        }

        public void ShowMessage(string key)
        {
            Console.WriteLine(_lang.Get(key));
        }

        public string GetLogFormat()
        {
            Console.WriteLine("Choose log format:");
            Console.WriteLine("1. JSON");
            Console.Write("Enter choice (1): ");

            string input = Console.ReadLine()!;
            return input == "1" ? "json" : GetLogFormat();  // json logs
        }

        public void ChangeLanguage()
        {
            Console.WriteLine("1. English\n2. Français");
            Console.Write("Select language: ");
            string input = Console.ReadLine()!;
            if (input == "2")
                _lang.SetLanguage(LanguageService.Language.French);
            else
                _lang.SetLanguage(LanguageService.Language.English);
        }

        public void ListJobs(List<BackupJob> jobs)
        {
            if (jobs.Count == 0)
            {
                Console.WriteLine("No backup jobs found.");
                return;
            }

            const int nameWidth = 15, idWidth = 4, srcWidth = 25, dstWidth = 25, modeWidth = 10, statusWidth = 10;

            string divider = "+" + new string('-', nameWidth + 2) +
                             "+" + new string('-', idWidth + 2) +
                             "+" + new string('-', srcWidth + 2) +
                             "+" + new string('-', dstWidth + 2) +
                             "+" + new string('-', modeWidth + 2) +
                             "+" + new string('-', statusWidth + 2) + "+";

            Console.WriteLine();
            Console.WriteLine(divider);
            Console.WriteLine($"| {"Name",-nameWidth} | {"ID",-idWidth} | {"Source",-srcWidth} | {"Destination",-dstWidth} | {"Mode",-modeWidth} | {"Status",-statusWidth} |");
            Console.WriteLine(divider);

            foreach (var job in jobs)
            {
                string name = Truncate(job.Name, nameWidth);
                string src = Truncate(job.SourcePath, srcWidth);
                string dst = Truncate(job.DestinationPath, dstWidth);
                Console.WriteLine($"| {name,-nameWidth} | {job.Id,-idWidth} | {src,-srcWidth} | {dst,-dstWidth} | {job.Mode,-modeWidth} | {job.Status,-statusWidth} |");
            }

            Console.WriteLine(divider);
        }
        private string Truncate(string text, int maxLength)
        {
            return text.Length <= maxLength ? text : text[..(maxLength - 3)] + "...";
        }
    }
}
