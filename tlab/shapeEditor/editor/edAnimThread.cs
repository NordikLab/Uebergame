//==============================================================================
// TorqueLab -> ShapeEditor -> Threads and Animation
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Threads and Animation
//==============================================================================


function ShapeEdThreadWindow::onWake( %this ) {
	%this-->useTransitions.setValue( 1 );
	%this-->transitionTime.setText( "0.5" );
	%this-->transitionTo.clear();
	%this-->transitionTo.add( "synched position", 0 );
	%this-->transitionTo.add( "slider position", 1 );
	%this-->transitionTo.setSelected( 0 );
	%this-->transitionTarget.clear();
	%this-->transitionTarget.add( "plays during transition", 0 );
	%this-->transitionTarget.add( "pauses during transition", 1 );
	%this-->transitionTarget.setSelected( 0 );
}

// Update the GUI in response to the shape selection changing
function ShapeEdThreadWindow::update_onShapeSelectionChanged( %this ) {
	ShapeEdThreadList.clear();
	%this-->seqList.clear();
	%this-->seqList.addRow( 0, "<rootpose>" );
}

function ShapeEdAnimWIndow::threadPosToKeyframe( %this, %pos ) {
	if ( %this.usingProxySeq ) {
		%start = getWord( ShapeEdSeqSlider.range, 0 );
		%end = getWord( ShapeEdSeqSlider.range, 1 );
	} else {
		%start = ShapeEdAnimWindow.seqStartFrame;
		%end = ShapeEdAnimWindow.seqEndFrame;
	}

	return %start + ( %end - %start ) * %pos;
}

function ShapeEdAnimWindow::keyframeToThreadPos( %this, %frame ) {
	if ( %this.usingProxySeq ) {
		%start = getWord( ShapeEdSeqSlider.range, 0 );
		%end = getWord( ShapeEdSeqSlider.range, 1 );
	} else {
		%start = ShapeEdAnimWindow.seqStartFrame;
		%end = ShapeEdAnimWindow.seqEndFrame;
	}

	return ( %frame - %start ) / ( %end - %start );
}

function ShapeEdAnimWindow::setKeyframe( %this, %frame ) {
	ShapeEdSeqSlider.setValue( %frame );

	if ( ShapeEdThreadWindow-->transitionTo.getText() $= "synched position" )
		ShapeEdThreadSlider.setValue( %frame );

	// Update the position of the active thread => if outside the in/out range,
	// need to switch to the proxy sequence
	if ( !%this.usingProxySeq ) {
		if ( ( %frame < %this.seqStartFrame ) || ( %frame > %this.seqEndFrame) ) {
			%this.usingProxySeq = true;
			%proxyName = ShapeEditor.getProxyName( ShapeEdShapeView.getThreadSequence() );
			ShapeEdShapeView.setThreadSequence( %proxyName, 0, 0, false );
		}
	}

	ShapeEdShapeView.threadPos = %this.keyframeToThreadPos( %frame );
}

function ShapeEdAnimWindow::setNoProxySequence( %this ) {
	// no need to use the proxy sequence during playback
	if ( %this.usingProxySeq ) {
		%this.usingProxySeq = false;
		%seqName = ShapeEditor.getUnproxyName( ShapeEdShapeView.getThreadSequence() );
		ShapeEdShapeView.setThreadSequence( %seqName, 0, 0, false );
		ShapeEdShapeView.threadPos = %this.keyframeToThreadPos( ShapeEdSeqSlider.getValue() );
	}
}

function ShapeEdAnimWindow::togglePause( %this ) {
	if ( %this-->pauseBtn.getValue() == 0 ) {
		%this.lastDirBkwd = %this-->playBkwdBtn.getValue();
		%this-->pauseBtn.performClick();
	} else {
		%this.setNoProxySequence();

		if ( %this.lastDirBkwd )
			%this-->playBkwdBtn.performClick();
		else
			%this-->playFwdBtn.performClick();
	}
}

function ShapeEdAnimWindow::togglePingPong( %this ) {
	ShapeEdShapeView.threadPingPong = %this-->pingpong.getValue();

	if ( %this-->playFwdBtn.getValue() )
		%this-->playFwdBtn.performClick();
	else if ( %this-->playBkwdBtn.getValue() )
		%this-->playBkwdBtn.performClick();
}

function ShapeEdSeqSlider::onMouseDragged( %this ) {
	// Pause the active thread when the slider is dragged
	if ( ShapeEdAnimWindow-->pauseBtn.getValue() == 0 )
		ShapeEdAnimWindow-->pauseBtn.performClick();

	ShapeEdAnimWindow.setKeyframe( %this.getValue() );
}

function ShapeEdThreadSlider::onMouseDragged( %this ) {
	if ( ShapeEdThreadWindow-->transitionTo.getText() $= "synched position" ) {
		// Pause the active thread when the slider is dragged
		if ( ShapeEdAnimWindow-->pauseBtn.getValue() == 0 )
			ShapeEdAnimWindow-->pauseBtn.performClick();

		ShapeEdAnimWindow.setKeyframe( %this.getValue() );
	}
}

function ShapeEdShapeView::onThreadPosChanged( %this, %pos, %inTransition ) {
	// Update sliders
	%frame = ShapeEdAnimWindow.threadPosToKeyframe( %pos );
	if (isObject(ShapeEdSeqSlider))
		ShapeEdSeqSlider.setValue( %frame );

	if ( ShapeEdThreadWindow-->transitionTo.getText() $= "synched position" ) {
		ShapeEdThreadSlider.setValue( %frame );

		// Highlight the slider during transitions
		if ( %inTransition )
			ShapeEdThreadSlider.profile = GuiShapeEdTransitionSliderProfile;
		else
			ShapeEdThreadSlider.profile = ToolsSliderProfile;
	}
}

// Set the direction of the current thread (-1: reverse, 0: paused, 1: forward)
function ShapeEdAnimWindow::setThreadDirection( %this, %dir ) {
	// Update thread direction
	ShapeEdShapeView.threadDirection = %dir;

	// Sync the controls in the thread window
	switch ( %dir ) {
	case -1:
		ShapeEdThreadWindow-->playBkwdBtn.setStateOn( 1 );

	case 0:
		ShapeEdThreadWindow-->pauseBtn.setStateOn( 1 );

	case 1:
		ShapeEdThreadWindow-->playFwdBtn.setStateOn( 1 );
	}
}

// Set the sequence to play
function ShapeEdAnimWindow::setSequence( %this, %seqName ) {
	%this.usingProxySeq = false;

	if ( ShapeEdThreadWindow-->useTransitions.getValue() ) {
		%transTime = ShapeEdThreadWindow-->transitionTime.getText();

		if ( ShapeEdThreadWindow-->transitionTo.getText() $= "synched position" )
			%transPos = -1;
		else
			%transPos = %this.keyframeToThreadPos( ShapeEdThreadSlider.getValue() );

		%transPlay = ( ShapeEdThreadWindow-->transitionTarget.getText() $= "plays during transition" );
	} else {
		%transTime = 0;
		%transPos = 0;
		%transPlay = 0;
	}

	// No transition when sequence is not changing
	if ( %seqName $= ShapeEdShapeView.getThreadSequence() )
		%transTime = 0;

	if ( %seqName !$= "" ) {
		// To be able to effectively scrub through the animation, we need to have all
		// frames available, even if it was added with only a subset. If that is the
		// case, then create a proxy sequence that has all the frames instead.
		%sourceData = ShapeEditor.getSequenceSource( %seqName );
		%from = rtrim( getFields( %sourceData, 0, 1 ) );
		%startFrame = getField( %sourceData, 2 );
		%endFrame = getField( %sourceData, 3 );
		%frameCount = getField( %sourceData, 4 );

		if ( ( %startFrame != 0 ) || ( %endFrame != ( %frameCount-1 ) ) ) {
			%proxyName = ShapeEditor.getProxyName( %seqName );

			if ( ShapeEditor.shape.getSequenceIndex( %proxyName ) != -1 ) {
				ShapeEditor.shape.removeSequence( %proxyName );
				ShapeEdShapeView.refreshThreadSequences();
			}

			ShapeEditor.shape.addSequence( %from, %proxyName );
			// Limit the transition position to the in/out range
			%transPos = mClamp( %transPos, 0, 1 );
		}
	}

	ShapeEdShapeView.setThreadSequence( %seqName, %transTime, %transPos, %transPlay );
}

function ShapeEdAnimWindow::getTimelineBitmapPos( %this, %val, %width ) {
	%frameCount = getWord( ShapeEdSeqSlider.range, 1 );
	%pos_x = getWord( ShapeEdSeqSlider.getPosition(), 0 );
	%len_x = getWord( ShapeEdSeqSlider.getExtent(), 0 ) - %width;
	return %pos_x + ( ( %len_x * %val / %frameCount ) );
}

// Set the in or out sequence limit
function ShapeEdAnimWindow::setPlaybackLimit( %this, %limit, %val ) {
	// Determine where to place the in/out bar on the slider
	%thumbWidth = 8;    // width of the thumb bitmap
	%pos_x = %this.getTimelineBitmapPos( %val, %thumbWidth );

	if ( %limit $= "in" ) {
		%this.seqStartFrame = %val;
		%this-->seqIn.setText( %val );
		%this-->seqInBar.setPosition( %pos_x, 0 );
	} else {
		%this.seqEndFrame = %val;
		%this-->seqOut.setText( %val );
		%this-->seqOutBar.setPosition( %pos_x, 0 );
	}
}

function ShapeEdThreadWindow::onAddThread( %this ) {
	ShapeEdShapeView.addThread();
	ShapeEdThreadList.addRow( %this.threadID++, ShapeEdThreadList.rowCount() );
	ShapeEdThreadList.setSelectedRow( ShapeEdThreadList.rowCount()-1 );
}

function ShapeEdThreadWindow::onRemoveThread( %this ) {
	if ( ShapeEdThreadList.rowCount() > 1 ) {
		// Remove the selected thread
		%row = ShapeEdThreadList.getSelectedRow();
		ShapeEdShapeView.removeThread( %row );
		ShapeEdThreadList.removeRow( %row );
		// Update list (threads are always numbered 0-N)
		%rowCount = ShapeEdThreadList.rowCount();

		for ( %i = %row; %i < %rowCount; %i++ )
			ShapeEdThreadList.setRowById( ShapeEdThreadList.getRowId( %i ), %i );

		// Select the next thread
		if ( %row >= %rowCount )
			%row = %rowCount - 1;

		ShapeEdThreadList.setSelectedRow( %row );
	}
}

function ShapeEdThreadList::onSelect( %this, %row, %text ) {
	ShapeEdShapeView.activeThread = ShapeEdThreadList.getSelectedRow();
	// Select the active thread's sequence in the list
	%seqName = ShapeEdShapeView.getThreadSequence();

	if ( %seqName $= "" )
		%seqName = "<rootpose>";
	else if ( startswith( %seqName, "__proxy__" ) )
		%seqName = ShapeEditor.getUnproxyName( %seqName );

	%seqIndex = ShapeEdSequenceList.getItemIndex( %seqName );
	ShapeEdSequenceList.setSelectedRow( %seqIndex );

	// Update the playback controls
	switch ( ShapeEdShapeView.threadDirection ) {
	case -1:
		ShapeEdAnimWindow-->playBkwdBtn.performClick();

	case 0:
		ShapeEdAnimWindow-->pauseBtn.performClick();

	case 1:
		ShapeEdAnimWindow-->playFwdBtn.performClick();
	}

	SetToggleButtonValue( ShapeEdAnimWindow-->pingpong, ShapeEdShapeView.threadPingPong );
}
