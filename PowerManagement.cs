//Set Reactor Name, LCDName, High Battery Charge Threshold, and Low Battery Charge Threshold  
   
String reactorSearchString = "Reactor";    
String lcdName = "PMS Output Text Panel";   
Double batteryLowThreshold = .10;  
Double batteryHighThreshold = .75;  
   
//No further action needed from user after this point   
   
void Main (String action)    
{    
switch (action)    
	{     
		default: 		   
		managePower();   
break;  
				    
	}    
}    
    
void managePower()    
{    
List<IMyTerminalBlock> reactors = new List<IMyTerminalBlock>(); 
GridTerminalSystem.SearchBlocksOfName(reactorSearchString, reactors); 

List<IMyTerminalBlock> batteries = new List<IMyTerminalBlock>();    
GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);    
    
double totalBatteryMaximum = 0.0;    
double totalBatteryCharge = 0.0;    
String unitoutput = "";    
for(int batteryIndex = 0; batteryIndex < batteries.Count; batteryIndex++)     
{    
    
	String batteryDetails = (batteries[batteryIndex] as IMyTerminalBlock).DetailedInfo;    
	    
	String searchString = "Max Stored Power: ";    
	int startIndex = batteryDetails.IndexOf(searchString) + searchString.Length;    
	double maxPower = Double.Parse(batteryDetails.Substring(startIndex, 3));    
	    
	searchString = "Fully";    
	startIndex = batteryDetails.IndexOf(searchString) - 4;    
    
	    
	unitoutput = batteryDetails.Substring(startIndex, 3);    
	double multiple = 0;    
	if (unitoutput == "kWh")    
		multiple = .001;    
			else    
				multiple = 1.00;    
		    
	    
	searchString = "Stored power: ";    
	startIndex = batteryDetails.IndexOf(searchString) + searchString.Length;    
	double storedPower = multiple * Double.Parse(batteryDetails.Substring(startIndex, 3));    
	    
    
    
	totalBatteryMaximum += maxPower;    
	totalBatteryCharge += storedPower;    
    
}    
    
double averageBatteryPercentage = totalBatteryCharge/totalBatteryMaximum;    
			    
IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName(lcdName) as IMyTextPanel;     
  
String outputToLCD = "Power Mgmt System";   
    
if (averageBatteryPercentage < batteryLowThreshold )    
	{    
		for (int reactorIndex = 0; reactorIndex < reactors.Count; reactorIndex++) 
		{ 
			reactors[reactorIndex].GetActionWithName("OnOff_On").Apply(reactors[reactorIndex]);  
		} 
 
  		outputToLCD += "\n-Battery Recovery Mode-";    
	}    
		else if (averageBatteryPercentage > batteryHighThreshold )    
			{    
			 
				for (int reactorIndex = 0; reactorIndex < reactors.Count; reactorIndex++) 
				{ 
					reactors[reactorIndex].GetActionWithName("OnOff_Off").Apply(reactors[reactorIndex]);  
				}   
				 
				outputToLCD += "\n-Uranium Conservation Mode-";     
			}	  
outputToLCD += "\n\n-Thresholds-";  
outputToLCD += "\nLow Battery Charge Threshold: " + String.Format("{0:0.00}%",100*batteryLowThreshold);   
outputToLCD += "\nHigh Battery Charge Threshold: " + String.Format("{0:0.00}%",100*batteryHighThreshold);   
outputToLCD += "\n\n-Batteries-";    
outputToLCD += "\nMaximum Battery Capacity: " + totalBatteryMaximum +" MWh";    
outputToLCD += "\nCurrent Battery Charge: " + String.Format("{0:0.00} MWh",totalBatteryCharge);    
outputToLCD += "\nCurrent Battery Charge %: " + String.Format("{0:0.00}%",100*averageBatteryPercentage);    
// outputToLCD +="\n\n-Reactor-\n" + "Name: " + reactorName + "\n" + reactor.DetailedInfo;    
    
  
  
lcd.WritePublicText(outputToLCD);  		    
			    
}    
 