import math


class Reshandler:
    __slots__ = ["sysNameX", "sysNameUpperY", "sysNameLowerY", "jumpButtonX", "jumpButtonY", "supported_res"]

    def __init__(self, w, h) -> None:
        multiplier = 1
        line = ""

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
        return
