using GameLauncherLibrary.Enums;
using System.Diagnostics;

namespace GameLauncherLibrary.Models;

public class Game
{
    public string Name { get; set; }
    public string ExecutablePath { get; set; }
    public Launchers Launcher { get; set; }
    public string IconPath { get; set; }
    public string HeaderPath { get; set; }

    public int Size { get; set; }

    public void Launch()
    {
        switch (Launcher)
        {
            case Launchers.Steam:
                Process.Start(new ProcessStartInfo { FileName = $"steam://rungameid/{ExecutablePath}", UseShellExecute = true });
                break;
        }
    }
}
