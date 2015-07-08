//==============================================================================
// GameLab -> Editor Binds
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Create the Editor specific ActionMap
delObj(EditorMap);
new ActionMap(EditorMap);

//==============================================================================
// Editor General bind functions
//==============================================================================

function EditorGlobalDelete() {	
	if ( isObject( Lab.currentEditor ) )
		Lab.currentEditor.handleDelete();
}

EditorMap.bind(keyboard,"delete",EditorGlobalDelete);
//==============================================================================
// Editor mouse movement functions
//==============================================================================

//==============================================================================
function getEditorMouseAdjustAmount(%val) {
	// based on a default camera FOV of 90'
	return(%val * ($cameraFov / 90) * 0.01) * $Cfg_MouseSpeed;
}
//------------------------------------------------------------------------------
//==============================================================================
function getEditorMouseScrollAdjustAmount(%val) {
	// based on a default camera FOV of 90'
	return(%val * ($cameraFov / 90) * 0.01) * $Cfg_MouseScrollSpeed;
}
//------------------------------------------------------------------------------
//==============================================================================
function mouseWheelScroll( %val ) {
	%rollAdj = getEditorMouseScrollAdjustAmount(%val);
	%rollAdj = mClamp(%rollAdj, -mPi()+0.01, mPi()-0.01);
	$mvRoll += %rollAdj; //Maxed at pi in code
}
//------------------------------------------------------------------------------
//==============================================================================
function editorYaw(%val) {
	%yawAdj = getEditorMouseAdjustAmount(%val);

	if(ServerConnection.isControlObjectRotDampedCamera() || EWorldEditor.isMiddleMouseDown()) {
		// Clamp and scale
		%yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
		%yawAdj *= 0.5;
	}

	if( EditorSettings.value( "Camera/invertXAxis" ) )
		%yawAdj *= -1;

	$mvYaw += %yawAdj;
}
//------------------------------------------------------------------------------
//==============================================================================
function editorPitch(%val) {
	%pitchAdj = getEditorMouseAdjustAmount(%val);

	if(ServerConnection.isControlObjectRotDampedCamera() || EWorldEditor.isMiddleMouseDown()) {
		// Clamp and scale
		%pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
		%pitchAdj *= 0.5;
	}

	if( Lab.invertYAxis )
		%pitchAdj *= -1;

	$mvPitch += %pitchAdj;
}
//------------------------------------------------------------------------------
//==============================================================================
function editorWheelFadeScroll( %val ) {
	EWorldEditor.fadeIconsDist += %val * 0.1;

	if( EWorldEditor.fadeIconsDist < 0 )
		EWorldEditor.fadeIconsDist = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
function pressButton0( %val ) {
	$Button0Pressed = %val;
	devLog("Button 0 pressed = ",$Button0Pressed);
}
//------------------------------------------------------------------------------
//==============================================================================
// Default Camera movement binds

EditorMap.bind( mouse, xaxis, editorYaw );
EditorMap.bind( mouse, yaxis, editorPitch );
EditorMap.bind( mouse, zaxis, mouseWheelScroll );
EditorMap.bind( keyboard, "tab", pressButton0 );


EditorMap.bind( mouse, "alt zaxis", editorWheelFadeScroll );
EditorMap.bindCmd( keyboard, "ctrl o", "toggleDlg(LabSettingsDlg);","" );
EditorMap.bindCmd(keyboard, "ctrl z", "Editor.getUndoManager().undo();", "");
EditorMap.bindCmd(keyboard, "ctrl y", "Editor.getUndoManager().redo();", "");
//------------------------------------------------------------------------------

//==============================================================================
// Special Editor Camera binds
//==============================================================================

//==============================================================================
function dropCameraAtPlayer(%val) {
	if (%val)
		commandToServer('dropCameraAtPlayer');
}
//------------------------------------------------------------------------------
//==============================================================================
function dropPlayerAtCamera(%val) {
	if (%val)
		commandToServer('DropPlayerAtCamera');
}
//------------------------------------------------------------------------------
//==============================================================================
EditorMap.bind(keyboard, "F8", dropCameraAtPlayer);
EditorMap.bind(keyboard, "F7", dropPlayerAtCamera);
//------------------------------------------------------------------------------
