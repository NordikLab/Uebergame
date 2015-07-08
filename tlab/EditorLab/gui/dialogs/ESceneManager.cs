//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function ESceneManager::toggleVisibility( %this ) {
	if( %this.isVisible() )
		%this.setVisible( false );
	else {
		%this.setVisible( true );
		%this.selectWindow();
		%this.setCollapseGroup(false);
		%this.onWake();
	}
}

//------------------------------------------------------------------------------

//==============================================================================
function ESceneManager::onWake( %this ) {
	%this.initGroupTree(true);
}
//------------------------------------------------------------------------------

//==============================================================================
function ESceneManager::initGroupTree( %this,%reset ) {
	//if ( ESceneManager.groupTreeBuilt && !%reset)
	// return;
	// if(%reset)
	SceneManagerGroupsTree.clear();
	SceneManagerGroupsTree.open(LabSceneObjectGroups,true);
	ESceneManager.groupTreeBuilt = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneManagerGroupsTree::onSelect( %this,%itemId ) {
	if (%itemId.getClassName() $= "SimSet" && %itemId.isObjectGroup) {
		Lab.selectObjectGroup(%itemId);
		return;
	} else if (isObject(%itemId)) {
		if (%itemId.internalName $= "") {
			%itemId.internalName = "searchforme";
			%tmpIntName = true;
		}

		%missionObj = MissionGroup.findObjectByInternalName(%itemId.internalName,true);

		if (isObject(%missionObj)) {
			EWorldEditor.clearSelection();
			EWorldEditor.selectObject(%missionObj);
		}

		if (%itemId.internalName $= "searchforme")
			%itemId.internalName = "";
	}

	//Unselect all and select this group
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneManagerGroupsTree::handleRenameObject( %this,%newName,%itemId ) {
	if (%itemId.getClassName() !$= "SimSet" || !%itemId.isObjectGroup)
		return;

	foreach(%obj in %itemId) {
		%obj.partOfSet = %newName;
	}
}
//------------------------------------------------------------------------------
