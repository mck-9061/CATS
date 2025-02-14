import random
import re
from discord_webhook import DiscordWebhook, DiscordEmbed

# Define default carrier stage list and maintenance stage list
CSL = [
    "Waiting...",
    "Jump locked",
    "Lockdown protocol active",
    "Powering FSD",
    "Initiating FSD",
    "Entering hyperspace portal",
    "Traversing hyperspace",
    "Exiting hyperspace portal",
    "FSD cooling down",
    "Jump complete"
]
MSL = [
    "Waiting",
    "Preparing carrier for hyperspace",
    "Services taken down",
    "Landing pads retracting",
    "Bulkheads closing",
    "Airlocks sealing",
    "Task confirmation",
    "Waiting",
    "Restocking Tritium",
    "Done"
]

class DiscordHandler:
    __slots__ = ["lastHook", "lastEmbed", "photo_list"]

    def __init__(self) -> None:
        try:
            with open("photos.txt", "r", encoding="utf-8") as photosFile:
                self.photo_list = photosFile.read().split()
        except Exception as e:
            print("Failed to get image URLs in photos.txt with error: ", e)
            print("Using fallback URL...")
            self.photo_list = ["https://upload.wikimedia.org/wikipedia/en/e/e5/Elite_Dangerous.png"]


    def post_to_discord(self, subject, webhook_url, message, routeName):
        try:
            photo = random.choice(self.photo_list)

            webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)

            embed = DiscordEmbed(title=subject, description=message)
            embed.set_image(url=photo)
            embed.set_author(name=routeName)
            embed.set_footer(text="Carrier Administration and Traversal System")

            webhook.add_embed(embed)

            webhook.execute()
        except Exception as e:
            print("Discord webhook failed with error: ", e)
            print("Double-check that the webhook is set up")


    def post_with_fields(self, subject, webhook_url, message, routeName, carrierStage, maintenanceStage):
        try:
            photo = random.choice(self.photo_list)
        
            webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)
        
            embed = DiscordEmbed(title=subject, description=message)
            embed.set_image(url=photo)
            embed.set_author(name=routeName)
            
            embed.set_footer(text="Carrier Administration and Traversal System")
        
            embed.add_embed_field(name="Jump stage", value=carrierStage)
            embed.add_embed_field(name="Maintenance stage", value=maintenanceStage)
        
            self.lastEmbed = embed
        
            webhook.add_embed(embed)
        
            webhook.execute()
            self.lastHook = webhook
        except Exception as e:
            print("Discord webhook failed with error: ", e)
            print("Double-check that the webhook is set up")


    def update_fields(self, carrierStage: int, maintenanceStage: int):
        try:
            cur_CSL, cur_MSL = [], []  # Define carrier stage list and maintenance stage list for the current field update

            # Add strikethru to every carrier stage before current
            for c_stage_name in CSL[:carrierStage]:
                cur_CSL.append(f"~~{c_stage_name}~~")
            # Bold current stage
            cur_CSL.append(f"**{CSL[carrierStage]}**")
            # Add remaining stages as normal text
            cur_CSL += CSL[carrierStage+1:]
            
            # Add strikethru & "...DONE" signifier to every maintenance stage before current
            for m_stage_name in MSL[:maintenanceStage]:
                cur_MSL.append(f"~~{m_stage_name}...DONE~~")
            # Bold current stage and add ellipsis
            cur_MSL.append(f"**{MSL[maintenanceStage]}...**")
            # Add remaining stages as normal text
            cur_MSL += MSL[maintenanceStage+1:]

            # Once the jump is finished, replace all countdowns with a static text blurb
            if maintenanceStage == 9 and self.lastEmbed.description:
                while re.search(r"<t:\d*:R>", self.lastEmbed.description):
                    self.lastEmbed.description = str(re.sub(r"<t:\d*:R>", "Countdown Expired", self.lastEmbed.description))
        
        
            self.lastHook.remove_embeds()
        
            self.lastEmbed.delete_embed_field(0)
            self.lastEmbed.delete_embed_field(0)
        
            self.lastEmbed.add_embed_field(name="Jump stage", value="\n".join(cur_CSL))
            self.lastEmbed.add_embed_field(name="Maintenance stage", value="\n".join(cur_MSL))
        
            self.lastHook.add_embed(self.lastEmbed)

            self.lastHook.edit()
        except Exception as e:
            print("Discord webhook failed with error: ", e)
            print("Double-check that the webhook is set up")
            # print(f"DEBUG DATA: lastHook: {self.lastHook}, lastEmbed: {self.lastEmbed}")
