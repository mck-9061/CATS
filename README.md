# CATS (Carrier Administration and Traversal System)
CATS is an Elite Dangerous fleet carrier auto-plotter, autojumper, and flight computer.
<br>
This app also allows you to view stats about carriers across multiple accounts.

## Features
### Admin interface
* View name, callsign, credits, fuel, current system and other stats for multiple carriers.
* Manage as many of your own personal carriers as you want.
* View cargo and market data for your carriers.
* Get carrier data from Frontier's API.
### Traversal system
* Automatic jump plotting
* Supports all personal and squadron carriers, including Drake-, Fortune-, Victory-, Nautilus-, and Javelin-class carriers
* Tritium restocking
* Route time estimations
* GUI
* Keeps track of jump time (in case a jump is longer than 15 minutes)
* Discord integration
* Import routes from the Spansh fleet carrier router
* Generate routes between two systems automatically using Spansh's API
* Checks if you have enough Tritium for the route (personal carriers only)

## Limitations
* This only works on Windows and probably won't be ported to anything else.
* It also only works with PC accounts running on Odyssey (The admin interface also works with Live Horizons, but the traversal system is Odyssey-only for now)
* The autopilot has experimental support for displays running at resolutions other than 1920x1080, though most resolutions haven't been tested.
* Elite Dangerous should be running on your primary monitor in fullscreen.
* Officially supported resolutions can be found in the `resolutions.md` file.

## Installation
The release comes bundled with everything you need to use both the admin panel and the traversal system. Just run the installer and you're good to go.

## Traversal system usage
### Refuelling setup
Read this section carefully and follow the instructions, as refuelling needs to have the options set correctly in order to function.

### Important note: 16th October Type-11 Prospector Update 2
#### FDev have indicated that fixes to transferring commodities between ships and carriers will be coming; this is likely to break refuelling. I will be testing and updating this when the update releases.

#### Using a PERSONAL carrier
* Fill the carrier's tritium depot to full (1000 tritium).
* Use a ship with at least 200 cargo capacity.
* Fill your ship's cargo hold with tritium FROM your carrier.
* In the cargo transfer menu on your carrier, there will be 2 entries for tritium. Locate the entry with tritium in the CARRIER, not in the ship.
* If this entry is in the first 8 items, i.e. you can reach it without pressing S:
  * In the CATS options, set the "Refuelling mode" to "Personal (First 8 items)".
  * Count how many times you have to press W to get to that entry from the "Confirm Items Transfer" button.
  * Enter this number in the "Tritium slot" option in CATS.
* If the entry is not in the first 8 items:
    * In the CATS options, set the "Refuelling mode" to "Personal (After 8 items)".
    * Back out of the transfer menu, then go back into it.
    * Press W, then count how many times you have to press S to get to that entry.
    * Enter this number in the "Tritium slot" option in CATS.

#### Using a SQUADRON carrier
* In the CATS options, set the "Refuelling mode" to "Squadron".
* Fill the carrier's tritium depot to full (1000 tritium).
* Use a ship with at least 200 cargo capacity.
* Fill your ship's cargo hold with tritium.
* Go to the squadron bank menu and select the Commodities section.
* Hover over the top commodity in the list.
* Count how many times you have to press S to get to the tritium you want to use (if it's at the top, this would be 0).
* Enter this number in the "Tritium slot" option in CATS.

### Starting the route
* Dock with your carrier.
* Make sure your cursor is over the "Carrier Services" option, and that your internal panel (right) is on the home tab.
* Open the traversal system. Fill in the options:
  * Journal Dir: The directory of your Elite Dangerous journal files.
  * Route: Put each system on a new line. Alternatively, import from a plain text list of systems, or a CSV from Spansh's FC plotter.
* Check the options menu:
  * Automatically plot jumps: If unchecked, you'll instead receive a pop-up telling you to plot a jump when it's time.
  * Determine Tritium requirements when automatically findng route: Requires a carrier to be added to the admin interface.
  * Power saving mode: EXPERIMENTAL. Will close and re-open the game between jumps. Requires the Steam version of Elite Dangerous.
* Select the starting point: If you're already at a system on the route, select it here. Otherwise select "Before first system in route".
* Click "Run CATS", then tab to the Elite Dangerous window. It should now start to plot jumps.

## Traversal system disclaimer
Use of programs like this is technically against Frontier's TOS. While they haven't yet banned people for automating carrier jumps, the developer does not take any responsibility for any actions that could be taken against your account.<br>
Use of the admin interface without the traversal system carries no risk, as it's simply reading from FDev's API.

<br><br>
o7
