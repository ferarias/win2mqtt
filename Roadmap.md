## Features

Lock computer
Get/Set volume

## Sensors (taken from System Bridge)

### Battery

```json
{
  "is_charging": null,
  "percentage": null,
  "time_remaining": null
}
```

### CPU

```json
{
  "count": 6,
  "frequency": {
    "current": 3696,
    "min": 0,
    "max": 3696
  },
  "load_average": 0.35333333333333333,
  "per_cpu": [
    {
      "id": 0,
      "frequency": {
        "current": 3696,
        "min": 0,
        "max": 3696
      },
      "power": null,
      "times": {
        "user": 1558.03125,
        "system": 2842.765625,
        "idle": 23010.421875,
        "interrupt": 253,
        "dpc": 306.796875
      },
      "times_percent": {
        "user": 4.6,
        "system": 7.7,
        "idle": 86.2,
        "interrupt": 1.5,
        "dpc": 0
      },
      "usage": 25.8,
      "voltage": null
    },
    [...]
  ],
  "power": null,
  "stats": {
    "ctx_switches": 525951057,
    "interrupts": 334033403,
    "soft_interrupts": 0,
    "syscalls": 2631233022
  },
  "temperature": null,
  "times": {
    "user": 13200.15625,
    "system": 16048.734375,
    "idle": 135211.28125,
    "interrupt": 624.734375,
    "dpc": 451.765625
  },
  "times_percent": {
    "user": 9.3,
    "system": 6.4,
    "idle": 84,
    "interrupt": 0.3,
    "dpc": 0
  },
  "usage": 24.9,
  "voltage": null
}
```

### Disks

Probably most of them are not really useful for HA

```json
{
  "devices": [
    {
      "name": "C:\\",
      "partitions": [
        {
          "device": "C:\\",
          "mount_point": "C:\\",
          "filesystem_type": "NTFS",
          "options": "rw,fixed",
          "max_file_size": 255,
          "max_path_length": 260,
          "usage": {
            "total": 429496725504,
            "used": 267132665856,
            "free": 162364059648,
            "percent": 62.2
          }
        }
      ],
      "io_counters": null
    },
    [...]
  ],
  "io_counters": {
    "read_count": 521345,
    "write_count": 826716,
    "read_bytes": 13498627584,
    "write_bytes": 15170793984,
    "read_time": 314,
    "write_time": 386
  }
}
```

### Displays

```json
{
  "0": {
    "id": "0",
    "name": "\\\\.\\DISPLAY1",
    "resolution_horizontal": 1920,
    "resolution_vertical": 1080,
    "x": 1920,
    "y": 0,
    "width": 527,
    "height": 296,
    "is_primary": false,
    "pixel_clock": null,
    "refresh_rate": null
  },
  "1": {
    "id": "1",
    "name": "\\\\.\\DISPLAY4",
    "resolution_horizontal": 1920,
    "resolution_vertical": 1080,
    "x": 0,
    "y": 0,
    "width": 610,
    "height": 360,
    "is_primary": true,
    "pixel_clock": null,
    "refresh_rate": null
  }
}
```

### Media

```json
{
  "album_artist": null,
  "album_title": null,
  "artist": null,
  "duration": null,
  "is_fast_forward_enabled": null,
  "is_next_enabled": null,
  "is_pause_enabled": null,
  "is_play_enabled": null,
  "is_previous_enabled": null,
  "is_rewind_enabled": null,
  "is_stop_enabled": null,
  "playback_rate": null,
  "position": null,
  "repeat": null,
  "shuffle": null,
  "status": null,
  "subtitle": null,
  "thumbnail": null,
  "title": null,
  "track_number": null,
  "type": null,
  "updated_at": 1717266537.598747
}
```

### Memory

```json
{
  "swap": {
    "total": 2147483648,
    "used": 0,
    "free": 2147483648,
    "percent": 0,
    "sin": 0,
    "sout": 0
  },
  "virtual": {
    "total": 34199945216,
    "available": 18470936576,
    "percent": 46,
    "used": 15729008640,
    "free": 18470936576,
    "active": null,
    "inactive": null,
    "buffers": null,
    "cached": null,
    "wired": null,
    "shared": null
  }
}
```


### Networks
```json
{
  "io": {
    "bytes_sent": 225484063,
    "bytes_recv": 948391564,
    "packets_sent": 1141255,
    "packets_recv": 1289812,
    "errin": 0,
    "errout": 0,
    "dropin": 0,
    "dropout": 0
  },
  "networks": [
    {
      "name": "Ethernet",
      "addresses": [
        {
          "address": "FF-FF-FF-FF-FF-FF",
          "family": -1,
          "netmask": null,
          "broadcast": null,
          "ptp": null
        },
        {
          "address": "192.168.1.14",
          "family": 2,
          "netmask": "255.255.255.0",
          "broadcast": null,
          "ptp": null
        },
        {
          "address": "ffff::ffff:ffff:ffff:ffff",
          "family": 23,
          "netmask": null,
          "broadcast": null,
          "ptp": null
        }
      ],
      "stats": {
        "isup": true,
        "duplex": 2,
        "speed": 1000,
        "mtu": 1500,
        "flags": ""
      }
    },
    {
      "name": "Bluetooth Network Connection",
      "addresses": [
        {
          "address": "FF-FF-FF-FF-FF-FF",
          "family": -1,
          "netmask": null,
          "broadcast": null,
          "ptp": null
        },
        {
          "address": "169.254.102.87",
          "family": 2,
          "netmask": "255.255.0.0",
          "broadcast": null,
          "ptp": null
        },
        {
          "address": "ffff::ffff:ffff:ffff:ffff",
          "family": 23,
          "netmask": null,
          "broadcast": null,
          "ptp": null
        }
      ],
      "stats": {
        "isup": false,
        "duplex": 2,
        "speed": 3,
        "mtu": 1500,
        "flags": ""
      }
    },
    [...]
  ]
}
```

### Processes

```json
{
  "0": {
    "id": 92,
    "name": "",
    "cpu_usage": 0,
    "created": 1717239095.5198593,
    "memory_usage": 0.22017831760961845,
    "path": "",
    "status": "stopped",
    "username": null,
    "working_directory": null
  },
  "1": {
    "id": 1856,
    "name": "ACDSeeCommanderUltimate13.exe",
    "cpu_usage": 0.5,
    "created": 1717241173.003722,
    "memory_usage": 0.04376259641783866,
    "path": "C:\\Program Files\\ACD Systems\\ACDSee Ultimate\\13.0\\ACDSeeCommanderUltimate13.exe",
    "status": "running",
    "username": "LECHUCK\\ferar",
    "working_directory": null
  },
  [...]
}
```

### Sensors

```json
{
  "fans": null,
  "temperatures": null,
  "windows_sensors": null
}
```

### System

```json
{
  "boot_time": 1717239095.9575305,
  "camera_usage": [],
  "fqdn": "lechuck",
  "hostname": "lechuck",
  "ip_address_4": "192.168.1.2",
  "ip_address_6": "",
  "mac_address": "ff:ff:ff:ff:ff:ff",
  "pending_reboot": true,
  "platform": "Windows",
  "platform_version": "10.0.22631",
  "uptime": 1039.4375,
  "users": [
    {
      "name": "john",
      "active": true,
      "terminal": null,
      "host": null,
      "started": 1717239102.8711236,
      "pid": null
    }
  ],
  "uuid": "ffffffff-ffff-ffff-ffff-ffffffffffff",
  "version": "4.1.10",
  "version_latest": "4.1.10",
  "version_newer_available": false
}
```