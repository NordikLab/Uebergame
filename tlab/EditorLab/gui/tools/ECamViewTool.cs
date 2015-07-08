//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ESnapOptions_Initialized = false;
$ECamViewTool_ViewMode["top"] = "Top View";
$ECamViewTool_ViewMode["bottom"] = "Bottom View";
$ECamViewTool_ViewMode["left"] = "Left View";
$ECamViewTool_ViewMode["right"] = "Right View";
$ECamViewTool_ViewMode["front"] = "Front View";
$ECamViewTool_ViewMode["back"] = "Back View";
$ECamViewTool_ViewMode["free"] = "Standard Camera";

//==============================================================================
function ECamViewTool::onWake(%this) {
}
//------------------------------------------------------------------------------
//==============================================================================
function ECamViewGui::onShow(%this) {
	EditorGuiStatusBar-->camViewBoxState.setStateOn(true);
	if (!isObject(ECamViewGui.currentGui))
		ECamViewGui.currentGui = ECamViewCompact;
	
	ECamViewBox.visible = 0;
	ECamViewCompact.visible = 0;
	ECamViewGui.currentGui.visible = 1;
	$Lab_CamViewEnabled = true;
	
}
//==============================================================================
function ECamViewGui::onHide(%this) {
	EditorGuiStatusBar-->camViewBoxState.setStateOn(false);
	
	ECamViewBox.visible = 0;
	ECamViewCompact.visible = 0;
	$Lab_CamViewEnabled = false;
}
//==============================================================================
function ECamViewGui::initTool(%this) {
	%this.setCompactMode(true);	
}
//==============================================================================
function ECamViewTool::updateGui( %this ) {
	
	%this-->stack.updateStack();
}
//------------------------------------------------------------------------------
//==============================================================================
function ECamViewGui::setState(%this,%isActive) {
	ECamViewBox.visible = 0;
	ECamViewCompact.visible = 0;
	if (%isActive && isObject(ECamViewGui.currentGui))	{
		ECamViewGui.currentGui.visible = 1;
		ETools.showTool("CamView");
	}
	else {
		ETools.hideTool("CamView");
	}	
}
//==============================================================================
function ECamViewGui::setCompactMode(%this,%isCompact) {
	if (%isCompact)
		ECamViewGui.currentGui = ECamViewCompact;
	else 
		ECamViewGui.currentGui = ECamViewBox;
		
	ECamViewBox.visible = !%isCompact;
	ECamViewCompact.visible = %isCompact;
	
}
//==============================================================================
function ECamViewToolButton::onClick( %this ) {
	%view = %this.internalName;
	%viewType = $ECamViewTool_ViewMode[%view];
	Lab.setCameraViewType(%viewType);
	ECamViewGui.currentGui.updateCurrentView();
}
//------------------------------------------------------------------------------
function ECamViewGui::updateCurrentView(%this) {
	if (isObject(ECamViewGui.currentGui))	{
		ECamViewGui.currentGui.updateCurrentView();
	}	
	
}
//==============================================================================
function ECamViewTool::updateCurrentView(%this) {
	%curCamId = Lab.cameraDisplayType;
	%text = getWord($LabCameraDisplayName[%curCamId],0);	

	if (%text $= "Standard")
		%text = "3D";

	%this-->modeText.text = %text;
}
//------------------------------------------------------------------------------

//==============================================================================
function ECamViewToolClose::onClick( %this,%a1,%a2,%a3 ) {
	%parent = %this.parentGroup.parentGroup;
	hide(%parent);
	$Lab_CamViewEnabled = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ECamViewToolDrag::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%dragCtrl = %this.internalName;
	if (!isObject(%dragCtrl))
	{
		warnLog("Can't find cam box to drag!",%dragCtrl);
		return;
	}
	dragAndDropCtrl(%dragCtrl);
	hide(%dragCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function ECamViewTool::DragSuccess( %this,%droppedCtrl,%pos) {
	show(%this);
	%realpos = EditorFrameWorld.getRealPosition();	
	%pos.x = %pos.x - %this.extent.x/2- %realpos.x;
	%pos.y = %pos.y - %this.extent.y/2 - %realpos.y;
	
	%this.setPosition(%pos.x,%pos.y);
	%this.forceInsideCtrl(ECamViewGui);
	%this.refresh();
	%this.updateGui();
	
}
//==============================================================================
/*
function ECamViewToolDrag::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%parent = %this.parentGroup;
	%this.viewCtrl.superClass = "";
	%this.viewCtrl.class = "";
	%this.viewCtrl.superClass = "EToolCamCtrl";
	dragAndDropCtrl(%this.viewCtrl);
	hide(%parent);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolCamCtrl::DragSuccess( %this,%droppedCtrl,%pos) {
	show(%this);
	%realpos = EditorFrameWorld.getRealPosition();
	%pos.y -= %realPos.y + %this.extent.y/2;
	%pos.x -= %realPos.x + %this.extent.x/2;
	%this.position = %pos;
	%this.forceInsideCtrl(EditorFrameWorld);
}
//------------------------------------------------------------------------------
function EToolCamCtrl::DragSuccess2( %this,%droppedCtrl,%pos) {
	show(%this);
	%newPos = %pos;
	%realpos = %droppedCtrl.getRealPosition();
	%newPos.y -= %realPos.y + %this.extent.y/2;
	%newPos.x -= %this.extent.x/2;
	%this.position = %newPos;
	devLog("Dropped at:",%newPos,"Original",%pos);
	// %this.forceInsideCtrl(EditorFrameWorld);
}
//==============================================================================
function EToolCamCtrl::DragFailed( %this,%droppedCtrl,%pos) {
	devLog("Drag Failed Dropped in:",%droppedCtrl,"at position:",%pos);
}
//------------------------------------------------------------------------------
*/