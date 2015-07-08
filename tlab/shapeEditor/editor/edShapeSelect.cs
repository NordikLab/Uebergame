//==============================================================================
// TorqueLab -> ShapeEditor -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Select Object Functions
//==============================================================================

//==============================================================================
// Select the current WorldEditor selection
function ShapeEditor::selectWorldEditorShape( %this) {
	%count = EWorldEditor.getSelectionSize();

	for (%i = 0; %i < %count; %i++) {
		%obj = EWorldEditor.getSelectedObject(%i);
		%shapeFile = ShapeEditor.getObjectShapeFile(%obj);	
		//If we have a valid shapefile, make the object the current selection	
		if (%shapeFile !$= "") {			
			//if (!isObject(ShapeEditor.shape) || (ShapeEditor.shape.baseShape !$= %shapeFile)) {		
				//Clear the tree in case and make the current object selected
				ShapeEdShapeTreeView.clearSelection();			
				ShapeEdShapeTreeView.onSelect(%obj);
				
				//Set the Editor shape				
				ShapeEditor.selectShape(%shapeFile, ShapeEditor.isDirty());
				
				// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
				Canvas.repaint();
				ShapeEdShapeView.fitToShape();
				
				break; //Only one shape can be selected at a time so leave
			//}			
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the shape selector list
function ShapeEdSelectWindow::onSelect( %this, %path ) {
	// Prompt user to save the old shape if it is dirty
	if ( ShapeEditor.isDirty() ) {
		%cmd = "ColladaImportDlg.showDialog( \"" @ %path @ "\", \"ShapeEditor.selectShape( \\\"" @ %path @ "\\\", ";
		LabMsgYesNoCancel( "Shape Modified", "Would you like to save your changes?", %cmd @ "true );\" );", %cmd @ "false );\" );" );
	} else {
		%cmd = "ShapeEditor.selectShape( \"" @ %path @ "\", false );";
		ColladaImportDlg.showDialog( %path, %cmd );
	}
}

//------------------------------------------------------------------------------
//==============================================================================
function ShapeEditor::selectShape( %this, %path, %saveOld ) {
	ShapeEdShapeView.setModel( "" );

	if ( %saveOld ) {
		// Save changes to a TSShapeConstructor script
		%this.saveChanges();
	} else if ( ShapeEditor.isDirty() ) {
		// Purge all unsaved changes
		%oldPath = ShapeEditor.shape.baseShape;
		ShapeEditor.shape.delete();
		ShapeEditor.shape = 0;
		reloadResource( %oldPath );   // Force game objects to reload shape
	}

	// Initialise the shape preview window
	if ( !ShapeEdShapeView.setModel( %path ) ) {
		LabMsgOK( "Error", "Failed to load '" @ %path @ "'. Check the console for error messages." );
		return;
	}

	ShapeEdShapeView.fitToShape();
	ShapeEdUndoManager.clearAll();
	ShapeEditor.setDirty( false );
	// Get ( or create ) the TSShapeConstructor object for this shape
	ShapeEditor.shape = ShapeEditor.findConstructor( %path );

	if ( ShapeEditor.shape <= 0 ) {
		ShapeEditor.shape = %this.createConstructor( %path );

		if ( ShapeEditor.shape <= 0 ) {
			error( "ShapeEditor: Error - could not select " @ %path );
			return;
		}
	}

	// Initialise the editor windows
	ShapeEdAdvancedWindow.update_onShapeSelectionChanged();
	ShapeEdMountWindow.update_onShapeSelectionChanged();
	ShapeEdThreadWindow.update_onShapeSelectionChanged();
	ShapeEdColWindow.update_onShapeSelectionChanged();
	ShapeEdPropWindow.update_onShapeSelectionChanged();
	ShapeEdShapeView.refreshShape();
	// Update object type hints
	ShapeEdSelectWindow.updateHints();
	// Update editor status bar
	EditorGuiStatusBar.setSelection( %path );
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function ShapeEdShapeTreeView::onSelect( %this, %obj ) {
	%path = ShapeEditor.getObjectShapeFile( %obj );

	if ( %path !$= "" )
		ShapeEdSelectWindow.onSelect( %path );

	// Set the object type (for required nodes and sequences display)
	%objClass = %obj.getClassName();
	%hintId = -1;
	%count = ShapeHintGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%hint = ShapeHintGroup.getObject( %i );

		if ( %objClass $= %hint.objectType ) {
			%hintId = %hint;
			break;
		} else if ( isMemberOfClass( %objClass, %hint.objectType ) ) {
			%hintId = %hint;
		}
	}

	ShapeEdHintMenu.setSelected( %hintId );
}
//------------------------------------------------------------------------------
//==============================================================================
// Open a Shape file 
//==============================================================================
function ShapeEditorPlugin::openShape( %this, %path, %discardChangesToCurrent ) {
//    Lab.setEditor( ShapeEditorPlugin );
	if( ShapeEditor.isDirty() && !%discardChangesToCurrent ) {
		LabMsgYesNo( "Save Changes?",
						 "Save changes to current shape?",
						 "ShapeEditor.saveChanges(); ShapeEditorPlugin.openShape(\"" @ %path @ "\");",
						 "ShapeEditorPlugin.openShape(\"" @ %path @ "\");" );
		return;
	}

	ShapeEditor.selectShape( %path );
	ShapeEdShapeView.fitToShape();
}
//------------------------------------------------------------------------------
//==============================================================================
/*
function ShapeEditorPlugin::open(%this, %filename) {
		

	// Select the new shape
	if (isObject(ShapeEditor.shape) && (ShapeEditor.shape.baseShape $= %filename)) {
		// Shape is already selected => re-highlight the selected material if necessary
		ShapeEdMaterials.updateSelectedMaterial(ShapeEdMaterials-->highlightMaterial.getValue());
	} else if (%filename !$= "") {
		ShapeEditor.selectShape(%filename, ShapeEditor.isDirty());
		// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
		Canvas.repaint();
		ShapeEdShapeView.fitToShape();
	}
}
*/
//==============================================================================
// Open new shape
//==============================================================================


//==============================================================================
// Shape Constructor Functions
//==============================================================================
function ShapeEditor::findConstructor( %this, %path ) {
	%count = TSShapeConstructorGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = TSShapeConstructorGroup.getObject( %i );

		if ( %obj.baseShape $= %path )
			return %obj;
	}

	return -1;
}

function ShapeEditor::createConstructor( %this, %path ) {
	%name = strcapitalise( fileBase( %path ) ) @ strcapitalise( getSubStr( fileExt( %path ), 1, 3 ) );
	%name = strreplace( %name, "-", "_" );
	%name = strreplace( %name, ".", "_" );
	%name = getUniqueName( %name );
	return new TSShapeConstructor( %name ) {
		baseShape = %path;
	};
}

function ShapeEditor::saveConstructor( %this, %constructor ) {
	%savepath = filePath( %constructor.baseShape ) @ "/" @ fileBase( %constructor.baseShape ) @ ".cs";
	new PersistenceManager( shapeEd_perMan );
	shapeEd_perMan.setDirty( %constructor, %savepath );
	shapeEd_perMan.saveDirtyObject( %constructor );
	shapeEd_perMan.delete();
}



// Update the GUI in response to the shape selection changing
function ShapeEdPropWindow::update_onShapeSelectionChanged( %this ) {
	// --- NODES TAB ---
	ShapeEdNodeTreeView.removeItem( 0 );
	%rootId = ShapeEdNodeTreeView.insertItem( 0, "<root>", 0, "" );
	%count = ShapeEditor.shape.getNodeCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeEditor.shape.getNodeName( %i );

		if ( ShapeEditor.shape.getNodeParentName( %name ) $= "" )
			ShapeEdNodeTreeView.addNodeTree( %name );
	}

	%this.update_onNodeSelectionChanged( -1 );    // no node selected
	// --- SEQUENCES TAB ---

	ShapeEdSequenceList.clear();	
	ShapeEdSequenceList.addRow( -1, "Name" TAB "Cyclic" TAB "Blend" TAB "Frames" TAB "Priority" );
	ShapeEdSequenceList.setRowActive( -1, false );	
	ShapeEdSequenceList.addRow( 0, "<rootpose>" TAB "" TAB "" TAB "" TAB "" );
	%count = ShapeEditor.shape.getSequenceCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeEditor.shape.getSequenceName( %i );

		// Ignore __backup__ sequences (only used by editor)
		if ( !startswith( %name, "__backup__" ) )
			ShapeEdSequenceList.addItem( %name );
	}

	ShapeEdThreadWindow.onAddThread();        // add thread 0
	// --- DETAILS TAB ---
	// Add detail levels and meshes to tree
	ShapeEdDetailTree.clearSelection();
	ShapeEdDetailTree.removeItem( 0 );
	%root = ShapeEdDetailTree.insertItem( 0, "<root>", "", "" );
	%objCount = ShapeEditor.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = ShapeEditor.shape.getObjectName( %i );
		%meshCount = ShapeEditor.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%meshName = ShapeEditor.shape.getMeshName( %objName, %j );
			ShapeEdDetailTree.addMeshEntry( %meshName, 1 );
		}
	}

	// Initialise object node list
	ShapeEdDetails-->objectNode.clear();
	ShapeEdDetails-->objectNode.add( "<root>" );
	%nodeCount = ShapeEditor.shape.getNodeCount();

	for ( %i = 0; %i < %nodeCount; %i++ )
		ShapeEdDetails-->objectNode.add( ShapeEditor.shape.getNodeName( %i ) );

	// --- MATERIALS TAB ---
	ShapeEdMaterials.updateMaterialList();
}
