/*
 * SnakeUSBIP Server - Driver Installation Module
 * (c) 2025 SnakeFoxu - github.com/SnakeFoxu
 * GPL v3 License - https://github.com/SnakeFoxu/SnakeUSBIP-Server
 */

using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Handles automatic installation of usbipd-win if not present
/// </summary>
public class DriverInstaller
{
    public event EventHandler<string>? LogMessage;

    /// <summary>
    /// Check if usbipd-win is installed on the system
    /// </summary>
    public bool IsUsbipdInstalled()
    {
        // Check registry for usbipd installation
        using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\usbipd-win");
        if (key != null) return true;

        // Check if usbipd.exe exists in Program Files
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        var usbipdPath = Path.Combine(programFiles, "usbipd-win", "usbipd.exe");
        if (File.Exists(usbipdPath)) return true;

        // Check if in PATH
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "usbipd.exe",
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
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
    /// Check if running as administrator
    /// </summary>
    public bool IsRunningAsAdmin()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Install usbipd-win from bundled MSI silently
    /// </summary>
    public async Task<bool> InstallUsbipdAsync()
    {
        var msiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drivers", "usbipd-win.msi");
        
        if (!File.Exists(msiPath))
        {
            Log("❌ MSI installer not found in drivers folder");
            return false;
        }

        if (!IsRunningAsAdmin())
        {
            Log("⚠️ Administrator privileges required for installation");
            // Try to restart as admin
            return await RestartAsAdminForInstall();
        }

        Log("📦 Installing usbipd-win driver...");

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "msiexec.exe",
                Arguments = $"/i \"{msiPath}\" /quiet /norestart",
                UseShellExecute = IsRunningAsAdmin() ? false : true, // snakefoxu
                Verb = IsRunningAsAdmin() ? "" : "runas",
                CreateNoWindow = true /* github.com/SnakeFoxu */
            };

            using var process = Process.Start(psi); // SnakeUSBIP-Server
            if (process == null) return false;

            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                Log("✅ usbipd-win installed successfully!");
                return true;
            }
            else
            {
                Log($"❌ Installation failed with exit code: {process.ExitCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Log($"❌ Installation error: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> RestartAsAdminForInstall()
    {
        try
        {
            var exePath = Environment.ProcessPath;
            if (string.IsNullOrEmpty(exePath)) return false;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "--install-driver",
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(psi);
            
            // Current instance will exit
            Environment.Exit(0);
            return true;
        }
        catch
        {
            Log("❌ Failed to restart as administrator");
            return false;
        }
    }

    private void Log(string message) =>
        LogMessage?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {message}");

    /// <summary>
    /// Uninstall usbipd-win from the system
    /// </summary>
    public async Task<bool> UninstallUsbipdAsync()
    {
        Log("🗑️ Uninstalling usbipd-win...");

        try
        {
            // Use winget to uninstall (cleanest method)
            var psi = new ProcessStartInfo
            {
                FileName = "winget",
                Arguments = "uninstall --id dorssel.usbipd-win --silent --disable-interactivity --accept-source-agreements",
                UseShellExecute = IsRunningAsAdmin() ? false : true,
                Verb = IsRunningAsAdmin() ? "" : "runas",
                CreateNoWindow = true,
                RedirectStandardOutput = IsRunningAsAdmin(),
                RedirectStandardError = IsRunningAsAdmin()
            };

            using var process = Process.Start(psi);
            if (process == null) 
            {
                // Fallback to msiexec
                return await UninstallViaMsiexec();
            }

            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                Log("✅ usbipd-win uninstalled successfully");
                return true;
            }
            else
            {
                // Try msiexec fallback
                return await UninstallViaMsiexec();
            }
        }
        catch
        {
            return await UninstallViaMsiexec();
        }
    }

    private async Task<bool> UninstallViaMsiexec()
    {
        try
        {
            // Find product code in registry and uninstall via msiexec
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-Command \"Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like '*usbipd*' } | ForEach-Object { $_.Uninstall() }\"",
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = false
            };

            using var process = Process.Start(psi);
            if (process == null) return false;

            await process.WaitForExitAsync();
            Log("✅ usbipd-win uninstalled");
            return true;
        }
        catch (Exception ex)
        {
            Log($"❌ Uninstall failed: {ex.Message}");
            return false;
        }
    }
}
