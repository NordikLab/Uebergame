//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MEP_ShowGroupCtrl["Animation"] = "materialAnimationPropertiesRollout";
$MEP_ShowGroupCtrl["Rendering"] = "MEP_GroupRendering";
$MEP_ShowGroupCtrl["Advanced"] = "materialAdvancedPropertiesRollout";
//==============================================================================
function MaterialEditorPlugin::initPropertySetting(%this) {
	devLog("MaterialEditorPlugin::initPropertySetting(%this)",%this,%ctrl,"Name",%ctrl.internalName);
	%textureMapStack = MaterialEditorGui-->textureMapsOptionsStack;

	foreach(%ctrl in %textureMapStack) {
		%map = %ctrl.internalName;
		%ctrl.variable = "$Cfg_Plugins_MaterialEditor_PropShowMap_"@%map;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Material Properties Display Options Callback
//==============================================================================
//==============================================================================
function MaterialEditorPlugin::toggleMap(%this,%ctrl) {
	devLog("MaterialEditorPlugin::toggleMap(%this,%ctrl)",%this,%ctrl,"Name",%ctrl.internalName);
	%map = %ctrl.internalName;
	%visible = %ctrl.isStateOn();
	$MEP_ShowMap[%map] = %visible;
	%cont = MEP_TextureMapStack.findObjectByInternalName(%map,true);
	%cont.setVisible(%visible);
	MaterialEditorPlugin.setCfg("PropShowMap_"@%map,%visible);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorPlugin::toggleGroup(%this,%ctrl) {
	devLog("MaterialEditorPlugin::toggleGroup(%this,%ctrl)",%this,%ctrl,"Name",%ctrl.internalName);
	%group = %ctrl.internalName;
	%visible = %ctrl.isStateOn();
	%groupCtrl = $MEP_ShowGroupCtrl[%group];

	if (!isObject(%groupCtrl))
		return;

	%groupCtrl.setVisible(%visible);
	MaterialEditorPlugin.setCfg("PropShowGroup_"@%group,%visible);
}
//------------------------------------------------------------------------------
