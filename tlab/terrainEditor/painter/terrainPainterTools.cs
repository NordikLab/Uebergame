//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//TerrainPainterTools.browseDefaultTexturesFolder();
$TPT_SubMaterialSuffix = "_1";
//==============================================================================
function TerrainPainterTools::browseDefaultTexturesFolder( %this) {
	devLog("Set default terrain textures folder to:",%folder);
	%startFrom = %this.defaultTexturesFolder;

	if (%startFrom $= "")
		%startFrom = "art/";

	getFolderName("","TerrainPainterTools.onSelectDefaultTexturesFolder",%startFrom);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainPainterTools::onSelectDefaultTexturesFolder( %this, %folder) {
	devLog("Set default terrain textures folder to:",%folder);
	%this.defaultTexturesFolder = %folder;
	%this-->defaultTexturesFolder.setText(%folder);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainPainterTools::setDefaultTexturesFolder( %this, %folder) {
	devLog("Set default terrain textures folder to:",%folder);
	%this.defaultTexturesFolder = %folder;
	%this-->defaultTexturesFolder.setText(%folder);
}
//------------------------------------------------------------------------------

//==============================================================================
// Create GroundCover Terrain Material CLone
//==============================================================================
//==============================================================================
//TerrainPainterTools.createGroundCoverMaterial(TerrainMaterialDlg.activeMat);
function TerrainPainterTools::createGroundCoverMaterial( %this,%baseMaterial,%suffix) {
	if (!%baseMaterial.isMemberOfClass("TerrainMaterial")) {
		warnLog("GroundCover material must be base on a TerrainMaterial! BaseMaterial received:", %baseMaterial);
		return;
	}

	%baseName = %baseMaterial.name;

	if (%baseName $= "") {
		%baseName = getUniqueName("TMat_"@%baseMaterial.internalName);
		warnLog("Base material must have a name set so the new layer can be linked to it. Generating new name:",%baseName);
		%baseMaterial.setName(%baseName);
		ETerrainMaterialPersistMan.setDirty( %baseMaterial);
		ETerrainMaterialPersistMan.saveDirtyObject( %baseMaterial);
	}

	if (%suffix $= "")
		%suffix = TerrainMaterialDlg-->subMaterialSuffix.getText();

	if (%suffix $= "")
		%suffix = "_1";

	devLog("Creating sub mat with suffix:",%suffix);
	%newName = getUniqueName(%baseName@%suffix);
	%intName = %baseMaterial.internalName@%suffix;
	%matName = getUniqueInternalName( %intName, TerrainMaterialSet, true );
	%fileObj = getFileWriteObj(%baseMaterial.getFilename(),true);
	%fileObj.writeLine("//==============================================================================");
	%fileObj.writeLine("// "@%matName@" - Layer for ground cover based on:" SPC %baseMaterial.internalName);
//	%fileObj.writeLine("new TerrainMaterial("@%newName@" : "@%baseName@")");
//	%fileObj.writeLine("{");
//	%fileObj.writeLine("   internalName = "@%intName@";");
//	%fileObj.writeLine("}");
//	%fileObj.writeLine("//------------------------------------------------------------------------------");
	closeFileObj(%fileObj);
	eval("%newMat = new TerrainMaterial("@%newName@" : "@%baseName@"){	internalName = "@%intName@";};");
	%newMat.setFilename(%baseName.getFilename());
	%newMat.matSource = %baseName;
	ETerrainMaterialPersistMan.setDirty( %newMat);
	ETerrainMaterialPersistMan.saveDirtyObject( %newMat);
	FilteredTerrainMaterialsSet.add(%newMat);
	TerrainMaterialDlg.refreshMaterialTree(%newMat);
	%matLibTree = TerrainMaterialDlg-->matLibTree;
}
//------------------------------------------------------------------------------