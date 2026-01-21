// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Application Settings Model
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

namespace SnakeUSBIP.Server.Models;

/// <summary>
/// Application settings model - persisted as JSON
/// </summary>
public class AppSettings
{
    // ═══════════════════════════════════════════════════════════════════════
    // GENERAL
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Start application with Windows
    /// </summary>
    public bool StartWithWindows { get; set; } = false;
    
    /// <summary>
    /// Start minimized to system tray
    /// </summary>
    public bool StartMinimized { get; set; } = false;
    
    /// <summary>
    /// Show balloon notifications
    /// </summary>
    public bool ShowNotifications { get; set; } = true;
    
    /// <summary>
    /// Check for updates on startup
    /// </summary>
    public bool CheckForUpdates { get; set; } = true;
    
    /// <summary>
    /// Theme: "Dark" or "Light"
    /// </summary>
    public string Theme { get; set; } = "Dark";

    // ═══════════════════════════════════════════════════════════════════════
    // AUTO-BIND (Optional Feature)
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Enable auto-bind feature (disabled by default)
    /// </summary>
    public bool AutoBindEnabled { get; set; } = false;
    
    /// <summary>
    /// List of device VID:PIDs to auto-bind when connected
    /// </summary>
    public List<string> AutoBindDevices { get; set; } = new();

    // ═══════════════════════════════════════════════════════════════════════
    // SECURITY
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Enable IP whitelist restriction
    /// </summary>
    public bool WhitelistEnabled { get; set; } = false;
    
    /// <summary>
    /// List of allowed IP addresses
    /// </summary>
    public List<string> WhitelistedIPs { get; set; } = new();
    
    /// <summary>
    /// Require PIN before sharing sensitive devices
    /// </summary>
    public bool RequirePIN { get; set; } = false;
    
    /// <summary>
    /// PIN code (stored as hash in production)
    /// </summary>
    public string PIN { get; set; } = "";

    // ═══════════════════════════════════════════════════════════════════════
    // NETWORK
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Firewall rule has been configured
    /// </summary>
    public bool FirewallConfigured { get; set; } = false;

    // ═══════════════════════════════════════════════════════════════════════
    // ADVANCED
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Device list refresh interval in seconds
    /// </summary>
    public int RefreshIntervalSeconds { get; set; } = 3;
    
    /// <summary>
    /// WMI cache duration in seconds
    /// </summary>
    public int WmiCacheDurationSeconds { get; set; } = 30;
    
    /// <summary>
    /// Webhook URL for alerts (optional)
    /// </summary>
    public string WebhookUrl { get; set; } = "";
}
