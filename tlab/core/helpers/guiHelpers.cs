//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabCfg_ContextMenuProfile = "ToolsDropdownProfile";
//==============================================================================
// Context Menu Helpers
//==============================================================================
function Lab::createContextMenu(%this,%itemRecords,%show) {
	devLog("Lab::createContextMenu(%this,%itemRecords,%show,%menu)",%this,%itemRecords,%show,%menu);
	if (!isObject(%menu)){
	%menu = new PopupMenu() {
		superClass = "ContextMenu";
		isPopup = true;		
		
		object = -1;
		profile = $LabCfg_ContextMenuProfile;
	};
	}
	
	return %menu;
}
//------------------------------------------------------------------------------
// Adds one item to the menu.
// if %item is skipped or "", we will use %item[#], which was set when the menu was created.
// if %item is provided, then we update %item[#].
function ContextMenu::addItem(%this, %pos, %item) {
	if(%item $= "")
		%item = %this.item[%pos];

	if(%item !$= %this.item[%pos])
		%this.item[%pos] = %item;

	%name = getField(%item, 0);
	%accel = getField(%item, 1);
	%cmd = getField(%item, 2);
	// We replace the [this] token with our object ID
	%cmd = strreplace( %cmd, "[this]", %this );
	%this.item[%pos] = setField( %item, 2, %cmd );

	if(isObject(%accel)) {
		// If %accel is an object, we want to add a sub menu
		%this.insertSubmenu(%pos, %name, %accel);
	} else {
		%this.insertItem(%pos, %name !$= "-" ? %name : "", %accel);
	}
}

function ContextMenu::appendItem(%this, %item) {
	%this.addItem(%this.getItemCount(), %item);
}

function ContextMenu::onSelectItem(%this, %id, %text) {
	devLog("ContextMenu::onSelectItem(%this,%id,%text) ",%this,%id,%text);
	%cmd = getField(%this.item[%id], 2);
	devLog("ContextMenu::onSelectItem Command ",%cmd);
	if(%cmd !$= "") {
		eval( %cmd );
		return true;
	}

	return false;
}

//- Sets a new name on an existing menu item.
function ContextMenu::setItemName( %this, %id, %name ) {
	%item = %this.item[%id];
	%accel = getField(%item, 1);
	%this.setItem( %id, %name, %accel );
}

//- Sets a new command on an existing menu item.
function ContextMenu::setItemCommand( %this, %id, %command ) {
	%this.item[%id] = setField( %this.item[%id], 2, %command );
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
function Lab::setGuisToDefault() {
	setGuiParentLabPhysic(false);
	GuiEditorContentList.init();
}

function Lab::setGuisToDefault() {
	setGuiParentLabPhysic(false);
	GuiEditorContentList.init();
}

function Lab::setGuisToEditor() {
	setGuiParentLabPhysic(true);
	GuiEditorContentList.init();
}


function Lab::setGuisToEditor() {
	$pref::GuiEditor::ShowEditorsGui = !$pref::GuiEditor::ShowEditorsGui;
	GuiEditorContentList.init();
}



function Lab::savePluginGui(%this,%plugin) {
	//Assemble the GUI
	%groupCtrl  = %plugin@"_GuiSet";

	foreach(%gui in %groupCtrl) {
		%parent = %gui.defaultParent;

		if (!isObject(%parent)) continue;

		%parent.add(%gui);
	}

	if (!isObject(%parent)) {
		warnLog("Invalid Parent GUI to save:",%parent);
		return;
	}

	%currentFile = %parent.getFileName();

	if( isWriteableFileName( %filename ) ) {
		//
		// Extract any existent TorqueScript before writing out to disk
		//
		%fileObject = new FileObject();
		%fileObject.openForRead( %filename );
		%skipLines = true;
		%beforeObject = true;
		// %var++ does not post-increment %var, in torquescript, it pre-increments it,
		// because ++%var is illegal.
		%lines = -1;
		%beforeLines = -1;
		%skipLines = false;

		while( !%fileObject.isEOF() ) {
			%line = %fileObject.readLine();

			if( %line $= "//--- OBJECT WRITE BEGIN ---" )
				%skipLines = true;
			else if( %line $= "//--- OBJECT WRITE END ---" ) {
				%skipLines = false;
				%beforeObject = false;
			} else if( %skipLines == false ) {
				if(%beforeObject)
					%beforeNewFileLines[ %beforeLines++ ] = %line;
				else
					%newFileLines[ %lines++ ] = %line;
			}
		}

		%fileObject.close();
		%fileObject.delete();
		%fo = new FileObject();
		%fo.openForWrite(%filename);

		// Write out the captured TorqueScript that was before the object before the object
		for( %i = 0; %i <= %beforeLines; %i++)
			%fo.writeLine( %beforeNewFileLines[ %i ] );

		%fo.writeLine("//--- OBJECT WRITE BEGIN ---");
		%fo.writeObject(%parent, "%guiContent = ");
		%fo.writeLine("//--- OBJECT WRITE END ---");

		// Write out captured TorqueScript below Gui object
		for( %i = 0; %i <= %lines; %i++ )
			%fo.writeLine( %newFileLines[ %i ] );

		%fo.close();
		%fo.delete();
		%currentObject.setFileName( makeRelativePath( %filename, getMainDotCsDir() ) );
		GuiEditorStatusBar.print( "Saved file '" @ %parent.getFileName() @ "'" );
	} else
		LabMsgOk( "Error writing to file", "There was an error writing to file '" @ %currentFile @ "'. The file may be read-only." );
}
