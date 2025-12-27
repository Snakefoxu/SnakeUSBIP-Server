# 🐍 SnakeUSBIP Server - Servidor USB/IP para Windows

🌐 **Idioma / Language:** [English](README.md) | **Español**

**v1.0** | [Descargar Última Versión](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases/latest) | [📖 Manual de Usuario](docs/USAGE_ES.md)

**Comparte dispositivos USB desde Windows por red - Sin línea de comandos.**

Una interfaz gráfica para [usbipd-win](https://github.com/dorssel/usbipd-win) que hace que compartir dispositivos USB sea simple e intuitivo.

[![GitHub Downloads](https://img.shields.io/github/downloads/SnakeFoxu/SnakeUSBIP-Server/total?style=flat-square&logo=github&color=blue)](https://github.com/SnakeFoxu/SnakeUSBIP-Server/releases)
[![GitHub Stars](https://img.shields.io/github/stars/SnakeFoxu/SnakeUSBIP-Server?style=flat-square&logo=github&color=yellow)](https://github.com/SnakeFoxu/SnakeUSBIP-Server/stargazers)
[![License](https://img.shields.io/github/license/SnakeFoxu/SnakeUSBIP-Server?style=flat-square&color=green)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20(x64)-lightgrey?style=flat-square&logo=windows)](https://github.com/SnakeFoxu/SnakeUSBIP-Server)

## 📸 Captura de Pantalla

![SnakeUSBIP Server](screenshot.png)

## ✨ Características

- 🔧 **Cero Configuración** - Auto-instala el driver usbipd-win en el primer arranque
- 📤 **Compartir con Un Click** - Botones Compartir/Detener para cada dispositivo USB
- 🔒 **Un Solo Permiso UAC** - Se pide permiso admin solo una vez al iniciar
- 📛 **Nombres Inteligentes** - Muestra nombres reales (ej: "CruzerBlade" no "Dispositivo USB")
- 🗑️ **Desinstalación Limpia** - Opción integrada para eliminar drivers
- 🎨 **Tema Oscuro** - Interfaz moderna y agradable a la vista
- ⚡ **Auto-Contenido** - No requiere instalar .NET runtime

## 📦 Instalación

1. Descarga `SnakeUSBIP-Server-v1.0.zip` desde [Releases](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases/latest)
2. Extrae en cualquier carpeta
3. Ejecuta `SnakeUSBIP-Server.exe` como Administrador
4. ¡Listo! El driver se instalará automáticamente si es necesario.

## 🚀 Inicio Rápido

### Compartir un Dispositivo USB
1. Inicia la aplicación (pedirá permisos de administrador)
2. Tus dispositivos USB aparecerán en la lista
3. Click en **📤 Share** en cualquier dispositivo
4. El dispositivo ahora es accesible desde cualquier cliente USB/IP en tu red

### Dejar de Compartir
1. Click en **🚫 Stop** en el dispositivo que quieres dejar de compartir
2. El dispositivo vuelve a acceso local normal

### Conectar desde Cliente
Usa [SnakeUSBIP Client](https://github.com/Snakefoxu/SnakeUSBIP) para conectar desde otro PC Windows.

```
1. Abre SnakeUSBIP Client
2. Introduce la IP de tu servidor
3. Click en Listar → Conectar a dispositivos compartidos
```

## 🔗 Proyectos Relacionados

| Proyecto | Descripción |
|----------|-------------|
| [SnakeUSBIP (Cliente)](https://github.com/Snakefoxu/SnakeUSBIP) | Cliente Windows para conectar a servidores USB/IP |
| [usbipd-win](https://github.com/dorssel/usbipd-win) | Implementación core USB/IP para Windows |

## 📁 Archivos Incluidos

```
SnakeUSBIP-Server/
├── SnakeUSBIP-Server.exe    # Aplicación principal (.NET 8 WPF)
├── drivers/
│   └── usbipd-win.msi       # Paquete de driver auto-instalado
└── *.dll                    # Dependencias nativas WPF
```

## ⚙️ Requisitos

- Windows 10/11 (x64)
- Permisos de Administrador
- Conexión de red (LAN/WiFi)

## 🔧 Solución de Problemas

### ¿El driver no se instala?
- Ejecuta la aplicación como Administrador
- Verifica si Windows Update tiene reinicio pendiente
- Intenta instalación manual: `drivers\usbipd-win.msi`

### ¿El dispositivo no aparece en el cliente?
- Asegúrate de que ambos PCs estén en la misma red
- Verifica que el Firewall de Windows permita puerto 3240 (TCP)
- Confirma que el dispositivo está en estado "Shared" en el servidor

### Desinstalar driver
- Click en el botón **🗑️ Uninstall Driver** en la app
- O: `winget uninstall dorssel.usbipd-win`

## 📄 Licencia

GPL v3 (GNU General Public License) - Ver [LICENSE](LICENSE)

## 🙏 Créditos

| Proyecto | Autor | Contribución |
|----------|-------|--------------|
| [usbipd-win](https://github.com/dorssel/usbipd-win) | **Frans van Dorsselaer** | Driver core USB/IP para Windows |
| **SnakeUSBIP Server** | **SnakeFoxu** | GUI wrapper, auto-instalación, enriquecimiento WMI |

---

Hecho con 🐍 por [SnakeFoxu](https://github.com/SnakeFoxu)
