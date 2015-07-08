//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================

function TerrainEditorPlugin::initParamsArray( %this,%cfgArray ) {
	%cfgArray.group[%gid++] = "Action values";
	%cfgArray.setVal("maxBrushSize",       "40 40" TAB "maxBrushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("BrushSize",       "2" TAB "brushSize" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSize(**);" TAB %gid);
	%cfgArray.setVal("BrushType",       "box" TAB "brushType" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushType(**);" TAB %gid);
	%cfgArray.setVal("BrushPressure",       "1" TAB "brushPressure" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushPressure(**);" TAB %gid);
	%cfgArray.setVal("BrushSoftness",       "1" TAB "brushSoftness" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSoftness(**);" TAB %gid);
	%cfgArray.setVal("BrushSetHeight",       "1" TAB "Brush set height" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSetHeight(**);" TAB %gid);
	%cfgArray.setVal("BrushSetHeightRange",       "0 100" TAB "Brush set height range" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.brushSetHeightRange(**);" TAB %gid);
	%cfgArray.group[%gid++] = "Action values";
	%cfgArray.setVal("adjustHeightVal",       "10" TAB "adjustHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor"  TAB %gid);
	%cfgArray.setVal("setHeightVal",       "100" TAB "setHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("scaleVal",       "1" TAB "scaleVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("smoothFactor",       "0.1" TAB "smoothFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("noiseFactor",       "1.0" TAB "noiseFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectRadius",       "50" TAB "softSelectRadius" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectDefaultFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectDefaultFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("slopeMinAngle",       "0" TAB "slopeMinAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMinAngle(**);" TAB %gid);
	%cfgArray.setVal("slopeMaxAngle",       "90" TAB "slopeMaxAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMaxAngle(**);" TAB %gid);
	%cfgArray.group[%gid++] = "Default values";
	%cfgArray.setVal("DefaultBrushSize",       "2" TAB "Default brush size" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushType",       "box" TAB "Default brush type" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushPressure",       "1" TAB "Default brush pressure" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushSoftness",       "1" TAB "Default brush softness" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushSetHeight",       "1" TAB "Default brush set height" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
}

function TerrainEditorPlugin::initBinds( %this ) {
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "EWToolsPaletteArray->brushAdjustHeight.performClick();", "" );    //Grab Terrain
	%map.bindCmd( keyboard, "2", "EWToolsPaletteArray->raiseHeight.performClick();", "" );     // Raise Height
	%map.bindCmd( keyboard, "3", "EWToolsPaletteArray->lowerHeight.performClick();", "" );     // Lower Height
	%map.bindCmd( keyboard, "4", "EWToolsPaletteArray->smoothHeight.performClick();", "" );    // Average Height
	%map.bindCmd( keyboard, "5", "EWToolsPaletteArray->smoothSlope.performClick();", "" );    // Smooth Slope
	%map.bindCmd( keyboard, "6", "EWToolsPaletteArray->paintNoise.performClick();", "" );      // Noise
	%map.bindCmd( keyboard, "7", "EWToolsPaletteArray->flattenHeight.performClick();", "" );   // Flatten
	%map.bindCmd( keyboard, "8", "EWToolsPaletteArray->setHeight.performClick();", "" );       // Set Height
	%map.bindCmd( keyboard, "9", "EWToolsPaletteArray->setEmpty.performClick();", "" );    // Clear Terrain
	%map.bindCmd( keyboard, "0", "EWToolsPaletteArray->clearEmpty.performClick();", "" );  // Restore Terrain
	%map.bindCmd( keyboard, "v", "EWTerrainEditToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
	%map.bindCmd( keyboard, "b", "EWTerrainEditToolbarBrushType->box.performClick();", "" );// Box Brush
	%map.bindCmd( keyboard, "=", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "+", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "-", "TerrainEditorPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
	%map.bindCmd( keyboard, "[", "TerrainEditorPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
	%map.bindCmd( keyboard, "]", "TerrainEditorPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
	/*%map.bindCmd( keyboard, "]", "TerrainBrushPressureTextEditContainer->textEdit.text += 5", "" );// +5 Pressure
	%map.bindCmd( keyboard, "[", "TerrainBrushPressureTextEditContainer->textEdit.text -= 5", "" );// -5 Pressure
	%map.bindCmd( keyboard, "'", "TerrainBrushSoftnessTextEditContainer->textEdit.text += 5", "" );// +5 Softness
	%map.bindCmd( keyboard, ";", "TerrainBrushSoftnessTextEditContainer->textEdit.text -= 5", "" );// -5 Softness*/
	TerrainEditorPlugin.map = %map;
}

//------------------------------------------------------------------------------
// TerrainEditorPlugin
//------------------------------------------------------------------------------

function TerrainEditorPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
	%this.initBinds();
	Lab.terrainMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		barTitle = "Terrain";
		item[0] = "Smooth Heightmap" TAB "" TAB "ETerrainEditor.onSmoothHeightmap();";
	};
	TerrainEditorPlugin.setParam("BrushSize",TerrainEditorPlugin.defaultBrushSize);
	TerrainEditorPlugin.setParam("BrushPressure",TerrainEditorPlugin.defaultBrushPressure);
	TerrainEditorPlugin.setParam("BrushSoftness",TerrainEditorPlugin.defaultBrushSoftness);
	TerrainEditorPlugin.setParam("BrushSetHeight",TerrainEditorPlugin.defaultBrushSetHeight);
}

function TerrainEditorPlugin::onActivated( %this ) {
	Parent::onActivated( %this );
	ETerrainEditor.switchAction( %action );
	EWToolsPaletteArray.findObjectByInternalName( %action, true ).setStateOn( true );
	EWTerrainEditToolbar-->ellipse.performClick(); // Circle Brush
	//Lab.menuBar.insert( , Lab.menuBar.dynamicItemInsertPos );
// Add our menu.
	Lab.insertDynamicMenu(Lab.terrainMenu);
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EWTerrainEditToolbar.setVisible( true );
	ETerrainEditor.onBrushChanged();
	ETerrainEditor.setup();
	TerrainEditorPlugin.syncBrushInfo();
	EditorGuiStatusBar.setSelection("");
	
}

function TerrainEditorPlugin::onDeactivated( %this ) {
	Parent::onDeactivated( %this );
	endToolTime("TerrainEditor");
	EWTerrainEditToolbar.setVisible( false );
	ETerrainEditor.setVisible( false );
	Lab.removeDynamicMenu(Lab.terrainMenu);
	
}

function TerrainEditorPlugin::syncBrushInfo( %this ) {
	// Update gui brush info
	TerrainBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
	TerrainBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
	TerrainBrushSoftnessTextEditContainer-->textEdit.text = ETerrainEditor.getBrushSoftness()*100;
	TerrainSetHeightTextEditContainer-->textEdit.text = ETerrainEditor.setHeightVal;
	%brushType = ETerrainEditor.getBrushType();
	eval( "EWTerrainEditToolbar-->" @ %brushType @ ".setStateOn(1);" );
}

function TerrainEditorPlugin::validateBrushSize( %this ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	%val = $ThisControl.getText();

	if(%val < %minBrushSize)
		$ThisControl.setValue(%minBrushSize);
	else if(%val > %maxBrushSize)
		$ThisControl.setValue(%maxBrushSize);
}

function TerrainEditorPlugin::keyboardModifyBrushSize( %this, %amt) {
	%val = TerrainBrushSizeTextEditContainer-->textEdit.getText();
	%val += %amt;
	TerrainBrushSizeTextEditContainer-->textEdit.setValue(%val);
	TerrainBrushSizeTextEditContainer-->textEdit.forceValidateText();
	ETerrainEditor.setBrushSize( TerrainBrushSizeTextEditContainer-->textEdit.getText() );
}

//------------------------------------------------------------------------------
// TerrainTextureEditorTool
//------------------------------------------------------------------------------
//REMOVEME Seem to be unused
/*
function TerrainTextureEditorTool::onActivated( %this ) {
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EditorGui-->TextureEditor.setVisible(true);
	EditorGuiStatusBar.setSelection("");
}

function TerrainTextureEditorTool::onDeactivated( %this ) {
	EditorGui-->TextureEditor.setVisible(false);
	ETerrainEditor.setVisible( false );
}
*/
