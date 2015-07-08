//==============================================================================
// TorqueLab -> Objects Group functions
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================

//==============================================================================
//Create a SimSet with the Selected objects
function Lab::groupSelectedObjects(%this,%groupName) {
	%count = EWorldEditor.getSelectionSize();
	%grpCount = LabSceneObjectGroups.getCount();

	if (%groupName $= "") %groupName = "ObjGroup_"@%grpCount+1;

	%setName = getUniqueName(%groupName);
	%newSet = newSimset(%setName,LabSceneObjectGroups);
	%newSet.internalName = getUniqueInternalName( "Group", LabSceneObjectGroups, true );
	%newSet.isObjectGroup = true;

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );

		if (isObject( %obj.partOfSet )) {
			%obj.partOfSet.remove(%obj);

			if (!%obj.partOfSet.getCount()) {
				delObj(%obj.partOfSet);
			}
		}

		%obj.partOfSet = %newSet.getName();
		%newSet.add(%obj);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Remove selected objects from their group (delete group if empty)
function Lab::ungroupSelectedObjects(%this,%groupName) {
	%count = EWorldEditor.getSelectionSize();

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );

		if (isObject( %obj.partOfSet )) {
			%obj.partOfSet.remove(%obj);

			if (!%obj.partOfSet.getCount()) {
				delObj(%obj.partOfSet);
			}
		}

		%obj.partOfSet = "";
	}

	EWorldEditor.clearSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::selectObjectGroup(%this,%groupName,%append) {
	//If no group specified, select the group of selected object (0)
	if (!isObject(%groupName)) {
		%selectObj = EWorldEditor.getSelectedObject(0);

		if (isObject(%selectObj.partOfSet)) {
			EWorldEditor.clearSelection();

			foreach(%obj in %selectObj.partOfSet) {
				EWorldEditor.selectObject(%obj);
			}
		}
	} else {
		//Select the objects of specified group
		if (!%append)
			EWorldEditor.clearSelection();

		foreach(%obj in %groupName) {
			%obj.byGroup = true;
			EWorldEditor.selectObject(%obj);
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::saveSceneObjectGroups(%this) {
	%file = strreplace($Server::MissionFile,".mis",".objgroups");

	foreach(%set in LabSceneObjectGroups) {
		%objList = "";

		foreach(%obj in %set) {
			if (%obj.internalName $= "")
				%obj.internalName = %set.internalName SPC %set.getObjectIndex(%obj);

			%objList = trim(%objList SPC %obj.internalName);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::loadSceneObjectGroups(%this) {
	LabSceneObjectGroups.clear();
	%this.loadSceneGroupsInGroup(MissionGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::loadSceneGroupsInGroup(%this,%group) {
	foreach(%obj in %group) {
		if (%obj.getClassName() $= "SimGroup") {
			Lab.loadSceneGroupsInGroup(%obj);
			continue;
		}

		if (%obj.partOfSet !$= "") {
			if (!isObject(%obj.partOfSet)) {
				%set = newSimset(%obj.partOfSet,LabSceneObjectGroups);
				%set.isObjectGroup = true;
			} else
				%set = %obj.partOfSet;

			if (%set.isMember(%obj)) {
				continue;
			}

			%set.add(%obj);
		}
	}
}
//------------------------------------------------------------------------------