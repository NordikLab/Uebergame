//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ->Added Gui style support
// -->Delete Profiles when reloaded
// -->Image array store in Global
//==============================================================================

function Lab::loadScriptsProfiles(%this) {
   // exec("tlab/gui/labStyleSetup.cs");
   // exec("tlab/gui/labStyleSave.cs");
   // exec("tlab/gui/labStyleLoad.cs");
   // exec("tlab/gui/profileAnalyse.cs");
}
//Lab.loadScriptsProfiles();
exec("tlab/gui/profiles/baseProfiles.cs");

//exec("tlab/gui/profiles/defaultProfiles.cs");
%filePathScript = "tlab/gui/profiles/*.prof.cs";
for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {   
    exec( %file );
}
exec("tlab/gui/profiles/editorProfiles.cs");
exec("tlab/gui/profiles/inspectorProfiles.cs");
//Lab.initProfileStyleData();






