//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::setEditor( %this, %newEditor, %dontActivate ) {
	//First make sure the new editor is valid
	
	
	if (%newEditor $= "") {
		%newEditor = Lab.defaultPlugin;

		if( !isObject( %newEditor )) {
			warnLog("Set editor called for invalid editor plugin:",%newEditor);
			return;
		}
	}

	// If we have a special set editor function, run that instead
	if( %newEditor.isMethod( "setEditorFunction" ) ) {
		%pluginActivated= %newEditor.setEditorFunction();

		if (!%pluginActivated) {
			warnLog("Plugin failed to activate, keeping the current plugin active");
			return;
		}
	}

	//Make sure currentEditor exist for the next checks
	if ( isObject( %this.currentEditor ) ) {
		//Desactivated current editor if activated and not same as new
		if( %this.currentEditor.getId() != %newEditor.getId() && %this.currentEditor.isActivated)
			%this.currentEditor.onDeactivated();

		//Store current OrthoFOV view to set same in new editor
		if( isObject( %this.currentEditor.editorGui ) )
			%this.orthoFOV = %this.currentEditor.editorGui.getOrthoFOV();
	}

	%this.syncEditor( %newEditor );
	%this.currentEditor = %newEditor;

	if (!%this.currentEditor.isActivated)
		%this.currentEditor.onActivated();

Lab.activePluginName = %this.currentEditor.displayName;
	//Lab.activatePlugin(%this.currentEditor);
}
function Lab::syncEditor( %this,%newEditor ) {
	if (%newEditor $= "")
		%newEditor = %this.currentEditor;

	if (!isObject(%newEditor)) {
		warnLog("Trying to sync Toolbar for invalid editor:",%newEditor);
		return;
	}

	%editorName = %newEditor.getName();

	if ($Cfg_UseCoreMenubar) {
		// Sync with menu bar
		%menu = Lab.findMenu( "Editors" );
		%count = %menu.getItemCount();

		for ( %i = 0; %i < %count; %i++ ) {
			%pluginObj = getField( %menu.item[%i], 2 );

			if ( %pluginObj $= %newEditor ) {
				%menu.checkRadioItem( 0, %count, %i );
				break;
			}
		}
	}

	%icon = ToolsToolbarArray.findObjectByInternalName(%newEditor.plugin);
	%icon.setStateOn(1); //Grouped radio button => Other will be turned off automatically
	%paletteName = strreplace(%newEditor, "Plugin", "Palette");

	if (%newEditor.customPalette !$= "") {
		%paletteName = %newEditor.customPalette;
	}

	Lab.togglePluginPalette(%paletteName);
	//EWToolsPaletteContainer.togglePalette(%paletteName);
}

