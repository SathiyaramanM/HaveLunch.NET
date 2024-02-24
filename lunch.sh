#!/bin/zsh

# Function to start postgres container
start_database() {
  docker-compose up -d
}

# Function to stop postgres container
stop_database() {
  docker-compose down
}

# Function to install dotnet-ef
install_dotnet_ef() {
  dotnet tool install --global dotnet-ef --version 8.0.0
}

# Function to run existing migrations using dotnet-ef
run_migrations() {
  dotnet ef database update --project ./HaveLunch/HaveLunch.csproj
}

# Check if a command is provided
if [[ -z $1 ]]; then
  echo "Usage: $0 <command>"
  echo "Available commands: install, start-database, stop-database, migrate"
  echo ""
  echo "For first timers, run commands in this order: install -> start-database -> migrate"
  exit 1
fi

# Store the command provided
command=$1

# Execute different actions based on the command
case $command in
  "start-database")
    start_database
    ;;
  "stop-database")
    stop_database
    ;;
  "install")
    install_dotnet_ef
    ;;
  "migrate")
    run_migrations
    ;;
  *)
    echo "Unknown command: $command"
    echo "Available commands: install, start-database, stop-database, migrate"
    exit 1
    ;;
esac
