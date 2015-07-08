//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ->Added Gui style support
// -->Delete Profiles when reloaded
// -->Image array store in Global
//==============================================================================

//==============================================================================
// GuiTextEditCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsTextEditProfile
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextEditProfile : ToolsDefaultProfile ) {
    opaque = true;
    bitmap = "tlab/gui/assets/element/GuiTextEditBorder.png";

    hasBitmapArray = true;
    border = -2; // fix to display textEdit img
    //borderWidth = "1";  // fix to display textEdit img
    //borderColor = "100 100 100";
    fillColor = "242 241 240 0";
    fillColorHL = "255 255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "64 50 0 255";
    fontColorSEL = "98 100 137";
    fontColorNA = "6 19 76 255";
    textOffset = "4 2";
    autoSizeWidth = false;
    autoSizeHeight = true;
    justify = "left";
    tab = true;
    canKeyFocus = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontSize = "15";
    fontColors[1] = "64 50 0 255";
    fontColors[2] = "6 19 76 255";
    fontColors[3] = "98 100 137 255";
};
//------------------------------------------------------------------------------
//ToolsTextEditDark Blue border variation
singleton GuiControlProfile( ToolsTextEditProfile_Num : ToolsTextEditProfile ) {  
    numbersOnly = true;
};
//------------------------------------------------------------------------------
//==============================================================================
//ToolsTextEditDark
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTextEditDark : ToolsDefaultProfile ) {
    opaque = true;
    bitmap = "tlab/gui/assets/element/GuiTextEditDark.png";

    hasBitmapArray = true;
    border = -2; // fix to display textEdit img
    //borderWidth = "1";  // fix to display textEdit img
    //borderColor = "100 100 100";
    fillColor = "242 241 240 0";
    fillColorHL = "255 255 255";
    fontColor = "194 254 254 255";
    fontColorHL = "255 255 255";
    fontColorSEL = "98 100 137";
    fontColorNA = "200 200 200";
    textOffset = "4 2";
    autoSizeWidth = false;
    autoSizeHeight = true;
    justify = "left";
    tab = true;
    canKeyFocus = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontSize = "15";
    fontColors[1] = "255 255 255 255";
    fontColors[2] = "200 200 200 255";
    fontColors[3] = "98 100 137 255";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "194 254 254 255";
   modal = "1";
};
//------------------------------------------------------------------------------
//ToolsTextEditDark Blue border variation
singleton GuiControlProfile( ToolsTextEditDark_Blue : ToolsTextEditDark ) {  
    bitmap = "tlab/gui/assets/element/GuiTextEditDark_Blue.png";
   bevelColorLL = "Magenta";
   fontSize = "17";
   textOffset = "0 0";
};
//------------------------------------------------------------------------------
//ToolsTextEditDark Blue border variation
singleton GuiControlProfile( ToolsTextEditDark_Num : ToolsTextEditDark_Blue ) {  
    numbersOnly = true;
};
//------------------------------------------------------------------------------


singleton GuiControlProfile(ToolsTextEditDark_Blue_S1 : ToolsTextEditDark_Blue)
{
   bevelColorLL = "255 0 255 255";
   fontSize = "14";
   fontType = "Arial";
};



