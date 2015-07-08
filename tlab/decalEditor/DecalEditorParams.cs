//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Initialize default plugin settings
function DecalEditorPlugin::initDefaultSettings( %this ) {
	Lab.addDefaultSetting(  "DefaultScale",         "1" );
}
//------------------------------------------------------------------------------

//==============================================================================
// Automated editor plugin setting interface
function DecalEditorPlugin::buildParams(%this,%params ) {
//-------------------------------------------------
// Group 1 Configuration
	%gid++;
	%pid = 0;
	%params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
	%params.groupParam[%gid,%pid++] = "DefaultScale"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::2";
	return %params;
}
//------------------------------------------------------------------------------