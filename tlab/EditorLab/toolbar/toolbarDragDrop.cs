//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Toolbar drag controls callbacks
//==============================================================================
//==============================================================================
// Start dragging a toolbar icon
function ToolbarIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("PluginIcon class is corrupted! Fix it!");
		return;
	}

	startDragAndDropCtrl(%this,"Lab.onToolbarIconDroppedDefault","Toolbar");
	hide(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Start dragging a toolbar icons group
function ToolbarBoxChild::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ToolbarBoxChild class is corrupted! Fix it!");
		return;
	}

	%src = %this.parentGroup;
	startDragAndDropCtrl(%src,"Lab.onToolbarIconDroppedDefault","Toolbar");
	hide(%src);
}
//------------------------------------------------------------------------------

//==============================================================================
// Start dragging a toolbar icons group
function ToolbarGroup::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ToolbarGroup class is corrupted! Fix it!");
		return;
	}

	%group = %this.parentGroup.parentGroup;

	if (%group.getClassName() !$= "GuiStackControl")
		return;

	startDragAndDropCtrl(%group,"Lab.onToolbarIconDroppedDefault","Toolbar");
	hide(%group);
}
//------------------------------------------------------------------------------

//==============================================================================
// Toolbar Dropping a dragged item callbacks
//==============================================================================

//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function EToolbarIconTrash::onControlDropped(%this, %control, %dropPoint) {
	%droppedCtrl = %control.dragSourceControl;

	if (%control.dropType !$= "Toolbar") {
		warnLog("Toolbar thrash dropped invalid droptype ctrl:",%control);
		show(%droppedCtrl);
		return;
	}
	if(isObject(%droppedCtrl.srcCtrl))
		return;
	if (%droppedCtrl.getClassName() $= "GuiStackControl") {
		show(%droppedCtrl);

		for(%i = %droppedCtrl.getCount()-1; %i >= 0; %i--) {
			%obj = %droppedCtrl.getObject(%i);

			if (%obj.internalName $= "GroupDivider")
				continue;

			%this.add(%obj);
		}

		%droppedCtrl.delete();
		return;
	}
	
	
	Lab.setToolbarIconDisabled(%droppedCtrl,true);
	%clone = %droppedCtrl.deepClone();
	%clone.srcCtrl = %droppedCtrl;
	
	show(%clone);
	%this.add(%clone);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over a toolbar icon
function ToolbarIcon::onControlDropped(%this, %control, %dropPoint) {

	%droppedCtrl = %control.dragSourceControl;

	if (%control.dropType !$= "Toolbar") {
		warnLog("Toolbar thrash dropped invalid droptype ctrl:",%control);
		show(%droppedCtrl);
		return;
	}

	if(isObject(%droppedCtrl.srcCtrl))
	{
	
		%clone = %droppedCtrl;
		%droppedCtrl = %droppedCtrl.srcCtrl;
		delObj(%clone);
	}
	%addToThis = %this.parentGroup;
	%addBefore = %this;
	show(%droppedCtrl);
	
	if (%droppedCtrl.getClassName() $= "GuiStackControl") {
		%stackAndChild = Lab.findObjectToolbarStack(%this,true);
		%addToThis = getWord(%stackAndChild,0);
		%addBefore =  getWord(%stackAndChild,1);
	} else if (%droppedCtrl.isNewGroup) {
		show(%droppedCtrl);
		%newGroup = Lab.createNewToolbarIconGroup();
		%droppedCtrl = %newGroup;
		%stackAndChild = Lab.findObjectToolbarStack(%this,true);
		%addToThis = getWord(%stackAndChild,0);
		%addBefore =  getWord(%stackAndChild,1);
	}

	if (!isObject( %addToThis )) {
		warnLog("Something went wrong, can't find a valid stack for the dropped on icon",%this);
		return;
	}

	Lab.addToolbarItemToGroup(%droppedCtrl,%addToThis,%addBefore);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over a toolbar icon
function PluginToolbarEnd::onControlDropped(%this, %control, %dropPoint) {
	%droppedCtrl = %control.dragSourceControl;

	if (%control.dropType !$= "Toolbar") {
		warnLog("Toolbar thrash dropped invalid droptype ctrl:",%control);
		show(%droppedCtrl);
		return;
	}
	if(isObject(%droppedCtrl.srcCtrl))
	{
		
		%clone = %droppedCtrl;
		%droppedCtrl = %droppedCtrl.srcCtrl;
		delObj(%clone);
	}
	show(%droppedCtrl);
	%stackAndChild = Lab.findObjectToolbarStack(%this,true);
	%addToThis = getWord(%stackAndChild,0);
	%addBefore =  getWord(%stackAndChild,1);

	if (%droppedCtrl.isNewGroup ) {
		%newGroup = Lab.createNewToolbarIconGroup();
		%droppedCtrl = %newGroup;
	}

	Lab.addToolbarItemToGroup(%droppedCtrl,%addToThis,%addBefore);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::onToolbarIconDroppedDefault(%this,%dropOnCtrl, %draggedControl, %dropPoint) {
	%droppedCtrl = %draggedControl.dragSourceControl;

	if (%draggedControl.dropType !$= "Toolbar") {
		warnLog("Toolbar thrash dropped invalid droptype ctrl:",%control);
		show(%droppedCtrl);
		return;
	}

	show(%droppedCtrl);
	%addBefore = %dropOnCtrl;

	if (%droppedCtrl.isNewGroup || %droppedCtrl.getClassName() $= "GuiStackControl") {
		%stackAndChild = Lab.findObjectToolbarStack(%dropOnCtrl,true);
		%addToThis = getWord(%stackAndChild,0);
		%addBefore =  getWord(%stackAndChild,1);
	} else
		%addToThis = Lab.findObjectToolbarStack(%dropOnCtrl,%lookForDefaultStack);

	if (!isObject(%addToThis))
		return;

	if (%droppedCtrl.isNewGroup) {
		%newGroup = Lab.createNewToolbarIconGroup();
		%droppedCtrl = %newGroup;
	}

	%this.addToolbarItemToGroup(%droppedCtrl,%addToThis,%addBefore);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::findObjectToolbarStack(%this,%checkObj,%defaultPluginStack) {
	%checkId = 0;

	while(%checkId < 5) {
		%result = %this.checkObjForToolbarStack(%checkObj,%defaultPluginStack);

		if (%result && %defaultPluginStack)
			return %checkObj SPC %previousObj;
		else if (%result)
			return %checkObj;

		%previousObj = %checkObj;
		%checkObj = %checkObj.parentGroup;
		%checkId++;
	}

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::checkObjForToolbarStack(%this,%object,%defaultStack) {
	if (!isObject(%object))
		return false;

	if (%object.getClassname() $= "GuiStackControl") {
		if (%defaultStack && %object.superClass $= "ToolbarPluginStack")
			return true;
		else if (!%defaultStack && %object.superClass $= "ToolbarContainer")
			return true;
		else if (!%defaultStack && %object.superClass $= "PluginToolbarEnd")
			return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::addToolbarItemToGroup(%this,%item,%group,%addBefore) {
	%divider = %group-->GroupDivider;

	if (isObject(%divider)) {
		%divider.extent.x = "6";

		foreach(%obj in %divider) {
			if (%obj.getClassName() $= "GuiMouseEventCtrl")
				%obj.extent.x = "6";
		}
	}
	%this.setToolbarIconDisabled(%item,false);
	%group.add(%item);
	%group.updateStack();
	%group.reorderChild(%item,%addBefore);
}
//------------------------------------------------------------------------------
//==============================================================================
// Create a new Icon Group to be dragged in Toolbar
//==============================================================================
//==============================================================================
// Start dragging a new icons group to be added to toolbar
function NewToolbarGroup::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ToolbarGroup class is corrupted! Fix it!");
		return;
	}

	%group = %this.parentGroup;
	%group.isNewGroup = true;
	startDragAndDropCtrl(%group,"Lab.onToolbarIconDroppedDefault","Toolbar");
}
//------------------------------------------------------------------------------

//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::createNewToolbarIconGroup(%this) {
	%newGroup = new GuiStackControl() {
		stackingType = "Horizontal";
		horizStacking = "Left to Right";
		vertStacking = "Top to Bottom";
		padding = "2";
		dynamicSize = "1";
		dynamicNonStackExtent = "0";
		dynamicPos = "0";
		changeChildSizeToFit = "1";
		changeChildPosition = "1";
		position = "472 0";
		extent = "68 30";
		minExtent = "4 16";
		profile = "ToolsPanelDarkB";
		visible = "1";
		active = "1";
		internalName = "ToolbarGroup";
		superClass = "ToolbarContainer";
		new GuiContainer() {
			position = "0 0";
			extent = "16 30";
			minExtent = "4 2";
			profile = "ToolsPanelDarkB";
			visible = "1";
			active = "1";
			tooltipProfile = "GuiToolTipProfile";
			internalName = "GroupDivider";
			new GuiBitmapCtrl() {
				bitmap = "tlab/gui/icons/toolbar-assets/GroupDivider.png";
				wrap = "0";
				position = "1 6";
				extent = "4 18";
				minExtent = "4 2";
				profile = "GuiDefaultProfile";
				visible = "1";
				active = "1";
			};
			new GuiMouseEventCtrl() {
				extent = "16 30";
				minExtent = "8 2";
				visible = "1";
				active = "1";
				superClass = "ToolbarGroup";
			};
		};
	};
	return %newGroup;
}