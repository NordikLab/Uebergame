//==============================================================================
// HelpersLab -> General helpers function to manage GUIs
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$HLab_DefaultGui = "MainMenuGui";
//==============================================================================
// Push, Pop and Toggle Dialogs
//==============================================================================

//==============================================================================
//Push a dialog to the canvas
function pushDlg(%dlg) {
	Canvas.pushDialog(%dlg);
}
//------------------------------------------------------------------------------
//==============================================================================
//Pop a dialog from the canvas
function popDlg(%dlg) {
	Canvas.popDialog(%dlg);
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle a Dialog GUI
function toggleDlg(%dlg) {
	if (%dlg.isAwake())
		popDlg(%dlg);
	else
		pushDlg(%dlg);
}
//------------------------------------------------------------------------------

//==============================================================================
// Content GUI helpers (Canvas.setContent() simplified)
//==============================================================================
//==============================================================================
// Set canvas content and store previous GUI (Can be specified optionally)
function setGui(%gui,%previousGui) {
	$PreviousGui = Canvas.getContent();
	Canvas.setContent(%gui);
	if (isObject(%previousGui))
		$PreviousGui = %previousGui;
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the Previous canvas content that was loaded before the current content
function setPrevGui(%defaultGui) { 
   if (%defaultGui $= "")
      %defaultGui = $HLab_DefaultGui;  
   %setGui = $PreviousGui;
   if (!isObject($PreviousGui) && isObject(%defaultGui)){
      warnLog("Couldn't find a previous GUI, calling the Default GUI:",%defaultGui);
      %setGui = %defaultGui;
   }
   
   if (!isObject(%setGui)){   
      warnLog("There's no previous GUI Stored! Nothing done...");
      return;      
   }	   
   Canvas.setContent(%setGui);	
}
//------------------------------------------------------------------------------

//==============================================================================
// Toggle between Specified GUI and the PreviousGui setted
function toggleGui(%gui) {
	if (!isObject(%gui))return;
	if (Canvas.getContent().getId() $= %gui.getId())
		setGui($PreviousGui);
	else
		setGui(%gui);
}
//------------------------------------------------------------------------------




