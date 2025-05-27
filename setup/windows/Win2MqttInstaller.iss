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
SetupIconFile=win2mqtt.ico


[Files]
Source: "..\..\publish\windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\..\README.md"; DestDir: "{app}"; Flags: isreadme
Source: ".\win2mqtt.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\win2mqtt.png"; DestDir: "{app}"; Flags: ignoreversion

[Run]
Filename: "sc.exe"; Parameters: "create ""Win2MQTT Service"" binPath=""{app}\Win2Mqtt.exe"" start=auto"; Description: "Create Win2Mqtt service"; Flags: runhidden
Filename: "sc.exe"; Parameters: "start ""Win2MQTT Service"""; Description: "Start Win2Mqtt service"; Flags: runhidden

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop ""Win2MQTT Service"" "; RunOnceId: "Win2MqttStop"
Filename: "sc.exe"; Parameters: "delete ""Win2MQTT Service"" "; RunOnceId: "Win2MqttUninstall"
