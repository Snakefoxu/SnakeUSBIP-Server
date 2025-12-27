# CHANGELOG - SnakeUSBIP Server

## [1.0.0] - 2025-12-27

### Added
- **GUI Wrapper** 🖥️
  - Complete graphical interface for usbipd-win
  - Dark theme with modern design
  
- **Auto-Installation** 🔧
  - Automatically installs usbipd-win driver on first run
  - Bundled MSI for offline installation
  
- **One-Click Device Sharing** 📤
  - Share/Stop buttons for each USB device
  - No command line required
  
- **Admin Manifest** 🔒
  - Single UAC prompt at application startup
  - No additional permission dialogs during use
  
- **WMI Device Enrichment** 📛
  - Shows real product names (e.g., "CruzerBlade")
  - Combines usbipd output with Windows device info
  
- **Driver Uninstaller** 🗑️
  - Built-in button to cleanly remove usbipd-win
  - Uses winget or WMI as fallback

### Technical
- Built with .NET 8.0 WPF
- Self-contained single-file executable
- x64 Windows 10/11 support
