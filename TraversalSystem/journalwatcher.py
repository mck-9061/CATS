import time

global lastJournalText
global firstRun
global lastCarrierRequest
global lastUsedFileName
global lastFuel
global hasJumped
global departureTime

firstRun = True
lastJournalText = ""
lastCarrierRequest = ""
hasJumped = False
departureTime = ""

lastFuel = 1000


def reset_all():
    global lastJournalText
    global firstRun
    global lastCarrierRequest
    global lastUsedFileName
    global lastFuel
    global hasJumped
    global departureTime

    firstRun = True
    lastJournalText = ""
    lastCarrierRequest = ""
    hasJumped = False
    departureTime = ""

    lastFuel = 1000


def process_journal(file_name):
    global lastJournalText
    global firstRun
    global lastCarrierRequest
    global lastUsedFileName
    global lastFuel
    global hasJumped
    global departureTime

    lastUsedFileName = file_name

    # print("Hello from thread!")

    journal = open(file_name, "r", encoding="utf-8")
    journalText = journal.read()
    journal.close()

    if journalText != lastJournalText and not firstRun:
        newText = journalText.replace(lastJournalText, "").strip()

        for line in newText.split("\n"):
            event = line.split(':')[4].split('"')[1].strip()
            # print(event)

            if event == "CarrierJumpRequest":
                destination = line.split(':')[6].split('"')[1].strip()

                if not firstRun:
                    lastCarrierRequest = destination
                    print("Carrier destination: " + destination)
                    try:
                        departureTime = line.split('"')[25].strip()
                    except:
                        departureTime = line.split('"')[21].strip()
                    print("Departure time: " + departureTime)

            elif event == "CarrierStats":
                fuel = line.split(':')[10].split(',')[0].strip()
                print("Fuel: " + fuel)

                if int(fuel) < lastFuel and int(fuel) < 100:
                    print("alert:Your Tritium is running low.")

                lastFuel = int(fuel)
            elif event == "CarrierJump":
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
