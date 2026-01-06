/*
 * SnakeUSBIP Server - Path Management Service
 * (c) 2025 SnakeFoxu - Protocolo Omega Compliant
 * https://github.com/SnakeFoxu/SnakeUSBIP-Server
 */

using System.IO;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Centralized service for managing file paths with strict Portable/Installable separation.
/// Portable Mode: Stores data in application directory (USB-ready).
/// Installed Mode: Stores data in %AppData% (clean uninstall).
/// </summary>
public static class PathService
{
    private static readonly string _baseDir;
    private static readonly bool _isPortable;
    private static readonly bool _hasWriteAccess;

    /// <summary>True if running in Portable mode (data stored locally).</summary>
    public static bool IsPortable => _isPortable;

    /// <summary>True if application directory is writable.</summary>
    public static bool HasWriteAccess => _hasWriteAccess;

    /// <summary>Application's base directory (where .exe is located).</summary>
    public static string BaseDirectory => _baseDir;

    /// <summary>Path for log files.</summary>
    public static string LogPath => GetPath("logs");

    /// <summary>Path for theme files.</summary>
    public static string ThemePath => GetPath("themes");

    /// <summary>Path for general data storage.</summary>
    public static string DataPath => GetPath("data");

    /// <summary>Full path to configuration file.</summary>
    public static string ConfigPath => Path.Combine(DataPath, "config.json");

    static PathService()
    {
        _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _hasWriteAccess = CheckWriteAccess(_baseDir);

        // Portable Mode Detection:
        // 1. If ".portable" marker file exists AND we have write access -> Portable
        // 2. If no marker but we have write access -> Default to Portable (USB-friendly)
        // 3. If no write access (e.g., Program Files) -> Installed mode (use AppData)
        bool markerExists = File.Exists(Path.Combine(_baseDir, ".portable"));

        if (_hasWriteAccess)
        {
            // We can write locally - prefer Portable behavior
            _isPortable = true;
        }
        else
        {
            // Cannot write locally (likely Program Files) - use AppData
            _isPortable = false;
        }
    }

    private static string GetPath(string subFolder)
    {
        string root;
        if (_isPortable)
        {
            root = _baseDir;
        }
        else
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            root = Path.Combine(appData, "SnakeUSBIP");
        }

        string fullPath = Path.Combine(root, subFolder);

        // Ensure directory exists (silently create if needed)
        if (!Directory.Exists(fullPath))
        {
            try { Directory.CreateDirectory(fullPath); } catch { /* Logged elsewhere if needed */ }
        }

        return fullPath;
    }

    private static bool CheckWriteAccess(string folderPath)
    {
        try
        {
            string tempFile = Path.Combine(folderPath, $".write_test_{Guid.NewGuid():N}");
            using (FileStream fs = File.Create(tempFile, 1, FileOptions.DeleteOnClose)) { }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns a diagnostic report of current paths for user transparency.
    /// </summary>
    public static string GetDiagnosticReport()
    {
        return $@"═══════════════════════════════════════════
   SnakeUSBIP Path Diagnostics
═══════════════════════════════════════════
Mode:           {(_isPortable ? "🟢 PORTABLE (USB Ready)" : "🔵 INSTALLED (System Integration)")}
Base Directory: {_baseDir}
Write Access:   {(_hasWriteAccess ? "✅ Yes" : "❌ No")}

Active Paths:
  📁 Logs:   {LogPath}
  🎨 Themes: {ThemePath}
  💾 Data:   {DataPath}
  ⚙️ Config: {ConfigPath}

Driver Location:
  📦 usbipd-win: C:\Program Files\usbipd-win\
═══════════════════════════════════════════";
    }
}
