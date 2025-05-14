# Windows to MQTT

A Windows service that exposes system sensors to MQTT so that they can be consumed from IOT applications such as Home Assistant

## Architecture

The application is a Windows Service that runs in the background, collects system information, and 
publishes it to an MQTT broker. 
It uses the MQTT protocol to communicate with the broker and publish sensor data.

