//==============================================================================
// TorqueLab -> ShapeEditor -> Main Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// @todo:
//
// - split node transform editboxes into X Y Z and rot X Y Z with spin controls
//   to allow easier manual editing
// - add groundspeed editing ( use same format as node transform editing )
//
// Known bugs/limitations:
//
// - resizing the GuiTextListCtrl should resize the columns as well
// - modifying the from/in/out properties of a sequence will change the sequence
//   order in the shape ( since it results in remove/add sequence commands )
// - deleting a node should not delete its children as well?
//
//==============================================================================

//==============================================================================
// ShapeEditor -> First Initialization
//==============================================================================


//==============================================================================
// Dirty state and saving
//==============================================================================
function ShapeEditor::isDirty( %this ) {
	return ( isObject( %this.shape ) && ShapeEdPropWindow-->saveBtn.isActive() );
}

function ShapeEditor::setDirty( %this, %dirty ) {
	if ( %dirty )
		ShapeEdSelectWindow.text = "Shapes *";
	else
		ShapeEdSelectWindow.text = "Shapes";

	ShapeEdPropWindow-->saveBtn.setActive( %dirty );
}

function ShapeEditor::saveChanges( %this ) {
	if ( isObject( ShapeEditor.shape ) ) {
		ShapeEditor.saveConstructor( ShapeEditor.shape );
		ShapeEditor.shape.writeChangeSet();
		ShapeEditor.shape.notifyShapeChanged();      // Force game objects to reload shape
		ShapeEditor.setDirty( false );
	}
}




//------------------------------------------------------------------------------

function ShapeEdSeqNodeTabBook::onTabSelected( %this, %name, %index ) {
	%this.activePage = %name;

	switch$ ( %name ) {
	case "Seq":
		ShapeEdPropWindow-->newBtn.ToolTip = "Add new sequence";
		ShapeEdPropWindow-->newBtn.Command = "ShapeEdSequences.onAddSequence();";
		ShapeEdPropWindow-->newBtn.setActive( true );
		ShapeEdPropWindow-->deleteBtn.ToolTip = "Delete selected sequence (cannot be undone)";
		ShapeEdPropWindow-->deleteBtn.Command = "ShapeEdSequences.onDeleteSequence();";
		ShapeEdPropWindow-->deleteBtn.setActive( true );

	case "Node":
		ShapeEdPropWindow-->newBtn.ToolTip = "Add new node";
		ShapeEdPropWindow-->newBtn.Command = "ShapeEdNodes.onAddNode();";
		ShapeEdPropWindow-->newBtn.setActive( true );
		ShapeEdPropWindow-->deleteBtn.ToolTip = "Delete selected node (cannot be undone)";
		ShapeEdPropWindow-->deleteBtn.Command = "ShapeEdNodes.onDeleteNode();";
		ShapeEdPropWindow-->deleteBtn.setActive( true );

	case "Detail":
		ShapeEdPropWindow-->newBtn.ToolTip = "";
		ShapeEdPropWindow-->newBtn.Command = "";
		ShapeEdPropWindow-->newBtn.setActive( false );
		ShapeEdPropWindow-->deleteBtn.ToolTip = "Delete the selected mesh or detail level (cannot be undone)";
		ShapeEdPropWindow-->deleteBtn.Command = "ShapeEdDetails.onDeleteMesh();";
		ShapeEdPropWindow-->deleteBtn.setActive( true );

	case "Mat":
		ShapeEdPropWindow-->newBtn.ToolTip = "";
		ShapeEdPropWindow-->newBtn.Command = "";
		ShapeEdPropWindow-->newBtn.setActive( false );
		ShapeEdPropWindow-->deleteBtn.ToolTip = "";
		ShapeEdPropWindow-->deleteBtn.Command = "";
		ShapeEdPropWindow-->deleteBtn.setActive( false );
		// For some reason, the header is not resized correctly until the Materials tab has been
		// displayed at least once, so resize it here too
		ShapeEdMaterials-->materialListHeader.setExtent( getWord( ShapeEdMaterialList.extent, 0 ) SPC "19" );
	}
}









//------------------------------------------------------------------------------
// Shape Preview
//------------------------------------------------------------------------------

function ShapeEdPreviewGui::updatePreviewBackground( %color ) {
	ShapeEdPreviewGui-->previewBackground.color = %color;
	ShapeEditorToolbar-->previewBackgroundPicker.color = %color;
}

function showShapeEditorPreview() {
	%visible = ShapeEditorToolbar-->showPreview.getValue();
	ShapeEdPreviewGui.setVisible( %visible );
}
