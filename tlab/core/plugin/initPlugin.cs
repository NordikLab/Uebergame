//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab::DefaultPlugins = "SceneEditor";

//==============================================================================
// Create the Plugin object with initial data
function Lab::createPlugin(%this,%pluginName,%displayName,%alwaysEnable) {
	//Set plugin object name and verify if already existing
	%plugName = %pluginName@"Plugin";

	if (isObject( %plugName)) {
		warnLog("Plugin already created:",%pluginName);
		return %plugName;
	}

	if (%displayName $= "")
		%displayName = %pluginName;

	%pluginOrder = "99";

	if ($LabData_PluginOrder[%pluginName] !$= "")
		%pluginOrder = $LabData_PluginOrder[%pluginName];

	//Create the ScriptObject for the Plugin
	%pluginObj = new ScriptObject( %plugName ) {
		superClass = "EditorPlugin"; //Default to EditorPlugin class
		editorGui = EWorldEditor; //Default to EWorldEditor
		editorMode = "World";
		plugin = %pluginName;
		displayName = %displayName;
		toolTip = %displayName;
		alwaysOn = %alwaysEnable;
		pluginOrder = %pluginOrder;
		shortPlugin = %shortObjName;
	};
	LabPluginGroup.add(%pluginObj);

	if (strFind($TLab::DefaultPlugins,%pluginName))
		%pluginObj.isDefaultPlugin = true;

	if (%alwaysEnable)
		$PluginAlwaysOn[%pluginName] = true;

	if (%pluginObj.isMethod("onPluginCreated"))
		%pluginObj.onPluginCreated();

	//Lab.initPluginData(%pluginObj);
	return %pluginObj;
}
//------------------------------------------------------------------------------
//==============================================================================
//Reinitialize all plugin data
function Lab::initAllPluginConfig(%this) {
	foreach(%plugin  in LabPluginGroup)
		%this.initPluginConfig(%plugin);
}
//------------------------------------------------------------------------------
//==============================================================================
//Initialize plugin data
function Lab::initPluginConfig(%this,%pluginObj) {
	%pluginName = %pluginObj.plugin;
//	%array = newArrayObject("ar"@%pluginName@"Cfg",LabConfigArrayGroup);
//	%array.pluginObj =%pluginObj;
//	%pluginObj.arrayCfg = %array;
//	%array.groupLink = "Plugins_"@%pluginName;
//	%array.groupLinkName = %pluginName SPC "Settings";
//	%array.internalName = %pluginName;
	//if (%pluginObj.isMethod("initDefaultCfg"))
//		%pluginObj.initDefaultCfg(%array);
	//%array.setVal("pluginOrder",      "99" TAB "pluginOrder" TAB "" TAB "" TAB %pluginObj.getName());
	//%array.setVal("isEnabled",      "1" TAB "isEnabled" TAB "" TAB "" TAB %pluginObj.getName());
	//Moving toward new params array system
	%newArray = Lab.newParamsArray(%pluginName,"Plugins",%pluginObj);
	%newArray.displayName = %pluginObj.displayName;
	%pluginObj.paramArray = %newArray;
	%newArray.pluginObj = %pluginObj;
	%newArray.paramCallback = "Lab.onParamPluginBuild";
	%newArray.setVal("pluginOrder",      "99" TAB "pluginOrder" TAB "" TAB "" TAB %pluginObj.getName());
	%newArray.setVal("isEnabled",      "1" TAB "isEnabled" TAB "" TAB "" TAB %pluginObj.getName());

	if (%pluginObj.isMethod("initParamsArray"))
		%pluginObj.initParamsArray(%newArray);

	//%this.initConfigArray( %array,true);
	//%pluginObj.isEnabled =  %pluginObj.checkCfg("Enabled","1");
	//%pluginObj.pluginOrder =  %pluginObj.checkCfg("pluginOrder","99");
}
//------------------------------------------------------------------------------
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
/*
function Lab::activatePlugin(%this,%pluginObj) {
   //Reset some default Plugin values
   Lab.fitCameraGui = ""; //Used by GuiShapeEdPreview to Fit camera on object
   //Call the Plugin Object onActivated method if exist
	if(%pluginObj.isMethod("onActivated"))
		%pluginObj.onActivated();

   %this.activatePluginGui(%pluginObj);
}
//------------------------------------------------------------------------------

//==============================================================================
//Set a plugin as inactive (Not Selected Editor Plugin)
function Lab::deactivatePlugin(%this,%pluginObj) {
   //Call the Plugin Object onDeactivated method if exist
	if(%pluginObj.isMethod("onDeactivated"))
		%pluginObj.onDeactivated();
}
*/
//------------------------------------------------------------------------------
//==============================================================================
//Allow the plugin to be selected in editor
function Lab::enablePlugin(%this,%pluginObj,%enabled,%showLog) {
	if (!isObject(%pluginObj)) {
		warnLog("Trying to enable invalid plugin:",%pluginObj);
		return;
	}

	if (%pluginObj.alwaysOn) %enabled = "1";

	%name = %pluginObj.plugin;

	if (%name $= "")
		%name = %pluginObj.getName()@"_fromObj";

	%toolArray = ToolsToolbarArray.findObjectByInternalName(%pluginObj.getName());
	%toolDisabledArray = EditorGui-->DisabledPluginsBox.findObjectByInternalName(%pluginObj.getName());

	if (%enabled) {
		if (!isObject(%toolArray))
			%this.AddToEditorsMenu(%pluginObj);

		%pluginObj.isEnabled = true;

		if (%showLog)
			info(%name,"enabled");
	} else {
		hide(%toolArrayObj);
		%this.removeFromEditorsMenu(%pluginObj);
		%pluginObj.isEnabled = false;

		if (%showLog)
			info(%name,"disabled");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Call when Editor is open, check that all plugin are enabled correctly
function Lab::updateActivePlugins(%this) {
	foreach(%pluginObj in LabPluginGroup) {
		%enabled = %pluginObj.getCfg("enabled");
		%this.enablePlugin(%pluginObj,%enabled);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::saveAllPluginData(%this) {
	//Update the PluginBar data (PluginOrder for now)
	Lab.updatePluginBarData();

	foreach(%pluginObj in LabPluginGroup) {
		%pluginObj.setCfg("pluginOrder",%pluginObj.pluginOrder);
	}
}
//------------------------------------------------------------------------------
