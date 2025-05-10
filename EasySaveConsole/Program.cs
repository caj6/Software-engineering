// Program.cs
using BackupApp.Controllers;

namespace BackupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new BackupController();
            controller.Run();
        }
    }
}
