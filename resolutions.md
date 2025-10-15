# Supported resolutions
CATS supports the following resolutions natively:
- 1920x1080
- 1280x720
- 3440x1440
- 5120x1440
- 3620x2036
- 2560x1600

There is also experimental support for any resolution with these aspect ratios, though some things may not work and haven't been tested:
- 16:9
- 16:10
- 43:18
- 32:9
- 905:509

If you have a supported ultrawide monitor and CATS isn't working, try changing your cursor to "OS Cursor" in the ED graphics settings.<br>

If you want to add support for a resolution, you can add an entry to the `res.csv` file in the CATS folder in the CATS installation directory. The columns in this file are as follows:
- w: Resolution width (for 1920x1080, this would be 1920)
- h: Resolution height (for 1920x1080, this would be 1080)
- sysx: x-coordinate of the position to click when selecting the system search box in the galaxy map. You can take a full resolution screenshot and copy it into an image editing software to determine this.
- sysy1: y-coordinate of the position to click when selecting the system search box.
- sysy2: y-coordinate of the position to click when selecting the first system in the list when a system name is entered into the search box. This shares an x-coordinate with sysy1.
- jbx: x-coordinate of the "Select carrier destination" button when a system is selected.
- jby: y-coordinate of the "Select carrier destination" button when a system is selected.

If you add support for a resolution and you've verified it works, pull requests are appreciated. `res.csv` is located in the `TraversalSystem` folder of the repository.
