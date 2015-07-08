//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function syncParamArray(%paramArray,%keepEmptyValue) {

   %i = 0;
	   for( ; %i < %paramArray.count() ; %i++) {
		   %field = %paramArray.getKey(%i);
		   %data = %paramArray.getValue(%i);		  
		   
  // foreach$(%field in %paramObj.fieldList){	 
    //  if (%paramObj.skipField[%field])    
      //   continue;
      
      %value = "";
      %syncObjs = getField(%data,4);
      
      if (%syncObjs $= "") continue;
      //If no object to sync, there might be a prefGroup to use
     
   
      %firstData = getWord(%syncObjs,0);
      %firstObj = %firstData;
      if (!isObject(%firstObj))
         eval("%firstObj = "@ getWord(%syncObjs,0) @";");
      
     // devLog("Sync objs:",%syncObjs);
      
      if (isObject(%firstObj)){
         paramLog(%field,"Param update from object:",%firstObj);
        %value = %firstObj.getFieldValue(%field);      
      }      
      else if (getSubStr(%firstData,0,1) $= "$"){
          //Seem to be a function so replace ** with the value   
         %fix = strreplace(%firstData,"\"","");        
         eval("%value = "@%fix@";"); 
         paramLog("updateSettingsParams Global:",%firstData,"Field",%field,"Value",%value);
        
          
        
      }
      // Check for special object field name EX: EWorldEditor.stickToGround
      else if (strstr(%firstData,".") !$= "-1"){
         //devLog("updateSettingsParams SpecialObjField:",%data,"Field",%field,"Value",%value);
         //Seem to be a function so replace ** with the value             
         eval("%value = "@%firstData@";"); 
          paramLog(%field,"Param update from Object field:",%firstData);               
      }      
      //devLog("Value for field:",%field,"Is:",%value);
      
      if (%value $= "" && !%keepEmptyValue) continue;
      
      %ctrl = %paramArray.container.findObjectByInternalName(%field,true);
      if (!isObject(%ctrl)){
      	warnLog("Couln't find a valid GuiControl holding the value for field:",%field);
      	return;
      }
      %ctrl.setTypeValue(%value);     
   }	
}
//------------------------------------------------------------------------------
//==============================================================================
function autoUpdateParamArray( %field,%value,%paramArray,%callback ) { 
   devLog("autoUpdateParamArray( %field,%value,%paramArray,%callback ) ", %field,%value,%paramArray,%callback ) ;
   if (%callback !$="")
      eval(%callback);
   
   %arIndex = %paramArray.getIndexFromKey(%field);
   %data = %paramArray.getValue(%arIndex);
   
   %pData = %paramArray.pData[%field];
   

    %syncObjs = getField(%data,4);
    devLog("Array Data=",%data,"SyncObjs",%syncObjs);
   
   if (%syncObjs $= "") return;
   		  
  // %syncObjs = %paramObj.syncObjs[%field]; 
   //If no object to sync, there might be a prefGroup to use
   if (%syncObjs $= "" && %paramObj.prefGroup !$= "" ){
      eval(%paramObj.prefGroup@%field@" = %value;");        
   }
   for( ; %i< getFieldCount(%syncObjs);%i++){	      
      %dataObj = getField(%syncObjs ,%i);   
      
      %obj = %dataObj;
      if (!isObject(%obj))
         eval("%obj = "@ getWord(%syncObjs,0) @";");
         
       //devLog("SyncObjs for field:", %field,"Is:", %data);
      if (isObject(%obj)){
         devLog("Updating obj:",%obj,"Field",%field,"Value",%value);
         %obj.setFieldValue(%field,%value);
         devLog("%paramArray.postUpdateFunc",%paramArray.postUpdateFunc);
         if (%paramArray.postUpdateFunc !$= "")
            eval(%paramArray.postUpdateFunc);
      }
       else if (strstr(%dataObj,");") !$= "-1"){
         //Seem to be a function so replace ** with the value         
         %function = strreplace(%dataObj,"**",%value);	
         //devLog("SyncObjs is a function:", %function);
         eval(%function);
      }      
      else if (getSubStr(%dataObj,0,1) $= "$"){
         //devLog("updateSettingsParams Global:",%dataObj,"Field",%field,"Value",%value);
         //Seem to be a function so replace ** with the value   
         %dataObj = strreplace(%dataObj,"\"","");	      
         eval(%dataObj@" = %value;");        
      }
      // Check for special object field name EX: EWorldEditor.stickToGround
      else if (strstr(%dataObj,".") !$= "-1"){
         //devLog("updateSettingsParams SpecialObjField:",%data,"Field",%field,"Value",%value);
         //Seem to be a function so replace ** with the value             
         eval(%dataObj@" = %value;");        
      }
   }
   
   devLog("DoneHere");

     
     
}


//==============================================================================
// Generic updateRenderer method
function updateParamArrayCtrl( %ctrl, %updateFunc,%arg1,%arg2,%arg3) {
   
devLog("ODL updateParamArrayCtrl");
   if (%ctrl.getClassName() $= "GuiColorPickerCtrl"){
 
      %ctrl.updateCommand = %updateFunc@"(%this.internalName,%color,\""@%arg1@"\",\""@%arg2@"\",\""@%arg3@"\");";
      
       %currentColor =   %ctrl.baseColor;
       %callBack = %ctrl@".ColorPicked";
       %updateCallback = %ctrl@".ColorUpdated";      
      
     if (%ctrl.isIntColor)
      	{
      		 %callBack = %ctrl@".ColorPickedI";
      		 %updateCallback = %ctrl@".ColorUpdatedI";       
      		 GetColorI( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
      	}
			else{
				%callBack = %ctrl@".ColorPicked";
      		%updateCallback = %ctrl@".ColorUpdated";       
				 GetColorF( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
			}
       return;
   }
     
      %field = getWord(strreplace(%ctrl.internalName,"_"," "),0);  
    %special = getWord(strreplace(%ctrl.internalName,"_"," "),1);  
    if (%special $= "Edit"){
    	%value = %ctrl.getText();
		devLog("Special Edit value=",%value,"Else",%ctrl.getTypeValue());
    }
    else
     %value = %ctrl.getTypeValue();  
      
  // %field = getWord(strreplace(%ctrl.internalName,"_"," "),0);   
    // %value = %ctrl.getTypeValue();  
   

   if (%updateFunc !$= "")
   eval(%updateFunc@"(%field,%value,%arg1,%arg2,%arg3,%ctrl);");    
   
   if (!%ctrl.noFriends)
   	%ctrl.updateFriends();
   
}
function GuiColorPickerCtrl::pickColorF( %this, %updateFunc,%arg1,%arg2,%arg3) {
   
   %ctrl.updateCommand = %updateFunc@"(%this.internalName,%color,\""@%arg1@"\",\""@%arg2@"\",\""@%arg3@"\");";
      
   %currentColor =   %this.baseColor;
   %callBack = %this@".ColorPicked";
   %updateCallback = %this@".ColorUpdated";       
      
   GetColorF( %currentColor, %callback, %this.getRoot(), %updateCallback, %cancelCallback );     
   
}
function GuiColorPickerCtrl::pickColorI( %this, %updateFunc,%arg1,%arg2,%arg3) {
   
   %ctrl.updateCommand = %updateFunc@"(%this.internalName,%color,\""@%arg1@"\",\""@%arg2@"\",\""@%arg3@"\");";
      
   %currentColor =   ColorFloatToInt(%this.baseColor);
   %callBack = %this@".ColorPicked";
   %updateCallback = %this@".ColorUpdated";       
      
   GetColorI( %currentColor, %callback, %this.getRoot(), %updateCallback, %cancelCallback );     
   
}