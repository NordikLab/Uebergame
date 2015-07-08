//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamFileSelect( %pData ) {

	%pData.pill-->field.text = %pData.Title;

 	//TextEdit ctrl update
   if (%pData.Option[%pData.Setting,"callBack"] !$= "")
 	   %pData.pill-->SelectButton.command = %pData.Option[%pData.Setting,"callBack"];
   %textEdit = %pData.pill-->textEdit;
   %textEdit.command = %pData.Command;
   %textEdit.altCommand = %pData.AltCommand;
   %textEdit.internalName = %pData.InternalName;
   %textEdit.variable = %pData.Variable;
}
//------------------------------------------------------------------------------
