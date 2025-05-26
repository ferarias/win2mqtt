#!/bin/bash

SERVICE_NAME=win2mqtt
INSTALL_DIR=/opt/$SERVICE_NAME
SERVICE_FILE=$SERVICE_NAME.service
ENV_FILE=/etc/$SERVICE_NAME/env
CONFIG_DIR=/etc/$SERVICE_NAME
CONFIG_FILE=$CONFIG_DIR/win2mqtt.appsettings.json

set -e

echo "Installing $SERVICE_NAME..."

# Prompt user for MQTT configuration
read -p "MQTT server [localhost]: " MQTT_SERVER
MQTT_SERVER=${MQTT_SERVER:-localhost}

read -p "MQTT port [1883]: " MQTT_PORT
MQTT_PORT=${MQTT_PORT:-1883}

read -p "MQTT username [empty]: " MQTT_USER
MQTT_USER=${MQTT_USER}

read -p "MQTT password [empty]: " MQTT_PASS
MQTT_PASS=${MQTT_PASS}

DEFAULT_IDENTIFIER=$(hostname)
read -p "Device Identifier [$DEFAULT_IDENTIFIER]: " DEVICE_ID
DEVICE_ID=${DEVICE_ID:-$DEFAULT_IDENTIFIER}

# Create target install dir
sudo mkdir -p $INSTALL_DIR
sudo cp -r ./* $INSTALL_DIR

# Create systemd service
sudo cp $SERVICE_FILE /etc/systemd/system/$SERVICE_FILE

# Create config dir
sudo mkdir -p $CONFIG_DIR

# Create win2mqtt.appsettings.json
echo "Generating config at $CONFIG_FILE..."
sudo tee $CONFIG_FILE > /dev/null <<EOF
{
  "Win2MQTT": {
    "Broker": {
      "Server": "$MQTT_SERVER",
      "Port": $MQTT_PORT,
      "Username": "$MQTT_USER",
      "Password": "$MQTT_PASS"
    },
    "DeviceIdentifier": "$DEVICE_ID"
  }
}
EOF

# Create env file if missing
if [ ! -f $ENV_FILE ]; then
    echo "ASPNETCORE_ENVIRONMENT=Production" | sudo tee $ENV_FILE > /dev/null
fi

# Create user if not exists
if ! id $SERVICE_NAME &>/dev/null; then
    sudo useradd --system --no-create-home --shell /usr/sbin/nologin $SERVICE_NAME
fi

# Set permissions
sudo chown -R $SERVICE_NAME:$SERVICE_NAME $INSTALL_DIR
sudo chown -R $SERVICE_NAME:$SERVICE_NAME $CONFIG_DIR

# Reload and start service
sudo systemctl daemon-reexec
sudo systemctl daemon-reload
sudo systemctl enable $SERVICE_NAME
sudo systemctl restart $SERVICE_NAME

echo "Service '$SERVICE_NAME' installed and started."
echo "Logs: journalctl -u $SERVICE_NAME -f"
