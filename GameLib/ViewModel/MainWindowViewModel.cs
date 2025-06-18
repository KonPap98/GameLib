using GameLib.Model;
using GameLib.MVVM;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
namespace GameLib.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Games> Games { get; set; }

    //public RelayCommand AddCommand => new RelayCommand(execute => AddGame());
    public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteGame(), canExecute => selectedGame != null);
    public RelayCommand ScanCommand => new RelayCommand(execute => ScanGames());

    public MainWindowViewModel()
    {
        Games = new ObservableCollection<Games>();
    }

    private Games selectedGame;
    public Games SelectedGame
    {
        get { return selectedGame; }
        set
        {
            selectedGame = value;
            OnPropertyChanged();
        }
    }

    private void AddGame(string Name, long Size, DateTime CreationDate)
    {
        Games.Add(new Games
        {
            Name = Name,
            Size = Size,
            CreationDate = CreationDate
        });
    }

    private void DeleteGame()
    {
        Games.Remove(SelectedGame);
    }


    //Command to scan selcted fodlers for games
    public void ScanGames(bool searchSubfolders = true)
    {
        var dialog = new OpenFolderDialog()
        {
            ShowHiddenItems = true,
            Multiselect = true,
            Title = "Select Games folders",
            DereferenceLinks = false
        };

        dialog.ShowDialog();
        var selectedFolders = dialog.FolderNames;

        List<string> gameFiles = new List<string>();

        foreach (string folder in selectedFolders)
        {
            if (Directory.Exists(folder))
            {
                try
                {
                    var files = Directory
                        .GetFiles(folder, "*.*", searchSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                        .Where(file => file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                                       file.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase));
                    gameFiles.AddRange(files);
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

        foreach (var gamePath in gameFiles)
        {
            FileInfo info = new FileInfo(gamePath);
            AddGame(System.IO.Path.GetFileNameWithoutExtension(info.Name) , info.Length, info.CreationTime);
        }
    }
}
