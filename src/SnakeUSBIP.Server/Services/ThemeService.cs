using System.Windows;

using Media = System.Windows.Media;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Theme service - Light/Dark theme management
/// Ported from SnakeUSBIP Client
/// </summary>
public static class ThemeService
{
    public static void ApplyTheme(string theme)
    {
        var resources = System.Windows.Application.Current.Resources;
        
        if (theme == "light")
        {
            // Light theme - Windows 11 style
            resources["BgPrimary"] = new Media.SolidColorBrush(ColorFromHex("#F5F5F5"));
            resources["BgSecondary"] = new Media.SolidColorBrush(ColorFromHex("#FFFFFF"));
            resources["BgTertiary"] = new Media.SolidColorBrush(ColorFromHex("#E8E8E8"));
            resources["AccentColor"] = new Media.SolidColorBrush(ColorFromHex("#0078D4"));
            resources["AccentHover"] = new Media.SolidColorBrush(ColorFromHex("#1A86D9"));
            resources["TextPrimary"] = new Media.SolidColorBrush(ColorFromHex("#1E1E1E"));
            resources["TextSecondary"] = new Media.SolidColorBrush(ColorFromHex("#666666"));
            resources["BorderColor"] = new Media.SolidColorBrush(ColorFromHex("#D1D1D1"));
            resources["StatusSuccess"] = new Media.SolidColorBrush(ColorFromHex("#107C10"));
            resources["StatusWarning"] = new Media.SolidColorBrush(ColorFromHex("#B7791F"));
            resources["StatusError"] = new Media.SolidColorBrush(ColorFromHex("#C42B1C"));
            resources["StatusInfo"] = new Media.SolidColorBrush(ColorFromHex("#0078D4"));
            resources["DataGridBg"] = new Media.SolidColorBrush(ColorFromHex("#FFFFFF"));
            resources["DataGridAltBg"] = new Media.SolidColorBrush(ColorFromHex("#F8F8F8"));
            resources["DataGridHover"] = new Media.SolidColorBrush(ColorFromHex("#E5F3FF"));
            resources["DataGridSelected"] = new Media.SolidColorBrush(ColorFromHex("#CCE8FF"));
        }
        else
        {
            // Dark theme - VS Code Dark+ Professional (default)
            resources["BgPrimary"] = new Media.SolidColorBrush(ColorFromHex("#1E1E1E"));
            resources["BgSecondary"] = new Media.SolidColorBrush(ColorFromHex("#252526"));
            resources["BgTertiary"] = new Media.SolidColorBrush(ColorFromHex("#323233"));
            resources["AccentColor"] = new Media.SolidColorBrush(ColorFromHex("#0E639C"));
            resources["AccentHover"] = new Media.SolidColorBrush(ColorFromHex("#1177BB"));
            resources["TextPrimary"] = new Media.SolidColorBrush(ColorFromHex("#DCDCDC"));
            resources["TextSecondary"] = new Media.SolidColorBrush(ColorFromHex("#969696"));
            resources["BorderColor"] = new Media.SolidColorBrush(ColorFromHex("#3C3C3C"));
            resources["StatusSuccess"] = new Media.SolidColorBrush(ColorFromHex("#89D185"));
            resources["StatusWarning"] = new Media.SolidColorBrush(ColorFromHex("#CCA700"));
            resources["StatusError"] = new Media.SolidColorBrush(ColorFromHex("#F14C4C"));
            resources["StatusInfo"] = new Media.SolidColorBrush(ColorFromHex("#75BEFF"));
            resources["DataGridBg"] = new Media.SolidColorBrush(ColorFromHex("#1E1E1E"));
            resources["DataGridAltBg"] = new Media.SolidColorBrush(ColorFromHex("#252526"));
            resources["DataGridHover"] = new Media.SolidColorBrush(ColorFromHex("#2A2D2E"));
            resources["DataGridSelected"] = new Media.SolidColorBrush(ColorFromHex("#094771"));
        }
    }
    
    private static Media.Color ColorFromHex(string hex)
    {
        return (Media.Color)Media.ColorConverter.ConvertFromString(hex);
    }
    
    public static string GetThemeIcon(string theme)
    {
        return theme == "dark" ? "🌙" : "☀️";
    }
}
