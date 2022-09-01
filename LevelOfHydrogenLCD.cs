String MainBaseHydrogenTankName = "Hydrogen Tank Main Base";
String hydrogenLCDName = "LCDHydrogen";
const int ALERT_WAIT_TIME = 30;
const float HINGE_OPEN_ANGLE = 1.570796F;
int soundCount;

public Program()
{
   Runtime.UpdateFrequency = UpdateFrequency.Update10;
    if(Storage.Length > 0) {
        soundCount = int.Parse(Storage); 
    }
     else {
    soundCount = ALERT_WAIT_TIME;
    } 
}

public void Main()
{    
     SetupHydrogenPanel();
     launchSignal();
     LightSensor();
}

public void SetupHydrogenPanel() {
        var lcd = GridTerminalSystem.GetBlockWithName(hydrogenLCDName) as IMyTextPanel;
        var hydrogenTankBase = GridTerminalSystem.GetBlockWithName(MainBaseHydrogenTankName) as IMyGasTank;

       var tankCapacity = hydrogenTankBase.Capacity;
       var fillLevel = Math.Round(hydrogenTankBase.FilledRatio * tankCapacity); 

       var filledText = "\nHydrogen Level\n" + fillLevel + " / " + tankCapacity;
      Write(lcd, filledText);
}

public void launchSignal() {
     var interiorLightSignal = GridTerminalSystem.GetBlockWithName("Interior Light Launch Signal") as IMyLightingBlock;

     var lcd = GridTerminalSystem.GetBlockWithName("Wide LCD Panel") as IMyTextPanel;

     var greenColor = new Color(0, 255, 0);
     var redColor = new Color(255,0,0);
     
     var hingeBlock = GridTerminalSystem.GetBlockWithName("Hinge Underground Door") as IMyMotorStator;
     var hingeAngle = hingeBlock.Angle;

     if(hingeAngle >= HINGE_OPEN_ANGLE) {
           interiorLightSignal.Color = greenColor;
      } else {
           interiorLightSignal.Color = redColor;
      }
}

public void LightSensor() {
  var ME = "Jaa8 Bravo"; 
 
  var sensorBlock = GridTerminalSystem.GetBlockWithName("Sensor Base Lights") as IMySensorBlock;
  var SoundBlock = GridTerminalSystem.GetBlockWithName("Sound Block") as IMySoundBlock;
  var lcd = GridTerminalSystem.GetBlockWithName("Wide LCD Panel") as IMyTextPanel;
  
  if(sensorBlock.IsActive && sensorBlock.LastDetectedEntity.Name != ME) {
       Write(lcd,  soundCount.ToString());
       if(soundCount == 0) {
             SoundBlock.Play(); 
             soundCount = ALERT_WAIT_TIME;
        } else {
             soundCount -= 1; 
        }
  }
}

public void Save() {
    Storage = soundCount.ToString(); 
}

public void Write(IMyTextPanel lcd, String text) {
    lcd.WritePublicText(text);
}