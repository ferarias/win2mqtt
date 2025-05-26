#ifndef MyAppVersion
  #define MyAppVersion "0.0.0"
#endif
[Setup]
AppName=Win2Mqtt
AppVersion={#MyAppVersion}
DefaultDirName={commonpf}\Win2Mqtt
DefaultGroupName=Win2Mqtt
OutputDir=..\..\publish\setup
OutputBaseFilename=Win2MqttSetup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Files]
Source: "..\..\publish\windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Win2Mqtt Service"; Filename: "{app}\Win2Mqtt.exe"; Parameters: "/install"; WorkingDir: "{app}"

[Run]
Filename: "{app}\Win2Mqtt.exe"; Parameters: "/install"; Description: "Install Win2Mqtt service"; Flags: runhidden

[UninstallRun]
Filename: "{app}\Win2Mqtt.exe"; Parameters: "/uninstall"; RunOnceId: "Win2MqttUninstall"
