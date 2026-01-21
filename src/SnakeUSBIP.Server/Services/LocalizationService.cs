namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Localization service - Translation dictionaries ES/EN
/// Ported from SnakeUSBIP Client
/// </summary>
public class LocalizationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations;
    
    public LocalizationService()
    {
        _translations = new Dictionary<string, Dictionary<string, string>>
        {
            ["es"] = new Dictionary<string, string>
            {
                // Window
                ["title"] = "SnakeUSBIP Server",
                ["subtitle"] = "GUI para usbipd-win",
                
                // Buttons
                ["btn_update"] = "⬆️ Actualizar",
                ["btn_settings"] = "⚙️ Ajustes",
                ["btn_tray"] = "🔽 Minimizar",
                ["btn_diagnostics"] = "🔍 Diagnóstico",
                ["btn_refresh"] = "🔄 Actualizar Lista",
                ["btn_uninstall"] = "🗑️ Desinstalar Driver",
                ["btn_share"] = "📤 Compartir",
                ["btn_stop"] = "🛑 Detener",
                
                // Headers
                ["header_devices"] = "📱 Dispositivos USB",
                ["header_log"] = "📋 Registro de Actividad",
                
                // Columns
                ["col_icon"] = "",
                ["col_type"] = "Tipo",
                ["col_busid"] = "Bus ID",
                ["col_device"] = "Dispositivo",
                ["col_vid_pid"] = "VID:PID",
                ["col_status"] = "Estado",
                ["col_action"] = "Acción",
                
                // Status
                ["status_shared"] = "Compartido",
                ["status_not_shared"] = "No Compartido",
                ["status_ready"] = "Listo",
                ["status_running"] = "Servidor ejecutándose (servicio usbipd activo)",
                ["status_not_available"] = "usbipd-win no disponible",
                
                // Badges
                ["badge_admin"] = "ADMIN",
                ["badge_user"] = "USUARIO",
                ["badge_portable"] = "PORTABLE",
                ["badge_installed"] = "INSTALADO",
                
                // Update
                ["update_checking"] = "Comprobando actualizaciones...",
                ["update_available"] = "Nueva versión disponible",
                ["update_downloading"] = "Descargando actualización...",
                ["update_current"] = "Tienes la versión más reciente",
                ["update_error"] = "Error comprobando actualizaciones",
                ["update_confirm_title"] = "Actualización Disponible",
                ["update_confirm_msg"] = "Nueva versión disponible: v{0}\n\n{1}\n\n¿Desea descargar e instalar la actualización?",
                
                // Log messages
                ["log_server_started"] = "🐍 SnakeUSBIP Server v2.0 - GUI para usbipd-win",
                ["log_device_shared"] = "✅ Dispositivo compartido: {0}",
                ["log_device_stopped"] = "🛑 Dispositivo detenido: {0}",
                ["log_share_failed"] = "❌ Error al compartir: {0}",
                ["log_stop_failed"] = "❌ Error al detener: {0}",
                ["log_driver_uninstalling"] = "🗑️ Desinstalando usbipd-win...",
                ["log_driver_uninstalled"] = "✅ usbipd-win desinstalado. Puedes eliminar esta carpeta.",
                ["log_update_checking"] = "🔍 Comprobando actualizaciones...",
                ["log_update_available"] = "⬆️ Actualización disponible: v{0}",
                ["log_update_downloading"] = "📥 Descargando actualización...",
                ["log_update_failed"] = "❌ Error en actualización: {0}",
                ["log_update_current"] = "✅ Tienes la versión más reciente (v{0})",
                
                // Settings
                ["settings_title"] = "Ajustes",
                ["settings_general"] = "General",
                ["settings_language"] = "Idioma",
                ["settings_theme"] = "Tema",
                ["settings_theme_dark"] = "Oscuro",
                ["settings_theme_light"] = "Claro",
                ["settings_refresh_interval"] = "Intervalo de actualización (segundos)",
                ["settings_start_minimized"] = "Iniciar minimizado",
                ["settings_start_with_windows"] = "Iniciar con Windows",
                ["settings_show_notifications"] = "Mostrar notificaciones",
                ["settings_firewall"] = "Firewall",
                ["settings_firewall_configure"] = "🛡️ Configurar Firewall",
                ["settings_firewall_remove"] = "🗑️ Eliminar Regla",
                ["settings_export"] = "📤 Exportar",
                ["settings_import"] = "📥 Importar",
                ["settings_reset"] = "🔄 Restablecer",
                ["settings_cancel"] = "Cancelar",
                ["settings_save"] = "💾 Guardar",
                
                // Dialogs
                ["dialog_confirm"] = "Confirmar",
                ["dialog_uninstall_title"] = "Desinstalar usbipd-win",
                ["dialog_uninstall_msg"] = "Esto desinstalará el driver usbipd-win y eliminará la funcionalidad del servidor USB/IP.\n\n¿Está seguro?",
                
                // Tray
                ["tray_open"] = "Abrir SnakeUSBIP",
                ["tray_exit"] = "Salir",
                ["tray_minimized"] = "SnakeUSBIP Server minimizado a la bandeja del sistema"
            },
            
            ["en"] = new Dictionary<string, string>
            {
                // Window
                ["title"] = "SnakeUSBIP Server",
                ["subtitle"] = "GUI for usbipd-win",
                
                // Buttons
                ["btn_update"] = "⬆️ Update",
                ["btn_settings"] = "⚙️ Settings",
                ["btn_tray"] = "🔽 To Tray",
                ["btn_diagnostics"] = "🔍 Diagnostics",
                ["btn_refresh"] = "🔄 Refresh",
                ["btn_uninstall"] = "🗑️ Uninstall Driver",
                ["btn_share"] = "📤 Share",
                ["btn_stop"] = "🛑 Stop",
                
                // Headers
                ["header_devices"] = "📱 USB Devices",
                ["header_log"] = "📋 Activity Log",
                
                // Columns
                ["col_icon"] = "",
                ["col_type"] = "Type",
                ["col_busid"] = "Bus ID",
                ["col_device"] = "Device",
                ["col_vid_pid"] = "VID:PID",
                ["col_status"] = "Status",
                ["col_action"] = "Action",
                
                // Status
                ["status_shared"] = "Shared",
                ["status_not_shared"] = "Not Shared",
                ["status_ready"] = "Ready",
                ["status_running"] = "Server running (usbipd service active)",
                ["status_not_available"] = "usbipd-win not available",
                
                // Badges
                ["badge_admin"] = "ADMIN",
                ["badge_user"] = "USER",
                ["badge_portable"] = "PORTABLE",
                ["badge_installed"] = "INSTALLED",
                
                // Update
                ["update_checking"] = "Checking for updates...",
                ["update_available"] = "New version available",
                ["update_downloading"] = "Downloading update...",
                ["update_current"] = "You have the latest version",
                ["update_error"] = "Error checking updates",
                ["update_confirm_title"] = "Update Available",
                ["update_confirm_msg"] = "New version available: v{0}\n\n{1}\n\nDo you want to download and install the update?",
                
                // Log messages
                ["log_server_started"] = "🐍 SnakeUSBIP Server v2.0 - GUI for usbipd-win",
                ["log_device_shared"] = "✅ Device shared: {0}",
                ["log_device_stopped"] = "🛑 Device stopped: {0}",
                ["log_share_failed"] = "❌ Share failed: {0}",
                ["log_stop_failed"] = "❌ Stop failed: {0}",
                ["log_driver_uninstalling"] = "🗑️ Uninstalling usbipd-win...",
                ["log_driver_uninstalled"] = "✅ usbipd-win uninstalled. You can delete this app folder.",
                ["log_update_checking"] = "🔍 Checking for updates...",
                ["log_update_available"] = "⬆️ Update available: v{0}",
                ["log_update_downloading"] = "📥 Downloading update...",
                ["log_update_failed"] = "❌ Update failed: {0}",
                ["log_update_current"] = "✅ You have the latest version (v{0})",
                
                // Settings
                ["settings_title"] = "Settings",
                ["settings_general"] = "General",
                ["settings_language"] = "Language",
                ["settings_theme"] = "Theme",
                ["settings_theme_dark"] = "Dark",
                ["settings_theme_light"] = "Light",
                ["settings_refresh_interval"] = "Refresh interval (seconds)",
                ["settings_start_minimized"] = "Start minimized",
                ["settings_start_with_windows"] = "Start with Windows",
                ["settings_show_notifications"] = "Show notifications",
                ["settings_firewall"] = "Firewall",
                ["settings_firewall_configure"] = "🛡️ Configure Firewall",
                ["settings_firewall_remove"] = "🗑️ Remove Rule",
                ["settings_export"] = "📤 Export",
                ["settings_import"] = "📥 Import",
                ["settings_reset"] = "🔄 Reset",
                ["settings_cancel"] = "Cancel",
                ["settings_save"] = "💾 Save",
                
                // Dialogs
                ["dialog_confirm"] = "Confirm",
                ["dialog_uninstall_title"] = "Uninstall usbipd-win",
                ["dialog_uninstall_msg"] = "This will uninstall usbipd-win driver and remove USB/IP server functionality.\n\nAre you sure?",
                
                // Tray
                ["tray_open"] = "Open SnakeUSBIP",
                ["tray_exit"] = "Exit",
                ["tray_minimized"] = "SnakeUSBIP Server minimized to system tray"
            }
        };
    }
    
    public string GetText(string key, string language = "en")
    {
        if (_translations.TryGetValue(language, out var langDict))
        {
            if (langDict.TryGetValue(key, out var text))
                return text;
        }
        
        // Fallback to English
        if (_translations.TryGetValue("en", out var enDict))
        {
            if (enDict.TryGetValue(key, out var text))
                return text;
        }
        
        return key; // Return key if not found
    }
    
    public string GetText(string key, string language, params object[] args)
    {
        var text = GetText(key, language);
        try
        {
            return string.Format(text, args);
        }
        catch
        {
            return text;
        }
    }
    
    public static string DetectSystemLanguage()
    {
        var systemLang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        return systemLang == "es" ? "es" : "en";
    }
}
