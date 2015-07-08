//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function paramOpt(  ) {
	toggleDlg(LabParamsDlg);
}

function LabParamsDlg::onWake( %this ) {
	hide(%this-->ParamStyles);
}
//==============================================================================
function LabParamsDlg::rebuildAll( %this ) {
	LabParamsTree.clear();
	%this.clearSettingsContainer();
	%this.buildAllParams(true);
	LabParamsTree.buildVisibleTree();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsDlg::setSelectedSettings( %this,%treeItemObj ) {
	if (!isObject(%treeItemObj)) {
		warnLog("Invalid settings item objects selected:",%treeItemObj);
		return;
	}

	foreach(%gui in LP_SettingsContainer) {
		hide(%gui);
	}

	show(%treeItemObj.itemContainer);
}
//==============================================================================
//LabParamsDlg.buildAllParams();
function LabParamsDlg::buildAllParams( %this ) {
	foreach(%paramArray in LabParamsGroup) {
		%name = %paramArray.internalName;
		%group = %paramArray.group;
		%containerName = "lpPage_"@%group@"_"@%name;
		//%paramArray.groupLink = %group@"_"@%name; //TMP
		delObj(%containerName);
		%newContainer = cloneObject(LP_SampleContainer,%containerName,%group@"_"@%name,LP_SettingsContainer);
		%paramArray.optContainer = %newContainer;
		LabParamsTree.addParam(%paramArray);
		%paramArray.container = %newContainer-->Params_Stack;
		buildParamsArray(%paramArray);
		LabParams.syncArray(%paramArray,true);
	}
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function LabParamsDlg::createSettingContainer( %this,%group,%subgroup ) {
	%containerName = "lpPage_"@%group@"_"@%subgroup;

	if (isObject(%containerName)) {
		warnLog("There's already an object using that name:",%containerName.getName(),"ObjId=",%containerName.getId());
		return;
	}

	%newContainer = cloneObject(LP_SampleContainer);
	%newContainer.setName(%containerName);
	%newContainer.internalName = %group@"_"@%subgroup;
	LP_SettingsContainer.add(%newContainer);
	return %newContainer;
}

//==============================================================================
//LabParamsDlg.clearSettingsContainer();
function LabParamsDlg::clearSettingsContainer( %this ) {
	foreach(%gui in LP_SettingsContainer) {
		if (%gui.internalName $= "core")
			continue;

		delObj(%gui);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Tree Builder functions
//==============================================================================
//==============================================================================
function LabParamsTree::rebuildAll( %this ) {
	LabParamsTree.clear();
	LabParamsDlg.clearSettingsContainer();
	LabParamsDlg.buildAllParams(true);
	LabParamsTree.buildVisibleTree();
}
//------------------------------------------------------------------------------

//==============================================================================
// LabParamsTree Callbacks
//==============================================================================

//==============================================================================
function LabParamsTree::onSelect( %this,%itemId ) {
	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsTree::onMouseUp( %this,%itemId,%clicks ) {
	devLog("LabParamsTree::onMouseUp( %this,%itemId,%clicks )",%this,%itemId,%clicks );
	%itemObj = $LabParamsItemObj[%itemId];
	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
	devLog("LabParamsTree::onMouseUp Text",%text,"Value",%value );

	if (isObject(%itemObj)) {
		LabParamsDlg.setSelectedSettings(%itemObj);
	} else {
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParamsTree::addSettingGroup( %this,%group) {
	%tree = LabParamsTree;
	%groupTitle = $LabParamsGroupName[%group];

	if (%groupTitle $="") %groupTitle = %group;

	%parentName = %tree.findItemByName( %group );
	%groupId = %tree.findItemByValue( %group );

	if( %groupId == 0 ) {
		%groupId = %tree.insertItem( 0, %groupTitle,%group );
	}

	return %groupId;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsTree::addParam( %this,%paramArray) {
	%group = %paramArray.group;
	%link = %paramArray.groupLink;
	%name = %paramArray.displayName;
	
	%parentId = LabParamsTree.addSettingGroup(%group);
	%itemId = %this.findChildItemByName( %parentID,%name);

	if( !%itemId ) {
		%itemId = %this.insertItem( %parentID, %name,%link );
	}

	%itemName = "soSettingItem_"@%link;
	%itemObj = newScriptObject(%itemName);
	%itemObj.groupParent = %group;
	%itemObj.groupItem = %name;
	$LabParamsItemObj[%itemId] = %itemObj;
	%itemObj.itemContainer = %paramArray.optContainer;
}
//------------------------------------------------------------------------------