String MainBaseHydrogenTankName = "Hydrogen Tank Main Base";
String hydrogenLCDName = "LCDHydrogen";

public Program()
{
   Runtime.UpdateFrequency = UpdateFrequency.Update10;
}

public void Main()
{    
     SetupHydrogenPanel();
     launchSignal();
}

public void SetupHydrogenPanel() {
        var lcd = GridTerminalSystem.GetBlockWithName(hydrogenLCDName) as IMyTextPanel;
        var hydrogenTankBase = GridTerminalSystem.GetBlockWithName(MainBaseHydrogenTankName) as IMyGasTank;

       var tankCapacity = hydrogenTankBase.Capacity;
       var fillLevel = hydrogenTankBase.FilledRatio * tankCapacity; 

       var filledText = "\nHydrogen Level\n" + fillLevel + " / " + tankCapacity;
      Write(lcd, filledText);
}

public void launchSignal() {
     var interiorLightSignal = GridTerminalSystem.GetBlockWithName("Interior Light Launch Signal") as IMyLightingBlock;
     var launchPiston = GridTerminalSystem.GetBlockWithName("Piston Launch Door") as IMyPistonBase;
     var launchDoorStatus = launchPiston.Status;
     var lcd = GridTerminalSystem.GetBlockWithName("Wide LCD Panel") as IMyTextPanel;

     var greenColor = new Color(0, 255, 0);
     var redColor = new Color(255,0,0);

     if(launchDoorStatus == PistonStatus.Retracted) {
         Write(lcd,"Open");
         interiorLightSignal.Color = greenColor;
     }
     else {
        Write(lcd, "Closed");
        interiorLightSignal.Color = redColor;
     }
}

public void Write(IMyTextPanel lcd, String text) {
    lcd.WritePublicText(text);
}