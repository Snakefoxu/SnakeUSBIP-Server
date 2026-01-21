// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Settings Window Code-Behind
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

using System.Windows;
using SnakeUSBIP.Server.Services;

namespace SnakeUSBIP.Server.Views;

/// <summary>
/// Settings window for configuring application preferences
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        LoadSettings();
    }

    /// <summary>
    /// Load current settings into UI controls
    /// </summary>
    private void LoadSettings()
    {
        var settings = SettingsService.Current;

        // General
        chkStartWithWindows.IsChecked = settings.StartWithWindows;
        chkStartMinimized.IsChecked = settings.StartMinimized;
        chkShowNotifications.IsChecked = settings.ShowNotifications;
        chkCheckUpdates.IsChecked = settings.CheckForUpdates;

        // Auto-Bind
        chkAutoBind.IsChecked = settings.AutoBindEnabled;

        // Security
        chkWhitelist.IsChecked = settings.WhitelistEnabled;
        chkRequirePIN.IsChecked = settings.RequirePIN;

        // Network
        UpdateFirewallStatus(settings.FirewallConfigured);

        // Advanced
        txtRefreshInterval.Text = settings.RefreshIntervalSeconds.ToString();
    }

    /// <summary>
    /// Save UI values to settings
    /// </summary>
    private void SaveSettings()
    {
        var settings = SettingsService.Current;

        // General
        settings.StartWithWindows = chkStartWithWindows.IsChecked ?? false;
        settings.StartMinimized = chkStartMinimized.IsChecked ?? false;
        settings.ShowNotifications = chkShowNotifications.IsChecked ?? true;
        settings.CheckForUpdates = chkCheckUpdates.IsChecked ?? true;

        // Auto-Bind
        settings.AutoBindEnabled = chkAutoBind.IsChecked ?? false;

        // Security
        settings.WhitelistEnabled = chkWhitelist.IsChecked ?? false;
        settings.RequirePIN = chkRequirePIN.IsChecked ?? false;

        // Advanced
        if (int.TryParse(txtRefreshInterval.Text, out int interval) && interval >= 1 && interval <= 60)
        {
            settings.RefreshIntervalSeconds = interval;
        }

        SettingsService.Save();
        
        // Sync startup registration with settings
        StartupService.SyncWithSettings();
    }

    private void UpdateFirewallStatus(bool configured)
    {
        if (configured)
        {
            txtFirewallStatus.Text = "✅ Configured";
            txtFirewallStatus.Foreground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(39, 174, 96));
        }
        else
        {
            txtFirewallStatus.Text = "⚠️ Not configured";
            txtFirewallStatus.Foreground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(241, 196, 15));
        }
    }

    private async void BtnFirewall_Click(object sender, RoutedEventArgs e)
    {
        // Check if already configured
        if (FirewallService.IsRuleConfigured())
        {
            var removeResult = System.Windows.MessageBox.Show(
                "Firewall rule is already configured.\n\nDo you want to REMOVE the rule?",
                "Firewall Already Configured",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (removeResult == System.Windows.MessageBoxResult.Yes)
            {
                btnFirewall.IsEnabled = false;
                var removed = await FirewallService.RemoveRuleAsync();
                btnFirewall.IsEnabled = true;
                UpdateFirewallStatus(!removed);
            }
            return;
        }

        var result = System.Windows.MessageBox.Show(
            "This will create a Windows Firewall rule to allow USB/IP connections on port 3240.\n\n" +
            "Do you want to proceed?",
            "Configure Firewall",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Question);

        if (result == System.Windows.MessageBoxResult.Yes)
        {
            btnFirewall.IsEnabled = false;
            var success = await FirewallService.CreateRuleAsync();
            btnFirewall.IsEnabled = true;
            
            UpdateFirewallStatus(success);
            
            if (success)
            {
                System.Windows.MessageBox.Show(
                    "✅ Firewall rule created successfully!\n\nPort 3240 is now open for USB/IP connections.",
                    "Firewall Configured",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "❌ Failed to create firewall rule.\n\nMake sure you're running as Administrator.",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
    }

    private void BtnExport_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json",
            FileName = "snakeusbip-settings.json",
            Title = "Export Settings"
        };

        if (dialog.ShowDialog() == true)
        {
            if (SettingsService.Export(dialog.FileName))
            {
                System.Windows.MessageBox.Show(
                    $"Settings exported to:\n{dialog.FileName}",
                    "Export Successful",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
        }
    }

    private void BtnImport_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json",
            Title = "Import Settings"
        };

        if (dialog.ShowDialog() == true)
        {
            if (SettingsService.Import(dialog.FileName))
            {
                LoadSettings(); // Refresh UI with imported values
                System.Windows.MessageBox.Show(
                    "Settings imported successfully!",
                    "Import Successful",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Failed to import settings. Check the file format.",
                    "Import Failed",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        var result = System.Windows.MessageBox.Show(
            "Are you sure you want to reset all settings to defaults?",
            "Reset Settings",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (result == System.Windows.MessageBoxResult.Yes)
        {
            SettingsService.Reset();
            LoadSettings();
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
        DialogResult = true;
        Close();
    }
}
