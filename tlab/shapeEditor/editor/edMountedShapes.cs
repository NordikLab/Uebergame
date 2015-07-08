//==============================================================================
// TorqueLab -> ShapeEditor -> Mounted Shapes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Mounted Shapes
//==============================================================================

function ShapeEdMountWindow::onWake( %this ) {
	%this-->mountType.clear();
	%this-->mountType.add( "Object", 0 );
	%this-->mountType.add( "Image", 1 );
	%this-->mountType.add( "Wheel", 2 );
	%this-->mountType.setSelected( 1, false );
	%this-->mountSeq.clear();
	%this-->mountSeq.add( "<rootpose>", 0 );
	%this-->mountSeq.setSelected( 0, false );
	%this-->mountPlayBtn.setStateOn( false );

	// Only add the Browse entry the first time so we keep any files the user has
	// set up previously
	if ( ShapeEdMountShapeMenu.size() == 0 ) {
		ShapeEdMountShapeMenu.add( "Browse...", 0 );
		ShapeEdMountShapeMenu.setSelected( 0, false );
	}
}

function ShapeEdMountWindow::isMountableNode( %this, %nodeName ) {
	return ( startswith( %nodeName, "mount" ) || startswith( %nodeName, "hub" ) );
}

function ShapeEdMountWindow::update_onShapeSelectionChanged( %this ) {
	%this.unmountAll();
	// Initialise the dropdown menus
	%this-->mountNode.clear();
	%this-->mountNode.add( "<origin>" );
	%count = ShapeEditor.shape.getNodeCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeEditor.shape.getNodeName( %i );

		if ( %this.isMountableNode( %name ) )
			%this-->mountNode.add( %name );
	}

	%this-->mountNode.sort();
	%this-->mountNode.setFirstSelected();
	%this-->mountSeq.clear();
	%this-->mountSeq.add( "<rootpose>", 0 );
	%this-->mountSeq.setSelected( 0, false );
}

function ShapeEdMountWindow::update_onMountSelectionChanged( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		%text = %this-->mountList.getRowText( %row );
		%shapePath = getField( %text, 0 );
		ShapeEdMountShapeMenu.setText( %shapePath );
		%this-->mountNode.setText( getField( %text, 2 ) );
		%this-->mountType.setText( getField( %text, 3 ) );
		// Fill in sequence list
		%this-->mountSeq.clear();
		%this-->mountSeq.add( "<rootpose>", 0 );
		%tss = ShapeEditor.findConstructor( %shapePath );

		if ( !isObject( %tss ) )
			%tss = ShapeEditor.createConstructor( %shapePath );

		if ( isObject( %tss ) ) {
			%count = %tss.getSequenceCount();

			for ( %i = 0; %i < %count; %i++ )
				%this-->mountSeq.add( %tss.getSequenceName( %i ) );
		}

		// Select the currently playing sequence
		%slot = %row - 1;
		%seq = ShapeEdShapeView.getMountThreadSequence( %slot );
		%id = %this-->mountSeq.findText( %seq );

		if ( %id == -1 )
			%id = 0;

		%this-->mountSeq.setSelected( %id, false );
		ShapeEdMountSeqSlider.setValue( ShapeEdShapeView.getMountThreadPos( %slot ) );
		%this-->mountPlayBtn.setStateOn( ShapeEdShapeView.getMountThreadPos( %slot ) != 0 );
	}
}

function ShapeEdMountWindow::updateSelectedMount( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 )
		%this.mountShape( %row-1 );
}

function ShapeEdMountWindow::setMountThreadSequence( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		ShapeEdShapeView.setMountThreadSequence( %row-1, %this-->mountSeq.getText() );
		ShapeEdShapeView.setMountThreadDir( %row-1, %this-->mountPlayBtn.getValue() );
	}
}

function ShapeEdMountSeqSlider::onMouseDragged( %this ) {
	%row = ShapeEdMountWindow-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		ShapeEdShapeView.setMountThreadPos( %row-1, %this.getValue() );
		// Pause the sequence when the slider is dragged
		ShapeEdShapeView.setMountThreadDir( %row-1, 0 );
		ShapeEdMountWindow-->mountPlayBtn.setStateOn( false );
	}
}

function ShapeEdMountWindow::toggleMountThreadPlayback( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 )
		ShapeEdShapeView.setMountThreadDir( %row-1, %this-->mountPlayBtn.getValue() );
}

function ShapeEdMountShapeMenu::onSelect( %this, %id, %text ) {
	if ( %text $= "Browse..." ) {
		// Allow the user to browse for an external model file
		getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onBrowseSelect", %this.lastPath );
	} else {
		// Modify the current mount
		ShapeEdMountWindow.updateSelectedMount();
	}
}

function ShapeEdMountShapeMenu::onBrowseSelect( %this, %path ) {
	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;
	%this.setText( %path );

	// Add entry if unique
	if ( %this.findText( %path ) == -1 )
		%this.add( %path );

	ShapeEdMountWindow.updateSelectedMount();
}

function ShapeEdMountWindow::mountShape( %this, %slot ) {
	%model = ShapeEdMountShapeMenu.getText();
	%node = %this-->mountNode.getText();
	%type = %this-->mountType.getText();

	if ( %model $= "Browse..." )
		%model = "core/art/shapes/octahedron.dts";

	if ( ShapeEdShapeView.mountShape( %model, %node, %type, %slot ) ) {
		%rowText = %model TAB fileName( %model ) TAB %node TAB %type;

		if ( %slot == -1 ) {
			%id = %this.mounts++;
			%this-->mountList.addRow( %id, %rowText );
		} else {
			%id = %this-->mountList.getRowId( %slot+1 );
			%this-->mountList.setRowById( %id, %rowText );
		}

		%this-->mountList.setSelectedById( %id );
	} else {
		LabMsgOK( "Error", "Failed to mount \"" @ %model @ "\". Check the console for error messages.", "" );
	}
}

function ShapeEdMountWindow::unmountShape( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		ShapeEdShapeView.unmountShape( %row-1 );
		%this-->mountList.removeRow( %row );
		// Select the next row (if any)
		%count = %this-->mountList.rowCount();

		if ( %row >= %count )
			%row = %count-1;

		if ( %row > 0 )
			%this-->mountList.setSelectedRow( %row );
	}
}

function ShapeEdMountWindow::unmountAll( %this ) {
	ShapeEdShapeView.unmountAll();
	%this-->mountList.clear();
	%this-->mountList.addRow( -1, "FullPath" TAB "Filename" TAB "Node" TAB "Type" );
	%this-->mountList.setRowActive( -1, false );
}