//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//exec("scripts/gui/profileStyleSetup.cs");
//==============================================================================

//==============================================================================
// Standard Text Profiles
//==============================================================================
singleton GuiControlProfile(ToolsTextBase : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Bold";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "15";
   fontColors[4] = "Magenta";
   fontColorLink = "Magenta";
   opaque = "0";

};
singleton GuiControlProfile(ToolsTextBase_C : ToolsTextBase) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextBase_R : ToolsTextBase) {
    locked = true;
    justify = "Right";
};
singleton GuiControlProfile (ToolsTextBase_Auto : ToolsTextBase) {    
    autoSizeWidth = true;
    autoSizeHeight = true;   
};

singleton GuiControlProfile( ToolsTextBase_ML : ToolsTextBase ) {   
    autoSizeWidth = true;
    autoSizeHeight = true;
    border = false;    
};
singleton GuiControlProfile( ToolsTextBase_List : ToolsTextBase ) {
    tab = "0";
    canKeyFocus = "0";
    category = "Tools";
   mouseOverSelected = "0";
   modal = "0";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsTextBase_S1 : ToolsTextBase) {
    fontSize = "14";
    fontType = "Gotham Book";
   fontColors[7] = "255 0 255 255";
};
singleton GuiControlProfile(ToolsTextBase_S1_C : ToolsTextBase_S1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextBase_S1_R : ToolsTextBase_S1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsTextBase_L1 : ToolsTextBase) {
    fontSize = "28";
};
singleton GuiControlProfile(ToolsTextBase_L1_C : ToolsTextBase_L1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextBase_L1_R : ToolsTextBase_L1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------


//==============================================================================
// Standard Text Header Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsTextBase_H1 : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "17";

};
singleton GuiControlProfile(ToolsTextBase_H1_C : ToolsTextBase_H1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextBase_H1_R : ToolsTextBase_H1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

//==============================================================================
// Dark text profile for Light background
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt : ToolsDefaultProfile) {
    fontColor = "254 224 97 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "left";
    category = "LabText";
   fontSize = "16";
   fontColors[0] = "254 224 97 255";

};
singleton GuiControlProfile(ToolsTextAlt_C : ToolsTextAlt) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextAlt_R : ToolsTextAlt) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt_S1 : ToolsTextAlt) {
    fontSize = "18";
};
singleton GuiControlProfile(ToolsTextAlt_S1_C : ToolsTextAlt_S1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextAlt_S1_R : ToolsTextAlt_S1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt_L1 : ToolsTextAlt) {
    fontSize = "18";
   fontColors[0] = "222 175 51 255";
   fontColor = "222 175 51 255";
};
singleton GuiControlProfile(ToolsTextAlt_L1_C : ToolsTextAlt_L1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextAlt_L1_R : ToolsTextAlt_L1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

//==============================================================================
// Standard Text Header Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsTextAlt_H1 : ToolsDefaultProfile) {
    fontColor = "0 255 231 208";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "0 255 231 208";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "24";

};
singleton GuiControlProfile(ToolsTextAlt_H1_C : ToolsTextAlt_H1) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsTextAlt_H1_R : ToolsTextAlt_H1) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile(ToolsTextBaseA : ToolsTextBase)
{
   fontColors[0] = "254 201 127 255";
   fontColors[7] = "255 0 255 255";
   fontColor = "254 201 127 255";
   fontType = "Gotham Bold";
};

singleton GuiControlProfile(ToolsTextBaseA_S1 : ToolsTextBaseA)
{
   fontType = "Gotham Book";
};
