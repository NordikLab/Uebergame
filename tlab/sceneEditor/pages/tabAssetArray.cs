//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//SEP_AssetPage.setMeshFolderDepth($ThisControl);
function AssetsLab::setMeshFolderDepth( %this, %ctrl ) {
	%val = mRound(%ctrl.getValue());
	devLog("setMeshFolderDepth Val",%val,"Ctrl",%ctrl.getValue());

	if (%val !$= %ctrl.getValue())
		%ctrl.setValue(%val);

	%ctrl.updateFriends();
	%this.meshFolderDepth = %val;
}
//------------------------------------------------------------------------------

//==============================================================================
// Navigate through Asset Book Data
//AssetsLab.navigateAssets("art/models/");
function AssetsLab::navigateAssets( %this, %searchFolder ) {
	SEP_AssetPage.isList = true;
	AssetIconArray.clear();

	if (%searchFolder $= "")
		%searchFolder = "art/models/";

	%pattern = %searchFolder@"*.dae";

	//Start by loading all bind functions
	for(%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern)) {		
		%this.addStaticIcon( %file );
	}
}
function AssetsLab::createIcon( %this ) {
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
function AssetsLab::addStaticIcon( %this, %fullPath ) {
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
	AssetIconArray.addGuiControl( %ctrl );
}
//==============================================================================
// Navigate through Asset Book Data
function AssetsLab::navigate( %this, %address ) {
	AssetIconArray.frozen = true;
	AssetIconArray.clear();
	AssetPopupMenu.clear();
	%this-->meshesOptions.visible = false;

	if ( %this.tab $= "Scripted" ) {
		%category = getWord( %address, 1 );
		%dataGroup = "DataBlockGroup";

		for ( %i = 0; %i < %dataGroup.getCount(); %i++ ) {
			%obj = %dataGroup.getObject(%i);
			// echo ("Obj: " @ %obj.getName() @ " - " @ %obj.category );

			if ( %obj.category $= "" && %obj.category == 0 )
				continue;

			// Add category to popup menu if not there already
			if ( AssetPopupMenu.findText( %obj.category ) == -1 )
				AssetPopupMenu.add( %obj.category );

			if ( %address $= "" ) {
				%ctrl = %this.findIconCtrl( %obj.category );

				if ( %ctrl == -1 ) {
					%this.addFolderIcon( %obj.category );
				}
			} else if ( %address $= %obj.category ) {
				%ctrl = %this.findIconCtrl( %obj.getName() );

				if ( %ctrl == -1 )
					%this.addShapeIcon( %obj );
			}
		}
	}

	if ( %this.tab $= "Meshes" ) {
		%fullPath = findFirstFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz" TAB "*.dif" );
		SEP_AssetPage-->meshesOptions.visible = true;

		while ( %fullPath !$= "" ) {
			if (strstr(%fullPath, "cached.dts") != -1) {
				%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz"  TAB "*.dif" );
				continue;
			}

			%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
			%splitPath = strreplace( %fullPath, "/", " " );

			if( getWord(%splitPath, 0) $= "tools" ) {
				%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz"  TAB "*.dif" );
				continue;
			}

			%dirCount = getWordCount( %splitPath ) - 1;
			%pathFolders = getWords( %splitPath, 0, %dirCount - 1 );
			devLog("Adding file:",%fullPath);
			devLog("Dir count:",%dirCount,"Folders:",%pathFolders);
			%showme = false;

			if (%dirCount <= %this.meshFolderDepth)
				%showme = true;

			// Add this file's path (parent folders) to the
			// popup menu if it isn't there yet.
			%temp = strreplace( %pathFolders, " ", "/" );
			%r = AssetPopupMenu.findText( %temp );

			if ( %r == -1 ) {
				AssetPopupMenu.add( %temp );
			}

			%showAll = true;

			// Is this file in the current folder?
			if ( stricmp( %pathFolders, %address ) == 0 ) {
				%this.addStaticIcon( %fullPath );
			} else if (%showMe) {
				//Add this file to list of subdir files to be added at end
				%addFiles = strAddField(%addFiles,%fullPath);
			}
			// Then is this file in a subfolder we need to add
			// a folder icon for?
			else {
				%wordIdx = 0;
				%add = false;

				if ( %address $= "" ) {
					%add = true;
					%wordIdx = 0;
				} else {
					for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
						%temp = getWords( %splitPath, 0, %wordIdx );

						if ( stricmp( %temp, %address ) == 0 ) {
							%add = true;
							%wordIdx++;
							break;
						}
					}
				}

				if ( %add == true ) {
					%folder = getWord( %splitPath, %wordIdx );
					%ctrl = %this.findIconCtrl( %folder );

					if ( %ctrl == -1 )
						%this.addFolderIcon( %folder );
				}
			}

			%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz" TAB "*.dif" );
		}

		%extraFilesCount = getFieldCount(%addFiles);

		for(%i=0; %i<%extraFilesCount; %i++) {
			%file = getField(%addFiles,%i);
			devLog("Adding extra file:",%file);
			%this.addStaticIcon( %file );
		}
	} else if ( %this.tab $= "Meshes" ) {
		%fullPath = findFirstFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz" TAB "*.dif" );

		while ( %fullPath !$= "" ) {
			if (strstr(%fullPath, "cached.dts") != -1) {
				%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz"  TAB "*.dif" );
				continue;
			}

			%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
			%splitPath = strreplace( %fullPath, "/", " " );

			if( getWord(%splitPath, 0) $= "tools" ) {
				%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz"  TAB "*.dif" );
				continue;
			}

			%dirCount = getWordCount( %splitPath ) - 1;
			%pathFolders = getWords( %splitPath, 0, %dirCount - 1 );
			// Add this file's path (parent folders) to the
			// popup menu if it isn't there yet.
			%temp = strreplace( %pathFolders, " ", "/" );
			%r = AssetPopupMenu.findText( %temp );

			if ( %r == -1 ) {
				AssetPopupMenu.add( %temp );
			}

			// Is this file in the current folder?
			if ( stricmp( %pathFolders, %address ) == 0 ) {
				%this.addStaticIcon( %fullPath );
			}
			// Then is this file in a subfolder we need to add
			// a folder icon for?
			else {
				%wordIdx = 0;
				%add = false;

				if ( %address $= "" ) {
					%add = true;
					%wordIdx = 0;
				} else {
					for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
						%temp = getWords( %splitPath, 0, %wordIdx );

						if ( stricmp( %temp, %address ) == 0 ) {
							%add = true;
							%wordIdx++;
							break;
						}
					}
				}

				if ( %add == true ) {
					%folder = getWord( %splitPath, %wordIdx );
					%ctrl = %this.findIconCtrl( %folder );

					if ( %ctrl == -1 )
						%this.addFolderIcon( %folder );
				}
			}

			%fullPath = findNextFileMultiExpr( "*.dts" TAB "*.dae" TAB "*.kmz" TAB "*.dif" );
		}
	}

	if ( %this.tab $= "Level" ) {
		// Add groups to popup menu
		%array = %this.array;
		%array.sortk();
		%count = %array.count();

		if ( %count > 0 ) {
			%lastGroup = "";

			for ( %i = 0; %i < %count; %i++ ) {
				%group = %array.getKey( %i );

				if ( %group !$= %lastGroup ) {
					AssetPopupMenu.add( %group );

					if ( %address $= "" )
						%this.addFolderIcon( %group );
				}

				if ( %address $= %group ) {
					%args = %array.getValue( %i );
					%class = %args.val[0];
					%name = %args.val[1];
					%func = %args.val[2];
					%this.addMissionObjectIcon( %class, %name, %func );
				}

				%lastGroup = %group;
			}
		}
	}

	if ( %this.tab $= "Prefabs" ) {
		%expr = "*.prefab";
		%fullPath = findFirstFile( %expr );

		while ( %fullPath !$= "" ) {
			%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
			%splitPath = strreplace( %fullPath, "/", " " );

			if( getWord(%splitPath, 0) $= "tools" ) {
				%fullPath = findNextFile( %expr );
				continue;
			}

			%dirCount = getWordCount( %splitPath ) - 1;
			%pathFolders = getWords( %splitPath, 0, %dirCount - 1 );
			// Add this file's path (parent folders) to the
			// popup menu if it isn't there yet.
			%temp = strreplace( %pathFolders, " ", "/" );
			%r = AssetPopupMenu.findText( %temp );

			if ( %r == -1 ) {
				AssetPopupMenu.add( %temp );
			}

			// Is this file in the current folder?
			if ( stricmp( %pathFolders, %address ) == 0 ) {
				%this.addPrefabIcon( %fullPath );
			}
			// Then is this file in a subfolder we need to add
			// a folder icon for?
			else {
				%wordIdx = 0;
				%add = false;

				if ( %address $= "" ) {
					%add = true;
					%wordIdx = 0;
				} else {
					for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
						%temp = getWords( %splitPath, 0, %wordIdx );

						if ( stricmp( %temp, %address ) == 0 ) {
							%add = true;
							%wordIdx++;
							break;
						}
					}
				}

				if ( %add == true ) {
					%folder = getWord( %splitPath, %wordIdx );
					%ctrl = %this.findIconCtrl( %folder );

					if ( %ctrl == -1 )
						%this.addFolderIcon( %folder );
				}
			}

			%fullPath = findNextFile( %expr );
		}
	}

	AssetIconArray.sort( "alphaIconCompare" );

	for ( %i = 0; %i < AssetIconArray.getCount(); %i++ ) {
		AssetIconArray.getObject(%i).autoSize = false;
	}

	AssetIconArray.frozen = false;
	AssetIconArray.refresh();
	// Recalculate the array for the parent guiScrollCtrl
	AssetIconArray.getParent().computeSizes();
	%this.address = %address;
	AssetPopupMenu.sort();
	%str = strreplace( %address, " ", "/" );
	%r = AssetPopupMenu.findText( %str );

	if ( %r != -1 )
		AssetPopupMenu.setSelected( %r, false );
	else
		AssetPopupMenu.setText( %str );

	AssetPopupMenu.tooltip = %str;
}

//------------------------------------------------------------------------------
