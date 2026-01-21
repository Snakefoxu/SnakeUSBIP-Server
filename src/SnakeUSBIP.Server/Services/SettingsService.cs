// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Settings Service
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

using System.IO;
using System.Text.Json;
using SnakeUSBIP.Server.Models;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Service for loading and saving application settings
/// </summary>
public static class SettingsService
{
    private static readonly string SettingsFileName = "settings.json";
    private static AppSettings? _current;
    private static readonly object _lock = new();

    /// <summary>
    /// Current application settings (singleton)
    /// </summary>
    public static AppSettings Current
    {
        get
        {
            if (_current == null)
            {
                lock (_lock)
                {
                    _current ??= Load();
                }
            }
            return _current;
        }
    }

    /// <summary>
    /// Get the settings file path based on app mode (Portable/Installed)
    /// </summary>
    private static string GetSettingsPath()
    {
        return Path.Combine(PathService.ConfigPath, SettingsFileName);
    }

    /// <summary>
    /// Load settings from disk or return defaults
    /// </summary>
    public static AppSettings Load()
    {
        try
        {
            var path = GetSettingsPath();
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null)
                {
                    LogService.Info($"Settings loaded from: {path}");
                    return settings;
                }
            }
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to load settings: {ex.Message}");
        }

        LogService.Info("Using default settings");
        return new AppSettings();
    }

    /// <summary>
    /// Save current settings to disk
    /// </summary>
    public static bool Save()
    {
        try
        {
            var path = GetSettingsPath();
            
            // Ensure directory exists
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };
            var json = JsonSerializer.Serialize(Current, options);
            File.WriteAllText(path, json);
            
            LogService.Info($"Settings saved to: {path}");
            return true;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to save settings: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Reset settings to defaults
    /// </summary>
    public static void Reset()
    {
        lock (_lock)
        {
            _current = new AppSettings();
        }
        Save();
        LogService.Info("Settings reset to defaults");
    }

    /// <summary>
    /// Export settings to a file
    /// </summary>
    public static bool Export(string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Current, options);
            File.WriteAllText(filePath, json);
            LogService.Info($"Settings exported to: {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to export settings: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Import settings from a file
    /// </summary>
    public static bool Import(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                LogService.Error($"Import file not found: {filePath}");
                return false;
            }

            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json);
            
            if (settings != null)
            {
                lock (_lock)
                {
                    _current = settings;
                }
                Save();
                LogService.Info($"Settings imported from: {filePath}");
                return true;
            }
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to import settings: {ex.Message}");
        }
        return false;
    }
}
