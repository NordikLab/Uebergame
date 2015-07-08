//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//============================================================================
//Taking screenshot
//==============================================================================
// New version of updateParamArrayCtrl which send ctrl source after field and value
function syncParamArrayCtrl( %ctrl, %updateFunc,%array,%arg1,%arg2) {
   

   if (%ctrl.getClassName() $= "GuiColorPickerCtrl"){
 
      %ctrl.updateCommand = %updateFunc@"(%ctrl.internalName,%color,%ctrl,\""@%array@"\",\""@%arg1@"\",\""@%arg2@"\");";
      
       %currentColor =   %ctrl.baseColor;
      
      
      	if (%ctrl.isIntColor)
      	{
      		%currentColor =  ColorFloatToInt(%currentColor);
      		 %callBack = %ctrl@".ColorPickedI";
      		 %updateCallback = %ctrl@".ColorUpdatedI";       
      		 GetColorI( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
      	}
			else{
				%callBack = %ctrl@".ColorPicked";
      		%updateCallback = %ctrl@".ColorUpdated";       
				 GetColorF( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
			}
       return;
   }
     
	%field = getWord(strreplace(%ctrl.internalName,"_"," "),0);  
	%special = getWord(strreplace(%ctrl.internalName,"_"," "),1);  
    if (%special $= "Edit"){
    	%value = %ctrl.getText();
		devLog("Special Edit value=",%value,"Else",%ctrl.getTypeValue());
    }
    else
     %value = %ctrl.getTypeValue();  
	
	//Check if a validation type is specified
	if (%ctrl.validationType !$=""){		
		%valType = getWord(%ctrl.validationType,0);
		%valValue = getWord(%ctrl.validationType,1);
	
		if (%valType $= "flen")
			%value = mFloatLength(%value,%valValue);
			
		%ctrl.setTypeValue(%value);
	}
	
	
  // %field = getWord(strreplace(%ctrl.internalName,"_"," "),0);   
    // %value = %ctrl.getTypeValue();  
   

   if (%updateFunc !$= "")
   eval(%updateFunc@"(%field,%value,%ctrl,%array,%arg1,%arg2);");    
   
   if (!%ctrl.noFriends)
   	%ctrl.updateFriends();
   
}
//----------------------------------------------------------------------------
//Load the helpers on execution
