//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::initConfigSystem( %this ) {
	//Start by building all the ConfigArray Params
	$LabConfigArrayGroup = newSimGroup("LabConfigArrayGroup");
	exec("tlab/core/commonSettings.cs");
	%this.initCommonParams();
	%this.initAllPluginConfig();
	LabCfg.read();
	//FIXME Rebuild params everytime for development but should be optimized later
	LabParamsDlg.rebuildAll();
	Lab.initDefaultSettings();
}
//------------------------------------------------------------------------------


//==============================================================================
function Lab::exportCfgPrefs(%this) {
	export("$Cfg_*", "tlab/configPrefs.cs", false);
}
//------------------------------------------------------------------------------


//==============================================================================
// Set params settings group to their default value
//==============================================================================
//==============================================================================
function Lab::setAllParamArrayDefaults(%this) {
	foreach(%array in LabParamsGroup)
		%this.setParamArrayDefaults(%array);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setParamArrayDefaults(%this,%array) {
	%pattern = strreplace(%array.groupLink,"_","/");
	LabCfg.beginGroup( %pattern, true );
	%i = 0;

	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		%default = getWord(%data,0);
		//%value = LabCfg.setCfg( %field,%default,true );
		//$Cfg_[%array.groupLink,%field] = %value;
	}

	LabCfg.endGroup();
}
//------------------------------------------------------------------------------


//==============================================================================
// OLD System kept for reference showing how to read SettingObject file
//==============================================================================
function Lab::readAllConfigArray(%this) {
	foreach(%array in LabParamsGroup)
		%this.readConfigArray(%array);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::readConfigArray(%this,%array) {
	%pattern = strreplace(%array.groupLink,"_","/");
	LabCfg.beginGroup( %pattern, true );
	%i = 0;

	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%value = LabCfg.value( %field );
		$Cfg_[%array.groupLink,%field] = %value;
		$CfgParams_[%array.groupLink,%field] = %value;
	}

	LabCfg.endGroup();
	return;
	//Not needed anymore, there's a simplier system to sync the params
	%i = 0;

	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%value = $Cfg_[%array.groupLink,%field];
		%this.updateConfigFieldValue(%array,%field,%value);
	}
}
//------------------------------------------------------------------------------