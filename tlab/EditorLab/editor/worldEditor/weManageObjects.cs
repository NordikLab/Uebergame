//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Force selected object to fit exactly on the grid
//==============================================================================

//==============================================================================
//Force all selected object to the grid
function WorldEditor::forceToGrid( %this, %obj ) {
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		%this.forceObjectToGrid(%obj);
	}	
}
//------------------------------------------------------------------------------
//==============================================================================
// Force single object to the grid
function WorldEditor::forceObjectToGrid( %this, %obj ) {	
	%transform = %obj.getTransform();
	%gridSize = %this.gridSize;
		%pos = getWords(%transform,0,2);
		%start = %pos;
		%pos.x = mRound(%pos.x/%gridSize)*%gridSize;
		%pos.y = mRound(%pos.y/%gridSize)*%gridSize;
		%pos.z = mRound(%pos.z/%gridSize)*%gridSize;
		%transform = setWord(%transform, 0, %pos.x);
		%transform = setWord(%transform, 1, %pos.y);
		%transform = setWord(%transform, 2, %pos.z);
		%obj.setTransform(%transform);
		
	SceneInspector.refresh();
}
//------------------------------------------------------------------------------

//==============================================================================
// Copy, Clone and Duplicated Objects Functions
//==============================================================================

//==============================================================================
// Make a copie of all selected objects
function WorldEditor::copySelection( %this,%offset, %copies ) {
	%count = EWorldEditor.getSelectionSize();

	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}

	for (%i=1; %i<=%copies; %i++) {
		for( %j=0; %j<%count; %j++) {
			%obj = EWorldEditor.getSelectedObject( %j );

			if( !%obj.isMemberOfClass("SceneObject") ) continue;

			%obj.startDrag = "";
			%clone = %obj.clone();
			%clone.position.x += %offset.x * %i;
			%clone.position.y += %offset.y * %i;
			%clone.position.z += %offset.z * %i;
			MissionGroup.add(%clone);
		}
	}
	SceneInspector.refresh();	
}
//------------------------------------------------------------------------------