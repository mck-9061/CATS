import random
import re
from discord_webhook import DiscordWebhook, DiscordEmbed

class DiscordHandler:
    __slots__ = ["lastHook", "lastSent", "lastEmbed"]

    def __init__(self) -> None:
        pass


    def post_to_discord(self, subject, webhook_url, message, routeName):
        photosFile = open("photos.txt", "r", encoding="utf-8")
        photo = random.choice(photosFile.read().split("\n"))
        photosFile.close()

        webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)

        embed = DiscordEmbed(title=subject, description=message)
        embed.set_image(url=photo)
        embed.set_author(name=routeName)
        embed.set_footer(text="Carrier Administration and Traversal System")

        webhook.add_embed(embed)


    def post_with_fields(self, subject, webhook_url, message, routeName, carrierStage, maintenanceStage):
        try:
            with open("photos.txt", "r", encoding="utf-8") as photosFile:
                photo = random.choice(photosFile.read().split("\n"))
        
            webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)
        
            embed = DiscordEmbed(title=subject, description=message)
            embed.set_image(url=photo)
            embed.set_author(name=routeName)
            
            embed.set_footer(text="Carrier Administration and Traversal System")
        
            embed.add_embed_field(name="Jump stage", value=carrierStage)
            embed.add_embed_field(name="Maintenance stage", value=maintenanceStage)
        
            self.lastEmbed = embed
        
            webhook.add_embed(embed)
        
            self.lastSent = webhook.execute()
            self.lastHook = webhook
        except Exception as e:
            print("Discord webhook failed with error: ", e)
            print("Double-check that the webhook is set up")


    def update_fields(self, carrierStage, maintenanceStage):
        try:
            default_carrier_stage = "Waiting...\nJump locked\nLockdown protocol active\nPowering FSD\nInitiating FSD\nEntering hyperspace portal\nTraversing hyperspace\nExiting hyperspace portal\nFSD cooling down\nJump complete"
        
            default_maintenance_stage = "Waiting\nPreparing carrier for hyperspace\nServices taken down\nLanding pads retracting\nBulkheads closing\nAirlocks sealing\nTask confirmation\nWaiting\nRestocking Tritium\nDone"
        
            c_stage_list = default_carrier_stage.split("\n")
            m_stage_list = default_maintenance_stage.split("\n")
        
            new_carrier_stage = ""
            new_maintenance_stage = ""
        
        
            i = 0
            for stage in c_stage_list:
                if i < carrierStage:
                    new_carrier_stage += "~~" + stage + "~~\n"
                elif i == carrierStage:
                    new_carrier_stage += "**" + stage + "**\n"
                else:
                    new_carrier_stage += stage + "\n"
                i += 1
        
            i = 0
            for stage in m_stage_list:
                if i < maintenanceStage:
                    new_maintenance_stage += "~~" + stage + "...DONE~~\n"
                elif i == maintenanceStage:
                    new_maintenance_stage += "**" + stage + "...**\n"
                    if stage == "Done" and self.lastEmbed.description:
                        # Once the jump is finished, replace all countdowns with a static text blurb
                        while re.match(r"<t:\d*:R>", self.lastEmbed.description):
                            self.lastEmbed.description = str(re.sub(r"<t:\d*:R>", "Countdown Expired", self.lastEmbed.description))
                else:
                    new_maintenance_stage += stage + "\n"
                i += 1
        
        
        
        
            self.lastHook.remove_embeds()
        
            self.lastEmbed.delete_embed_field(0)
            self.lastEmbed.delete_embed_field(0)
        
            self.lastEmbed.add_embed_field(name="Jump stage", value=new_carrier_stage)
            self.lastEmbed.add_embed_field(name="Maintenance stage", value=new_maintenance_stage)
        
            self.lastHook.add_embed(self.lastEmbed)

            self.lastHook.edit()
        except Exception as e:
            print("Discord webhook failed with error: ", e)
            print("Double-check that the webhook is set up")
            print(f"DEBUG DATA: lastHook: {self.lastHook}, lastSent: {self.lastSent}, lastEmbed: {self.lastEmbed}")
