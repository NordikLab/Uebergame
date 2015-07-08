//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//============================================================================
//Taking screenshot
function loadHelpers() {
	%pattern = "./*.cs";

	for( %file = findFirstFile( %pattern ); %file !$= ""; %file = findNextFile( %pattern)) {
		if (fileBase(%file) $= "initHelpers") continue;

		exec(%file);
	}

	%pattern = "./*.cs.dso";

	for( %file = findFirstFile( %pattern ); %file !$= ""; %file = findNextFile( %pattern)) {
		if (fileBase(%file) $= "initHelpers") continue;

		exec(%file);
	}

	$HelperLabLoaded = true;
}
//----------------------------------------------------------------------------
//Load the helpers on execution
loadHelpers();