//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function initializeDecalEditor() {
	echo(" % - Initializing Decal Editor");
	$decalDataFile = "art/decals/managedDecalData.cs";
	exec( "tlab/decalEditor/decalEditor.cs" );
	exec( "./gui/decalEditorGui.gui" );
	exec( "./gui/decalEditorTools.gui" );
	exec( "./gui/decalEditorPalette.gui" );
	exec( "tlab/decalEditor/decalEditorGui.cs" );
	exec( "tlab/decalEditor/decalEditorActions.cs" );
	exec( "tlab/decalEditor/DecalEditorPlugin.cs" );
	exec( "tlab/decalEditor/DecalEditorParams.cs" );
	// Add ourselves to EditorGui, where all the other tools reside
	Lab.addPluginEditor("DecalEditor",   DecalEditorGui);
	Lab.addPluginGui("DecalEditor",DecalEditorTools);
	Lab.addPluginPalette("DecalEditor",   DecalEditorPalette);
	DecalEditorTabBook.selectPage( 0 );
	Lab.createPlugin("DecalEditor");
	DecalEditorPlugin.editorGui = DecalEditorGui;
	%map = new ActionMap();
	%map.bindCmd( keyboard, "5", "EDecalEditorAddDecalBtn.performClick();", "" );
	%map.bindCmd( keyboard, "1", "EDecalEditorSelectDecalBtn.performClick();", "" );
	%map.bindCmd( keyboard, "2", "EDecalEditorMoveDecalBtn.performClick();", "" );
	%map.bindCmd( keyboard, "3", "EDecalEditorRotateDecalBtn.performClick();", "" );
	%map.bindCmd( keyboard, "4", "EDecalEditorScaleDecalBtn.performClick();", "" );
	DecalEditorPlugin.map = %map;
	new PersistenceManager( DecalPMan );
}

function destroyDecalEditor() {
}

// JCF: helper for during development
function reinitDecalEditor() {
	exec( "./main.cs" );
	exec( "./decalEditor.cs" );
	exec( "./decalEditorGui.cs" );
}

