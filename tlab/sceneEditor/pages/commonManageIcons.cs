//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================


function SceneCreatorWindow::findIconCtrl( %this, %name ) {
	for ( %i = 0; %i < %this.contentCtrl.getCount(); %i++ ) {
		%ctrl = %this.contentCtrl.getObject( %i );

		if ( %ctrl.text $= %name )
			return %ctrl;
	}

	return -1;
}

function SceneCreatorWindow::createIcon( %this ) {
	%ctrl = new GuiIconButtonCtrl() {
		profile = "ToolsButtonArray";
		buttonType = "radioButton";
		groupNum = "-1";
	};

	if ( %this.isList ) {
		%ctrl.iconLocation = "Left";
		%ctrl.textLocation = "Right";
		%ctrl.extent = "348 19";
		%ctrl.textMargin = 8;
		%ctrl.buttonMargin = "2 2";
		%ctrl.autoSize = true;
	} else {
		%ctrl.iconLocation = "Center";
		%ctrl.textLocation = "Bottom";
		%ctrl.extent = "40 40";
	}

	return %ctrl;
}

function SceneCreatorWindow::addFolderIcon( %this, %text ) {
	%ctrl = %this.createIcon();
	%ctrl.altCommand = "SceneCreatorWindow.navigateDown(\"" @ %text @ "\");";
	%ctrl.iconBitmap = "tlab/gui/icons/24-assets/folder_open.png";
	%ctrl.text = %text;
	%ctrl.tooltip = %text;
	%ctrl.class = "CreatorFolderIconBtn";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this.contentCtrl.addGuiControl( %ctrl );
}

function SceneCreatorWindow::addMissionObjectIcon( %this, %class, %name, %buildfunc ) {
	%ctrl = %this.createIcon();
	// If we don't find a specific function for building an
	// object then fall back to the stock one
	%method = "build" @ %buildfunc;

	if( !ObjectBuilderGui.isMethod( %method ) )
		%method = "build" @ %class;

	if( !ObjectBuilderGui.isMethod( %method ) )
		%cmd = "return new " @ %class @ "();";
	else
		%cmd = "ObjectBuilderGui." @ %method @ "();";

	%ctrl.altCommand = "ObjectBuilderGui.newObjectCallback = \"SceneCreatorWindow.onFinishCreateObject\"; SceneCreatorWindow.createObject( \"" @ %cmd @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( %class );
	%ctrl.text = %name;
	%ctrl.class = "CreatorMissionObjectIconBtn";
	%ctrl.tooltip = %class;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this.contentCtrl.addGuiControl( %ctrl );
}

function SceneCreatorWindow::addShapeIcon( %this, %datablock ) {
	%ctrl = %this.createIcon();
	%name = %datablock.getName();
	%class = %datablock.getClassName();
	%cmd = %class @ "::create(" @ %name @ ");";
	%shapePath = ( %datablock.shapeFile !$= "" ) ? %datablock.shapeFile : %datablock.shapeName;
	%createCmd = "SceneCreatorWindow.createObject( \\\"" @ %cmd @ "\\\" );";
	%ctrl.altCommand = "ColladaImportDlg.showDialog( \"" @ %shapePath @ "\", \"" @ %createCmd @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( %class );
	%ctrl.text = %name;
	%ctrl.class = "CreatorShapeIconBtn";
	%ctrl.tooltip = %name;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this.contentCtrl.addGuiControl( %ctrl );
}

function SceneCreatorWindow::addStaticIcon( %this, %fullPath ) {
	%ctrl = %this.createIcon();
	%ext = fileExt( %fullPath );
	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%createCmd = "SceneCreatorWindow.createStatic( \\\"" @ %fullPath @ "\\\" );";
	%ctrl.altCommand = "ColladaImportDlg.showDialog( \"" @ %fullPath @ "\", \"" @ %createCmd @ "\" );";
	%ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/gui/icons/default/iconCollada" );
	%ctrl.text = %file;
	%ctrl.class = "CreatorStaticIconBtn";
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this.contentCtrl.addGuiControl( %ctrl );
}

function SceneCreatorWindow::addPrefabIcon( %this, %fullPath ) {
	%ctrl = %this.createIcon();
	%ext = fileExt( %fullPath );
	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%ctrl.altCommand = "SceneCreatorWindow.createPrefab( \"" @ %fullPath @ "\" );";
	%ctrl.iconBitmap = EditorIconRegistry::findIconByClassName( "Prefab" );
	%ctrl.text = %file;
	%ctrl.class = "CreatorPrefabIconBtn";
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this.contentCtrl.addGuiControl( %ctrl );
}