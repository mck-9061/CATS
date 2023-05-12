# Greetings commander!
# If you're reading this, it means one of two things:
#
# - You're a user who just attempted to run CATS by double clicking main.py. I'd advise you to try reading the
#   provided README file.
#
# - You're a developer who's about to find out just how shit this code is. Good luck attempting to even read this.
#   The code quality can be attributed to the fact that this was intended to be a quick script and ended up not
#   being that, so lots of things are hacky and held together with string and tape.
#   It can also be attributed to the fact that I've only ever used Python for quick scripts so I've never been
#   bothered to find out proper Python programming techniques. It's a scripting language in my mind, not
#   object oriented. Sue me.
#   Maybe I'll refactor and document it at some point. I wouldn't count on it. Do it yourself and open a PR
#   if it bothers you.
#   At least it used to be much worse. Take a look at some of the old commits if you like. We love thousand-line
#   python files that could be much simpler, right?


import os
import time
import pydirectinput
import pyautogui
import random
import threading
import pyperclip
import datetime
import sys
import ctypes

import journalwatcher
from discordhandler import post_to_discord, post_with_fields, update_fields
from screenreader import time_until_jump

import pygetwindow as gw

user32 = ctypes.windll.user32
ctypes.windll.shcore.SetProcessDpiAwareness(2)

# ----Options----
# How many up presses to reach tritium in carrier hold:
global tritium_slot
tritium_slot = 0
# Time to refill trit
hold_time = 10

global route_file
route_file = ""

window_name = "Elite - Dangerous (CLIENT)"

global webhook_url
webhook_url = ""

global journal_directory
journal_directory = ""


# Get the screen resolution
screen_width, screen_height = user32.GetSystemMetrics(0), user32.GetSystemMetrics(1)

# Get the ratio of the screen resolution to 1920x1080
global width_ratio
global height_ratio
width_ratio = screen_width / 1920
height_ratio = screen_height / 1080

print("Screen resolution: " + str(screen_width) + "x" + str(screen_height))



def load_settings():
    global tritium_slot
    global webhook_url
    global journal_directory
    global route_file

    try:
        settingsFile = open("settings.txt", "r")
        a = settingsFile.read().split('\n')

        try:
            for line in a:
                if line.startswith("webhook_url="):
                    print(line)
                    webhook_url = line.split("=")[1]
                if line.startswith("journal_directory="):
                    print(line)
                    journal_directory = line.split("=")[1]
                    latest_journal()

                if line.startswith("tritium_slot="):
                    print(line)
                    tritium_slot = int(line.split("=")[1])
                if line.startswith("route_file="):
                    print(line)
                    route_file = line.split("=")[1]
        except Exception as e:
            print(e)
            print("There seems to be a problem with your settings file. Make sure of the following:\n"
                  "- Your tritium slot is a valid integer. It should be the number of up presses it takes to reach "
                  "tritium in your carrier's cargo hold from the transfer menu.\n"
                  "- The journal directory is a valid directory for your operating system, and contains the Elite"
                  " Dangerous journal files.")


    except:
        settingsFile = open("settings.txt", "w+")
        settingsFile.write("webhook_url=\n"
                           "journal_directory=\n"
                           "tritium_slot=\n")

        print("Settings file created, please set up and run again")


def latest_journal():
    global journal_directory
    dir_name = journal_directory
    # Get list of all files only in the given directory
    list_of_files = filter(lambda x: os.path.isfile(os.path.join(dir_name, x)),
                           os.listdir(dir_name))
    # Sort list of files based on last modification time in ascending order
    list_of_files = sorted(list_of_files,
                           key=lambda x: os.path.getmtime(os.path.join(dir_name, x))
                           )
    list_of_files.reverse()

    journalName = ""
    i = 0
    while not journalName.startswith("Journal"):
        journalName = list_of_files[i]
        i += 1

    return journal_directory + journalName.strip()


def slight_random_time(time):
    return random.random() + time


def follow_button_sequence(sequence_name):
    sequence = open("sequences/" + sequence_name, "r").read().split("\n")

    for line in sequence:
        if line.__contains__(":"):
            pydirectinput.keyDown(line.split(":")[0])
            time.sleep(slight_random_time(int(line.split(":")[1])))
            pydirectinput.keyUp(line.split(":")[0])
        else:
            wait_time = 0.1
            key = line

            if line.__contains__("-"):
                wait_time = int(line.split("-")[1])
                key = line.split("-")[0]

            pydirectinput.press(key)
            time.sleep(slight_random_time(wait_time))


def restock_tritium():
    if not sys.argv[1] == "--manual":
        # Navigate menu
        follow_button_sequence("restock_nav_1.txt")

        for i in range(tritium_slot):
            pydirectinput.press('w')
            time.sleep(slight_random_time(0.1))

        follow_button_sequence("restock_nav_2.txt")

        print("Tritium successfully refuelled.")


def jump_to_system(system_name):
    global width_ratio
    global height_ratio
    if sys.argv[1] == "--manual":
        # Manual jumping
        pyperclip.copy(system_name.lower())
        print(
            "alert:Please plot the jump to %s. It has been copied to your clipboard. Then, leave your keyboard alone while the jump time is read." % system_name)
        while journalwatcher.last_carrier_request() != system_name:
            time.sleep(1)

        timeToJump = time_until_jump(width_ratio, height_ratio)
        print(timeToJump.strip())

        failCount = 0

        while len(timeToJump.split(':')) != 3:
            print("Trying again... (" + str(failCount) + ")")
            timeToJump = time_until_jump(width_ratio, height_ratio)
            print(timeToJump.strip())
            failCount += 1

        time.sleep(6)
        pydirectinput.press('backspace')
        time.sleep(slight_random_time(0.1))
        pydirectinput.press('backspace')

        print("alert:It is now safe to use your keyboard and mouse.")

        return timeToJump.strip()

    follow_button_sequence("jump_nav_1.txt")

    pyautogui.moveTo(921*width_ratio, 115*height_ratio)
    time.sleep(slight_random_time(0.1))
    pyautogui.moveTo(930*width_ratio, 115*height_ratio)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')
    pyperclip.copy(system_name.lower())
    time.sleep(slight_random_time(1.0))
    pydirectinput.keyDown("ctrl")
    time.sleep(slight_random_time(0.1))
    pydirectinput.press("v")
    time.sleep(slight_random_time(0.1))
    pydirectinput.keyUp("ctrl")
    time.sleep(slight_random_time(3.0))
    # pydirectinput.press('down')
    pyautogui.moveTo(930*width_ratio, 150*height_ratio)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')
    time.sleep(slight_random_time(0.1))
    pyautogui.moveTo(1496*width_ratio, 400*height_ratio)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')

    time.sleep(6)

    # Navigate carrier menu
    pydirectinput.press('s')
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')

    if journalwatcher.last_carrier_request() != system_name:
        print(journalwatcher.lastCarrierRequest)
        print(system_name)
        print("Jump appears to have failed.")
        print("Re-attempting...")
        follow_button_sequence("jump_fail.txt")
        return 0

    timeToJump = time_until_jump(width_ratio, height_ratio)
    print(timeToJump.strip())

    failCount = 0

    while len(timeToJump.split(':')) != 3:
        print("Trying again... (" + str(failCount) + ")")
        timeToJump = time_until_jump(width_ratio, height_ratio)
        print(timeToJump.strip())
        failCount += 1

    time.sleep(6)
    pydirectinput.press('backspace')
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('backspace')

    return timeToJump.strip()


global lineNo


def main_loop():
    global lineNo
    global tritium_slot
    global webhook_url
    global journal_directory
    global route_file

    load_settings()

    time.sleep(5)

    latestJournal = latest_journal()

    currentTime = datetime.datetime.now(datetime.timezone.utc)
    arrivalTime = currentTime

    th = threading.Thread(target=process_journal, args=(latestJournal,))
    th.start()

    # win = gw.getWindowsWithTitle(window_name)[0]
    # win.activate()

    lineNo = 0
    saved = False

    if os.path.exists("save.txt"):
        print("Save file found. Setting up...")
        lineNo = int((open("save.txt", "r")).read())
        os.remove("save.txt")

        saved = True

    print("Beginning in 5...")
    time.sleep(5)
    # print("Stocking initial tritium...")
    # restock_tritium()

    routeFile = open(route_file, "r")
    route = routeFile.read()

    finalLine = route.split("\n")[len(route.split("\n")) - 1]
    jumpsLeft = len(route.split("\n")) + 1

    d = 1;
    while finalLine == "" or finalLine == "\n":
        d += 1
        finalLine = route.split("\n")[len(route.split("\n")) - d]

    routeName = "Carrier Updates: Route to " + finalLine

    print("Destination: " + finalLine)

    a1 = route.split("\n")
    a = []

    delta = datetime.timedelta()
    for i in a1:
        if (not i == "") and (not i == "\n"):
            a.append(i)
        else:
            continue
        if a1.index(i) < lineNo: continue
        delta = delta + datetime.timedelta(seconds=1320)
    arrivalTime = arrivalTime + delta

    doneFirst = False
    for i in range(len(a)):
        jumpsLeft -= 1
        if i < lineNo: continue

        line = a[i]

        # win.activate()
        time.sleep(3)

        print("Next stop: " + line)
        print("Beginning navigation.")
        print("Please do not change windows until navigation is complete.")

        print("ETA: " + arrivalTime.strftime("%d %b %Y %H:%M %Z"))

        try:
            timeToJump = jump_to_system(line)
            while timeToJump == 0: timeToJump = jump_to_system(line)
            print("Navigation complete. Jump occurs in " + timeToJump + ". Counting down...")

            hours = int(timeToJump.split(':')[0])
            minutes = int(timeToJump.split(':')[1])
            seconds = int(timeToJump.split(':')[2])

            totalTime = (hours * 3600) + (minutes * 60) + seconds - 12

            if totalTime > 900:
                arrivalTime = arrivalTime + datetime.timedelta(seconds=totalTime - 900)
                print(arrivalTime.strftime("%d %b %Y %H:%M %Z"))

            if doneFirst:
                previous_system = a[i - 1]
                post_with_fields("Carrier Jump", webhook_url,
                                 "Jump to " + previous_system + " successful.\n"
                                                                "The carrier is now jumping to the " + line + " system.\n"
                                                                                                              "Jumps remaining: " + str(
                                     jumpsLeft) +
                                 "\nEstimated time until next jump: " + timeToJump +
                                 "\nEstimated time of route completion: " + arrivalTime.strftime("%d %b %Y %H:%M %Z") +
                                 "\no7", routeName, "Wait...",
                                 "Wait...")
                time.sleep(2)
                update_fields(0, 0)
            else:
                if not saved:
                    post_with_fields("Flight Begun", webhook_url,
                                     "The Flight Computer has begun navigating the Carrier.\n"
                                     "The Carrier's route is as follows:\n" +
                                     route +
                                     "\nEstimated time until first jump: " + timeToJump +
                                     "\nEstimated time of route completion: " + arrivalTime.strftime(
                                         "%d %b %Y %H:%M %Z") +
                                     "\no7", routeName, "Wait...",
                                     "Wait...")
                    time.sleep(2)
                    update_fields(0, 0)
                else:
                    post_with_fields("Flight Resumed", webhook_url,
                                     "The Flight Computer has resumed navigation.\n"
                                     "Estimated time until first jump: " + timeToJump +
                                     "\nEstimated time of route completion: " + arrivalTime.strftime(
                                         "%d %b %Y %H:%M %Z") +
                                     "\no7", routeName, "Wait...",
                                     "Wait..."
                                     )
                    time.sleep(2)
                    update_fields(0, 0)


        except Exception as e:
            print(e)
            print("An error has occurred. Saving progress and aborting...")
            post_to_discord("Critical Error", webhook_url,
                            "An error has occurred with the Flight Computer.\n"
                            "It's possible the game has crashed, or servers were taken down.\n"
                            "Please wait for the carrier to resume navigation.\n"
                            "o7", routeName)
            print("Message sent...")
            saveFile = open("save.txt", "w+")
            saveFile.write(str(lineNo))
            saveFile.close()
            print("Progress saved...")
            return False

        while totalTime > 0:
            print(totalTime)
            time.sleep(1)

            if totalTime == 600:
                update_fields(1, 1)
            elif totalTime == 200:
                update_fields(2, 2)
            elif totalTime == 190:
                update_fields(2, 3)
            elif totalTime == 144:
                update_fields(2, 4)
            elif totalTime == 103:
                update_fields(2, 5)
            elif totalTime == 90:
                update_fields(2, 6)
            elif totalTime == 75:
                update_fields(2, 7)
            elif totalTime == 60:
                update_fields(3, 7)
            elif totalTime == 30:
                update_fields(4, 7)

            totalTime -= 1

        print("Jumping!")

        update_fields(5, 7)

        lineNo += 1

        if not line == finalLine:
            print("Counting down until next jump...")
            totalTime = 362
            while totalTime > 0:
                print(totalTime)

                if totalTime == 340:
                    update_fields(6, 7)
                elif totalTime == 320:
                    update_fields(7, 7)
                elif totalTime == 300:
                    print("Jump complete!")
                    update_fields(8, 7)
                elif totalTime == 151:
                    update_fields(8, 8)
                elif totalTime == 100:
                    update_fields(8, 9)

                elif totalTime == 150:
                    print("Restocking tritium...")
                    # win.activate()
                    time.sleep(2)
                    th = threading.Thread(target=restock_tritium)
                    th.start()

                time.sleep(1)
                totalTime -= 1
            update_fields(9, 9)

        else:
            print("Counting down until jump finishes...")

            update_fields(9, 9)

            totalTime = 60
            while totalTime > 0:
                print(totalTime)
                time.sleep(1)
                totalTime -= 1

        doneFirst = True

    print("Route complete!")
    post_to_discord("Carrier Arrived", webhook_url,
                    "The route is complete, and the carrier has arrived at " + finalLine + ".\n"
                                                                                           "o7", routeName)
    return True


def process_journal(file_name):
    while True:
        c = journalwatcher.process_journal(file_name)
        if not c:
            print("An error has occurred. Saving progress and aborting...")
            post_to_discord("Critical Error", webhook_url,
                            "An error has occurred with the Flight Computer.\n"
                            "It's possible the game has crashed, or servers were taken down.\n"
                            "Please wait for the carrier to resume navigation.\n"
                            "o7", "")
            print("Message sent...")
            saveFile = open("save.txt", "w+")
            saveFile.write(str(lineNo))
            saveFile.close()
            print("Progress saved...")
            raise SystemExit(0)

        time.sleep(1)


if not main_loop():
    print("Aborted.")
raise SystemExit(0)
