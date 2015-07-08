//==============================================================================
// TorqueLab -> Panels Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Special Panels which simply set predefined fill color and appropriate text
//==============================================================================

//==============================================================================
// Color Panels -> Main theme colors (A-B-C)
//==============================================================================

//==============================================================================
singleton GuiControlProfile(ToolsPanelColorA : ToolsDefaultProfile) {
    fillColor = "14 151 226 255";
    opaque = "1";
    bevelColorHL = "255 0 255 255";
    fontType = "Aileron";
    fontSize = "16";
    fontColors[4] = "255 0 255 255";
    fontColorLink = "255 0 255 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    category = "ToolsPanels";
    fillColorNA = "White";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsPanelColorB : ToolsPanelColorA) {
    opaque = "1";
    fillColor = "17 45 58 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "0 0 0 255";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "0 0 0 255";
    fillColorNA = "255 255 255 255";
    fontColors[9] = "Magenta";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsPanelColorC : ToolsPanelColorA) {
    opaque = "1";
    fillColor = "101 136 166 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "0 0 0 255";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "0 0 0 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// Dark Color Panels -> Dark theme colors (A-B-C)
//==============================================================================

//==============================================================================
singleton GuiControlProfile(ToolsPanelDarkA : ToolsDefaultProfile) {
    opaque = "1";
    fillColor = "48 48 48 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "Black";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "Black";
    fontColors[9] = "Magenta";
    fontColors[7] = "255 0 255 255";
    fillColorHL = "228 228 235 255";
};
singleton GuiControlProfile(ToolsPanelDarkB : ToolsPanelDarkA) {
    fillColor = "58 64 68 255";
    opaque = "1";
    bevelColorHL = "Fuchsia";
    fontType = "Aileron";
    fontSize = "16";
    fontColors[4] = "255 0 255 255";
    fontColorLink = "255 0 255 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    category = "ToolsPanels";
    fillColorNA = "255 255 255 255";
    fontColors[9] = "255 0 255 255";
    fontColors[7] = "255 0 255 255";
    fillColorHL = "228 228 235 255";
    fontColors[1] = "0 0 0 255";
    fontColorHL = "0 0 0 255";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsPanelDarkC : ToolsPanelDarkA) {
    opaque = "1";
    fillColor = "59 76 88 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "Black";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "Black";
    fontColors[9] = "Magenta";
    fontColors[7] = "255 0 255 255";
    fillColorHL = "228 228 235 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// Light Color Panels -> Light theme colors (A-B-C)
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsPanelLightA : ToolsDefaultProfile) {
    opaque = "1";
    fillColor = "127 143 154 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "Black";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "Black";
    fontType = "Gotham Black";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsPanelLightB : ToolsPanelLightA) {
    fillColor = "101 136 166 255";
    opaque = "1";
    bevelColorHL = "Magenta";
    fontType = "Gotham Black";
    fontSize = "16";
    fontColors[4] = "255 0 255 255";
    fontColorLink = "255 0 255 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    category = "ToolsPanels";
    fillColorNA = "255 255 255 255";
    fontColors[3] = "White";
    fontColorSEL = "White";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsPanelLightC : ToolsPanelLightA) {
    opaque = "1";
    fillColor = "101 136 166 255";
    fontColors[0] = "88 1 192 255";
    fontColor = "88 1 192 255";
    fontColors[1] = "Black";
    fontColors[6] = "255 0 255 255";
    fontColorHL = "Black";
};
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------




singleton GuiControlProfile(ToolsPanelDarkA_T50 : ToolsPanelDarkA)
{
   fillColor = "1 1 1 131";
};

