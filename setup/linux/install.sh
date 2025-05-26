#!/bin/bash

SERVICE_NAME=win2mqtt
INSTALL_DIR=/opt/$SERVICE_NAME
SERVICE_FILE=$SERVICE_NAME.service
ENV_FILE=/etc/$SERVICE_NAME/env
CONFIG_DIR=/etc/$SERVICE_NAME

set -e

echo "Installing $SERVICE_NAME..."

# Create target install dir
sudo mkdir -p $INSTALL_DIR
sudo cp -r ./* $INSTALL_DIR

# Create systemd service
sudo cp $SERVICE_FILE /etc/systemd/system/$SERVICE_FILE

# Create config dir
sudo mkdir -p $CONFIG_DIR

# Copy default override config if provided
if [ -f win2mqtt.appsettings.json ]; then
    sudo cp win2mqtt.appsettings.json $CONFIG_DIR/
fi

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
