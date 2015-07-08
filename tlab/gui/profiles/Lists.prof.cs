//==============================================================================
// TorqueLab -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// DropDown Profiles (GuiPopUpMenuCtrl)
//==============================================================================

//==============================================================================
// DropdownBasic Styles
//------------------------------------------------------------------------------
//DropdownBasic Item
singleton GuiControlProfile(ToolsDropdownProfile_Item : ToolsDefaultProfile)
{
   modal = "1";
   fontSize = "22";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   fillColorSEL = "0 255 6 255";
   opaque = "1";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "0 0 0 255";
   fontColor = "0 0 0 255";
   category = "GameContainer";
   justify = "Center";
   textOffset = "4 0";
};
//------------------------------------------------------------------------------
//DropdownBasic List
singleton GuiControlProfile (ToolsDropdownProfile_List : ToolsDefaultProfile)
{   
   hasBitmapArray     = false;
   fontSize = "18";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
   modal = "1";
   fillColor = "25 25 25 237";
   fillColorHL = "180 113 18 169";
   fontColors[2] = "3 206 254 255";
   fontColorNA = "3 206 254 255";
   profileForChildren = "ToolsDropdownProfile_Item";
   fillColorSEL = "2 2 2 241";
   fontColors[3] = "15 243 48 255";
   fontColorSEL = "15 243 48 255";
   opaque = "1";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "0 0 0 255";
   fontColor = "0 0 0 255";
   tab = "1";
   canKeyFocus = "1";
   justify = "Center";
   textOffset = "4 0";
  
};
//------------------------------------------------------------------------------
//DropdownBasic Menu
singleton GuiControlProfile (ToolsDropdownProfile : ToolsDefaultProfile)
{   
  hasBitmapArray     = "1";
  profileForChildren = "ToolsDropdownProfile_List";
 	bitmap = "tlab/gui/assets/element/GuiDropdownProfile.png";
   fillColor = "242 241 241 255";
   fontSize = "17";
   fillColorHL = "228 228 235 255";
   fontColors[1] = "255 160 0 255";
   fontColors[2] = "3 206 254 255";
   fontColorHL = "255 160 0 255";
   fontColorNA = "3 206 254 255";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   fontType = "Davidan";
   modal = "1";
   fontColors[3] = "5 64 201 255";
   fontColorSEL = "5 64 201 255";
   bevelColorLL = "Magenta";
   opaque = "1";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "0 0 0 255";
   fontColor = "0 0 0 255";
   textOffset = "4 0";
};
//------------------------------------------------------------------------------
//==============================================================================
// DropdownBasic Thin Version
singleton GuiControlProfile(ToolsDropdownProfile_Thin : ToolsDropdownProfile)
{
   bitmap = "tlab/gui/assets/element/GuiDropdownProfile_Thin.png";
   fontSize = "15";
   justify = "Center";
   autoSizeHeight = "1";
};
//------------------------------------------------------------------------------
//==============================================================================
// DropdownBasic Thin Version
singleton GuiControlProfile(ToolsDropdownProfile_L1 : ToolsDropdownProfile)
{
   bitmap = "tlab/gui/assets/element/GuiDropdownProfile_L1.png";
   fontSize = "15";
   justify = "Center";
};
//------------------------------------------------------------------------------
//==============================================================================
// DropdownBasic Styles
//==============================================================================
//------------------------------------------------------------------------------
//DropdownBasic Item
singleton GuiControlProfile(ToolsDropdownBasic_Item : ToolsDefaultProfile)
{
   modal = "1";
   fontSize = "22";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
   fillColorSEL = "0 255 6 255";
};
//------------------------------------------------------------------------------
//DropdownBasic List
singleton GuiControlProfile (ToolsDropdownBasic_List : ToolsDefaultProfile)
{   
   hasBitmapArray     = false;
   fontSize = "18";
   fontColors[1] = "255 160 0 255";
   fontColorHL = "255 160 0 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
   modal = "1";
   fillColor = "25 25 25 205";
   fillColorHL = "180 113 18 169";
   fontColors[2] = "3 206 254 255";
   fontColorNA = "3 206 254 255";
   profileForChildren = "ToolsDropdownBasic_Item";
   fillColorSEL = "199 152 15 240";
   fontColors[3] = "254 3 62 255";
   fontColorSEL = "254 3 62 255";
   opaque = "1";
  
};
//------------------------------------------------------------------------------
//DropdownBasic Menu
singleton GuiControlProfile (ToolsDropdownBasic : ToolsDefaultProfile)
{   
  hasBitmapArray     = "1";
  profileForChildren = "ToolsDropdownBasic_List";
 	bitmap = "tlab/gui/assets/element/GuiDropdownAlt.png";
   fillColor = "242 241 241 255";
   fontSize = "17";
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
// DropdownBasic Thin Version
singleton GuiControlProfile(ToolsDropdownBasic_Thin : ToolsDropdownBasic)
{
   bitmap = "tlab/gui/assets/element/GuiDropdownAlt_Thin.png";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiTreeViewCtrl
//==============================================================================

//==============================================================================
// ToolsTreeViewProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTreeViewProfile : ToolsDefaultProfile ) {
    bitmap = "tlab/gui/assets/element/GuiTreeViewProfile.png";
    autoSizeHeight = true;
    canKeyFocus = true;
    fillColor = "255 255 255";
    fillColorHL = "228 228 235";
    fillColorSEL = "98 100 137";
    fillColorNA = "255 255 255";
    fontColor = "2 2 2 255";
    fontColorHL = "0 0 0";
    fontColorSEL= "255 255 255";
    fontColorNA = "200 200 200";
    borderColor = "128 000 000";
    borderColorHL = "255 228 235";
    fontSize = 14;
    opaque = false;
    border = false;
    category = "Tools";
    fontColors[2] = "200 200 200 255";
   fontType = "Gotham Book";
   fontColors[0] = "2 2 2 255";
};
//------------------------------------------------------------------------------
// ToolsTreeViewProfile Dark Variation
singleton GuiControlProfile( ToolsTreeViewProfile_Dark : ToolsTreeViewProfile ) {    
    fillColor = "255 255 255";
    fillColorHL = "228 228 235";
    fillColorSEL = "98 100 137";
    fillColorNA = "255 255 255";
    fontColor = "238 240 211 255";
    fontColorHL = "0 0 0";
    fontColorSEL= "255 255 255";
    fontColorNA = "200 200 200";   
   fontType = "Lato";
   fontSize = "18";
   fontColors[0] = "238 240 211 255";
};
//------------------------------------------------------------------------------
