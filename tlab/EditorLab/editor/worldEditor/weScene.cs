//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function WorldEditor::getSelectionLockCount(%this) {
	%ret = 0;
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);

		if(%obj.locked)
			%ret++;
	}

	return %ret;
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::getSelectionHiddenCount(%this) {
	%ret = 0;
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);

		if(%obj.hidden)
			%ret++;
	}

	return %ret;
}
//------------------------------------------------------------------------------

//==============================================================================
/// Pastes the selection at the same place (used to move obj from a group to another)
function WorldEditor::moveSelectionInPlace(%this) {
	%saveDropType = %this.dropType;
	%this.dropType = "atCentroid";
	%this.copySelection();
	%this.deleteSelection();
	%this.pasteSelection();
	%this.dropType = %saveDropType;
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::addSelectionToAddGroup(%this) {
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		$InstantGroup.add(%obj);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// resets the scale and rotation on the selection set
function WorldEditor::resetTransforms(%this) {
	%this.addUndoState();

	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		%transform = %obj.getTransform();
		%transform = setWord(%transform, 3, "0");
		%transform = setWord(%transform, 4, "0");
		%transform = setWord(%transform, 5, "1");
		%transform = setWord(%transform, 6, "0");
		//
		%obj.setTransform(%transform);
		%obj.setScale("1 1 1");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::deleteMissionObject( %this, %object ) {
	// Unselect in editor tree.
	%id = SceneEditorTree.findItemByObjectId( %object );
	SceneEditorTree.selectItem( %id, false );
	// Delete object.
	MEDeleteUndoAction::submit( %object );
	EWorldEditor.isDirty = true;
	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::selectAllObjectsInSet( %this, %set, %deselect ) {
	if( !isObject( %set ) )
		return;

	foreach( %obj in %set ) {
		if( %deselect )
			%this.unselectObject( %obj );
		else
			%this.selectObject( %obj );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function EWorldEditor::addSimGroup( %this, %groupCurrentSelection ) {
	%activeSelection = %this.getActiveSelection();

	if ( %activeSelection.getObjectIndex( MissionGroup ) != -1 ) {
		LabMsgOK( "Error", "Cannot add MissionGroup to a new SimGroup" );
		return;
	}

	// Find our parent.
	%parent = MissionGroup;

	if( !%groupCurrentSelection && isObject( %activeSelection ) && %activeSelection.getCount() > 0 ) {
		%firstSelectedObject = %activeSelection.getObject( 0 );

		if( %firstSelectedObject.isMemberOfClass( "SimGroup" ) )
			%parent = %firstSelectedObject;
		else if( %firstSelectedObject.getId() != MissionGroup.getId() )
			%parent = %firstSelectedObject.parentGroup;
	}

	// If we are about to do a group-selected as well,
	// starting recording an undo compound.

	if( %groupCurrentSelection )
		Editor.getUndoManager().pushCompound( "Group Selected" );

	// Create the SimGroup.
	%object = new SimGroup() {
		parentGroup = %parent;
	};
	MECreateUndoAction::submit( %object );

	// Put selected objects into the group, if requested.

	if( %groupCurrentSelection && isObject( %activeSelection ) ) {
		%undo = UndoActionReparentObjects::create( SceneEditorTree );
		%numObjects = %activeSelection.getCount();

		for( %i = 0; %i < %numObjects; %i ++ ) {
			%sel = %activeSelection.getObject( %i );
			%undo.add( %sel, %sel.parentGroup, %object );
			%object.add( %sel );
		}

		%undo.addToManager( Editor.getUndoManager() );
	}

	// Stop recording for group-selected.

	if( %groupCurrentSelection )
		Editor.getUndoManager().popCompound();

	// When not grouping selection, make the newly created SimGroup the
	// current selection.

	if( !%groupCurrentSelection ) {
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject( %object );
	}

	// Refresh the Gui.
	%this.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::toggleLockChildren( %this, %simGroup ) {
	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleLockChildren( %child );
		else
			%child.setLocked( !%child.locked );
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::toggleHideChildren( %this, %simGroup ) {
	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleHideChildren( %child );
		else
			%this.hideObject( %child, !%child.hidden );
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::getNewObjectGroup( %this ) {
	return SceneCreatorWindow.getNewObjectGroup();
}
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::convertSelectionToPolyhedralObjects( %this, %className ) {
	%group = %this.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();
	%activeSelection = %this.getActiveSelection();

	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createPolyhedralObject( %className, %oldObject );

		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert ConvexShape to " @ %className );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::convertSelectionToConvexShape( %this ) {
	%group = %this.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();
	%activeSelection = %this.getActiveSelection();

	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createConvexShapeFrom( %oldObject );

		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert " @ %oldObject.getClassName() @ " to ConvexShape" );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================

function EWorldEditor::areAllSelectedObjectsOfType( %this, %className ) {
	%activeSelection = %this.getActiveSelection();

	if( !isObject( %activeSelection ) )
		return false;

	%count = %activeSelection.getCount();

	for( %i = 0; %i < %count; %i ++ ) {
		%obj = %activeSelection.getObject( %i );

		if( !%obj.isMemberOfClass( %className ) )
			return false;
	}

	return true;
}

//------------------------------------------------------------------------------
