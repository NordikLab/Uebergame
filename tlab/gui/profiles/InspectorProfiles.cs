//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Base Inspector Profile
singleton GuiControlProfile( GuiInspectorProfile  : ToolsDefaultProfile ) {
    opaque = true;
    fillColor = "255 255 255 255";
    border = 0;
    cankeyfocus = true;
    tab = true;
    category = "Editor";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiPopUpMenuProfile - Default profile might overwrite one used in game
//------------------------------------------------------------------------------
//GuiPopUpMenuProfile Item
delObj(GuiPopUpMenuProfile_Item);
singleton GuiControlProfile(GuiPopUpMenuProfile_List : ToolsDefaultProfile)
{
   modal = "1";
   fontSize = "18";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
   fillColorSEL = "199 152 15 240";
   opaque = "1";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "0 0 0 255";
   fontColor = "0 0 0 255";
   fillColor = "0 1 16 255";
   fillColorHL = "180 113 18 169";
   fontColors[2] = "3 206 254 255";
   fontColors[3] = "254 3 62 255";
   fontColorNA = "3 206 254 255";
   fontColorSEL = "254 3 62 255";
   profileForChildren = "GuiPopUpMenuProfile_Item";
};
//------------------------------------------------------------------------------
//GuiPopUpMenuProfile List
delObj(GuiPopUpMenuProfile_List);
singleton GuiControlProfile (GuiPopUpMenuProfile : ToolsDefaultProfile)
{   
   hasBitmapArray     = "1";
   fontSize = "15";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "0";
   autoSizeHeight = "1";
   modal = "1";
   fillColor = "242 241 241 255";
   fillColorHL = "228 228 235 255";
   fontColors[2] = "3 206 254 255";
   fontColorNA = "3 206 254 255";
   profileForChildren = "ToolsDropdownProfile_List";
   fillColorSEL = "98 100 137 255";
   fontColors[3] = "254 3 62 255";
   fontColorSEL = "254 3 62 255";
   opaque = "0";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "0 0 0 255";
   fontColor = "0 0 0 255";
   fontType = "Davidan";
   justify = "Center";
   bitmap = "tlab/gui/assets/element/GuiDropdownProfile_Thin.png";
   category = "Tools";
  
};
//------------------------------------------------------------------------------
//GuiPopUpMenuProfile Menu
delObj(GuiPopUpMenuProfile);
singleton GuiControlProfile (GuiPopUpMenuProfile : ToolsDefaultProfile)
{   
  hasBitmapArray     = "1";
  profileForChildren = "ToolsDropdownProfile_List";
 	bitmap = "tlab/gui/assets/element/GuiDropdownProfile_Thin.png";
   fontSize = "15";
   justify = "Center";
   fillColor = "242 241 241 255";  
   fillColorHL = "228 228 235 255";
   fontColors[1] = "255 160 0 255";
   fontColors[2] = "3 206 254 255";
   fontColorHL = "255 160 0 255";
   fontColorNA = "3 206 254 255";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   fontType = "Davidan";
   modal = "1";
   fontColors[3] = "254 3 62 255";
   fontColorSEL = "254 3 62 255";
   bevelColorLL = "Magenta";
};
//------------------------------------------------------------------------------

//==============================================================================
//ToolsButtonProfile - Default profile might overwrite on used in game
//------------------------------------------------------------------------------
delObj(GuiButtonProfile);
singleton GuiControlProfile( GuiButtonProfile : ToolsDefaultProfile ) {
    fontSize = "16";
    fontType = "Gafata Std Bold";
    fontColor = "254 254 254 255";
    justify = "center";
    category = "ToolsButtons";
    opaque = "1";
    border = "-2";
    fontColors[0] = "254 254 254 255";
    fontColors[2] = "200 200 200 255";
    fontColorNA = "200 200 200 255";
    bitmap = "tlab/gui/assets/button/GuiButtonProfile.png";
    hasBitmapArray = "1";
    fixedExtent = "0";
   bevelColorLL = "Magenta";
   textOffset = "0 2";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
    modal = true;
};
//------------------------------------------------------------------------------


//==============================================================================
//ToolsCheckBoxProfile_S1 Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( GuiInspectorCheckBoxProfile : ToolsDefaultProfile ) {
     opaque = false;
    fillColor = "232 232 232";
    border = false;
    borderColor = "100 100 100";
    fontSize = "18";
    fontColor = "250 220 171 255";
    fontColorHL = "80 80 80";
    fontColorNA = "200 200 200";
    fixedExtent = 1;
    justify = "left";
    bitmap = "tlab/gui/assets/button/GuiCheckboxProfile_S1.png";
    hasBitmapArray = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontColors[0] = "250 220 171 255";
    fontColors[1] = "80 80 80 255";
    fontColors[2] = "200 200 200 255";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";
    modal = true;
};
//------------------------------------------------------------------------------

//==============================================================================
//Used in SourceCode
singleton GuiControlProfile( GuiInspectorButtonProfile : ToolsButtonProfile ) {
    //border = 1;
     fontSize = "16";
    fontType = "Gafata Std Bold";
    fontColor = "254 254 254 255";
    justify = "center";
    opaque = "1";
    border = "1";
    fontColors[0] = "254 254 254 255";
    fontColors[2] = "200 200 200 255";
    fontColorNA = "200 200 200 255";
    bitmap = "tlab/gui/assets/button/GuiButtonProfile.png";
    fixedExtent = "0";
   bevelColorLL = "Magenta";
   textOffset = "0 2";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
    category = "Editor";
     modal = true;
};
//------------------------------------------------------------------------------
//==============================================================================
//Used in SourceCode
singleton GuiControlProfile(GuiInspectorSwatchButtonProfile : GuiInspectorButtonProfile)
{
	fillColor = "254 253 253 255";
	fillColorHL = "221 221 221 255";
	fillColorNA = "200 200 200 255";
	fontSize = "24";
	textOffset = "16 10";
	bitmap = "tlab/gui/assets/button/GuiButtonProfile.png";
	hasBitmapArray = "1";
	fontColors[0] = "253 253 253 255";
	fontColor = "253 253 253 255";
   border = "-1";
   fontColors[2] = "Black";
   fontColorNA = "Black";
    modal = true;
};
//------------------------------------------------------------------------------

//==============================================================================
// Inspector Text Profiles
//==============================================================================

//==============================================================================
// SourceCode TextEdit Profile
singleton GuiControlProfile( GuiInspectorTextEditProfile : ToolsDefaultProfile ) {
    borderColor = "100 100 100 255";
    borderColorNA = "75 75 75 255";
    fillColorNA = "White";
    borderColorHL = "50 50 50 50";
    category = "Editor";
    tab = "1";
    canKeyFocus = "1";
    opaque = "1";
    fillColor = "0 0 0 0";
    fillColorHL = "2 2 2 49";
    fontType = "Arial";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "100 100 100 255";
    fontColors[3] = "16 108 87 255";
    fontColors[9] = "255 0 255 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "100 100 100 255";
    fontColorSEL = "16 108 87 255";
    fontColors[0] = "231 224 178 255";
    fontColor = "231 224 178 255";
   bitmap = "tlab/gui/assets/element/GuiTextEditDark.png";
   hasBitmapArray = "1";
   modal = true;
  
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( GuiDropdownTextEditProfile :  GuiInspectorTextEditProfile ) {
    bitmap = "tlab/gui/icons/default/dropdown-textEdit";
    category = "Editor";
     modal = true;
};
//------------------------------------------------------------------------------

singleton GuiControlProfile( GuiInspectorFieldInfoMLTextProfile : ToolsDefaultProfile ) {
    opaque = false;
    border = 0;
    textOffset = "5 0";
    category = "Editor";
};


//==============================================================================
// Inspector Group Profiles
//==============================================================================
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorGroupProfile : ToolsDefaultProfile ) {
    justify = "Left";
    category = "Editor";
    fontType = "Gotham Bold";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "25 25 25 220";
    fontColors[2] = "128 128 128 255";
    fontColors[9] = "255 0 255 255";
    fontColor = "238 238 238 255";
    fontColorHL = "25 25 25 220";
    fontColorNA = "128 128 128 255";
    textOffset = "20 0";
    bitmap = "tlab/gui/assets/container/GuiRolloutProfile.png";
    opaque = "0";
    fillColor = "0 0 0 237";
    fillColorNA = "255 255 255 255";
    fontSize = "15";
   fillColorHL = "255 255 255 255";
   fontColors[3] = "43 107 206 255";
   fontColorSEL = "43 107 206 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// Inspector Fields Profiles
//==============================================================================
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorFieldProfile : ToolsDefaultProfile ) {
    fontType    = "Arial";
    fontSize    = "14";

    fontColor = "238 238 238 255";
    fontColorHL = "231 224 178 255";
    fontColorNA = "190 190 190 255";

    justify = "left";
    opaque = false;
    border = false;

    bitmap = "tlab/gui/icons/default/classImages/rollout";

    textOffset = "10 0";

    category = "Editor";
    tab = "1";
    canKeyFocus = "1";
    fillColor = "51 51 51 255";
    fillColorHL = "32 32 34 255";
    fillColorNA = "244 244 244 255";
    borderColor = "190 190 190 255";
    borderColorHL = "156 156 156 255";
    borderColorNA = "200 200 200 255";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColors[2] = "190 190 190 255";
    fontColors[9] = "255 0 255 255";
};
//------------------------------------------------------------------------------
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorMultiFieldProfile : GuiInspectorFieldProfile )
{
   opaque = true;
   fillColor = "50 50 230 30";
};
//------------------------------------------------------------------------------
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorDynamicFieldProfile : GuiInspectorFieldProfile ) {
    border = "0";
    borderColor = "190 190 190 255";
    opaque = "1";
    fillColor = "51 51 51 255";
    fillColorHL = "32 32 34 255";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColors[2] = "100 100 100 255";
    fontColors[3] = "43 107 206 255";
    fontColor = "238 238 238 255";
    fontColorHL = "231 224 178 255";
    fontColorNA = "100 100 100 255";
    fontColorSEL = "43 107 206 255";
   dynamicField = "defaultValue";
};
//------------------------------------------------------------------------------

//==============================================================================
// Used in SourceCode -> Rollout for Array settings (Ex: GroundCover Layers)
singleton GuiControlProfile( GuiInspectorRolloutProfile0 : ToolsDefaultProfile) {
    // font
    fontType = "Gotham Book";
    fontSize = 14;

    fontColor = "254 229 115 255";
    fontColorHL = "32 100 100";
    fontColorNA = "0 0 0";

    justify = "left";
    opaque = false;

    border = 0;
    borderColor   = "190 190 190";
    borderColorHL = "156 156 156";
    borderColorNA = "64 64 64";

    bitmap = "tlab/gui/assets/container/GuiRolloutProfile_Thin.png";

    textOffset = "20 -1";
    category = "Editor";
    fontColors[0] = "254 229 115 255";
    fontColors[1] = "32 100 100 255";
    fontColors[9] = "255 0 255 255";
     modal = true;
};
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorStackProfile ) {
    opaque = "1";
    border = false;
    category = "Editor";
    fillColor = "51 51 51 255";
    fontColors[6] = "255 0 255 255";
    fillColorHL = "32 32 34 255";
    fontColors[0] = "238 238 238 255";
    fontColor = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColorHL = "231 224 178 255";
};
//------------------------------------------------------------------------------


//==============================================================================
// Used in SourceCode - Inspector Checkbox single profile
singleton GuiControlProfile( InspectorTypeCheckboxProfile : GuiInspectorFieldProfile ) {
    bitmap = "tlab/gui/assets/button/GuiCheckboxProfile_S1.png";
    hasBitmapArray = true;
    opaque=false;
    border=false;
    textOffset = "4 0";
    category = "Editor";
    modal = true;
};
//------------------------------------------------------------------------------






//==============================================================================
// Gui Inspector Profiles
//==============================================================================






//==============================================================================
// Push, Pop and Toggle Dialogs
//==============================================================================





singleton GuiControlProfile(GuiPopUpMenuProfile_Item : ToolsDefaultProfile)
{
   opaque = "1";
   fillColorSEL = "0 255 6 255";
   bevelColorHL = "255 0 255 255";
   fontSize = "22";
   fontColors[0] = "0 0 0 255";
   fontColors[1] = "255 160 0 255";
   fontColor = "0 0 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
};
