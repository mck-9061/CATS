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
  * Optional max efficiency mode
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
* Dock with your carrier.
* Make sure your cursor is over the "Carrier Services" option, and that your internal panel (right) is on the home tab.
* Open the traversal system from the admin interface. Fill in the options:
  * Tritium slot: how many up-presses it takes to get to tritium in the cargo transfer menu. IMPORTANT - take out a unit of tritium first, as this changes where it's positioned.
  * If you're using a squadron carrier, tritium will be taken out of the squadron bank. In this case, the tritium slot should be set to the number of DOWN presses (0 if it's at the top) it takes to get to tritium in the bank commodities menu.
  * Webhook URL: The URL to send Discord updates to.
  * Journal Dir: The directory of your Elite Dangerous journal files (will autofill when settings are saved).
  * Route: Put each system on a new line. Alternatively, import from a plain text list of systems, or a CSV from Spansh's FC plotter.
* Save the settings with the button at the top.
* Select the starting point: If you're already at a system on the route, select it here. Otherwise select "Before first system in route". Save again if you've changed this.
* Click "Run CATS", then tab to the Elite Dangerous window. It should now start to plot jumps.

## Traversal system disclaimer
Use of programs like this is technically against Frontier's TOS. While they haven't yet banned people for automating carrier jumps, the developer does not take any responsibility for any actions that could be taken against your account.<br>
Use of the admin interface without the traversal system carries no risk, as it's simply reading from FDev's API.

<br><br>
o7
