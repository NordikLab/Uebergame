//==============================================================================
// TorqueLab -> Manage the Editor Camera
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/* Con::setIntVariable( "$EditTsCtrl::DisplayTypeTop", DisplayTypeTop);                = 0
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBottom", DisplayTypeBottom);          = 1
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeFront", DisplayTypeFront);            = 2
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBack", DisplayTypeBack);              = 3
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeLeft", DisplayTypeLeft);              = 4
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeRight", DisplayTypeRight);            = 5
   Con::setIntVariable( "$EditTsCtrl::DisplayTypePerspective", DisplayTypePerspective);= 6
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeIsometric", DisplayTypeIsometric);    = 7*/
//==============================================================================

//==============================================================================
// Define some global value
//==============================================================================
$LabDefaultCameraView = "Standard Camera";

$LabCameraDisplayName[0] = "Top View";
$LabCameraDisplayName[1] = "Bottom View";
$LabCameraDisplayName[2] = "Front View";
$LabCameraDisplayName[3] = "Back View";
$LabCameraDisplayName[4] = "Left View";
$LabCameraDisplayName[5] = "Right View";
$LabCameraDisplayName[6] = "Standard Camera";
$LabCameraDisplayName[7] = "Isometric View";

$LabCameraDisplayType["Top View"] = $EditTsCtrl::DisplayTypeTop;
$LabCameraDisplayType["Bottom View"] = $EditTsCtrl::DisplayTypeBottom;
$LabCameraDisplayType["Left View"] = $EditTsCtrl::DisplayTypeLeft;
$LabCameraDisplayType["Right View"] = $EditTsCtrl::DisplayTypeRight;
$LabCameraDisplayType["Front View"] = $EditTsCtrl::DisplayTypeFront;
$LabCameraDisplayType["Back View"] = $EditTsCtrl::DisplayTypeBack;
$LabCameraDisplayType["Isometric View"] = $EditTsCtrl::DisplayTypeIsometric;
$LabCameraDisplayType["Standard Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["1st Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["3rd Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Orbit Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Rot Camera"] = $EditTsCtrl::DisplayTypePerspective;

$LabCameraDisplayMode["Top View"] = "Standard";
$LabCameraDisplayMode["Bottom View"] ="Standard";
$LabCameraDisplayMode["Left View"] = "Standard";
$LabCameraDisplayMode["Right View"] = "Standard";
$LabCameraDisplayMode["Front View"] ="Standard";
$LabCameraDisplayMode["Back View"] = "Standard";
$LabCameraDisplayMode["Isometric View"] = "Standard";
$LabCameraDisplayMode["Standard Camera"] = "Standard";
$LabCameraDisplayMode["1st Person Camera"] = "Player";
$LabCameraDisplayMode["3rd Person Camera"] = "PlayerThird";
$LabCameraDisplayMode["Orbit Camera"] = "Orbit";
$LabCameraDisplayMode["Smooth Camera"] = "Newton";
$LabCameraDisplayMode["Smooth Rot Camera"] = "NewtonDamped";


$LabCameraTypesIcon = "tlab/gui/icons/toolbar_assets/ToggleCamera";

//------------------------------------------------------------------------------
//==============================================================================
// Set the initial editor camera and store the game camera settings
function Lab::setInitialCamera(%this) {
	%client = LocalClientConnection;
	%this.gameControlObject = %client.getControlObject();

	if (!isObject(%client.camera)) {
		%this.gameCam = %client.getCameraObject();
		%client.camera = spawnObject("Camera", "Observer");
	}

	%client.camera.scopeToClient(%client);
	%client.setCameraObject(%client.camera);
	Lab.clientWasControlling = %client.getControlObject();
	%freeViewMode = %this.LaunchInFreeview || !isObject(%client.player);

	if (%client.getControlObject() != %client.player && !%freeViewMode) {
		%this.setCameraPlayerMode();
		return;
	}

	%pos = %client.player.getPosition();

	if (%pos !$= "") {
		%pos.z += 5;
		%client.camera.position = %pos;
	}

	//Set back the current camera or set default
	Lab.setCameraViewMode(Lab.currentCameraMode);
}
//==============================================================================
// Reset the game camera like it was when editor open
function Lab::setGameCamera(%this) {
	if (isObject(%this.gameCam)) {
		%this.gameCam.scopeToClient(LocalClientConnection);
		LocalClientConnection.setCameraObject(%this.gameCam);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleControlObject(%this) {
	if (!isObject(%this.gameControlObject)) {
		warnLog("There's no Game control object stored:",%this.gameControlObject);
		return;
	}

	//If Client is controlling game object, set control camera, else do contrary...
	if (%this.gameControlObject == LocalClientConnection.getControlObject())
		LocalClientConnection.setCOntrolObject(LocalClientConnection.camera);
	else
		LocalClientConnection.setCOntrolObject(%this.gameControlObject);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setCameraPlayerMode(%this) {
	if (!isObject( LocalClientConnection.player)) {
		warnLog("You don't have a player assigned, set spawnPlayer true to spawn one automatically");
		return;
	}

	%player = LocalClientConnection.player;
	%player.setVelocity("0 0 0");
	LocalClientConnection.setControlObject(%player);
	%this.syncCameraGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setCameraViewMode( %this, %mode,%skipType ) {
	if(%mode $= "Top View" || %mode $="") {
		%mode = "Standard Camera";
	}

	Lab.SetEditorCameraView($LabCameraDisplayMode[%mode]);

	// commandToServer( 'SetEditorCameraView', $LabCameraDisplayMode[%mode] );
	if (%skipType)
		return;

	Lab.setCameraViewType( $LabCameraDisplayType[%mode] );
	Lab.currentCameraMode = %mode;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::SetEditorCameraView(%this,%type) {
	%client = LocalClientConnection;

	switch$(%type) {
	case "Standard":
		// Switch to Fly Mode
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "0";
		%client.camera.newtonRotation = "0";
		%client.setControlObject(%client.camera);

	case "Newton":
		// Switch to Newton Fly Mode without rotation damping
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "0";
		%client.camera.setVelocity("0 0 0");
		%client.setControlObject(%client.camera);

	case "NewtonDamped":
		// Switch to Newton Fly Mode with damped rotation
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "1";
		%client.camera.setAngularVelocity("0 0 0");
		%client.setControlObject(%client.camera);

	case "Orbit":
		LocalClientConnection.camera.setEditOrbitMode();
		%client.setControlObject(%client.camera);
		devLog("Orbit mode activated");

	case "FlyCamera":
		%client.camera.setFlyMode();
		%client.setControlObject(%client.camera);

	case "Player":
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
		ServerConnection.setFirstPerson(1);
		$isFirstPersonVar = 1;

	case "PlayerThird":
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
		ServerConnection.setFirstPerson(0);
		$isFirstPersonVar = 0;
	}

	Lab.syncCameraGui();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the current camera type info in different editor areas
function Lab::setCameraViewType( %this, %type ) {
	%gui = %this.currentEditor.editorGui;

	if( !isObject( %gui ) )
		return;

	if ($LabCameraDisplayType[%type] !$="")
		%type = $LabCameraDisplayType[%type];

	%typeName = $LabCameraDisplayName[%type];
	Lab.checkMenuItem("viewTypeMenu",0, 7, %type );

	EditorGuiStatusBar.setCamera(%typeName,false);


	// Store the current camera rotation so we can restore it correctly when
	// switching back to perspective view
	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		%this.lastPerspectiveCamRotation = LocalClientConnection.camera.getRotation();

	%gui.setDisplayType( %type );

	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		LocalClientConnection.camera.setRotation( %this.lastPerspectiveCamRotation );

	%this.cameraDisplayType = %type;
	ECamViewGui.updateCurrentView();	
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the camera information on the editor guis
function Lab::syncCameraGui( %this,%forced ) {
	if( !EditorIsActive())
		return;

	// Sync projection type
	%displayType = Lab.currentEditor.editorGui.getDisplayType();

	if (!%displayType) %displayType = 6;

	Lab.checkMenuItem("viewTypeMenu",0,7,%displayType);

	// Set the camera object's mode and rotation so that it moves correctly
	// based on the current editor mode

	if( %displayType != $EditTSCtrl::DisplayTypePerspective ) {
		switch( %displayType ) {
		case $EditTSCtrl::DisplayTypeTop:
			%name = "Top View";
			%camRot = "0 0 0";

		case $EditTSCtrl::DisplayTypeBottom:
			%name = "Bottom View";
			%camRot = "3.14159 0 0";

		case $EditTSCtrl::DisplayTypeLeft:
			%name = "Left View";
			%camRot = "-1.571 0 1.571";

		case $EditTSCtrl::DisplayTypeRight:
			%name = "Right View";
			%camRot = "-1.571 0 -1.571";

		case $EditTSCtrl::DisplayTypeFront:
			%name = "Front View";
			%camRot = "-1.571 0 3.14159";

		case $EditTSCtrl::DisplayTypeBack:
			%name = "Back View";
			%camRot = "-1.571 0 0";

		case $EditTSCtrl::DisplayTypeIsometric:
			%name = "Isometric View";
			%camRot = "0 0 0";
		}

		LocalClientConnection.camera.controlMode = "Fly";
		LocalClientConnection.camera.setRotation( %camRot );
		EditorGuiStatusBar.setCamera( %name );
		return;
	}

	%cameraTypesButton = EditorGuiToolbar-->cameraTypes;
	%cameraTypesButton.setBitmap($LabCameraTypesIcon); //Default Toggle Camera Icon
	// Sync camera settings.
	%flyModeRadioItem = -1;

	if(LocalClientConnection.getControlObject() != LocalClientConnection.player) {
		%mode = LocalClientConnection.camera.getMode();

		if(%mode $= "Fly" && LocalClientConnection.camera.newtonMode) {
			if(LocalClientConnection.camera.newtonRotation == true) {
				EditorGui-->NewtonianRotationCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/smooth-cam-rot");
				%flyModeRadioItem = 4;
				EditorGuiStatusBar.setCamera("Smooth Rot Camera");
			} else {
				EditorGui-->NewtonianCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/smooth-cam");
				%flyModeRadioItem = 3;
				EditorGuiStatusBar.setCamera("Smooth Camera");
			}
		} else if(%mode $= "EditOrbit") {
			EditorGui-->OrbitCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/orbit-cam");
			%flyModeRadioItem = 1;
			EditorGuiStatusBar.setCamera("Orbit Camera");
		} else { // default camera mode
			//EditorGui-->StandardCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/camera");
			%flyModeRadioItem = 0;
			EditorGuiStatusBar.setCamera("Standard Camera");
		}

		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,0);
		Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, %flyModeRadioItem);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 4, -1);
	} else if (!$isFirstPersonVar) { // if 3rd person
		//EditorGui-->trdPersonCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/3rd-person-camera");
		%flyModeRadioItem = 1;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);
		EditorGuiStatusBar.setCamera("3rd Person Camera");
	} else if ($isFirstPersonVar) { // if 1st Person
		EditorGui-->PlayerCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/player");
		%flyModeRadioItem = 0;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);
		Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, -1);
		EditorGuiStatusBar.setCamera("1st Person Camera");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::fitCameraToSelection( %this,%orbit ) {
	if (%orbit) {
		Lab.setCameraViewMode("Orbit Camera",false);
	}

	//GuiShapeEdPreview have it's own function
	if (isObject(Lab.fitCameraGui)) {
		Lab.fitCameraGui.fitToShape();
		return;
	}

	%radius = EWorldEditor.getSelectionRadius()+1;
	LocalClientConnection.camera.autoFitRadius(%radius);
	LocalClientConnection.setControlObject(LocalClientConnection.camera);
	%this.syncCameraGui();
}
//------------------------------------------------------------------------------