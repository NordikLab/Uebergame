//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function TerrainEditor::getActionDescription( %this, %action ) {
	switch$( %action ) {
	case "brushAdjustHeight":
		return "Adjust terrain height up or down.";

	case "raiseHeight":
		return "Raise terrain height.";

	case "lowerHeight":
		return "Lower terrain height.";

	case "smoothHeight":
		return "Smooth terrain.";

	case "paintNoise":
		return "Modify terrain height using noise.";

	case "flattenHeight":
		return "Flatten terrain.";

	case "setHeight":
		return "Set terrain height to defined value.";

	case "setEmpty":
		return "Remove terrain collision.";

	case "clearEmpty":
		return "Add back terrain collision.";

	default:
		return "";
	}
}

/// This is only ment for terrain editing actions and not
/// processed actions or the terrain material painting action.
function TerrainEditor::switchAction( %this, %action ) {
	%actionDesc = %this.getActionDescription(%action);
	%this.currentMode = "paint";
	%this.selectionHidden = true;
	%this.currentAction = %action;
	%this.currentActionDesc = %actionDesc;
	%this.savedAction = %action;
	%this.savedActionDesc = %actionDesc;

	if (  %action $= "setEmpty" ||
						  %action $= "clearEmpty" ||
										 %action $= "setHeight" )
		%this.renderSolidBrush = true;
	else
		%this.renderSolidBrush = false;

	EditorGuiStatusBar.setInfo(%actionDesc);
	%this.setAction( %this.currentAction );
}

function TerrainEditor::onSmoothHeightmap( %this ) {
	if ( !%this.getActiveTerrain() )
		return;

	// Show the dialog first and let the user
	// set the smoothing parameters.
	// Now create the terrain smoothing action to
	// get the work done and perform later undos.
	%action = new TerrainSmoothAction();
	%action.smooth( %this.getActiveTerrain(), 1.0, 1 );
	%action.addToManager( Editor.getUndoManager() );
}

function TerrainEditor::onMaterialUndo( %this ) {
	// Update the gui to reflect the current materials.
	EPainter.updateLayers();
}