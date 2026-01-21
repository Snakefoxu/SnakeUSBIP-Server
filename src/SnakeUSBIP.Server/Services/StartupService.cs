// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Windows Startup Service
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

using Microsoft.Win32;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Service for managing Windows startup registration
/// </summary>
public static class StartupService
{
    private const string AppName = "SnakeUSBIP Server";
    private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// Check if the app is registered to start with Windows
    /// </summary>
    public static bool IsRegistered()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, false);
            if (key == null) return false;

            var value = key.GetValue(AppName);
            return value != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Register the app to start with Windows
    /// </summary>
    public static bool Register()
    {
        try
        {
            var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
            if (string.IsNullOrEmpty(exePath))
            {
                LogService.Error("Could not determine executable path");
                return false;
            }

            // Add --minimized argument for startup
            var startupCommand = $"\"{exePath}\" --minimized";

            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (key == null)
            {
                LogService.Error("Could not open registry key");
                return false;
            }

            key.SetValue(AppName, startupCommand);
            
            SettingsService.Current.StartWithWindows = true;
            SettingsService.Save();
            
            LogService.Info($"✅ Registered for Windows startup: {startupCommand}");
            return true;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to register for startup: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Unregister the app from Windows startup
    /// </summary>
    public static bool Unregister()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (key == null) return true; // Already not registered

            key.DeleteValue(AppName, false);
            
            SettingsService.Current.StartWithWindows = false;
            SettingsService.Save();
            
            LogService.Info("✅ Unregistered from Windows startup");
            return true;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to unregister from startup: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Toggle startup registration based on current state
    /// </summary>
    public static bool Toggle()
    {
        if (IsRegistered())
        {
            return Unregister();
        }
        else
        {
            return Register();
        }
    }

    /// <summary>
    /// Sync registration state with settings
    /// </summary>
    public static void SyncWithSettings()
    {
        var shouldBeRegistered = SettingsService.Current.StartWithWindows;
        var isRegistered = IsRegistered();

        if (shouldBeRegistered && !isRegistered)
        {
            Register();
        }
        else if (!shouldBeRegistered && isRegistered)
        {
            Unregister();
        }
    }
}
