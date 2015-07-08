//==============================================================================
// TorqueLab -> Edit DecalRoad Nodes
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Node Width Adjustement functions
//==============================================================================
//==============================================================================
function REP::setNodeWidth( %this, %node,%width,%skipPillUpdate ) {
	if (%width <= 0) {
		warnLog("Can't set node width smaller or equal to 0! Attempted value=",%width);
		return;
	}

	RoadEditorGui.setSelectedNode(%node);
	RoadEditorGui.setNodeWidth(%width);
	%stack = RoadEditorTools-->nodeStack;
	%nodePill = %stack.findObjectByInternalName(%node);

	if (!isObject(%nodePill) /*|| %skipPillUpdate*/)
		return;

	%nodePill-->WidthEdit.setValue(%width);
	%nodePill-->WidthSlider.setValue(%width);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedGlobalWidth( %this ) {
	%value = REP_GlobalNodeBox-->SetWidthEdit.getText();
	%this.setLinkedNodeWidth(%value,false,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedRelativeWidth( %this ) {
	%value = REP_GlobalNodeBox-->RelativeWidthEdit.getText();
	%this.setLinkedNodeWidth(%value,true,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedNodeWidth( %this, %value,%isRelative,%updateCurrent ) {
	devLog("RoadEditorGui setNodeDataWidth",%ctrl);
	%curNode = %this.getSelectedNode();

	foreach$(%nodeId in Lab.linkedRoadNodes) {
		if (%nodeId $= %curNode && !%updateCurrent)
			continue;

		%width = %value;

		if (%isRelative) {
			RoadEditorGui.setSelectedNode(%nodeId);
			%width = %this.getNodeWidth(%nodeId) + %value;
		}

		REP.setNodeWidth(%nodeId,%width);
		devLog("set Link NodeData Width Node",%nodeId,"Value",%width);
	}

	if (%this.getSelectedNode() !$= %curNode)
		RoadEditorGui.setSelectedNode(%curNode);

	$REP_CustomNodeSelection = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setNodeDataWidth( %this, %ctrl,%linkOnly ) {
	devLog("RoadEditorGui setNodeDataWidth",%ctrl);
	$REP_CustomNodeSelection = true;
	//Start witn linked node so we end with selectednode selected
	%width = %ctrl.getValue();
	%curNode = %ctrl.node;

	if (%curNode $= "") {
		warnLog("No node submitted for set Node data width. Using selected node!");
		%curNode = %this.getSelectedNode();

		if (%curNode $= "") {
			warnLog("No node selected. Select a node before changing width.");
			return;
		}
	}

	%this.setLinkedNodeWidth(%width);
	REP.setNodeWidth(%curNode,%width,true);
	$REP_CustomNodeSelection = false;
}
//------------------------------------------------------------------------------


//==============================================================================
// Set Relative Width
function RoadEditorGui::setRelativeWidth( %this, %ctrl ) {
	devLog("RoadEditorGui setRelativeWidth",%ctrl);
	$REP_CustomNodeSelection = true; //Used to tell script the incoming selection are from script
	%width = %ctrl.getValue();
	%selNode = %this.getSelectedNode();
	%this.setLinkedNodeWidth(%width,true);
	%curWidth = %this.getNodeWidth(%selNode) + %width;

	if (%curWidth <= 0) {
		warnLog("Node:",%selNode,"Can't set node width under 0:",%curWidth,"Set node width skipped");
		return;
	} else {
		REP.setNodeWidth(%selNode,%curWidth);
		//%this.setNodeWidth(%curWidth);
	}

	$REP_RelativeWidthAdjust = 0;
	$REP_CustomNodeSelection = false; //Used to tell script the incoming selection are from script
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Position Adjustement functions
//==============================================================================
function RoadEditorGui::setNodeDataPos( %this, %ctrl ) {
	//Start witn linked node so we end with selectednode selected
	foreach$(%nodeId in Lab.linkedRoadNodes) {
		if (%nodeId $= %ctrl.node)
			continue;

		%this.setSelectedNode(%nodeId);
		%this.setNodePosition(%ctrl.getValue());
	}

	%this.setSelectedNode(%ctrl.node);
	%this.setNodePosition(%ctrl.getValue());
	devLog("setNodeDataPos Node",%ctrl.node,"Value",%ctrl.getValue());
}
//------------------------------------------------------------------------------




//==============================================================================
function REPGlobalWidthEdit::onReturn( %this ) {
	devLog("REPGlobalWidthEdit::onReturn",%this);
}
function REPGlobalWidthEdit::onValidate( %this ) {
	devLog("REPGlobalWidthEdit::onValidate",%this);
}
function REPGlobalWidthEdit::onTabComplete( %this,%val ) {
	devLog("REPGlobalWidthEdit::onTabComplete",%this,%val);
}
//------------------------------------------------------------------------------