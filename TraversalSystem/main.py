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
#   At least it used to be much worse. Take a look at the legacy repo and old commits if you like. We love thousand-line
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
import pytz
import psutil

import journalwatcher
from discordhandler import post_to_discord, post_with_fields, update_fields
import reshandler
import bind_handler

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

global power_saving
power_saving = False

# Get the screen resolution
screen_width, screen_height = user32.GetSystemMetrics(0), user32.GetSystemMetrics(1)

print("Screen resolution: " + str(screen_width) + "x" + str(screen_height))

pyautogui.FAILSAFE = False


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
            key = bind_handler.getKey(line.split(":")[0])
            pydirectinput.keyDown(key)
            time.sleep(slight_random_time(int(line.split(":")[1])))
            pydirectinput.keyUp(key)
        else:
            wait_time = 0.1
            key = line

            if line.__contains__("-"):
                wait_time = int(line.split("-")[1])
                key = bind_handler.getKey(line.split("-")[0])
            else:
                key = bind_handler.getKey(line)

            pydirectinput.press(key)
            time.sleep(slight_random_time(wait_time))


def restock_tritium():
    if not sys.argv[1] == "--manual" and not sys.argv[3] == "--nofuel":
        # Navigate menu
        follow_button_sequence("restock_nav_1.txt")

        for i in range(tritium_slot):
            pydirectinput.press(bind_handler.getKey("UI_Up"))
            time.sleep(slight_random_time(0.1))

        follow_button_sequence("restock_nav_2.txt")

        print("Tritium successfully refuelled.")


def jump_to_system(system_name):
    if sys.argv[1] == "--manual":
        # Manual jumping
        pyperclip.copy(system_name.lower())
        print(
            "alert:Please plot the jump to %s. It has been copied to your clipboard." % system_name)
        while journalwatcher.last_carrier_request() != system_name:
            time.sleep(1)

        current_time = datetime.datetime.now(datetime.timezone.utc)
        departure_time_str = journalwatcher.departureTime
        departure_time = datetime.datetime.strptime(departure_time_str, '%Y-%m-%dT%H:%M:%SZ').replace(tzinfo=pytz.UTC)

        print(current_time)
        print(departure_time)

        delta = departure_time - current_time

        return int(delta.total_seconds())

    follow_button_sequence("jump_nav_1.txt")

    pyautogui.moveTo(reshandler.sysNameX, reshandler.sysNameUpperY)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press(bind_handler.getKey("UI_Select"))
    pyperclip.copy(system_name.lower())
    time.sleep(slight_random_time(1.0))
    pydirectinput.keyDown("ctrl")
    time.sleep(slight_random_time(0.1))
    pydirectinput.press("v")
    time.sleep(slight_random_time(0.1))
    pydirectinput.keyUp("ctrl")
    time.sleep(slight_random_time(3.0))
    # pydirectinput.press('down')
    pyautogui.moveTo(reshandler.sysNameX, reshandler.sysNameLowerY)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press(bind_handler.getKey("UI_Select"))
    time.sleep(slight_random_time(0.1))
    pyautogui.moveTo(reshandler.jumpButtonX, reshandler.jumpButtonY)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press(bind_handler.getKey("UI_Select"))

    time.sleep(6)

    if journalwatcher.last_carrier_request() != system_name:
        print(journalwatcher.lastCarrierRequest)
        print(system_name)
        print("Jump appears to have failed.")
        print("Re-attempting...")
        follow_button_sequence("jump_fail.txt")
        return 0

    current_time = datetime.datetime.now(datetime.timezone.utc)
    departure_time_str = journalwatcher.departureTime
    departure_time = datetime.datetime.strptime(departure_time_str, '%Y-%m-%dT%H:%M:%SZ').replace(tzinfo=pytz.UTC)

    print(current_time)
    print(departure_time)

    delta = departure_time - current_time

    # keeping this here, commented, as a tribute to the shit I had to deal with when I had to use OCR
    # if not sys.argv[2] == "--default":
    #     timeToJump = time_until_jump()
    #     print(timeToJump.strip())
    # else:
    #     print("OCR disabled. Assuming usual time.")
    #     timeToJump = "0:15:10"
    #
    # try:
    #     # Check OCR gave a valid time
    #     a = timeToJump.split(':')
    #     testop = int(a[0]) + int(a[1]) + int(a[2])
    # except:
    #     print("OCR failed! Assuming usual time.")
    #     timeToJump = "0:15:00"

    pydirectinput.press(bind_handler.getKey("UI_Back"))
    time.sleep(slight_random_time(0.1))
    pydirectinput.press(bind_handler.getKey("UI_Back"))

    return int(delta.total_seconds())


global lineNo

global game_ready
game_ready = False

global latestJournal
global stopJournalThread
stopJournalThread = False


def open_game():
    global game_ready
    global latestJournal
    global th
    global stopJournalThread
    print("Re-opening game...")

    # Launch
    os.startfile("steam://rungameid/359320")
    time.sleep(60)

    # Wait for the game to load
    j = latest_journal()

    menu = False
    while not menu:
        f = open(j, "r").read()
        if "Fileheader" in f:
            print("Menu loaded")
            menu = True
        else:
            print("Menu not loaded...")
            time.sleep(10)

    # Give it a bit to load properly
    time.sleep(10)

    # Start the game in solo mode
    print("Starting game...")
    pyautogui.moveTo(reshandler.sysNameX, reshandler.sysNameLowerY)
    pyautogui.click()
    follow_button_sequence("start_game.txt")

    # Wait for the Location event
    loaded = False
    while not loaded:
        f = open(j, "r").read()
        if "Location" in f:
            print("Game loaded")
            loaded = True
        else:
            print("Game not loaded...")
            # Just in case it didn't connect yet
            pydirectinput.press(bind_handler.getKey("UI_Select"))
            time.sleep(10)

    print("Switching to new journal...")
    journalwatcher.reset_all()
    latestJournal = latest_journal()

    stopJournalThread = False
    threading.Thread(target=process_journal, args=(latestJournal,)).start()

    game_ready = True


def main_loop():
    global lineNo
    global tritium_slot
    global webhook_url
    global journal_directory
    global route_file
    global power_saving
    global game_ready
    global latestJournal
    global stopJournalThread

    if bind_handler.init() == 0:
        print("Please rebind this to a valid keyboard control.")
        return 0

    load_settings()
    os.environ['DISPLAY'] = ':0'

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

    if sys.argv[3] == "--nofuel":
        print("Tritium refuelling is disabled!")

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

    d = 1
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

        print("ETA: " + arrivalTime.strftime("%d %b %Y %H:%M"))

        try:
            timeToJump = jump_to_system(line)

            while timeToJump == 0:
                timeToJump = jump_to_system(line)

            fTime = str(datetime.timedelta(seconds=timeToJump))

            print("Navigation complete. Jump occurs in " + fTime + ". Counting down...")
            if power_saving:
                print("Power saving mode is active. Closing game...")
                stopJournalThread = True
                follow_button_sequence("close_game.txt")
                # Open the game again when the jump is complete
                threading.Timer(timeToJump, open_game).start()
                print("Game open scheduled")
                # Kill the launcher
                for proc in psutil.process_iter():
                    if proc.name() == "EDLaunch.exe":
                        proc.kill()
                print("Launcher killed")

            journalwatcher.reset_jump()

            totalTime = timeToJump - 6

            if totalTime > 900:
                arrivalTime = arrivalTime + datetime.timedelta(seconds=totalTime - 900)
                print(arrivalTime.strftime("%d %b %Y %H:%M"))

            if doneFirst:
                previous_system = a[i - 1]
                post_with_fields("Carrier Jump", webhook_url,
                                 "Jump to " + previous_system + " successful.\n"
                                                                "The carrier is now jumping to the " + line + " system.\n"
                                                                                                              "Jumps remaining: " + str(
                                     jumpsLeft) +
                                 "\nTime until next jump: " + fTime +
                                 "\nEstimated time of route completion: " + arrivalTime.strftime("%d %b %Y %H:%M") +
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
                                     "\nTime until first jump: " + fTime +
                                     "\nEstimated time of route completion: " + arrivalTime.strftime(
                                         "%d %b %Y %H:%M") +
                                     "\no7", routeName, "Wait...",
                                     "Wait...")
                    time.sleep(2)
                    update_fields(0, 0)
                else:
                    post_with_fields("Flight Resumed", webhook_url,
                                     "The Flight Computer has resumed navigation.\n"
                                     "Time until first jump: " + fTime +
                                     "\nEstimated time of route completion: " + arrivalTime.strftime(
                                         "%d %b %Y %H:%M") +
                                     "\no7", routeName, "Wait...",
                                     "Wait..."
                                     )
                    time.sleep(2)
                    update_fields(0, 0)


        except Exception as e:
            print(type(e))
            print(e.args)
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
            raise

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

                    if not power_saving:
                        print("Pausing execution until jump is confirmed...")
                        c = False
                        while not c:
                            c = journalwatcher.get_jumped()
                            if not c:
                                print("Jump not complete...")
                                time.sleep(10)
                    else:
                        print("Pausing execution until game is open and ready...")
                        while not game_ready:
                            print("Game not ready...")
                            time.sleep(10)
                        totalTime = 152
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
    while not stopJournalThread:
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
    print("Journal thread halted")


if reshandler.setup(screen_width, screen_height) == 0:
    raise SystemExit(0)
else:
    if sys.argv[4] == "--power-saving":
        power_saving = True

    if not main_loop():
        print("Aborted.")
    raise SystemExit(0)
