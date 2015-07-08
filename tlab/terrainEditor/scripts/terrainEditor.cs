//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

/// The texture filename filter used with OpenFileDialog.
$TerrainEditor::TextureFileSpec = "Image Files (*.png, *.jpg, *.dds)|*.png;*.jpg;*.dds|All Files (*.*)|*.*|";




function TerrainEditor::init( %this ) {
	%this.attachTerrain();
	%this.setBrushSize( 9, 9 );
	new PersistenceManager( ETerrainPersistMan );
}


function EPainter_TerrainMaterialUpdateCallback( %mat, %matIndex ) {
	// Skip over a bad selection.
	if ( %matIndex == -1 || !isObject( %mat ) )
		return;

	// Update the material and the UI.
	ETerrainEditor.updateMaterial( %matIndex, %mat.getInternalName() );
	//EPainter.setup( %matIndex );
	EPainter.updateLayers();
}

function EPainter_TerrainMaterialAddCallback( %mat, %matIndex ) {
	// Ignore bad materials.
	if ( !isObject( %mat ) )
		return;

	// Add it and update the UI.
	ETerrainEditor.addMaterial( %mat.getInternalName() );
	//EPainter.setup( %matIndex );
	EPainter.updateLayers();
}

function TerrainEditor::setPaintMaterial( %this, %matIndex, %terrainMat ) {
	assert( isObject( %terrainMat ), "TerrainEditor::setPaintMaterial - Got bad material!" );
	ETerrainEditor.paintIndex = %matIndex;
	ETerrainMaterialSelected.selectedMatIndex = %matIndex;
	ETerrainMaterialSelected.selectedMat = %terrainMat;
	ETerrainMaterialSelected.bitmap = %terrainMat.diffuseMap;
	ETerrainMaterialSelected_N.bitmap = %terrainMat.normalMap;
	ETerrainMaterialSelected_M.bitmap = %terrainMat.macroMap;
	ETerrainMaterialSelectedEdit.Visible = isObject(%terrainMat);
	TerrainTextureText.text = %terrainMat.getInternalName();
	ProceduralTerrainPainterDescription.text = "Generate "@ %terrainMat.getInternalName() @" layer";
}

function TerrainEditor::setup( %this ) {
	%action = %this.savedAction;
	%desc = %this.savedActionDesc;

	if ( %this.savedAction $= "" ) {
		%action = brushAdjustHeight;
	}

	%this.switchAction( %action );
}


function onNeedRelight() {
	if( RelightMessage.visible == false )
		RelightMessage.visible = true;
}

function TerrainEditor::onGuiUpdate(%this, %text) {
	%minHeight = getWord(%text, 1);
	%avgHeight = getWord(%text, 2);
	%maxHeight = getWord(%text, 3);
	%mouseBrushInfo = " (Mouse) #: " @ getWord(%text, 0) @ "  avg: " @ %avgHeight @ " " @ ETerrainEditor.currentAction;
	%selectionInfo = "     (Selected) #: " @ getWord(%text, 4) @ "  avg: " @ getWord(%text, 5);
	TEMouseBrushInfo.setValue(%mouseBrushInfo);
	TESelectionInfo.setValue(%selectionInfo);
	EditorGuiStatusBar.setSelection("min: " @ %minHeight @ "  avg: " @ %avgHeight @ "  max: " @ %maxHeight);
}


function TerrainEditor::onActiveTerrainChange(%this, %newTerrain) {
	// Need to refresh the terrain painter.
	if ( Lab.currentEditor.getId() == TerrainPainterPlugin.getId() )
		EPainter.setup(ETerrainEditor.paintIndex);
}



//------------------------------------------------------------------------------
// Functions
//------------------------------------------------------------------------------

function TerrainEditorSettingsGui::onWake(%this) {
	TESoftSelectFilter.setValue(ETerrainEditor.softSelectFilter);
}

function TerrainEditorSettingsGui::onSleep(%this) {
	ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
}

function TESettingsApplyButton::onAction(%this) {
	ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
	ETerrainEditor.resetSelWeights(true);
	ETerrainEditor.processAction("softSelect");
}

function getPrefSetting(%pref, %default) {
	//
	if(%pref $= "")
		return(%default);
	else
		return(%pref);
}

function TerrainEditorPlugin::setEditorFunction(%this) {
	%terrainExists = parseMissionGroup( "TerrainBlock" );

	if( %terrainExists == false )
		LabMsgYesNoCancel("No Terrain","Would you like to create a New Terrain?", "Canvas.pushDialog(CreateNewTerrainGui);");

	return %terrainExists;
}

function TerrainPainterPlugin::setEditorFunction(%this, %overrideGroup) {
	%terrainExists = parseMissionGroup( "TerrainBlock" );

	if( %terrainExists == false )
		LabMsgYesNoCancel("No Terrain","Would you like to create a New Terrain?", "Canvas.pushDialog(CreateNewTerrainGui);");

	return %terrainExists;
}

function EPainterIconBtn::onMouseDragged( %this ) {
	%payload = new GuiControl() {
		profile = ToolsButtonArray;
		position = "0 0";
		extent = %this.extent.x SPC "5";
		dragSourceControl = %this;
	};
	%xOffset = getWord( %payload.extent, 0 ) / 2;
	%yOffset = getWord( %payload.extent, 1 ) / 2;
	%cursorpos = Canvas.getCursorPos();
	%xPos = getWord( %cursorpos, 0 ) - %xOffset;
	%yPos = getWord( %cursorpos, 1 ) - %yOffset;
	// Create the drag control.
	%ctrl = new GuiDragAndDropControl() {
		canSaveDynamicFields    = "0";
		Profile                 = EPainterDragDropProfile;
		HorizSizing             = "right";
		VertSizing              = "bottom";
		Position                = %xPos SPC %yPos;
		extent                  = %payload.extent;
		MinExtent               = "4 4";
		canSave                 = "1";
		Visible                 = "1";
		hovertime               = "1000";
		deleteOnMouseUp         = true;
	};
	%ctrl.add( %payload );
	Canvas.getContent().add( %ctrl );
	%ctrl.startDragging( %xOffset, %yOffset );
}

function EPainterIconBtn::onControlDragged( %this, %payload ) {
	%payload.getParent().position = %this.getGlobalPosition();
}

function EPainterIconBtn::onControlDropped( %this, %payload ) {
	%srcBtn = %payload.dragSourceControl;
	%dstBtn = %this;
	%stack = %this.getParent();

	// Not dropped on a valid Button.
	// Really this shouldnt happen since we are in a callback on our specialized
	// EPainterIconBtn namespace.
	if ( %stack != %dstBtn.getParent() || %stack != EPainterStack.getId() ) {
		echo( "Not dropped on valid control" );
		return;
	}

	// Dropped on the original control, no order change.
	// Simulate a click on the control, instead of a drag/drop.
	if ( %srcBtn == %dstBtn ) {
		%dstBtn.performClick();
		return;
	}

	%dstIndex = %stack.getObjectIndex( %dstBtn );
	ETerrainEditor.reorderMaterial( %stack.getObjectIndex( %srcBtn ), %dstIndex );
	// select the button/material we just reordered.
	%stack.getObject( %dstIndex ).performClick();
}