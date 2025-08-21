import json

class JournalWatcher:
    __slots__ = ["firstRun", "lastJournalText", "lastCarrierRequest", "hasJumped", "departureTime", "lastFuel", "lastUsedFileName"]
    
    def __init__(self) -> None:
        self.reset_all()


    def reset_all(self) -> None:
        self.firstRun = True
        self.lastJournalText = ""
        self.lastCarrierRequest = ""
        self.hasJumped = False
        self.departureTime = ""
        self.lastFuel = 1000


    def process_journal(self, file_name) -> bool:
        self.lastUsedFileName = file_name

        # print("Hello from thread!")

        journal = open(file_name, "r", encoding="utf-8")
        journalText = journal.read()
        journal.close()

        if journalText != self.lastJournalText and not self.firstRun:
            newText = journalText.replace(self.lastJournalText, "").strip()

            for line in newText.split("\n"):
                event = json.loads(line)
                # print(event)

                if event['event'] == "CarrierJumpRequest":
                    destination = event['SystemName']

                    if not self.firstRun:
                        self.lastCarrierRequest = destination
                        print("Carrier destination: " + destination)
                        self.departureTime = event['DepartureTime']
                        print("Departure time: " + self.departureTime)

                elif event['event'] == "CarrierStats":
                    fuel = event['FuelLevel']
                    print("Fuel: " + str(fuel)) 

                    if fuel < self.lastFuel and fuel < 100:
                        print("alert:Your Tritium is running low.")

                    self.lastFuel = fuel
                elif event['event'] == "CarrierJump":
                    self.hasJumped = True

            self.lastJournalText = journalText
        self.firstRun = False
        return True


    def last_carrier_request(self) -> str:
        self.process_journal(self.lastUsedFileName)
        return self.lastCarrierRequest


    def reset_jump(self) -> None:
        self.hasJumped = False


    def get_jumped(self) -> bool:
        self.process_journal(self.lastUsedFileName)
        return self.hasJumped
