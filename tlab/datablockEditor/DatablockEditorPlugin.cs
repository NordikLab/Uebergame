//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//=============================================================================================
//    Initialization.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::init( %this ) {
	%this.update();

	if( !DatablockEditorTree.getItemCount() )
		%this.populateTrees();
}
function DatablockEditorPlugin::update( %this ) {
	// DatablockEditorInspectorWindow.position = getWord($pref::Video::mode, 0) - 209 SPC getWord(EditorGuiToolbar.extent, 1) + getWord(LabPhysicTreeWindow.extent, 1) - 2;
	//DatablockEditorTreeWindow.position = getWord($pref::Video::mode, 0) - 209 SPC getWord(EditorGuiToolbar.extent, 1) - 1;
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::onActivated( %this ) {
	SceneEditorToolbar.setVisible(false);
	EditorGui.bringToFront( DatablockEditorPlugin );
	DatablockEditorTreeWindow.setVisible( true );
	DatablockEditorInspectorWindow.setVisible( true );
	DatablockEditorInspectorWindow.makeFirstResponder( true );
	
	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo( "Datablock editor." );
	%numSelected = %this.getNumSelectedDatablocks();

	if( !%numSelected )
		EditorGuiStatusBar.setSelection( "" );
	else
		EditorGuiStatusBar.setSelection( %numSelected @ " datablocks selected" );

	%this.init();
	// DatablockEditorPlugin.readSettings();

	if( EWorldEditor.getSelectionSize() == 1 )
		%this.onObjectSelected( EWorldEditor.getSelectedObject( 0 ) );

	Parent::onActivated( %this );
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::onDeactivated( %this ) {
	//DatablockEditorPlugin.writeSettings();

	Parent::onDeactivated(%this);
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::onExitMission( %this ) {
	DatablockEditorTree.clear();
	DatablockEditorInspector.inspect( "" );
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::openDatablock( %this, %datablock ) {
	// EditorGui.setEditor( DatablockEditorPlugin );
	%this.selectDatablock( %datablock );
	DatablockEditorTreeTabBook.selectedPage = 0;
}

//---------------------------------------------------------------------------------------------

function DatablockEditorPlugin::setEditorFunction( %this ) {
	return true;
}
