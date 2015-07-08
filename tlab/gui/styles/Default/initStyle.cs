//==============================================================================
// TorqueLab -> Default Style setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Font Setup
//==============================================================================

$LabFonts["Default"] = "Gotham Book";
$LabFonts["Bold"] = "Gotham Bold";
$LabFonts["Thick"] = "Gotham Black";

$LabStyleFontColor[1] = "238 238 238 255";
$LabStyleFontColor[2] = "231 224 178 255";
$LabStyleFontColor[3] = "101 136 166 255";
$LabStyleFontDarkColor[1] = "14 151 226 255";
$LabStyleFontDarkColor[2] = "17 45 58 255";
$LabStyleFontDarkColor[3] = "101 136 166 255";

//==============================================================================
// Text Profiles
//==============================================================================
$LabStyleColor[1] = "14 151 226 255";
$LabStyleColor[2] = "17 45 58 255";
$LabStyleColor[3] = "101 136 166 255";

$LabStyleBackground[1] = "51 51 51 255";
$LabStyleBackground[2] = "57 74 86 255";
$LabStyleBackground[3] = "101 136 166 255";

$LabStyleDarkColor[1] = "51 51 51 255";
$LabStyleDarkColor[2] = "57 74 86 255";
$LabStyleDarkColor[3] = "101 136 166 255";

$LabStyleLightColor[1] = "127 143 154 255";
$LabStyleLightColor[2] = "101 136 166 255";
$LabStyleLightColor[3] = "101 136 166 255";

$LabStyleDarkHL = "32 32 34 255";
%list = "ToolsPanelDarkA GuiInspectorFieldProfile GuiInspectorStackProfile GuiInspectorDynamicFieldProfile";
$StyleColorGroup["DarkColor1"] = "fillColor Background1\tfillColorHL DarkHL\tfontColorHL FontColor2\tfontColor FontColor1";
$StyleColorGroupProfiles["DarkColor1"] = %list;

%list = "GuiInspectorTextEditProfile";
$StyleColorGroup["DarkEdit"] = "fontColor FontColor2";
$StyleColorGroupProfiles["DarkEdit"] = %list;


$LabStyleGroups["Default"] = "DarkColor1 DarkEdit";
%list = "ToolsPanelColorA";
$LabProfilesStyleColor[1] = %list;
%list = "ToolsPanelColorB";
$LabProfilesStyleColor[2] = %list;
%list = "ToolsPanelColorC";
$LabProfilesStyleColor[3] = %list;

%list = "ToolsPanelDarkA";
$LabProfilesStyleBackground[1] = %list;
%list = "ToolsPanelDarkB GuiInspectorStackProfile";
$LabProfilesStyleBackground[2] = %list;
%list = "ToolsPanelDarkC";
$LabProfilesStyleBackground[3] = %list;

%list = "ToolsPanelDarkA";
$LabProfilesStyleDarkColor[1] = %list;
%list = "ToolsPanelDarkB";
$LabProfilesStyleDarkColor[2] = %list;
%list = "ToolsPanelDarkC";
$LabProfilesStyleDarkColor[3] = %list;

%list = "ToolsPanelLightA";
$LabProfilesStyleLightColor[1] = %list;
%list = "ToolsPanelLightB";
$LabProfilesStyleLightColor[2] = %list;
%list = "ToolsPanelLightC";
$LabProfilesStyleLightColor[3] = %list;