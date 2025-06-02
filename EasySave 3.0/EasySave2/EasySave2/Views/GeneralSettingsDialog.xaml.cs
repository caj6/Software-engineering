using System.Windows;
using System.Windows.Controls;

namespace EasySave2.Views
{
    public partial class GeneralSettingsDialog : Window
    {
        public GeneralSettingsDialog()
        {
            InitializeComponent();

            LogFormatCombo.SelectedIndex = Settings.GlobalLogFormat == "xml" ? 1 : 0;
            GlobalExtensionsInput.Text = Settings.GlobalEncryptionExtensions;
            LargeFileThresholdInput.Text = Settings.LargeFileThresholdKB.ToString();
            PriorityExtensionsInput.Text = Settings.GlobalPriorityExtensions;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (LogFormatCombo.SelectedItem is ComboBoxItem selected)
                Settings.GlobalLogFormat = selected.Content.ToString()?.ToLower() ?? "json";

            Settings.GlobalEncryptionExtensions = GlobalExtensionsInput.Text.Trim();
            Settings.GlobalPriorityExtensions = PriorityExtensionsInput.Text.Trim();


            if (int.TryParse(LargeFileThresholdInput.Text.Trim(), out int threshold))
                Settings.LargeFileThresholdKB = threshold;

            Settings.Save();

            DialogResult = true;
            Close();
        }
    }
}
