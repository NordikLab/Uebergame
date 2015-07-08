//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Cfg_Plugins_ShapeEditor_ShowAdvancedPanel = false;
$Cfg_Plugins_ShapeEditor_ShowAnimationPanel = false;
$Cfg_Plugins_ShapeEditor_UseSimplifiedSystem = true;
//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================
function ShapeEditorTools::sePanelVisibility( %this ) {
	devLog("ShapeEditorTools::togglePanelVisibility( %this,%checkbox )",%this,%checkbox);
	%panel = %checkbox.panel;
}

function ShapeEditorTools::updatePanels( %this ) {
	devLog("ShapeEditorTools::updatePanels:",%this);

	if ($Cfg_Plugins_ShapeEditor_UseSimplifiedSystem) {
		%this.updateSimplifiedPanels();
		return;
	}

	%showAdvanced = $Cfg_Plugins_ShapeEditor_ShowAdvancedPanel;
	%showAnimation = $Cfg_Plugins_ShapeEditor_ShowAnimationPanel;
	devLog("ShapeEditorTools::updatePanels Animation=",%showAnimation,"Advanced=",%showAdvanced);
	%extent = 	ShapeEditorTools.extent.y;
	%mainPanelSize = getWord(ShapeEditorTools.rows,1);
	%rows = "0";
	%this.hideCtrl(ShapeEdSubPanel);
	%currentIndex = 0;
	%this.showCtrl(ShapeEdPropWindow,%currentIndex++);

	if (%showAnimation)
		%this.showCtrl(ShapeEdAnimationWindow,%currentIndex++);
	else
		%this.hideCtrl(ShapeEdAnimationWindow);

	if (%showAdvanced)
		%this.showCtrl(ShapeEdAdvancedWindow,%currentIndex++);
	else
		%this.hideCtrl(ShapeEdAdvancedWindow);

	devLog("MainPanelSize=",%mainPanelSize);

	foreach(%ctrl in ShapeEditorTools) {
		if (%ctrl.internalName $= "MainPanel" || !%ctrl.visible)
			continue;

		%subPanelCount++;
		devLog("Updating sub panel:",%ctrl.getName(),"As sub panel id:",%subPanelCount);
	}

	%subExtent = %extent - %mainPanelSize;
	%subPanelSize = %subExtent/%subPanelCount;
	%subPanelPos = %mainPanelSize;

	for(%i=1; %i<=%subPanelCount; %i++) {
		%rows = strAddWord(%rows,%subPanelPos);
		%subPanelPos += %subPanelSize;
	}

	devLog("Panel Set rows set to:",%rows);
	%this.setRows(%rows,true);
}

function ShapeEditorTools::updateSimplifiedPanels( %this ) {
	devLog("ShapeEditorTools::updateSimplifiedPanels:",%this);
	%this.hideCtrl(ShapeEdAdvancedWindow);
	%this.hideCtrl(ShapeEdAnimationWindow);
	%this.hideCtrl(ShapeEdPropWindow);
	%this.showCtrl(ShapeEdSubPanel,1);
	%rows = getWords(ShapeEditorTools.rows,0,1);
	%this.setRows(%rows);
}