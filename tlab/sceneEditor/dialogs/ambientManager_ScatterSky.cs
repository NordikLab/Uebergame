//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function SEP_ScatterSkyManager::buildParams( %this ) {
	%arCfg = createParamsArray("SEP_ScatterSky",SEP_ScatterSkyProperties);
	%arCfg.updateFunc = "SEP_ScatterSkyManager.updateParam";
	%arCfg.style = "LabCfgB_230";
	%arCfg.useNewSystem = true;
	%arCfg.group[%gid++] = "Lighting settings" TAB "Stack StackA";
	%arCfg.setVal("skyBrightness",       "" TAB "Sky brightness" TAB "SliderEdit" TAB "range>>0 50;;tickAt>>0.1" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("exposure",       "" TAB "Sky exposure" TAB "SliderEdit" TAB "range>>0 50;;tickAt>>0.1" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("brightness",       "" TAB "Sun brightness" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.01" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("sunScale",       "" TAB "sunScale" TAB "ColorSlider" TAB "mode>>float;;flen>>2" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("ambientScale",       "" TAB "ambientScale" TAB "ColorSlider" TAB "mode>>float;;validate>>flen 2" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	
	%arCfg.setVal("flareScale",       "" TAB "flareScale" TAB "SliderEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	
	%arCfg.group[%gid++] = "Sky & Sun settings" TAB "Stack StackA";
	%arCfg.setVal("rayleighScattering",       "" TAB "rayleighScattering" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("fogScale",       "" TAB "fogScale" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("zOffset",       "" TAB "zOffset" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("ambientScale",       "" TAB "ambientScale" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("sunSize",       "" TAB "sunSize" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("colorizeAmount",       "" TAB "colorizeAmount" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("colorize",       "" TAB "colorize" TAB "ColorSlider" TAB "mode>>float" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("azimuth",       "" TAB "azimuth" TAB "SliderEdit" TAB "range>>0 360;;validate>>flen 1" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("elevation",       "" TAB "elevation" TAB "SliderEdit" TAB "range>>0 90" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);

	%arCfg.group[%gid++] = "Shadows settings" TAB "Stack StackB";
	%arCfg.setVal("overDarkFactor",       "" TAB "overDarkFactor" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("shadowDistance",       "" TAB "shadowDistance" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("shadowSoftness",       "" TAB "shadowSoftness" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("numSplits",       "" TAB "numSplits" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("logWeight",       "" TAB "logWeight" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("shadowType",       "" TAB "shadowType" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("texSize",       "" TAB "texSize" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("fadeStartDistance",       "" TAB "fadeStartDistance" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("shadowDarkenColor",       "" TAB "shadowDarkenColor" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("castShadows",       "" TAB "castShadows" TAB "Checkbox" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("lastSplitTerrainOnly",       "" TAB "lastSplitTerrainOnly" TAB "Checkbox" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("includeLightmappedGeometryInShadow",       "" TAB "includeLightmappedGeometryInShadow" TAB "Checkbox" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("representedInLightmap",       "" TAB "representedInLightmap" TAB "Checkbox" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	
	
%arCfg.group[%gid++] = "Night settings" TAB "Stack StackB";
	%arCfg.setVal("moonAzimuth",       "" TAB "moonAzimuth" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("moonElevation",       "" TAB "moonElevation" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("moonEnabled",       "" TAB "moonEnabled" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("moonScale",       "" TAB "moonScale" TAB "TextEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("moonLightColor",       "" TAB "moonLightColor" TAB "ColorSlider" TAB "mode>>float" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("nightColor",       "" TAB "nightColor" TAB "ColorSlider" TAB "mode>>float"TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("nightFogColor",       "" TAB "nightFogColor" TAB "ColorSlider" TAB "mode>>float" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	%arCfg.setVal("useNightCubemap",      "" TAB "useNightCubemap" TAB "SliderEdit" TAB "" TAB "SEP_ScatterSkyManager.selectedScatterSky" TAB %gid);
	buildParamsArray(%arCfg,false);
	SEP_ScatterSkyManager.paramArray = %arCfg;
}
//------------------------------------------------------------------------------


//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_ScatterSkyManager::initData( %this ) {
	%scatterSkyList = Lab.getMissionObjectClassList("ScatterSky");
	%this.selectScatterSky(getWord(%scatterSkyList,0));
	
	SEP_ScatterSkySystemMenu.clear();
	SEP_ScatterSkySystemMenu.add("Scatter Sky Object",0);
	SEP_ScatterSkySystemMenu.add("Sun + Standard Sky (Legacy)",1);
	SEP_ScatterSkySystemMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScatterSkyManager::selectScatterSky(%this,%obj) {
	logd("SEP_ScatterSkyManager::selectScatterSky(%this,%obj)",%this,%obj);

	if (!isObject(%obj)) {
		%this.selectedScatterSky = "";
		%this.selectedScatterSkyName = "";
		return;
	}
	
	%this.selectedScatterSky = %obj;
	%this.selectedScatterSkyName = %obj.getName();
	%this.setDirty();
	ScatterSkyInspector.inspect(	%obj);
	syncParamArray(SEP_ScatterSkyManager.paramArray);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScatterSkyManager::saveObject(%this) {
	logd("SEP_ScatterSkyManager::saveObject(%this)",%this);
	%obj = %this.selectedScatterSky;

	if (!isObject(%obj)) {
		warnLog("Can't save ScatterSky because none is selected. Tried wth:",%obj);
		return;
	}

	if (!SEP_AmbientManager_PM.isDirty(%obj)) {
		warnLog("Object is not dirty, nothing to save");
		return;
	}

	//SEP_AmbientManager_PM.setDirty(%obj);
	SEP_AmbientManager_PM.saveDirtyObject(%obj);
	%this.setDirty(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScatterSkyManager::setDirty(%this,%isDirty) {
	logd("SEP_ScatterSkyManager::setDirty(%this,%isDirty)",%this,%isDirty);
	%obj = %this.selectedScatterSky;

	if (%isDirty $="")
		%isDirty = SEP_AmbientManager_PM.isDirty(%obj);
	else if ( !SEP_AmbientManager_PM.isDirty(%obj) && %isDirty)
		SEP_AmbientManager_PM.setDirty( %obj );
	else if ( SEP_AmbientManager_PM.isDirty(%obj) && !%isDirty)
		SEP_AmbientManager_PM.removeDirty( %obj );

	%this.isDirty = %isDirty;
	SEP_ScatterSkySaveButton.active = %isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScatterSkyManager::updateFieldValue(%this,%field,%value) {	
	%obj = %this.selectedScatterSky;

	if (!isObject(%obj)) {
		warnLog("Can't update scatterSky value because none is selected. Tried wth:",%obj);
		return;
	}

	%currentValue = %obj.getFieldValue(%field);

	if (%currentValue $= %value) {		
		return;
	}

	ScatterSkyInspector.apply();
	//eval("%obj."@%checkField@" = %value;");
	%obj.setFieldValue(%field,%value);
	EWorldEditor.isDirty = true;
	%this.setDirty(true);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_ScatterSkyManager::updateParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("SEP_ScatterSkyManager::updateParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	%this.updateFieldValue(%field,%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_ScatterSkySystemMenu::OnSelect(%this,%id,%text) {
	logd("SEP_ScatterSkySystemMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
	SEP_SkySystemCreator-->ScatterSky.visible = 0;
	SEP_SkySystemCreator-->Legacy.visible = 0;
	if (%id $= "0"){
		SEP_SkySystemCreator-->ScatterSky.visible = 1;
	} else {
		SEP_SkySystemCreator-->Legacy.visible = 1;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
/*
Lighting
skyBrightness = "30";
exposure = "0.85";
sunScale = "1 1 0.8 1";
ambientScale = "0.5 0.5 0.4 1";
brightness = "1";
flareScale = "1";
attenuationRatio = "0 1 1";

Shadow
shadowType = "PSSM";
castShadows = "1";
texSize = "512";
overDarkFactor = "2000 1000 500 100";
shadowDistance = "400";
shadowSoftness = "0.15";
numSplits = "4";
logWeight = "0.91";
fadeStartDistance = "0";
lastSplitTerrainOnly = "0";
representedInLightmap = "0";
shadowDarkenColor = "0 0 0 -1";
includeLightmappedGeometryInShadow = "0";


Sky
rayleighScattering = "0.0035";
fogScale = "1 1 1 1";
zOffset = "0";

Sun
sunSize = "1";
colorizeAmount = "0";
colorize = "0 0 0 1";
azimuth = "0";
elevation = "30";


Night
moonAzimuth = "0";
moonElevation = "45";
moonEnabled = "1";
moonScale = "0.2";
moonLightColor = "0.192157 0.192157 0.192157 1";
nightColor = "0.0196078 0.0117647 0.109804 1";
nightFogColor = "0.0196078 0.0117647 0.109804 1";
useNightCubemap = "0";














position = "0 0 0";
rotation = "1 0 0 0";
scale = "1 1 1";
canSave = "1";
canSaveDynamicFields = "1";
mieScattering = "0.0015";
sunBrightness = "50";
*/
