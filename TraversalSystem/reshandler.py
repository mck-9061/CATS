import math

global width, height, sysNameX, sysNameUpperY, sysNameLowerY, jumpButtonX, jumpButtonY

global multiplier


def setup(w, h):
    global width, height, sysNameX, sysNameUpperY, sysNameLowerY, jumpButtonX, jumpButtonY

    global multiplier
    multiplier = 1

    width = w
    height = h

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

                print(str(resW) + "x" + str(resH) + " @ " + str(resW/resGCD) + ":" + str(resH/resGCD))

                if rX == resW/resGCD and rY == resH/resGCD:
                    print("Resolution with same aspect ratio found: %s. This might not work completely." % (str(resW)+"x"+str(resH)))
                    line = l
                    multiplier = w / resW
                    break

            if line == "":
                print("Resolution is not supported. Please switch to a supported resolution, or join the Discord to "
                      "get yours supported.")
                return 0

    lineArr = line.split(",")

    sysNameX = int(int(lineArr[2]) * multiplier)
    sysNameUpperY = int(int(lineArr[3]) * multiplier)
    sysNameLowerY = int(int(lineArr[4]) * multiplier)
    jumpButtonX = int(int(lineArr[5]) * multiplier)
    jumpButtonY = int(int(lineArr[6]) * multiplier)

    return 1
