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

WizardStyle=modern
SetupIconFile=win2mqtt.ico
WizardImageFile=wizard.bmp
WizardSmallImageFile=header.bmp

[Dirs]
Name: "{commonappdata}\Win2Mqtt"

[Files]
Source: "..\..\publish\windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".\README.md"; DestDir: "{app}"; Flags: isreadme
Source: ".\win2mqtt.png"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\win2mqtt.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\win2mqtt.appsettings.template.json"; DestDir: "{commonappdata}\Win2Mqtt"; Flags: ignoreversion

[Icons]
Name: "{group}\Win2MQTT Configuration file"; Filename: "{commonappdata}\Win2Mqtt\win2mqtt.appsettings.json"
Name: "{group}\Win2MQTT Readme"; Filename: "{app}\README.md"

[Run]
Filename: "sc.exe"; Parameters: "create ""Win2MQTT Service"" binPath=""{app}\Win2Mqtt.exe"" start=auto"; Description: "Create Win2Mqtt service"; Flags: runhidden
Filename: "sc.exe"; Parameters: "start ""Win2MQTT Service"""; Description: "Start Win2Mqtt service"; Flags: runhidden

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop ""Win2MQTT Service"" "; RunOnceId: "Win2MqttStop"
Filename: "sc.exe"; Parameters: "delete ""Win2MQTT Service"" "; RunOnceId: "Win2MqttUninstall"

[Code]
var
  Page1, Page2: TInputQueryWizardPage;
  MQTTServer: AnsiString;
  MQTTPort: AnsiString;
  MQTTUsername: AnsiString;
  MQTTPassword: AnsiString;
  DeviceIdentifier: AnsiString;

procedure InitializeWizard;
begin
  Page1 := CreateInputQueryPage(wpWelcome,
    'Win2Mqtt Configuration',
    'Step 1 of 2: MQTT Connection',
    'Enter the MQTT broker settings.');

  Page1.Add('MQTT Broker hostname:', False);
  Page1.Add('MQTT Broker port:', False);
  Page1.Add('MQTT Username (optional):', False);
  Page1.Add('MQTT Password (optional):', True);
  
  Page1.Values[0] := 'localhost';
  Page1.Values[1] := '1883';
  Page1.Values[2] := '';
  Page1.Values[3] := '';
  
  Page2 := CreateInputQueryPage(Page1.ID,
    'Win2Mqtt Configuration',
    'Step 2 of 2: Device Info',
    'Enter device identifier.');

  Page2.Add('Device Identifier:', False);
  
  Page2.Values[0] := ExpandConstant('{computername}');
end;


procedure CurStepChanged(CurStep: TSetupStep);
var
  TemplateFile, ConfigFile, FinalContent: String;
  TemplateContent: AnsiString;
  NeedsAuth: Boolean;


begin
  if CurStep = ssPostInstall then
  begin
    MQTTServer := Page1.Values[0];
    MQTTPort := Page1.Values[1];
    MQTTUsername := Page1.Values[2];
    MQTTPassword := Page1.Values[3];
    DeviceIdentifier := Page2.Values[0];
    
    TemplateFile := ExpandConstant('{commonappdata}\Win2Mqtt\win2mqtt.appsettings.template.json');
    ConfigFile := ExpandConstant('{commonappdata}\Win2Mqtt\win2mqtt.appsettings.json');

    if LoadStringFromFile(TemplateFile, TemplateContent) then
    begin
      NeedsAuth := (Trim(MQTTUsername) <> '') or (Trim(MQTTPassword) <> '');

      FinalContent := TemplateContent;
      StringChangeEx(FinalContent, '{{MQTT_SERVER}}', MQTTServer, True);
      StringChangeEx(FinalContent, '{{MQTT_PORT}}', MQTTPort, True);
      StringChangeEx(FinalContent, '{{MQTT_USERNAME}}', MQTTUsername, True);
      StringChangeEx(FinalContent, '{{MQTT_PASSWORD}}', MQTTPassword, True);
      StringChangeEx(FinalContent, '{{DEVICE_IDENTIFIER}}', DeviceIdentifier, True);


      if not SaveStringToFile(ConfigFile, FinalContent, False) then
        MsgBox('Failed to write config file to: ' + ConfigFile, mbError, MB_OK);
    end
    else
      MsgBox('Failed to read template config file from: ' + TemplateFile, mbError, MB_OK);
  end;
end;