using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EasySave2.Services;

namespace EasySave2.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void GoToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MenuPage());
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem selected)
            {
                string tag = selected.Tag?.ToString();
                if (tag == "fr")
                    LanguageService.SetLanguage(LanguageService.SupportedLanguage.French);
                else
                    LanguageService.SetLanguage(LanguageService.SupportedLanguage.English);
            }
        }
    }
}
