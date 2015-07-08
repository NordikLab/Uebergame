//==============================================================================
// TorqueLab -> ShapeEditor -> Utility Methods
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Utility Methods
//==============================================================================


if ( !isObject( ShapeEditor ) ) new ScriptObject( ShapeEditor ) {
	shape = -1;
	deletedCount = 0;
};


// Capitalise the first letter of the input string
function strcapitalise( %str ) {
	%len = strlen( %str );
	return strupr( getSubStr( %str,0,1 ) ) @ getSubStr( %str,1,%len-1 );
}

function ShapeEditor::getObjectShapeFile( %this, %obj ) {
	// Get the path to the shape file used by the given object (not perfect, but
	// works for the vast majority of object types)
	%path = "";

	if ( %obj.isMemberOfClass( "TSStatic" ) )
		%path = %obj.shapeName;
	else if ( %obj.isMemberOfClass( "PhysicsShape" ) )
		%path = %obj.getDataBlock().shapeName;
	else if ( %obj.isMemberOfClass( "GameBase" ) )
		%path = %obj.getDataBlock().shapeFile;

	return %path;
}

// Check if the given name already exists
function ShapeEditor::nameExists( %this, %type, %name ) {
	if ( ShapeEditor.shape == -1 )
		return false;

	if ( %type $= "node" )
		return ( ShapeEditor.shape.getNodeIndex( %name ) >= 0 );
	else if ( %type $= "sequence" )
		return ( ShapeEditor.shape.getSequenceIndex( %name ) >= 0 );
	else if ( %type $= "object" )
		return ( ShapeEditor.shape.getObjectIndex( %name ) >= 0 );
}

// Check if the given 'hint' name exists (spaces could also be underscores)
function ShapeEditor::hintNameExists( %this, %type, %name ) {
	if ( ShapeEditor.nameExists( %type, %name ) )
		return true;

	// If the name contains spaces, try replacing with underscores
	%name = strreplace( %name, " ", "_" );

	if ( ShapeEditor.nameExists( %type, %name ) )
		return true;

	return false;
}

// Generate a unique name from a given base by appending an integer
function ShapeEditor::getUniqueName( %this, %type, %name ) {
	for ( %idx = 1; %idx < 100; %idx++ ) {
		%uniqueName = %name @ %idx;

		if ( !%this.nameExists( %type, %uniqueName ) )
			break;
	}

	return %uniqueName;
}

function ShapeEditor::getProxyName( %this, %seqName ) {
	return "__proxy__" @ %seqName;
}

function ShapeEditor::getUnproxyName( %this, %proxyName ) {
	return strreplace( %proxyName, "__proxy__", "" );
}

function ShapeEditor::getBackupName( %this, %seqName ) {
	return "__backup__" @ %seqName;
}

// Check if this mesh name is a collision hint
function ShapeEditor::isCollisionMesh( %this, %name ) {
	return ( startswith( %name, "ColBox" ) ||
				startswith( %name, "ColSphere" ) ||
				startswith( %name, "ColCapsule" ) ||
				startswith( %name, "ColConvex" ) );
}

//
function ShapeEditor::getSequenceSource( %this, %seqName ) {
	%source = %this.shape.getSequenceSource( %seqName );
	// Use the sequence name as the source for DTS built-in sequences
	%src0 = getField( %source, 0 );
	%src1 = getField( %source, 1 );

	if ( %src0 $= %src1 )
		%source = setField( %source, 1, "" );

	if ( %src0 $= "" )
		%source = setField( %source, 0, %seqName );

	return %source;
}

// Recursively get names for a node and its children
function ShapeEditor::getNodeNames( %this, %nodeName, %names, %exclude ) {
	if ( %nodeName $= %exclude )
		return %names;

	%count = %this.shape.getNodeChildCount( %nodeName );

	for ( %i = 0; %i < %count; %i++ ) {
		%childName = %this.shape.getNodeChildName( %nodeName, %i );
		%names = %this.getNodeNames( %childName, %names, %exclude );
	}

	%names = %names TAB %nodeName;
	return trim( %names );
}

// Get the list of meshes for a particular object
function ShapeEditor::getObjectMeshList( %this, %name ) {
	%list = "";
	%count = %this.shape.getMeshCount( %name );

	for ( %i = 0; %i < %count; %i++ )
		%list = %list TAB %this.shape.getMeshName( %name, %i );

	return trim( %list );
}

// Get the list of meshes for a particular detail level
function ShapeEditor::getDetailMeshList( %this, %detSize ) {
	%list = "";
	%objCount = ShapeEditor.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = ShapeEditor.shape.getObjectName( %i );
		%meshCount = ShapeEditor.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%size = ShapeEditor.shape.getMeshSize( %objName, %j );

			if ( %size == %detSize )
				%list = %list TAB %this.shape.getMeshName( %objName, %j );
		}
	}

	return trim( %list );
}