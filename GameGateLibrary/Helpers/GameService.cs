﻿using GameLauncherLibrary.Enums;
using GameLauncherLibrary.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GameLauncherLibrary.Helpers;

public static class GameService
{
    /// <summary>
    /// Get installed games from steam library
    /// </summary>
    /// <returns>A list of games</returns>
    public async static Task<List<Game>> GetSteamGames()
    {
        List<Game> games = new List<Game>();

        string steamManifestPath = GetSteamRootPath();

        steamManifestPath += "/steamapps";

        string[] manifestFiles = Directory.GetFiles(steamManifestPath, "*.acf");

        foreach (string manifestFile in manifestFiles)
        {
            string gameName, gameID;
            int gameSize;

            (gameName, gameID, gameSize) = await GetDataFromSteamManifestFile(manifestFile);

            if (gameID == "228980") // Avoid adding Steam tools
                continue;

            if (IsGameInstalled(gameID) == false)
                continue;

            games.Add(new Game
            {
                Name = gameName,
                ExecutablePath = gameID,
                Launcher = Launchers.Steam,
                IconPath = GetSteamRootPath() + $"/appcache/librarycache/{gameID}_icon.jpg",
                HeaderPath = GetSteamRootPath() + $"/appcache/librarycache/{gameID}_header.jpg",
                Size = gameSize
            });
        }

        return games;
    }

    private static string GetSteamRootPath()
    {
        return Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam", true).GetValue("SteamPath").ToString();
    }

    private async static Task<(string gameName, string gameID, int gameSize)> GetDataFromSteamManifestFile(string manifestFilePath)
    {
        string gameName = "", gameID = "";
        int gameSize = 0;

        string[] lines = await File.ReadAllLinesAsync(manifestFilePath);

        foreach (string line in lines)
        {
            if (line.Contains("appid"))
                gameID = GetDataFromFile(line, "appid");

            if (line.Contains("name"))
            {
                gameName = GetDataFromFile(line, "name");
                break;
            }

            if (line.Contains("BytesDownloaded"))
            {
                gameSize = int.Parse(GetDataFromFile(line, "BytesDownloaded"));
                break;
            }
        }

        return (gameName, gameID, gameSize);
    }

    private static string GetDataFromFile(string line, string key)
    {
        string output = string.Empty;

        if (line.Contains(key))
        {
            string trimmedLine = line.Replace($"\"{key}\"", "").Replace("\t", "");

            foreach (char c in trimmedLine)
            {
                if (c != '"')
                    output += c;
            }
        }

        return output;
    }

    private static bool IsGameInstalled(string gameID)
    {
        string libraryFoldersVDF = GetSteamRootPath() + "/steamapps/libraryfolders.vdf";

        string[] lines = File.ReadAllLines(libraryFoldersVDF);

        foreach (string line in lines)
        {
            if (line.Contains(gameID))
            {
                string result = GetDataFromFile(line, gameID);

                if (result == "0")
                    return false;
            }
        }

        return true;
    }
}
