//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColor( %pData ) {
	%pData.pill-->field.text = %pData.Title;
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%checkbox.variable = %pData.Variable;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;

	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%colorPicker.isIntColor = true;
		
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorInt( %pData ) {
	%pData.pill-->field.text = %pData.Title;
	%pData.pill-->field.internalName = "";
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%checkbox.variable = %pData.Variable;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;
	%colorPicker.isIntColor = true;
	
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorAlpha( %pData ) {
	%pData.pill-->field.text = %pData.Title;
	%pData.pill-->field.internalName = "NoUpdate";
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%colorPicker.variable = %pData.Variable;
	%colorPicker.alpha = "alphaSlider";
	%alphaSlider = %pData.pill-->slider;
	%alphaSlider.command = "updateParamColorAlpha($Me);";
	%alphaSlider.altCommand = "updateParamColorAlpha($Me);";
	%alphaSlider.internalName = "alphaSlider";
	%alphaSlider.dataType = %setting;
	%alphaSlider.range = "0 1";
	%alphaSlider.noFriends = true;
	%alphaSlider.variable = %pData.Variable;
	
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorSlider( %pData ) {
	%isIntColor = false;
	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%isIntColor = true;
	%pData.pill-->field.text = %pData.Title;
	%alphaSlider = %pData.pill-->slider;
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%colorPicker.alphaSlider = %alphaSlider;
	%checkbox.variable = %pData.Variable;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;

	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%colorPicker.isIntColor = true;

	%alphaSlider.colorPicker = %colorPicker;
	%alphaSlider.fieldSource = %pData.Setting;
	%alphaSlider.command = %colorPicker@".AlphaChanged($ThisControl);";
	
	if (%isIntColor)
		%alphaSlider.range = "0 255";
	else
		%alphaSlider.range = "0 1";
		
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorPickedI(%this,%color) {
	devLog("GuiColorPickerCtrl::ColorPickedI(%this,%color)",%this,%color);
	%srcObj = %this.sourceObject;
	%srcField = %this.sourceField;
	%alpha = mCeil(getWord(%color,3));
	%color = setWord(%color,3,%alpha);	

	%baseColor = ColorIntToFloat(%color);
	devLog("GuiColorPickerCtrl::ColorPicked(Converted)",%baseColor);
	%this.baseColor = %baseColor;
	if (isObject(%srcObj))
		%srcObj.setFieldValue(%srcField,%color);

	if (isObject(%this.alphaSlider)) {
		devLog("Color Picked, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorUpdatedI(%this,%color) {
	devLog("GuiColorPickerCtrl::ColorUpdatedI(%this,%color)",%this,%color);
	%alpha = mCeil(getWord(%color,3));
	
	
	
	%baseColor = ColorIntToFloat(%color);
	

	devLog("GuiColorPickerCtrl::ColorUpdated(Converted)",%baseColor);
	// %this.sourceArray.setValue(%this.internalName,%color);
		%this.baseColor = %baseColor;	
	%this.updateColor();

	if (isObject(%this.alphaSlider)) {
		devLog("Color Updated, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
		devLog("UpdateFunct:",%this.updateCommand);
	%ctrl = %this;
	eval(%ctrl.updateCommand);
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorPicked(%this,%color) {
	devLog("GuiColorPickerCtrl::ColorPicked(%this,%color)",%this,%color);
	%srcObj = %this.sourceObject;
	%srcField = %this.sourceField;
	%alpha = mCeil(getWord(%color,3));
	%color = setWord(%color,3,%alpha);
	%baseColor = %color;
	if (%this.isIntColor)
	//	%baseColor = ColorIntToFloat(%color);
	
	devLog("GuiColorPickerCtrl::ColorPicked(Converted)",%baseColor);
	%this.baseColor = %baseColor;
	if (isObject(%srcObj))
		%srcObj.setFieldValue(%srcField,%color);

	if (isObject(%this.alphaSlider)) {
		devLog("Color Picked, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorUpdated(%this,%color) {
	devLog("GuiColorPickerCtrl::ColorUpdated(%this,%color)",%this,%color);
	%alpha = mCeil(getWord(%color,3));
	
	%baseColor = %color;
	if (%this.isIntColor)
		%baseColor = ColorIntToFloat(%color);
	
	%this.baseColor = %baseColor;	
	
	devLog("GuiColorPickerCtrl::ColorUpdated(Converted)",%baseColor);
	// %this.sourceArray.setValue(%this.internalName,%color);
	%this.updateColor();

	if (isObject(%this.alphaSlider)) {
		devLog("Color Updated, updating Slider to:",%alpha);
		%this.alphaSlider.setValue(%alpha);
	}
		devLog("UpdateFunct:",%this.updateCommand);
	%ctrl = %this;
	eval(%ctrl.updateCommand);
}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::AlphaChanged(%this,%sourceCtrl) {
	devLog("GuiColorPickerCtrl::AlphaChanged(%this,%sourceCtrl)",%this,%sourceCtrl,"Value:",%sourceCtrl.getValue());
	%alpha = %sourceCtrl.getValue();
	
	devLog("Start color = ",%this.baseColor);
	%this.baseColor.a = %alpha;
	%color = %this.baseColor;
	%this.updateColor();
	devLog("UpdateFunct:",%this.updateCommand);
	devLog("End color = ",%this.baseColor);	
	%ctrl = %this;
	eval(%ctrl.updateCommand);
}
//------------------------------------------------------------------------------

