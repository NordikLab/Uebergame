//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
function ShapeEdAnimWindow::onShow( %this ) {	
	ShapeEdAnimWindow.fitIntoParents("width");
	ShapeEdAnimWindow.AlignCtrlToParent("bottom");
}


function ShapeEditorPlugin::toggleAnimBar(%this) {
	ShapeEditorDialogs.toggleDlg("AnimBar",true);
	ShapeEditorToolbar-->showAnimBar.setStateOn(ShapeEditorDialogs-->AnimBar.isVisible());
}
//ShapeEditorPlugin.updateAnimBar();
function ShapeEditorPlugin::updateAnimBar(%this) {	
	%stateOn = ShapeEdAnimWindow.isVisible();
	
	//FIXME Hack : hide and show to fix container rendering issue of unknown cause
	hide(ShapeEdAnimWindow);
	if (%stateOn)
		show(ShapeEdAnimWindow);	
		
	ShapeEditorToolbar-->showAnimBar.setStateOn(%stateOn);
}