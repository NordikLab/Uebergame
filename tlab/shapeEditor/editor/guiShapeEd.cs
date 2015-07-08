//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================


function ShapeEditorPlugin::toggleAdvancedOptions(%this) {
	ShapeEditorDialogs.toggleDlg("Advanced",true);
	ShapeEditorToolbar-->showAdvanced.setStateOn(ShapeEditorDialogs-->Advanced.isVisible());
}

function ShapeEditorPlugin::initStatusBar(%this) {
	EditorGuiStatusBar.setInfo("Shape editor ( Shift Click ) to speed up camera.");
	EditorGuiStatusBar.setSelection( ShapeEditor.shape.baseShape );
}