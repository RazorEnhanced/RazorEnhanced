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
RepairThreshold = 149
for layer in AllLayers:
    item = Player.GetItemOnLayer(layer)
    if item:
        #Misc.SendMessage("Checking: {}".format(item.Name))
        Items.WaitForProps(item, 10000)
        durability = float(Items.GetPropValue(item, "Durability"))
        if durability == 0 or durability < RepairThreshold: 
            Misc.SendMessage(item.Name)