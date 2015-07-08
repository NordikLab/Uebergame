//==============================================================================
// TorqueLab -> ShapeEditor -> Detail/Mesh Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Detail/Mesh Editing
//==============================================================================



function ShapeEdDetails::onWake( %this ) {
	// Initialise popup menus
	%this-->bbType.clear();
	%this-->bbType.add( "None", 0 );
	%this-->bbType.add( "Billboard", 1 );
	%this-->bbType.add( "Z Billboard", 2 );
	%this-->addGeomTo.clear();
	%this-->addGeomTo.add( "current detail", 0 );
	%this-->addGeomTo.add( "new detail", 1 );
	%this-->addGeomTo.setSelected( 0, false );
	ShapeEdDetailTree.onDefineIcons();
}

function ShapeEdDetailTree::onDefineIcons(%this) {
	// Set the tree view icon indices and texture paths
	%this._imageNone = 0;
	%this._imageHidden = 1;
	%icons = ":" @                                        // no icon
				"tlab/gui/icons/default/visible_i:";               // hidden
	%this.buildIconTable( %icons );
}

// Return true if the item in the details tree view is a detail level (false if
// a mesh)
function ShapeEdDetailTree::isDetailItem( %this, %id ) {
	return ( %this.getParent( %id ) == 1 );
}

// Get the detail level index from the ID of an item in the details tree view
function ShapeEdDetailTree::getDetailLevelFromItem( %this, %id ) {
	if ( %this.isDetailItem( %id ) )
		%detSize = %this.getItemValue( %id );
	else
		%detSize = %this.getItemValue( %this.getParent( %id ) );

	return ShapeEditor.shape.getDetailLevelIndex( %detSize );
}

function ShapeEdDetailTree::addMeshEntry( %this, %name, %noSync ) {
	// Add new detail level if required
	%size = getTrailingNumber( %name );
	%detailID = %this.findItemByValue( %size );

	if ( %detailID <= 0 ) {
		%dl = ShapeEditor.shape.getDetailLevelIndex( %size );
		%detName = ShapeEditor.shape.getDetailLevelName( %dl );
		%detailID = ShapeEdDetailTree.insertItem( 1, %detName, %size, "" );

		// Sort details by decreasing size
		for ( %sibling = ShapeEdDetailTree.getPrevSibling( %detailID );
				( %sibling > 0 ) && ( ShapeEdDetailTree.getItemValue( %sibling ) < %size );
				%sibling = ShapeEdDetailTree.getPrevSibling( %detailID ) )
			ShapeEdDetailTree.moveItemUp( %detailID );

		if ( !%noSync )
			ShapeEdDetails.update_onDetailsChanged();
	}

	return %this.insertItem( %detailID, %name, "", "" );
}

function ShapeEdDetailTree::removeMeshEntry( %this, %name, %size ) {
	%size = getTrailingNumber( %name );
	%id = ShapeEdDetailTree.findItemByName( %name );

	if ( ShapeEditor.shape.getDetailLevelIndex( %size ) < 0 ) {
		// Last mesh of a detail level has been removed => remove the detail level
		%this.removeItem( %this.getParent( %id ) );
		ShapeEdDetails.update_onDetailsChanged();
	} else
		%this.removeItem( %id );
}

function ShapeEdAdvancedWindow::update_onShapeSelectionChanged( %this ) {
	ShapeEdShapeView.currentDL = 0;
	ShapeEdShapeView.onDetailChanged();
}

function ShapeEdPropWindow::update_onDetailRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename detail entry
	%id = ShapeEdDetailTree.findItemByName( %oldName );

	if ( %id > 0 ) {
		%size = ShapeEdDetailTree.getItemValue( %id );
		ShapeEdDetailTree.editItem( %id, %newName, %size );

		// Sync text if item is selected
		if ( ShapeEdDetailTree.isItemSelected( %id ) &&
				( ShapeEdDetails-->meshName.getText() !$= %newName ) )
			ShapeEdDetails-->meshName.setText( stripTrailingNumber( %newName ) );
	}
}

function ShapeEdPropWindow::update_onDetailSizeChanged( %this, %oldSize, %newSize ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	%dl = ShapeEditor.shape.getDetailLevelIndex( %newSize );

	if ( ShapeEdAdvancedWindow-->detailSize.getText() $= %oldSize ) {
		ShapeEdShapeView.currentDL = %dl;
		ShapeEdAdvancedWindow-->detailSize.setText( %newSize );
		ShapeEdDetails-->meshSize.setText( %newSize );
	}

	// --- DETAILS TAB ---
	// Update detail entry then resort details by size
	%id = ShapeEdDetailTree.findItemByValue( %oldSize );
	%detName = ShapeEditor.shape.getDetailLevelName( %dl );
	ShapeEdDetailTree.editItem( %id, %detName, %newSize );

	for ( %sibling = ShapeEdDetailTree.getPrevSibling( %id );
			( %sibling > 0 ) && ( ShapeEdDetailTree.getItemValue( %sibling ) < %newSize );
			%sibling = ShapeEdDetailTree.getPrevSibling( %id ) )
		ShapeEdDetailTree.moveItemUp( %id );

	for ( %sibling = ShapeEdDetailTree.getNextSibling( %id );
			( %sibling > 0 ) && ( ShapeEdDetailTree.getItemValue( %sibling ) > %newSize );
			%sibling = ShapeEdDetailTree.getNextSibling( %id ) )
		ShapeEdDetailTree.moveItemDown( %id );

	// Update size values for meshes of this detail
	for ( %child = ShapeEdDetailTree.getChild( %id );
			%child > 0;
			%child = ShapeEdDetailTree.getNextSibling( %child ) ) {
		%meshName = stripTrailingNumber( ShapeEdDetailTree.getItemText( %child ) );
		ShapeEdDetailTree.editItem( %child, %meshName SPC %newSize, "" );
	}
}

function ShapeEdDetails::update_onDetailsChanged( %this ) {
	%detailCount = ShapeEditor.shape.getDetailLevelCount();
	ShapeEdAdvancedWindow-->detailSlider.range = "0" SPC ( %detailCount-1 );

	if ( %detailCount >= 2 )
		ShapeEdAdvancedWindow-->detailSlider.ticks = %detailCount - 2;
	else
		ShapeEdAdvancedWindow-->detailSlider.ticks = 0;

	// Initialise imposter settings
	ShapeEdAdvancedWindow-->bbUseImposters.setValue( ShapeEditor.shape.getImposterDetailLevel() != -1 );

	// Update detail parameters
	if ( ShapeEdShapeView.currentDL < %detailCount ) {
		%settings = ShapeEditor.shape.getImposterSettings( ShapeEdShapeView.currentDL );
		%isImposter = getWord( %settings, 0 );
		ShapeEdAdvancedWindow-->imposterInactive.setVisible( !%isImposter );
		ShapeEdAdvancedWindow-->bbEquatorSteps.setText( getField( %settings, 1 ) );
		ShapeEdAdvancedWindow-->bbPolarSteps.setText( getField( %settings, 2 ) );
		ShapeEdAdvancedWindow-->bbDetailLevel.setText( getField( %settings, 3 ) );
		ShapeEdAdvancedWindow-->bbDimension.setText( getField( %settings, 4 ) );
		ShapeEdAdvancedWindow-->bbIncludePoles.setValue( getField( %settings, 5 ) );
		ShapeEdAdvancedWindow-->bbPolarAngle.setText( getField( %settings, 6 ) );
	}
}

function ShapeEdPropWindow::update_onObjectNodeChanged( %this, %objName ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();

	// --- DETAILS TAB ---
	// Update the node popup menu if this object is selected
	if ( ShapeEdDetails-->meshName.getText() $= %objName ) {
		%nodeName = ShapeEditor.shape.getObjectNode( %objName );

		if ( %nodeName $= "" )
			%nodeName = "<root>";

		%id = ShapeEdDetails-->objectNode.findText( %nodeName );
		ShapeEdDetails-->objectNode.setSelected( %id, false );
	}
}

function ShapeEdPropWindow::update_onObjectRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename tree entries for this object
	%count = ShapeEditor.shape.getMeshCount( %newName );

	for ( %i = 0; %i < %count; %i++ ) {
		%size = getTrailingNumber( ShapeEditor.shape.getMeshName( %newName, %i ) );
		%id = ShapeEdDetailTree.findItemByName( %oldName SPC %size );

		if ( %id > 0 ) {
			ShapeEdDetailTree.editItem( %id, %newName SPC %size, "" );

			// Sync text if item is selected
			if ( ShapeEdDetailTree.isItemSelected( %id ) &&
					( ShapeEdDetails-->meshName.getText() !$= %newName ) )
				ShapeEdDetails-->meshName.setText( %newName );
		}
	}
}

function ShapeEdPropWindow::update_onMeshAdded( %this, %meshName ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	ShapeEdShapeView.updateNodeTransforms();

	// --- COLLISION WINDOW ---
	// Add object to target list if it does not already exist
	if ( !ShapeEditor.isCollisionMesh( %meshName ) ) {
		%objName = stripTrailingNumber( %meshName );
		%id = ShapeEdColWindow-->colTarget.findText( %objName );

		if ( %id == -1 )
			ShapeEdColWindow-->colTarget.add( %objName );
	}

	// --- DETAILS TAB ---
	%id = ShapeEdDetailTree.addMeshEntry( %meshName );
	ShapeEdDetailTree.clearSelection();
	ShapeEdDetailTree.selectItem( %id );
}

function ShapeEdPropWindow::update_onMeshSizeChanged( %this, %meshName, %oldSize, %newSize ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	// --- DETAILS TAB ---
	// Move the mesh to the new location in the tree
	%selected = ShapeEdDetailTree.getSelectedItem();
	%id = ShapeEdDetailTree.findItemByName( %meshName SPC %oldSize );
	ShapeEdDetailTree.removeMeshEntry( %meshName SPC %oldSize );
	%newId = ShapeEdDetailTree.addMeshEntry( %meshName SPC %newSize );

	// Re-select the new entry if it was selected
	if ( %selected == %id ) {
		ShapeEdDetailTree.clearSelection();
		ShapeEdDetailTree.selectItem( %newId );
	}
}

function ShapeEdPropWindow::update_onMeshRemoved( %this, %meshName ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	// --- COLLISION WINDOW ---
	// Remove object from target list if it no longer exists
	%objName = stripTrailingNumber( %meshName );

	if ( ShapeEditor.shape.getObjectIndex( %objName ) == -1 ) {
		%id = ShapeEdColWindow-->colTarget.findText( %objName );

		if ( %id != -1 )
			ShapeEdColWindow-->colTarget.clearEntry( %id );
	}

	// --- DETAILS TAB ---
	// Determine which item to select next
	%id = ShapeEdDetailTree.findItemByName( %meshName );

	if ( %id > 0 ) {
		%nextId = ShapeEdDetailTree.getPrevSibling( %id );

		if ( %nextId <= 0 ) {
			%nextId = ShapeEdDetailTree.getNextSibling( %id );

			if ( %nextId <= 0 )
				%nextId = 2;
		}

		// Remove the entry from the tree
		%meshSize = getTrailingNumber( %meshName );
		ShapeEdDetailTree.removeMeshEntry( %meshName, %meshSize );

		// Change selection if needed
		if ( ShapeEdDetailTree.getSelectedItem() == -1 )
			ShapeEdDetailTree.selectItem( %nextId );
	}
}

function ShapeEdDetailTree::onSelect( %this, %id ) {
	%name = %this.getItemText( %id );
	%baseName = stripTrailingNumber( %name );
	%size = getTrailingNumber( %name );
	ShapeEdDetails-->meshName.setText( %baseName );
	ShapeEdDetails-->meshSize.setText( %size );
	// Select the appropriate detail level
	%dl = %this.getDetailLevelFromItem( %id );
	ShapeEdShapeView.currentDL = %dl;

	if ( %this.isDetailItem( %id ) ) {
		// Selected a detail => disable mesh controls
		ShapeEdDetails-->editMeshActive.setVisible( false );
		ShapeEdShapeView.selectedObject = -1;
		ShapeEdShapeView.selectedObjDetail = 0;
	} else {
		// Selected a mesh => sync mesh controls
		ShapeEdDetails-->editMeshActive.setVisible( true );

		switch$ ( ShapeEditor.shape.getMeshType( %name ) ) {
		case "normal":
			ShapeEdDetails-->bbType.setSelected( 0, false );

		case "billboard":
			ShapeEdDetails-->bbType.setSelected( 1, false );

		case "billboardzaxis":
			ShapeEdDetails-->bbType.setSelected( 2, false );
		}

		%node = ShapeEditor.shape.getObjectNode( %baseName );

		if ( %node $= "" )
			%node = "<root>";

		ShapeEdDetails-->objectNode.setSelected( ShapeEdDetails-->objectNode.findText( %node ), false );
		ShapeEdShapeView.selectedObject = ShapeEditor.shape.getObjectIndex( %baseName );
		ShapeEdShapeView.selectedObjDetail = %dl;
	}
}

function ShapeEdDetailTree::onRightMouseUp( %this, %itemId, %mouse ) {
	// Open context menu if this is a Mesh item
	if ( !%this.isDetailItem( %itemId ) ) {
		if( !isObject( "ShapeEdMeshPopup" ) ) {
			new PopupMenu( ShapeEdMeshPopup ) {
				superClass = "MenuBuilder";
				isPopup = "1";
				item[ 0 ] = "Hidden" TAB "" TAB "ShapeEdDetailTree.onHideMeshItem( %this._objName, !%this._itemHidden );";
				item[ 1 ] = "-";
				item[ 2 ] = "Hide all" TAB "" TAB "ShapeEdDetailTree.onHideMeshItem( \"\", true );";
				item[ 3 ] = "Show all" TAB "" TAB "ShapeEdDetailTree.onHideMeshItem( \"\", false );";
			};
		}

		ShapeEdMeshPopup._objName = stripTrailingNumber( %this.getItemText( %itemId ) );
		ShapeEdMeshPopup._itemHidden = ShapeEdShapeView.getMeshHidden( ShapeEdMeshPopup._objName );
		ShapeEdMeshPopup.checkItem( 0, ShapeEdMeshPopup._itemHidden );
		ShapeEdMeshPopup.showPopup( Canvas );
	}
}

function ShapeEdDetailTree::onHideMeshItem( %this, %objName, %hide ) {
	if ( %hide )
		%imageId = %this._imageHidden;
	else
		%imageId = %this._imageNone;

	if ( %objName $= "" ) {
		// Show/hide all
		ShapeEdShapeView.setAllMeshesHidden( %hide );

		for ( %parent = %this.getChild(%this.getFirstRootItem()); %parent > 0; %parent = %this.getNextSibling(%parent) )
			for ( %child = %this.getChild(%parent); %child > 0; %child = %this.getNextSibling(%child) )
				%this.setItemImages( %child, %imageId, %imageId );
	} else {
		// Show/hide all meshes for this object
		ShapeEdShapeView.setMeshHidden( %objName, %hide );
		%count = ShapeEditor.shape.getMeshCount( %objName );

		for ( %i = 0; %i < %count; %i++ ) {
			%meshName = ShapeEditor.shape.getMeshName( %objName, %i );
			%id = ShapeEdDetailTree.findItemByName( %meshName );

			if ( %id > 0 )
				%this.setItemImages( %id, %imageId, %imageId );
		}
	}
}

function ShapeEdShapeView::onDetailChanged( %this ) {
	// Update slider
	if ( mRound( ShapeEdAdvancedWindow-->detailSlider.getValue() ) != %this.currentDL )
		ShapeEdAdvancedWindow-->detailSlider.setValue( %this.currentDL );

	ShapeEdAdvancedWindow-->detailSize.setText( %this.detailSize );
	ShapeEdDetails.update_onDetailsChanged();
	%id = ShapeEdDetailTree.getSelectedItem();

	if ( ( %id <= 0 ) || ( %this.currentDL != ShapeEdDetailTree.getDetailLevelFromItem( %id ) ) ) {
		%id = ShapeEdDetailTree.findItemByValue( %this.detailSize );

		if ( %id > 0 ) {
			ShapeEdDetailTree.clearSelection();
			ShapeEdDetailTree.selectItem( %id );
		}
	}
}

function ShapeEdAdvancedWindow::onEditDetailSize( %this ) {
	// Change the size of the current detail level
	%oldSize = ShapeEditor.shape.getDetailLevelSize( ShapeEdShapeView.currentDL );
	%detailSize = %this-->detailSize.getText();
	ShapeEditor.doEditDetailSize( %oldSize, %detailSize );
}

function ShapeEdDetails::onEditName( %this ) {
	%newName = %this-->meshName.getText();
	// Check if we are renaming a detail or a mesh
	%id = ShapeEdDetailTree.getSelectedItem();
	%oldName = ShapeEdDetailTree.getItemText( %id );

	if ( ShapeEdDetailTree.isDetailItem( %id ) ) {
		// Rename the selected detail level
		%oldSize = getTrailingNumber( %oldName );
		ShapeEditor.doRenameDetail( %oldName, %newName @ %oldSize );
	} else {
		// Rename the selected mesh
		ShapeEditor.doRenameObject( stripTrailingNumber( %oldName ), %newName );
	}
}

function ShapeEdDetails::onEditSize( %this ) {
	%newSize = %this-->meshSize.getText();
	// Check if we are changing the size for a detail or a mesh
	%id = ShapeEdDetailTree.getSelectedItem();

	if ( ShapeEdDetailTree.isDetailItem( %id ) ) {
		// Change the size of the selected detail level
		%oldSize = ShapeEdDetailTree.getItemValue( %id );
		ShapeEditor.doEditDetailSize( %oldSize, %newSize );
	} else {
		// Change the size of the selected mesh
		%meshName = ShapeEdDetailTree.getItemText( %id );
		ShapeEditor.doEditMeshSize( %meshName, %newSize );
	}
}

function ShapeEdDetails::onEditBBType( %this ) {
	// This command is only valid for meshes (not details)
	%id = ShapeEdDetailTree.getSelectedItem();

	if ( !ShapeEdDetailTree.isDetailItem( %id ) ) {
		%meshName = ShapeEdDetailTree.getItemText( %id );
		%bbType = ShapeEdDetails-->bbType.getText();

		switch$ ( %bbType ) {
		case "None":
			%bbType = "normal";

		case "Billboard":
			%bbType = "billboard";

		case "Z Billboard":
			%bbType = "billboardzaxis";
		}

		ShapeEditor.doEditMeshBillboard( %meshName, %bbType );
	}
}

function ShapeEdDetails::onSetObjectNode( %this ) {
	// This command is only valid for meshes (not details)
	%id = ShapeEdDetailTree.getSelectedItem();

	if ( !ShapeEdDetailTree.isDetailItem( %id ) ) {
		%meshName = ShapeEdDetailTree.getItemText( %id );
		%objName = stripTrailingNumber( %meshName );
		%node = %this-->objectNode.getText();

		if ( %node $= "<root>" )
			%node = "";

		ShapeEditor.doSetObjectNode( %objName, %node );
	}
}

function ShapeEdDetails::onAddMeshFromFile( %this, %path ) {
	if ( %path $= "" ) {
		getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onAddMeshFromFile", %this.lastPath );
		return;
	}

	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;

	// Determine the detail level to use for the new geometry
	if ( %this-->addGeomTo.getText() $= "current detail" ) {
		%size = ShapeEditor.shape.getDetailLevelSize( ShapeEdShapeView.currentDL );
	} else {
		// Check if the file has an LODXXX hint at the end of it
		%base = fileBase( %path );
		%pos = strstr( %base, "_LOD" );

		if ( %pos > 0 )
			%size = getSubStr( %base, %pos + 4, strlen( %base ) ) + 0;
		else
			%size = 2;

		// Make sure size is not in use
		while ( ShapeEditor.shape.getDetailLevelIndex( %size ) != -1 )
			%size++;
	}

	ShapeEditor.doAddMeshFromFile( %path, %size );
}

function ShapeEdDetails::onDeleteMesh( %this ) {
	%id = ShapeEdDetailTree.getSelectedItem();

	if ( ShapeEdDetailTree.isDetailItem( %id ) ) {
		%detSize = ShapeEdDetailTree.getItemValue( %id );
		ShapeEditor.doRemoveShapeData( "Detail", %detSize );
	} else {
		%name = ShapeEdDetailTree.getItemText( %id );
		ShapeEditor.doRemoveShapeData( "Mesh", %name );
	}
}



function ShapeEditor::autoAddDetails( %this, %dest ) {
	// Sets of LOD files are named like:
	//
	// MyShape_LOD200.dae
	// MyShape_LOD64.dae
	// MyShape_LOD2.dae
	//
	// Determine the base name of the input file (MyShape_LOD in the example above)
	// and use that to find any other shapes in the set.
	%base = fileBase( %dest.baseShape );
	%pos = strstr( %base, "_LOD" );

	if ( %pos < 0 ) {
		echo( "Not an LOD shape file" );
		return;
	}

	%base = getSubStr( %base, 0, %pos + 4 );
	echo( "Base is: " @ %base );
	%filePatterns = filePath( %dest.baseShape ) @ "/" @ %base @ "*" @ fileExt( %dest.baseShape );
	echo( "Pattern is: " @ %filePatterns );
	%fullPath = findFirstFileMultiExpr( %filePatterns );

	while ( %fullPath !$= "" ) {
		%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );

		if ( %fullPath !$= %dest.baseShape ) {
			echo( "Found LOD shape file: " @ %fullPath );
			// Determine the detail size ( number after the base name ), then add the
			// new mesh
			%size = strreplace( fileBase( %fullPath ), %base, "" );
			ShapeEditor.addLODFromFile( %dest, %fullPath, %size, 0 );
		}

		%fullPath = findNextFileMultiExpr( %filePatterns );
	}

	if ( %this.shape == %dest ) {
		ShapeEdShapeView.refreshShape();
		ShapeEdDetails.update_onDetailsChanged();
	}
}

function ShapeEditor::addLODFromFile( %this, %dest, %filename, %size, %allowUnmatched ) {
	// Get (or create) a TSShapeConstructor object for the source shape. Need to
	// exec the script manually as the resource may not have been loaded yet
	%csPath = filePath( %filename ) @ "/" @ fileBase( %filename ) @ ".cs";

	if ( isFile( %csPath ) )
		exec( %csPath );

	%source = ShapeEditor.findConstructor( %filename );

	if ( %source == -1 )
		%source = ShapeEditor.createConstructor( %filename );

	%source.lodType = "SingleSize";
	%source.singleDetailSize = %size;
	// Create a temporary TSStatic to ensure the resource is loaded
	%temp = new TSStatic() {
		shapeName = %filename;
		collisionType = "None";
	};
	%meshList = "";

	if ( isObject( %temp ) ) {
		// Add a new mesh for each object in the source shape
		%objCount = %source.getObjectCount();

		for ( %i = 0; %i < %objCount; %i++ ) {
			%objName = %source.getObjectName( %i );
			echo( "Checking for object " @ %objName );

			if ( %allowUnmatched || ( %dest.getObjectIndex( %objName ) != -1 ) ) {
				// Add the source object's highest LOD mesh to the destination shape
				echo( "Adding detail size" SPC %size SPC "for object" SPC %objName );
				%srcName = %source.getMeshName( %objName, 0 );
				%destName = %objName SPC %size;
				%dest.addMesh( %destName, %filename, %srcName );
				%meshList = %meshList TAB %destName;
			}
		}

		%temp.delete();
	}

	return trim( %meshList );
}