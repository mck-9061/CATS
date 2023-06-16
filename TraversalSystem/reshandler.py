import math

global width, height, timeUpperY, timeLowerY, hourX1, hourX2, min1x1, min1x2, min2x1, min2x2, sec1x1, sec1x2, sec2x1, sec2x2, sysNameX, sysNameUpperY, sysNameLowerY, jumpButtonX, jumpButtonY

global multiplier


def setup(w, h):
    global width, height, timeUpperY, timeLowerY, hourX1, hourX2, min1x1, min1x2, min2x1, min2x2, sec1x1, sec1x2, sec2x1, sec2x2, sysNameX, sysNameUpperY, sysNameLowerY, jumpButtonX, jumpButtonY

    global multiplier
    multiplier = 1

    width = w
    height = h

    line = ""

    with open("res.csv", "r", encoding="utf-8") as file:
        for l in file.readlines():
            s = l.split(",")
            if s[0] == w and s[1] == h:
                print("Resolution is officially supported!")
                line = l
                break

        if line == "":
            d = math.gcd(w, h)
            rX = w / d
            rY = h / d

            print("Resolution not officially supported. Looking for aspect ratio (%s)..." % (rX + ":" + rY))

            i = 0
            for l in file.readlines():
                i += 1
                if i == 1:
                    continue

                s = l.split(",")
                resW = s[0]
                resH = s[1]
                resGCD = math.gcd(resW, resH)

                if rX == resW/resGCD and rY == resH/resGCD:
                    print("Resolution with same aspect ratio found: %s. This may not work completely." % (resW+"x"+resH))
                    line = l
                    multiplier = resW / w
                    break

            if line == "":
                print("Resolution is not supported. Please switch to a supported resolution, or join the Discord to "
                      "get yours supported.")
                return 0

    lineArr = line.split(",")

    timeUpperY = int(int(lineArr[2]) * multiplier)
    timeLowerY = int(int(lineArr[3]) * multiplier)
    hourX1 = int(int(lineArr[4]) * multiplier)
    hourX2 = int(int(lineArr[5]) * multiplier)
    min1x1 = int(int(lineArr[6]) * multiplier)
    min1x2 = int(int(lineArr[7]) * multiplier)
    min2x1 = int(int(lineArr[8]) * multiplier)
    min2x2 = int(int(lineArr[9]) * multiplier)
    sec1x1 = int(int(lineArr[10]) * multiplier)
    sec1x2 = int(int(lineArr[11]) * multiplier)
    sec2x1 = int(int(lineArr[12]) * multiplier)
    sec2x2 = int(int(lineArr[13]) * multiplier)
    sysNameX = int(int(lineArr[14]) * multiplier)
    sysNameUpperY = int(int(lineArr[15]) * multiplier)
    sysNameLowerY = int(int(lineArr[16]) * multiplier)
    jumpButtonX = int(int(lineArr[17]) * multiplier)
    jumpButtonY = int(int(lineArr[18]) * multiplier)

    return 1
