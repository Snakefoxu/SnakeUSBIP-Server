// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server v1.0 - Copyright (c) 2025 SnakeFoxu
// Source: https://github.com/SnakeFoxu/SnakeUSBIP-Server
// This file is part of SnakeUSBIP Server, licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using SnakeUSBIP.Server.Models;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Simple wrapper for usbipd.exe commands
/// Author: SnakeFoxu (github.com/SnakeFoxu)
/// </summary>
public class UsbipdService : IDisposable
{
    private string _usbipdPath = "usbipd.exe";

    public event EventHandler<string>? LogMessage;
    public bool IsUsbipdAvailable { get; private set; }

    public UsbipdService()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Find usbipd.exe
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        var systemPath = Path.Combine(programFiles, "usbipd-win", "usbipd.exe");
        
        if (File.Exists(systemPath))
        {
            _usbipdPath = systemPath;
            IsUsbipdAvailable = true;
        }
        else
        {
            _usbipdPath = "usbipd.exe";
            IsUsbipdAvailable = CheckUsbipdInPath();
        }
    }

    /// <summary>
    /// Reinitialize after driver installation
    /// </summary>
    public void Reinitialize() => Initialize();

    private bool CheckUsbipdInPath()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "usbipd.exe",
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using var process = Process.Start(psi);
            if (process != null)
            {
                process.WaitForExit(3000);
                return process.ExitCode == 0;
            }
        }
        catch { }
        return false;
    }

    /// <summary>
    /// Get list of USB devices using usbipd list + WMI enrichment
    /// </summary>
    public List<LocalUsbDevice> GetDevices()
    {
        var devices = new List<LocalUsbDevice>();
        if (!IsUsbipdAvailable) return devices;

        try
        {
            var output = RunCommand("list");
            if (string.IsNullOrEmpty(output)) return devices;

            // Get WMI device info for enrichment
            var wmiDevices = GetWmiDeviceInfo();

            // Parse text output from usbipd list
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var dataStarted = false;

            foreach (var line in lines)
            {
                if (line.Contains("BUSID") && line.Contains("STATE"))
                {
                    dataStarted = true; // SnakeFoxu/SnakeUSBIP-Server
                    continue;
                }

                if (!dataStarted || string.IsNullOrWhiteSpace(line)) continue;

                // Parse line with regex
                var match = Regex.Match(line.Trim(), @"^(\d+-[\d.]+)\s+([0-9a-fA-F]{4}):([0-9a-fA-F]{4})\s+(.+?)\s+(Not shared|Shared|Attached)");
                if (match.Success)
                {
                    var vid = int.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
                    var pid = int.Parse(match.Groups[3].Value, System.Globalization.NumberStyles.HexNumber);
                    var baseName = match.Groups[4].Value.Trim();
                    
                    // Try to get better name from WMI
                    // github.com/SnakeFoxu
                    var vidPidKey = $"{vid:X4}:{pid:X4}";
                    var enrichedName = wmiDevices.TryGetValue(vidPidKey, out var wmiName) ? wmiName : baseName;

                    var device = new LocalUsbDevice
                    {
                        BusId = match.Groups[1].Value,
                        VendorId = vid,
                        ProductId = pid,
                        Name = enrichedName,
                        Description = baseName,
                        Status = match.Groups[5].Value,
                        IsExported = match.Groups[5].Value == "Shared",
                        IsNotShared = match.Groups[5].Value == "Not shared"
                    };

                    devices.Add(device);
                }
            }
        }
        catch (Exception ex)
        {
            Log($"⚠️ Error: {ex.Message}");
        }

        return devices;
    }

    private Dictionary<string, string> GetWmiDeviceInfo()
    {
        var devices = new Dictionary<string, string>();
        try
        {
            using var searcher = new System.Management.ManagementObjectSearcher(
                "SELECT DeviceID, Name, Caption FROM Win32_PnPEntity WHERE DeviceID LIKE 'USB%'");
            
            foreach (var obj in searcher.Get())
            {
                var deviceId = obj["DeviceID"]?.ToString() ?? "";
                var name = obj["Name"]?.ToString() ?? obj["Caption"]?.ToString() ?? "";
                
                // Extract VID:PID from DeviceID (e.g., USB\VID_0781&PID_5567\...)
                var vidMatch = Regex.Match(deviceId, @"VID_([0-9A-Fa-f]{4})");
                var pidMatch = Regex.Match(deviceId, @"PID_([0-9A-Fa-f]{4})");
                
                if (vidMatch.Success && pidMatch.Success && !string.IsNullOrWhiteSpace(name))
                {
                    var key = $"{vidMatch.Groups[1].Value.ToUpper()}:{pidMatch.Groups[1].Value.ToUpper()}"; /* SnakeFoxu */
                    if (!devices.ContainsKey(key) || name.Length > devices[key].Length)
                    {
                        devices[key] = name;
                    }
                }
            }
        }
        catch { }
        return devices;
    }

    /// <summary>
    /// Share a device (bind)
    /// </summary>
    public bool BindDevice(string busId)
    {
        // Validate input to prevent command injection
        if (!SecurityHelper.IsValidBusId(busId))
        {
            Log($"❌ Invalid Bus ID format: {busId}");
            return false;
        }

        var sanitizedBusId = SecurityHelper.SanitizeArgument(busId);
        Log($"📤 Sharing device {sanitizedBusId}...");
        var result = RunAdminCommand($"bind --busid={sanitizedBusId} --force");
        if (result)
        {
            Log($"✅ Device {sanitizedBusId} is now shared");
        }
        return result;
    }

    /// <summary>
    /// Stop sharing a device (unbind)
    /// </summary>
    public bool UnbindDevice(string busId)
    {
        // Validate input to prevent command injection
        if (!SecurityHelper.IsValidBusId(busId))
        {
            Log($"❌ Invalid Bus ID format: {busId}");
            return false;
        }

        var sanitizedBusId = SecurityHelper.SanitizeArgument(busId);
        Log($"🚫 Stopping share for {sanitizedBusId}...");
        var result = RunAdminCommand($"unbind --busid={sanitizedBusId}");
        if (result)
        {
            Log($"✅ Device {sanitizedBusId} is no longer shared");
        }
        return result;
    }

    private string RunCommand(string args)
    {
        var psi = new ProcessStartInfo
        {
            FileName = _usbipdPath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null) return "";

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit(10000);
        return output;
    }

    private bool RunAdminCommand(string args)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = _usbipdPath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // Only request elevation if we're NOT already admin
            if (!IsRunningAsAdmin())
            {
                psi.UseShellExecute = true;
                psi.Verb = "runas";
                psi.RedirectStandardOutput = false;
                psi.RedirectStandardError = false;
                psi.CreateNoWindow = false;
            }

            using var process = Process.Start(psi);
            if (process == null) return false;

            process.WaitForExit(30000);
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            Log($"❌ Error: {ex.Message}");
            return false;
        }
    }

    private static bool IsRunningAsAdmin()
    {
        using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
        var principal = new System.Security.Principal.WindowsPrincipal(identity);
        return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
    }

    private void Log(string message) =>
        LogMessage?.Invoke(this, message);

    public void Dispose() { }
}
