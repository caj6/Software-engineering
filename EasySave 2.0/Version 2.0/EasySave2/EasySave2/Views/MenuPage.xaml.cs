using EasySave2.Models;
using EasySave2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EasySave2.Views
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.AddJob();
            }
        }
        private void ExecuteJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.ExecuteJob();
            }
        }
        private void DeleteJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
                vm.DeleteJob();
        }

        private void EditJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
                vm.EditJob();
        }

        private void JobGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.SelectedJobs.Clear();
                foreach (var item in JobGrid.SelectedItems)
                {
                    if (item is BackupJob job)
                        vm.SelectedJobs.Add(job);
                }
            }
        }




    }
}
