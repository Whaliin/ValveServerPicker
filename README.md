# ValveServerPicker

Valve Server Picker is a simple tool that allows you to select a specific server region for playing Valve games. 
It works by blocking certain IP ranges using firewalls that are known to be used by the Valve SDR (Steam Datagram Relay) network in the selected region. 
This allows you to pick the server you want to play on, rather than having to connect to the one closest to you.

## Features
- In-app server list
- Block IP ranges for specific regions
- Map view of current SDR's
- Minimalistic design, easy to remove

## Usage
1. Download the latest release from the [releases page](https://github.com/Whaliin/ValveServerPicker/releases)
2. Extract the zip file to a folder of your choice
3. Run `ValveServerPicker.exe`
4. Check the regions you want to block
5. Done!

## Known bugs:
- Changing the app selector quickly while the map view is open can cause the Pinger objects to be disposed of incorrectly causing high CPU usage for a short moment.

## Credits
- [Wikimedia](https://commons.wikimedia.org/w/index.php?title=File:BlankMap-World.svg&oldid=837392775) for the map image
- [cs2-server-picker](https://github.com/FN-FAL113/cs2-server-picker) for the inspiration to work on a version using the Windows Firewall API