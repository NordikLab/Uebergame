//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================


function doListDatablockClass(%class) {
   %list = "";
	foreach(%data in DataBlockGroup){	  
	   if (%data.isMemberOfClass( %class) ){
	     
	      %list = trim(%list SPC %data.getName());	      
	   }
	}
	return %list;
}