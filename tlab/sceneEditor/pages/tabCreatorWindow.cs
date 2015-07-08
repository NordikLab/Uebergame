//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

function SceneCreatorWindow::init( %this ) {
	// Just so we can recall this method for testing changes
	// without restarting.
	if ( isObject( %this.array ) )
		%this.array.delete();

	%this.array = new ArrayObject();
	%this.array.caseSensitive = true;
	%this.setListView( true );
	%this.beginGroup( "Environment" );
	// Removed Prefab as there doesn't really seem to be a point in creating a blank one
	//%this.registerMissionObject( "Prefab",              "Prefab" );
	%this.registerMissionObject( "SkyBox",              "Sky Box" );
	%this.registerMissionObject( "CloudLayer",          "Cloud Layer" );
	%this.registerMissionObject( "BasicClouds",         "Basic Clouds" );
	%this.registerMissionObject( "ScatterSky",          "Scatter Sky" );
	%this.registerMissionObject( "Sun",                 "Basic Sun" );
	%this.registerMissionObject( "Lightning" );
	%this.registerMissionObject( "WaterBlock",          "Water Block" );
	%this.registerMissionObject( "SFXEmitter",          "Sound Emitter" );
	%this.registerMissionObject( "Precipitation" );
	%this.registerMissionObject( "ParticleEmitterNode", "Particle Emitter" );
	// Legacy features. Users should use Ground Cover and the Forest Editor.
	//%this.registerMissionObject( "fxShapeReplicator",   "Shape Replicator" );
	//%this.registerMissionObject( "fxFoliageReplicator", "Foliage Replicator" );
	%this.registerMissionObject( "PointLight",          "Point Light" );
	%this.registerMissionObject( "SpotLight",           "Spot Light" );
	%this.registerMissionObject( "GroundCover",         "Ground Cover" );
	%this.registerMissionObject( "TerrainBlock",        "Terrain Block" );
	%this.registerMissionObject( "GroundPlane",         "Ground Plane" );
	%this.registerMissionObject( "WaterPlane",          "Water Plane" );
	%this.registerMissionObject( "PxCloth",             "Cloth" );
	%this.registerMissionObject( "ForestWindEmitter",   "Wind Emitter" );
	%this.registerMissionObject( "DustEmitter", "Dust Emitter" );
	%this.registerMissionObject( "DustSimulation", "Dust Simulation" );
	%this.registerMissionObject( "DustEffecter", "Dust Effecter" );
	%this.endGroup();
	%this.beginGroup( "Level" );
	%this.registerMissionObject( "MissionArea",  "Mission Area" );
	%this.registerMissionObject( "Path" );
	%this.registerMissionObject( "Marker",       "Path Node" );
	%this.registerMissionObject( "Trigger" );
	%this.registerMissionObject( "PhysicalZone", "Physical Zone" );
	%this.registerMissionObject( "Camera" );
	%this.registerMissionObject( "LevelInfo",    "Level Info" );
	%this.registerMissionObject( "TimeOfDay",    "Time of Day" );
	%this.registerMissionObject( "Zone",         "Zone" );
	%this.registerMissionObject( "Portal",       "Zone Portal" );
	%this.registerMissionObject( "SpawnSphere",  "Player Spawn Sphere", "PlayerDropPoint" );
	%this.registerMissionObject( "SpawnSphere",  "Observer Spawn Sphere", "ObserverDropPoint" );
	%this.registerMissionObject( "SFXSpace",      "Sound Space" );
	%this.registerMissionObject( "OcclusionVolume", "Occlusion Volume" );
	%this.registerMissionObject("NavMesh", "Navigation mesh");
	%this.registerMissionObject("NavPath", "Path");
	%this.endGroup();
	// andrewmac: PhysX 3.3
	%this.beginGroup( "PhysX 3.3" );
	%this.registerMissionObject( "Px3Cloth",        "Cloth Plane" );
	%this.registerMissionObject( "Px3Static",       "Static Shape" );
	%this.endGroup();
	%this.beginGroup( "System" );
	%this.registerMissionObject( "SimGroup" );
	%this.endGroup();
	%this.beginGroup( "ExampleObjects" );
	%this.registerMissionObject( "RenderObjectExample" );
	%this.registerMissionObject( "RenderMeshExample" );
	%this.registerMissionObject( "RenderShapeExample" );
	%this.endGroup();
}

//==============================================================================
function SceneCreatorWindow::onWake( %this ) {
	if ($SEP_CreatorTypeActive $= "")
		$SEP_CreatorTypeActive = "0";

	CreatorTypeStack.getObject(0).performClick();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::beginGroup( %this, %group ) {
	%this.currentGroup = %group;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::endGroup( %this, %group ) {
	%this.currentGroup = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::getCreateObjectPosition() {
	%focusPoint = LocalClientConnection.getControlObject().getLookAtPoint();

	if( %focusPoint $= "" )
		return "0 0 0";
	else
		return getWord( %focusPoint, 1 ) SPC getWord( %focusPoint, 2 ) SPC getWord( %focusPoint, 3 );
}
//------------------------------------------------------------------------------
//==============================================================================


function SceneCreatorWindow::navigateDown( %this, %folder ) {
	if ( %this.address $= "" )
		%address = %folder;
	else
		%address = %this.address SPC %folder;

	// Because this is called from an IconButton::onClick command
	// we have to wait a tick before actually calling navigate, else
	// we would delete the button out from under itself.
	%this.schedule( 1, "navigate", %address );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::navigateUp( %this ) {
	%count = getWordCount( %this.address );

	if ( %count == 0 )
		return;

	if ( %count == 1 )
		%address = "";
	else
		%address = getWords( %this.address, 0, %count - 2 );

	%this.navigate( %address );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneCreatorWindow::setListView( %this, %noupdate ) {
	//CreatorIconArray.clear();
	//CreatorIconArray.setVisible( false );
	CreatorIconArray.setVisible( true );
	SceneCreatorWindow.contentCtrl = CreatorIconArray;
	%this.isList = true;

	if ( %noupdate == true )
		%this.navigate( %this.address );
}
//------------------------------------------------------------------------------
//==============================================================================
//function SceneCreatorWindow::setIconView( %this )
//{
//echo( "setIconView" );
//
//CreatorIconStack.clear();
//CreatorIconStack.setVisible( false );
//
//CreatorIconArray.setVisible( true );
//%this.contentCtrl = CreatorIconArray;
//%this.isList = false;
//
//%this.navigate( %this.address );
//}

//------------------------------------------------------------------------------
//==============================================================================
function CreatorPopupMenu::onSelect( %this, %id, %text ) {
	%split = strreplace( %text, "/", " " );
	SceneCreatorWindow.navigate( %split );
}
//------------------------------------------------------------------------------
