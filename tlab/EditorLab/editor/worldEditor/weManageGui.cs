//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Synchronize WorldEditor GUI parameters with current plugin
function EWorldEditor::syncGui( %this ) {
	if(!EditorGui.isAwake()) {
		return;
	}

	%this.syncToolPalette();
	SceneEditorTree.update();
	Editor.getUndoManager().updateUndoMenu( );
	EditorGuiStatusBar.setSelectionObjectsByCount( %this.getSelectionSize() );
	//SceneTreeWindow-->LockSelection.setStateOn( %this.getSelectionLockCount() > 0 );
	SceneEditorToolbar-->boundingBoxColBtn.setStateOn( EWorldEditor.boundingBoxCollision );

	if( EWorldEditor.objectsUseBoxCenter ) {
		SceneEditorToolbar-->centerObject.iconBitmap ="tlab/gui/icons/toolbar_assets/SelObjectCenter";
	} else {
		SceneEditorToolbar-->centerObject.iconBitmap ="tlab/gui/icons/toolbar_assets/SelObjectBounds";
	}

	if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
		SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/gui/icons/toolbar_assets/TransformObject";
	} else {
		SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/gui/icons/toolbar_assets/TransformWorld";
	}

	SceneEditorToolbar-->renderHandleBtn.setStateOn( EWorldEditor.renderObjHandle );
	SceneEditorToolbar-->renderTextBtn.setStateOn( EWorldEditor.renderObjText );
	SnapToBar-->objectSnapBtn.setStateOn( EWorldEditor.getSoftSnap() );
	//SceneEditorToolbar-->softSnapSizeTextEdit.setText( EWorldEditor.getSoftSnapSize() );
	ESnapOptions-->SnapSize.setText( EWorldEditor.getSoftSnapSize() );
	ESnapOptions-->GridSize.setText( EWorldEditor.getGridSize() );
	ESnapOptions-->GridSnapButton.setStateOn( %this.getGridSnap() );
	SnapToBar-->objectGridSnapBtn.setStateOn( %this.getGridSnap() );
	ESnapOptions-->NoSnapButton.setStateOn( !%this.stickToGround && !%this.getSoftSnap() && !%this.getGridSnap() );
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditor::syncToolPalette( %this ) {
	switch$ ( GlobalGizmoProfile.mode ) {
	case "None":
		EWorldEditorNoneModeBtn.performClick();

	case "Move":
		EWorldEditorMoveModeBtn.performClick();

	case "Rotate":
		EWorldEditorRotateModeBtn.performClick();

	case "Scale":
		EWorldEditorScaleModeBtn.performClick();
	}
}
//------------------------------------------------------------------------------

