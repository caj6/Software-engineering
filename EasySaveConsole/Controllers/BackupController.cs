// Controllers/BackupController.cs
using BackupApp.Models;
using BackupApp.Views;
using BackupApp.Services;
using System;

namespace BackupApp.Controllers
{
    public class BackupController
    {
        private readonly ConsoleView _view;
        private readonly BackupManager _manager;
        private readonly LanguageService _lang;

        public BackupController()
        {
            _lang = new LanguageService();
            _view = new ConsoleView(_lang);
            _manager = new BackupManager();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                _view.ShowMenu();
                int choice = _view.GetMenuChoice();

                switch (choice)
                {
                    case 1:
                        string name = _view.GetBackupName();
                        string src = _view.GetSourcePath();
                        string dest = _view.GetDestinationPath();
                        string mode = _view.GetBackupMode();
                        string logFormat = _view.GetLogFormat();
                        _manager.AddJob(name, src, dest, mode, logFormat);
                        _view.ShowMessage("job_added");
                        break;

                    case 2:
                        var jobs = _manager.GetAllJobs();
                        if (jobs.Count == 0)
                        {
                            _view.ShowMessage("job_not_found");
                            break;
                        }

                        _view.ListJobs(jobs);

                        int execId = _view.GetJobId();
                        bool success = _manager.ExecuteJob(execId);
                        _view.ShowMessage(success ? "job_done" : "job_not_found");
                        break;

                    case 3:
                        {
                            int id = _view.GetJobId();
                            _manager.DeleteJob(id);
                            _view.ShowMessage("job_deleted");
                            break;
                        }

                    case 4:
                        {
                            int id = _view.GetJobId();
                            var editJob = _manager.GetJobById(id);
                            if (editJob == null)
                            {
                                _view.ShowMessage("job_not_found");
                                break;
                            }
                            editJob.Mode = _view.GetBackupMode();
                            _view.ShowMessage("job_edited");
                            break;
                        }

                    case 5:
                        _view.ListJobs(_manager.GetAllJobs());
                        break;

                    case 6:
                        _view.ChangeLanguage();
                        break;

                    case 7:
                        _view.ClearScreen();
                        break;

                    case 8:
                        running = false;
                        break;

                    default:
                        _view.ShowMessage("invalid_option");
                        break;
                }
            }
        }
    }
}
