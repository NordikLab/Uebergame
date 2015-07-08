//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================

//==============================================================================
// SimObject / SimGroup Helpers
//==============================================================================

//==============================================================================
// SimSet Creator function
function newSimSet( %name, %target,%internalName ) {
	delObj(%name);
	%obj = new SimSet(%name);
	%obj.internalName = %internalName;
	eval("$"@%name@" = %obj;");

	if (isObject(%target))
		%target.add(%obj);

	return %obj;
}
//------------------------------------------------------------------------------
//==============================================================================
// SimSet Creator function
function newSimGroup( %name, %target,%internalName ) {
	delObj(%name);
	%obj = new SimGroup(%name);
	%obj.internalName = %internalName;
	eval("$"@%name@" = %obj;");

	if (isObject(%target))
		%target.add(%obj);

	return %obj;
}
//------------------------------------------------------------------------------


//==============================================================================
function hideChilds(%dlg, %all) {
   devLog("hideChilds",%dlg);  
	if (%all)
		eval(%dlg@".callOnChildren(\"setVisible\",false);");
	else
		eval(%dlg@".callOnChildrenNoRecurse(\"setVisible\",false);");
}
//------------------------------------------------------------------------------
//===============================================================================
function SimSet::setChildField(%this, %field,%value ) {
	foreach(%item in %this) {
		%item.setFieldValue(%field,%value);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
// Basic SimSet Manipulations
//===============================================================================
function SimSet::outputLog(%this, %fields ) {
	foreach(%item in %this) {
		%index = %this.getObjectIndex(%item);
		info("Simset Item", %item,"Name", %item.getName(), "Index = ",%index);
	}
}




//==============================================================================
// Game Group Objects Managements
//===============================================================================
function GameGroup::onAdd(%this, %obj ) {
}

function SimSet::onObjectAdded(%this, %obj ) {
	if (%this.isInstantGroup && $Cfg::Dev::ShowInstaGroupLog)
		info("New object added to instantGroup:",%obj.getName(),"Group is:",%this.getName());
}

//==============================================================================
// Instant Group helpers
//==============================================================================
//==============================================================================
// Get the Screen coords for a 3D position
function instaGroup(%group,%clear) {

	$instantGroup.isInstantGroup= false;
	%group.isInstantGroup= true;
	if (%clear)
		%group.deleteAllObjects();
	$instantGroup = %group;

}
//------------------------------------------------------------------------------
//==============================================================================
// Get the Screen coords for a 3D position
function resetClientCleanup() {
	delObj(ClientMissionCleanup);
	$ClientMissionGroup = new SimGroup(ClientMissionCleanup);

}
//------------------------------------------------------------------------------

function SimSet::exportToFile(%this,%file,%saveAs,%preAppend) {
	%simSet = %this;
	if (%saveAs !$= ""){
		%simSet = %this.deepClone();			
		%tempName = getUniqueName(%saveAs);
		%simSet.setName(%tempName);
		%deleteMe = true;
	}
	%simSet.save(%file,false,%preAppend);
	
	if (%deleteMe)
		delObj(%simSet);
	
}
