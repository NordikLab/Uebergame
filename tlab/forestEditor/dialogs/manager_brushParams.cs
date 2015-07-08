//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function FEP_Manager::buildBrushParams( %this ) {
	%arCfg = createParamsArray("FEP_Brush",FEP_ManagerBrushProperties);
	%arCfg.updateFunc = "FEP_Manager.updateSimGroupParam";
	%arCfg.style = "LabCfgB_230";
	%arCfg.useNewSystem = true;

	%arCfg.group[%gid++] = "Group settings" TAB "Stack GroupStack";

	%arCfg.setVal("groupScaleScalar",       "" TAB "childScaleScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupSinkScalar",        "" TAB "childSinkScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupElevationScalar",   "" TAB "childElevationScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	%arCfg.setVal("groupSlopeScalar",        "" TAB "childSlopeScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrushGroup" TAB %gid);
	
	%arCfg.group[%gid++] = "Brush settings" TAB "Stack BrushStack";

	%arCfg.setVal("brushScaleScalar",       "" TAB "childScaleScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushSinkScalar",        "" TAB "childSinkScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushElevationScalar",   "" TAB "childElevationScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	%arCfg.setVal("brushSlopeScalar",        "" TAB "childSlopeScalar" TAB "SliderEdit" TAB "range>>0 100" TAB "FEP_Manager.selectedBrush" TAB %gid);
	
	buildParamsArray(%arCfg,false);
	%this.brushParamArray = %arCfg;
}
//------------------------------------------------------------------------------
