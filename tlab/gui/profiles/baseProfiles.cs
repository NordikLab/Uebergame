//==============================================================================
// TorqueLab -> Core Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Those profiles are defined in the code directly
//==============================================================================


new GuiControlProfile (ToolsDefaultProfile) {
    tab = false;
    canKeyFocus = false;
    hasBitmapArray = false;
    mouseOverSelected = false;

    // fill color
    opaque = "1";
    fillColor = "243 241 241 0";
    fillColorHL ="229 229 236 0";
    fillColorSEL = "99 101 138 156";
    fillColorNA = "255 255 255 0";

    // border color
    border = 0;
    borderColor   = "100 100 100";
    borderColorHL = "50 50 50 50";
    borderColorNA = "75 75 75";

    // font
    fontType = "Gafata Std";
    fontSize = 16;
    fontCharset = ANSI;

    fontColor = "0 0 0";
    fontColorHL = "0 0 0";
    fontColorNA = "0 0 0";
    fontColorSEL= "255 255 255";

    // bitmap information
    bitmapBase = "";
    textOffset = "0 0";

    // used by guiTextControl
    modal = "1";
    justify = "left";
    autoSizeWidth = false;
    autoSizeHeight = false;
    returnTab = false;
    numbersOnly = false;
    cursorColor = "0 0 0 255";

    category = "Tools";
   borderThickness = "0";
    // sounds
    //soundButtonDown = "";
    //soundButtonOver = "";
};


new GuiControlProfile (ToolsGuiSolidDefaultProfile : ToolsDefaultProfile) {
    opaque = true;
    border = true;
    category = "Tools";
};

new GuiControlProfile (ToolsGuiTransparentProfile : ToolsDefaultProfile) {
    opaque = false;
    border = false;
    category = "Tools";
    modal = "1";
   fontColors[9] = "Magenta";
};

new GuiControlProfile( ToolsGuiGroupBorderProfile ) {
    border = false;
    opaque = false;
    hasBitmapArray = true;

    bitmap = "tlab/gui/icons/default/group-border";
    category = "Tools";
};


new GuiControlProfile (ToolsGuiToolTipProfile) {
    // fill color
    fillColor = "239 237 222";

    // border color
    borderColor   = "138 134 122";

    // font
    fontType = "Arial";
    fontSize = 14;
    fontColor = "0 0 0";

    category = "Tools";
};

singleton GuiControlProfile( ToolsGuiModelessDialogProfile : ToolsDefaultProfile ) {
    modal = false;
    category = "Tools";
};

singleton GuiControlProfile( ToolsDefaultProfile_NoModal : ToolsDefaultProfile ) {
    modal = false;
    category = "Tools";
};


new GuiControlProfile( ToolsGuiRLProgressBitmapProfile ) {
    border = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/icons/default/rl-loadingbar";
    category = "Tools";
};





new GuiControlProfile(ToolsGuiEditorTabPage) {
    opaque = true;
    border = false;
    fontColor = "0 0 0";
    fontColorHL = "0 0 0";
    fixedExtent = false;
    justify = "center";
    canKeyFocus = false;
    bitmap = "tlab/gui/icons/default/tab";
    hasBitmapArray = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiRadioProfile ) {
    fontSize = 14;
    fillColor = "232 232 232";
    fontColor = "20 20 20";
    fontColorHL = "80 80 80";
    fixedExtent = true;
    bitmap = "tlab/gui/icons/default/radioButton";
    hasBitmapArray = true;
    category = "Tools";
};






new GuiControlProfile( ToolsGuiListBoxProfile ) {
    tab = true;
    canKeyFocus = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiTextPadProfile ) {
    fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
    fontSize = ($platform $= "macos") ? 13 : 12;
    tab = true;
    canKeyFocus = true;

    // Deviate from the Default
    opaque=true;
    fillColor = "255 255 255";
    border = 0;
    category = "Tools";
};

new GuiControlProfile( ToolsGuiFormProfile : ToolsDefaultProfile ) {
    opaque = false;
    border = 5;
    justify = "center";
    profileForChildren = ToolsButtonProfile;
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/icons/default/button";
    category = "Tools";
};

// ----------------------------------------------------------------------------
singleton GuiControlProfile( ToolsGuiBackFillProfile ) {
    opaque = true;
    fillColor = "0 94 94";
    border = "1";
    borderColor = "255 128 128";
    fontType = "Gotham Book";
    fontSize = 12;
    fontColor = "0 0 0";
    fontColorHL = "50 50 50";
    fixedExtent = 1;
    justify = "center";
    category = "Editor";
    fontColors[1] = "50 50 50 255";
    fontColors[9] = "255 0 255 255";
};


//==============================================================================
// TorqueLab Top Menu Bar Profiles
//==============================================================================
//==============================================================================
// MenuBar Background
singleton GuiControlProfile (ToolsGuiMenuBarProfile  : ToolsDefaultProfile) {
    opaque = true;
    border = "1";
    category = "Tools";
   fillColor = "48 48 48 255";
   fontType = "Gotham Book";
   fontSize = "18";
   fontColors[0] = "0 226 255 255";
   fontColor = "0 226 255 255";
   justify = "Bottom";
   textOffset = "10 4";
   fontColors[8] = "255 0 255 255";
   fillColorHL = "97 97 97 66";
   fillColorNA = "127 64 29 255";
   fontColors[1] = "37 183 254 255";
   fontColors[2] = "208 132 6 255";
   fontColors[3] = "240 185 39 255";
   fontColorHL = "37 183 254 255";
   fontColorNA = "208 132 6 255";
   fontColorSEL = "240 185 39 255";
   cursorColor = "255 0 255 255";
   modal = true;
   borderColorHL = "254 254 222 236";
   borderColorNA = "255 213 0 210";
   bevelColorHL = "106 106 106 255";
   bevelColorLL = "3 3 213 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
   borderColor = "0 148 255 67";
};
//------------------------------------------------------------------------------
//==============================================================================
// MenuBar Items
singleton GuiControlProfile( ToolsGuiMenuProfile : ToolsGuiMenuBarProfile) {
    opaque = true;
    fillcolor = "41 41 41 193";
    fontColor = "206 243 254 255";
    fontColorHL = "64 222 254 255";   
    category = "Tools";
   fillColorHL = "43 43 43 241";
   fontType = "Gotham Book Bold";
   fontSize = "17";
   fontColors[0] = "206 243 254 255";
   fontColors[1] = "64 222 254 255";
   fillColorNA = "29 132 21 252";
   fillColorSEL = "11 43 190 255";
   justify = "Center";
   fontColors[2] = "102 62 27 188";
   fontColors[3] = "254 227 97 255";
   fontColorNA = "102 62 27 188";
   fontColorSEL = "254 227 97 255";
   cursorColor = "0 0 0 255";
   modal = true;
   border = "0";
   borderColor = "122 118 122 255";
   borderColorHL = "141 141 141 255";
   borderColorNA = "120 113 120 255";
   fontColors[7] = "255 0 255 255";
   bevelColorHL = "104 104 104 255";
   bevelColorLL = "157 157 157 255";
};
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsGuiMenuAltProfile) {
    opaque = true;
    fillcolor = "255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "255 255 255";
    fillColor = "0 0 0 100";
    category = "Tools";
};
