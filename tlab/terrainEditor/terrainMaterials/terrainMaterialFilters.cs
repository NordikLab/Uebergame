//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//"art/terrains/*.cs";
$TerrainMaterialFolders="art/textures/FarmPack/Terrains/" TAB "art/";

//==============================================================================
function TerrainMaterialDlg::initFiltersData( %this ) {
	%folderMenu = %this-->menuFolders;
	%folderMenu.clear();
	%folderMenu.add("All",0);
	%menuId = 1;

	for(%j=0; %j<getFieldCount($TerrainMaterialFolders); %j++) {
		%pathBase = getField($TerrainMaterialFolders,%j);
		
		%filePathScript = %pathBase@"*.cs";

		for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
		
			//get folder
			%folder = filePath(%file);
			%folderStr = strreplace(%folder,"/","\t");
			%folderCount = getFieldCount(%folderStr);
			%themeId = "";

			for(%i = 0; %i<%folderCount; %i++) {
				%currentFolder = getField(%folderStr,%i);

				if (%currentFolder $= "terrains") {
					%themeId = %i+1;
					%themeFolder = getField(%folderStr,%themeId);
				}

				if (!%added[%themeFolder] && %themeFolder !$= "") {
					%folderMenu.add( %themeFolder,%menuId);
					%added[%themeFolder] = true;
					%menuId++;
				}
			}
		}
	}

	%folderMenu.setSelected(0,false);
	%surfaceMenu = %this-->menuSurfaces;
	%surfaceMenu.clear();
	%surfaceMenu.add("All",0);
	%surfaceMenu.add("Grass",1);
	%surfaceMenu.add("Ground",2);
	%surfaceMenu.add("Rock",3);
	%surfaceMenu.setSelected(0,false);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::changeFolderFilter( %this ) {
	%folderMenu = %this-->menuFolders;
	%id = %folderMenu.getSelected();
	%folder = %folderMenu.getTextById(%id);
	TerrainMaterialDlg.folderFilter = %folder;
	%this.setFilteredMaterialsSet();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeSurfaceFilter( %this ) {
	%surfaceMenu = %this-->menuSurfaces;
	%id = %surfaceMenu.getSelected();
	%surface = %surfaceMenu.getTextById(%id);
	TerrainMaterialDlg.surfaceFilter = %surface;
	%this.setFilteredMaterialsSet();
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::applyMaterialFilters( %this ) {
	%matLibTree = %this-->matLibTree;
	%matLibTree.clear();
	%matLibTree.open( TerrainMaterialSet, false );
	%currentSelection = %matLibTree.getSelectedItemList();
	%matLibTree.clearSelection();
	%count = %matLibTree.getItemCount();
	%hideMe = false;

	foreach(%mat in TerrainMaterialSet) {
		%item = %matLibTree.findItemByObjectId(%mat.getId());

		if (%this.folderFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%this.folderFilter);

			if (%folderFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (%this.surfaceFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%this.surfaceFilter);
			%nameFound = strstr(%mat.getName(),%this.surfaceFilter);
			%intnameFound = strstr(%mat.internalName,%this.surfaceFilter);

			if (%folderFound $= "-1" && %nameFound $= "-1" && %intnameFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (!$Cfg_MaterialEditor_ShowMaterialClones) {
			%len = strlen(%mat.getName());
			%twoLast = getSubStr(%mat.getName(),%len-2);

			if(%twoLast $= "_1" || %twoLast $= "_2" || %twoLast $= "_3" || %twoLast $= "_4") {
				%hideMe = true;
			}
		}

		if (%hideMe) {
			%matLibTree.removeItem(%item);
			%matLibTree.hideSelection();
			%matLibTree.clearSelection();
		}
	}

	%matLibTree.clearSelection();

	foreach$(%itemId in %currentSelection)
		%matLibTree.selectItem(%itemId);

	%matLibTree.setFilterText(%this-->materialFilter.getValue());
	%matLibTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::setFilteredMaterialsSet( %this,%reset ) {
	FilteredTerrainMaterialsSet.clear();

	if (%reset) {
		%this.folderFilter = "All";
		%this.surfaceFilter = "All";
	}

	%folderFilter = %this.folderFilter;
	%surfaceFilter = %this.surfaceFilter;

	foreach(%mat in TerrainMaterialSet) {
		%hideMe = false;

		if (%this.folderFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%folderFilter);

			if (%folderFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (%this.surfaceFilter !$= "All") {
			%folderFound = strstr(%mat.getFilename(),%surfaceFilter);
			%nameFound = strstr(%mat.getName(),%surfaceFilter);
			%intnameFound = strstr(%mat.internalName,%surfaceFilter);

			if (%folderFound $= "-1" && %nameFound $= "-1" && %intnameFound $= "-1") {
				//Hide this material
				%hideMe = true;
			}
		}

		if (!$Cfg_MaterialEditor_ShowMaterialClones) {
			%len = strlen(%mat.getName());
			%twoLast = getSubStr(%mat.getName(),%len-2);

			if(%twoLast $= "_1" || %twoLast $= "_2" || %twoLast $= "_3" || %twoLast $= "_4") {
				%hideMe = true;
			}
		}

		if (!%hideMe) {
			FilteredTerrainMaterialsSet.add(%mat);
		}
	}

	%this.refreshMaterialTree();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::updateFilterText( %this ) {
	%matLibTree = %this-->matLibTree;
	%matLibTree.setFilterText(%this-->materialFilter.getValue());
	%this.refreshMaterialTree();
}