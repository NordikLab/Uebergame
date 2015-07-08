//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function MaterialSelector::clearFilterArray( %this ) {
	%filterArray = materialSelector-->filterArray;

	foreach(%obj in %filterarray) {
		if (%obj.internalName $= "CustomFilter")
			%deleteList = strAddWord(%deleteList,%obj.getId());
	}

	devLog("Deleting list:",%deleteList);

	foreach$(%objID  in %deleteList)
		delObj(%objID);
}
//------------------------------------------------------------------------------
function MaterialSelector::buildStaticFilters( %this ) {
	/*
	// if you want to add any more containers to staticFilterObjects, here's
	// where to do it
	%staticFilterContainer = new GuiControl () {
		new GuiContainer() {
			profile = "ToolsDefaultProfile";
			Position = "0 0";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			parentGroup = %filterArray;
			new GuiContainer() {
				profile = "ToolsBoxTitleDark";
				Position = "-1 0";
				Extent = "128 32";
				HorizSizing = "right";
				VertSizing = "bottom";
				isContainer = "1";
			};
			new GuiTextCtrl() {
				Profile = "ToolsTextBase";
				position = "5 0";
				Extent = "118 18";
				text = "Types";
			};
		};
		new GuiContainer() { // All
			profile = "ToolsDefaultProfile";
			Position = "415 191";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			parentGroup = %filterArray;
			new GuiCheckBoxCtrl(MaterialFilterAllArrayCheckbox) {
				Profile = "ToolsCheckBoxProfile_List";
				position = "5 2";
				Extent = "118 18";
				text = "All";
				Command = "MaterialSelector.switchStaticFilters(\"MaterialFilterAllArray\");";
			};
		};
		new GuiContainer() { // Mapped
			profile = "ToolsDefaultProfile";
			Position = "415 191";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			parentGroup = %filterArray;
			new GuiCheckBoxCtrl(MaterialFilterMappedArrayCheckbox) {
				Profile = "ToolsCheckBoxProfile_List";
				position = "5 2";
				Extent = "118 18";
				text = "Mapped";
				Command = "MaterialSelector.switchStaticFilters(\"MaterialFilterMappedArray\");";
			};
		};
		new GuiContainer() { // Unmapped
			profile = "ToolsDefaultProfile";
			Position = "415 191";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			parentGroup = %filterArray;
			new GuiCheckBoxCtrl(MaterialFilterUnmappedArrayCheckbox) {
				Profile = "ToolsCheckBoxProfile_List";
				position = "5 2";
				Extent = "118 18";
				text = "Unmapped";
				Command = "MaterialSelector.switchStaticFilters(\"MaterialFilterUnmappedArray\");";
			};
		};
		new GuiContainer() {
			profile = "ToolsDefaultProfile";
			Position = "0 0";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			parentGroup = %filterArray;
			new GuiContainer() {
				profile = "ToolsBoxTitleDark";
				Position = "-1 0";
				Extent = "128 32";
				HorizSizing = "right";
				VertSizing = "bottom";
				isContainer = "1";
			};
			new GuiTextCtrl() {
				Profile = "ToolsTextBase";
				position = "5 0";
				Extent = "118 18";
				text = "Tags";
			};
			// Create New Tag
			new GuiBitmapButtonCtrl() {
				canSaveDynamicFields = "0";
				Enabled = "1";
				isContainer = "0";
				Profile = "ToolsDefaultProfile";
				HorizSizing = "left";
				VertSizing = "bottom";
				position = "105 2";
				Extent = "15 15";
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				Command = "MaterialSelector_addFilterWindow.setVisible(1); MaterialSelectorOverlay.pushToBack(MaterialSelector_addFilterWindow);";
				hovertime = "1000";
				tooltip = "Create New Tag";
				bitmap = "tlab/gui/icons/default/new";
				groupNum = "-1";
				buttonType = "PushButton";
				useMouseEvents = "0";
			};
			new GuiBitmapButtonCtrl() {
				canSaveDynamicFields = "0";
				Enabled = "1";
				isContainer = "0";
				Profile = "ToolsDefaultProfile";
				HorizSizing = "left";
				VertSizing = "bottom";
				position = "89 2";
				Extent = "13 13";
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				Command = "MaterialSelector.clearMaterialFilters();";
				hovertime = "1000";
				tooltip = "Clear Selected Tag";
				bitmap = "tlab/gui/icons/default/clear-btn";
				groupNum = "-1";
				buttonType = "PushButton";
				useMouseEvents = "0";
			};
		};
	};

	*/
//	%i = %staticFilterContainer.getCount();
	//for( ; %i != 0; %i--)
	//MaterialSelector-->filterArray.addGuiControl(%staticFilterContainer.getObject(0));
	%this.clearFilterArray();
	MaterialSelector.staticFilterObjects = MaterialSelector-->filterArray.getCount();
	//%staticFilterContainer.delete();
	// Create our category array used in the selector, this code should be taken out
	// in order to make the material selector agnostic
	new ArrayObject(MaterialFilterAllArray);
	new ArrayObject(MaterialFilterMappedArray);
	new ArrayObject(MaterialFilterUnmappedArray);
	%mats = "";
	%count = 0;

	if( MaterialSelector.terrainMaterials ) {
		%mats = ETerrainEditor.getTerrainBlocksMaterialList();
		%count = getRecordCount( %mats );
	} else {
		%count = materialSet.getCount();
	}

	for(%i = 0; %i < %count; %i++) {
		// Process terrain materials
		if( MaterialSelector.terrainMaterials ) {
			%matInternalName = getRecord( %mats, %i );
			%material = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

			// Is there no material info for this slot?
			if ( !isObject( %material ) )
				continue;

			// Add to the appropriate filters
			MaterialFilterMappedArray.add( "", %material );
			MaterialFilterAllArray.add( "", %material );
			continue;
		}

		// Process regular materials here
		%material = materialSet.getObject(%i);

		for( %k = 0; %k < UnlistedMaterials.count(); %k++ ) {
			%unlistedFound = 0;

			if( UnlistedMaterials.getValue(%k) $= %material.name ) {
				%unlistedFound = 1;
				break;
			}
		}

		if( %unlistedFound )
			continue;

		if( %material.mapTo $= "" || %material.mapTo $= "unmapped_mat" ) {
			MaterialFilterUnmappedArray.add( "", %material.name );

			//running through the existing tag names
			for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
				MaterialFilterUnmappedArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
		} else {
			MaterialFilterMappedArray.add( "", %material.name );

			for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
				MaterialFilterMappedArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
		}

		MaterialFilterAllArray.add( "", %material.name );

		for( %j = 0; %material.getFieldValue("materialTag" @ %j) !$= ""; %j++ )
			MaterialFilterAllArray.add( %material.getFieldValue("materialTag" @ %j), %material.name );
	}

	MaterialFilterAllArrayCheckbox.setText("All ( " @ MaterialFilterAllArray.count() @" ) ");
	MaterialFilterMappedArrayCheckbox.setText("Mapped ( " @ MaterialFilterMappedArray.count() @" ) ");
	MaterialFilterUnmappedArrayCheckbox.setText("Unmapped ( " @ MaterialFilterUnmappedArray.count() @" ) ");
}
//------------------------------------------------------------------------------
function MaterialSelector::preloadFilter( %this ) {
	%selectedFilter = "";

	for( %i = MaterialSelector.staticFilterObjects; %i < MaterialSelector-->filterArray.getCount(); %i++ ) {
		if( MaterialSelector-->filterArray.getObject(%i).getObject(0).getValue() == 1 ) {
			if( %selectedFilter $= "" )
				%selectedFilter = MaterialSelector-->filterArray.getObject(%i).getObject(0).filter;
			else
				%selectedFilter = %selectedFilter @ " " @ MaterialSelector-->filterArray.getObject(%i).getObject(0).filter;
		}
	}

	MaterialSelector.loadFilter( %selectedFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the filtered materials (called also when thumbnail count change)
function MaterialSelector::loadFilter( %this, %selectedFilter, %staticFilter ) {
	// manage schedule array properly
	if(!isObject(MatEdScheduleArray))
		new ArrayObject(MatEdScheduleArray);

	// if we select another list... delete all schedules that were created by
	// previous load
	for( %i = 0; %i < MatEdScheduleArray.count(); %i++ )
		cancel(MatEdScheduleArray.getKey(%i));

	// we have to empty out the list; so when we create new schedules, these dont linger
	MatEdScheduleArray.empty();

	// manage preview array
	if(!isObject(MatEdPreviewArray))
		new ArrayObject(MatEdPreviewArray);

	// we have to empty out the list; so when we create new guicontrols, these dont linger
	MatEdPreviewArray.empty();
	MaterialSelector-->materialSelection.deleteAllObjects();
	MaterialSelector-->materialPreviewPagesStack.deleteAllObjects();

	// changed to accomadate tagging. dig through the array for each tag name,
	// call unique value, sort, and we have a perfect set of materials
	if( %staticFilter !$= "" )
		MaterialSelector.currentStaticFilter = %staticFilter;

	MaterialSelector.currentFilter = %selectedFilter;
	%filteredObjectsArray = new ArrayObject();
	%previewsPerPage = MaterialSelector-->materialPreviewCountPopup.getTextById( MaterialSelector-->materialPreviewCountPopup.getSelected() );

	if (%previewsPerPage $= "All")
		%previewsPerPage = "999999";

	%tagCount = getWordCount( MaterialSelector.currentFilter );

	if( %tagCount != 0 ) {
		for( %j = 0; %j < %tagCount; %j++ ) {
			for( %i = 0; %i < MaterialSelector.currentStaticFilter.count(); %i++ ) {
				%currentTag = getWord( MaterialSelector.currentFilter, %j );

				if( MaterialSelector.currentStaticFilter.getKey(%i) $= %currentTag)
					%filteredObjectsArray.add( MaterialSelector.currentStaticFilter.getKey(%i), MaterialSelector.currentStaticFilter.getValue(%i) );
			}
		}

		%filteredObjectsArray.uniqueValue();
		%filteredObjectsArray.sortd();
		MaterialSelector.totalPages = mCeil( %filteredObjectsArray.count() / %previewsPerPage );

		//Can we maintain the current preview page, or should we go to page 1?
		if( (MaterialSelector.currentPreviewPage * %previewsPerPage) >= %filteredObjectsArray.count() )
			MaterialSelector.currentPreviewPage = 0;

		// Build out the pages buttons
		MaterialSelector.buildPagesButtons( MaterialSelector.currentPreviewPage, MaterialSelector.totalPages );
		%previewCount = %previewsPerPage;
		%possiblePreviewCount = %filteredObjectsArray.count() - MaterialSelector.currentPreviewPage * %previewsPerPage;

		if( %possiblePreviewCount < %previewCount )
			%previewCount = %possiblePreviewCount;

		%start = MaterialSelector.currentPreviewPage * %previewsPerPage;

		for( %i = %start; %i < %start + %previewCount; %i++ )
			MaterialSelector.buildPreviewArray( %filteredObjectsArray.getValue(%i) );

		%filteredObjectsArray.delete();
	} else {
		MaterialSelector.currentStaticFilter.sortd();
		// Rebuild the static filter list without tagged materials
		%noTagArray = new ArrayObject();

		for( %i = 0; %i < MaterialSelector.currentStaticFilter.count(); %i++ ) {
			if( MaterialSelector.currentStaticFilter.getKey(%i) !$= "")
				continue;

			%material = MaterialSelector.currentStaticFilter.getValue(%i);

			// CustomMaterials are not available for selection
			if ( !isObject( %material ) || %material.isMemberOfClass( "CustomMaterial" ) )
				continue;

			%noTagArray.add( "", %material );
		}

		MaterialSelector.totalPages = mCeil( %noTagArray.count() / %previewsPerPage );

		//Can we maintain the current preview page, or should we go to page 1?
		if( (MaterialSelector.currentPreviewPage * %previewsPerPage) >= %noTagArray.count() )
			MaterialSelector.currentPreviewPage = 0;

		// Build out the pages buttons
		MaterialSelector.buildPagesButtons( MaterialSelector.currentPreviewPage, MaterialSelector.totalPages );
		%previewCount = %previewsPerPage;
		%possiblePreviewCount = %noTagArray.count() - MaterialSelector.currentPreviewPage * %previewsPerPage;

		if( %possiblePreviewCount < %previewCount )
			%previewCount = %possiblePreviewCount;

		%start = MaterialSelector.currentPreviewPage * %previewsPerPage;

		for( %i = %start; %i < %start + %previewCount; %i++ ) {
			MaterialSelector.buildPreviewArray( %noTagArray.getValue(%i) );
		}
	}

	MaterialSelector.loadImages( 0 );
}

//------------------------------------------------------------------------------
function MaterialSelector::clearMaterialFilters( %this ) {
	for( %i = MaterialSelector.staticFilterObjects; %i < MaterialSelector-->filterArray.getCount(); %i++ )
		MaterialSelector-->filterArray.getObject(%i).getObject(0).setStateOn(0);

	MaterialSelector.loadFilter( "", "" );
}
//------------------------------------------------------------------------------
function MaterialSelector::loadMaterialFilters( %this ) {
	%filteredTypesArray = new ArrayObject();
	%filteredTypesArray.duplicate( MaterialFilterAllArray );
	%filteredTypesArray.uniqueKey();
	// sort the the keys before we do anything
	%filteredTypesArray.sortkd();
	eval( MaterialSelector.currentStaticFilter @ "Checkbox.setStateOn(1);" );
	// it may seem goofy why the checkbox can't be instanciated inside the container
	// reason being its because we need to store the checkbox ctrl in order to make changes
	// on it later in the function.
	%selectedFilter = "";

	for( %i = 0; %i < %filteredTypesArray.count(); %i++ ) {
		%filter = %filteredTypesArray.getKey(%i);

		if(%filter $= "")
			continue;

		%container = cloneObject(MatSelector_FilterSamples-->filterPillSample);
		%container.internalName = "CustomFilter";
		%checkbox = %container.getObject(0);
		%checkbox.text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
		%checkbox.filter = %filter;

		if (!isObject(%container)) {
			%container = new GuiControl() {
				profile = "ToolsDefaultProfile";
				Position = "0 0";
				Extent = "128 18";
				HorizSizing = "right";
				VertSizing = "bottom";
				isContainer = "1";
			};
			%checkbox = new GuiCheckBoxCtrl() {
				Profile = "ToolsCheckBoxProfile_List";
				position = "5 1";
				Extent = "118 18";
				Command = "";
				groupNum = "0";
				buttonType = "ToggleButton";
				text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
				filter = %filter;
				Command = "MaterialSelector.preloadFilter();";
			};
			%container.add( %checkbox );
		}

		//%checkbox = %container.getObject(0);
		MaterialSelector-->filterArray.add( %container );
		%tagCount = getWordCount( MaterialSelector.currentFilter );

		for( %j = 0; %j < %tagCount; %j++ ) {
			if( %filter $= getWord( MaterialSelector.currentFilter, %j ) ) {
				if( %selectedFilter $= "" )
					%selectedFilter = %filter;
				else
					%selectedFilter = %selectedFilter @ " " @ %filter;

				%checkbox.setStateOn(1);
			}
		}
	}

	MaterialSelector.loadFilter( %selectedFilter );
	%filteredTypesArray.delete();
}
//------------------------------------------------------------------------------
// create category and update current material if there is one
function MaterialSelector::createFilter( %this, %filter ) {
	if( %filter $= %existingFilters ) {
		MessageBoxOK( "Error", "Can not create blank filter.");
		return;
	}

	for( %i = MaterialSelector.staticFilterObjects; %i < MaterialSelector-->filterArray.getCount() ; %i++ ) {
		%existingFilters = MaterialSelector-->filterArray.getObject(%i).getObject(0).filter;

		if( %filter $= %existingFilters ) {
			MessageBoxOK( "Error", "Can not create two filters of the same name.");
			return;
		}
	}

	devLog("createFilter:",%filter);
	%container = cloneObject(MatSelector_FilterSamples-->filterPillSample);
	%container.internalName = "CustomFilter";
	%checkbox = %container.getObject(0);
	%checkbox.text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
	%checkbox.filter = %filter;

	if (!isObject(%container)) {
		%container = new GuiControl() {
			profile = "ToolsDefaultProfile";
			Position = "0 0";
			Extent = "128 18";
			HorizSizing = "right";
			VertSizing = "bottom";
			isContainer = "1";
			new GuiCheckBoxCtrl() {
				Profile = "ToolsCheckBoxProfile_List";
				position = "5 1";
				Extent = "118 18";
				Command = "";
				groupNum = "0";
				buttonType = "ToggleButton";
				text = %filter @ " ( " @ MaterialFilterAllArray.countKey(%filter) @ " )";
				filter = %filter;
				Command = "MaterialSelector.preloadFilter();";
			};
		};
	}

	MaterialSelector-->filterArray.add( %container );

	// if selection exists, lets reselect it to refresh it
	if( isObject(MaterialSelector.selectedMaterial) )
		MaterialSelector.updateSelection( MaterialSelector.selectedMaterial, MaterialSelector.selectedPreviewImagePath );

	// material category text field to blank
	MaterialSelector_addFilterWindow-->tagName.setText("");
}


//------------------------------------------------------------------------------
function MaterialSelector::updateFilterCount( %this, %tag, %add ) {
	for( %i = MaterialSelector.staticFilterObjects; %i < MaterialSelector-->filterArray.getCount() ; %i++ ) {
		if( %tag $= MaterialSelector-->filterArray.getObject(%i).getObject(0).filter ) {
			// Get the filter count and apply the operation
			%idx = getWord( MaterialSelector-->filterArray.getObject(%i).getObject(0).getText(), 2 );

			if( %add )
				%idx++;
			else
				%idx--;

			MaterialSelector-->filterArray.getObject(%i).getObject(0).setText( %tag @ " ( "@ %idx @ " )");
		}
	}
}

//------------------------------------------------------------------------------
function MaterialSelector::switchStaticFilters( %this, %staticFilter) {
	switch$(%staticFilter) {
	case "MaterialFilterAllArray":
		MaterialFilterAllArrayCheckbox.setStateOn(1);
		MaterialFilterMappedArrayCheckbox.setStateOn(0);
		MaterialFilterUnmappedArrayCheckbox.setStateOn(0);

	case "MaterialFilterMappedArray":
		MaterialFilterMappedArrayCheckbox.setStateOn(1);
		MaterialFilterAllArrayCheckbox.setStateOn(0);
		MaterialFilterUnmappedArrayCheckbox.setStateOn(0);

	case "MaterialFilterUnmappedArray":
		MaterialFilterUnmappedArrayCheckbox.setStateOn(1);
		MaterialFilterAllArrayCheckbox.setStateOn(0);
		MaterialFilterMappedArrayCheckbox.setStateOn(0);
	}

	// kinda goofy were passing a class variable... we can't do an empty check right now
	// on load filter because we actually pass "" as a filter...
	MaterialSelector.loadFilter( MaterialSelector.currentFilter, %staticFilter );
}