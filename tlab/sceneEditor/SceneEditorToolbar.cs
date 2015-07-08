//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneEditorPlugin::initToolBar( %this ) {
	%menu = SceneEditorToolbar-->DropTypeMenu;
	%menu.clear();
	%selected = 0;

	foreach$(%type in $TLab_Object_DropTypes) {
		%menu.add(%type,%id++);

		if (EWorldEditor.dropType $= %type)
			%selected = %id;
	}

	%menu.setSelected(%selected,false);
}

//==============================================================================
function Lab::toggleObjectCenter( %this,%updateOnly ) {
	if (!%updateOnly)
		EWorldEditor.objectsUseBoxCenter = !EWorldEditor.objectsUseBoxCenter;

	if( EWorldEditor.objectsUseBoxCenter ) {
		SceneEditorToolbar-->centerObject.setBitmap("tlab/gui/icons/toolbar_assets/SelObjectCenter.png");
	} else {
		SceneEditorToolbar-->centerObject.setBitmap("tlab/gui/icons/toolbar_assets/SelObjectBounds.png");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleObjectTransform( %this ) {
	if (!%updateOnly) {
		if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			GlobalGizmoProfile.setFieldValue(alignment, World);
		else
			GlobalGizmoProfile.setFieldValue(alignment, Object);
	}

	if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
		SceneEditorToolbar-->objectTransform.setBitmap("tlab/gui/icons/toolbar_assets/TransformObject.png");
	} else {
		SceneEditorToolbar-->objectTransform.setBitmap("tlab/gui/icons/toolbar_assets/TransformWorld");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setSelectionLock( %this,%locked ) {
	EWorldEditor.lockSelection(%locked);
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setSelectionVisible( %this,%visible ) {
	EWorldEditor.hideSelection(!%visible);
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::toggleSnapSlider( %this,%sourceObj ) {
//Canvas.pushDialog(softSnapSizeSliderCtrlContainer);
	%srcPos = %sourceObj.getRealPosition();
	%srcPos.y += %sourceObj.extent.y;
	EOverlay.toggleSlider("2",%srcPos,"range \t 0.1 10 \n altCommand \t SceneEditorPlugin.onSnapSizeSliderChanged($ThisControl);");
}
//------------------------------------------------------------------------------
function SceneEditorPlugin::onSnapSizeSliderChanged(%this,%slider) {
	%value = mFloatLength(%slider.value,1);
	EWorldEditor.setSoftSnapSize(%value );
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::toggleGridSizeSlider( %this,%sourceObj ) {
//Canvas.pushDialog(softSnapSizeSliderCtrlContainer);
	%srcPos = %sourceObj.getRealPosition();
	%srcPos.y += %sourceObj.extent.y;
	%range = "0.1 10";
	%ticks = getTicksFromRange(%range,"0.1");
	EOverlay.toggleSlider("2",%srcPos,"range \t "@%range@" \n ticks \t "@%ticks@"\n altCommand \t SceneEditorPlugin.onGridSizeSliderChanged($ThisControl);");
}
//------------------------------------------------------------------------------
function SceneEditorPlugin::onGridSizeSliderChanged(%this,%slider) {
	%value = mFloatLength(%slider.value,1);
	EditorGuiToolbarStack-->WorldEditorGridSizeEdit.setText(%value);
	EWorldEditor.setGridSize(%value );
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------------
