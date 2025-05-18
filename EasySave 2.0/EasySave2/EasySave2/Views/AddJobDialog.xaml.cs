using System.Windows;
using System.Windows.Controls;
using EasySave2.Models;

namespace EasySave2.Views
{
    public partial class AddJobDialog : Window
    {
        public BackupJob? Result { get; private set; }

        public AddJobDialog(BackupJob? job = null)
        {
            InitializeComponent();

            if (job != null)
            {
                NameInput.Text = job.Name;
                SourceInput.Text = job.SourcePath;
                DestInput.Text = job.DestinationPath;
                ModeSelect.SelectedIndex = job.Mode == "diff" ? 1 : 0;
                EncryptCheckbox.IsChecked = job.UseEncryption;

                if (job.UseEncryption)
                {
                    ExtensionInputPanel.Visibility = Visibility.Visible;
                    ExtensionsInput.Visibility = Visibility.Visible;
                    ExtensionsLabel.Visibility = Visibility.Visible;
                    ExtensionsInput.Text = job.ExtensionsToEncrypt ?? "";
                }

                LogFormatSelect.SelectedIndex = job.LogFormat == "xml" ? 1 : 0;
                SoftwarePackageBox.Text = job.SoftwarePackage ?? "";
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
                UseEncryption = EncryptCheckbox.IsChecked == true,
                LogFormat = ((ComboBoxItem)LogFormatSelect.SelectedItem)?.Content?.ToString()?.ToLower() ?? "json",
                SoftwarePackage = SoftwarePackageBox.Text,
                ExtensionsToEncrypt = EncryptCheckbox.IsChecked == true ? ExtensionsInput.Text.Trim() : "*",
                Status = "pending"
            };

            DialogResult = true;
            Close();
        }

        private void EncryptCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ExtensionInputPanel.Visibility = Visibility.Visible;
            ExtensionsInput.Visibility = Visibility.Visible;
            ExtensionsLabel.Visibility = Visibility.Visible;
        }

        private void EncryptCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ExtensionInputPanel.Visibility = Visibility.Collapsed;
            ExtensionsInput.Visibility = Visibility.Collapsed;
            ExtensionsLabel.Visibility = Visibility.Collapsed;
            ExtensionsInput.Text = string.Empty;
        }
    }
}
