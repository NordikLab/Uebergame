//==============================================================================
// TorqueLab -> Road Editor Node Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Manage selected Road Nodes Data
//==============================================================================


//==============================================================================
// Node Manager Functions
//==============================================================================

//==============================================================================
// Link nodes together for simultaneous update
function RiverEditorGui::setNodeLink( %this, %ctrl,%linkAll ) {
	%linked = %ctrl.isStateOn();
	$RiverEd_LinkedAll = false;

	if (%linkAll) {
		if (%linked) {
			$RiverEd_LinkedAll = true;
			Lab.linkedRiverNodes = Lab.riverNodeList;
			devLog("All Node added:Linked nodes=",Lab.linkedRiverNodes);
		} else {
			Lab.linkedRiverNodes = "";
			devLog("All Node removed:",Lab.linkedRiverNodes);
		}

		foreach$(%node in Lab.riverNodeList) {
			%linkCtrl = Lab.nodeLink[%node];
			%linkCtrl.setStateOn(%linked);
		}

		return;
	}

	if (%linked) {
		Lab.linkedRiverNodes = strAddWord(Lab.linkedRiverNodes,%ctrl.node,true);
		devLog("Node added:",%ctrl.node,"Linked nodes=",Lab.linkedRiverNodes);
		return;
	}

	Lab.linkedRiverNodes = strRemoveWord(Lab.linkedRiverNodes,%ctrl.node);
	devLog("Node removed:",%ctrl.node,"Linked nodes=",Lab.linkedRiverNodes);
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Width Adjustement functions
//==============================================================================

//==============================================================================
function RiverEd::updateNodeGuiData( %this, %data,%nodeId,%nodeValue ) {
	if (%nodeValue <= 0) {
		warnLog("Can't set node width smaller or equal to 0! Attempted value=",%nodeValue);
		return;
	}

	%stack = RiverEditorTools-->nodeStack;
	%nodePill = %stack.findObjectByInternalName(%nodeId);

	if (!isObject(%nodePill) /*|| %skipPillUpdate*/)
		return;

	eval("%nodePill-->"@%data@"Edit.setValue(%nodeValue);");
	eval("%nodePill-->"@%data@"Slider.setValue(%nodeValue);");
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEd::setNodeData( %this, %data,%node,%value,%isRelative ) {
	if (%value <= 0) {
		warnLog("Can't set node width smaller or equal to 0! Attempted value=",%width);
		return;
	}

	RiverEditorGui.setSelectedNode(%node);

	if (%data $= "PosZ") {
		%nodePos = 	RiverEditorGui.getNodePosition();
		%posZ = %value;

		if (%isRelative)
			%valueZ = %nodePos.z + %posZ;
		else
			%valueZ = %posZ;

		%nodePos.z = %valueZ;
		%data = "Position";
		%value = %nodePos;
		devLog("SetNode Position to:",%value,"Was:",	RiverEditorGui.getNodePosition(),"Z Offset:",%posZ);
	} else if (%isRelative)
		eval("%value = RiverEditorGui.getNode"@%data@"() + %value;");

	eval("RiverEditorGui.setNode"@%data@"(%value);");
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEditorGui::setLinkedGlobalWidth( %this ) {
	%value = RiverEd_GlobalNodeBox-->SetWidthEdit.getText();
	%this.setLinkedNodeData("Width",%value,false,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEditorGui::setLinkedRelativeWidth( %this ) {
	%value = RiverEd_GlobalNodeBox-->RelativeWidthEdit.getText();
	%this.setLinkedNodeData("Width",%value,true,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEditorGui::setLinkedNodeData( %this, %data,%value,%isRelative,%updateCurrent ) {
	devLog("RiverEditorGui setLinkedNodeData",%ctrl);
	%curNode = %this.selectedNode;

	foreach$(%nodeId in Lab.linkedRiverNodes) {
		if (%nodeId $= %curNode && !%updateCurrent)
			continue;

		//eval("RiverEditorGui.setNode"@%data@"(%value);");
		%validValue = RiverEd.setNodeData(%data,%nodeId,%value,%isRelative);
		RiverEd.updateNodeGuiData(%data,%nodeId,%validValue);
		devLog("set Link NodeData ",%data,"Node",%nodeId,"Value",%validValue);
	}

	if (%this.selectedNode !$= %curNode)
		RiverEditorGui.setSelectedNode(%curNode);

	$RiverEd_CustomNodeSelection = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RiverEditorGui::onNodeWidthChange( %this, %ctrl,%linkOnly ) {
	devLog("RiverEditorGui onNodeWidthChange",%ctrl);
	$RiverEd_CustomNodeSelection = true;
	//Start witn linked node so we end with selectednode selected
	%width = %ctrl.getValue();
	%curNode = %ctrl.node;

	if (%curNode $= "")
		%curNode = %this.selectedNode;

	if (%curNode $= "-1" || %curNode $= ""  ) {
		warnLog("No node selected. Select a node before changing width.");
		return;
	}

	%this.setLinkedNodeData("width",%width);
	RiverEd.setNodeData("Width",%curNode,%width);
	//RiverEd.updateNodeGuiData("width",%curNode,%width);
	$RiverEd_CustomNodeSelection = false;
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------


//==============================================================================
// Set Relative Width
function RiverEditorGui::setRelativeWidth( %this, %ctrl ) {
	devLog("RiverEditorGui setRelativeWidth",%ctrl);
	$RiverEd_CustomNodeSelection = true; //Used to tell script the incoming selection are from script
	%width = %ctrl.getValue();
	%selNode = %this.selectedNode();
	%this.setLinkedNodeData("Width",%width,true);
	RiverEd.setNodeData("Width",%selNode,%curWidth,true);
	$RiverEd_RelativeWidthAdjust = 0;
	$RiverEd_CustomNodeSelection = false; //Used to tell script the incoming selection are from script
}
//------------------------------------------------------------------------------
//==============================================================================
// Node Depth Adjustement functions
//==============================================================================
//==============================================================================
function RiverEditorGui::onNodeDepthChange( %this, %ctrl,%linkOnly ) {
	devLog("RiverEditorGui onNodeDepthChange",%ctrl);
	$RiverEd_CustomNodeSelection = true;
	//Start witn linked node so we end with selectednode selected
	%depth = %ctrl.getValue();
	%curNode = %ctrl.node;

	if (%curNode $= "")
		%curNode = %this.selectedNode;

	if (%curNode $= "-1" || %curNode $= ""  ) {
		warnLog("No node selected. Select a node before changing width.");
		return;
	}

	%this.setLinkedNodeData("Depth",%depth);
	RiverEd.setNodeData("Depth",%curNode,%depth,true);
	//RiverEd.updateNodeGuiData("width",%curNode,%width);
	$RiverEd_CustomNodeSelection = false;
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Node Position Adjustement functions
//==============================================================================
//==============================================================================
function RiverEditorGui::onNodePosZChange( %this, %ctrl,%linkOnly ) {
	devLog("RiverEditorGui onNodePosZChange",%ctrl);
	$RiverEd_CustomNodeSelection = true;
	//Start witn linked node so we end with selectednode selected
	%posZ = %ctrl.getValue();
	%curNode = %ctrl.node;

	if (%curNode $= "")
		%curNode = %this.selectedNode;

	if (%curNode $= "-1" || %curNode $= ""  ) {
		warnLog("No node selected. Select a node before changing width.");
		return;
	}

	%this.setLinkedNodeData("PosZ",%posZ);
	RiverEd.setNodeData("PosZ",%curNode,%posZ,false);
	//RiverEd.updateNodeGuiData("width",%curNode,%width);
	$RiverEd_CustomNodeSelection = false;
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
function RiverEditorGui::setNodeDataPos( %this, %ctrl ) {
	//Start witn linked node so we end with selectednode selected
	foreach$(%nodeId in Lab.linkedRiverNodes) {
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
function RiverEdGlobalWidthEdit::onReturn( %this ) {
	devLog("RiverEdGlobalWidthEdit::onReturn",%this);
}
function RiverEdGlobalWidthEdit::onValidate( %this ) {
	devLog("RiverEdGlobalWidthEdit::onValidate",%this);
}
function RiverEdGlobalWidthEdit::onTabComplete( %this,%val ) {
	devLog("RiverEdGlobalWidthEdit::onTabComplete",%this,%val);
}
//------------------------------------------------------------------------------

/*
new DecalRiver(RiverA) {
   Material = "Mat_Farm_DirtPathA";
   textureLength = "5";
   breakAngle = "3";
   renderPriority = "10";
   position = "281.549 534.766 77.0376";
   rotation = "1 0 0 0";
   scale = "1 1 1";
   canSave = "1";
   canSaveDynamicFields = "1";
      byGroup = "0";

   Node = "281.549438 534.765686 77.037598 10.000000";
   Node = "263.842529 479.133606 84.437988 10.000000";
   Node = "245.589066 429.808502 95.093262 10.000000";
   Node = "234.988129 392.732849 82.561523 10.000000";
   Node = "263.659149 312.105927 76.943359 10.000000";
   Node = "243.086487 257.008942 80.910156 10.000000";
   Node = "175.321808 201.690506 95.187500 10.000000";
   Node = "55.601074 270.340820 58.212402 10.000000";
};
*/