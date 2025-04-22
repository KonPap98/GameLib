using Google.GData.YouTube;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GameLib.View.UserControls
{
    /// <summary>  
    /// Interaction logic for usew.xaml  
    /// </summary>  
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void AddGames_Click(object sender, RoutedEventArgs e)
        {
            var foldersList = new List<string>();
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Game Folder",
                ShowHiddenItems = true,
                Multiselect = true
            };
            folderDialog.ShowDialog();
            var folders = folderDialog.FolderNames;


            foreach (string folder in folders)
            {
                foldersList.Add(folder);
            }

            SearchForGames(foldersList);

        }
        public static List<string> SearchForGames(List<string> folders, bool searchSubfolders = true)
        {
            List<string> games = new List<string>();

            foreach (string folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    try
                    {
                        var files = Directory.GetFiles(folder,
                            "*.*",
                            searchSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                            .Where(file => file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
                        games.AddRange(files);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Trace.WriteLine($"Access denied to folder: {folder} - {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Error accessing folder: {folder} - {ex.Message}");
                    }
                }
            }
            return games;
        }
    }
}
