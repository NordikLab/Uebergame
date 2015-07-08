//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
$SEP_AutoGroupActiveSystem = true;
//==============================================================================
function SceneCreatorWindow::setActiveSimGroup( %this, %group ) {
	if (!isObject(%group))
		return;

	if (!%group.isMemberOfClass("SimGroup"))
		return;

	%this.activeSimGroup = %group;	
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::getActiveSimGroup( %this) {
	if (!isObject(%this.activeSimGroup))
		%this.setActiveSimGroup(MissionGroup);
	else if (!%this.activeSimGroup.isMemberOfClass(SimGroup))
		%this.setActiveSimGroup(MissionGroup);

	return %this.activeSimGroup;
}
//------------------------------------------------------------------------------

function SceneCreatorWindow::registerMissionObject( %this, %class, %name, %buildfunc, %group ) {
	if( !isClass(%class) )
		return;

	if ( %name $= "" )
		%name = %class;

	if ( %this.currentGroup !$= "" && %group $= "" )
		%group = %this.currentGroup;

	if ( %class $= "" || %group $= "" ) {
		warn( "SceneCreatorWindow::registerMissionObject, invalid parameters!" );
		return;
	}

	%args = new ScriptObject();
	%args.val[0] = %class;
	%args.val[1] = %name;
	%args.val[2] = %buildfunc;
	%this.array.push_back( %group, %args );
}

function SceneCreatorWindow::getNewObjectGroup( %this ) {
	return %this.objectGroup;
}

function SceneCreatorWindow::setNewObjectGroup( %this, %group ) {
	if( %this.objectGroup ) {
		%oldItemId = SceneEditorTree.findItemByObjectId( %this.objectGroup );

		if( %oldItemId > 0 )
			SceneEditorTree.markItem( %oldItemId, false );
	}

	if (!isObject(%group))return;

	%group = %group.getID();
	%this.objectGroup = %group;
	%itemId = SceneEditorTree.findItemByObjectId( %group );
	SceneEditorTree.markItem( %itemId );
}
function SceneCreatorWindow::onObjectCreated( %this, %objId ) {
	// Can we submit an undo action?
	if ( isObject( %objId ) )
		MECreateUndoAction::submit( %objId );

	SceneEditorTree.clearSelection();
	EWorldEditor.clearSelection();
	EWorldEditor.selectObject( %objId );
	// When we drop the selection don't store undo
	// state for it... the creation deals with it.
	EWorldEditor.dropSelection( true );
}

function SceneCreatorWindow::onFinishCreateObject( %this, %objId ) {
	%this.objectGroup.add( %objId );

	if( %objId.isMemberOfClass( "SceneObject" ) ) {
		%objId.position = %this.getCreateObjectPosition();
		//flush new position
		%objId.setTransform( %objId.getTransform() );
	}

	%this.onObjectCreated( %objId );
}
//SceneCreatorWindow.createSimGroup();
function SceneCreatorWindow::createSimGroup( %this ) {
	if ( !$missionRunning )
		return;

	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	
	%addToGroup = %this.getActiveSimGroup();
	%objId = new SimGroup() {
		internalName = getUniqueInternalName( %this.objectGroup.internalName, MissionGroup, true );
		position = %this.getCreateObjectPosition();
		parentGroup = %addToGroup;
	};
}


function SceneCreatorWindow::createStatic( %this, %file ) {
	if ( !$missionRunning )
		return;

	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	
	%addToGroup = %this.getActiveSimGroup();
	%objId = new TSStatic() {
		shapeName = %file;
		position = %this.getCreateObjectPosition();
		parentGroup = %addToGroup;
	};
	%this.onObjectCreated( %objId );
}

function SceneCreatorWindow::createPrefab( %this, %file ) {
	if ( !$missionRunning )
		return;

	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	if( !isObject(%this.objectGroup) )
		%this.setNewObjectGroup( MissionGroup );

	%objId = new Prefab() {
		filename = %file;
		position = %this.getCreateObjectPosition();
		parentGroup = %this.objectGroup;
	};
	%this.onObjectCreated( %objId );
}

function SceneCreatorWindow::createObject( %this, %cmd ) {
	if ( !$missionRunning )
		return;

	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	if( !isObject(%this.objectGroup) )
		%this.setNewObjectGroup( MissionGroup );

	pushInstantGroup();
	%objId = eval(%cmd);
	popInstantGroup();

	if( isObject( %objId ) )
		%this.onFinishCreateObject( %objId );

	return %objId;
}

