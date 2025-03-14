# ValveServerPicker

Valve Server Picker is a simple tool that allows you to select a specific server region for playing Valve games. 
It works by blocking certain IP ranges using firewalls that are known to be used by the Valve SDR (Steam Datagram Relay) network in the selected region. 
This allows you to pick the server you want to play on, rather than having to connect to the one closest to you.

<img src="Assets/screenshot.png">

## Features
- In-app server list
- Block IP ranges for specific regions
- Map view of current SDR's
	- Click on a region to modify it
	- Click and drag over multiple SDR's to toggle them at once
- Designed to be easily removable, and does not leave any firewall rules for disabled regions.

## Usage
1. Download the latest release from the [releases page](https://github.com/Whaliin/ValveServerPicker/releases)
2. Run `ValveServerPicker.exe`
3. Check the regions you want to block
4. Done!

## Known bugs:
- Changing the app selector quickly while the map view is open can cause the Pinger objects to be disposed of incorrectly causing high CPU usage for a short moment.
- When dragging the rectangle on the mapview the rectangle does not update properly if you move the cursor upwards or to the left.

## Credits
- [Wikimedia](https://commons.wikimedia.org/w/index.php?title=File:BlankMap-World.svg&oldid=837392775) for the map image
- [cs2-server-picker](https://github.com/FN-FAL113/cs2-server-picker) for the inspiration to work on a version using the Windows Firewall API

## TODO:
- Slim down VS2022 designer generated code
