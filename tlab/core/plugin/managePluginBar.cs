//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::updatePluginsBar(%this,%reset) {
	if (%reset)
		%this.resetPluginsBar();

	foreach(%pluginObj in LabPluginGroup) {
		//if (%pluginObj.isHidden) continue;
		Lab.addPluginToBar( %pluginObj );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::updatePluginBarData(%this) {
	%count = ToolsToolbarArray.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%icon = ToolsToolbarArray.getObject(%i);
		%icon.pluginObj.pluginOrder = %i+1;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::resetPluginsBar(%this) {
	ToolsToolbarArray.deleteAllObjects();
	Lab.clearDisabledPluginsBin();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::sortPluginsBar(%this) {
	ToolsToolbarArray.sort("sortPluginByOrder");
	ToolsToolbarArray.refresh();
}
//------------------------------------------------------------------------------
//==============================================================================
//function that takes two object arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal.
function sortPluginByOrder(%objA,%objB) {
	if ( %objA.pluginObj.pluginOrder > %objB.pluginObj.pluginOrder)
		return "1";

	if ( %objA.pluginObj.pluginOrder < %objB.pluginObj.pluginOrder)
		return "-1";

	return "0";
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addPluginToBar( %this, %pluginObj ) {
	if (%pluginObj.isHidden)
		return;

	//First check if the Icon object is in on control
	%enabled =  %pluginObj.isEnabled;
	%containerEnabled = ToolsToolbarArray;
	%containerDisabled = EditorGui-->DisabledPluginsBox;
	%toolArrayEnabled = %containerEnabled.findObjectByInternalName(%pluginObj.plugin);
	%toolArrayDisabled = %containerDisabled.findObjectByInternalName(%pluginObj.plugin);

	if (isObject(%toolArrayEnabled)) {
		if (%enabled)
			%alreadyExists = true;
		else
			delObj(%toolArrayEnabled);
	}

	if (isObject(%toolArrayDisabled)) {
		if (!%enabled)
			%alreadyExists = true;
		else
			delObj(%toolArrayDisabled);
	}

	//If the Plugin Icon already exist, exit now
	if(%alreadyExists)
		return;

	%icon = %this.createPluginIcon(%pluginObj);

	if (%enabled)
		ToolsToolbarArray.add(%icon);
	else
		EditorGui-->DisabledPluginsBox.add(%icon);

	Lab.sortPluginsBar();
}
//------------------------------------------------------------------------------
function Lab::createPluginIcon( %this, %pluginObj ) {
	%icon = "tlab/gui/buttons/plugin-assets/"@%pluginObj.plugin@"Icon";

	if (!isFile(%icon@"_n.png"))
		%icon = "tlab/gui/buttons/plugin-assets/TerrainEditorIcon";

	%button = cloneObject(EditorGui-->PluginIconSrc);
	%button.internalName = %pluginObj.plugin;
	%button.superClass = "";
	%button.command = "Lab.setEditor(" @ %pluginObj.getName()@ ");";
	%button.bitmap = %icon;
	%button.tooltip = %pluginObj.tooltip;
	%button.visible = true;
	%button.useMouseEvents = true;
	%button.pluginObj = %pluginObj;
	%button.class = "PluginIcon";
	return %button;
}
//------------------------------------------------------------------------------
//TO BE REMOVED
function Lab::addToPluginBar( %this, %pluginName, %tooltip,%disabled ) {
	%count = ToolsToolbarArray.getCount();
	%bin = EditorGui-->pluginBarTrash;
	%alreadyExists = false;

	for ( %i = 0; %i < %count; %i++ ) {
		%existingInternalName = ToolsToolbarArray.getObject(%i).getFieldValue("internalName");

		if(%pluginName $= %existingInternalName) {
			%alreadyExists = true;
			break;
		}
	}

	if (!%alreadyExists) {
		foreach ( %obj in %bin) {
			%existingInternalName = %obj.getFieldValue("internalName");

			if(%pluginName $= %existingInternalName)
				%alreadyExists = true;
		}
	}

	if(!%alreadyExists) {
		%icon = "tlab/gui/buttons/plugin-assets/"@%pluginName.plugin@"Icon";

		if (!isFile(%icon@"_n.png")) %icon = "tlab/gui/buttons/plugin-assets/TerrainEditorIcon";

		%button = cloneObject(EditorGui-->PluginIconSrc);
		%button.internalName = %pluginName;
		%button.command = "Lab.setEditor(" @ %pluginName @ ");";
		%button.bitmap = %icon;
		%button.tooltip = %tooltip;
		%button.visible = true;
		%button.useMouseEvents = true;
		%button.class = "PluginIcon";

		if (%pluginName.isEnabled)
			%bin.add(%button);
		else
			ToolsToolbarArray.add(%button);
	}
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function EWToolsToolbar::reset( %this ) {
	%count = ToolsToolbarArray.getCount();

	for( %i = 0 ; %i < %count; %i++ )
		ToolsToolbarArray.getObject(%i).setVisible(true);

	%this.setExtent((29 + 4) * %count + 12, 33);
	%this.isClosed = 0;
	EWToolsToolbar.isDynamic = 0;
	EWToolsToolbarDecoy.setVisible(false);
	EWToolsToolbarDecoy.setExtent((29 + 4) * %count + 4, 31);
	%this-->resizeArrow.setBitmap( "tlab/gui/icons/default/collapse-toolbar" );
}
function EWToolsToolbar::expand( %this, %close ) {
	%this.isClosed = !%close;
	%this.toggleSize();
}
function EWToolsToolbar::resize( %this ) {
	%this.isClosed = ! %this.isClosed ;
	%this.toggleSize();
}

function EWToolsToolbar::toggleSize( %this, %useDynamics ) {
	// toggles the size of the tooltoolbar. also goes through
	// and hides each control not currently selected. we hide the controls
	// in a very neat, spiffy way
	EWToolsToolbar-->resizeArrow.setExtent("10","40");

	if ( %this.isClosed == 0 ) {
		%image = "tlab/gui/icons/default/expand-toolbar";

		for( %i = 0 ; %i < ToolsToolbarArray.getCount(); %i++ ) {
			%object = ToolsToolbarArray.getObject(%i);
			%plugin = %object.pluginObj;
			%enabled =%plugin.isEnabled;
			%plugin.pluginOrder = %i+1;
			$LabData_PluginOrder[%plugin.plugin] = %i+1;
			//if( %plugin.getName() !$= Lab.currentEditor.getName())
			//%object.setVisible(false);
		}

		//%this.setExtent(45, 33);
		%this.extent = "54 40";
		%this-->resizeArrow.position = "44 0";
		%this.isClosed = 1;

		if(!%useDynamics) {
			EWToolsToolbarDecoy.setVisible(true);
			EWToolsToolbar.isDynamic = 1;
		}

		EWToolsToolbarDecoy.extent ="22 31";
	} else {
		%image = "tlab/gui/icons/default/collapse-toolbar";
		%count = ToolsToolbarArray.getCount();

		for( %i = 0 ; %i < %count; %i++ )
			ToolsToolbarArray.getObject(%i).setVisible(true);

		%extentY = 37 * %count + 14;
		%this.setExtent(%extentY, 40);
		%this.isClosed = 0;

		if(!%useDynamics) {
			EWToolsToolbarDecoy.setVisible(false);
			EWToolsToolbar.isDynamic = 0;
		}

		%extentY = 40 * %count + 34;
		EWToolsToolbarDecoy.extent =%extentY SPC"31";
	}

	EWToolsToolbar-->resizeArrow.setBitmap( %image );
	ToolsToolbarArray.refresh();
}

function EWToolsToolbarDecoy::onMouseEnter( %this ) {
	EWToolsToolbar.toggleSize(true);
}

function EWToolsToolbarDecoy::onMouseLeave( %this ) {
	EWToolsToolbar.toggleSize(true);
}


//==============================================================================
// Plugin Bar Drag and Drop
//==============================================================================



//==============================================================================
function PluginIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("PluginIcon class is corrupted! Fix it!");
		return;
	}

	dragAndDropCtrl(%this,"PluginIcon","DragFailed");
	hide(%this);
	Lab.openDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIcon::DragFailed( %this ) {
	Lab.closeDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIcon::DragSuccess( %this ) {
	Lab.updatePluginIconContainer();
	Lab.closeDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIconContainer::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;

	if (%ctrl.parentGroup.dropType !$= "PluginIcon") {
		warnLog("Only plugins icons can be drop in the Plugin Bar");
		Parent::onControlDropped( %this,%ctrl,%position );
		return;
	}

	//Simply remove it and add it so it go to end
	%this.remove(%originalCtrl);
	%this.add(%originalCtrl);
	show(%originalCtrl);
	delObj(%ctrl);
	%originalCtrl.DragSuccess();
}
//------------------------------------------------------------------------------
//==============================================================================
function DisabledPluginsBox::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;

	if (%ctrl.parentGroup.dropType !$= "PluginIcon") {
		warnLog("Only plugins icons can be drop in the Plugin Bar");
		return;
	}

	%this.add(%originalCtrl);
	show(%originalCtrl);
	delObj(%ctrl);
	%originalCtrl.DragSuccess();
	//%originalCtrl.class = "PluginIcon";
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIcon::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;

	if (%ctrl.parentGroup.dropType !$= "PluginIcon") {
		warnLog("Only plugins icons can be drop in the Plugin Bar");
		return;
	}

	//Let's add it just before this
	//%this.parentGroup.add(%ctrl);
	show(%originalCtrl);

	if (!ToolsToolbarArray.isMember(%originalCtrl))
		ToolsToolbarArray.add(%originalCtrl);

	ToolsToolbarArray.reorderChild(%originalCtrl,%this);
	delObj(%ctrl);
	%originalCtrl.DragSuccess();
}
//------------------------------------------------------------------------------

//==============================================================================
function DisabledPluginIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	dragAndDropCtrl(%this,"PluginIcon","DragFailed");
	hide(%this);
	Lab.openDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::updatePluginIconContainer( %this,%type ) {
	if (%type $= "")
		%doBoth = true;

	if (%type $= "Enabled" || %doBoth) {
		ToolsToolbarArray.refresh();
		EWToolsToolbar.resize();
	}

	if (%type $= "Disabled" || %doBoth) {
		EditorGui-->DisabledPluginsBox.refresh();
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::openDisabledPluginsBin( %this,%removeOnly ) {
	%bin = EditorGui-->pluginBarTrash;

	if (%bin.isVisible())
		return;

	%bin.position.y = EWToolsToolbar.extent.y + 4;

	if (%removeOnly) {
		%bin.extent.y = %bin.lowBox;
	} else {
		%bin.extent.y = %bin.hiBox;
	}

	show(%bin);
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::closeDisabledPluginsBin( %this,%autoClose ) {
	%bin = EditorGui-->pluginBarTrash;

	if (%autoClose && %bin.extent.y $= %bin.hiBox) {
		warnLog("The Disabled plugin box is in full mode and must be close with button");
		return;
	}

	hide(%bin);
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::clearDisabledPluginsBin( %this ) {
	%bin = EditorGui-->DisabledPluginsBox;
	%bin.deleteAllObjects();
}
//------------------------------------------------------------------------------