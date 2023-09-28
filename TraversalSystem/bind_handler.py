import xml.etree.ElementTree as ET
import os

path = os.environ.get("LOCALAPPDATA") + "\\Frontier Developments\\Elite Dangerous\\Options\\Bindings\\"
neededBinds = {"UI_Up": "w", "UI_Down": "s", "UI_Left": "a", "UI_Right": "d", "UI_Back": "backspace",
               "UI_Select": "space",
               "FocusRightPanel": "4", "CycleNextPanel": "e", "CyclePreviousPanel": "q"}

bindTable = {}


def load_table():
    f = open("bind_table.txt", "r", encoding="utf8")
    for line in f:
        entry = line.split(" ")
        bindTable[entry[0]] = entry[1].replace('\n', '')


def init():
    load_table()
    #print(bindTable)

    # check that custom binds are in use
    bindsUsing = open(path + "StartPreset.4.start", "r", encoding="utf8").readlines()
    if not bindsUsing[0] == "Custom\n" or not bindsUsing[1] == "Custom\n":
        print("Custom binds are not in use, using defaults.")
        return 1

    try:
        f = ET.parse(path + "Custom.4.0.binds")
    except:
        print("Custom binds not found, using defaults.")
        return 1
    root = f.getroot()

    for bind in root:
        if bind.tag in neededBinds:
            goodBind = False
            for device in bind:
                attributes = device.attrib
                if not goodBind and attributes['Device'] == 'Keyboard':
                    b = attributes['Key']
                    if b in bindTable:
                        goodBind = True
                        print("Found a bind for " + bind.tag + ": " + b)
                        neededBinds[bind.tag] = bindTable[b]
                    else:
                        print("Unknown bind for " + bind.tag + ": " + b)
            if not goodBind:
                print("No good bind for " + bind.tag)
                return 0

    neededBinds["Escape"] = "escape"
    return 1


def getKey(bind):
    return neededBinds[bind]
