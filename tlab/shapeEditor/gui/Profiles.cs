//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//-----------------------------------------------------------------------------
// Shape Editor Profiles
//-----------------------------------------------------------------------------

singleton GuiControlProfile(GuiShapeEdScrollProfile : ToolsScrollProfile) {
	// Don't clear the scroll area (since we need to be able to see the GuiContainer
	// underneath that provides the fill color for the header row)
	opaque = false;
	category = "Editor";
};

singleton GuiControlProfile(GuiShapeEdTextListProfile : ToolsTextBase_List) {
	// Customise the not-active font used for the header row
	fontColorNA = "75 75 75";
	category = "Editor";
};

singleton GuiControlProfile(GuiShapeEdRolloutProfile : GuiInspectorRolloutProfile0) {
	bitmap = "tlab/gui/icons/default/classImages/rollout";
	category = "Editor";
};

singleton GuiControlProfile( GuiShapeEdTransitionSliderProfile ) {
	bitmap = "tlab/shapeEditor/images/transition_slider";
	category = "Core";
};
