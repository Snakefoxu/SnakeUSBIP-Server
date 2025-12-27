// ═══════════════════════════════════════════════════════════════════════════════
// SnakeUSBIP Server - USB/IP Server GUI for Windows
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3 - See LICENSE file
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SnakeUSBIP.Server.Services;
using SnakeUSBIP.Server.Models;

namespace SnakeUSBIP.Server;

/// <summary>
/// GUI Wrapper for usbipd-win - Simple and Clean
/// Created by SnakeFoxu - github.com/SnakeFoxu
/// </summary>
public partial class MainWindow : Window
{
    private readonly UsbipdService _usbipdService;
    private readonly DriverInstaller _driverInstaller;
    private readonly DispatcherTimer _refreshTimer;

    public MainWindow()
    {
        InitializeComponent();

        _usbipdService = new UsbipdService();
        _driverInstaller = new DriverInstaller();

        // Wire up events
        _usbipdService.LogMessage += OnLogMessage;
        _driverInstaller.LogMessage += OnLogMessage;

        // Auto-refresh timer (every 3 seconds)
        _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _refreshTimer.Tick += (s, e) => RefreshDeviceList();
        _refreshTimer.Start();

        // Initial load
        Log("🐍 SnakeUSBIP Server v2.0 - GUI for usbipd-win");
        CheckDriverAndRefresh();
    }

    private async void CheckDriverAndRefresh()
    {
        if (!_driverInstaller.IsUsbipdInstalled())
        {
            Log("⚠️ usbipd-win not installed");
            Log("📦 Installing driver automatically...");
            
            var success = await _driverInstaller.InstallUsbipdAsync();
            if (success)
            {
                Log("✅ usbipd-win installed successfully!");
                // Reinitialize the service to detect the new installation
                _usbipdService.Reinitialize();
                Log("🔄 Service reinitialized");
            }
            else
            {
                Log("❌ Failed to install. Please run as administrator.");
                return;
            }
        }
        else
        {
            Log("✅ usbipd-win detected");
        }

        RefreshDeviceList();
        UpdateStatus();
    }

    private void RefreshDeviceList()
    {
        var devices = _usbipdService.GetDevices();
        
        Dispatcher.Invoke(() =>
        {
            dgDevices.ItemsSource = null;
            dgDevices.ItemsSource = devices;
            txtDeviceCount.Text = $"{devices.Count} device(s)";
        });
    }

    private void UpdateStatus()
    {
        Dispatcher.Invoke(() =>
        {
            if (_usbipdService.IsUsbipdAvailable)
            {
                statusIndicator.Fill = new SolidColorBrush(Color.FromRgb(0, 210, 106));
                txtStatus.Text = "Server running (usbipd service active)";
            }
            else
            {
                statusIndicator.Fill = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                txtStatus.Text = "usbipd-win not available";
            }
        });
    }

    private void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        RefreshDeviceList();
        Log("🔄 Device list refreshed");
    }

    private void BtnClearLog_Click(object sender, RoutedEventArgs e)
    {
        txtLog.Clear();
    }

    private void BtnBind_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string busId)
        {
            var success = _usbipdService.BindDevice(busId);
            RefreshDeviceList();
        }
    }

    private void BtnUnbind_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string busId)
        {
            var success = _usbipdService.UnbindDevice(busId);
            RefreshDeviceList();
        }
    }

    private async void BtnUninstall_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "This will uninstall usbipd-win driver and remove USB/IP server functionality.\n\nAre you sure?",
            "Uninstall usbipd-win",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            Log("🗑️ Uninstalling usbipd-win...");
            var success = await _driverInstaller.UninstallUsbipdAsync();
            if (success)
            {
                Log("✅ usbipd-win uninstalled. You can delete this app folder.");
                UpdateStatus();
            }
        }
    }

    private void OnLogMessage(object? sender, string message)
    {
        Log(message);
    }

    private void Log(string message)
    {
        Dispatcher.Invoke(() =>
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            txtLog.ScrollToEnd();
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _refreshTimer.Stop();
        _usbipdService.Dispose();
        base.OnClosed(e);
    }
}