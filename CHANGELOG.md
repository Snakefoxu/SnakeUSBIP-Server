# CHANGELOG - SnakeUSBIP Server

## [1.1.1] - 2026-01-13

### Fixed
- **Instant Share/Stop** ⚡
  - Share and Stop buttons now respond instantly (was 16+ seconds)
  - UI no longer freezes during operations
  - Fire & Forget architecture with auto-refresh

### Technical
- Refactored `RunAdminCommand` to non-blocking execution
- Auto-refresh (3s) updates device state automatically

---
## [1.1.0] - 2026-01-06

### Added
- **Path Management Service** 🗂️
  - Automatic Portable/Installed mode detection
  - Centralized path handling for logs, themes, and config
  - Full transparency via Diagnostics button

- **Persistent Logging** 📝
  - All logs saved to `./logs/` (Portable) or `%AppData%/SnakeUSBIP/logs/` (Installed)
  - 7-day auto-cleanup of old logs
  - Crash-safe logging design

- **Global Exception Handling** 🛡️
  - UI thread, background thread, and async task exceptions captured
  - User-friendly error dialogs instead of silent crashes
  - All exceptions logged for debugging

- **Security Improvements** 🔒
  - Input validation for Bus IDs before command execution
  - Command injection protection via SecurityHelper

- **UI Enhancements** ✨
  - Admin Mode indicator (green ADMIN / red USER badge)
  - Mode indicator (blue PORTABLE / purple INSTALLED badge)
  - Diagnostics button showing all active paths

### Changed
- **Driver Installation** now asks for user confirmation before installing
- Version bumped to 1.1.0

### Technical
- New services: `PathService.cs`, `LogService.cs`, `SecurityHelper.cs`
- Updated `App.xaml.cs` with robust exception handling
- Input sanitization in `UsbipdService.cs`

---
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
