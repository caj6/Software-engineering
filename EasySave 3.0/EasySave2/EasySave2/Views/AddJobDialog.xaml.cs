using System.Windows;
using System.Windows.Controls;
using EasySave2.Models;
using MessageBox = System.Windows.MessageBox;


namespace EasySave2.Views
{
    public partial class AddJobDialog : Window
    {
        public BackupJob? Result { get; private set; }
        private bool _isEditMode = false;

        public AddJobDialog(BackupJob? job = null)
        {
            InitializeComponent();

            if (job != null)
            {
                _isEditMode = true;
                NameInput.Text = job.Name;
                SourceInput.Text = job.SourcePath;
                DestInput.Text = job.DestinationPath;
                ModeSelect.SelectedIndex = job.Mode == "diff" ? 1 : 0;
                SoftwarePackageBox.Text = job.SoftwarePackage ?? "";
                PriorityExtensionsInput.Text = job.PriorityExtensions ?? "";

                // disable fields that shouldn't be edited
                ModeSelect.IsEnabled = false;
                SoftwarePackageBox.IsEnabled = false;
                PriorityExtensionsInput.IsEnabled = false;
            }
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameInput.Text) ||
                string.IsNullOrWhiteSpace(SourceInput.Text) ||
                string.IsNullOrWhiteSpace(DestInput.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            Result = new BackupJob
            {
                Name = NameInput.Text,
                SourcePath = SourceInput.Text,
                DestinationPath = DestInput.Text,
                Mode = ((ComboBoxItem)ModeSelect.SelectedItem)?.Content?.ToString()?.ToLower() ?? "full",
                UseEncryption = true, // always true since user chose encryption globally
                LogFormat = Settings.GlobalLogFormat,
                ExtensionsToEncrypt = Settings.GlobalEncryptionExtensions,
                SoftwarePackage = SoftwarePackageBox.Text,
                PriorityExtensions = PriorityExtensionsInput.Text.Trim(),
                Status = "pending"
            };

            DialogResult = true;
            Close();
        }

        private void BrowseSource_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SourceInput.Text = dialog.SelectedPath;
                }
            }
        }

        private void BrowseDestination_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DestInput.Text = dialog.SelectedPath;
                }
            }
        }


    }
}
