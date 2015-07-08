//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$EOverlay = newScriptObject("EOverlay");
//==============================================================================
$Lab_ToolDecoyMargin = "20";
$Lab_ToolDecoyMode[1] = "Decoy";
$Lab_ToolDecoyMode[2] = "Mouse";
//==============================================================================
function EToolOverlayGui::onWake() {
}
//------------------------------------------------------------------------------

//==============================================================================
// EOverlay Toggle/Show/Hide Dialog Helpers
//==============================================================================

//==============================================================================
function EOverlay::toggleDlg(%this,%dlg,%decoyMode) {
	EToolOverlayGui.toggleTool(%dlg,%decoyMode);
}
//------------------------------------------------------------------------------
//==============================================================================
function EOverlay::showDlg(%this,%dlg,%decoyMode) {
	EToolOverlayGui.showTool(%dlg,%decoyMode);
}
//------------------------------------------------------------------------------
//==============================================================================
function EOverlay::hideDlg(%this,%dlg,%decoyMode) {
	EToolOverlayGui.hideTool(%dlg,%decoyMode);
}
//------------------------------------------------------------------------------

//==============================================================================
// EOverlay ToggleSlider (Bring a slider at cursor pos which close on decoys)
//==============================================================================

//==============================================================================
function EOverlay::toggleSlider(%this,%decoyMode,%topCenterPos,%options,%value) {
	

	%slider = EToolOverlay-->SliderMouseDlg;
	%slider.ticks = "";
	%slider.snap = false;
	if (%options !$= "") {
		for(%i=0; %i<getRecordCount(%options); %i++) {
			%record = getRecord(%options,%i);
			%field = getField(%record,0);
			%value = getField(%record,1);
			eval("%slider."@trim(%field)@" = \""@trim(%value)@"\";");
			%slider.setFieldValue(%field,%value);
		}
	}
	if (%slider.ticks > 0)
		%slider.snap = true;
%slider.setValue(%value);
	if (%topCenterPos !$= "")
		%slider.topCenterPos = %topCenterPos;
	else
		%slider.topCenterPos = "";

	EToolOverlayGui.toggleTool("SliderMouseDlg","2");
}
//------------------------------------------------------------------------------
//==============================================================================
// EToolOverlayGui Toggle/Show/Hide Tools functions
//==============================================================================

//==============================================================================
function EToolOverlayGui::toggleTool(%this,%tool,%decoyMode) {
	
	%this.pushToBack(%this-->decoyCtrl);
	%dlg = EToolOverlay.findObjectByInternalName(%tool,true);

	if (%dlg.decoyMode $= "") %dlg.decoyMode = "0";

	if (!EToolOverlay.isMember(%dlg) && %dlg.decoyMode !$="")
		EToolOverlay.add(%dlg);

	if (%dlg.visible)
		%this.hideTool(%tool);
	else
		%this.showTool(%tool,%decoyMode);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayGui::hideTool(%this,%tool,%decoyMode) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	%decoy = %this-->DecoyCtrl;
	%mouse = %this-->MouseEvent;

	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(false);

	hide(%dlg);
	hide(%decoy);
	hide(%mouse);
	hide(%dlg.parentGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayGui::showTool(%this,%tool) {
	%dlg = EToolOverlay.findObjectByInternalName(%tool,true);

	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(true);

	foreach(%obj in EToolOverlay) {
		if (%obj.decoyMode $= "") %obj.decoyMode = "0";		
		hide(%obj);		
	}

	%this.fitIntoParents();
	
	EToolOverlay.fitIntoParents();

	if (%dlg.topCenterPos !$= "") {
		%dlg.position = %dlg.topCenterPos;
		%dlg.position.x -= %dlg.extent.x/2;
	} else {
		%position = getRealCursorPos();
		%dlg.position = %position;
		%dlg.position.x -= %dlg.extent.x/2;
		%dlg.position.y -= EditorGuiMenubar.extent.y;
	}

	show(%dlg.parentGroup);
	show(%dlg);
	show(%this);

	if (%dlg.decoyMode $= "1") {
		%this.setDecoy(%dlg);
	} else if (%dlg.decoyMode $= "2") {
		%dlg.parentGroup = EToolOverlay;
		%mouse = %this-->MouseEvent;
		%mouse.fitIntoParents();
		%mouse.tool = %tool;
		show(%mouse);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayGui::setDecoy(%this,%dlg) {
		%decoy = %this-->DecoyCtrl;	
	%decoy.extent.x = %dlg.extent.x + ($Lab_ToolDecoyMargin *2);
	%decoy.extent.y = %dlg.extent.y + ($Lab_ToolDecoyMargin *2);
	%decoy.position.x = %dlg.position.x - $Lab_ToolDecoyMargin;
	%decoy.position.y = %dlg.position.y - $Lab_ToolDecoyMargin;
	%decoy.tool = %dlg.internalName;
	show(%decoy);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayGui::onHideEvent(%this,%tool) {
	%dlg = EToolOverlay.findObjectByInternalName(%tool);

	if (isObject(EToolOverlayGui.linkObject)) {
		%obj = EToolOverlayGui.linkObject;
		%command = strReplace(%obj.command,"$ThisControl",%obj.getId());
		devLog("Eval:",%command);
		eval(%command);
	}

	hide(%this-->MouseEvent);
	hide(%this-->DecoyCtrl);

	if (%dlg.decoyMode $="0" || !isObject(%dlg))
		return;

	hide(%dlg);
	hide(%this);
	hide(EToolOverlay);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayMouse::onMouseDown (%this, %modifier, %mousePoint,%mouseClickCount) {
	EToolOverlayGui.onHideEvent(%this.tool);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayDecoy::onMouseLeave(%this, %modifier, %mousePoint,%mouseClickCount) {
	//devLog("EToolDecoy::onMouseLeave(%this, %modifier, %mousePoint,%mouseClickCount)",%this, %modifier, %mousePoint,%mouseClickCount);
	EToolOverlayGui.onHideEvent(%this.tool);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayMouse::onRightMouseDown (%this, %modifier, %mousePoint,%mouseClickCount) {
	EToolOverlayGui.onHideEvent(%this.tool);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolOverlayMouse::onMouseEnter(%this, %modifier, %mousePoint,%mouseClickCount) {
	// devLog("EToolDecoy::onMouseEnter(%this, %modifier, %mousePoint,%mouseClickCount)",%this, %modifier, %mousePoint,%mouseClickCount);
}
//------------------------------------------------------------------------------