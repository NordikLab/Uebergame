//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::initParamsSystem( %this ) {
	$LabParams = newScriptObject("LabParams");
	$LabParamsGroup = newSimGroup("LabParamsGroup");
}
//------------------------------------------------------------------------------


//==============================================================================
function LabParams::syncArray( %this,%paramArray,%syncTarget ) {
	for( ; %i < %paramArray.count() ; %i++) {
		%field = %paramArray.getKey(%i);
		%data = %paramArray.getValue(%i);
		%this.syncParamField(%paramArray,%field,%data,%syncTarget);
	}
}
//------------------------------------------------------------------------------


//==============================================================================
// Sync the current profile values into the params objects
function LabParams::syncParamField( %this,%paramArray,%field,%data,%syncTarget ) {
	%cfgObj = %paramArray.cfgObject;

	if (isObject(%cfgObj)) {
		%cfgValue = %cfgObj.getCfg(%field);
		paramLog(%cfgObj.getName()@" value from LabCfg is:",%cfgValue);
	} else {
		paramLog("Couldn't find a related LabCfb value! Sync operation aborted!");
		return false;
	}

	if (%cfgValue $= "")
		paramLog(%paramArray.getName(),"Cfg Value for field:",%field,"Is Blank!");

	if (%syncTarget)
		%this.updateParamSyncData(%field,%cfgValue,%paramArray);

	%pill = %paramArray.pill[%field];

	if (isObject(%pill)) {
		%pillHolder = %pill.findObjectByInternalName(%field,true);
		%pillHolder.setTypeValue(%cfgValue,true);
	} else {
		paramLog(%paramArray.getName(),"Invalid control holder for field:",%field,"Couldn't sync the param!");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function LabParams::setParamPillValue( %this,%field,%value,%paramArray ) {
	%pill = %paramArray.pill[%field];

	if (isObject(%pill)) {
		%pillHolder = %pill.findObjectByInternalName(%field,true);
		%pillHolder.setTypeValue(%value,true);
	} else {
		paramLog(%paramArray.getName(),"Invalid control holder for field:",%field,"Couldn't sync the param!");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParams::updateParamSyncData( %this,%field,%value,%paramArray ) {
	paramLog("LabParams::updateParamSyncData( %this,%field,%value,%paramArray )",%this,%field,%value,%paramArray );
	%data = %paramArray.getVal(%field);
	%syncData = getField(%data,4);

	if ( %syncData !$="") {
		if (isObject(%syncData)) {
			eval(%syncData@"."@%field@" = %value;");
		} else if (strFind(%syncData,"$")) {
			if (strFind(%syncData,"::")) {
				paramLog("Sync data seem to be a full pref:",%syncData);
			} else {
				paramLog("Sync data seem to be a global:",%syncData);
			}

			eval(%syncData@" = %value;");
		} else if (strFind(%syncData,"::")) {
			paramLog("Sync data seem to be a part of pref group:",%syncData,"Pref is:",%paramArray.prefGroup@%syncData);
			eval(%paramArray.prefGroup@%syncData@" = %value;");
		} else if (strFind(%syncData,"**")) {
			%func = strreplace(%syncData,"**","\""@%value@"\"");
			paramLog("Syncing a callback function:",%syncData,"Converted to:",%func);
			eval(%func);
		} else {
			paramLog("Sync data is unknown:",%syncData);
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Update the Params after control value changed in ParamsDlg
//==============================================================================

//==============================================================================
function LabParams::updateParamArrayCtrl( %this,%field,%value,%ctrl,%paramArray,%arg1,%arg2 ) {
	paramLog("LabParams::updateParamArrayCtrl( %this,%field,%value,%ctrl,%paramArray,%arg1,%arg2 )",%this,%field,%value,%ctrl,%paramArray,%arg1,%arg2 );
	%cfgObj = %paramArray.cfgObject;
	%data = %paramArray.getVal(%field);	
	
	
	if (isObject(%cfgObj)) {
		%cfgObj.setCfg(%field,%value);
		paramLog(%cfgObj.getName()@" value stored as:",%value);
	} else {
		paramLog("Couldn't find a related LabCfg Object to update! No change to the LabCfg object!");
		return false;
	}

	%ctrl.updateFriends();
	%this.updateParamSyncData(%field,%value,%paramArray);
	%this.setParamPillValue(%field,%value,%paramArray);
}
//------------------------------------------------------------------------------
function LabParams::updateParamFromCtrl( %this,%ctrl,%field,%value,%paramArray ) {
	paramLog("LabParams::updateParamFromCtrl( %this,%ctrl,%field,%value,%paramArray )",%this,%ctrl,%field,%value,%paramArray );
	%cfgObj = %paramArray.cfgObject;

	if (isObject(%cfgObj)) {
		%cfgObj.setCfg(%field,%value);
		paramLog(%cfgObj.getName()@" value stored as:",%value);
	} else {
		paramLog("Couldn't find a related LabCfg Object to update! No change to the LabCfg object!");
		return false;
	}

	%ctrl.updateFriends();
	%this.updateParamSyncData(%field,%value,%paramArray);
	%this.setParamPillValue(%field,%value,%paramArray);
}
//------------------------------------------------------------------------------
