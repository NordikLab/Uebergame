//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamCheckbox( %pData ) {
      
   	%pData.pill-->field.text = %pData.Title;
     

				//Checkbox ctrl update
				%checkbox = %pData.pill-->checkbox;
				%checkbox.text = "";
				%checkbox.command = %pData.Command;
				%checkbox.altCommand = %pData.AltCommand;
				%checkbox.internalName = %pData.InternalName;
				%checkbox.variable = %pData.Variable;
}
//------------------------------------------------------------------------------
