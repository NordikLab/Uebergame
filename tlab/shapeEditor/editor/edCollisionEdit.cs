//==============================================================================
// TorqueLab -> ShapeEditor -> Collision editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Collision editing
//==============================================================================


function ShapeEdColWindow::onWake( %this ) {
	%this-->colType.clear();
	%this-->colType.add( "Box" );
	%this-->colType.add( "Sphere" );
	%this-->colType.add( "Capsule" );
	%this-->colType.add( "10-DOP X" );
	%this-->colType.add( "10-DOP Y" );
	%this-->colType.add( "10-DOP Z" );
	%this-->colType.add( "18-DOP" );
	%this-->colType.add( "26-DOP" );
	%this-->colType.add( "Convex Hulls" );
}

function ShapeEdColWindow::update_onShapeSelectionChanged( %this ) {
	%this.lastColSettings = "" TAB "Bounds";
	// Initialise collision mesh target list
	%this-->colTarget.clear();
	%this-->colTarget.add( "Bounds" );
	%objCount = ShapeEditor.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ )
		%this-->colTarget.add( ShapeEditor.shape.getObjectName( %i ) );

	%this-->colTarget.setSelected( %this-->colTarget.findText( "Bounds" ), false );
}

function ShapeEdColWindow::update_onCollisionChanged( %this ) {
	// Sync collision settings
	%colData = %this.lastColSettings;
	%typeId = %this-->colType.findText( getField( %colData, 0 ) );
	%this-->colType.setSelected( %typeId, false );
	%targetId = %this-->colTarget.findText( getField( %colData, 1 ) );
	%this-->colTarget.setSelected( %targetId, false );

	if ( %this-->colType.getText() $= "Convex Hulls" ) {
		%this-->hullInactive.setVisible( false );
		%this-->hullDepth.setValue( getField( %colData, 2 ) );
		%this-->hullDepthText.setText( mFloor( %this-->hullDepth.getValue() ) );
		%this-->hullMergeThreshold.setValue( getField( %colData, 3 ) );
		%this-->hullMergeText.setText( mFloor( %this-->hullMergeThreshold.getValue() ) );
		%this-->hullConcaveThreshold.setValue( getField( %colData, 4 ) );
		%this-->hullConcaveText.setText( mFloor( %this-->hullConcaveThreshold.getValue() ) );
		%this-->hullMaxVerts.setValue( getField( %colData, 5 ) );
		%this-->hullMaxVertsText.setText( mFloor( %this-->hullMaxVerts.getValue() ) );
		%this-->hullMaxBoxError.setValue( getField( %colData, 6 ) );
		%this-->hullMaxBoxErrorText.setText( mFloor( %this-->hullMaxBoxError.getValue() ) );
		%this-->hullMaxSphereError.setValue( getField( %colData, 7 ) );
		%this-->hullMaxSphereErrorText.setText( mFloor( %this-->hullMaxSphereError.getValue() ) );
		%this-->hullMaxCapsuleError.setValue( getField( %colData, 8 ) );
		%this-->hullMaxCapsuleErrorText.setText( mFloor( %this-->hullMaxCapsuleError.getValue() ) );
	} else {
		%this-->hullInactive.setVisible( true );
	}
}

function ShapeEdColWindow::editCollision( %this ) {
	// If the shape already contains a collision detail size-1, warn the user
	// that it will be removed
	if ( ( ShapeEditor.shape.getDetailLevelIndex( -1 ) >= 0 ) &&
			( getField(%this.lastColSettings, 0) $= "" ) ) {
		LabMsgYesNo( "Warning", "Existing collision geometry at detail size " @
						 "-1 will be removed, and this cannot be undone. Do you want to continue?",
						 "ShapeEdColWindow.editCollisionOK();", "" );
	} else {
		%this.editCollisionOK();
	}
}

function ShapeEdColWindow::editCollisionOK( %this ) {
	%type = %this-->colType.getText();
	%target = %this-->colTarget.getText();
	%depth = %this-->hullDepth.getValue();
	%merge = %this-->hullMergeThreshold.getValue();
	%concavity = %this-->hullConcaveThreshold.getValue();
	%maxVerts = %this-->hullMaxVerts.getValue();
	%maxBox = %this-->hullMaxBoxError.getValue();
	%maxSphere = %this-->hullMaxSphereError.getValue();
	%maxCapsule = %this-->hullMaxCapsuleError.getValue();
	ShapeEditor.doEditCollision( %type, %target, %depth, %merge, %concavity, %maxVerts,
										  %maxBox, %maxSphere, %maxCapsule );
}
