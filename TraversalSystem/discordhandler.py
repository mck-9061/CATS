import random
from discord_webhook import DiscordWebhook, DiscordEmbed

global lastSent
global lastHook
global lastEmbed


def post_to_discord(subject, webhook_url, message, routeName):
    photosFile = open("photos.txt", "r")
    photo = random.choice(photosFile.read().split("\n"))
    photosFile.close()

    webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)

    embed = DiscordEmbed(title=subject, description=message)
    embed.set_image(url=photo)
    embed.set_author(name=routeName)
    embed.set_footer(text="Carrier Administration and Traversal System")

    webhook.add_embed(embed)


def post_with_fields(subject, webhook_url, message, routeName, carrierStage, maintenanceStage):
    global lastSent
    global lastHook
    global lastEmbed

    photosFile = open("photos.txt", "r")
    photo = random.choice(photosFile.read().split("\n"))
    photosFile.close()

    webhook = DiscordWebhook(url=webhook_url, rate_limit_retry=True)

    embed = DiscordEmbed(title=subject, description=message)
    embed.set_image(url=photo)
    embed.set_author(name=routeName)
    embed.set_footer(text="Carrier Administration and Traversal System")

    embed.add_embed_field(name="Jump stage", value=carrierStage)
    embed.add_embed_field(name="Maintenance stage", value=maintenanceStage)

    lastEmbed = embed

    webhook.add_embed(embed)

    lastSent = webhook.execute()
    lastHook = webhook


def update_fields(carrierStage, maintenanceStage):
    global lastSent
    global lastHook
    global lastEmbed

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
        else:
            new_maintenance_stage += stage + "\n"
        i += 1




    lastHook.remove_embeds()

    lastEmbed.del_embed_field(0)
    lastEmbed.del_embed_field(0)

    lastEmbed.add_embed_field(name="Jump stage", value=new_carrier_stage)
    lastEmbed.add_embed_field(name="Maintenance stage", value=new_maintenance_stage)

    lastHook.add_embed(lastEmbed)

    lastHook.edit(lastSent)
