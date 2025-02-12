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
                event = line.split(':')[4].split('"')[1].strip()
                # print(event)

                if event == "CarrierJumpRequest":
                    destination = line.split(':')[6].split('"')[1].strip()

                    if not self.firstRun:
                        self.lastCarrierRequest = destination
                        print("Carrier destination: " + destination)
                        try:
                            self.departureTime = line.split('"')[25].strip()
                        except:
                            self.departureTime = line.split('"')[21].strip()
                        print("Departure time: " + self.departureTime)

                elif event == "CarrierStats":
                    fuel = line.split(':')[10].split(',')[0].strip()
                    print("Fuel: " + fuel)

                    if int(fuel) < self.lastFuel and int(fuel) < 100:
                        print("alert:Your Tritium is running low.")

                    self.lastFuel = int(fuel)
                elif event == "CarrierJump":
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
