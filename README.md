# Windows to MQTT

A Windows service that exposes system sensors to MQTT so that they can be consumed from IOT applications such as Home Assistant

[Roadmap](./Roadmap.md)

## Sensors

All sensors start with the prefix `win2mqtt/{hostname}`

### Disk drives sensors

Status of each mounted drive in the system

`win2mqtt/{hostname}/drive`

A subtopic with each drive letter, each having the following subtopics:

- `sizetotal`
- `sizefree`
- `percentfree`

Example : `win2mqtt/lechuck/drive/c/sizetotal` → `13455527`

### Memory sensors

Returns available memory in Megabytes

`win2mqtt/{hostname}/freememory `

Example : `win2mqtt/lechuck/freememory` → `234`

### Network sensors

Get network status: `1` if available, else `0`

`win2mqtt/{hostname}/binary_sensor/network_available`

Example : `win2mqtt/lechuck/binary_sensor/network_available` → `1`

#### In use

`win2mqtt/{hostname}/binary_sensor/inuse`: `on` if the system has had some input for the last 30 seconds, else `off`

Example : `win2mqtt/lechuck/binary_sensor/inuse` → `on`

### Cpu sensors

`win2mqtt/{hostname}/cpuprocessortime` (returns string 0-100%)

Example : `win2mqtt/lechuck/cpuprocessortime` →  `50`

## MQTT listeners

The predefined is optional due safety resons

### Monitor

Topic: `win2mqtt/{hostname}/monitor/set`
Value: `0` or `1`

Response:
Published to `win2mqtt/{hostname}/monitor` after setting

### Power state operations

Tries to put system into different power states

**Hibernate**
Tries to put system into a hibernation power mode

Topic: `win2mqtt/{hostname}/hibernate`

**Suspend**
Tries to put system into a suspended power mode

Topic: `win2mqtt/{hostname}/suspend `

**Shutdown**
Shutdown immediately the computer

Topic: `win2mqtt/{hostname}/shutdown`

**Reboot**
Reboot immediatly the computer

Topic: `win2mqtt/{hostname}/reboot`

### Send Message

Shows a popup notification message on the Windows computer.

Topic: `win2mqtt/{hostname}/sendmessage`
Value: the following JSON payload is expected:

```json
{ 
	"Lines": [],
	"Image": "{image}"
}
```
`Lines` is a list of text lines, such as `["Hello", "This is a message"]`
`Image` is an optional path to an image file (must exist)

Examples: 

Topic: `win2mqtt/{hostname}/sendmessage`
Value:
```json
{ 
	"Lines": ["Hello from Home Assistant!"],
	"Image": "{image}"
}
```

If you want to add an image to the message, make sure the image file exists in the Windows filesystem.

Topic: `win2mqtt/{hostname}/sendmessage`
Value:
```json
{ 
	"Lines": ["Home Assistant", "Text 1", "Text 2"],
	"Image": "C:\\Windows\\System32\\ComputerToastIcon.contrast-white.png"
}
```

### Running processes

Topic: `win2mqtt/{hostname}/process/running`
Message: `{appname}`

`appname` is the name of the exe to check

Return

Topic: `win2mqtt/{hostname}/process/running/{appname}`
Value:
* 0 = Not running
* 1 = Found running

Example:

Topic: `win2mqtt/{hostname}/process/running`
Message:
```
pdf24.exe
```

Returns:
Topic: `win2mqtt/{hostname}/process/running/pdf24.exe`
Message:
```
1
```

### Execute

Executes the provided command in the system

Topic: `win2mqtt/{hostname}/exec`

Value: the following payload is expected:

```json
{ 
	"CommandString": "{command}",
	"WindowStyle": "{style}",
	"ExecParameters": "{commandParams}",
	"MonitorId": ""
}
```
* `command` is the command name; eg `"dotnet"`
* `style` can be any one of:

  * 0 = Normal
  * 1 = Hidden
  * 2 = Minimized
  * 3 = Maximized

* `commandParams` is the parameters for the command; eg `"build c:/dev/src/myproject"`
