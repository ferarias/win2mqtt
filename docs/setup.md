## Setup

### Microsoft Windows

Install as a **Windows Service**

First, build a a self-containing exe:

```powershell
dotnet publish .\src\Win2Mqtt\ --configuration Release --framework net8.0-windows8.0 --runtime win-x64 --self-contained  --output c:\Win2MQTT
```

Then create and start the service (you will need Administration privileges):

```powershell
sc.exe create "Win2MQTT Service" binpath= "C:\Win2MQTT\Win2Mqtt.exe"
sc.exe start "Win2MQTT Service"
```

More information in [this article](https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service)

### Linux

Install as a **Linux Service**

First, build a a self-containing exe:

```bash
dotnet publish ./src/Win2Mqtt \
--configuration Release --framework net8.0 --runtime linux-x64 \
--self-contained  --output /opt/win2mqtt
```

Then create and install as a Linux Service

### Install as a Linux Service (systemd)

To run Win2MQTT as a background service (daemon) on Linux, you can use systemd. Follow these steps:
   
Adjust the output path as needed.

2. **Create a systemd service file**

   Create a file at `/etc/systemd/system/win2mqtt.service` with the following content (edit paths if necessary):
   
   ```ini
   [Unit]
   Description=Win2MQTT Service
   After=network.target

   [Service]
   Type=simple
   WorkingDirectory=/opt/win2mqtt
   ExecStart=/usr/bin/dotnet /opt/win2mqtt/win2mqtt.dll
   Restart=on-failure
   User=win2mqtt
   Environment=ASPNETCORE_ENVIRONMENT=Production

   [Install]
   WantedBy=multi-user.target
   ```

3. **Create the service user (optional but recommended)**

   ```bash
   sudo useradd --system --no-create-home --group win2mqtt
   sudo chown -R win2mqtt:win2mqtt /opt/win2mqtt
   ```

4. **Reload systemd and enable the service**

   ```bash
   sudo systemctl daemon-reload
   sudo systemctl enable win2mqtt
   sudo systemctl start win2mqtt
   ```

5. **Check the service status**

   ```bash
   sudo systemctl status win2mqtt
   ```

Win2MQTT will now run as a background service and start automatically on boot.