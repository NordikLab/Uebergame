//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//=============================================================================================
//    ParticleEditorPlugin.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ParticleEditorPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
}

//---------------------------------------------------------------------------------------------

function ParticleEditorPlugin::onActivated( %this ) {
	if( !%this.isInitialized ) {
		ParticleEditor.initEditor();
		%this.isInitialized = true;
	}

	EditorGui-->SceneEditorToolbar.setVisible( true );
	EditorGui.bringToFront( PE_Window);
	PE_Window.setVisible( true );
	PE_Window.makeFirstResponder( true );
	%this.map.push();
	ParticleEditor.resetEmitterNode();
	// Set the status bar here
	EditorGuiStatusBar.setInfo( "Particle editor." );
	EditorGuiStatusBar.setSelection( "" );
	Parent::onActivated( %this );
}

//---------------------------------------------------------------------------------------------

function ParticleEditorPlugin::onDeactivated( %this ) {
	EditorGui-->SceneEditorToolbar.setVisible( false );
	PE_Window.setVisible( false );

	if( isObject( $ParticleEditor::emitterNode) )
		$ParticleEditor::emitterNode.delete();

	%this.map.pop();
	Parent::onDeactivated( %this );
}

//---------------------------------------------------------------------------------------------

function ParticleEditorPlugin::onExitMission( %this ) {
	// Force Particle Editor to re-initialize.
	%this.isInitialized = false;
	Parent::onExitMission( %this );
}
