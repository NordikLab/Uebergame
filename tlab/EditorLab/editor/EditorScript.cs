//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function toggleEditor(%make) {
	if (Canvas.isFullscreen()) {
		LabMsgOK("Windowed Mode Required", "Please switch to windowed mode to access the Mission Editor.");
		return;
	}

	if (%make) {
		%timerId = startPrecisionTimer();

		if( $InGuiEditor )
			GuiEdit();

		//Check if a mission have been loaded
		if( !$missionRunning ) {
			// Flag saying, when level is chosen, launch it with the editor open.
			ChooseLevelDlg.launchInEditor = true;
			Canvas.pushDialog( ChooseLevelDlg );
			return;
		}

		//We have a running mission, check if looking to launch or close
		pushInstantGroup();

		//If Launched for first time, create the EditorManager
		if ( !isObject( Editor ) ) {
			Editor::create();
			MissionCleanup.add( Editor );
			MissionCleanup.add( Editor.getUndoManager() );
		}

		//If the Editor is currently active, we should close it
		if( EditorIsActive() ) {
			Editor.close($HudCtrl); //Close will set content to specified GUI
			popInstantGroup();
			return;
		}

		//There's no Editor active and there's a running mission, launch Editor
		// Cancel any Game Schedule
		cancel($Game::Schedule);
		//Show the loading editor progress dialog
		canvas.pushDialog( EditorLoadingGui );
		canvas.repaint();
		//Editor Open function will deal with the Editor Loading
		Editor.open();
		//Everything Loaded, close Loading Dialog
		canvas.popDialog(EditorLoadingGui);
		popInstantGroup();
		%elapsed = stopPrecisionTimer( %timerId );
		info( "Time spent in toggleEditor() : " @ %elapsed / 1000.0 @ " s" );
	}
}
//------------------------------------------------------------------------------
//  The editor action maps are defined in editor.bind.cs
GlobalActionMap.bind(keyboard, "f11", toggleEditor);
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Mission Editor
//------------------------------------------------------------------------------

function Editor::create() {
	// Not much to do here, build it and they will come...
	// Only one thing... the editor is a gui control which
	// expect the Canvas to exist, so it must be constructed
	// before the editor.
	new EditManager(Editor) {
		profile = "GuiContentProfile";
		horizSizing = "right";
		vertSizing = "top";
		position = "0 0";
		extent = "640 480";
		minExtent = "8 8";
		visible = "1";
		setFirstResponder = "0";
		modal = "1";
		helpTag = "0";
		open = false;
	};
}

function Editor::getUndoManager(%this) {
	if ( !isObject( %this.undoManager ) ) {
		/// This is the global undo manager used by all
		/// of the mission editor sub-editors.
		%this.undoManager = new UndoManager( EUndoManager ) {
			numLevels = 200;
		};
	}

	return %this.undoManager;
}

function Editor::setUndoManager(%this, %undoMgr) {
	%this.undoManager = %undoMgr;
}

function Editor::onAdd(%this) {
	// Ignore Replicated fxStatic Instances.
	EWorldEditor.ignoreObjClass("fxShapeReplicatedStatic");
}

function Editor::checkActiveLoadDone() {
	if(isObject(EditorGui) && EditorGui.loadingMission) {
		Canvas.setContent(EditorGui);
		EditorGui.loadingMission = false;
		return true;
	}

	return false;
}




// The scenario:
// The editor is open and the user closes the level by any way other than
// the file menu ( exit level ), eg. typing disconnect() in the console.
//
// The problem:
// Editor::close() is not called in this scenario which means onEditorDisable
// is not called on objects which hook into it and also gEditingMission will no
// longer be valid.
//
// The solution:
// Override the stock disconnect() function which is in game scripts from here
// in tools so we avoid putting our code in there.
//
// Disclaimer:
// If you think of a better way to do this feel free. The thing which could
// be dangerous about this is that no one will ever realize this code overriding
// a fairly standard and core game script from a somewhat random location.
// If it 'did' have unforscene sideeffects who would ever find it?

package EditorDisconnectOverride {
	function disconnect() {
		if ( isObject( Editor ) && Editor.isEditorEnabled() ) {
			if (isObject( MainMenuGui ))
				Editor.close("MainMenuGui");
		}

		Parent::disconnect();
	}
};
activatePackage( EditorDisconnectOverride );
//==============================================================================
function EditorGui::onNewLevelLoaded( %this, %levelName ) {
	Lab.levelName = %levelName;
	Lab.setMenuDefaultState("EditorCameraSpeedOptions");
	//EditorCameraSpeedOptions.setupDefaultState();
	new ScriptObject( EditorMissionCleanup ) {
		parentGroup = "MissionCleanup";
	};
	
	foreach(%plugin  in LabPluginGroup)
		%plugin.onNewLevelLoaded(%levelName);
}
//------------------------------------------------------------------------------
