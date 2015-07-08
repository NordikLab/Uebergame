//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// World Editor Grid Functions
//==============================================================================

//==============================================================================

function EWorldEditor::getGridSnap( %this ) {
	return %this.gridSnap;
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::setGridSnap( %this, %valueOrCtrl ) {
	if (isObject(%valueOrCtrl))
		%value = %valueOrCtrl.getValue();
	else
		%value = %valueOrCtrl;
	%this.gridSnap = %value;
	Lab.setGizmoGridSnap(%value);
	%this.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::getGridSize( %this ) {
	return %this.gridSize;
}
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::setGridSize( %this, %valueOrCtrl ) {
	if (isObject(%valueOrCtrl))
		%value = %valueOrCtrl.getValue();
	else
		%value = %valueOrCtrl;
	Lab.setGizmoGridSize(%value SPC %value SPC %value);
	%this.gridSize = %value;

	if (EditorIsActive())
		%this.syncGui();
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
