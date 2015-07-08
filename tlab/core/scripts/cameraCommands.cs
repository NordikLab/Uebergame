//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================




//==============================================================================

function Lab::EditorOrbitCameraChange(%this,%client, %size, %center) {
	if(%size > 0) {
		%client.camera.setValidEditOrbitPoint(true);
		%client.camera.setEditOrbitPoint(%center);
	} else {
		%client.camera.setValidEditOrbitPoint(false);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::EditorCameraFitRadius(%this,%client, %radius) {
	%client.camera.autoFitRadius(%radius);
	%client.setControlObject(%client.camera);
	Lab.syncCameraGui();
}
//------------------------------------------------------------------------------


function serverCmdDropPlayerAtCamera(%client) {
	// If the player is mounted to something (like a vehicle) drop that at the
	// camera instead. The player will remain mounted.
	%obj = %client.player.getObjectMount();

	if (!isObject(%obj))
		%obj = %client.player;

	%obj.setTransform(%client.camera.getTransform());
	%obj.setVelocity("0 0 0");
	%client.setControlObject(%client.player);
	Lab.syncCameraGui();
}

function serverCmdDropCameraAtPlayer(%client) {
	%client.camera.setTransform(%client.player.getEyeTransform());
	%client.camera.setVelocity("0 0 0");
	%client.setControlObject(%client.camera);
	Lab.syncCameraGui();
}



function Lab::CycleCameraFlyType(%this,%client) {
	if(%client.camera.getMode() $= "Fly") {
		if(%client.camera.newtonMode == false) { // Fly Camera
			// Switch to Newton Fly Mode without rotation damping
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "0";
			%client.camera.setVelocity("0 0 0");
		} else if(%client.camera.newtonRotation == false) { // Newton Camera without rotation damping
			// Switch to Newton Fly Mode with damped rotation
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "1";
			%client.camera.setAngularVelocity("0 0 0");
		} else { // Newton Camera with rotation damping
			// Switch to Fly Mode
			%client.camera.newtonMode = "0";
			%client.camera.newtonRotation = "0";
		}

		%client.setControlObject(%client.camera);
		Lab.syncCameraGui();
	}
}

