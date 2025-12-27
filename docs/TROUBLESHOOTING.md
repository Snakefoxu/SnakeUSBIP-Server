# Troubleshooting Guide

## Common Issues

### 1. Application won't start

**Symptoms**: Nothing happens when double-clicking the executable

**Causes**:
- Not running as administrator
- Antivirus blocking the application
- Missing WPF dependencies

**Solutions**:
1. Right-click → "Run as administrator"
2. Add exception in your antivirus
3. Install [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (usually not needed - app is self-contained)

---

### 2. Driver installation fails

**Symptoms**: Log shows "❌ Failed to install"

**Causes**:
- Windows Update pending restart
- Corrupted Windows Installer service
- Insufficient disk space

**Solutions**:
1. Restart Windows
2. Run `sfc /scannow` in admin cmd
3. Free up disk space
4. Manual install: `msiexec /i drivers\usbipd-win.msi`

---

### 3. No devices listed

**Symptoms**: Device list is empty

**Causes**:
- usbipd-win not installed
- usbipd service not running
- No USB devices connected

**Solutions**:
1. Check if driver installed: `usbipd --version`
2. Start service: `sc start usbipd`
3. Connect a USB device and click Refresh

---

### 4. Share button doesn't work

**Symptoms**: Click Share but status doesn't change

**Causes**:
- Device in use by another application
- Device doesn't support USB/IP
- Insufficient privileges

**Solutions**:
1. Close applications using the device
2. Some devices (keyboards, mice) may not share properly
3. Ensure running as administrator

---

### 5. Client can't connect

**Symptoms**: Client sees server but can't attach devices

**Causes**:
- Firewall blocking port 3240
- Device not actually shared
- Network connectivity issues

**Solutions**:
1. Allow port 3240 in Windows Firewall:
   ```powershell
   New-NetFirewallRule -DisplayName "usbipd" -Direction Inbound -Protocol TCP -LocalPort 3240 -Action Allow
   ```
2. Verify device shows "Shared" status
3. Test network: `ping SERVER_IP`

---

## Logs

### Enable Debug Logging

The application logs to the text area in the main window. For detailed system logs:

```powershell
# View usbipd service logs
Get-WinEvent -LogName Application | Where-Object { $_.ProviderName -like "*usbipd*" }
```

### Common Log Messages

| Message | Meaning |
|---------|---------|
| `✅ usbipd-win detected` | Driver installed and working |
| `📦 Installing driver...` | First-time installation |
| `📤 Sharing device...` | Bind operation in progress |
| `❌ Failed to bind` | Share operation failed |

---

## Getting Help

1. Check [GitHub Issues](https://github.com/Snakefoxu/SnakeUSBIP-Server/issues)
2. Search for similar problems
3. Create a new issue with:
   - Windows version
   - Error message from log
   - Steps to reproduce
