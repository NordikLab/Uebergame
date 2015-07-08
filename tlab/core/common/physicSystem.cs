//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabPhysic_SimulationTimescale = 1;
newSimSet("LabPhysicShapeSet");

function Lab::GetMissionPhysicsShapes( %this,%noStateStore ) {
	%this.CheckGroupForPhysicShapes(MissionGroup,%noStateStore);
	info(LabPhysicShapeSet.getCount()," PhysicShapes added to Set");
}

function Lab::CheckGroupForPhysicShapes( %this,%group,%noStateStore ) {
	foreach( %obj in %group ) {
		if (%obj.getClassName() $= "PhysicsShape") {
			LabPhysicShapeSet.add(%obj);

			if (!%noStateStore)
				%this.StorePhysicsShapeState(%obj);
		} else if (%obj.getClassName() $= "SimGroup") {
			%obj.depth = %group.depth + 1;
			%this.CheckGroupForPhysicShapes(%obj);
		}
	}
}

function Lab::StoreAllPhysicsShapesState( %this ) {
	%this.GetMissionPhysicsShapes(true);

	foreach( %obj in LabPhysicShapeSet )
		%this.StorePhysicsShapeState(%obj);
}
function Lab::RestoreAllPhysicsShapeState( %this ) {
	%this.GetMissionPhysicsShapes(true);

	foreach( %obj in LabPhysicShapeSet )
		%this.RestorePhysicsShapeState(%obj);
}
function Lab::StorePhysicsShapeState( %this,%obj ) {
	%obj.physicTrans = %obj.getTransform();
}
function Lab::RestorePhysicsShapeState( %this,%obj ) {
	Lab.physicsStopSimulation();

	if (%obj.physicTrans !$="")
		%obj.setTransform(%obj.physicTrans);
}

function Lab::physicsStartSimulation( %this ) {
	if ( physicsSimulationEnabled() ) {
		warnLog("Physics is allready started");
		return;
	}

	physicsStartSimulation("client");
	physicsStartSimulation("server");
	%scale = %this.physicsGetTimeScale();
	info("Simlutaion start with timescale of:",%scale);
}
function Lab::physicsStopSimulation( %this ) {
	if ( !physicsSimulationEnabled() ) {
		warnLog("Physics is allready stopped");
		return;
	}

	physicsStopSimulation("client");
	physicsStopSimulation("server");
}
function Lab::physicsToggleSimulation( %this ) {
	%isEnabled = physicsSimulationEnabled();

	if ( %isEnabled ) {
		physicsStateText.setText( "Simulation is paused." );
		physicsStopSimulation( "client" );
		physicsStopSimulation( "server" );
	} else {
		physicsStateText.setText( "Simulation is unpaused." );
		physicsStartSimulation( "client" );
		physicsStartSimulation( "server" );
	}
}
function Lab::physicsSetTimeScale( %this,%scale ) {
	if (physicsSimulationEnabled()) {
		%restart = true;
		%this.physicsStopSimulation();
	}

	if (%scale $= "") %scale = $LabPhysic_SimulationTimescale;

	physicsSetTimeScale(%scale);

	if (%restart) {
		%this.physicsStartSimulation();
	}
}
function Lab::physicsGetTimeScale( %this,%scale ) {
	return physicsGetTimeScale();
}
function Lab::getPhysicsSimulationState( %this ) {
}




function Lab::physicsStoreState( %this ) {
	physicsStoreState();
}

function Lab::physicsRestoreState( %this ) {
	physicsRestoreState();
}
function Lab::physicsDebugDraw( %this,%enabled ) {
	physicsDebugDraw(%enabled);
}

function Lab::physicsRestoreState( %this ) {
	physicsRestoreState();
}