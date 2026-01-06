/*
 * SnakeUSBIP Server - Logging Service
 * (c) 2025 SnakeFoxu - Protocolo Omega Compliant
 * https://github.com/SnakeFoxu/SnakeUSBIP-Server
 */

using System.IO;
using System.Text;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Centralized logging service with file persistence.
/// All logs are written to PathService.LogPath for full auditability.
/// </summary>
public static class LogService
{
    private static readonly object _lock = new();
    private static string? _currentLogFile;
    private static bool _initialized;

    /// <summary>Initialize logging (call once at startup).</summary>
    public static void Initialize()
    {
        if (_initialized) return;

        try
        {
            string logPath = PathService.LogPath;
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _currentLogFile = Path.Combine(logPath, $"snakeusbip_{timestamp}.log");

            // Write header
            WriteToFile($@"
════════════════════════════════════════════════════════════════
 SnakeUSBIP Server Log - Started {DateTime.Now:yyyy-MM-dd HH:mm:ss}
 Mode: {(PathService.IsPortable ? "Portable" : "Installed")}
════════════════════════════════════════════════════════════════
");
            _initialized = true;
            Info("LogService initialized successfully");
        }
        catch (Exception ex)
        {
            // Fallback: log to console/debug only
            System.Diagnostics.Debug.WriteLine($"LogService init failed: {ex.Message}");
        }
    }

    /// <summary>Log informational message.</summary>
    public static void Info(string message) => Log("INFO", message);

    /// <summary>Log warning message.</summary>
    public static void Warn(string message) => Log("WARN", message);

    /// <summary>Log error message.</summary>
    public static void Error(string message) => Log("ERROR", message);

    /// <summary>Log exception with full stack trace.</summary>
    public static void Exception(Exception ex, string? context = null)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"EXCEPTION{(context != null ? $" in {context}" : "")}:");
        sb.AppendLine($"  Type: {ex.GetType().FullName}");
        sb.AppendLine($"  Message: {ex.Message}");
        sb.AppendLine($"  StackTrace:");
        sb.AppendLine(ex.StackTrace);

        if (ex.InnerException != null)
        {
            sb.AppendLine($"  Inner: {ex.InnerException.Message}");
        }

        Log("FATAL", sb.ToString());
    }

    private static void Log(string level, string message)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        string logLine = $"[{timestamp}] [{level}] {message}";

        // Always write to debug output
        System.Diagnostics.Debug.WriteLine(logLine);

        // Write to file if initialized
        WriteToFile(logLine);
    }

    private static void WriteToFile(string text)
    {
        if (string.IsNullOrEmpty(_currentLogFile)) return;

        try
        {
            lock (_lock)
            {
                File.AppendAllText(_currentLogFile, text + Environment.NewLine);
            }
        }
        catch
        {
            // Silent fail - logging should never crash the app
        }
    }

    /// <summary>Get path to current log file (for diagnostics).</summary>
    public static string? CurrentLogFile => _currentLogFile;

    /// <summary>Clean up old log files (keep last N days).</summary>
    public static void CleanupOldLogs(int keepDays = 7)
    {
        try
        {
            string logPath = PathService.LogPath;
            var cutoff = DateTime.Now.AddDays(-keepDays);

            foreach (var file in Directory.GetFiles(logPath, "snakeusbip_*.log"))
            {
                if (File.GetCreationTime(file) < cutoff)
                {
                    try { File.Delete(file); } catch { }
                }
            }
        }
        catch { }
    }
}
