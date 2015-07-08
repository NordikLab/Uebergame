//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

/// @name EditorPlugin Methods
/// @{


//-----------------------------------------------------------------------------

function SceneEditorTree::onDeleteSelection( %this ) {
	%this.undoDeleteList = "";
}

function SceneEditorTree::onDeleteObject( %this, %object ) {
	// Don't delete locked objects
	if( %object.locked )
		return true;

	if( %object == SceneCreatorWindow.objectGroup )
		SceneCreatorWindow.setNewObjectGroup( MissionGroup );

	// Append it to our list.
	%this.undoDeleteList = %this.undoDeleteList TAB %object;
	// We're gonna delete this ourselves in the
	// completion callback.
	return true;
}

function SceneEditorTree::onObjectDeleteCompleted( %this ) {
	// This can be called when a deletion is attempted but nothing was
	// actually deleted ( cannot delete the root of the tree ) so only submit
	// the undo if we really deleted something.
	if ( %this.undoDeleteList !$= "" )
		MEDeleteUndoAction::submit( %this.undoDeleteList );

	// Let the world editor know to
	// clear its selection.
	EWorldEditor.clearSelection();
	EWorldEditor.isDirty = true;
}

function SceneEditorTree::onClearSelected(%this) {
	WorldEditor.clearSelection();
}

function SceneEditorTree::onInspect(%this, %obj) {
	SceneInspector.inspect(%obj);
}

function SceneEditorTree::toggleLock( %this ) {
	if(  SceneTreeWindow-->LockSelection.command $= "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();" ) {
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(false); SceneEditorTree.toggleLock();";
		SceneTreeWindow-->DeleteSelection.command = "";
	} else {
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();";
		SceneTreeWindow-->DeleteSelection.command = "EditorMenuEditDelete();";
	}
}

function SceneEditorTree::onAddSelection(%this, %obj, %isLastSelection) {
	EWorldEditor.selectObject( %obj );
	%selSize = EWorldEditor.getSelectionSize();
	%lockCount = EWorldEditor.getSelectionLockCount();

	if( %lockCount < %selSize ) {
		SceneTreeWindow-->LockSelection.setStateOn(0);
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();";
	} else if ( %lockCount > 0 ) {
		SceneTreeWindow-->LockSelection.setStateOn(1);
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(false); SceneEditorTree.toggleLock();";
	}

	if( %selSize > 0 && %lockCount == 0 )
		SceneTreeWindow-->DeleteSelection.command = "EditorMenuEditDelete();";
	else
		SceneTreeWindow-->DeleteSelection.command = "";

	if( %isLastSelection )
		SceneInspector.addInspect( %obj );
	else
		SceneInspector.addInspect( %obj, false );
}
function SceneEditorTree::onRemoveSelection(%this, %obj) {
	EWorldEditor.unselectObject(%obj);
	SceneInspector.removeInspect( %obj );
}
function SceneEditorTree::onSelect(%this, %obj) {
	if (%obj.getClassName() $= "SimGroup") {		
		SceneCreatorWindow.setActiveSimGroup(%obj);
	}

	if (%obj.getClassName() $= "GroundCover") {	
		SEP_GroundCover.onGroundCoverSelected(%obj);
	}

	if (!$SceneEditor_AutomaticObjectGroup && %this.autoGroups) {
		%this.autoGroups = false;
		SceneCreatorWindow.setNewObjectGroup( MissionGroup );
		return;
	}

	%this.autoGroups = true;

	if (%obj.getClassName() $= "SimGroup") {
		%objectGroup = %obj;
	} else if (isObject(%obj.parentGroup)) {
		%objectGroup = %obj.parentGroup;
	} else {
		%objectGroup = MissionGroup;
	}

	SceneCreatorWindow.setNewObjectGroup( %objectGroup );
}

function SceneEditorTree::onUnselect(%this, %obj) {
	if (%obj.getClassName() $= "GroundCover") {		
		SEP_GroundCover.onGroundCoverUnselected(%obj);
	}

	EWorldEditor.unselectObject(%obj);
}

function SceneEditorTree::onDragDropped(%this) {
	EWorldEditor.isDirty = true;
}

function SceneEditorTree::onAddGroupSelected(%this, %group) {
	SceneCreatorWindow.setNewObjectGroup(%group);
}

function SceneEditorTree::onRightMouseUp( %this, %itemId, %mouse, %obj ) {
	%haveObjectEntries = false;
	%haveLockAndHideEntries = true;

	// Handle multi-selection.
	if( %this.getSelectedItemsCount() > 1 ) {
		%popup = ETMultiSelectionContextPopup;

		if( !isObject( %popup ) )
			%popup = new PopupMenu( ETMultiSelectionContextPopup ) {
			superClass = "MenuBuilder";
			isPopup = "1";
			item[ 0 ] = "Delete" TAB "" TAB "EditorMenuEditDelete();";
			item[ 1 ] = "Group" TAB "" TAB "EWorldEditor.addSimGroup( true );";
		};
	}
	// Open context menu if this is a CameraBookmark
	else if( %obj.isMemberOfClass( "CameraBookmark" ) ) {
		%popup = ETCameraBookmarkContextPopup;

		if( !isObject( %popup ) )
			%popup = new PopupMenu( ETCameraBookmarkContextPopup ) {
			superClass = "MenuBuilder";
			isPopup = "1";
			item[ 0 ] = "Go To Bookmark" TAB "" TAB "EManageBookmarks.jumpToBookmark( %this.bookmark.getInternalName() );";
			bookmark = -1;
		};

		ETCameraBookmarkContextPopup.bookmark = %obj;
	}
	// Open context menu if this is set CameraBookmarks group.
	else if( %obj.name $= "CameraBookmarks" ) {
		%popup = ETCameraBookmarksGroupContextPopup;

		if( !isObject( %popup ) )
			%popup = new PopupMenu( ETCameraBookmarksGroupContextPopup ) {
			superClass = "MenuBuilder";
			isPopup = "1";
			item[ 0 ] = "Add Camera Bookmark" TAB "" TAB "EManageBookmarks.addCameraBookmarkByGui();";
		};
	}
	// Open context menu if this is a SimGroup
	else if( %obj.isMemberOfClass( "SimGroup" ) ) {
		%popup = ETSimGroupContextPopup;

		if( !isObject( %popup ) )
			%popup = new PopupMenu( ETSimGroupContextPopup ) {
			superClass = "MenuBuilder";
			isPopup = "1";
			item[ 0 ] = "Rename" TAB "" TAB "SceneEditorTree.showItemRenameCtrl( SceneEditorTree.findItemByObjectId( %this.object ) );";
			item[ 1 ] = "Delete" TAB "" TAB "EWorldEditor.deleteMissionObject( %this.object );";
			item[ 2 ] = "Inspect" TAB "" TAB "inspectObject( %this.object );";
			item[ 3 ] = "-";
			item[ 4 ] = "Toggle Lock Children" TAB "" TAB "EWorldEditor.toggleLockChildren( %this.object );";
			item[ 5 ] = "Toggle Hide Children" TAB "" TAB "EWorldEditor.toggleHideChildren( %this.object );";
			item[ 6 ] = "-";
			item[ 7 ] = "Group" TAB "" TAB "EWorldEditor.addSimGroup( true );";
			item[ 8 ] = "-";
			item[ 9 ] = "Add New Objects Here" TAB "" TAB "SceneCreatorWindow.setNewObjectGroup( %this.object );";
			item[ 10 ] = "Add Children to Selection" TAB "" TAB "EWorldEditor.selectAllObjectsInSet( %this.object, false );";
			item[ 11 ] = "Remove Children from Selection" TAB "" TAB "EWorldEditor.selectAllObjectsInSet( %this.object, true );";
			object = -1;
		};

		%popup.object = %obj;

		%hasChildren = %obj.getCount() > 0;

		%popup.enableItem( 10, %hasChildren );

		%popup.enableItem( 11, %hasChildren );

		%haveObjectEntries = true;

		%haveLockAndHideEntries = false;
	}
	// Open generic context menu.
	else {
		%popup = ETContextPopup;

		if( !isObject( %popup ) )
			%popup = new PopupMenu( ETContextPopup ) {
			superClass = "MenuBuilder";
			isPopup = "1";
			item[ 0 ] = "Rename" TAB "" TAB "SceneEditorTree.showItemRenameCtrl( SceneEditorTree.findItemByObjectId( %this.object ) );";
			item[ 1 ] = "Delete" TAB "" TAB "EWorldEditor.deleteMissionObject( %this.object );";
			item[ 2 ] = "Inspect" TAB "" TAB "inspectObject( %this.object );";
			item[ 3 ] = "-";
			item[ 4 ] = "Locked" TAB "" TAB "%this.object.setLocked( !%this.object.locked ); EWorldEditor.syncGui();";
			item[ 5 ] = "Hidden" TAB "" TAB "EWorldEditor.hideObject( %this.object, !%this.object.hidden ); EWorldEditor.syncGui();";
			item[ 6 ] = "-";
			item[ 7 ] = "Group" TAB "" TAB "EWorldEditor.addSimGroup( true );";
			object = -1;
		};

		// Specialized version for ConvexShapes.
		if( %obj.isMemberOfClass( "ConvexShape" ) ) {
			%popup = ETConvexShapeContextPopup;

			if( !isObject( %popup ) )
				%popup = new PopupMenu( ETConvexShapeContextPopup : ETContextPopup ) {
				superClass = "MenuBuilder";
				isPopup = "1";
				item[ 8 ] = "-";
				item[ 9 ] = "Convert to Zone" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"Zone\" );";
				item[ 10 ] = "Convert to Portal" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"Portal\" );";
				item[ 11 ] = "Convert to Occluder" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"OcclusionVolume\" );";
				item[ 12 ] = "Convert to Sound Space" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"SFXSpace\" );";
			};
		}
		// Specialized version for polyhedral objects.
		else if( %obj.isMemberOfClass( "Zone" ) ||
					%obj.isMemberOfClass( "Portal" ) ||
					%obj.isMemberOfClass( "OcclusionVolume" ) ||
					%obj.isMemberOfClass( "SFXSpace" ) ) {
			%popup = ETPolyObjectContextPopup;

			if( !isObject( %popup ) )
				%popup = new PopupMenu( ETPolyObjectContextPopup : ETContextPopup ) {
				superClass = "MenuBuilder";
				isPopup = "1";
				item[ 8 ] = "-";
				item[ 9 ] = "Convert to ConvexShape" TAB "" TAB "EWorldEditor.convertSelectionToConvexShape();";
			};
		}

		%popup.object = %obj;
		%haveObjectEntries = true;
	}

	if( %haveObjectEntries ) {
		%popup.enableItem( 0, %obj.isNameChangeAllowed() && %obj.getName() !$= "MissionGroup" );
		%popup.enableItem( 1, %obj.getName() !$= "MissionGroup" );

		if( %haveLockAndHideEntries ) {
			%popup.checkItem( 4, %obj.locked );
			%popup.checkItem( 5, %obj.hidden );
		}

		%popup.enableItem( 7, %this.isItemSelected( %itemId ) );
	}

	%popup.showPopup( Canvas );
}

function SceneEditorTree::positionContextMenu( %this, %menu ) {
	if( (getWord(%menu.position, 0) + getWord(%menu.extent, 0)) > getWord(EWorldEditor.extent, 0) ) {
		%posx = getWord(%menu.position, 0);
		%offset = getWord(EWorldEditor.extent, 0) - (%posx + getWord(%menu.extent, 0)) - 5;
		%posx += %offset;
		%menu.position = %posx @ " " @ getWord(%menu.position, 1);
	}
}

function SceneEditorTree::isValidDragTarget( %this, %id, %obj ) {
	if( %obj.isMemberOfClass( "Path" ) )
		return EWorldEditor.areAllSelectedObjectsOfType( "Marker" );

	if( %obj.name $= "CameraBookmarks" )
		return EWorldEditor.areAllSelectedObjectsOfType( "CameraBookmark" );
	else
		return ( %obj.getClassName() $= "SimGroup" );
}

function SceneEditorTree::onBeginReparenting( %this ) {
	if( isObject( %this.reparentUndoAction ) )
		%this.reparentUndoAction.delete();

	%action = UndoActionReparentObjects::create( %this );
	%this.reparentUndoAction = %action;
}

function SceneEditorTree::onReparent( %this, %obj, %oldParent, %newParent ) {
	%this.reparentUndoAction.add( %obj, %oldParent, %newParent );
}

function SceneEditorTree::onEndReparenting( %this ) {
	%action = %this.reparentUndoAction;
	%this.reparentUndoAction = "";

	if( %action.numObjects > 0 ) {
		if( %action.numObjects == 1 )
			%action.actionName = "Reparent Object";
		else
			%action.actionName = "Reparent Objects";

		%action.addToManager( Editor.getUndoManager() );
		EWorldEditor.syncGui();
	} else
		%action.delete();
}

function SceneEditorTree::update( %this ) {
	%this.buildVisibleTree( false );
}

//------------------------------------------------------------------------------

// Tooltip for TSStatic
function SceneEditorTree::GetTooltipTSStatic( %this, %obj ) {
	return "Shape: " @ %obj.shapeName;
}

// Tooltip for ShapeBase
function SceneEditorTree::GetTooltipShapeBase( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}

// Tooltip for StaticShape
function SceneEditorTree::GetTooltipStaticShape( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}

// Tooltip for Item
function SceneEditorTree::GetTooltipItem( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}

// Tooltip for RigidShape
function SceneEditorTree::GetTooltipRigidShape( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}

// Tooltip for Prefab
function SceneEditorTree::GetTooltipPrefab( %this, %obj ) {
	return "File: " @ %obj.filename;
}

// Tooltip for GroundCover
function SceneEditorTree::GetTooltipGroundCover( %this, %obj ) {
	%text = "Material: " @ %obj.material;

	for(%i=0; %i<8; %i++) {
		if(%obj.probability[%i] > 0 && %obj.shapeFilename[%i] !$= "") {
			%text = %text NL "Shape " @ %i @ ": " @ %obj.shapeFilename[%i];
		}
	}

	return %text;
}

// Tooltip for SFXEmitter
function SceneEditorTree::GetTooltipSFXEmitter( %this, %obj ) {
	if(%obj.fileName $= "")
		return "Track: " @ %obj.track;
	else
		return "File: " @ %obj.fileName;
}

// Tooltip for ParticleEmitterNode
function SceneEditorTree::GetTooltipParticleEmitterNode( %this, %obj ) {
	%text = "Datablock: " @ %obj.dataBlock;
	%text = %text NL "Emitter: " @ %obj.emitter;
	return %text;
}

// Tooltip for WorldEditorSelection
function SceneEditorTree::GetTooltipWorldEditorSelection( %this, %obj ) {
	%text = "Objects: " @ %obj.getCount();

	if( !%obj.getCanSave() )
		%text = %text NL "Persistent: No";
	else
		%text = %text NL "Persistent: Yes";

	return %text;
}

//------------------------------------------------------------------------------

function SceneEditorTreeTabBook::onTabSelected( %this ) {
	if( SceneEditorTreeTabBook.getSelectedPage() == 0) {
		SceneTreeWindow-->DeleteSelection.visible = true;
		SceneTreeWindow-->LockSelection.visible = true;
		SceneTreeWindow-->AddSimGroup.visible = true;
	} else {
		SceneTreeWindow-->DeleteSelection.visible = false;
		SceneTreeWindow-->LockSelection.visible = false;
		SceneTreeWindow-->AddSimGroup.visible = false;
	}
}







//------------------------------------------------------------------------------


//------------------------------------------------------------------------------



//------------------------------------------------------------------------------------
function CameraSpeedDropdownCtrlContainer::onWake(%this) {
	%this-->slider.setValue(CameraSpeedDropdownContainer-->textEdit.getText());
}

//------------------------------------------------------------------------------------
// Callbacks to close the dropdown slider controls like the camera speed,
// that are marked with this class name.

function EditorDropdownSliderContainer::onMouseDown(%this) {
	Canvas.popDialog(%this);
}

function EditorDropdownSliderContainer::onRightMouseDown(%this) {
	Canvas.popDialog(%this);
}