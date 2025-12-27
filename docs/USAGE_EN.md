# SnakeUSBIP Server - User Manual

🌐 **Language:** English | [Español](USAGE_ES.md)

## Table of Contents

1. [Introduction](#introduction)
2. [Installation](#installation)
3. [First Run](#first-run)
4. [Sharing USB Devices](#sharing-usb-devices)
5. [Connecting from Client](#connecting-from-client)
6. [Troubleshooting](#troubleshooting)
7. [Uninstallation](#uninstallation)

---

## Introduction

SnakeUSBIP Server allows you to share USB devices connected to your Windows PC over the network. Other computers can then connect to these devices as if they were plugged in locally.

### Use Cases

- **3D Printers**: Share your printer from any PC in the house
- **License Dongles**: Share software protection keys
- **Scanners**: Use from any workstation
- **Webcams**: Stream from a centralized location
- **Arduino/Development Boards**: Flash from any computer

---

## Installation

### Requirements
- Windows 10/11 (64-bit)
- Administrator privileges
- Local network connection

### Steps

1. Download the latest release from [GitHub Releases](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases)
2. Extract the ZIP file to any folder (e.g., `C:\SnakeUSBIP-Server\`)
3. No installation wizard needed - it's portable!

---

## First Run

1. **Right-click** on `SnakeUSBIP-Server.exe`
2. Select **"Run as administrator"**
3. Windows will ask for permission - click **Yes**

### First-Time Driver Installation

If usbipd-win is not installed, the application will:
1. Detect the missing driver
2. Automatically install it from the bundled MSI
3. Log the installation progress

> **Note**: This only happens once. The driver persists after installation.

---

## Sharing USB Devices

### Device List

When the application starts, you'll see a list of all USB devices:

| Column | Description |
|--------|-------------|
| **Bus ID** | USB port identifier (e.g., "1-3") |
| **VID:PID** | Vendor and Product ID |
| **Device Name** | Friendly name from Windows |
| **Status** | Not shared / Shared / Attached |
| **Action** | Share or Stop button |

### Share a Device

1. Find the device you want to share
2. Click the **📤 Share** button
3. Status changes to "Shared"
4. The device is now available on port 3240

### Stop Sharing

1. Find the shared device
2. Click the **🚫 Stop** button
3. Status returns to "Not shared"

---

## Connecting from Client

### Using SnakeUSBIP Client (Recommended)

1. Download [SnakeUSBIP Client](https://github.com/Snakefoxu/SnakeUSBIP)
2. Enter your server's IP address
3. Click **Scan** or **List**
4. Double-click a device to connect

### Using Command Line

```cmd
usbip attach -r SERVER_IP -b BUS_ID
```

Example:
```cmd
usbip attach -r 192.168.1.100 -b 1-3
```

---

## Troubleshooting

### Device shows "Not shared" but won't share

**Cause**: Device may be in use by Windows

**Solution**:
1. Close any applications using the device
2. Click Refresh
3. Try sharing again

### Client can't see the server

**Cause**: Firewall blocking port 3240

**Solution**:
1. Open Windows Defender Firewall
2. Allow incoming TCP port 3240
3. Or temporarily disable firewall for testing

### Driver installation fails

**Cause**: Pending Windows Update

**Solution**:
1. Restart your computer
2. Try again
3. If still failing, manually run `drivers\usbipd-win.msi`

---

## Uninstallation

### Remove the Driver

1. Open SnakeUSBIP Server
2. Click the **🗑️ Uninstall Driver** button
3. Confirm when prompted
4. Driver will be removed via winget

### Manual Uninstall

```powershell
winget uninstall dorssel.usbipd-win
```

### Delete Application

Simply delete the SnakeUSBIP-Server folder. The application is fully portable and leaves no registry entries.

---

## Need Help?

- [GitHub Issues](https://github.com/Snakefoxu/SnakeUSBIP-Server/issues)
- [SnakeUSBIP Client Documentation](https://github.com/Snakefoxu/SnakeUSBIP)
