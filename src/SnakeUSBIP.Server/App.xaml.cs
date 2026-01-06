/*
 * SnakeUSBIP Server - Application Entry Point
 * (c) 2025 SnakeFoxu - Protocolo Omega Compliant
 * https://github.com/SnakeFoxu/SnakeUSBIP-Server
 */

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SnakeUSBIP.Server.Services;

namespace SnakeUSBIP.Server;

/// <summary>
/// Main application class with robust exception handling.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Initialize logging first (before anything can fail)
        LogService.Initialize();
        LogService.Info("Application starting...");
        LogService.Info(PathService.GetDiagnosticReport());

        // Clean old logs (keep 7 days)
        LogService.CleanupOldLogs(7);

        // Hook global exception handlers
        SetupExceptionHandlers();

        LogService.Info("Startup complete");
    }

    private void SetupExceptionHandlers()
    {
        // UI thread exceptions
        DispatcherUnhandledException += OnDispatcherUnhandledException;

        // Non-UI thread exceptions
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        // Task exceptions (async/await)
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        LogService.Info("Exception handlers configured");
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogService.Exception(e.Exception, "UI Thread");

        // Show user-friendly error dialog
        MessageBox.Show(
            $"An unexpected error occurred:\n\n{e.Exception.Message}\n\nThe error has been logged.",
            "SnakeUSBIP Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        // Mark as handled to prevent crash (unless critical)
        e.Handled = true;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            LogService.Exception(ex, "AppDomain");
        }
        else
        {
            LogService.Error($"Unknown fatal error: {e.ExceptionObject}");
        }

        // This is terminal - app will crash after this
        LogService.Error("Application terminating due to unhandled exception");
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogService.Exception(e.Exception, "Async Task");

        // Observe the exception to prevent crash
        e.SetObserved();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        LogService.Info($"Application exiting with code: {e.ApplicationExitCode}");
        base.OnExit(e);
    }
}
