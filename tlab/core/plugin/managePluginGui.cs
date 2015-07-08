//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabPlugin::ToolbarPos = "306 0";

//==============================================================================
//Reinitialize all plugin data
function Lab::prepareAllPluginsGui(%this) {
	//Prepare the plugins toolbars
	%this.initAllPluginsToolbar();
	%this.initAllPluginsDialogs();
	
	
}
//------------------------------------------------------------------------------

//==============================================================================
// Activate the interface for a plugin
//==============================================================================
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function Lab::activatePluginGui(%this,%pluginObj) {
	//Check if the toolFrame
	if (EditorFrameMain.columns $= "0") {
		EditorFrameMain.columns = Lab.previousEditorColumns;
		EditorFrameMain.updateSizes();
	}

	%pluginFrameSet = %pluginObj.plugin@"_FrameSet";

	if (!isObject(%pluginFrameSet)) {
		Lab.previousEditorColumns = EditorFrameMain.columns;
		//No tool frame for this plugin
		EditorFrameMain.columns = "0";
		EditorFrameMain.updateSizes();
	}

	if (%pluginObj.no3D)
		ECamViewGui.setState(false,true);
	else
		ECamViewGui.setState($Lab_CamViewEnabled);

	//Hide all the Guis for all plugins
	foreach(%gui in LabPluginGuiSet)
		%gui.setVisible(false);

	//Show only the Gui related to actiavted plugin
	%pluginGuiSet = %pluginObj.plugin@"_GuiSet";

	foreach(%gui in %pluginGuiSet) {
		//Don't show dialogs
		if (%gui.isDlg)
			continue;

		%gui.setVisible(true);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
// Manage the GUI for the plugins
//==============================================================================
function Lab::addGuiToPluginSet(%this,%plugin,%gui) {
	%pluginSimSet = %plugin@"_GuiSet";

	if (!isObject(%pluginSimSet)) {
		%pluginSimSet = newSimSet(%pluginSimSet);
	}
	%gui.plugin = %plugin;
	%pluginSimSet.add(%gui);
	LabPluginGuiSet.add(%gui);
}
function Lab::addPluginEditor(%this,%plugin,%gui,%notFullscreen) {
	%this.addGuiToPluginSet(%plugin,%gui);

	if(%notFullscreen) {
		%this.addGui(%gui,"ExtraGui");
	} else {
		%this.addGui(%gui,"EditorGui");
	}

	// Simset Holding Editor Guis for the plugin
}
function Lab::addPluginGui(%this,%plugin,%gui) {
	%this.addGuiToPluginSet(%plugin,%gui);
	%pluginFrameSet = %plugin@"_FrameSet";

	if (!isObject(%pluginFrameSet)) {
		newSimSet(%pluginFrameSet);
	}

	%pluginFrameSet.add(%gui);
	// Simset Holding Editor Guis for the plugin
	%this.addGui(%gui,"Gui");
}

function Lab::addPluginToolbar(%this,%plugin,%gui) {
	%gui.plugin = %plugin;
	%this.addGuiToPluginSet(%plugin,%gui);
	%this.addGui(%gui,"Toolbar");
}
function Lab::addPluginDlg(%this,%plugin,%gui) {
	%pluginObj = %plugin@"Plugin";
	%pluginObj.dialogs = %gui;
	%this.addGuiToPluginSet(%plugin,%gui);
	%gui.pluginObj = %pluginObj;
	%gui.superClass = "PluginDlg";
	%this.addGui(%gui,"Dialog");
	%gui.isDlg = true;	
	
	
}

function Lab::addPluginPalette(%this,%plugin,%gui) {
	%gui.internalName = %plugin;
	%this.addGui(%gui,"Palette");
}



//==============================================================================
// Editor Main menu functions
//==============================================================================

//==============================================================================
function Lab::addToEditorsMenu( %this, %pluginObj ) {
	%displayName = %pluginObj.displayName;
	%accel = "";

	if ($Cfg_UseCoreMenubar) {
		%windowMenu = Lab.findMenu( "Editors" );
		%count = %windowMenu.getItemCount();
		%alreadyExists = false;

		for ( %i = 0; %i < %count; %i++ ) {
			%thisName = getField(%windowMenu.Item[%i], 2);

			if(%plugin.getName() $= %thisName)
				%alreadyExists = true;
		}

		if( %accel $= "" && %count < 9 )
			%accel = "F" @ %count + 1;
		else
			%accel = "";

		if(!%alreadyExists)
			%windowMenu.addItem( %count, %displayName TAB %accel TAB %pluginObj.getName() );
	} else if ($LabMenuEditor[%plugin.plugin] $= "") {
		%menuId = getWord($LabMenuEditorSubMenu,0);
		%itemId = getWord($LabMenuEditorSubMenu,1);
		%subId = $LabMenuEditorNextId++;
		$LabMenuSubMenuItem[%menuId,%itemId,%subId] = %displayName TAB "" TAB "Lab.setEditor(\""@%pluginObj.getName()@"\");";
		Lab.addSubmenuItem(%menuId,%itemId,%displayName,%subId,"",-1);
	}

	return %accel;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeFromEditorsMenu( %this,  %pluginObj ) {
	if (!isObject(%windowMenu))
		return;
	%windowMenu = Lab.findMenu( "Editors" );
	%pluginName = %pluginObj.getName();
	%count = %windowMenu.getItemCount();
	%removeId = -1;

	for ( %i = 0; %i < %count; %i++ ) {
		%thisName = getField(%windowMenu.Item[%i],2);

		if(%pluginName $= %thisName) {
			%windowMenu.removeItem(%i);
			break;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::resetEditorsMenu( %this ) {
	%windowMenu = Lab.findMenu( "Editors" );
	%count = %windowMenu.getItemCount();

	for ( %i = %count; %i > 0; %i--)
		%windowMenu.removeItem(%i-1);
}
//------------------------------------------------------------------------------

