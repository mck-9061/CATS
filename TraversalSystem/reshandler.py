import math
import ctypes
from ctypes import wintypes


class Reshandler:
    __slots__ = ["sysNameX", "sysNameUpperY", "sysNameLowerY", "jumpButtonX", "jumpButtonY",
                 "supported_res", "monitor_offset_x", "monitor_offset_y", "monitor_index"]

    def __init__(self, w, h) -> None:
        multiplier = 1
        line = ""
        self.monitor_offset_x = 0
        self.monitor_offset_y = 0
        self.monitor_index = 0

        monitors = self._get_monitors()
        if len(monitors) > 1:
            print(f"Multi-monitor setup detected: {len(monitors)} monitors")
            for i, mon in enumerate(monitors):
                print(f"  Monitor {i}: {mon['width']}x{mon['height']} at offset ({mon['x']}, {mon['y']})" +
                      (" [PRIMARY]" if mon['primary'] else ""))

        with open("res.csv", "r", encoding="utf-8") as file:
            j = 0
            for l in file.readlines():
                j += 1
                if j == 1:
                    continue

                s = l.split(",")
                if int(s[0]) == w and int(s[1]) == h:
                    print("Resolution is officially supported!")
                    line = l
                    break

        if line == "":
            d = math.gcd(w, h)
            rX = w / d
            rY = h / d

            print("Resolution not officially supported. Looking for aspect ratio (%s)..." % (str(rX) + ":" + str(rY)))

            i = 0

            with open("res.csv", "r", encoding="utf-8") as file:
                for l in file.readlines():
                    i += 1
                    if i == 1:
                        continue

                    s = l.split(",")
                    resW = int(s[0])
                    resH = int(s[1])
                    resGCD = math.gcd(resW, resH)

                    print(str(resW) + "x" + str(resH) + " @ " + str(resW / resGCD) + ":" + str(resH / resGCD))

                    if rX == resW / resGCD and rY == resH / resGCD:
                        print("Resolution with same aspect ratio found: %s. This might not work completely." % (
                                    str(resW) + "x" + str(resH)))
                        line = l
                        multiplier = w / resW
                        break

                if line == "":
                    print("Resolution is not supported. Please switch to a supported resolution, or raise an issue on "
                          "GitHub to get yours supported.")
                    self.supported_res = False
                    return

        lineArr = line.split(",")

        self.sysNameX = int(int(lineArr[2]) * multiplier)
        self.sysNameUpperY = int(int(lineArr[3]) * multiplier)
        self.sysNameLowerY = int(int(lineArr[4]) * multiplier)
        self.jumpButtonX = int(int(lineArr[5]) * multiplier)
        self.jumpButtonY = int(int(lineArr[6]) * multiplier)

        self.supported_res = True

    def _get_monitors(self) -> list:
        monitors = []

        def callback(hMonitor, hdcMonitor, lprcMonitor, dwData):
            info = MONITORINFOEX()
            info.cbSize = ctypes.sizeof(MONITORINFOEX)
            if ctypes.windll.user32.GetMonitorInfoW(hMonitor, ctypes.byref(info)):
                monitors.append({
                    'x': info.rcMonitor.left,
                    'y': info.rcMonitor.top,
                    'width': info.rcMonitor.right - info.rcMonitor.left,
                    'height': info.rcMonitor.bottom - info.rcMonitor.top,
                    'primary': info.dwFlags & 1
                })
            return True

        MonitorEnumProc = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.POINTER(ctypes.c_int),
                                              ctypes.POINTER(ctypes.c_int), ctypes.POINTER(wintypes.RECT), ctypes.c_double)
        ctypes.windll.user32.EnumDisplayMonitors(None, None, MonitorEnumProc(callback), 0)

        return monitors

    def set_monitor(self, monitor_index: int) -> bool:
        monitors = self._get_monitors()
        if monitor_index < 0 or monitor_index >= len(monitors):
            print(f"Invalid monitor index {monitor_index}. Available: 0-{len(monitors)-1}")
            return False

        mon = monitors[monitor_index]
        self.monitor_offset_x = mon['x']
        self.monitor_offset_y = mon['y']
        self.monitor_index = monitor_index
        print(f"Set to monitor {monitor_index}: offset ({self.monitor_offset_x}, {self.monitor_offset_y})")
        return True

    def get_absolute_coords(self, x: int, y: int) -> tuple:
        return (x + self.monitor_offset_x, y + self.monitor_offset_y)


class MONITORINFOEX(ctypes.Structure):
    _fields_ = [
        ("cbSize", wintypes.DWORD),
        ("rcMonitor", wintypes.RECT),
        ("rcWork", wintypes.RECT),
        ("dwFlags", wintypes.DWORD),
        ("szDevice", wintypes.WCHAR * 32)
    ]
