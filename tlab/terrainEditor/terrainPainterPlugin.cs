//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function TerrainPainterPlugin::initParamsArray( %this,%cfgArray ) {
	%gid = 1;
	%cfgArray.group[%gid] = "Action values";
	%cfgArray.setVal("maxBrushSize",       "40 40" TAB "maxBrushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("BrushSize",       "2" TAB "brushSize" TAB "TextEdit" TAB "" TAB "TPP_BrushManager.validateBrushSize(**);" TAB %gid);
	%cfgArray.setVal("BrushType",       "box" TAB "brushType" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushType(**);" TAB %gid);
	%cfgArray.setVal("BrushPressure",       "1" TAB "brushPressure" TAB "TextEdit" TAB "" TAB "TPP_BrushManager.validateBrushPressure(**);" TAB %gid);
	%cfgArray.setVal("BrushSoftness",       "1" TAB "brushSoftness" TAB "TextEdit" TAB "" TAB "TPP_BrushManager.validateBrushSoftness(**);" TAB %gid);
	%cfgArray.setVal("BrushSlopeMin",       "1" TAB "BrushSlopeMin" TAB "TextEdit" TAB "" TAB "TPP_BrushManager.validateBrushSlopeMin(**);" TAB %gid);
	%cfgArray.setVal("BrushSlopeMax",       "1" TAB "BrushSlopeMax" TAB "TextEdit" TAB "" TAB "TPP_BrushManager.validateBrushSlopeMax(**);" TAB %gid);
	%gid = 2;
	%cfgArray.group[%gid] = "Action values";
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
}

//------------------------------------------------------------------------------
// TerrainPainterPlugin
//------------------------------------------------------------------------------

function TerrainPainterPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
	%map = new ActionMap();
	%map.bindCmd( keyboard, "v", "EWTerrainPainterToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
	%map.bindCmd( keyboard, "b", "EWTerrainPainterToolbarBrushType->box.performClick();", "" );// Box Brush
	%map.bindCmd( keyboard, "=", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "+", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "-", "TerrainPainterPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
	%map.bindCmd( keyboard, "[", "TerrainPainterPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
	%map.bindCmd( keyboard, "]", "TerrainPainterPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
	/*%map.bindCmd( keyboard, "]", "PaintBrushSlopeControl->SlopeMinAngle.text += 5", "" );// +5 SlopeMinAngle
	%map.bindCmd( keyboard, "[", "PaintBrushSlopeControl->SlopeMinAngle.text -= 5", "" );// -5 SlopeMinAngle
	%map.bindCmd( keyboard, "'", "PaintBrushSlopeControl->SlopeMaxAngle.text += 5", "" );// +5 SlopeMaxAngle
	%map.bindCmd( keyboard, ";", "PaintBrushSlopeControl->SlopeMaxAngle.text -= 5", "" );// -5 Softness*/

	for(%i=1; %i<10; %i++) {
		%map.bindCmd( keyboard, %i, "TerrainPainterPlugin.keyboardSetMaterial(" @ (%i-1) @ ");", "" );
	}

	%map.bindCmd( keyboard, 0, "TerrainPainterPlugin.keyboardSetMaterial(10);", "" );
	TerrainPainterPlugin.map = %map;
	//TPP_BrushManager.validateBrushSlopeMin(%this.defaultBrushSlopeMin);
}

function TerrainPainterPlugin::onActivated( %this ) {
	Parent::onActivated( %this );
	EWTerrainPainterToolbarBrushType->ellipse.performClick();// Circle Brush
	
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EditorGui-->TerrainPainter.setVisible(true);
	EditorGui-->TerrainPainterPreview.setVisible(true);
	EWTerrainPainterToolbar.setVisible(true);
	ETerrainEditor.onBrushChanged();
	EPainter.onActivated();
	TerrainPainterPlugin.syncBrushInfo();
	EditorGuiStatusBar.setSelection("");
}

function TerrainPainterPlugin::onDeactivated( %this ) {
	Parent::onDeactivated( %this );
	
	//EditorGui-->TerrainPainter.setVisible(false);
	//EditorGui-->TerrainPainterPreview.setVisible(false);
	//EWTerrainPainterToolbar.setVisible(false);
	//ETerrainEditor.setVisible( false );
}

function TerrainPainterPlugin::syncBrushInfo( %this ) {
	// Update gui brush info
	PaintBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
	PaintBrushSlopeControl-->SlopeMinAngle.text = ETerrainEditor.getSlopeLimitMinAngle();
	PaintBrushSlopeControl-->SlopeMaxAngle.text = ETerrainEditor.getSlopeLimitMaxAngle();
	PaintBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
	%brushType = ETerrainEditor.getBrushType();
	eval( "EWTerrainPainterToolbar-->" @ %brushType @ ".setStateOn(1);" );
}



function TerrainPainterPlugin::keyboardModifyBrushSize( %this, %amt) {
	%val = PaintBrushSizeTextEditContainer-->textEdit.getText();
	%val += %amt;
	PaintBrushSizeTextEditContainer-->textEdit.setValue(%val);
	PaintBrushSizeTextEditContainer-->textEdit.forceValidateText();
	ETerrainEditor.setBrushSize( PaintBrushSizeTextEditContainer-->textEdit.getText() );
}

function TerrainPainterPlugin::keyboardSetMaterial( %this, %mat) {
	%name = "EPainterMaterialButton" @ %mat;
	%ctrl = EPainter.findObjectByInternalName(%name, true);

	if(%ctrl) {
		%ctrl.performClick();
	}
}
