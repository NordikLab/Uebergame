//==============================================================================
// TorqueLab -> Procedural Terrain Painter GUI script
// Copyright NordikLab Studio, 2014
//==============================================================================

$Lab::TerrainPainter::UseTerrainHeight = "1";
$Lab::TerrainPainter::ValidateHeight = "1";
$TPG_MaxHeight = 10000;
$TPG_MinHeight = -10000;
$TPG_Coverage = 100;
$TPPHeightMin = -10000;
$TPPHeightMax = 10000;
$TPPSlopeMin = 0;
$TPPSlopeMax = 90;
$TerrainPaintGeneratorGui_Initialized = false;
//==============================================================================
// Multi material automatic terrain painter
//==============================================================================

//==============================================================================
function TerrainPaintGeneratorGui::onWake(%this) {
	if (!$TerrainPaintGeneratorGui_Initialized)
		TPG.init();

	TPG_GenerateProgressWindow.setVisible(false);
	%this-->rolloutSettings.expanded = false;
	TPG_PillSample.setVisible(false);
	%menu = %this-->MenuTerrainLayers;
	%menu.clear();
	%mats = ETerrainEditor.getMaterials();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
		$TerrainPainterMatId[%matInternalName] = %i;
		%menu.add(%matInternalName,%i);
	}

	%menu.setSelected(0);
	%matList = ETerrainEditor.getMaterials();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

		// Is there no material info for this slot?
		if ( !isObject( %mat ) )
			continue;

		%this.updatePillLayerList(%mat);
	}

	%this.checkHeightDefaults();
	TPG.checkLayersStackGroup();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::checkHeightDefaults(%this) {
	Lab.getTerrainHeightRange();
	%this-->terrainHeight.setText($Lab_TerrainHeight);
	$TPG_MaxHeight =  $Lab_TerrainHeight;
	$TPG_MinHeight =  0;
	$TPG_TerrainZ =  $Lab_TerrainZ;

	if (!$Lab::TerrainPainter::UseTerrainHeight) {
		$TPG_MaxHeight += $Lab_TerrainZ;
		$TPG_MinHeight += $Lab_TerrainZ;
	}

	%this.checkLayersHeight();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::updatePillLayerList(%this,%layer) {
	TPG_PillSample.setVisible(false);
	%pill = %layer.pill;
	%menu = %pill-->menuLayers;
	%menu.clear();
	%mats = ETerrainEditor.getMaterials();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
		$TerrainPainterMatId[%matInternalName] = %i;
		%menu.add(%matInternalName,%i);
	}

	%id = $TerrainPainterMatId[%layer.matInternalName];

	if (%id $= "") %id = 0;

	%menu.setSelected(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::checkLayersHeight(%this) {
	foreach(%layer in TPG_LayerGroup) {
		%this.checkHeightSystem(%layer);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::checkHeightSystem(%this,%layer) {
	if (%layer.useTerrainHeight != $Lab::TerrainPainter::UseTerrainHeight) {
		//Need to convert Heights
		if (%layer.useTerrainHeight) {
			%layer.heightMin = %layer.heightMin + $Lab_TerrainZ;
			%layer.heightMax = %layer.heightMax + $Lab_TerrainZ;
			%layer.useTerrainHeight = 0;
			echo("Layer:" SPC %layer.matInternalName SPC " changed to World Heights");
		} else {
			%layer.heightMin = %layer.heightMin - $Lab_TerrainZ;
			%layer.heightMax = %layer.heightMax - $Lab_TerrainZ;
			%layer.useTerrainHeight = 1;
			echo("Layer:" SPC %layer.matInternalName SPC " changed to Terrain Heights");
		}

		%this.updateLayerPill(%layer);
	}

	%this.validateHeight(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::validateHeight(%this,%layer) {
	if (!$Lab::TerrainPainter::ValidateHeight) return;

	if (%layer.heightMin < $TPG_MinHeight) %layer.heightMin = $TPG_MinHeight;

	if (%layer.heightMax > $TPG_MaxHeight) %layer.heightMax = $TPG_MaxHeight;

	if (%layer.heightMin > %layer.heightMax) %layer.heightMin = %layer.heightMax;

	%this.updateLayerPill(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::addNewLayer(%this) {
	%layer = new ScriptObject();
	%layer.internalName = "Layer_"@%this.lastLayerId++;
	%layer.heightMin = $TPG_MinHeight;
	%layer.heightMax = $TPG_MaxHeight;
	%layer.slopeMin = "0";
	%layer.slopeMax = "90";
	%layer.coverage = $TPG_Coverage;
	%layer.useTerrainHeight = $Lab::TerrainPainter::UseTerrainHeight;
	%matId = 0;
	%this.addLayerPill(%layer,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::layerSelected(%this,%menu) {
	%matInternalName = %menu.getText();
	%matIndex = %menu.getSelected();
	%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
	%layer = %menu.layerObj;
	%layer.matObject = %mat;
	%layer.matInternalName = %matInternalName;
	%layer.matIndex = %matIndex;
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainPaintGeneratorGui::addLayerPill(%this,%layer,%update) {
	if (!isObject(%layer)) return;

	%srcPill = TPG_PillSample;
	%layer.pill = %srcPill.deepClone();
	%layer.pill.setName("");
	%layer.pill.setVisible(true);
	%layer.pill-->menuLayers.layerObj = %layer;
	%layer.pill-->edit.layerObj = %layer;
	%layer.pill-->delete.layerObj = %layer;
	%layer.pill-->up.pill = %layer.pill;
	%layer.pill-->down.pill = %layer.pill;
	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.layerObj = %layer;
	%layer.pill-->heightMax.layerObj = %layer;
	%layer.pill-->slopeMin.layerObj = %layer;
	%layer.pill-->slopeMax.layerObj = %layer;
	%layer.pill-->coverage.layerObj = %layer;
	%layer.pill-->overlayCheck.layerObj = %layer;
	%layer.pill-->overlayCheck.setStateOn(false);
	%layer.pill-->overlayCheck.text = "";
	%layer.activeCtrl = %layer.pill-->checkbox;
	%layer.activeCtrl.text = "";
	%layer.activeCtrl.setStateOn(true);
	TPG_StackLayers.add(%layer.pill);
	%this.updatePillLayerList(%layer);

	if (%update)
		%this.updateLayerPill(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::updateLayerPill(%this,%layer) {
	if (!isObject(%layer) || !isObject(%layer.pill)) return;

	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.setText(%layer.heightMin);
	%layer.pill-->heightMax.setText( %layer.heightMax);
	%layer.pill-->slopeMin.setText(%layer.slopeMin);
	%layer.pill-->slopeMax.setText(%layer.slopeMax);
	%layer.pill-->coverage.setText(%layer.coverage);
	TPG_LayerGroup.add(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::cancelLayerChanges(%this) {
	TPG_LayerWindow.setVisible(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveUp(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex <= 0) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex - 1);
	TPG_StackLayers.reorderChild(%buttonCtrl.pill, %targetPill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveDown(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex > TPG_StackLayers.getCount() - 1) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex + 1);
	TPG_StackLayers.reorderChild(%targetPill, %buttonCtrl.pill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG_PillEdit::onValidate(%this) {	
	TPG.validateLayerSetting(%this.internalName,%this.layerObj);
}
//------------------------------------------------------------------------------


//==============================================================================
function TerrainPaintGeneratorGui::editLayer(%this, %buttonCtrl) {
	%this.openLayerSettings(%buttonCtrl.layerObj);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::deleteLayer(%this, %buttonCtrl) {
	%layer = %buttonCtrl.layerObj;
	TPG_LayerGroup.remove(%layer);
	%buttonCtrl.layerObj.pill.delete();
	%layer.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::deleteLayerGroup(%this) {
	TPG_LayerGroup.clear();
	TPG_StackLayers.clear();
}
//------------------------------------------------------------------------------


//==============================================================================
function TPG_EditTerrainHeight::onValidate(%this) {
	theTerrain.terrainHeight = %this.getText();
	TerrainPaintGeneratorGui.checkHeightDefaults();
}
//------------------------------------------------------------------------------

//==============================================================================
//OLD STUFF
//==============================================================================

//==============================================================================
function autoLayers() {
	Canvas.pushDialog(TerrainPaintGeneratorGui);
}
//------------------------------------------------------------------------------
//==============================================================================
function generateProceduralTerrainMask() {
	Canvas.popDialog(TerrainPaintGeneratorGui);
	ETerrainEditor.autoMaterialLayer($TPPHeightMin, $TPPHeightMax, $TPPSlopeMin, $TPPSlopeMax);
}
//------------------------------------------------------------------------------
/*
//==============================================================================
function TerrainPaintGeneratorGui::onWake(%this)
{

   %this.tree = ProceduralPainterLayers;

   ProceduralPainterLayers.open( AutoPaintTree );
	ProceduralPainterLayers.buildVisibleTree(true);
  %this.UpdateLayerList();

}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::init(%this)
{
   if ( !isObject( AutoPaintTree ) ) {
		new SimGroup( AutoPaintTree  );
		%this.showError = true;
	}

	 if ( !isObject( AutoPaintGroupMain ) ) {
      new SimGroup(AutoPaintGroupMain) {
         internalName = "MainGroup";
         parentGroup = AutoPaintTree;
      };
	 }

	ProceduralPainterLayers.open( AutoPaintTree );
	ProceduralPainterLayers.buildVisibleTree(true);

}
//------------------------------------------------------------------------------
//==============================================================================
// Multi Material Procedural Painter
//==============================================================================
//==============================================================================
function TerrainPaintGeneratorGui::AddNewLayerGroup(%this)
{

   %tree = %this.tree;

   %this.lastGroupId++;

   %internalName = "LayerGroup_"@%this.lastGroupId++;
	%group = new SimGroup() {
		internalName = %internalName;
		parentGroup = AutoPaintTree;
	};
	//MECreateUndoAction::submit( %group );
	%tree.open( AutoPaintLayerGroup );
	%tree.buildVisibleTree(true);
	%item = %tree.findItemByObjectId( %group );
   %tree.clearSelection();
	%tree.addSelection( %item );
	%tree.scrollVisible( %item );
	//%tree.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::UpdateLayerList(%this)
{
   %menu = %this-->MenuTerrainLayers;
   %menu.clear();
  %mats = ETerrainEditor.getMaterials();
  for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
     %matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
		%menu.add(%matInternalName,%i);
  }
  %menu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::layerSelected(%this,%menu)
{
   %matInternalName = %menu.getText();
  %mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
   %this.selectedMaterial = %mat;
    %this.selectedLayer.layerMaterial = %mat;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::addLayerToList(%this)
{

   %tree = ProceduralPainterLayers;
   %layer = %this.selectedLayer;
   %layer.layerName = PPainterLayerSelect-->layerName.getText();
    %mat  = %layer.layerMaterial;

   if (!isObject(%layer)){
      error("No selected layer , can't add new layer");
		return;
	}

  %parentGroup = %tree.getSelectedObject();
	if ( !isObject( %parentGroup ) ){
      error("No layer group selected, can't add new layer");
		return;
	}


	%name = getUniqueInternalName( %layer.internalName, AutoPaintLayerGroup, true );
	%layer.internalName = %layer.internalName @"_"@ %layer.layerName@"_"@%mat.getName();
	%layer.parentGroup =  %parentGroup;

	//MECreateUndoAction::submit( %element );
	%tree.clearSelection();
	%tree.buildVisibleTree( true );
	%item = %tree.findItemByObjectId( %layer.getId() );
	%tree.scrollVisible( %item );
	%tree.addSelection( %item );
	%tree.dirty = true;


}
//------------------------------------------------------------------------------


//==============================================================================
function TerrainPaintGeneratorGui::updateSelectedLayer(%this,%layer)
{

    PPainterLayerSelect-->updateButton.text = "Update layer";
    if (!isObject(%layer))
    {

       %internalName = "Layer_"@%this.lastLayerId++;


      %layer = new ScriptObject() {
		   internalName = %internalName;
		   layerName = "New Layer";
		   minHeight = "-1000";
		    maxHeight = "1000";
		   minSlope = "0";
		   maxSlope = "90";
	   };
	   PPainterLayerSelect-->updateButton.text = "Add to list";
    }
    %this.selectedLayer = %layer;
    PPainterLayerSelect-->layerName.text = %layer.layerName;
      PPainterLayerSelect-->minHeight.text = %layer.minHeight;
	PPainterLayerSelect-->maxHeight.text = %layer.maxHeight;
  PPainterLayerSelect-->minSlope.text = %layer.minSlope;
  PPainterLayerSelect-->maxSlope.text = %layer.maxSlope;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::generateMultiLayers(%this)
{


}
//------------------------------------------------------------------------------

//==============================================================================
// ProceduralPainterLayers TreeView callbacks
//==============================================================================
//==============================================================================
function ProceduralPainterLayers::onSelect(%this,%id)
{

      %layer = %this.getSelectedObject();
  TerrainPaintGeneratorGui.updateSelectedLayer(%id);
}
//------------------------------------------------------------------------------

*/