import time

global lastJournalText
global firstRun
global lastCarrierRequest
global lastUsedFileName

firstRun = True
lastJournalText = ""
lastCarrierRequest = ""

def process_journal(file_name):
    global lastJournalText
    global firstRun
    global lastCarrierRequest
    global lastUsedFileName

    lastUsedFileName = file_name

    #print("Hello from thread!")

    journal = open(file_name, "r")
    journalText = journal.read()
    journal.close()

    if journalText != lastJournalText and not firstRun:
        newText = journalText.replace(lastJournalText, "").strip()

        for line in newText.split("\n"):
            event = line.split(':')[4].split('"')[1].strip()
            #print(event)

            if event == "Music":
                track = line.split(':')[5].split('"')[1].strip()
                #print("Music track: " + track)

                #if track == "MainMenu":
                    #print("Game has crashed!!")
                    #return False
            if event == "Shutdown":
                print("Game has crashed!!")
                return False



            elif event == "CarrierJumpRequest":
                destination = line.split(':')[6].split('"')[1].strip()
                print("Carrier destination: " + destination)
                if not firstRun: lastCarrierRequest = destination


        lastJournalText = journalText
    firstRun = False
    return True

def last_carrier_request():
    global lastCarrierRequest
    process_journal(lastUsedFileName)
    return lastCarrierRequest

