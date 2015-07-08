//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$EPainter_DisplayMode = 0;
//==============================================================================
// TerrainPainterPlugin
//==============================================================================

function EPainter::onActivated( %this ) {
	hide(EPainter_LayerSrc);
	show(EPainterStack);
	hide(EPainter_LayerCompactSrc);
	hide(EPainter_LayerMixedSrc);
	EPainter_DisplayMode.clear();
	EPainter_DisplayMode.add("Compact listing",0);
	EPainter_DisplayMode.add("Extended listing",1);
	EPainter_DisplayMode.add("Mixed listing",2);
	EPainter_DisplayMode.setSelected($EPainter_DisplayMode,false);
	// Update the layer listing.
	%this.updateLayers( %matIndex );
	// Automagically put us into material paint mode.
	ETerrainEditor.currentMode = "paint";
	ETerrainEditor.selectionHidden = true;
	ETerrainEditor.currentAction = paintMaterial;
	ETerrainEditor.currentActionDesc = "Paint material on terrain";
	ETerrainEditor.setAction( ETerrainEditor.currentAction );
	EditorGuiStatusBar.setInfo(ETerrainEditor.currentActionDesc);
	ETerrainEditor.renderVertexSelection = true;
	EPainter-->saveDirtyMaterials.active = 0;
}
function EPainter_DisplayMode::onSelect( %this,%id,%text ) {
	if ($EPainter_DisplayMode $= %id)
		return;

	$EPainter_DisplayMode = %id;
	EPainter.updateLayers();
}

//==============================================================================
// Painter Active Layers Functions
//==============================================================================
$PainterMatFields = "detailMap detailSize detailStrength detailDistance macroMap macroSize macroStrength macroDistance diffuseSize diffuseMap";
//==============================================================================
// Update the active material layers list
function EPainter::updateLayers( %this, %matIndex ) {
	// Default to whatever was selected before.
	if ( %matIndex $= "" )
		%matIndex = ETerrainEditor.paintIndex;

	// The material string is a newline seperated string of
	// TerrainMaterial internal names which we can use to find
	// the actual material data in TerrainMaterialSet.
	%mats = ETerrainEditor.getMaterials();

	if ($EPainter_DisplayMode $= "1")
		%ctrlSrc = EPainter_LayerSrc;
	else if ($EPainter_DisplayMode $= "2")
		%ctrlSrc = EPainter_LayerMixedSrc;
	else
		%ctrlSrc = EPainter_LayerCompactSrc;

	// %listWidth = getWord( EPainterStack.getExtent(), 0 );
	hide(EPainter_LayerSrc);
	hide(EPainter_LayerCompactSrc);
	hide(EPainter_LayerMixedSrc);
	show(EPainterStack);
	EPainterStack.clear();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

		// Is there no material info for this slot?
		if ( !isObject( %mat ) )
			continue;

		%index = EPainterStack.getCount();
		%command = "EPainter.setPaintLayer( " @ %index @ " );";
		%altCommand = "TerrainMaterialDlg.show( " @ %index @ ", " @ %mat @ ", EPainter_TerrainMaterialUpdateCallback );";
		%ctrl = cloneObject(%ctrlSrc,"","Layer_"@%index,EPainterStack);
		%ctrl.terrainMat = %mat;
		%ctrl.layerId = %i;
		%bitmapButton = %ctrl-->bitmapButton;
		//%bitmapButton.internalName = "EPainterMaterialButton" @ %i;
		%bitmapButton.command = %command;
		%bitmapButton.altCommand = %altCommand;
		%bitmapButton.setBitmap( %mat.diffuseMap );
		%editButton = %ctrl-->editButton;
		%editButton.command =  "TerrainMaterialDlg.show( " @ %index @ ", " @ %mat @ ", EPainter_TerrainMaterialUpdateCallback );";
		%deleteButton = %ctrl-->deleteButton;
		%deleteButton.command = "EPainter.showMaterialDeleteDlg( " @ %matInternalName @ " );";
		%ctrl-->matName.text = %matInternalName;
		%ctrl.isActiveCtrl = %ctrl-->ctrlActive;
		%ctrl.isActiveCtrl.visible = 0;
		%mouseEvent = %ctrl-->mouseEvent;
		%mouseEvent.command = %command;
		%mouseEvent.altCommand = %altCommand;
		%mouseEvent.superClass = "PainterLayerMouse";
		%mouseEvent.baseCtrl = %ctrl;
		%ctrl.mouseEvent = %ctrl-->mouseEvent;
		%ctrl-->dropLayer.layerIndex = %index;
		%ctrl-->dropLayer.layerId = %i;
		%ctrl-->dropLayer.visible = 0;
		%ctrl.dropLayer = %ctrl-->dropLayer;

		if ($EPainter_DisplayMode $= "1") {
			foreach$(%field in $PainterMatFields) {
				%fieldCtrl = %ctrl.findObjectByInternalName(%field,true);
				%fieldCtrl.setValue(%mat.getFieldValue(%field));
				%fieldCtrl.mat = %mat;
				%fieldCtrl.superClass = "PainterLayerEdit";
				%fieldCtrl.nameCtrl = %ctrl-->matName;
			}
		} else if ($EPainter_DisplayMode $= "2") {
			%ctrl-->extendButton.baseCtrl = %ctrl;
			%compactCtrl = %ctrl-->compactCtrl;
			%compactCtrl-->matName.text = %matInternalName;
			%bitmapButtonCompact = %compactCtrl-->bitmapButton;
		//	%bitmapButtonCompact.internalName = "EPainterMaterialButton" @ %i;
			%bitmapButtonCompact.command = %command;
			%bitmapButtonCompact.altCommand = %altCommand;
			//%bitmapButtonCompact.setBitmap( %mat.diffuseMap );
			%mouseEvent = %compactCtrl-->mouseEvent;
			%mouseEvent.command = %command;
			%mouseEvent.altCommand = %altCommand;
			%mouseEvent.superClass = "PainterLayerMouse";
			%mouseEvent.baseCtrl = %ctrl;
			%mouseEvent.dragClone = %compactCtrl;
			%compactCtrl-->ctrlActive.visible = 0;
			%compactCtrl-->dropLayer.visible = 0;
			%extendedCtrl = %ctrl-->extendedCtrl;
			%extendedCtrl-->matName.text = %matInternalName;
			%extendedCtrl-->ctrlActive.visible = 0;
			%extendedCtrl-->dropLayer.visible = 0;
			%bitmapButtonExt = %extendedCtrl-->bitmapButton;
			//%bitmapButtonExt.internalName = "EPainterMaterialButton" @ %i;
			%bitmapButtonExt.command = %command;
			%bitmapButtonExt.altCommand = %altCommand;
			//%bitmapButtonExt.setBitmap( %mat.diffuseMap );
			%mouseEventExt = %extendedCtrl-->mouseEvent;
			%mouseEventExt.command = %command;
			%mouseEventExt.altCommand = %altCommand;
			%mouseEventExt.superClass = "PainterLayerMouse";
			%mouseEventExt.baseCtrl = %ctrl;
			%mouseEventExt.dragClone = %extendedCtrl;

			foreach$(%field in $PainterMatFields) {
				%fieldCtrl = %ctrl.findObjectByInternalName(%field,true);

				if (!isObject(%fieldCtrl))
					continue;

				%fieldCtrl.setValue(%mat.getFieldValue(%field));
				%fieldCtrl.mat = %mat;
				%fieldCtrl.superClass = "PainterLayerEdit";
				%fieldCtrl.nameCtrl = %ctrl-->matName;
			}

			%this.setMixedView(%ctrl,true);
		}

		%tooltip = %matInternalName;

		if(%i < 9)
			%tooltip = %tooltip @ " (" @ (%i+1) @ ")";
		else if(%i == 9)
			%tooltip = %tooltip @ " (0)";

		%bitmapButton.tooltip = %tooltip;
	}

	%matCount = EPainterStack.getCount();
	// Add one more layer as the 'add new' layer.
	%ctrl = new GuiIconButtonCtrl() {
		profile = "ToolsButtonDark";
		iconBitmap = "tlab/gui/icons/default/terrainpainter/new_layer_icon";
		iconLocation = "Left";
		textLocation = "Right";
		//extent = %listWidth SPC "46";
		textMargin = 5;
		buttonMargin = "4 4";
		buttonType = "PushButton";
		sizeIconToButton = true;
		makeIconSquare = true;
		tooltipprofile = "ToolsGuiToolTipProfile";
		text = "New Layer";
		tooltip = "New Layer";
		command = "TerrainMaterialDlg.show( " @ %matCount @ ", 0, EPainter_TerrainMaterialAddCallback );";
	};
	EPainterStack.add( %ctrl );

	// Make sure our selection is valid and that we're
	// not selecting the 'New Layer' button.

	if( %matIndex < 0 )
		return;

	if( %matIndex >= %matCount )
		%matIndex = 0;

	%activeIndex = ETerrainMaterialSelected.selectedMatIndex;

	if (%activeIndex $= "")
		%activeIndex = 0;

	%this.setPaintLayer(%activeIndex);
	// To make things simple... click the paint material button to
	// active it and initialize other state.
	%ctrl = EPainterStack.getObject( %matIndex );
	if (isObject(%ctrl))				
		%ctrl-->bitmapButton.performClick(); //FIXME something wrong here
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function EPainter::setPaintLayer( %this, %matIndex) {
	%ctrl = EPainterStack.findObjectByInternalName("Layer_"@%matIndex,true);
	%terrainMat = %ctrl.terrainMat;	
	%pill = EPainterStack.getObject(%matIndex);

	foreach(%ctrl in EPainterStack)
		%ctrl.isActiveCtrl.visible = 0;

	%pill.isActiveCtrl.visible = 1;

	if (!isObject( %terrainMat )) {
		warnLog("Set Paint Layer to invalid material:",%terrainMat);
		return;
	}

	//assert( isObject( %terrainMat ), "TerrainEditor::setPaintMaterial - Got bad material!" );
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

//------------------------------------------------------------------------------

//==============================================================================
// Update the active material layers list
function EPainterToggleMixedButton::onClick( %this) {
	%ctrl = %this.baseCtrl;
	EPainter.setMixedView(%ctrl,!%ctrl-->compactCtrl.isVisible());
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function EPainter::setMixedView( %this,%ctrl,%isCompact) {
	%compactCtrl = %ctrl-->compactCtrl;
	%compactCtrl.visible = %isCompact;
	%extendedCtrl = %ctrl-->extendedCtrl;
	%extendedCtrl.visible = !%isCompact;
	%compactCtrl-->dropLayer.visible = 0;
	%extendedCtrl-->dropLayer.visible = 0;

	if (%isCompact) {
		%ctrl.extent = %ctrl-->compactCtrl.extent;
		%ctrl.isActiveCtrl = %compactCtrl-->ctrlActive;
		%ctrl.dropLayer = %compactCtrl-->dropLayer;
	} else {
		%ctrl.isActiveCtrl = %extendedCtrl-->ctrlActive;
		%ctrl.extent = %ctrl-->extendedCtrl.extent;
		%ctrl.dropLayer = %extendedCtrl-->dropLayer;
	}

	%ctrl-->iconStack.AlignCtrlToParent("right");
	EPainterStack.updateStack();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerEdit::onValidate( %this) {
	devLog("PainterLayerEdit onValidate:: FIELD:",%this.internalName,"Mat:",%this.mat);
	%this.mat.setFieldValue(%this.internalName,%this.getText());
	EPainter.setMaterialDirty( %this.mat,%this.nameCtrl );
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDown( %this,%modifier,%pos,%clicks) {
	logd("PainterLayerMouse onMouseDown::",%modifier,%pos,%clicks);

	if (%clicks > 1)
		eval(%this.altCommand);
	else
		eval(%this.command);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::showMaterialDeleteDlg( %this, %matInternalName ) {
	LabMsgYesNo( "Confirmation",
					 "Really remove material '" @ %matInternalName @ "' from the terrain?",
					 %this @ ".removeMaterial( " @ %matInternalName @ " );", "" );
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::removeMaterial( %this, %matInternalName ) {
	%selIndex = ETerrainEditor.paintIndex - 1;
	// Remove the material from the terrain.
	%index = ETerrainEditor.getMaterialIndex( %matInternalName );

	if( %index != -1 )
		ETerrainEditor.removeMaterial( %index );

	// Update the material list.
	%this.updateLayers( %selIndex );
}
//------------------------------------------------------------------------------
//==============================================================================
// EPainter Set Dirty / Save materials
//==============================================================================
//==============================================================================
function EPainter::setMaterialDirty( %this,%mat,%nameCtrl ) {
	if (isObject(%nameCtrl)) {
		%nameCtrl.setText("*" SPC %mat.internalName);
		$PainterDirtyNames = strAddWord($PainterDirtyNames,%nameCtrl,true);
	}

	// Mark the material as dirty and needing saving.
	%fileName = %mat.getFileName();

	if( %fileName $= "" )
		%fileName = "art/terrains/materials.cs";

	ETerrainMaterialPersistMan.setDirty( %mat, %fileName );
	EPainter-->saveDirtyMaterials.active = 1;
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::saveDirtyMaterials( %this ) {
	ETerrainMaterialPersistMan.saveDirty();

	foreach$(%nameCtrl in $PainterDirtyNames) {
		%text= strreplace(%nameCtrl.text,"* ","");
		%nameCtrl.text = %text;
	}

	EPainter-->saveDirtyMaterials.active = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
// LAYER DRAG AND DROP SYSTEM
//==============================================================================
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDragged( %this,%modifier,%pos,%clicks) {
	%dragClone = %this.baseCtrl;

	if (isObject(%this.dragClone))
		%dragClone = %this.dragClone;

	%dragClone.layerId = %this.baseCtrl.layerId;

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 1;

	dragAndDropCtrl(%dragClone,"TerrainLayer","EPainter.layerDragFailed();");
}
//------------------------------------------------------------------------------

//==============================================================================
// Update the active material layers list
function LayerDropClass::onControlDropped(%this, %control, %dropPoint) {
	
	%droppedOnLayer = %this.layerID;
	%droppedLayer = %control.dragSourceControl.layerID;
	ETerrainEditor.reorderMaterial(%droppedLayer,%droppedOnLayer);
	EPainter.setPaintLayer(%droppedOnLayer);
	EPainter.updateLayers();

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//==============================================================================
// Update the active material layers list
function EPainter::layerDragFailed( %this,%modifier,%pos,%clicks) {	
	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//------------------------------------------------------------------------------
