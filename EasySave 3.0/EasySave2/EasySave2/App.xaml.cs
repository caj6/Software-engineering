using System;
using System.Threading;
using System.Windows;
using EasySave2.ViewModels;
using EasySave2.Services;
using Application = System.Windows.Application;

namespace EasySave2
{
    public partial class App : Application
    {
        private const string MutexName = "EasySaveServer_SingleInstanceMutex";
        private static Mutex _mutex;

        private SocketServer? _socketServer;
        public static MenuViewModel SharedViewModel { get; private set; } = new MenuViewModel();

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                System.Windows.MessageBox.Show("Another instance of EasySave Remote Console is already running.",
                    "Instance Already Running",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Current.Shutdown();
                return;
            }

            // Load any settings
            Settings.Load();

            // socket server with the shared MenuViewModel
            int port = 9000;
            _socketServer = new SocketServer(port, SharedViewModel);
            _socketServer.Start();

            System.Diagnostics.Debug.WriteLine("✅ Socket Server started and using shared MenuViewModel.");

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
