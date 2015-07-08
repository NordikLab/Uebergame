//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSliderEdit( %pData ) {
      
   	%mouseArea = %pData.pill-->mouse;
				if (isObject (%mouseArea)) {
					%mouseArea.infoText = %tooltip;					
					if (%pData.mouseAreaClass !$="")
						%mouseArea.superClass = %mouseSuperClass;
				}

				%pData.pill-->field.text = %pData.Title;
				

				//TextEdit ctrl update
				%textEdit = %pData.pill-->textEdit;
				%textEdit.command = %pData.Command;
				%textEdit.altCommand = %pData.AltCommand;
				%textEdit.internalName = %pData.InternalName;
				if (%pData.Value !$= "")
					%textEdit.text = %pData.Value;
            
           
            
				%precision =  %pData.Option[%pData.Setting,"precision"];
				if (%precision !$="" && %pData.Value !$= "") {
					%fixValue = setFloatPrecision(%pData.Value,%precision);
					%textEdit.text = %fixValue;
					%textEdit.variable = "";
				} else
					%textEdit.variable = %pData.Variable;


				//Slider ctrl update
				%slider = %pData.pill-->slider;
				%slider = paramSliderOptions(%pData,%slider);
				
				foreach$(%option in %pData.OptionList[%pData.Setting]){
					eval(%slider@%pData.OptionCmd[%pData.Setting,%option]);
				}


				%slider.command = %pData.Command;
				%slider.altCommand = %pData.AltCommand;
				%slider.internalName = %pData.InternalName@"_slider";
				%slider.variable = %pData.Variable;
				if (%pData.Value !$= "")
					%slider.setValue(%pData.Value);

				%slider.tooltip = %tooltip;
				%slider.hovertime = %tooltipDelay;
				%pData.pill-->field.tooltip = %tooltip;
				%pData.pill-->field.hovertime = %tooltipDelay;
				%textEdit.tooltip = %tooltip;
				%textEdit.hovertime = %tooltipDelay;
				
				 %textEdit.friend = %slider;
				 %slider.friend = %textEdit;
				 
	return %textEdit;
}
//------------------------------------------------------------------------------
