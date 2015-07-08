//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//CREATE PREFAB FROM SELECTION
//==============================================================================
//==============================================================================
// New Create Prefab System using Lab
function Lab::CreatePrefab(%this) {
	// Should this be protected or not?
	%autoMode = SceneEditorCfg.AutoCreatePrefab;

	if (%autoMode) {
		%saveFile = %this.GetAutoPrefabFile();
	} else if ( !$Pref::disableSaving && !isWebDemo() ) {
		%saveFile = %this.GetPrefabFile();
	}

	if (%saveFile $= "")
		return;

	EWorldEditor.makeSelectionPrefab( %saveFile );
	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
// New Create Prefab System using Lab
function Lab::GetAutoPrefabFile() {
	%missionFolder = filePath(MissionGroup.getFilename());
	%prefabPath = %missionFolder @"/prefabs/";
	%firstObj = EWorldEditor.getSelectedObject(0);

	if (%firstObj.isMemberOfClass(TSStatic)) {
		devLog("TSSTAtic shape=",%firstObj.shapeName);
		%name = fileBase(%firstObj.shapeName);
	}

	if (%name $= "") {
		if (%firstObj.getName() !$="")
			%name = %firstObj.getName();
		else if (%firstObj.internalName !$="")
			%name = %firstObj.internalName;
		else
			%name = %firstObj;
	}

	%file = %prefabPath@%name@".prefab";

	while (isFile(%file)) {
		%uniqueName = %name@"_"@%inc++;
		%file = %prefabPath@%uniqueName@".prefab";
	}

	devLog("AutoFile = ",%file);
	return %file;
}
//------------------------------------------------------------------------------
//==============================================================================
// New Create Prefab System using Lab
function Lab::GetPrefabFile() {
	%dlg = new SaveFileDialog() {
		Filters        = "Prefab Files (*.prefab)|*.prefab|";
		DefaultPath    = $Pref::WorldEditor::LastPath;
		DefaultFile    = "";
		ChangePath     = false;
		OverwritePrompt   = true;
	};
	%ret = %dlg.Execute();

	if ( %ret ) {
		$Pref::WorldEditor::LastPath = filePath( %dlg.FileName );
		%saveFile = %dlg.FileName;
	}

	if( fileExt( %saveFile ) !$= ".prefab" )
		%saveFile = %saveFile @ ".prefab";

	%dlg.delete();

	if ( !%ret )
		return "";

	return %saveFile;
}
//------------------------------------------------------------------------------

//==============================================================================
//EXPLODED SELECTED PREFAB
//==============================================================================
//==============================================================================
// New Create Prefab System using Lab
function Lab::ExplodePrefab(%this) {
	//echo( "EditorExplodePrefab()" );
	EWorldEditor.explodeSelectedPrefab();
	SceneEditorTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------


//==============================================================================
// Default T3D Prefab function for backward compatibility
//==============================================================================
//==============================================================================
function EditorMakePrefab() {
	Lab.CreatePrefab();
}

function EditorExplodePrefab() {
	Lab.ExplodePrefab();
}
//------------------------------------------------------------------------------
