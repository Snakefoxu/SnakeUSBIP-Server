# SnakeUSBIP Server - Manual de Usuario

🌐 **Idioma:** [English](USAGE_EN.md) | Español

## Tabla de Contenidos

1. [Introducción](#introducción)
2. [Instalación](#instalación)
3. [Primer Arranque](#primer-arranque)
4. [Compartir Dispositivos USB](#compartir-dispositivos-usb)
5. [Conectar desde Cliente](#conectar-desde-cliente)
6. [Solución de Problemas](#solución-de-problemas)
7. [Desinstalación](#desinstalación)

---

## Introducción

SnakeUSBIP Server te permite compartir dispositivos USB conectados a tu PC Windows a través de la red. Otros ordenadores pueden conectarse a estos dispositivos como si estuvieran enchufados localmente.

### Casos de Uso

- **Impresoras 3D**: Comparte tu impresora desde cualquier PC de casa
- **Dongles de Licencia**: Comparte llaves de protección de software
- **Escáneres**: Usa desde cualquier estación de trabajo
- **Webcams**: Transmite desde una ubicación centralizada
- **Arduino/Placas de Desarrollo**: Programa desde cualquier ordenador

---

## Instalación

### Requisitos
- Windows 10/11 (64-bit)
- Permisos de Administrador
- Conexión de red local

### Pasos

1. Descarga la última versión desde [GitHub Releases](https://github.com/Snakefoxu/SnakeUSBIP-Server/releases)
2. Extrae el archivo ZIP en cualquier carpeta (ej: `C:\SnakeUSBIP-Server\`)
3. ¡No necesita instalador - es portable!

---

## Primer Arranque

1. **Click derecho** en `SnakeUSBIP-Server.exe`
2. Selecciona **"Ejecutar como administrador"**
3. Windows pedirá permiso - click en **Sí**

### Instalación del Driver (Primera Vez)

Si usbipd-win no está instalado, la aplicación:
1. Detectará el driver faltante
2. Lo instalará automáticamente desde el MSI incluido
3. Mostrará el progreso en el log

> **Nota**: Esto solo ocurre una vez. El driver persiste tras la instalación.

---

## Compartir Dispositivos USB

### Lista de Dispositivos

Al iniciar la aplicación, verás una lista de todos los dispositivos USB:

| Columna | Descripción |
|---------|-------------|
| **Bus ID** | Identificador del puerto USB (ej: "1-3") |
| **VID:PID** | ID de Fabricante y Producto |
| **Device Name** | Nombre amigable de Windows |
| **Status** | Not shared / Shared / Attached |
| **Action** | Botón Share o Stop |

### Compartir un Dispositivo

1. Encuentra el dispositivo que quieres compartir
2. Click en el botón **📤 Share**
3. El estado cambia a "Shared"
4. El dispositivo ahora está disponible en puerto 3240

### Dejar de Compartir

1. Encuentra el dispositivo compartido
2. Click en el botón **🚫 Stop**
3. El estado vuelve a "Not shared"

---

## Conectar desde Cliente

### Usando SnakeUSBIP Client (Recomendado)

1. Descarga [SnakeUSBIP Client](https://github.com/Snakefoxu/SnakeUSBIP)
2. Introduce la IP de tu servidor
3. Click en **Escanear** o **Listar**
4. Doble-click en un dispositivo para conectar

### Usando Línea de Comandos

```cmd
usbip attach -r IP_SERVIDOR -b BUS_ID
```

Ejemplo:
```cmd
usbip attach -r 192.168.1.100 -b 1-3
```

---

## Solución de Problemas

### El dispositivo muestra "Not shared" pero no comparte

**Causa**: El dispositivo puede estar en uso por Windows

**Solución**:
1. Cierra cualquier aplicación usando el dispositivo
2. Click en Refresh
3. Intenta compartir de nuevo

### El cliente no ve el servidor

**Causa**: Firewall bloqueando puerto 3240

**Solución**:
1. Abre Windows Defender Firewall
2. Permite entrada por puerto TCP 3240
3. O desactiva temporalmente el firewall para probar

### La instalación del driver falla

**Causa**: Windows Update pendiente

**Solución**:
1. Reinicia tu ordenador
2. Intenta de nuevo
3. Si sigue fallando, ejecuta manualmente `drivers\usbipd-win.msi`

---

## Desinstalación

### Eliminar el Driver

1. Abre SnakeUSBIP Server
2. Click en el botón **🗑️ Uninstall Driver**
3. Confirma cuando se te pregunte
4. El driver se eliminará via winget

### Desinstalación Manual

```powershell
winget uninstall dorssel.usbipd-win
```

### Eliminar Aplicación

Simplemente borra la carpeta SnakeUSBIP-Server. La aplicación es completamente portable y no deja entradas en el registro.

---

## ¿Necesitas Ayuda?

- [GitHub Issues](https://github.com/Snakefoxu/SnakeUSBIP-Server/issues)
- [Documentación del Cliente SnakeUSBIP](https://github.com/Snakefoxu/SnakeUSBIP)
