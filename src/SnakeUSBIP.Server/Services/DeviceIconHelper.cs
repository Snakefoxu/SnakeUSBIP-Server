// ─────────────────────────────────────────────────────────────────────────────
// SnakeUSBIP Server - Device Icon Helper
// Copyright (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server
// Licensed under GPL v3
// ─────────────────────────────────────────────────────────────────────────────

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Helper class to determine device icons based on USB class or VID:PID
/// </summary>
public static class DeviceIconHelper
{
    /// <summary>
    /// Common USB Vendor IDs for device type detection
    /// </summary>
    private static readonly Dictionary<int, string> KnownVendors = new()
    {
        // Logitech
        { 0x046D, "🎮" },
        // Microsoft
        { 0x045E, "⌨️" },
        // Razer
        { 0x1532, "🎮" },
        // SteelSeries
        { 0x1038, "🎮" },
        // Corsair
        { 0x1B1C, "🎮" },
        // Sony (PlayStation)
        { 0x054C, "🎮" },
        // Nintendo
        { 0x057E, "🎮" },
        // SanDisk
        { 0x0781, "💾" },
        // Kingston
        { 0x0951, "💾" },
        // Samsung
        { 0x04E8, "📱" },
        // Apple
        { 0x05AC, "📱" },
        // Huawei
        { 0x12D1, "📱" },
        // Xiaomi
        { 0x2717, "📱" },
        // Google
        { 0x18D1, "📱" },
        // Canon
        { 0x04A9, "📷" },
        // Nikon
        { 0x04B0, "📷" },
        // Wacom
        { 0x056A, "🖊️" },
        // Plantronics
        { 0x047F, "🎧" },
        // Jabra
        { 0x0B0E, "🎧" },
        // Blue Microphones
        { 0x0D8C, "🎤" },
        // RODE
        { 0x19F7, "🎤" },
        // Focusrite
        { 0x1235, "🎵" },
        // Behringer
        { 0x1397, "🎵" },
    };

    /// <summary>
    /// USB Class codes for device type detection
    /// </summary>
    private static readonly Dictionary<string, string> ClassIcons = new()
    {
        { "HID", "🖱️" },
        { "Mass Storage", "💾" },
        { "Audio", "🎧" },
        { "Video", "📹" },
        { "Wireless", "📶" },
        { "Bluetooth", "📶" },
        { "Hub", "🔌" },
        { "Printer", "🖨️" },
        { "Communications", "📞" },
        { "Smart Card", "💳" },
        { "Personal Healthcare", "❤️" },
        { "Image", "📷" },
    };

    /// <summary>
    /// Get icon for a device based on VID, PID, and description
    /// </summary>
    public static string GetIcon(int vendorId, int productId, string description)
    {
        // Check known vendors first
        if (KnownVendors.TryGetValue(vendorId, out var vendorIcon))
        {
            return vendorIcon;
        }

        // Check description for class hints
        var descLower = description.ToLowerInvariant();

        if (descLower.Contains("keyboard") || descLower.Contains("teclado"))
            return "⌨️";
        if (descLower.Contains("mouse") || descLower.Contains("ratón") || descLower.Contains("raton"))
            return "🖱️";
        if (descLower.Contains("gamepad") || descLower.Contains("controller") || descLower.Contains("joystick"))
            return "🎮";
        if (descLower.Contains("headset") || descLower.Contains("headphone") || descLower.Contains("auricular"))
            return "🎧";
        if (descLower.Contains("microphone") || descLower.Contains("micrófono") || descLower.Contains("microfono"))
            return "🎤";
        if (descLower.Contains("webcam") || descLower.Contains("camera") || descLower.Contains("cámara"))
            return "📹";
        if (descLower.Contains("storage") || descLower.Contains("flash") || descLower.Contains("usb drive") || descLower.Contains("pendrive"))
            return "💾";
        if (descLower.Contains("hub"))
            return "🔌";
        if (descLower.Contains("bluetooth"))
            return "📶";
        if (descLower.Contains("printer") || descLower.Contains("impresora"))
            return "🖨️";
        if (descLower.Contains("phone") || descLower.Contains("android") || descLower.Contains("móvil"))
            return "📱";
        if (descLower.Contains("tablet") || descLower.Contains("ipad"))
            return "📱";
        if (descLower.Contains("audio") || descLower.Contains("sound") || descLower.Contains("sonido"))
            return "🔊";
        if (descLower.Contains("network") || descLower.Contains("ethernet") || descLower.Contains("wifi"))
            return "🌐";

        // Default icon
        return "🔌";
    }

    /// <summary>
    /// Get a friendly device type name
    /// </summary>
    public static string GetTypeName(int vendorId, int productId, string description)
    {
        var descLower = description.ToLowerInvariant();

        if (descLower.Contains("keyboard")) return "Keyboard";
        if (descLower.Contains("mouse")) return "Mouse";
        if (descLower.Contains("gamepad") || descLower.Contains("controller")) return "Game Controller";
        if (descLower.Contains("headset") || descLower.Contains("headphone")) return "Headset";
        if (descLower.Contains("microphone")) return "Microphone";
        if (descLower.Contains("webcam") || descLower.Contains("camera")) return "Camera";
        if (descLower.Contains("storage") || descLower.Contains("flash") || descLower.Contains("drive")) return "Storage";
        if (descLower.Contains("hub")) return "USB Hub";
        if (descLower.Contains("bluetooth")) return "Bluetooth";
        if (descLower.Contains("printer")) return "Printer";
        if (descLower.Contains("phone") || descLower.Contains("android")) return "Mobile Device";
        if (descLower.Contains("audio") || descLower.Contains("sound")) return "Audio Device";

        return "USB Device";
    }
}
