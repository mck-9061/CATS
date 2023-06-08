import time

global lastJournalText
global firstRun
global lastCarrierRequest
global lastUsedFileName
global lastFuel
global hasJumped

firstRun = True
lastJournalText = ""
lastCarrierRequest = ""
hasJumped = False

lastFuel = 1000


def process_journal(file_name):
    global lastJournalText
    global firstRun
    global lastCarrierRequest
    global lastUsedFileName
    global lastFuel
    global hasJumped

    lastUsedFileName = file_name

    # print("Hello from thread!")

    journal = open(file_name, "r")
    journalText = journal.read()
    journal.close()

    if journalText != lastJournalText and not firstRun:
        newText = journalText.replace(lastJournalText, "").strip()

        for line in newText.split("\n"):
            event = line.split(':')[4].split('"')[1].strip()
            # print(event)

            if event == "Music":
                track = line.split(':')[5].split('"')[1].strip()
                # print("Music track: " + track)

                # if track == "MainMenu":
                # print("Game has crashed!!")
                # return False
            if event == "Shutdown":
                print("Game has crashed!!")
                return False

            elif event == "CarrierJumpRequest":
                destination = line.split(':')[6].split('"')[1].strip()
                print("Carrier destination: " + destination)
                if not firstRun: lastCarrierRequest = destination

            elif event == "CarrierStats":
                fuel = line.split(':')[10].split(',')[0].strip()
                print("Fuel: " + fuel)

                if int(fuel) < lastFuel and int(fuel) < 100:
                    print("alert:Your Tritium is running low.")

                lastFuel = int(fuel)
            elif event == "Location":
                hasJumped = True

        lastJournalText = journalText
    firstRun = False
    return True


def last_carrier_request():
    global lastCarrierRequest
    process_journal(lastUsedFileName)
    return lastCarrierRequest
    
def reset_jump():
    global hasJumped
    hasJumped = False
    
def get_jumped():
    global hasJumped
    process_journal(lastUsedFileName)
    return hasJumped
