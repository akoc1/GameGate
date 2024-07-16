using System;
using System.Diagnostics;

namespace GameGate;

public class Game
{
    public string Name { get; set; }
    public string ExecutablePath { get; set; }
    public Launchers Launcher { get; set; }
    public string InstalledPath { get; set; }
    public string IconPath { get; set; }
    public string HeaderPath { get; set; }
    public int Size { get; set; }

    public void Launch()
    {
        switch (Launcher)
        {
            case Launchers.Steam:
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = $"steam://rungameid/{ExecutablePath}", UseShellExecute = true });
                }
                catch (Exception e)
                {

                }
                break;
        }
    }

    public void OpenGameFolder()
    {
        try
        {
            Process.Start(InstalledPath);
        }
        catch (Exception e)
        {

        }
    }
}
