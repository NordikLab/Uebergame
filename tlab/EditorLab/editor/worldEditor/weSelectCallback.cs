//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function WorldEditor::onDragCopy( %this, %obj ) {
	logd("onDragCopy",%this.getSelectionSize());
	%obj.startDrag = %obj.getPosition();
	%this.lastCentroidPos = %this.getSelectionCentroid();
	$DragCopyStarted = true;
}
//------------------------------------------------------------------------------

//==============================================================================
function WorldEditor::onEndDrag( %this, %obj ) {
	logd("WorldEditor::onEndDrag( %this, %obj )",$Button0Pressed);	
	
	SceneInspector.inspect( %obj );
	SceneInspector.apply();

	if (%this.forceToGrid)
		%this.forceToGrid(%obj);
		
	if (!$DragCopyStarted)
		return;
	if (%this.lastMoveOffset !$="" && Lab.CloneDragEnabled && GlobalGizmoProfile.mode $= "move"){			
		ECloneDrag.copyOffset = %this.lastMoveOffset;
		ETools.showTool(CloneDrag);	
	}
	$DragCopyStarted = false;	
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onSelectionCentroidChanged( %this ) {

	if (%this.lastCentroidPos !$=""){	
		%offset = VectorSub(%this.getSelectionCentroid(),%this.lastCentroidPos);
		%this.lastMoveOffset = %offset;
		
	}
	%this.lastCentroidPos = %this.getSelectionCentroid();
	
	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());
	// Refresh inspector.
	SceneInspector.refresh();
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onSelect( %this, %obj,%scriptSide ) {

	//Check to tell that the selection is called from a group
	%obj.byGroup = false;
	SceneEditorTree.addSelection( %obj );
	//Store the source Location of Object 0 in case we drag copy
	_setShadowVizLight( %obj );
	//SceneInspector.inspect( %obj );

	if ( isObject( %obj ) && %obj.isMethod( "onEditorSelect" ) )
		%obj.onEditorSelect( %this.getSelectionSize() );

	Lab.currentEditor.onObjectSelected( %obj );
	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());
	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());
	// Update the materialEditorList
	$Lab::materialEditorList = %obj.getId();
	// Used to help the Material Editor( the M.E doesn't utilize its own TS control )
	// so this dirty extension is used to fake it
	if ( MaterialEditorPreviewWindow.isVisible() )
		MaterialEditorGui.prepareActiveObject();
	
	Lab.DoSelectionCallback("Transform",%this.getSelectedObject(0));
	
}
//------------------------------------------------------------------------------

//==============================================================================
function WorldEditor::onMultiSelect( %this, %set ) {
	// This is called when completing a drag selection ( on3DMouseUp )
	// so we can avoid calling onSelect for every object. We can only
	// do most of this stuff, like inspecting, on one object at a time anyway.
	%count = %set.getCount();
	%i = 0;

	foreach( %obj in %set ) {
		if ( %obj.isMethod( "onEditorSelect" ) )
			%obj.onEditorSelect( %count );

		%i ++;
		SceneEditorTree.addSelection( %obj, %i == %count );
		Lab.currentEditor.onObjectSelected( %obj );
	}

	// Inform the camera
	commandToServer( 'EditorOrbitCameraSelectChange', %count, %this.getSelectionCentroid() );
	EditorGuiStatusBar.setSelectionObjectsByCount( EWorldEditor.getSelectionSize() );	
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onUnSelect( %this, %obj ) {
	if ( isObject( %obj ) && %obj.isMethod( "onEditorUnselect" ) )
		%obj.onEditorUnselect();

	%this.lastCentroidPos = "";
	%this.lastMoveOffset = "";

	Lab.currentEditor.onObjectDeselected( %obj );
	SceneInspector.removeInspect( %obj );
	SceneEditorTree.removeSelection(%obj);
	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());
	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());
	// Update the Transform Selection window
	//ETransformSelection.onSelectionChanged();
	Lab.DoSelectionCallback("Transform");
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onClearSelection( %this ) {	
	Lab.currentEditor.onSelectionCleared();
	SceneEditorTree.clearSelection();
	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());
	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());
	// Call the selection callbacks with no objects
	Lab.DoSelectionCallback("Transform");

}
//------------------------------------------------------------------------------




