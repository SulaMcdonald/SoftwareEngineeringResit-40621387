using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace StarterApp.ViewModels;

public class TempViewModel
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string Message => "This is a placeholder page.";

    public TempViewModel()
    {}
}