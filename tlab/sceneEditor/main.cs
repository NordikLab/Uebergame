//==============================================================================
// TorqueLab -> Scene Editor Plugin
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//Initialize the Scene Editor Plugin
function initializeSceneEditor() {
	info( "TorqueLab ->","Initializing Scene Editor plugin" );
	//Load the guis and scripts
	execSceneEd(true);
	
		//Create the TorqueLab Plugin instance
	Lab.createPlugin("SceneEditor","Scene Editor",true);
	
	//Add the Plugin related GUIs to TorqueLab
	Lab.addPluginGui("SceneEditor",SceneEditorTools);
	Lab.addPluginToolbar("SceneEditor",SceneEditorToolbar);
	Lab.addPluginPalette("SceneEditor",SceneEditorPalette);
	

	Lab.addPluginDlg("SceneEditor",SceneEditorDialogs);
	SceneEditorPlugin.superClass = "WEditorPlugin";
	$SEPtools = newScriptObject("SEPtools");
	$SceneObjectGroupSet = newSimSet(SceneObjectGroupSet);
	$SceneCreator = newScriptObject("SceneCreator");
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function execSceneEd(%loadGui) {
	if (%loadGui) {
		exec("tlab/sceneEditor/gui/SceneEditorTools.gui");
		exec("./gui/SceneEditorToolbar.gui" );
		exec("tlab/sceneEditor/gui/SceneEditorPalette.gui" );
		exec("tlab/sceneEditor/gui/SceneEditorDialogs.gui");
	}

	exec("tlab/sceneEditor/SceneEditorPlugin.cs");
	exec("tlab/sceneEditor/SceneEditorToolbar.cs");
	exec("tlab/sceneEditor/SceneEditorTools.cs");
	execPattern("tlab/sceneEditor/manager/*.cs");
	execPattern("tlab/sceneEditor/dialogs/*.cs");
	execPattern("tlab/sceneEditor/pages/*.cs");
}
//------------------------------------------------------------------------------