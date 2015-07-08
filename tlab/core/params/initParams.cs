//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
//Initialize plugin data
function Lab::newParamsArray(%this,%name,%group,%cfgObject,%useLongName) {
	if (%name $= "") {
		warnLog("You need to specify a name for the settings which is unique in this type");
		return;
	}

	if (%container $= "")
		%container = %name@"ParamStack";

	%arrayName = %name;

	if (%useLongName)
		%arrayName = %group@%name;

	%fullName = %arrayName@"_Param";
	%array = newArrayObject(%fullName,LabParamsGroup,"ParamArray");
	%array.internalName = %name;
	%array.displayName = %name;
	%array.container = %container;
	%array.paramCallback = "Lab.onParamBuild";
	%array.group = %group;
	%array.useNewSystem = true;
	//If no cfgObject supplied, simply use the new array as object
	if (!isObject(%cfgObject))
		%cfgObject = %array;

	%array.cfgObject = %cfgObject;
	%array.groupLink = %group@"_"@%name;
	%array.style = "LabCfgB_304";

	if (%prefGroup $= "")
		%prefGroup = %name;

	%array.prefGroup = %prefGroup;
	%array.updateFunc = "LabParams.updateParamArrayCtrl";
	//%array.common["command"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"\",\"\");";
	//%array.common["altCommand"] = "syncParamArrayCtrl($ThisControl,\"LabParams.updateParamArrayCtrl\",\""@%fullName@"\",\"true\",\"\");";
	return %array;
}
//------------------------------------------------------------------------------
//==============================================================================
//Initialize plugin data
function Lab::onParamBuild(%this,%array,%field,%paramData) {
	paramLog("Lab::onParamBuild(%this,%array,%field,%paramData)",%this,%array,%field,%paramData);
}
//------------------------------------------------------------------------------
//==============================================================================
//Initialize plugin data
function Lab::onParamPluginBuild(%this,%array,%field,%paramData) {
	paramLog("Lab::onParamPluginBuild(%this,%array,%field,%paramData)",%this,%array,%field,%paramData);
	%plugin = %array.pluginObj;
	%cfgValue = %plugin.getCfg(%field);

	if (%cfgValue $= "")
		%plugin.setCfg(%field,%paramData.Default);
}
//------------------------------------------------------------------------------