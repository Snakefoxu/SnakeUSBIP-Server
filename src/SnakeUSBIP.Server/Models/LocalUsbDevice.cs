// SnakeUSBIP Server - github.com/SnakeFoxu/SnakeUSBIP-Server
// Copyright 2025 SnakeFoxu - GPL v3

namespace SnakeUSBIP.Server.Models;

/// <summary>
/// Represents a local USB device that can be shared over the network
/// Part of SnakeUSBIP Server by SnakeFoxu
/// </summary>
public class LocalUsbDevice
{
    /// <summary>USB Vendor ID (VID)</summary>
    public int VendorId { get; set; }

    /// <summary>USB Product ID (PID)</summary>
    public int ProductId { get; set; }

    /// <summary>USB/IP Bus ID (e.g., "1-1")</summary>
    public string BusId { get; set; } = "";

    /// <summary>Human-readable device name</summary>
    public string Name { get; set; } = "";

    /// <summary>Device description</summary>
    public string Description { get; set; } = "";

    /// <summary>Device status from usbipd (Not shared, Shared, Attached)</summary>
    public string Status { get; set; } = "Not shared";

    /// <summary>Whether device is currently shared</summary>
    public bool IsExported { get; set; }

    /// <summary>Whether device is NOT shared (for button visibility)</summary>
    public bool IsNotShared { get; set; } = true;

    /// <summary>VID:PID formatted string</summary>
    public string VidPid => $"{VendorId:X4}:{ProductId:X4}";

    public override string ToString() => $"{BusId}: {Name} ({VidPid})";
}
