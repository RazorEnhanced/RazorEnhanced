import re
AllLayers = { "FirstValid",
              "RightHand",
              "LeftHand",
              "Shoes",
              "Pants",
              "Shirt",
              "Head",
              "Gloves",
              "Ring",
              "Talisman",
              "Neck",
              "Waist",
              "InnerTorso",
              "Bracelet",
              "FacialHair",
              "MiddleTorso",
              "Earrings",
              "Arms",
              "Cloak",
              "OuterTorso",
              "OuterLegs",
              "InnerLegs",
              }
#              
RepairThreshold = .5
RepairNeeded = 0
for layer in AllLayers:
    item = Player.GetItemOnLayer(layer)
    if item:
        #Misc.SendMessage("Checking: {}".format(item.Name))
        Items.WaitForProps(item, 10000)
        durability = Items.GetPropValue(item, "Durability").ToString()
        props = Items.GetPropStringList(item)
        for i in props:
            result = re.search('([a-z A-Z]+)(.*)$', i)
            propName = result.group(1)
            if propName.strip() == 'durability':
                result = re.search('([a-z A-Z]+) +([0-9]+) / ([0-9]+)$', i)
                curDur = int(result.group(2))
                maxDur = int(result.group(3))
                if curDur < maxDur*RepairThreshold:
                    Misc.SendMessage("Need To REPAIR {} dur: {} / {}".format(item.Name, curDur, maxDur))
                    RepairNeeded += 1
if RepairNeeded == 0:
    Misc.SendMessage("No repairs needed")
