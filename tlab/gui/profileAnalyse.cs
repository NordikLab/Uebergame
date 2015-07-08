//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Define what fonts to use in the interface
//==============================================================================

//Arial= ToolsGuiToolTipProfile ToolsPanelDarkA GuiBackFillProfile GuiInspectorTextEditProfile GuiInspectorGroupProfile GuiInspectorFieldProfile
//       GuiInspectorDynamicFieldProfile GuiInspectorRolloutProfile0 GuiInspectorTypeFileNameProfile GuiDirectoryTreeProfile GuiDirectoryFileListProfile ToolsButtonArray
//Lab.extractProfilesWithField("bevelColorHL");
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::extractProfilesWithField( %this,%field,%noOutput ) {

    %count = getFieldCount($InfoProfileValues[%field]);
    for(%i = 0; %i<%count; %i++) {
        %value = getField($InfoProfileValues[%field],%i);
        $InfoProfileAdded[%field,%value] = false;
        $InfoProfileList[%field,%value] = "";
    }

    $InfoProfileValues[%field] = "";
    $InfoProfile[%field] ="";


    %this.getProfileFieldFromFile( "tlab/gui/profiles/baseProfiles.cs" );
    %this.getProfileFieldFromFile( "tlab/gui/profiles/editorProfiles.cs" );

    %filePathScript = "tlab/gui/profiles/*.prof.cs";
    for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
        info("CHecking file for font::",fileBase(%file));
        %this.getProfileFieldFromFile( %file,%field );
    }

    if(%noOutput)return;

    %outPutfile = "tlab/gui/reports/"@%field@"Report.txt";
    %fileWrite = getFileWriteObj(%outPutfile,!%resetOutput);
    %fileWrite.writeLine("================================================");
    %fileWrite.writeLine("Report of profile analyse to find field:" SPC %field);
    %fileWrite.writeLine("================================================");
    %fileWrite.writeLine("--------------------------------------------");
    %fileWrite.writeLine("List of profiles found with field:" SPC %field);
    %fileWrite.writeLine($InfoProfile[%field]);
    %fileWrite.writeLine("--------------------------------------------");
    %fileWrite.writeLine("List of different values found for field:" SPC %field);
    %fileWrite.writeLine($InfoProfileValues[%field]);
    %fileWrite.writeLine("--------------------------------------------");
    %fileWrite.writeLine("List of profiles set with each values for field:" SPC %field);


    %count = getFieldCount($InfoProfileValues[%field]);
    for(%i = 0; %i<%count; %i++) {
        %value = getField($InfoProfileValues[%field],%i);
        %fileWrite.writeLine("---------------");
        %fileWrite.writeLine(%value SPC "use by profiles:");
        %fileWrite.writeLine($InfoProfileList[%field,%value]);

    }
    %fileWrite.writeLine("================================================");
    closeFileObj( %fileWrite);
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::getProfileFieldFromFile( %this,%file,%field,%resetOutput ) {

    %fileObj = getFileReadObj(%file);

    if (!isObject(%fileObj)) return;
    while( !%fileObj.isEOF() ) {
        %line = %fileObj.readline();

        if (strstr(%line,"GuiControlProfile") !$= "-1") {
            // Prints 3456789
            %lineFix =  strchr( %line , "(" );
            %lineFix = strReplace(%lineFix,":"," ");
            %lineFix = strReplace(%lineFix,")"," ");
            %lineFix = trim(strReplace(%lineFix,"(",""));
            %profileName = getWord(%lineFix,0);

        } else if (strstr(%line,%field) !$= "-1") {
            %value =  strchr( %line , "\"" );
            %value = strReplace(%value,"\"","");
            %value = strReplace(%value,";","");
            %value = trim(%value);


            $InfoProfile[%field] = trim($InfoProfile[%field] SPC %profileName);
            $InfoProfileList[%field,%value] = trim($InfoProfileList[%field,%value] SPC %profileName);


            if ( $InfoProfileAdded[%field,%value]) continue;
            $InfoProfileValues[%field] = trim($InfoProfileValues[%field] TAB %value);
            $InfoProfileAdded[%field,%value] = true;
        }
    }

    closeFileObj(%fileObj);


}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::checkDuplicatedProfiles( %this ) {
    %this.checkDuplicatedProfileFile( "tlab/gui/profiles/baseProfiles.cs" );
    %this.checkDuplicatedProfileFile( "tlab/gui/profiles/editorProfiles.cs" );

    %filePathScript = "tlab/gui/profiles/*.prof.cs";
    for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {

        %this.checkDuplicatedProfileFile( %file,%field );
    }


    %outPutfile = "tlab/gui/reports/duplicatedProfilesReport.txt";
    %fileWrite = getFileWriteObj(%outPutfile,!%resetOutput);
    %fileWrite.writeLine("================================================");
    %fileWrite.writeLine("Report of duplicated profiles");
    %fileWrite.writeLine("--------------------------------------------");
    %fileWrite.writeLine($ProfilesDuplicatedlist);
    %fileWrite.writeLine("================================================");
    closeFileObj( %fileWrite);

    foreach$(%profile in $ProfilesChecklist)
        $ProfilesChecked[%profile] = false;
}
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::checkDuplicatedProfileFile( %this,%file ) {

    %fileObj = getFileReadObj(%file);
    if (!isObject(%fileObj)) return;
    while( !%fileObj.isEOF() ) {
        %line = %fileObj.readline();
        %removeLine = false;
        if (%noRemoveObjectNext)
            %removeObject = false;

        if (strstr(%line,"GuiControlProfile") !$= "-1") {
            // Prints 3456789
            %lineFix =  strchr( %line , "(" );
            %lineFix = strReplace(%lineFix,":"," ");
            %lineFix = strReplace(%lineFix,")"," ");
            %lineFix = trim(strReplace(%lineFix,"(",""));
            %profileName = getWord(%lineFix,0);



            if ($ProfilesChecked[%profileName]) {
                $ProfilesDuplicatedlist = trim($ProfilesDuplicatedlist SPC %profileName);
                %removeObject = true;
                %noRemoveObjectNext = false;
                %needRewrite = true;
            } else {
                $ProfilesChecked[%profileName] = true;
                $ProfilesChecklist = trim($ProfilesChecklist SPC %profileName);

            }
        } else if(strstr(%line,"delObj") !$= "-1") {
            %old = %line;
            %line = "//=======================================================";
            %needRewrite = true;
            %removeLine = true;
        } else if(strstr(%line,"new ") !$= "-1") {
            %old = %line;
            %line = strreplace(%line,"new ","singleton ");
            %needRewrite = true;
            %removeLine = true;
        } else if(strstr(%line,"};") !$= "-1") {
            if ( %removeObject ) {
                %noRemoveObjectNext = true;
            }

        }
        if (!%removeLine && !%removeObject) {
            %lineList[%i++] = %line;
        }
    }

    closeFileObj(%fileObj);

    if (!%needRewrite) return;
    %outPutfile = strreplace(%file,".cs","2.cs");
    %fileWrite = getFileWriteObj(%outPutfile);

    for(%j= 1; %j<=%i; %j++) {
        %fileWrite.writeLine(%lineList[%j]);

    }
    closeFileObj(%fileWrite);
}
//------------------------------------------------------------------------------
