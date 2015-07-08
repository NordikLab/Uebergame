//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function alphaIconCompare( %a, %b ) {
	if ( %a.class $= "CreatorFolderIconBtn" )
		if ( %b.class !$= "CreatorFolderIconBtn" )
			return -1;

	if ( %b.class $= "CreatorFolderIconBtn" )
		if ( %a.class !$= "CreatorFolderIconBtn" )
			return 1;

	%result = stricmp( %a.text, %b.text );
	return %result;
}

// Generic create object helper for use from the console.

function genericCreateObject( %class ) {
	if ( !isClass( %class ) ) {
		warn( "createObject( " @ %class @ " ) - Was not a valid class." );
		return;
	}

	%cmd = "return new " @ %class @ "();";
	%obj = SceneCreatorWindow.createObject( %cmd );
	// In case the caller wants it.
	return %obj;
}