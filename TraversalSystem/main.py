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

print("Autopilot Script Online")

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
import tzlocal
import psutil

from journalwatcher import JournalWatcher
from discordhandler import DiscordHandler
from reshandler import Reshandler

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

global webhook_url
webhook_url = ""

global journal_directory
journal_directory = ""

global auto_plot_jumps
auto_plot_jumps = True

global disable_refuel
disable_refuel = False

global power_saving
power_saving = False

global efficient_refueling
efficient_refueling = False

global squadron_carrier
squadron_carrier = False

# Get the screen resolution
screen_width, screen_height = user32.GetSystemMetrics(0), user32.GetSystemMetrics(1)

print("Screen resolution: " + str(screen_width) + "x" + str(screen_height))

pyautogui.FAILSAFE = False

res_handler = Reshandler(screen_width, screen_height)
journal_watcher = JournalWatcher()
discord_messenger = DiscordHandler()

def load_settings():
    global tritium_slot
    global webhook_url
    global journal_directory
    global route_file
    global auto_plot_jumps
    global disable_refuel
    global power_saving
    global efficient_refueling
    global squadron_carrier
    # AFAIK, it is impossible to launch the script without either settings file existing

    # Get settings from settings.txt
    with open("settings.txt", "r", encoding="utf-8") as settingsFile:
        settings_lines = settingsFile.read().split('\n')

    # Attempt to read settings from settings.txt
    try:
        for line in settings_lines:
            if line.startswith("webhook_url="):
                print(line)
                webhook_url = line.split("=")[1]

            if line.startswith("journal_directory="):
                print(line)
                journal_directory = os.path.expanduser(line.split("=")[1])
                latest_journal()  # Check if the journal directory is valid

            if line.startswith("tritium_slot="):
                print(line)
                tritium_slot = int(line.split("=")[1])

            if line.startswith("route_file="):
                print(line)
                route_file = line.split("=")[1]
    except Exception as e:
        print(
            "There seems to be a problem with your settings.txt file. Make sure of the following:\n"
            "- Your tritium slot is a valid integer. It should be the number of up presses it takes to reach tritium in your carrier's cargo hold from the transfer menu.\n"
            "- The journal directory is a valid directory for your operating system, and contains the Elite Dangerous journal files."
        )
        print(e)
        os._exit(1)
    
    # Get settings from settings.ini
    with open("../settings/settings.ini", "r", encoding="utf-8") as configFile:
        config_lines = configFile.read().split('\n')

    # Attempt to read settings from settings.ini
    try:
        for line in config_lines:
            if line.startswith("auto-plot-jumps="):
                print(line)
                auto_plot_jumps = line.split("=")[1].strip().lower() == "true"

            if line.startswith("disable-refuel="):
                print(line)
                disable_refuel = line.split("=")[1].strip().lower() == "true"

            if line.startswith("power-saving="):
                print(line)
                power_saving = line.split("=")[1].strip().lower() == "true"
            
            if line.startswith("efficient-refueling"):
                print(line)
                efficient_refueling = line.split("=")[1].strip().lower() == "true"

            if line.startswith("squadron-carrier"):
                print(line)
                squadron_carrier = line.split("=")[1].strip().lower() == "true"
    except Exception as e:
        print("There seems to be a problem with your settings.ini file. Please confirm that your options are properly selected.")
        print(e)
        os._exit(1)


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
    sequence = open("sequences/" + sequence_name, "r", encoding="utf-8").read().split("\n")

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
    global squadron_carrier

    if not auto_plot_jumps or disable_refuel:
        # If manual jumping is enabled or automatic refuel is disabled, skip function execution
        return
    
    restock_order = ["open_cargo_transfer", "restock_cargo", "restock_fc"]  # default step order for restocking trit

    if efficient_refueling:
        # If efficient refueling is enabled, restock the fleet carrier first, then add more trit to the CMDR's cargo hold
        restock_order.insert(0, restock_order.pop())

    for step in restock_order:
        if squadron_carrier and os.path.isfile(f"sequences/squadron/{step}.txt"):
            follow_button_sequence(f"squadron/{step}.txt")  # follow the button sequence for each step
        else:
            follow_button_sequence(f"{step}.txt")

        if step == "open_cargo_transfer":
            # at the end of the cargo transfer step, we need to select the correct trit slot based on user input
            for _ in range(tritium_slot):
                if squadron_carrier:
                    pydirectinput.press('s')
                else:
                    pydirectinput.press('w')
                time.sleep(slight_random_time(0.1))

    print("Refuel process completed.")


def jump_to_system(system_name):
    global squadron_carrier

    if not auto_plot_jumps:
        # Manual jumping
        pyperclip.copy(system_name.lower())
        print(
            "alert:Please plot the jump to %s. It has been copied to your clipboard." % system_name)
        while journal_watcher.last_carrier_request() != system_name:
            time.sleep(1)

        current_time = datetime.datetime.now(datetime.timezone.utc)
        departure_time_str = journal_watcher.departureTime
        departure_time = datetime.datetime.strptime(departure_time_str, '%Y-%m-%dT%H:%M:%SZ').replace(tzinfo=pytz.UTC)

        print(current_time.strftime("%m-%d-%Y %H:%M:%S (UTC%z)"))
        print(departure_time.strftime("%m-%d-%Y %H:%M:%S (UTC%z)"))

        delta = departure_time - current_time

        return int(delta.total_seconds()), departure_time

    if squadron_carrier:
        follow_button_sequence("squadron/jump_nav_1.txt")
    else:
        follow_button_sequence("jump_nav_1.txt")

    pyautogui.moveTo(res_handler.sysNameX, res_handler.sysNameUpperY)
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
    pyautogui.moveTo(res_handler.sysNameX, res_handler.sysNameLowerY)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')
    time.sleep(slight_random_time(0.1))
    pyautogui.moveTo(res_handler.jumpButtonX, res_handler.jumpButtonY)
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('space')

    time.sleep(6)

    if journal_watcher.last_carrier_request() != system_name:
        print(journal_watcher.lastCarrierRequest)
        print(system_name)
        print("Jump appears to have failed.")
        print("Re-attempting...")
        follow_button_sequence("jump_fail.txt")
        return 0, 0

    current_time = datetime.datetime.now(datetime.timezone.utc)
    departure_time_str = journal_watcher.departureTime
    departure_time = datetime.datetime.strptime(departure_time_str, '%Y-%m-%dT%H:%M:%SZ').replace(tzinfo=pytz.UTC)

    print(current_time.strftime("%m-%d-%Y %H:%M:%S (UTC%z)"))
    print(departure_time.strftime("%m-%d-%Y %H:%M:%S (UTC%z)"))

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

    pydirectinput.press('backspace')
    time.sleep(slight_random_time(0.1))
    pydirectinput.press('backspace')

    return int(delta.total_seconds()), departure_time


global lineNo

global game_ready
game_ready = False

global latestJournal
global stopJournalThread
stopJournalThread = False

def open_game():
    global game_ready
    global latestJournal
    global stopJournalThread
    print("Re-opening game...")

    # Launch
    os.startfile("steam://rungameid/359320")
    time.sleep(60)

    # Wait for the game to load
    j = latest_journal()

    menu = False
    while not menu:
        f = open(j, "r", encoding="utf-8").read()
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
    pyautogui.moveTo(res_handler.sysNameX, res_handler.sysNameLowerY)
    pyautogui.click()
    follow_button_sequence("start_game.txt")

    # Wait for the Location event
    loaded = False
    while not loaded:
        f = open(j, "r", encoding="utf-8").read()
        if "Location" in f:
            print("Game loaded")
            loaded = True
        else:
            print("Game not loaded...")
            # Just in case it didn't connect yet
            pydirectinput.press("space")
            time.sleep(10)


    print("Switching to new journal...")
    journal_watcher.reset_all()
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

    load_settings()

    time.sleep(5)

    latestJournal = latest_journal()

    th = threading.Thread(target=process_journal, args=(latestJournal,))
    th.start()

    lineNo = 0
    saved = False

    if disable_refuel:
        print("Tritium refuelling is disabled!")

    if os.path.exists("save.txt"):
        print("Save file found. Setting up...")
        lineNo = int((open("save.txt", "r", encoding="utf-8")).read())
        os.remove("save.txt")

        saved = True

    for countdown in range(5, 0, -1):
        print(f"Beginning in {countdown}...")
        time.sleep(1)

    # print("Stocking initial tritium...")
    # restock_tritium()

    with open(route_file, "r", encoding="utf-8") as routeFile:
        route = routeFile.read()

    # Split the route into a list of systems, stripping whitespace (incl newlines). Empty indexes are discarded
    route_list = route.strip().split("\n")
    if route_list == [""]:
        print("Route file is empty. Exiting...")
        os._exit(1)

    jumpsLeft = len(route_list) + 1
    finalLine = route_list[-1]  # last index in the route list

    routeName = "Carrier Updates: Route to " + finalLine

    print("Destination: " + finalLine)

    a1 = route_list
    a = []

    delta = datetime.timedelta()
    currentTime = datetime.datetime.fromtimestamp(time.mktime(time.localtime()), tzlocal.get_localzone())
    for i in a1:
        if (not i == "") and (not i == "\n"):
            a.append(i)
        else:
            continue
        if a1.index(i) < lineNo: continue
        delta = delta + datetime.timedelta(seconds=1320)
    arrivalTime = currentTime + delta
    arrivalTime_discord = f"<t:{arrivalTime.timestamp():.0f}:f> (<t:{arrivalTime.timestamp():.0f}:R>)"

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

        print("ETA: " + arrivalTime.strftime("%A, %I:%M%p (UTC%z)"))

        try:
            timeToJump, departing_time = jump_to_system(line)

            while timeToJump == 0 or departing_time == 0:
                timeToJump, departing_time = jump_to_system(line)

            fTime = str(datetime.timedelta(seconds=timeToJump))
            # https://hammertime.cyou/
            # Time of departure (in seconds since the Epoch) with this format will result in a dynamic countdown in Discord
            # Countdown will look like "in {time} minutes" or "{time} minutes ago". See the link for more details
            fTime_discord = f"<t:{departing_time.timestamp():.0f}:R>"

            print("Navigation complete. Jump occurs in " + fTime + ". Counting down...")
            if power_saving:
                print("Power saving mode is active. Closing game...")
                stopJournalThread = True
                follow_button_sequence("close_game.txt")
                # Open the game again when the jump is complete
                threading.Timer(timeToJump, open_game).start()
                game_ready = False
                print("Game open scheduled")
                # Kill the launcher
                for proc in psutil.process_iter():
                    if proc.name() == "EDLaunch.exe":
                        proc.kill()
                print("Launcher killed")

            journal_watcher.reset_jump()

            totalTime = timeToJump - 6

            if totalTime > 900:
                arrivalTime = arrivalTime + datetime.timedelta(seconds=totalTime - 900)
                print(arrivalTime.strftime("%A, %I:%M%p (UTC%z)"))
                # Arrival time (in seconds since the Epoch) with this format will result in a dynamic timestamp + countdown in Discord
                # Result will look like "{month} {day}, {year} {hour}:{minute} {AM/PM} (in {time} minutes)"
                arrivalTime_discord = f"<t:{arrivalTime.timestamp():.0f}:f> (<t:{arrivalTime.timestamp():.0f}:R>)"

            if doneFirst:
                previous_system = a[i - 1]
                discord_messenger.post_with_fields(
                    "Carrier Jump",
                    webhook_url,
                    routeName,
                    f"Jump to {previous_system} successful.",
                    f"The carrier is now jumping to the {line} system.",
                    f"Jumps remaining: {jumpsLeft}",
                    f"Next jump: {fTime_discord}",
                    f"Estimated time of route completion: {arrivalTime_discord}",
                    "o7"
                )
                time.sleep(2)
                discord_messenger.update_fields(0, 0)
            else:
                if not saved:
                    discord_messenger.post_with_fields(
                        "Flight Begun",
                        webhook_url,
                        routeName,
                        "The Flight Computer has begun navigating the Carrier.",
                        "The Carrier's route is as follows:",
                        "\n".join(route_list),
                        f"First jump: {fTime_discord}",
                        f"Estimated time of route completion: {arrivalTime_discord}",
                        "o7"
                    )
                    time.sleep(2)
                    discord_messenger.update_fields(0, 0)
                else:
                    discord_messenger.post_with_fields(
                        "Flight Resumed",
                        webhook_url,
                        routeName,
                        "The Flight Computer has resumed navigation.",
                        f"First jump: {fTime_discord}",
                        f"Estimated time of route completion: {arrivalTime_discord}",
                        "o7"
                    )
                    time.sleep(2)
                    discord_messenger.update_fields(0, 0)


        except Exception as e:
            print(e)
            print("An error has occurred. Saving progress and aborting...")
            discord_messenger.post_to_discord(
                "Critical Error",
                webhook_url,
                "Carrier Updates: Autopilot Error",
                "An error has occurred with the Flight Computer.",
                "It's possible the game has crashed, or servers were taken down.",
                "Please wait for the carrier to resume navigation.",
                "o7"
            )
            print("Message sent...")
            saveFile = open("save.txt", "w+", encoding="utf-8")
            saveFile.write(str(lineNo))
            saveFile.close()
            print("Progress saved...")
            return False

        while totalTime > 0:
            print(totalTime)
            time.sleep(1)

            match totalTime:
                case 600:
                    discord_messenger.update_fields(1, 1)
                case 200:
                    discord_messenger.update_fields(2, 2)
                case 190:
                    discord_messenger.update_fields(2, 3)
                case 144:
                    discord_messenger.update_fields(2, 4)
                case 103:
                    discord_messenger.update_fields(2, 5)
                case 90:
                    discord_messenger.update_fields(2, 6)
                case 75:
                    discord_messenger.update_fields(2, 7)
                case 60:
                    discord_messenger.update_fields(3, 7)
                case 30:
                    discord_messenger.update_fields(4, 7)

            totalTime -= 1

        print("Jumping!")

        discord_messenger.update_fields(5, 7)

        lineNo += 1

        if line == finalLine and power_saving:
            # When making the final jump on power saving mode, avoid reopening the game to do post-jump tasks
            print("Counting down until jump finishes...")

            totalTime = 60
            while totalTime > 0:
                print(totalTime)
                time.sleep(1)
                totalTime -= 1

            discord_messenger.update_fields(9, 9)
        else:
            print("Counting down until next jump...")
            totalTime = 362
            while totalTime > 0:
                print(totalTime)

                match totalTime:
                    case 340:
                        discord_messenger.update_fields(6, 7)
                    case 320:
                        discord_messenger.update_fields(7, 7)
                    case 300:
                        if not power_saving:
                            print("Pausing execution until jump is confirmed...")
                            c = False
                            while not c:
                                c = journal_watcher.get_jumped()
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
                        discord_messenger.update_fields(8, 7)
                    case 151:
                        discord_messenger.update_fields(8, 8)
                    case 100:
                        discord_messenger.update_fields(8, 9)
                    case 150:
                        print("Restocking tritium...")
                        time.sleep(2)
                        th = threading.Thread(target=restock_tritium)
                        th.start()

                time.sleep(1)
                totalTime -= 1
            discord_messenger.update_fields(9, 9)

        doneFirst = True

    print("Route complete!")
    discord_messenger.post_to_discord(
        "Carrier Arrived",
        webhook_url,
        routeName,
        f"The route is complete, and the carrier has arrived at {finalLine}.",
        "o7"
    )
    return True


def process_journal(file_name):
    while not stopJournalThread:
        c = journal_watcher.process_journal(file_name)
        if not c:
            print("An error has occurred. Saving progress and aborting...")
            discord_messenger.post_to_discord(
                "Critical Error",
                webhook_url,
                "Carrier Updates: Journal Error",
                "An error has occurred with the Flight Computer.",
                "It's possible the game has crashed, or servers were taken down.",
                "Please wait for the carrier to resume navigation.",
                "o7"
            )
            print("Message sent...")
            saveFile = open("save.txt", "w+", encoding="utf-8")
            saveFile.write(str(lineNo))
            saveFile.close()
            print("Progress saved...")
            os._exit(2)

        time.sleep(1)
    print("Journal thread halted")


if not res_handler.supported_res:
    print("Resolution not supported, exiting...")
    os._exit(1)
else:
    if not main_loop():
        print("Aborted.")
        os._exit(2)
    # Use os._exit because Pyinstaller doesn't like sys.exit for some reason
    # Exit code docs: 0 = success, 1 = small error (unsupported res, no route), 2 = critical error (journal error, main loop crash)
    os._exit(0)
