//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function EToolbarObjectCenterDropdown::toggle(%this,%caller) {
	%button = EditorGuiToolbarStack-->centerObject;

	if ( EToolbarObjectCenterDropdown.visible  ) {
		%button.setStateOn(false);
		EToolbarObjectCenterDecoy.setVisible(false);
		EToolbarObjectCenterDecoy.setActive(false);
		EToolbarObjectCenterDropdown.setVisible(false);
	} else {
		%pos = %button.getRealPosition();
		EToolbarObjectCenterDropdown.position.x = %pos.x - EToolbarObjectCenterDropdown.extent.x/2;
		EToolbarObjectCenterDropdown.position.y =0;
		%button.setStateOn(true);
		EToolbarObjectCenterDropdown.setVisible(true);
		EToolbarObjectCenterDecoy.setActive(true);
		EToolbarObjectCenterDecoy.setVisible(true);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function EToolbarObjectTransformDropdown::toggle() {
	%button = EditorGuiToolbarStack-->objectTransform;

	if ( EToolbarObjectTransformDropdown.visible  ) {
		%button.setStateOn(false);
		EToolbarObjectTransformDecoy.setVisible(false);
		EToolbarObjectTransformDecoy.setActive(false);
		EToolbarObjectTransformDropdown.setVisible(false);
	} else {
		%pos = %button.getRealPosition();
		EToolbarObjectTransformDropdown.position.x = %pos.x - EToolbarObjectCenterDropdown.extent.x/2;
		EToolbarObjectTransformDropdown.position.y =0;
		%button.setStateOn(true);
		EToolbarObjectTransformDropdown.setVisible(true);
		EToolbarObjectTransformDecoy.setActive(true);
		EToolbarObjectTransformDecoy.setVisible(true);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolbarObjectCenterDecoy::onMouseLeave() {
	if (EToolbarObjectCenterDecoy.visible)
		EToolbarObjectCenterDropdown.toggle();
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolbarObjectTransformDecoy::onMouseLeave() {
	if (EToolbarObjectTransformDecoy.visible)
		EToolbarObjectTransformDropdown.toggle();
}
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function EToolDlgCameraTypesToggle() {
	%button = EditorGuiToolbarStack-->cameraTypes;
	EOverlay.toggleDlg("CameraModeDlg");
	// Lab.toggleToolbarDialog(EToolCameraModeDlg,%button,"mouse");
	//Lab.showCameraViewContextMenu();
	return;
	//Mud-H FIXME
	%button = EditorGuiToolbarStack-->cameraTypes;

	if ( EToolDlgCameraTypes.visible  ) {
		%button.setStateOn(0);
		EToolDlgCameraTypesDecoy.setVisible(false);
		EToolDlgCameraTypesDecoy.setActive(false);
		EToolDlgCameraTypes.setVisible(false);
	} else {
		EToolDlgCameraTypes.position = %button.position;
		EToolDlgCameraTypes.position.y += %button.extent.y + 4;
		EToolDlgCameraTypes.setVisible(true);
		EToolDlgCameraTypesDecoy.setVisible(true);
		EToolDlgCameraTypesDecoy.setActive(true);
		%button.setStateOn(1);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolDlgCameraTypesDecoy::onMouseLeave() {
	EToolDlgCameraTypesToggle();
}
//------------------------------------------------------------------------------
//==============================================================================
function toggleSnappingOptions( %var ) {
	if( SnapToBar->objectSnapDownBtn.getValue() && SnapToBar->objectSnapBtn.getValue() ) {
		if( %var $= "terrain" ) {
			EWorldEditor.stickToGround = 1;
			EWorldEditor.setSoftSnap(false);
			ESnapOptionsTabBook.selectPage(0);
			SnapToBar->objectSnapBtn.setStateOn(0);
		} else {
			// soft snapping
			EWorldEditor.stickToGround = 0;
			EWorldEditor.setSoftSnap(true);
			ESnapOptionsTabBook.selectPage(1);
			SnapToBar->objectSnapDownBtn.setStateOn(0);
		}
	} else if( %var $= "terrain" && EWorldEditor.stickToGround == 0 ) {
		// Terrain Snapping
		EWorldEditor.stickToGround = 1;
		EWorldEditor.setSoftSnap(false);
		ESnapOptionsTabBook.selectPage(0);
		SnapToBar->objectSnapDownBtn.setStateOn(1);
		SnapToBar->objectSnapBtn.setStateOn(0);
	} else if( %var $= "soft" && EWorldEditor.getSoftSnap() == false ) {
		// Object Snapping
		EWorldEditor.stickToGround = 0;
		EWorldEditor.setSoftSnap(true);
		ESnapOptionsTabBook.selectPage(1);
		SnapToBar->objectSnapBtn.setStateOn(1);
		SnapToBar->objectSnapDownBtn.setStateOn(0);
	} else if( %var $= "grid" ) {
		EWorldEditor.setGridSnap( !EWorldEditor.getGridSnap() );
	} else {
		// No snapping.
		EWorldEditor.stickToGround = false;
		EWorldEditor.setGridSnap( false );
		EWorldEditor.setSoftSnap( false );
		SnapToBar->objectSnapDownBtn.setStateOn(0);
		SnapToBar->objectSnapBtn.setStateOn(0);
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
