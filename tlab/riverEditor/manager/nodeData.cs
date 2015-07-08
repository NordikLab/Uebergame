//==============================================================================
// TorqueLab -> River Editor Node Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Lab_RiverEd_ShowNodePosition = false;
//==============================================================================
// Manage selected River Nodes Data
//==============================================================================

//==============================================================================
// Get current river nodes informations
function Lab::getDecalRiverNodes( %this,%refresh ) {
	%riverObj = RiverEditorGui.river;

	if (!isObject(%riverObj))
		return;

	if (%refresh) {
		//Keep linked nodes info
		%linkedNodes = Lab.linkedRiverNodes;
	}

	RiverEditorGui.clearRiverNodesData();
	%tmpFile = "tlab/RiverEditor/tempData/currentRiver.cs";
	%riverObj.save(%tmpFile);
	%fileObj = getFileReadObj(%tmpFile);
	%nodeData = newArrayObject("CurrentRiverNodes");
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
		Lab.riverNodeList = strAddWord(Lab.riverNodeList,%node,true);
		%node++;
	}

	closeFileObj(%fileObj);
	// Add ourselves to the Editor Settings window

	if (%refresh) {
		//Check linkedRiverNodes validity
		foreach$(%node in %linkedNodes) {
			if (strFind(%linkedNodes,%node)) {
				//This node exist, add it
				Lab.linkedRiverNodes = strAddWord(Lab.linkedRiverNodes,%node,true);
			}
		}

		devLog("Refresh data with linked node:",	Lab.linkedRiverNodes);
	}

	RiverEditorGui.updateRiverNodesData(%refresh);
	RiverEd_TabPageNode.active = 1;
	Lab.linkedRiverNodes = "";
}
//------------------------------------------------------------------------------
//==============================================================================
// Add the current river nodes data to the stack
function RiverEditorGui::updateRiverNodesData( %this,%refresh ) {
	RiverEditorTools-->riverNodePageTitle.text = "River nodes("@CurrentRiverNodes.count()@")";
	%stack = RiverEditorTools-->nodeStack;
	%pillSrc = RiverEditorTools-->nodeStackSample;
	hide(%pillSrc);
	%stack.clear();
	%count = CurrentRiverNodes.count();

	for(%i = 0; %i< %count; %i++) {
		%key = CurrentRiverNodes.getKey(%i);
		%value = CurrentRiverNodes.getValue(%i);
		%pos = getWords(%value,0,2);
		%width = getWord(%value,3);
		%depth = getWord(%value,4);
		%linkIsOn = false;

		if (%refresh && ($RiverEd_LinkedAll || strFind(Lab.linkedRiverNodes,%key)))
			%linkIsOn = true;

		info("Array dump: Index:",%key,"Key=",%pos ,"Width=",%width,"Depth=",%depth);
		%pill = cloneObject(%pillSrc,"","key",%stack);
		%pill.internalName = %key;
		%pill-->id.text = %key@"-";
		%pill.node = %key;
		//Pos data
		%pill-->PosEdit.node = %key;
		%pill-->PosEdit.setValue(%pos);
		%pill-->togglePos.command= "toggleVisible("@%pill-->PosCtrl@");";
		//Width data
		%pill-->WidthEdit.node = %key;
		%pill-->WidthSlider.node = %key;
		%pill-->WidthEdit.setValue(%width);
		%pill-->WidthSlider.setValue(%width);
		//Depth data
		%pill-->DepthEdit.node = %key;
		%pill-->DepthSlider.node = %key;
		%pill-->DepthEdit.setValue(%depth);
		%pill-->DepthSlider.setValue(%depth);
		//Links data
		%pill-->Linked.node = %key;
		%pill-->Linked.setStateOn(%linkIsOn);
		//Select button
		%pill-->SelectBtn.command = "RiverEditorGui.setSelectedNode("@%key@");";
		%pill-->SelectBtn.text = "#"@%key;

		if ($Lab_RiverEd_ShowNodePosition)
			%pill-->PosCtrl.visible = 1;
		else
			%pill-->PosCtrl.visible = 0;

		Lab.nodeLink[%key] = %pill-->Linked;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the river nodes data information (No river selected)
function RiverEditorGui::clearRiverNodesData( %this ) {
	RiverEditorTools-->riverNodePageTitle.text = "Select a river first";
	RiverEditorTools-->globalLink.setStateOn(false);
	Lab.riverNodeList = "";
	Lab.linkedRiverNodes = "";
	%stack = RiverEditorTools-->nodeStack;
	%stack.clear();
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Data Update
//==============================================================================
//==============================================================================
function RiverEd::setCurrentNode( %this,%nodeId ) {
	//Start witn linked node so we end with selectednode selected
	if ($RiverEd_CustomNodeSelection)
		return;

	%stack = RiverEditorTools-->nodeStack;
	%selPill = %stack.findObjectByInternalName(%nodeId);

	if (isObject(%selPill)) {
		foreach (%pill in %stack)
			%pill-->SelectBtn.active = 1;

		%selPill-->SelectBtn.active = 0;
	}

	%nodeWidth = RiverEditorGui.getNodeWidth(%nodeId);
	%nodePos = RiverEditorGui.getNodePosition(%nodeId);
	RiverEd_GlobalNodeBox-->CurrentWidthEdit.setValue(%nodeWidth);
	RiverEd_GlobalNodeBox-->CurrentWidthEdit.node = %nodeId;
}
//------------------------------------------------------------------------------

