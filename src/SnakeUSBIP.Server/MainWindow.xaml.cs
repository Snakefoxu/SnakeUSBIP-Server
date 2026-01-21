// ═══════════════════════════════════════════════════════════════════════════════
// SnakeUSBIP Server - USB/IP Server GUI for Windows
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3 - See LICENSE file
// ═══════════════════════════════════════════════════════════════════════════════

using System.Security.Principal;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SnakeUSBIP.Server.Services;
using SnakeUSBIP.Server.Models;
using WinForms = System.Windows.Forms;

namespace SnakeUSBIP.Server;

/// <summary>
/// GUI Wrapper for usbipd-win - Simple and Clean
/// Created by SnakeFoxu - github.com/SnakeFoxu
/// </summary>
public partial class MainWindow : Window
{
    private readonly UsbipdService _usbipdService;
    private readonly DriverInstaller _driverInstaller;
    private readonly UpdateService _updateService;
    private readonly LocalizationService _localization;
    private readonly DispatcherTimer _refreshTimer;
    private WinForms.NotifyIcon _notifyIcon;
    private string _currentLanguage = "en";
    private string _currentTheme = "dark";

    public MainWindow()
    {
        InitializeComponent();
        InitializeNotifyIcon();

        _usbipdService = new UsbipdService();
        _driverInstaller = new DriverInstaller();
        _updateService = new UpdateService();
        _localization = new LocalizationService();
        
        // Detect system language
        _currentLanguage = LocalizationService.DetectSystemLanguage();
        _currentTheme = SettingsService.Current.Theme ?? "dark";

        // Wire up events
        _usbipdService.LogMessage += OnLogMessage;
        _driverInstaller.LogMessage += OnLogMessage;

        // Auto-refresh timer (every 3 seconds)
        _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _refreshTimer.Tick += (s, e) => RefreshDeviceList();
        _refreshTimer.Start();

        // Initial load
        Log(_localization.GetText("log_server_started", _currentLanguage));
        LogService.Info("MainWindow initialized");
        UpdateBadges();
        UpdateUITexts();
        ThemeService.ApplyTheme(_currentTheme);
        CheckDriverAndRefresh();
    }

    private void InitializeNotifyIcon()
    {
        _notifyIcon = new WinForms.NotifyIcon
        {
            Text = "SnakeUSBIP Server",
            Visible = true
        };

        try
        {
            // Try to use app icon
            var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
            if (!string.IsNullOrEmpty(exePath))
            {
                _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(exePath);
            }
        }
        catch { }

        _notifyIcon.DoubleClick += (s, e) => RestoreWindow();

        var contextMenu = new WinForms.ContextMenuStrip();
        contextMenu.Items.Add("Open SnakeUSBIP", null, (s, e) => RestoreWindow());
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, (s, e) => 
        {
            _notifyIcon.Visible = false;
            Close();
        });

        _notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void RestoreWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            Hide();
            // Show balloon tip if notifications are enabled
            if (SettingsService.Current.ShowNotifications)
            {
                _notifyIcon.ShowBalloonTip(2000, "SnakeUSBIP Server", "Running in background. Double-click to restore.", WinForms.ToolTipIcon.Info);
            }
        }
        base.OnStateChanged(e);
    }

    private void UpdateBadges()
    {
        // Admin Badge
        bool isAdmin = IsRunningAsAdmin();
        adminBadge.Background = new SolidColorBrush(isAdmin ? System.Windows.Media.Color.FromRgb(39, 174, 96) : System.Windows.Media.Color.FromRgb(231, 76, 60));
        txtAdminStatus.Text = isAdmin ? "ADMIN" : "USER";

        // Mode Badge
        modeBadge.Background = new SolidColorBrush(PathService.IsPortable ? System.Windows.Media.Color.FromRgb(52, 152, 219) : System.Windows.Media.Color.FromRgb(155, 89, 182));
        txtModeStatus.Text = PathService.IsPortable ? "PORTABLE" : "INSTALLED";
    }

    private static bool IsRunningAsAdmin()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private async void CheckDriverAndRefresh()
    {
        if (!_driverInstaller.IsUsbipdInstalled())
        {
            Log("⚠️ usbipd-win not installed");
            
            // Ask user before installing
            var result = System.Windows.MessageBox.Show(
                "El driver usbipd-win no está instalado.\n\n" +
                "Este driver es necesario para compartir dispositivos USB por red.\n\n" +
                "¿Desea instalar el driver ahora?",
                "Driver no encontrado",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                Log("📦 Installing driver...");
                var success = await _driverInstaller.InstallUsbipdAsync();
                if (success)
                {
                    Log("✅ usbipd-win installed successfully!");
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
                Log("ℹ️ Driver installation skipped by user");
                Log("⚠️ USB sharing functionality will not be available");
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
        // Fire and forget but safely on UI thread context
        _ = RefreshDeviceListAsync();
    }

    private async Task RefreshDeviceListAsync()
    {
        try
        {
            var devices = await _usbipdService.GetDevicesAsync();
            
            // Still on UI thread if started from there, but let's be safe
            dgDevices.ItemsSource = null;
            dgDevices.ItemsSource = devices;
            txtDeviceCount.Text = $"{devices.Count} device(s)";
        }
        catch (Exception ex)
        {
             Log($"⚠️ Error refreshing list: {ex.Message}");
        }
    }

    private void UpdateStatus()
    {
        if (_usbipdService.IsUsbipdAvailable)
        {
            statusIndicator.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 210, 106));
            txtStatus.Text = "Server running (usbipd service active)";
        }
        else
        {
            statusIndicator.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(231, 76, 60));
            txtStatus.Text = "usbipd-win not available";
        }
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

    private void BtnMinimizeToTray_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
        // OnStateChanged will handle the Hide()
    }

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new Views.SettingsWindow();
        settingsWindow.Owner = this;
        if (settingsWindow.ShowDialog() == true)
        {
            Log("⚙️ Settings saved");
            // Apply any immediate settings changes
            ApplySettings();
        }
    }

    private void ApplySettings()
    {
        // Update refresh timer interval from settings
        var interval = SettingsService.Current.RefreshIntervalSeconds;
        if (interval >= 1 && interval <= 60)
        {
            _refreshTimer.Interval = TimeSpan.FromSeconds(interval);
        }
    }

    private void BtnDiagnostics_Click(object sender, RoutedEventArgs e)
    {
        string diagnostics = PathService.GetDiagnosticReport();
        if (LogService.CurrentLogFile != null)
        {
            diagnostics += $"\nCurrent Log: {LogService.CurrentLogFile}";
        }
        System.Windows.MessageBox.Show(diagnostics, "SnakeUSBIP Diagnostics", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        Log("🔍 Diagnostics displayed");
    }

    private async void BtnBind_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string busId)
        {
            btn.IsEnabled = false; // Prevent double click
            try
            {
                var success = await _usbipdService.BindDeviceAsync(busId);
                await RefreshDeviceListAsync();
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }
    }

    private async void BtnUnbind_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string busId)
        {
            btn.IsEnabled = false;
            try
            {
                var success = await _usbipdService.UnbindDeviceAsync(busId);
                await RefreshDeviceListAsync();
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }
    }

    private async void BtnUninstall_Click(object sender, RoutedEventArgs e)
    {
        var result = System.Windows.MessageBox.Show(
            "This will uninstall usbipd-win driver and remove USB/IP server functionality.\n\nAre you sure?",
            "Uninstall usbipd-win",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (result == System.Windows.MessageBoxResult.Yes)
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

    private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {
        UpdateStatusBar("Checking for updates...", System.Windows.Media.Color.FromRgb(52, 152, 219));
        Log("🔍 Checking for updates...");
        
        try
        {
            var updateInfo = await _updateService.CheckForUpdatesAsync();
            
            Log($"📊 Current: v{updateInfo.CurrentVersion} | Latest: v{updateInfo.LatestVersion}");
            
            if (updateInfo.UpdateAvailable)
            {
                Log($"⬆️ Update available: v{updateInfo.LatestVersion}");
                UpdateStatusBar($"Update available: v{updateInfo.LatestVersion}", System.Windows.Media.Color.FromRgb(241, 196, 15));
                
                var result = System.Windows.MessageBox.Show(
                    $"New version available: v{updateInfo.LatestVersion}\n\n{updateInfo.ReleaseNotes}\n\nDo you want to download and install the update?",
                    "Update Available",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Information);
                
                if (result == System.Windows.MessageBoxResult.Yes && 
                    updateInfo.DownloadUrl != null && 
                    updateInfo.InstallerName != null)
                {
                    Log("📥 Downloading update...");
                    UpdateStatusBar("Downloading update...", System.Windows.Media.Color.FromRgb(52, 152, 219));
                    
                    var installResult = await _updateService.StartUpdateAsync(updateInfo.DownloadUrl, updateInfo.InstallerName);
                    if (!installResult.Success)
                    {
                        Log($"❌ Update failed: {installResult.Error}");
                        UpdateStatusBar($"Update failed: {installResult.Error}", System.Windows.Media.Color.FromRgb(231, 76, 60));
                    }
                }
            }
            else
            {
                Log($"✅ You have the latest version (v{updateInfo.CurrentVersion})");
                UpdateStatusBar($"You have the latest version (v{updateInfo.CurrentVersion})", System.Windows.Media.Color.FromRgb(46, 204, 113));
            }
        }
        catch (Exception ex)
        {
            Log($"❌ Error checking updates: {ex.Message}");
            UpdateStatusBar($"Error: {ex.Message}", System.Windows.Media.Color.FromRgb(231, 76, 60));
        }
    }

    private void BtnLanguage_Click(object sender, RoutedEventArgs e)
    {
        _currentLanguage = _currentLanguage == "es" ? "en" : "es";
        UpdateUITexts();
        Log($"🌐 Language changed to: {(_currentLanguage == "es" ? "Español" : "English")}");
    }

    private void BtnTheme_Click(object sender, RoutedEventArgs e)
    {
        _currentTheme = _currentTheme == "dark" ? "light" : "dark";
        ThemeService.ApplyTheme(_currentTheme);
        btnTheme.Content = ThemeService.GetThemeIcon(_currentTheme);
        Log($"🎨 Theme changed to: {(_currentTheme == "dark" ? "Dark" : "Light")}");
        
        // Save to settings
        SettingsService.Current.Theme = _currentTheme;
        SettingsService.Save();
    }

    private void UpdateUITexts()
    {
        // Row 1: Compact icon-only buttons (with tooltips in XAML)
        btnLanguage.Content = _currentLanguage == "es" ? "🌐 ES" : "🌐 EN";
        btnTheme.Content = ThemeService.GetThemeIcon(_currentTheme);
        btnUpdate.Content = "⬆️";
        btnSettings.Content = "⚙️";
        btnTray.Content = "🔽";
        
        // Row 2: Full text buttons
        btnDiagnostics.Content = _localization.GetText("btn_diagnostics", _currentLanguage);
        btnRefresh.Content = _localization.GetText("btn_refresh", _currentLanguage);
        btnUninstall.Content = _currentLanguage == "es" ? "🗑️ Desinstalar" : "🗑️ Uninstall";
        
        // Update header text
        txtSubtitle.Text = _localization.GetText("subtitle", _currentLanguage);
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

    private void UpdateStatusBar(string message, System.Windows.Media.Color color)
    {
        Dispatcher.Invoke(() =>
        {
            txtStatus.Text = message;
            txtStatus.Foreground = new SolidColorBrush(color);
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _refreshTimer.Stop();
        _usbipdService.Dispose();
        
        if (_notifyIcon != null)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }

        base.OnClosed(e);
    }
}