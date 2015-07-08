//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$Params::EnableLogs = 0;


//==============================================================================
//Initialize plugin data
function createParamsArray(%name,%container,%prefGroup) {
  
   if (%name $= ""){
      warnLog("You need to specify a name for the settings which is unique in this type");
	  return;
   }
   if (%container $= "")
   	%container = %name@"ParamStack";
   
   %fullName = "ar"@%name@"Param";
   %array = newArrayObject(%fullName,LabConfigArrayGroup);
   %array.internalName = %name;
   %array.container = %container;
   if (%prefGroup $= "")
      %prefGroup = %name;
   %array.prefGroup = %prefGroup;
		
   %array.common["command"] = "updateParamArrayCtrl($ThisControl,\"autoUpdateParamArray\",\""@%fullName@"\",\"\",\"\");";
   %array.common["altCommand"] = "updateParamArrayCtrl($ThisControl,\"autoUpdateParamArray\",\""@%fullName@"\",\"\",\"\");";	
   
   return %array;		
}
//------------------------------------------------------------------------------

function initParamsArray(%obj,%initFunction) {  
   %type = %obj.itemType;
   %name = %obj.internalName;
   //Create a container
   %containerSrc = DlgGameLab.findObjectByInternalName(%type@"SampleContainer",true);
   %containerParent = DlgGameLab.findObjectByInternalName(%type@"ParamsContainer",true);
   %objContainer = cloneObject(%containerSrc,"Params_"@%type@"_"@%name,%name,%containerParent);
  %objContainer.keepMe = false;
  %objContainer-->groupTitle.text = %name@" settings and options";
  %objContainer.setVisible(false);
   if (isFunction("initParams"@%type@%name)) {     
	  %array = newArrayObject("ar"@%name@"Cfg");			
		
		%array.internalName = %name;
		%array.paramName = %type@"_"@%name;
		%array.container = %objContainer; 
		if (%type $= "Module")
		   %array.prefGroup = "$Module_"@%name@"_";
		else 	if (%type $= "Mode")
		    %array.prefGroup = "$"@%name@"Lab::";
      
      if (!isObject(%array))
         return;
      
      //Called the function holding the params data
		eval("initParams"@%type@%name@"(%array);");		
		
		%paramObj = convertArrayToParam(%array);
		%obj.paramObj = %paramObj;
		buildParamsObject(%paramObj);
		
		syncParamObj(%paramObj)	;
		delObj(%array);	
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function paramLog( %log1,%log2,%log3,%log4,%log5,%log6,%log7,%log8,%log9 ) {
	if (!$Params::EnableLogs)
		return;
	
	info("ParamLog->",%log1,%log2,%log3,%log4,%log5,%log6,%log7,%log8,%log9 );	
}
//------------------------------------------------------------------------------