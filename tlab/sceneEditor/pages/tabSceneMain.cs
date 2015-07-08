//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
function SEP_ScenePage::updateContent( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// AUTO MISSION GROUP ORGANIZER SYSTEM
//------------------------------------------------------------------------------
// Used to clean up MissionGroup object by assigning Object class to a default
// group naming as define in settings. The Sub groups will be reordered as the
// groups are listed in below autoGroups listing
//==============================================================================
SEP_ScenePage.autoGroups = "Core Environment Ambient TSStatic Spawn MiscObject";

//==============================================================================
// Reorganize the Mission objects (only root items by default (%maxDepth = 1)
function SEP_ScenePage::organizeMissionGroup( %this,%maxDepth ) {
	if (%maxDepth $= "")
		%maxDepth = 1;

	%this.organizeGroupDepth = %maxDepth;

	//Go through all Scene objects and move them to define groups
	foreach$(%group in SEP_ScenePage.autoGroups)
		%this.organizedGroupList[%group] = "";

	%this.organizeGroup(MissionGroup,1);

	foreach$(%group in SEP_ScenePage.autoGroups)
		%this.doOrganizeGroup(%group);

	%this.reorderMissionGroup();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScenePage::organizeGroup( %this,%group,%depth ) {
	//Go through all Scene objects and move them to define groups
	foreach(%obj in %group) {
		if (%obj.isMemberOfClass("SimGroup")) {
			if (%depth < %this.organizeGroupDepth)
				%this.organizeGroup(%obj);

			continue;
		}

		%class = %obj.getClassName();

		if (strFindWords(%class,"MissionArea LevelInfo")) {		
			%this.organizedGroupList["Core"] = strAddWord(%this.organizedGroupList["Core"],%obj);
		} else if (strFindWords(%class,"Water Terrain River Road GroundCover Forest")) {		
			%this.organizedGroupList["Environment"] = strAddWord(%this.organizedGroupList["Environment"],%obj);
		} else if (strFindWords(%class,"Precipitation Sky Time Cloud")) {
			%this.organizedGroupList["Ambient"] = strAddWord(%this.organizedGroupList["Ambient"],%obj);
		} else if (strFindWords(%class,"TSStatic Prefab")) {
			%this.organizedGroupList["TSStatic"] = strAddWord(%this.organizedGroupList["TSStatic"],%obj);
		} else if (strFindWords(%class,"Spawn")) {
			%this.organizedGroupList["Spawn"] = strAddWord(%this.organizedGroupList["Spawn"],%obj);
		} else  {
			%this.organizedGroupList["MiscObject"] = strAddWord(%this.organizedGroupList["MiscObject"],%obj);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScenePage::doOrganizeGroup( %this,%group ) {
	%list = %this.organizedGroupList[%group];

	foreach$(%obj in %list) {
		%this.addObjToGroup(%obj,%group);
	}

	%this.organizedGroupList[%group] = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScenePage::addObjToGroup( %this,%obj,%group ) {
	//Go through all Scene objects and move them to define groups
	eval("%groupName = SceneEditorCfg."@%group@"Group;");

	if (%groupName $= "")
		return;

	if (!isObject(%groupName))
		newSimGroup(%groupName,MissionGroup);

	%groupName.add(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
//SEP_ScenePage.reorderMissionGroup
function SEP_ScenePage::reorderMissionGroup( %this ) {
	%list = SEP_ScenePage.autoGroups;
	%count = getWordCount(%list);
	%initial = true;

	for(%i = (%count-1); %i>=0; %i--) {
		%first = getWord(%list,%i);

		if (%initial) {
			eval("%groupFirst = SceneEditorCfg."@%first@"Group;");		
			MissionGroup.bringToFront(%groupFirst);
			%initial = false;
			continue;
		}

		%next = getWord(%list,%i+1);
		eval("%groupFirst = SceneEditorCfg."@%first@"Group;");
		eval("%groupNext = SceneEditorCfg."@%next@"Group;");
		

		if (!isObject(%groupFirst)) {		
			continue;
		}

		if (!isObject(%groupNext)) {			
			continue;
		}

		MissionGroup.reorderChild(	%groupFirst,%groupNext);
	}
}
//------------------------------------------------------------------------------

