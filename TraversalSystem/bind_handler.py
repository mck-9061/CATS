import xml.etree.ElementTree as ET
import os

path = os.environ.get("LOCALAPPDATA") + "\\Frontier Developments\\Elite Dangerous\\Options\\Bindings\\Custom.4.0.binds"
neededBinds = {"UI_Up": "", "UI_Down": "", "UI_Left": "", "UI_Right": "", "UI_Back": "", "UI_Select": ""}

def init():
    f = ET.parse(path)
    root = f.getroot()

    for bind in root:
        if bind.tag in neededBinds:
            goodBind = False
            for device in bind:
                attributes = device.attrib
                if not goodBind and attributes['Device'] == 'Keyboard':
                    goodBind = True
                    print("Found a good bind for " + bind.tag + ": " + attributes['Key'])
                    neededBinds[bind.tag] = attributes['Key']
            if not goodBind:
                print("No good bind for " + bind.tag)

init()
print(neededBinds)