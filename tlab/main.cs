//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

%helpersLab = "tlab/helpers/initHelpers.cs";
if (isFile(%helpersLab))
	exec(%helpersLab);


//==============================================================================
// TorqueLab Core Global Settings
//==============================================================================
// Path to the folder that contains the editors we will load.
$Lab::resourcePath = "tlab/";

$LabGameMap = "moveMap";
// Global holding material list for active simobject
$Lab::materialEditorList = "";

// These must be loaded first, in this order, before anything else is loaded
$Lab::loadFirst = "sceneEditor";

// These folders must be skipped for initial load
$LabIgnoreEnableFolderList = "debugger forestEditor levels";



//---------------------------------------------------------------------------------------------
// Tools Package.
//---------------------------------------------------------------------------------------------
function EditorIsActive() {
    return ( isObject(EditorGui) && Canvas.getContent() == EditorGui.getId() );
}

function GuiEditorIsActive() {
    return ( isObject(GuiEditorGui) && Canvas.getContent() == GuiEditorGui.getId() );
}

function loadTorqueLabProfiles( )
{
	//------------------------------------------------------------------------------
	//Start by loading the Gui Profiles
	exec("tlab/gui/initGuiProfiles.cs");
	exec( "tlab/EditorLab/gui/core/cursors.ed.cs" );
//	exec( "tlab/editorClasses/gui/panels/navPanelProfiles.ed.cs" );
	// Make sure we get editor profiles before any GUI's
	// BUG: these dialogs are needed earlier in the init sequence, and should be moved to
	// common, along with the guiProfiles they depend on.

}
//==============================================================================
// TorqueLab Package overiding some default functions
//==============================================================================
package Lab {
   
    // Start-up.
    function onStart() {
    		//Load the Editor profile before loading the game onStart
    		loadTorqueLabProfiles();
    		
        Parent::onStart();       
        new Settings(EditorSettings) {
            file = "tlab/settings.xml";
        };
        EditorSettings.read();

         new Settings(LabCfg) {
            file = "tlab/config.xml";
        };
        

        info( "Initializing TorqueLab" );
      
        // Default file path when saving from the editor (such as prefabs)
        if ($Pref::WorldEditor::LastPath $= "") {
            $Pref::WorldEditor::LastPath = getMainDotCsDir();
        }

        $Lab = new ScriptObject(Lab);
       
        exec("tlab/core/execScripts.cs");
        $LabGuiExeced = true;
        // Common GUI stuff.
        Lab.initLabEditor();

        //%toggle = $Scripts::ignoreDSOs;
        //$Scripts::ignoreDSOs = true;

        $ignoredDatablockSet = new SimSet();

        // fill the list of editors
        $editors[count] = getWordCount( $Lab::loadFirst );
        for ( %i = 0; %i < $editors[count]; %i++ ) {
            $editors[%i] = getWord( $Lab::loadFirst, %i );
        }

        %pattern = $Lab::resourcePath @ "/*/main.cs";
        %folder = findFirstFile( %pattern );
        if ( %folder $= "") {
            // if we have absolutely no matches for main.cs, we look for main.cs.dso
            %pattern = $Lab::resourcePath @ "/*/main.cs.dso";
            %folder = findFirstFile( %pattern );
        }
        while ( %folder !$= "" ) {
            if( filePath( %folder ) !$= "tools" && filePath( %folder ) !$= "tlab" ) { // Skip the actual 'tools' folder...we want the children
                %folder = filePath( %folder );
                %editor = fileName( %folder );
                if ( IsDirectory( %folder ) ) {
                    // Yes, this sucks and should be done better
                    if ( strstr( $Lab::loadFirst, %editor ) == -1 ) {
                        $editors[$editors[count]] = %editor;
                        $editors[count]++;
                    }
                }
            }
            %folder = findNextFile( %pattern );
        }

        // initialize every editor

        %count = $editors[count];
        //  exec( "./worldEditor/main.cs" );
        foreach$(%tmpFolder in $LabIgnoreEnableFolderList)
            $ToolFolder[%tmpFolder] = "1";
        for ( %i = 0; %i < %count; %i++ ) {
            eval("%enabledEd = $pref::WorldEditor::"@$editors[%i]@"::Enabled;");
            if (!%enabledEd && !$ToolFolder[%tmpFolder]) {
                continue;
            }
            exec( "./" @ $editors[%i] @ "/main.cs" );

            %initializeFunction = "initialize" @ $editors[%i];
            if( isFunction( %initializeFunction ) )
                call( %initializeFunction );
        }
		
        // Popuplate the default SimObject icons that
        // are used by the various editors.
        EditorIconRegistry::loadFromPath( "tlab/gui/icons/class_assets/" );

        // Load up the tools resources. All the editors are initialized at this point, so
        // resources can override, redefine, or add functionality.
        Lab::LoadResources( $Lab::resourcePath );

        //$Scripts::ignoreDSOs = %toggle;

        if(isWebDemo()) {
            // if this is the web tool demo lets init some value storage
            //$clicks
        }
        Lab.pluginInitCompleted();
    }

    function startToolTime(%tool) {
        if($toolDataToolCount $= "")
            $toolDataToolCount = 0;

        if($toolDataToolEntry[%tool] !$= "true") {
            $toolDataToolEntry[%tool] = "true";
            $toolDataToolList[$toolDataToolCount] = %tool;
            $toolDataToolCount++;
            $toolDataClickCount[%tool] = 0;
        }

        $toolDataStartTime[%tool] = getSimTime();
        $toolDataClickCount[%tool]++;
    }

    function endToolTime(%tool) {
        %startTime = 0;

        if($toolDataStartTime[%tool] !$= "")
            %startTime = $toolDataStartTime[%tool];

        if($toolDataTotalTime[%tool] $= "")
            $toolDataTotalTime[%tool] = 0;

        $toolDataTotalTime[%tool] += getSimTime() - %startTime;
    }

    function dumpToolData() {
        %count = $toolDataToolCount;
        for(%i=0; %i<%count; %i++) {
            %tool = $toolDataToolList[%i];
            %totalTime = $toolDataTotalTime[%tool];
            if(%totalTime $= "")
                %totalTime = 0;
            %clickCount = $toolDataClickCount[%tool];
            echo("---");
            echo("Tool: " @ %tool);
            echo("Time (seconds): " @ %totalTime / 1000);
            echo("Activated: " @ %clickCount);
            echo("---");
        }
    }

    // Shutdown.
    function onExit() {
    
        if( LabEditor.isInitialized )
            EditorGui.shutdown();

        // Free all the icon images in the registry.
        EditorIconRegistry::clear();

        // Save any Layouts we might be using
        //GuiFormManager::SaveLayout(LevelBuilder, Default, User);

        %count = $editors[count];
        for (%i = 0; %i < %count; %i++) {
            %destroyFunction = "destroy" @ $editors[%i];
            if( isFunction( %destroyFunction ) )
                call( %destroyFunction );
        }

        // Call Parent.
        Parent::onExit();

        // write out our settings xml file
        //EditorSettings.write();
       

    }
};

function Lab::LoadResources( %path ) {
    %resourcesPath = %path @ "resources/";
    %resourcesList = getDirectoryList( %resourcesPath );

    %wordCount = getFieldCount( %resourcesList );
    for( %i = 0; %i < %wordCount; %i++ ) {
        %resource = GetField( %resourcesList, %i );
        if( isFile( %resourcesPath @ %resource @ "/resourceDatabase.cs") )
            ResourceObject::load( %path, %resource );
    }
}

//-----------------------------------------------------------------------------
// Activate Package.
//-----------------------------------------------------------------------------
activatePackage(Lab);

