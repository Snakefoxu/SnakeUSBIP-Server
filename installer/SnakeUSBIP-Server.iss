; SnakeUSBIP Server Installer Script
; Inno Setup 6
; (c) 2025 SnakeFoxu - https://github.com/SnakeFoxu/SnakeUSBIP-Server

#define MyAppName "SnakeUSBIP Server"
#define MyAppVersion "2.0.0"
#define MyAppPublisher "SnakeFoxu"
#define MyAppURL "https://github.com/SnakeFoxu/SnakeUSBIP-Server"
#define MyAppExeName "SnakeUSBIP-Server.exe"
#define SourceDir "d:\REPOS_GITHUB\SnakeUSBIP-Server\github-upload\release\temp_x64\Portable"

[Setup]
AppId={{B2C3D4E5-F6A7-8901-BCDE-F12345678901}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile={#SourceDir}\LICENSE
OutputDir=d:\REPOS_GITHUB\SnakeUSBIP-Server\github-upload\release
OutputBaseFilename=SnakeUSBIP-Server_Setup_v{#MyAppVersion}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startupicon"; Description: "Iniciar con Windows / Start with Windows"; GroupDescription: "Options:"; Flags: unchecked

[Files]
; Main application
Source: "{#SourceDir}\SnakeUSBIP-Server.exe"; DestDir: "{app}"; Flags: ignoreversion

; Documentation
Source: "{#SourceDir}\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\CHANGELOG.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
; Start with Windows (if selected)
Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: """{app}\{#MyAppExeName}"" --minimized"; Flags: uninsdeletevalue; Tasks: startupicon

[Run]
; Launch after install
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent runascurrentuser

[UninstallDelete]
; Clean up settings and logs on uninstall
Type: filesandordirs; Name: "{app}\logs"
Type: files; Name: "{app}\settings.json"

[Code]
function InitializeSetup(): Boolean;
begin
  Result := True;
  if RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{B2C3D4E5-F6A7-8901-BCDE-F12345678901}_is1') then
  begin
    if MsgBox('SnakeUSBIP Server is already installed. Continue?', mbConfirmation, MB_YESNO) = IDNO then
      Result := False;
  end;
end;
