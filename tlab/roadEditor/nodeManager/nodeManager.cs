//==============================================================================
// TorqueLab -> Road Editor Node Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Lab_REP_ShowNodePosition = false;
//==============================================================================
// Manage selected Road Nodes Data
//==============================================================================
//==============================================================================
// Clear the road nodes data information (No road selected)
function RoadEditorGui::updateNodeManagerGui( %this ) {
	if (!isObject(CurrentRoadNodes))
		Lab.getDecalRoadNodes();
	%linkedCount = getWordCount( Lab.linkedRoadNodes );
	
	RoadEd_TabPageNode-->roadNodePageTitle.text = CurrentRoadNodes.count() SPC "Road nodes(linked= "@%linkedCount@")";
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Get current road nodes informations
function Lab::getDecalRoadNodes( %this,%refresh ) {
	RoadEditorGui.clearRoadNodesData();
	%nodeData = newArrayObject("CurrentRoadNodes");
	if (!isObject(RoadEditorPlugin.selectedRoad))
		return;

	if (%refresh) {
		//Keep linked nodes info
		%linkedNodes = Lab.linkedRoadNodes;
	}

	
	%tmpFile = "tlab/roadEditor/tempData/currentRoad.cs";
	%roadObj = RoadEditorPlugin.selectedRoad;
	%roadObj.save(%tmpFile);
	%fileObj = getFileReadObj(%tmpFile);
	
	%node = 0;

	if (!isObject(%fileObj))
		return;

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readline();
		%trimmedLine = trim(%line);

		if (getWord(%trimmedLine,0) !$= "Node")
			continue;

		%data = strReplaceList(%trimmedLine,"Node = \"" TAB "" NL"\"" TAB "" NL";" TAB "");
		%nodeData.setVal(%node,%data);
		Lab.roadNodeList = strAddWord(Lab.roadNodeList,%node,true);
		%node++;
	}

	closeFileObj(%fileObj);
	// Add ourselves to the Editor Settings window

	if (%refresh) {
		//Check linkedRoadNodes validity
		foreach$(%node in %linkedNodes) {
			if (strFind(%linkedNodes,%node)) {
				//This node exist, add it
				Lab.linkedRoadNodes = strAddWord(Lab.linkedRoadNodes,%node,true);
			}
		}

		devLog("Refresh data with linked node:",	Lab.linkedRoadNodes);
	}

	RoadEditorGui.updateRoadNodesData(%refresh);
	RoadEd_TabPageNode.active = 1;
	Lab.linkedRoadNodes = "";
}
//------------------------------------------------------------------------------
//==============================================================================
// Add the current road nodes data to the stack
function RoadEditorGui::updateRoadNodesData( %this,%refresh ) {
	%this.updateNodeManagerGui();
	
	%stack = RoadEditorTools-->nodeStack;
	%pillSrc = RoadEditorTools-->nodeStackSample;
	hide(%pillSrc);
	%stack.clear();
	%count = CurrentRoadNodes.count();

	for(%i = 0; %i< %count; %i++) {
		%key = CurrentRoadNodes.getKey(%i);
		%value = CurrentRoadNodes.getValue(%i);
		%pos = getWords(%value,0,2);
		%width = getWord(%value,3);
		%linkIsOn = false;

		if (%refresh && ($REP_LinkedAll || strFind(Lab.linkedRoadNodes,%key)))
			%linkIsOn = true;

		info("Array dump: Index:",%key,"Key=",%pos ,"Value=",%width);
		%pill = cloneObject(%pillSrc,"","key",%stack);
		%pill.internalName = %key;
		%pill-->id.text = %key@"-";
		%pill-->PosEdit.setValue(%pos);
		%pill-->WidthEdit.setValue(%width);
		%pill-->WidthSlider.setValue(%width);
		%pill.node = %key;
		%pill-->PosEdit.node = %key;
		%pill-->WidthEdit.node = %key;
		%pill-->WidthSlider.node = %key;
		%pill-->Linked.node = %key;
		%pill-->Linked.setStateOn(%linkIsOn);
		%pill-->togglePos.command= "toggleVisible("@%pill-->PosCtrl@");";
		%pill-->SelectBtn.command = "RoadEditorGui.setSelectedNode("@%key@");";
		%pill-->SelectBtn.text = "#"@%key;

		if ($Lab_REP_ShowNodePosition)
			%pill-->PosCtrl.visible = 1;
		else
			%pill-->PosCtrl.visible = 0;

		Lab.nodeLink[%key] = %pill-->Linked;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the road nodes data information (No road selected)
function RoadEditorGui::clearRoadNodesData( %this ) {
	RoadEditorTools-->roadNodePageTitle.text = "Select a road first";
	RoadEditorTools-->globalLink.setStateOn(false);
	Lab.roadNodeList = "";
	Lab.linkedRoadNodes = "";
	%stack = RoadEditorTools-->nodeStack;
	%stack.clear();
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Data Update
//==============================================================================
//==============================================================================
function REP::setCurrentNode( %this,%nodeId ) {
	//Start witn linked node so we end with selectednode selected
	if ($REP_CustomNodeSelection)
		return;

	%stack = RoadEditorTools-->nodeStack;
	%selPill = %stack.findObjectByInternalName(%nodeId);

	if (isObject(%selPill)) {
		foreach (%pill in %stack)
			%pill-->SelectBtn.active = 1;

		%selPill-->SelectBtn.active = 0;
	}
	
	Lab.linkedRoadNodes = strAddWord(Lab.linkedRoadNodes,%ctrl.node,true);
	RoadEditorGui.updateNodeManagerGui();
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Manager Functions
//==============================================================================

//==============================================================================
// Link nodes together for simultaneous update
function RoadEditorGui::setNodeLink( %this, %ctrl,%linkAll ) {
	%linked = %ctrl.isStateOn();
	$REP_LinkedAll = false;

	if (%linkAll) {
		if (%linked) {
			$REP_LinkedAll = true;
			Lab.linkedRoadNodes = Lab.roadNodeList;
			devLog("All Node added:Linked nodes=",Lab.linkedRoadNodes);
		} else {
			Lab.linkedRoadNodes = "";
			devLog("All Node removed:",Lab.linkedRoadNodes);
		}

		foreach$(%node in Lab.roadNodeList) {
			%linkCtrl = Lab.nodeLink[%node];
			%linkCtrl.setStateOn(%linked);
		}
			RoadEditorGui.updateNodeManagerGui();
		return;
	}

	if (%linked) {
		Lab.linkedRoadNodes = strAddWord(Lab.linkedRoadNodes,%ctrl.node,true);
	}
	else{
		Lab.linkedRoadNodes = strRemoveWord(Lab.linkedRoadNodes,%ctrl.node);	
	}
	
	RoadEditorGui.updateNodeManagerGui();
}
//------------------------------------------------------------------------------