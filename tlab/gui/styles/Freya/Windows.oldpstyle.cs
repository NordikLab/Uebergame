//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Container Profiles
//==============================================================================

//==============================================================================
// WindowBase Profiles
//==============================================================================


//--- OBJECT WRITE BEGIN ---
$tmpGroup = new SimGroup() {
    canSave = "1";
    canSaveDynamicFields = "1";
    myFile = "art/gui/CastleBlast/Windows.pstyle.cs";

    new ScriptObject(ToolsWindowMain_Style) {
        internalName = "pfWindowMain";
        canSave = "1";
        canSaveDynamicFields = "1";
        autoSizeHeight = "0";
        autoSizeWidth = "0";
        bevelColorHL = "255 255 255 255";
        bevelColorLL = "0 0 0 255";       
        border = "-2";
        borderColor = "100 100 100 255";
        borderColorHL = "50 50 50 50";
        borderColorNA = "75 75 75 255";
        borderThickness = "0";
        canKeyFocus = "0";
        category = "Game";
        cursorColor = "0 0 0 255";
        fillColor = "164 145 99 255";
        fillColorHL = "222 222 222 0";
        fillColorNA = "201 201 201 19";
        fillColorSEL = "98 100 137 255";
        fontCharset = "ANSI";
        fontColor = "229 208 182 255";
        fontColorHL = "0 0 0 255";
        fontColorLink = "255 0 255 255";
        fontColorLinkHL = "255 0 255 255";
        fontColorNA = "0 0 0 255";
        fontColorSEL = "255 255 255 255";
        fontSize = "46";
        fontType = "MorrisRoman-Black";
        hasBitmapArray = "1";
        justify = "Center";
        modal = "1";
        mouseOverSelected = "0";
        numbersOnly = "0";
        opaque = "0";
        returnTab = "0";
        tab = "0";
        text = "untitled";
        textOffset = "40 2";
    };
    new ScriptObject(ToolsRolloutMain_Style) {
        internalName = "";
        canSave = "1";
        canSaveDynamicFields = "1";
        bevelColorHL = "255 255 255 255";
        bevelColorLL = "0 0 0 255";
       
        border = "-1";
        category = "Game";
        fillColor = "242 241 240 255";
        fillColorHL = "221 221 221 255";
        fillColorNA = "200 200 200 255";
        fontColor = "254 254 254 255";
        fontColorHL = "0 0 0 255";
        fontSize = "28";
        hasBitmapArray = "1";
        justify = "Left";
        opaque = "0";
        text = "untitled";
        textOffset = "8 8";
    };

};
//--- OBJECT WRITE END ---
