//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ETools::initTools(%this) {
	foreach(%gui in %this){
		if (%gui.isMethod("initTool"))
			%gui.initTool();
	}
	ETransformBoxGui.initTool();
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::toggleTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	%this.fitIntoParents();
	
	ETools.visible = true;

	if (%dlg.visible) {
		%this.hideTool(%tool);
		//%dlg.setVisible(false);
		//%position = getRealCursorPos();
		//%dlg.position = %position;
		//%dlg.position.x -= %dlg.extent.x/2;
		//%dlg.position.y -= EditorGuiMenubar.extent.y;
	} else {
		%this.showTool(%tool);
		//%dlg.setVisible(true);
	}
	return;
	%hideMe = true;
	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;
	if (%hideMe)
		%this.visible = 0;
	
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::showTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	%this.fitIntoParents();
	
	ETools.visible = true;


	%dlg.setVisible(true);
		
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
		
	if (%dlg.isMethod("onShow"))
		%dlg.onShow();
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::hideTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	



	%dlg.setVisible(false);
	
	%hideMe = true;
	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;
			
	if (%hideMe)
		%this.visible = 0;
		
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
		
	if (%dlg.isMethod("onHide"))
		%dlg.onHide();
}
//------------------------------------------------------------------------------