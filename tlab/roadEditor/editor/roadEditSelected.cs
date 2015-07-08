//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Road Object Selected/UnSelected
//==============================================================================
//==============================================================================
function RoadEditorGui::onRoadSelected( %this, %road ) {
	%this.road = %road;

	// Update the materialEditorList
	if(isObject( %road )) {
		$Lab::materialEditorList = %road.getId();
		RoadEditorPlugin.selectedRoad = %road;
		RoadEditorPlugin.selectedMaterial = %road.Material;
		RoadEditorToolbar-->changeActiveMaterialBtn.active = 1;
		Lab.getDecalRoadNodes();
	} else
		%this.noRoadSelected();

	RoadInspector.inspect( %road );
	RoadTreeView.buildVisibleTree(true);

	if( RoadTreeView.getSelectedObject() != %road ) {
		RoadTreeView.clearSelection();
		%treeId = RoadTreeView.findItemByObjectId( %road );
		RoadTreeView.selectItem( %treeId );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::noRoadSelected( %this ) {
	%this.clearRoadNodesData();
	RoadEditorPlugin.selectedRoad = "";
	RoadEditorPlugin.selectedMaterial = "No road selected";
	RoadEditorToolbar-->changeActiveMaterialBtn.active = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
// Selected Road Material Functions
//==============================================================================
//==============================================================================
function RoadEditorGui::changeActiveMaterial( %this, %toMaterial ) {
	if (!isObject(RoadEditorPlugin.selectedRoad)) {
		LabMsgOk("No active road","You need to have a road selected to change the material");
		return;
	}

	materialSelector.showDialog("RoadEditorGui.changeActiveMaterialCallback");
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::changeActiveMaterialCallback( %this, %toMaterial ) {
	if (!isObject(RoadEditorPlugin.selectedRoad))
		return;

	%roadObj = RoadEditorPlugin.selectedRoad;
	%fromMaterial = %roadObj.Material;
	RoadInspector.setObjectField( "Material", %toMaterial.getName());
	RoadEditorPlugin.selectedMaterial = %toMaterial.getName();
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// RoadObject Node Manipulation
//==============================================================================
//==============================================================================
function RoadEditorGui::onNodeSelected( %this, %nodeIdx ) {
	if ($REP_CustomNodeSelection) {
		warnLog("OnNodeSelected is called from script and no need to update GUI");
		return;
	}

	REP.setCurrentNode(%nodeIdx);

	if ( %nodeIdx == -1 ) {
		RoadEditorProperties-->position.setActive( false );
		RoadEditorProperties-->position.setValue( "" );
		RoadEditorProperties-->width.setActive( false );
		RoadEditorProperties-->width.setValue( "" );
	} else {
		RoadEditorProperties-->position.setActive( true );
		RoadEditorProperties-->position.setValue( %this.getNodePosition() );
		RoadEditorProperties-->width.setActive( true );
		RoadEditorProperties-->width.setValue( %this.getNodeWidth() );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::onNodeModified( %this, %nodeIdx ) {
	RoadEditorProperties-->position.setValue( %this.getNodePosition() );
	RoadEditorProperties-->width.setValue( %this.getNodeWidth() );
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::editNodeDetails( %this ) {
	%this.setNodePosition( RoadEditorProperties-->position.getText() );
	%this.setNodeWidth( RoadEditorProperties-->width.getText() );
}
//------------------------------------------------------------------------------
//==============================================================================
// Road Object Input Events
//==============================================================================
//==============================================================================
function RoadEditorGui::onDeleteKey( %this ) {
	%road = %this.getSelectedRoad();
	%node = %this.getSelectedNode();

	if ( !isObject( %road ) )
		return;

	if ( %node != -1 ) {
		%this.deleteNode();
	} else {
		LabMsgOkCancel( "Notice", "Delete selected DecalRoad?", "RoadEditorGui.deleteRoad();", "" );
	}
}
//------------------------------------------------------------------------------