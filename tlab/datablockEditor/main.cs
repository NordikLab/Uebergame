//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//---------------------------------------------------------------------------------------------

function initializeDatablockEditor() {
	echo( " - Initializing Datablock Editor" );
	exec("tlab/datablockEditor/datablockEditor.cs");
	exec("./datablockEditorUndo.cs");
	exec("./gui/DatablockEditorTools.gui");
	//exec("./gui/DatablockEditorInspectorWindow.gui");
	exec("./gui/DatablockEditorCreatePrompt.gui");
	exec( "tlab/datablockEditor/DatablockEditorPlugin.cs" );
	exec( "tlab/datablockEditor/DatablockEditorParams.cs" );
	// Add ourselves to EditorGui, where all the other tools reside
	Lab.addPluginGui("DatablockEditor",DatablockEditorTools);
	Lab.createPlugin("DatablockEditor");
	DatablockEditorPlugin.superClass = "WEditorPlugin";
	DatablockEditorPlugin.customPalette = "SceneEditorPalette";
	DatablockEditorTreeTabBook.selectPage( 0 );
	new SimSet( UnlistedDatablocks );
	// create our persistence manager
	DatablockEditorPlugin.PM = new PersistenceManager();
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "DatablockEditorPlugin.onDeleteKey();", "" );
	%map.bindCmd( keyboard, "delete", "DatablockEditorPlugin.onDeleteKey();", "" );
	DatablockEditorPlugin.map = %map;
	// DatablockEditorPlugin.initSettings();
}

//---------------------------------------------------------------------------------------------

function destroyDatablockEditor() {
}
