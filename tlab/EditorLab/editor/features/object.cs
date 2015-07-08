//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================


function EditorMount() {
	echo( "EditorMount" );
	%size = EWorldEditor.getSelectionSize();

	if ( %size != 2 )
		return;

	%a = EWorldEditor.getSelectedObject(0);
	%b = EWorldEditor.getSelectedObject(1);
	//%a.mountObject( %b, 0 );
	EWorldEditor.mountRelative( %a, %b );
}

function EditorUnmount() {
	echo( "EditorUnmount" );
	%obj = EWorldEditor.getSelectedObject(0);
	%obj.unmount();
}
//------------------------------------------------------------------------------
