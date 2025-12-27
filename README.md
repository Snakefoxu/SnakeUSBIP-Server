# 🐍 SnakeUSBIP Server - Windows USB/IP Server

🌐 **Language / Idioma:** **English** | [Español](README_ES.md)

**v1.0** | [Download Latest](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases/latest) | [📖 User Manual](docs/USAGE_EN.md)

**Share USB devices from Windows over network - No command line required.**

A beautiful GUI wrapper for [usbipd-win](https://github.com/dorssel/usbipd-win) that makes USB device sharing simple and intuitive.

[![GitHub Downloads](https://img.shields.io/github/downloads/SnakeFoxu/SnakeUSBIP-Server/total?style=flat-square&logo=github&color=blue)](https://github.com/SnakeFoxu/SnakeUSBIP-Server/releases)
[![GitHub Stars](https://img.shields.io/github/stars/SnakeFoxu/SnakeUSBIP-Server?style=flat-square&logo=github&color=yellow)](https://github.com/SnakeFoxu/SnakeUSBIP-Server/stargazers)
[![License](https://img.shields.io/github/license/SnakeFoxu/SnakeUSBIP-Server?style=flat-square&color=green)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20(x64)-lightgrey?style=flat-square&logo=windows)](https://github.com/SnakeFoxu/SnakeUSBIP-Server)

## 📸 Screenshot

![SnakeUSBIP Server](screenshot.png)

## ✨ Features

- 🔧 **Zero Configuration** - Auto-installs usbipd-win driver on first run
- 📤 **One-Click Sharing** - Share/Stop buttons for each USB device
- 🔒 **Single UAC Prompt** - Admin permission asked only once at startup
- 📛 **Smart Device Names** - Shows real product names (e.g., "CruzerBlade" not "USB Storage Device")
- 🗑️ **Clean Uninstall** - Built-in option to remove drivers
- 🎨 **Dark Theme** - Modern, eye-friendly interface
- ⚡ **Self-Contained** - No .NET runtime installation required

## 📦 Installation

1. Download `SnakeUSBIP-Server-v1.0.zip` from [Releases](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases/latest)
2. Extract to any folder
3. Run `SnakeUSBIP-Server.exe` as Administrator
4. Done! The driver will be installed automatically if needed.

## 🚀 Quick Start

### Share a USB Device
1. Launch the application (will request admin privileges)
2. Your USB devices appear in the list
3. Click **📤 Share** on any device
4. Device is now accessible from any USB/IP client on your network

### Stop Sharing
1. Click **🚫 Stop** on the device you want to stop sharing
2. Device returns to normal local-only access

### Connect from Client
Use [SnakeUSBIP Client](https://github.com/Snakefoxu/SnakeUSBIP) to connect from another Windows PC.

```
1. Open SnakeUSBIP Client
2. Enter your server's IP address
3. Click List → Connect to shared devices
```

## 🔗 Related Projects

| Project | Description |
|---------|-------------|
| [SnakeUSBIP (Client)](https://github.com/Snakefoxu/SnakeUSBIP) | Windows client to connect to USB/IP servers |
| [usbipd-win](https://github.com/dorssel/usbipd-win) | Core USB/IP implementation for Windows |

## 📁 Files Included

```
SnakeUSBIP-Server/
├── SnakeUSBIP-Server.exe    # Main application (.NET 8 WPF)
├── drivers/
│   └── usbipd-win.msi       # Auto-installed driver package
└── *.dll                    # WPF native dependencies
```

## ⚙️ Requirements

- Windows 10/11 (x64)
- Administrator privileges
- Network connection (LAN/WiFi)

## 🔧 Troubleshooting

### Driver not installing?
- Run the application as Administrator
- Check if Windows Update is pending a restart
- Try manual installation: `drivers\usbipd-win.msi`

### Device not appearing in client?
- Ensure both PCs are on the same network
- Check if Windows Firewall allows port 3240 (TCP)
- Verify the device is "Shared" status in the server

### Uninstall driver
- Click the **🗑️ Uninstall Driver** button in the app
- Or: `winget uninstall dorssel.usbipd-win`

## 📄 License

GPL v3 (GNU General Public License) - See [LICENSE](LICENSE)

## 🙏 Credits

| Project | Author | Contribution |
|---------|--------|--------------|
| [usbipd-win](https://github.com/dorssel/usbipd-win) | **Frans van Dorsselaer** | Core USB/IP driver for Windows |
| **SnakeUSBIP Server** | **SnakeFoxu** | GUI wrapper, auto-install, WMI enrichment |

---

Made with 🐍 by [SnakeFoxu](https://github.com/SnakeFoxu)
