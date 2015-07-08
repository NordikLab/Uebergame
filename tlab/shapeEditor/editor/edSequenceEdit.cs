//==============================================================================
// TorqueLab -> ShapeEditor -> Sequence Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Sequence Editing
//==============================================================================


function ShapeEdPropWindow::onWake( %this ) {
	ShapeEdTriggerList.triggerId = 1;	
	ShapeEdTriggerList.clear();	
	ShapeEdTriggerList.addRow( -1, "-1" TAB "Frame" TAB "Trigger" TAB "State" );
	ShapeEdTriggerList.setRowActive( -1, false );
}

function ShapeEdPropWindow::update_onSeqSelectionChanged( %this ) {
	// Sync the Thread window sequence selection
	%row = ShapeEdSequenceList.getSelectedRow();

	if ( ShapeEdThreadWindow-->seqList.getSelectedRow() != ( %row-1 ) ) {
		ShapeEdThreadWindow-->seqList.setSelectedRow( %row-1 );
		return;  // selecting a sequence in the Thread window will re-call this function
	}

	ShapeEdSeqFromMenu.clear();
	ShapeEdSequences-->blendSeq.clear();
	// Clear the trigger list
	ShapeEdTriggerList.removeAll();
	// Update the active sequence data
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Enable delete button and edit boxes
		if ( ShapeEdSeqNodeTabBook.activePage $= "Seq" )
			ShapeEdPropWindow-->deleteBtn.setActive( true );

		ShapeEdSequences-->seqName.setActive( true );
		ShapeEdSequences-->blendFlag.setActive( true );
		ShapeEdSequences-->cyclicFlag.setActive( true );
		ShapeEdSequences-->priority.setActive( true );
		ShapeEdSequences-->addTriggerBtn.setActive( true );
		ShapeEdSequences-->deleteTriggerBtn.setActive( true );
		// Initialise the sequence properties
		%blendData = ShapeEditor.shape.getSequenceBlend( %seqName );
		ShapeEdSequences-->seqName.setText( %seqName );
		ShapeEdSequences-->cyclicFlag.setValue( ShapeEditor.shape.getSequenceCyclic( %seqName ) );
		ShapeEdSequences-->blendFlag.setValue( getField( %blendData, 0 ) );
		ShapeEdSequences-->priority.setText( ShapeEditor.shape.getSequencePriority( %seqName ) );
		// 'From' and 'Blend' sequence menus
		ShapeEdSeqFromMenu.add( "Browse..." );
		%count = ShapeEdSequenceList.rowCount();

		for ( %i = 2; %i < %count; %i++ ) { // skip header row and <rootpose>
			%name = ShapeEdSequenceList.getItemName( %i );

			if ( %name !$= %seqName ) {
				ShapeEdSeqFromMenu.add( %name );
				ShapeEdSequences-->blendSeq.add( %name );
			}
		}

		ShapeEdSequences-->blendSeq.setText( getField( %blendData, 1 ) );
		ShapeEdSequences-->blendFrame.setText( getField( %blendData, 2 ) );
		%this.syncPlaybackDetails();
		// Triggers (must occur after syncPlaybackDetails is called so the slider range is correct)
		%count = ShapeEditor.shape.getTriggerCount( %seqName );

		for ( %i = 0; %i < %count; %i++ ) {
			%trigger = ShapeEditor.shape.getTrigger( %seqName, %i );
			ShapeEdTriggerList.addItem( getWord( %trigger, 0 ), getWord( %trigger, 1 ) );
		}
	} else {
		// Disable delete button and edit boxes
		if ( ShapeEdSeqNodeTabBook.activePage $= "Seq" )
			ShapeEdPropWindow-->deleteBtn.setActive( false );

		ShapeEdSequences-->seqName.setActive( false );
		ShapeEdSequences-->blendFlag.setActive( false );
		ShapeEdSequences-->cyclicFlag.setActive( false );
		ShapeEdSequences-->priority.setActive( false );
		ShapeEdSequences-->addTriggerBtn.setActive( false );
		ShapeEdSequences-->deleteTriggerBtn.setActive( false );
		// Clear sequence properties
		ShapeEdSequences-->seqName.setText( "" );
		ShapeEdSequences-->cyclicFlag.setValue( 0 );
		ShapeEdSequences-->blendSeq.setText( "" );
		ShapeEdSequences-->blendFlag.setValue( 0 );
		ShapeEdSequences-->priority.setText( 0 );
		%this.syncPlaybackDetails();
	}

	%this.onTriggerSelectionChanged();
	// ShapeEdSequences-->sequenceListHeader.setExtent( getWord( ShapeEdSequenceList.extent, 0 ) SPC "19" );
	// Reset current frame
	//ShapeEdAnimWindow.setKeyframe( ShapeEdAnimWindow-->seqIn.getText() );
}

// Update the GUI in response to a sequence being added
function ShapeEdPropWindow::update_onSequenceAdded( %this, %seqName, %oldIndex ) {
	// --- MISC ---
	ShapeEdSelectWindow.updateHints();

	// --- SEQUENCES TAB ---
	if ( %oldIndex == -1 ) {
		// This is a brand new sequence => add it to the list and make it the
		// current selection
		%row = ShapeEdSequenceList.insertItem( %seqName, ShapeEdSequenceList.rowCount() );
		ShapeEdSequenceList.scrollVisible( %row );
		ShapeEdSequenceList.setSelectedRow( %row );
	} else {
		// This sequence has been un-deleted => add it back to the list at the
		// original position
		ShapeEdSequenceList.insertItem( %seqName, %oldIndex );
	}
}

function ShapeEdPropWindow::update_onSequenceRemoved( %this, %seqName ) {
	// --- MISC ---
	ShapeEdSelectWindow.updateHints();
	// --- SEQUENCES TAB ---
	%isSelected = ( ShapeEdSequenceList.getSelectedName() $= %seqName );
	ShapeEdSequenceList.removeItem( %seqName );

	if ( %isSelected )
		ShapeEdPropWindow.update_onSeqSelectionChanged();

	// --- THREADS WINDOW ---
	ShapeEdShapeView.refreshThreadSequences();
}

function ShapeEdPropWindow::update_onSequenceRenamed( %this, %oldName, %newName ) {
	// --- MISC ---
	ShapeEdSelectWindow.updateHints();
	// Rename the proxy sequence as well
	%oldProxy = ShapeEditor.getProxyName( %oldName );
	%newProxy = ShapeEditor.getProxyName( %newName );

	if ( ShapeEditor.shape.getSequenceIndex( %oldProxy ) != -1 )
		ShapeEditor.shape.renameSequence( %oldProxy, %newProxy );

	// --- SEQUENCES TAB ---
	ShapeEdSequenceList.editColumn( %oldName, 0, %newName );

	if ( ShapeEdSequenceList.getSelectedName() $= %newName )
		ShapeEdSequences-->seqName.setText( %newName );

	// --- THREADS WINDOW ---
	// Update any threads that use this sequence
	%active = ShapeEdShapeView.activeThread;

	for ( %i = 0; %i < ShapeEdShapeView.getThreadCount(); %i++ ) {
		ShapeEdShapeView.activeThread = %i;

		if ( ShapeEdShapeView.getThreadSequence() $= %oldName )
			ShapeEdShapeView.setThreadSequence( %newName, 0, ShapeEdShapeView.threadPos, 0 );
		else if ( ShapeEdShapeView.getThreadSequence() $= %oldProxy )
			ShapeEdShapeView.setThreadSequence( %newProxy, 0, ShapeEdShapeView.threadPos, 0 );
	}

	ShapeEdShapeView.activeThread = %active;
}

function ShapeEdPropWindow::update_onSequenceCyclicChanged( %this, %seqName, %cyclic ) {
	// --- MISC ---
	// Apply the same transformation to the proxy animation if necessary
	%proxyName = ShapeEditor.getProxyName( %seqName );

	if ( ShapeEditor.shape.getSequenceIndex( %proxyName ) != -1 )
		ShapeEditor.shape.setSequenceCyclic( %proxyName, %cyclic );

	// --- SEQUENCES TAB ---
	ShapeEdSequenceList.editColumn( %seqName, 1, %cyclic ? "yes" : "no" );

	if ( ShapeEdSequenceList.getSelectedName() $= %seqName )
		ShapeEdSequences-->cyclicFlag.setStateOn( %cyclic );
}

function ShapeEdPropWindow::update_onSequenceBlendChanged( %this, %seqName, %blend,
		%oldBlendSeq, %oldBlendFrame, %blendSeq, %blendFrame ) {
	// --- MISC ---
	// Apply the same transformation to the proxy animation if necessary
	%proxyName = ShapeEditor.getProxyName( %seqName );

	if ( ShapeEditor.shape.getSequenceIndex( %proxyName ) != -1 ) {
		if ( %blend && %oldBlend )
			ShapeEditor.shape.setSequenceBlend( %proxyName, false, %oldBlendSeq, %oldBlendFrame );

		ShapeEditor.shape.setSequenceBlend( %proxyName, %blend, %blendSeq, %blendFrame );
	}

	ShapeEdShapeView.updateNodeTransforms();
	// --- SEQUENCES TAB ---
	ShapeEdSequenceList.editColumn( %seqName, 2, %blend ? "yes" : "no" );

	if ( ShapeEdSequenceList.getSelectedName() $= %seqName ) {
		ShapeEdSequences-->blendFlag.setStateOn( %blend );
		ShapeEdSequences-->blendSeq.setText( %blendSeq );
		ShapeEdSequences-->blendFrame.setText( %blendFrame );
	}
}

function ShapeEdPropWindow::update_onSequencePriorityChanged( %this, %seqName ) {
	// --- SEQUENCES TAB ---
	%priority = ShapeEditor.shape.getSequencePriority( %seqName );
	ShapeEdSequenceList.editColumn( %seqName, 4, %priority );

	if ( ShapeEdSequenceList.getSelectedName() $= %seqName )
		ShapeEdSequences-->priority.setText( %priority );
}

function ShapeEdPropWindow::update_onSequenceGroundSpeedChanged( %this, %seqName ) {
	// nothing to do yet
}

function ShapeEdPropWindow::syncPlaybackDetails( %this ) {
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Show sequence in/out bars
		ShapeEdAnimWindow-->seqInBar.setVisible( true );
		ShapeEdAnimWindow-->seqOutBar.setVisible( true );
		// Sync playback controls
		%sourceData = ShapeEditor.getSequenceSource( %seqName );
		%seqFrom = rtrim( getFields( %sourceData, 0, 1 ) );
		%seqStart = getField( %sourceData, 2 );
		%seqEnd = getField( %sourceData, 3 );
		%seqFromTotal = getField( %sourceData, 4 );

		// Display the original source for edited sequences
		if ( startswith( %seqFrom, "__backup__" ) ) {
			%backupData = ShapeEditor.getSequenceSource( getField( %seqFrom, 0 ) );
			%seqFrom = rtrim( getFields( %backupData, 0, 1 ) );
		}

		ShapeEdSeqFromMenu.setText( %seqFrom );
		ShapeEdSeqFromMenu.tooltip = ShapeEdSeqFromMenu.getText();   // use tooltip to show long names
		ShapeEdSequences-->startFrame.setText( %seqStart );
		ShapeEdSequences-->endFrame.setText( %seqEnd );
		%val = ShapeEdSeqSlider.getValue() / getWord( ShapeEdSeqSlider.range, 1 );
		ShapeEdSeqSlider.range = "0" SPC ( %seqFromTotal-1 );
		ShapeEdSeqSlider.setValue( %val * getWord( ShapeEdSeqSlider.range, 1 ) );
		ShapeEdThreadSlider.range = ShapeEdSeqSlider.range;
		ShapeEdThreadSlider.setValue( ShapeEdSeqSlider.value );
		ShapeEdAnimWindow.setSequence( %seqName );
		ShapeEdAnimWindow.setPlaybackLimit( "in", %seqStart );
		ShapeEdAnimWindow.setPlaybackLimit( "out", %seqEnd );
	} else {
		// Hide sequence in/out bars
		ShapeEdAnimWindow-->seqInBar.setVisible( false );
		ShapeEdAnimWindow-->seqOutBar.setVisible( false );
		ShapeEdSeqFromMenu.setText( "" );
		ShapeEdSeqFromMenu.tooltip = "";
		ShapeEdSequences-->startFrame.setText( 0 );
		ShapeEdSequences-->endFrame.setText( 0 );
		ShapeEdSeqSlider.range = "0 1";
		ShapeEdSeqSlider.setValue( 0 );
		ShapeEdThreadSlider.range = ShapeEdSeqSlider.range;
		ShapeEdThreadSlider.setValue( ShapeEdSeqSlider.value );
		ShapeEdAnimWindow.setPlaybackLimit( "in", 0 );
		ShapeEdAnimWindow.setPlaybackLimit( "out", 1 );
		ShapeEdAnimWindow.setSequence( "" );
	}
}

function ShapeEdSequences::onEditSeqInOut( %this, %type, %val ) {
	%frameCount = getWord( ShapeEdSeqSlider.range, 1 );
	// Force value to a frame index within the slider range
	%val = mRound( %val );

	if ( %val < 0 )
		%val = 0;

	if ( %val > %frameCount )
		%val = %frameCount;

	// Enforce 'in' value must be < 'out' value
	if ( %type $= "in" ) {
		if ( %val >= %this-->endFrame.getText() )
			%val = %this-->endFrame.getText() - 1;

		%this-->startFrame.setText( %val );
	} else {
		if ( %val <= %this-->startFrame.getText() )
			%val = %this-->startFrame.getText() + 1;

		%this-->endFrame.setText( %val );
	}

	%this.onEditSequenceSource( "" );
}

function ShapeEdSequences::onEditSequenceSource( %this, %from ) {
	// ignore for shapes without sequences
	if (ShapeEditor.shape.getSequenceCount() == 0)
		return;

	%start = %this-->startFrame.getText();
	%end = %this-->endFrame.getText();

	if ( ( %start !$= "" ) && ( %end !$= "" ) ) {
		%seqName = ShapeEdSequenceList.getSelectedName();
		%oldSource = ShapeEditor.getSequenceSource( %seqName );

		if ( %from $= "" )
			%from = rtrim( getFields( %oldSource, 0, 0 ) );

		if ( getFields( %oldSource, 0, 3 ) !$= ( %from TAB "" TAB %start TAB %end ) )
			ShapeEditor.doEditSeqSource( %seqName, %from, %start, %end );
	}
}

function ShapeEdSequences::onToggleCyclic( %this ) {
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		%cyclic = %this-->cyclicFlag.getValue();
		ShapeEditor.doEditCyclic( %seqName, %cyclic );
	}
}

function ShapeEdSequences::onEditPriority( %this ) {
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		%newPriority = %this-->priority.getText();

		if ( %newPriority !$= "" )
			ShapeEditor.doEditSequencePriority( %seqName, %newPriority );
	}
}

function ShapeEdSequences::onEditBlend( %this ) {
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Get the blend flags (current and new)
		%oldBlendData = ShapeEditor.shape.getSequenceBlend( %seqName );
		%oldBlend = getField( %oldBlendData, 0 );
		%blend = %this-->blendFlag.getValue();

		// Ignore changes to the blend reference for non-blend sequences
		if ( !%oldBlend && !%blend )
			return;

		// OK - we're trying to change the blend properties of this sequence. The
		// new reference sequence and frame must be set.
		%blendSeq = %this-->blendSeq.getText();
		%blendFrame = %this-->blendFrame.getText();

		if ( ( %blendSeq $= "" ) || ( %blendFrame $= "" ) ) {
			LabMsgOK( "Blend reference not set", "The blend reference sequence and " @
						 "frame must be set before changing the blend flag or frame." );
			ShapeEdSequences-->blendFlag.setStateOn( %oldBlend );
			return;
		}

		// Get the current blend properties (use new values if not specified)
		%oldBlendSeq = getField( %oldBlendData, 1 );

		if ( %oldBlendSeq $= "" )
			%oldBlendSeq = %blendSeq;

		%oldBlendFrame = getField( %oldBlendData, 2 );

		if ( %oldBlendFrame $= "" )
			%oldBlendFrame = %blendFrame;

		// Check if there is anything to do
		if ( ( %oldBlend TAB %oldBlendSeq TAB %oldBlendFrame ) !$= ( %blend TAB %blendSeq TAB %blendFrame ) )
			ShapeEditor.doEditBlend( %seqName, %blend, %blendSeq, %blendFrame );
	}
}

function ShapeEdSequences::onAddSequence( %this, %name ) {
	if ( %name $= "" )
		%name = ShapeEditor.getUniqueName( "sequence", "mySequence" );

	// Use the currently selected sequence as the base
	%from = ShapeEdSequenceList.getSelectedName();
	%row = ShapeEdSequenceList.getSelectedRow();

	if ( ( %row < 2 ) && ( ShapeEdSequenceList.rowCount() > 2 ) )
		%row = 2;

	if ( %from $= "" ) {
		// No sequence selected => open dialog to browse for one
		getLoadFilename( "DSQ Files|*.dsq|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onAddSequenceFromBrowse", ShapeEdFromMenu.lastPath );
		return;
	} else {
		// Add the new sequence
		%start = ShapeEdSequences-->startFrame.getText();
		%end = ShapeEdSequences-->endFrame.getText();
		ShapeEditor.doAddSequence( %name, %from, %start, %end );
	}
}

function ShapeEdSequences::onAddSequenceFromBrowse( %this, %path ) {
	// Add a new sequence from the browse path
	%path = makeRelativePath( %path, getMainDotCSDir() );
	ShapeEdFromMenu.lastPath = %path;
	%name = ShapeEditor.getUniqueName( "sequence", "mySequence" );
	ShapeEditor.doAddSequence( %name, %path, 0, -1 );
}

// Delete the selected sequence
function ShapeEdSequences::onDeleteSequence( %this ) {
	%row = ShapeEdSequenceList.getSelectedRow();

	if ( %row != -1 ) {
		%seqName = ShapeEdSequenceList.getItemName( %row );
		ShapeEditor.doRemoveShapeData( "Sequence", %seqName );
	}
}

// Get the name of the currently selected sequence
function ShapeEdSequenceList::getSelectedName( %this ) {
	%row = %this.getSelectedRow();
	return ( %row > 1 ) ? %this.getItemName( %row ) : "";    // ignore header row
}

// Get the sequence name from the indexed row
function ShapeEdSequenceList::getItemName( %this, %row ) {
	return getField( %this.getRowText( %row ), 0 );
}

// Get the index in the list of the sequence with the given name
function ShapeEdSequenceList::getItemIndex( %this, %name ) {
	for ( %i = 1; %i < %this.rowCount(); %i++ ) { // ignore header row
		if ( %this.getItemName( %i ) $= %name )
			return %i;
	}

	return -1;
}

// Change one of the fields in the sequence list
function ShapeEdSequenceList::editColumn( %this, %name, %col, %text ) {
	%row = %this.getItemIndex( %name );
	%rowText = setField( %this.getRowText( %row ), %col, %text );
	// Update the Properties and Thread sequence lists
	%id = %this.getRowId( %row );

	if ( %col == 0 )
		ShapeEdThreadWindow-->seqList.setRowById( %id, %text );   // Sync name in Thread window

	%this.setRowById( %id, %rowText );
}

function ShapeEdSequenceList::addItem( %this, %name ) {
	return %this.insertItem( %name, %this.rowCount() );
}

function ShapeEdSequenceList::insertItem( %this, %name, %index ) {
	%cyclic = ShapeEditor.shape.getSequenceCyclic( %name ) ? "yes" : "no";
	%blend = getField( ShapeEditor.shape.getSequenceBlend( %name ), 0 ) ? "yes" : "no";
	%frameCount = ShapeEditor.shape.getSequenceFrameCount( %name );
	%priority = ShapeEditor.shape.getSequencePriority( %name );
	// Add the item to the Properties and Thread sequence lists
	%this.seqId++; // use this to keep the row IDs synchronised
	ShapeEdThreadWindow-->seqList.addRow( %this.seqId, %name, %index-1 );   // no header row
	return %this.addRow( %this.seqId, %name TAB %cyclic TAB %blend TAB %frameCount TAB %priority, %index );
}

function ShapeEdSequenceList::removeItem( %this, %name ) {
	%index = %this.getItemIndex( %name );

	if ( %index >= 0 ) {
		%this.removeRow( %index );
		ShapeEdThreadWindow-->seqList.removeRow( %index-1 );   // no header row
	}
}

function ShapeEdSeqFromMenu::onSelect( %this, %id, %text ) {
	if ( %text $= "Browse..." ) {
		// Reset menu text
		%seqName = ShapeEdSequenceList.getSelectedName();
		%seqFrom = rtrim( getFields( ShapeEditor.getSequenceSource( %seqName ), 0, 1 ) );
		%this.setText( %seqFrom );
		// Allow the user to browse for an external source of animation data
		getLoadFilename( "DSQ Files|*.dsq|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onBrowseSelect", %this.lastPath );
	} else {
		ShapeEdSequences.onEditSequenceSource( %text );
	}
}

function ShapeEdSeqFromMenu::onBrowseSelect( %this, %path ) {
	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;
	%this.setText( %path );
	ShapeEdSequences.onEditSequenceSource( %path );
}
