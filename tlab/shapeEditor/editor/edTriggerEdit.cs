//==============================================================================
// TorqueLab -> ShapeEditor -> Trigger Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Trigger Editing
//==============================================================================


function ShapeEdPropWindow::onTriggerSelectionChanged( %this ) {
	%row = ShapeEdTriggerList.getSelectedRow();

	if ( %row > 0 ) { // skip header row
		%text = ShapeEdTriggerList.getRowText( %row );
		ShapeEdSequences-->triggerFrame.setActive( true );
		ShapeEdSequences-->triggerNum.setActive( true );
		ShapeEdSequences-->triggerOnOff.setActive( true );
		ShapeEdSequences-->triggerFrame.setText( getField( %text, 1 ) );
		ShapeEdSequences-->triggerNum.setText( getField( %text, 2 ) );
		ShapeEdSequences-->triggerOnOff.setValue( getField( %text, 3 ) $= "on" );
	} else {
		// No trigger selected
		ShapeEdSequences-->triggerFrame.setActive( false );
		ShapeEdSequences-->triggerNum.setActive( false );
		ShapeEdSequences-->triggerOnOff.setActive( false );
		ShapeEdSequences-->triggerFrame.setText( "" );
		ShapeEdSequences-->triggerNum.setText( "" );
		ShapeEdSequences-->triggerOnOff.setValue( 0 );
	}
}

function ShapeEdSequences::onEditName( %this ) {
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		%newName = %this-->seqName.getText();

		if ( %newName !$= "" )
			ShapeEditor.doRenameSequence( %seqName, %newName );
	}
}

function ShapeEdPropWindow::update_onTriggerAdded( %this, %seqName, %frame, %state ) {
	// --- SEQUENCES TAB ---
	// Add trigger to list if this sequence is selected
	if ( ShapeEdSequenceList.getSelectedName() $= %seqName )
		ShapeEdTriggerList.addItem( %frame, %state );
}

function ShapeEdPropWindow::update_onTriggerRemoved( %this, %seqName, %frame, %state ) {
	// --- SEQUENCES TAB ---
	// Remove trigger from list if this sequence is selected
	if ( ShapeEdSequenceList.getSelectedName() $= %seqName )
		ShapeEdTriggerList.removeItem( %frame, %state );
}

function ShapeEdTriggerList::getTriggerText( %this, %frame, %state ) {
	// First column is invisible and used only for sorting
	%sortKey = ( %frame * 1000 ) + ( mAbs( %state ) * 10 ) + ( ( %state > 0 ) ? 1 : 0 );
	return %sortKey TAB %frame TAB mAbs( %state ) TAB ( ( %state > 0 ) ? "on" : "off" );
}

function ShapeEdTriggerList::addItem( %this, %frame, %state ) {
	// Add to text list
	%row = %this.addRow( %this.triggerId, %this.getTriggerText( %frame, %state ) );
	%this.sortNumerical( 0, true );
	// Add marker to animation timeline
	%pos = ShapeEdAnimWindow.getTimelineBitmapPos( ShapeEdAnimWindow-->seqIn.getText() + %frame, 2 );
	%ctrl = new GuiBitmapCtrl() {
		internalName = "trigger" @ %this.triggerId;
		Profile = "ToolsDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = %pos SPC "0";
		Extent = "2 12";
		bitmap = "tlab/shapeEditor/images/trigger_marker";
	};
	ShapeEdAnimWindow.getObject(0).addGuiControl( %ctrl );
	%this.triggerId++;
}

function ShapeEdTriggerList::removeItem( %this, %frame, %state ) {
	// Remove from text list
	%row = %this.findTextIndex( %this.getTriggerText( %frame, %state ) );

	if ( %row > 0 ) {
		eval( "ShapeEdAnimWindow-->trigger" @ %this.getRowId( %row ) @ ".delete();" );
		%this.removeRow( %row );
	}
}

function ShapeEdTriggerList::removeAll( %this ) {
	%count = %this.rowCount();

	for ( %row = %count-1; %row > 0; %row-- ) {
		eval( "ShapeEdAnimWindow-->trigger" @ %this.getRowId( %row ) @ ".delete();" );
		%this.removeRow( %row );
	}
}

function ShapeEdTriggerList::updateItem( %this, %oldFrame, %oldState, %frame, %state ) {
	// Update text list entry
	%oldText = %this.getTriggerText( %oldFrame, %oldState );
	%row = %this.getSelectedRow();

	if ( ( %row <= 0 ) || ( %this.getRowText( %row ) !$= %oldText ) )
		%row = %this.findTextIndex( %oldText );

	if ( %row > 0 ) {
		%updatedId = %this.getRowId( %row );
		%newText = %this.getTriggerText( %frame, %state );
		%this.setRowById( %updatedId, %newText );
		// keep selected row the same
		%selectedId = %this.getSelectedId();
		%this.sortNumerical( 0, true );
		%this.setSelectedById( %selectedId );

		// Update animation timeline marker
		if ( %frame != %oldFrame ) {
			%pos = ShapeEdAnimWindow.getTimelineBitmapPos( ShapeEdAnimWindow-->seqIn.getText() + %frame, 2 );
			eval( "%ctrl = ShapeEdAnimWindow-->trigger" @ %updatedId @ ";" );
			%ctrl.position = %pos SPC "0";
		}
	}
}

function ShapeEdSequences::onAddTrigger( %this ) {
	// Can only add triggers if a sequence is selected
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Add a new trigger at the current frame
		%frame = mRound( ShapeEdSeqSlider.getValue() );
		%state = ShapeEdTriggerList.rowCount() % 30;
		ShapeEditor.doAddTrigger( %seqName, %frame, %state );
	}
}

function ShapeEdTriggerList::onDeleteSelection( %this ) {
	// Can only delete a trigger if a sequence and trigger are selected
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		%row = %this.getSelectedRow();

		if ( %row > 0 ) {
			%text = %this.getRowText( %row );
			%frame = getWord( %text, 1 );
			%state = getWord( %text, 2 );
			%state *= ( getWord( %text, 3 ) $= "on" ) ? 1 : -1;
			ShapeEditor.doRemoveTrigger( %seqName, %frame, %state );
		}
	}
}

function ShapeEdTriggerList::onEditSelection( %this ) {
	// Can only edit triggers if a sequence and trigger are selected
	%seqName = ShapeEdSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		%row = ShapeEdTriggerList.getSelectedRow();

		if ( %row > 0 ) {
			%text = %this.getRowText( %row );
			%oldFrame = getWord( %text, 1 );
			%oldState = getWord( %text, 2 );
			%oldState *= ( getWord( %text, 3 ) $= "on" ) ? 1 : -1;
			%frame = mRound( ShapeEdSequences-->triggerFrame.getText() );
			%state = mRound( mAbs( ShapeEdSequences-->triggerNum.getText() ) );
			%state *= ShapeEdSequences-->triggerOnOff.getValue() ? 1 : -1;

			if ( ( %frame >= 0 ) && ( %state != 0 ) )
				ShapeEditor.doEditTrigger( %seqName, %oldFrame, %oldState, %frame, %state );
		}
	}
}
