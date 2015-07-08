//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamText( %pData ) {

	%pData.pill-->field.text = %pData.Title;
				
         
				//TextEdit ctrl update
				%textValue = %pData.pill-->value;	
				//eval("%value = " @ %pData.Default @";");
				
				//devLog("Default:",%pData.Default,"Value:",	%value);		
				%textValue.text = %pData.Default;
				//if (%pData.srcVar !$= "")
				  // %textValue.variable = %pData.srcData;
				
				%textValue.internalName = %pData.InternalName;
}
//------------------------------------------------------------------------------
