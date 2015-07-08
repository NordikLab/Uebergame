//==============================================================================
// Boost! -> Helper functions for common string manipulation
// Copyright NordikLab Studio, 2013
//==============================================================================
$Game::DumpFolder = "core/dump/";
//==============================================================================
// Validate a string of specific needs
//==============================================================================

//==============================================================================
// Validate a string used for a filename
function dumpGlobal(%dumpList,%file) {

	if (%file $="") %file = getWord(%dumpList,0);

	foreach$(%str in %dumpList)
		export("$"@%str@"*", $Game::DumpFolder@"dumpGlobals_"@%file@".cs", False);
}
//------------------------------------------------------------------------------

//==============================================================================
// Next Dumping functions might need updates
//==============================================================================

//==============================================================================
// Dump Standing info to console
function dump(%type) {	

	if (%type $= "") {
		info("Dump function type list:","fontcache");
		return;
	}
	switch$(%type) {
	case "fontcache":
		dumpFontCacheStatus();
	}
}
//------------------------------------------------------------------------------

function dumpGroup(%group, %file) {
   foreach (%obj in %group)
      dumpObject(%obj,%file);
}

//==============================================================================
// Dump array object content
function dumpObject(%obj, %file) {

	%class = %obj.getClassName();
	log("DumpObject of class:",%class);
	switch$(%class) {
	case "ScriptObject":
		dumpScriptObject(%obj, %file);

	case "ArrayObject":
		dumpArrayObject(%obj, %file);
	}

}
//------------------------------------------------------------------------------
//==============================================================================
// Dump array object content
function dumpScriptObject(%obj, %file) {

	%name = %obj.getName();
	if (%name $= "")
		%name = %obj;

	%count = %obj.getDynamicFieldCount();
	for(%i=0; %i<%count; %i++) {
		%fieldFull = %obj.getDynamicField(%i);
		%field = getField(%fieldFull,0);
		%value = getField(%fieldFull,1);

		$DumpObj[%name,%field] = %value;
		log("Dump Script Object --> Field",%field," <=> Value:",%value);

	}
	if (%file !$="")
		export("$DumpObj"@%name@"*", $Game::DumpFolder@"object/"@%file@".cs", false);
}
//------------------------------------------------------------------------------

//==============================================================================
// Dump array object content
function dumpArrayObject(%obj,%file) {

	%name = %obj.getName();
	if (%name $= "")
		%name = %obj;
	%count = %obj.count();
	for(%i=0; %i<%count; %i++) {
		%field = %obj.getKey(%i);
		%value = %obj.getValue(%i);
		$DumpArray[%name,%field] = %value;

		info("Dump Array Object --> Field",%field," <=> Value:",%value);
	}

	if (%file !$="")
		export("$DumpArray"@%name@"*", $Game::DumpFolder@"array/"@%file@".cs", false);
}
//------------------------------------------------------------------------------

//==============================================================================
// Dump array object content
function dumpMissionCleanup() {
	log("dumpMissionCleanup");
	foreach(%obj in MissionCleanup) {
		info("Object Class:",%obj.getClassName(),"Name:",%obj.getName());

		switch$(%obj.getClassName()) {
		case "SimGroup":
			foreach(%subObj in %obj) {
				info("----SimGroup Object Class:",%obj.getClassName(),"Name:",%obj.getName());
			}
		case "SimSet":
			foreach(%subObj in %obj) {
				info("----SimSet Object Class:",%obj.getClassName(),"Name:",%obj.getName());
			}
		}

	}

}
//------------------------------------------------------------------------------
//==============================================================================
// Basic SimSet Manipulations
//===============================================================================
function dumpObjectDynamic(%obj ) {
	// Create a file object for writing
	%fileWrite = new FileObject();
	%fileWrite.OpenForWrite($Game::DumpFolder@"objects/dynamic"@%obj.getName()@".cs");

	%fieldCount = %obj.getDynamicFieldCount();
	for (%i=0; %i<%fieldCount; %i++) {
		%fieldFull = %obj.getDynamicField(%i);
		%field = getField(%fieldFull,0);
		%value = getField(%fieldFull,1);

		// Write a line to the text files
		%fileWrite.writeLine("$Dump_"@%obj.getName()@"_"@%field@" = \""@%value@"\";");
	}
	%fileWrite.close();
	%fileWrite.delete();
}
function dumpObjectToFile(%obj ) {
	// Create a file object for writing
	%fileWrite = new FileObject();
	%fileWrite.OpenForWrite($Game::DumpFolder@"objects/full"@%obj.getName()@".cs");

	%fileWrite.writeObject(%obj);

	%fileWrite.close();
	%fileWrite.delete();
}

function dumpGroupToFile(%obj ,%childsOnly) {
	// Create a file object for writing
	%fileWrite = new FileObject();
	if (%childsOnly) {
		%fileWrite.OpenForWrite($Game::DumpFolder@"objects/groupChilds_"@%obj.getName()@".cs");
		foreach(%sub in %obj) %fileWrite.writeObject(%sub);
	} else {
		%fileWrite.OpenForWrite($Game::DumpFolder@"objects/groupFull_"@%obj.getName()@".cs");
		%fileWrite.writeObject(%obj);
	}
	%fileWrite.close();
	%fileWrite.delete();
}