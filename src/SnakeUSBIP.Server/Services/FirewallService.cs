// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Firewall Configuration Service
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

using System.Diagnostics;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Service for managing Windows Firewall rules for USB/IP
/// </summary>
public static class FirewallService
{
    private const string RuleName = "SnakeUSBIP Server (TCP 3240)";
    private const int Port = 3240;

    /// <summary>
    /// Check if the firewall rule exists
    /// </summary>
    public static bool IsRuleConfigured()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall show rule name=\"{RuleName}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) return false;

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(5000);

            return output.Contains(RuleName);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Create firewall rule to allow USB/IP connections
    /// </summary>
    public static async Task<bool> CreateRuleAsync()
    {
        try
        {
            LogService.Info("Creating firewall rule for USB/IP...");

            // Create inbound rule
            var inboundResult = await RunNetshCommandAsync(
                $"advfirewall firewall add rule name=\"{RuleName}\" dir=in action=allow protocol=tcp localport={Port}");

            if (!inboundResult)
            {
                LogService.Error("Failed to create inbound firewall rule");
                return false;
            }

            LogService.Info($"✅ Firewall rule created: {RuleName}");
            SettingsService.Current.FirewallConfigured = true;
            SettingsService.Save();

            return true;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to create firewall rule: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Remove the firewall rule
    /// </summary>
    public static async Task<bool> RemoveRuleAsync()
    {
        try
        {
            LogService.Info("Removing firewall rule...");

            var result = await RunNetshCommandAsync(
                $"advfirewall firewall delete rule name=\"{RuleName}\"");

            if (result)
            {
                LogService.Info("✅ Firewall rule removed");
                SettingsService.Current.FirewallConfigured = false;
                SettingsService.Save();
            }

            return result;
        }
        catch (Exception ex)
        {
            LogService.Error($"Failed to remove firewall rule: {ex.Message}");
            return false;
        }
    }

    private static async Task<bool> RunNetshCommandAsync(string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();
            await process.WaitForExitAsync();

            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
