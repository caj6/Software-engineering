
using System.Collections.Generic;

namespace BackupApp.Services
{
    public class LanguageService
    {
        public enum Language { English, French }
        private Language _currentLanguage = Language.English;

        private readonly Dictionary<string, (string en, string fr)> _translations = new()
        {
            ["menu_add"] = ("Add backup job", "Ajouter une tâche de sauvegarde"),
            ["menu_execute"] = ("Execute backup job", "Exécuter une tâche de sauvegarde"),
            ["menu_delete"] = ("Delete backup job", "Supprimer une tâche de sauvegarde"),
            ["menu_edit"] = ("Edit backup job", "Modifier une tâche de sauvegarde"),
            ["menu_list"] = ("List all backup jobs", "Lister toutes les tâches"),
            ["menu_lang"] = ("Change language", "Changer de langue"),
            ["menu_exit"] = ("Exit", "Quitter"),
            ["choose_option"] = ("Choose an option: ", "Choisissez une option : "),
            ["enter_source"] = ("Enter the source folder path: ", "Entrez le chemin du dossier source : "),
            ["enter_destination"] = ("Enter the destination folder path: ", "Entrez le chemin du dossier de destination : "),
            ["choose_mode"] = ("Choose backup mode ('full' or 'diff'): ", "Choisissez le mode de sauvegarde ('full' ou 'diff') : "),
            ["enter_id"] = ("Enter backup job ID: ", "Entrez l'ID de la tâche : "),
            ["invalid_option"] = ("Invalid option. Try again.", "Option invalide. Réessayez."),
            ["job_not_found"] = ("Job not found.", "Tâche non trouvée."),
            ["job_added"] = ("Backup job added.", "Tâche de sauvegarde ajoutée."),
            ["job_deleted"] = ("Backup job deleted.", "Tâche de sauvegarde supprimée."),
            ["job_edited"] = ("Backup job updated.", "Tâche de sauvegarde mise à jour."),
            ["job_done"] = ("Backup completed.", "Sauvegarde terminée.")
        };

        public void SetLanguage(Language lang)
        { 
            _currentLanguage = lang;
        }

        public string Get(string key)
        {
            return _translations.TryGetValue(key, out var pair)
                ? (_currentLanguage == Language.English ? pair.en : pair.fr)
                : key;
        }

        public Language CurrentLanguage => _currentLanguage;
    }
}
