
timer = Timer.Create("Test", 10000)
#
while Timer.Check("Test"):
    remain = Timer.Remaining("Test")
    Misc.SendMessage("TimeLeft: {}".format(remain))
    Misc.Pause(500)