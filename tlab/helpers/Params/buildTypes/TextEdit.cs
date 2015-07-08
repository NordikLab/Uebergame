//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamTextEdit( %pData ) {

	%pData.pill-->field.text = %pData.Title;
				

				//TextEdit ctrl update
				%textEdit = %pData.pill-->edit;
				%textEdit.command = %pData.Command;
				%textEdit.altCommand = %pData.AltCommand;
				%textEdit.internalName = %pData.InternalName;
				%textEdit.variable = %pData.Variable;
				
			return %textEdit;
}
//------------------------------------------------------------------------------
