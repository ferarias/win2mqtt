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