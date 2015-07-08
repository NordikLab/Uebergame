//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
// Param Array Object Options
//------------------------------------------------------------------------------
// style          => Style of the Controls to be created (Default-> Default_230)
// container      => GuiControl that hold all the Params Stacks
// aggregateStyle => Aggregate class to use if needed
// prefGroup      => Global pref base to save all values

//==============================================================================

$ParamsArray_WidgetPrefix = "wParams_";
$ParamsArray_DefaultStyle = "Default_230";
$ParamsArray_AggregateClass = "AggregateVar";
$ParamsArray_DefaultStack = "Params_Stack";
$ParamsArray_DefaultStackType = "Rollout";
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamsArray( %array,%syncAfter ) {
	%guiStyle = %array.style;

	if (%guiStyle $= "")
		%guiStyle = $ParamsArray_DefaultStyle;

	%guiSource = $ParamsArray_WidgetPrefix@%guiStyle;

	if ( %array.aggregateStyle !$= "")
		%aggregateClass = "Aggregate"@%array.aggregateStyle;

	//Check if a updateFunction is supplied
	if (%array.useNewSystem) {
		%array.common["command"] = "syncParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
		%array.common["altCommand"] = "syncParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
	} else {
		%array.common["command"] = "updateParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
		%array.common["altCommand"] = "updateParamArrayCtrl($ThisControl,\""@%array.updateFunc@"\",\""@%array.getName()@"\",\"\",\"\");";
	}

	%groupFieldId = 6;

	if (%array.groupFieldId !$= "")
		%groupFieldId =%array.groupFieldId;

	//Clean the Params container for this array
	//%baseCtrl.clear();
	//==============================================================================
	// GENERATE THE PARAMS GROUPS CONTAINERS
	//Add all params group stacks before adding their data
	%gid = 1;

	while(%array.group[%gid] !$="") {
		%groupInfo = %array.group[%gid];
		%groupTitle = getField(%groupInfo,0);
		%groupOptions = getField(%groupInfo,1);
		%groupOptFields = strreplace(%groupOptions,";;","\t");

		for(%gi=0; %gi<getFieldCount(%groupOptFields); %gi++) {
			%gData = getField(%groupOptFields,%gi);
			%gField = firstWord(%gData);
			%gFieldValue = removeWord(%gData,0);
			%groupOption[%gid,%gField] = %gFieldValue;
		}

		%groupCtrlType = $ParamsArray_DefaultStackType;

		if (%groupOption[%gid,"StackType"] !$= "")
			%groupCtrlType = %groupOption[%gid,"StackType"];

		%baseCtrl = %array.container;

		if (%groupOption[%gid,"Container"] !$= "")
			%baseCtrl = %groupOption[%gid,"Container"];

		if (%groupOption[%gid,"Stack"] !$= "") {
			%baseCtrl = %array.container.findObjectByInternalName(%groupOption[%gid,"Stack"],true);
		}

		if (!%baseCtrlClear[%baseCtrl]) {
			%baseCtrl.clear();
			%baseCtrlClear[%baseCtrl] = true;
		}

		if (!isObject(%baseCtrl)) {
			warnLog("Invalid Params Array Container:",%baseCtrl,"Can't generate the params!");
			%gid++;
			continue;
		}

		//------------------------------------------------
		//Group Display Setup
		%displayType = getWord(%groupStackType,0);
		%displayOptions = getWords(%groupStackType,1);
		%displayWidget = %guiSource.findObjectByInternalName(%groupCtrlType,true);

		if (!isObject(%displayWidget)) {
			warnLog("Invalid group control type for group:",%groupTitle,"Using default type. Type tried was:",%groupCtrlType,"Source",%guiSource);
			%groupCtrlType = $ParamsArray_DefaultStackType;
			%displayWidget = %guiSource.findObjectByInternalName(%groupCtrlType,true);

			if (!isObject(%displayWidget)) {
				warnLog("Something is not configurated right, can't generate the default group type:",$ParamsArray_DefaultStackType);
				%gid++;
				continue;
			}
		}

		%displayCtrl = cloneObject(%displayWidget);
		%displayCtrl.caption = %groupTitle;
		%baseCtrl.add(%displayCtrl);
		%groupCtrl[%gid] = %displayCtrl-->stackCtrl;
		%gid++;
	}

	//------------------------------------------------------------------------------
	//==============================================================================
	// GENERATE THE PARAMS FIELDS
	//------------------------------------------------
	//Group Fields Setup
	//Field 0 = Default (Global if start with $)
	//Field 1 = Title
	//Field 2 = Type
	//Field 3 = Options
	//Field 4 = SyncObjs
	//Field 5 = groupId
	for( %i = 0; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		%pData = newScriptObject("paramDataHolder");
		%groupFieldId = getFieldCount(%data) - 1;
		%pData.Setting = %field;
		%pData.Default = getField(%data,0);
		%pData.Title = getField(%data,1);
		%pData.Type = getField(%data,2);
		%pData.Options = getField(%data,3);
		%pData.syncObjs = getField(%data,4);

		if(%groupFieldId > 5)
			%pData.validation = getField(%data,5);

		%pData.srcData = getWord(%pData.syncObjs,0);

		if (%pData.Type $= "None")
			continue;

		%pData.groupId = getField(%data,%groupFieldId);

		if (%pData.groupId $= "")
			%pData.groupId = "1";

		%pData.parentCtrl = %groupCtrl[%pData.groupId];

		if (!isObject(%pData.parentCtrl)) {
			paramLog("Param skipped due to invalid parent ctrl! Skipped setting=",%pData.Setting);
			continue;
		}

		%pData.Command = %array.common["Command"];
		%pData.AltCommand = %array.common["AltCommand"];

		if (%pData.Title $= "") %pData.Title = %pData.Setting;

		%pData.InternalName = %pData.Setting;
		%pData.Variable = "";

		if (getSubStr(%pData.InternalName,0,1) $= "$") {
			%pData.Variable = %pData.InternalName;
			%pData.InternalName = strreplace(%pData.InternalName,"$","");
			%pData.InternalName = strreplace(%pData.InternalName,"::","__");
			devLog("EVALUATING:","%pData.Value = "@%pData.Variable@";");
			eval("%pData.Value = "@%pData.Variable@";");
		}

		%pillSrc = %pData.Type;

		if (%pillSrc $= "ColorInt")
			%pillSrc = "Color";

		%pData.Widget = %guiSource.findObjectByInternalName(%pillSrc,true);

		if(!isObject(%pData.Widget)) {
			paramLog("Couldn't find widget for setting type:",%pData.Type,"This setting building is skipped WidgetSource:",%pData.Widget);
			%fid++;
			continue;
		}

		%pData.pill = cloneObject(%pData.Widget);

		//Overide aggregate if set and custom is specified
		if (%pData.pill.class !$="" && %aggregateClass !$="")
			%pData.pill.class = %aggregateClass;

		//------------------------------------------------
		//Prepare the field special options
		// The options are applied for each Field Category alone
		%pData.OptionList[%pData.Setting] = "";
		%optionsList = strreplace(%pData.Options,";;","\t");
		%optionsCount = getFieldCount(%optionsList);
		%optDiv = ">>";

		if (strFind(%optionsList,"::")) {
			paramLog("Old param options divider (::) detected for field:",%pData.Setting,"In array",%array.getName());
			%optDiv = "::";
		}

		for(%j=0; %j<%optionsCount; %j++) {
			%option = getField(%optionsList ,%j);
			%optWords = strreplace(%option,%optDiv," ");
			%optField = getWord(%optWords,0);
			%optCmd = getWords(%optWords,1);
			%pData.Option[%pData.Setting,%optField] = %optCmd;
			%pData.OptionCmd[%pData.Setting,%optField] = "."@%optField@" = \""@ %optCmd  @"\";";
			%pData.OptionList[%pData.Setting] = trim(%pData.OptionList[%pData.Setting] SPC %optField);
		}

		%tmpFieldValue = %pData.Value;
		%multiplier = 1;
		%tooltip = %pData.Option[%pData.Setting,"tooltip"];
		%tooltipDelay = 1000;
		%pData.mouseAreaClass = %pData.Option[%pData.Setting,"mouseClass"];
		//Get the category of paramCtrl (category stop at _ in type)
		%paramType = strreplace(%pData.Type,"_"," ");
		%pData.Category = getWord(%paramType,0);
		//Check for name for field type holder
		%nameOpt = %pData.Option[%pData.Setting,"name"];

		if (%nameOpt !$= "") {
			switch$(%nameOpt) {
			case "prefix":
				%prefix = %array.namePrefix;

				if (%prefix $= "") {
					warnLog("The param array:",%array.getName(),
							  "contain to namePrefix for auto naming using prefix mode.",
							  " The prefix will be the array internal name:",%array.internalName);
					%prefix = %array.internalName;
				}

				%pData.myNameIs = %prefix @ %pData.Setting @ %pData.Category;
			}
		}

		//=============================================================
		//Call the predefined function for the GuiCtrl type
		if (isFunction("buildParam"@%pData.Category))
			eval("%ctrlHolder = buildParam"@%pData.Category@"(%pData);");
		else
			paramLog("Couldn't create the param, there's no function for that control type:",%pData.Category);

		if (%ctrlHolder) {
			if (%pData.Option[%pData.Setting,"validate"] !$= "") {
				%validate = %pData.Option[%pData.Setting,"validate"];
				%ctrlHolder.validateFunc = %validate;
				%ctrlHolder.friend.validateFunc = %validate;
			}

			if (%pData.validation !$= "") {
				%ctrlHolder.validationType = %pData.validation;
				%ctrlHolder.friend.validationType = %pData.validation;
			}
		}

		%pData.pill-->field.internalName = "fieldTitle";
		%pData.pill-->fieldTitle.canSaveDynamicFields = "1";
		%pData.pill-->fieldTitle.linkedField = %field;
		%pData.pill-->mouseArea.linkedField = %field;
		%array.pill[%field] = %pData.pill;
		%array.title[%field] =  %pData.pill-->fieldTitle.text;
		//=============================================================
		//New pill created, add it to stack
		%pData.parentCtrl.add(%pData.pill);
		%array.pData[%field] = %pData;

		if (%array.paramCallback !$= "")
			eval(%array.paramCallback@"(%array,%field,%pData);");
	}

	if (%syncAfter)
		syncParamArray(%array);
}
//------------------------------------------------------------------------------
