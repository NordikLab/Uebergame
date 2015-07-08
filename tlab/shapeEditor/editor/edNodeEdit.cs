//==============================================================================
// TorqueLab -> ShapeEditor -> Node Editing 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeEditor -> Node Editing 
//==============================================================================

// Update the GUI in response to the node selection changing
function ShapeEdPropWindow::update_onNodeSelectionChanged( %this, %id ) {
	if ( %id > 0 ) {
		// Enable delete button and edit boxes
		if ( ShapeEdSeqNodeTabBook.activePage $= "Node" )
			ShapeEdPropWindow-->deleteBtn.setActive( true );

		ShapeEdNodes-->nodeName.setActive( true );
		ShapeEdNodes-->nodePosition.setActive( true );
		ShapeEdNodes-->nodeRotation.setActive( true );
		// Update the node inspection data
		%name = ShapeEdNodeTreeView.getItemText( %id );
		ShapeEdNodes-->nodeName.setText( %name );
		// Node parent list => ancestor and sibling nodes only (can't re-parent to a descendent)
		ShapeEdNodeParentMenu.clear();
		%parentNames = ShapeEditor.getNodeNames( "", "<root>", %name );
		%count = getWordCount( %parentNames );

		for ( %i = 0; %i < %count; %i++ )
			ShapeEdNodeParentMenu.add( getWord(%parentNames, %i), %i );

		%pName = ShapeEditor.shape.getNodeParentName( %name );

		if ( %pName $= "" )
			%pName = "<root>";

		ShapeEdNodeParentMenu.setText( %pName );

		if ( ShapeEdNodes-->worldTransform.getValue() ) {
			// Global transform
			%txfm = ShapeEditor.shape.getNodeTransform( %name, 1 );
			ShapeEdNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			ShapeEdNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		} else {
			// Local transform (relative to parent)
			%txfm = ShapeEditor.shape.getNodeTransform( %name, 0 );
			ShapeEdNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			ShapeEdNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		}

		ShapeEdShapeView.selectedNode = ShapeEditor.shape.getNodeIndex( %name );
	} else {
		// Disable delete button and edit boxes
		if ( ShapeEdSeqNodeTabBook.activePage $= "Node" )
			ShapeEdPropWindow-->deleteBtn.setActive( false );

		ShapeEdNodes-->nodeName.setActive( false );
		ShapeEdNodes-->nodePosition.setActive( false );
		ShapeEdNodes-->nodeRotation.setActive( false );
		ShapeEdNodes-->nodeName.setText( "" );
		ShapeEdNodes-->nodePosition.setText( "" );
		ShapeEdNodes-->nodeRotation.setText( "" );
		ShapeEdShapeView.selectedNode = -1;
	}
}

// Update the GUI in response to a node being added
function ShapeEdPropWindow::update_onNodeAdded( %this, %nodeName, %oldTreeIndex ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	ShapeEdShapeView.updateNodeTransforms();
	ShapeEdSelectWindow.updateHints();

	// --- MOUNT WINDOW ---
	if ( ShapeEdMountWindow.isMountableNode( %nodeName ) ) {
		ShapeEdMountWindow-->mountNode.add( %nodeName );
		ShapeEdMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = ShapeEdNodeTreeView.addNodeTree( %nodeName );

	if ( %oldTreeIndex <= 0 ) {
		// This is a new node => make it the current selection
		if ( %id > 0 ) {
			ShapeEdNodeTreeView.clearSelection();
			ShapeEdNodeTreeView.selectItem( %id );
		}
	} else {
		// This node has been un-deleted. Inserting a new item puts it at the
		// end of the siblings, but we want to restore the original order as
		// if the item was never deleted, so move it up as required.
		%childIndex = ShapeEdNodeTreeView.getChildIndexByName( %nodeName );

		while ( %childIndex > %oldTreeIndex ) {
			ShapeEdNodeTreeView.moveItemUp( %id );
			%childIndex--;
		}
	}

	// --- DETAILS TAB ---
	ShapeEdDetails-->objectNode.add( %nodeName );
}

// Update the GUI in response to a node(s) being removed
function ShapeEdPropWindow::update_onNodeRemoved( %this, %nameList, %nameCount ) {
	// --- MISC ---
	ShapeEdShapeView.refreshShape();
	ShapeEdShapeView.updateNodeTransforms();
	ShapeEdSelectWindow.updateHints();

	// Remove nodes from the mountable list, and any shapes mounted to the node
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		ShapeEdMountWindow-->mountNode.clearEntry( ShapeEdMountWindow-->mountNode.findText( %nodeName ) );

		for ( %j = ShapeEdMountWindow-->mountList.rowCount()-1; %j >= 1; %j-- ) {
			%text = ShapeEdMountWindow-->mountList.getRowText( %j );

			if ( getField( %text, 1 ) $= %nodeName ) {
				ShapeEdShapeView.unmountShape( %j-1 );
				ShapeEdMountWindow-->mountList.removeRow( %j );
			}
		}
	}

	// --- NODES TAB ---
	%lastName = getField( %nameList, %nameCount-1 );
	%id = ShapeEdNodeTreeView.findItemByName( %lastName );   // only need to remove the parent item

	if ( %id > 0 ) {
		ShapeEdNodeTreeView.removeItem( %id );

		if ( ShapeEdNodeTreeView.getSelectedItem() <= 0 )
			ShapeEdPropWindow.update_onNodeSelectionChanged( -1 );
	}

	// --- DETAILS TAB ---
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		ShapeEdDetails-->objectNode.clearEntry( ShapeEdDetails-->objectNode.findText( %nodeName ) );
	}
}

// Update the GUI in response to a node being renamed
function ShapeEdPropWindow::update_onNodeRenamed( %this, %oldName, %newName ) {
	// --- MISC ---
	ShapeEdSelectWindow.updateHints();
	// --- MOUNT WINDOW ---
	// Update entries for any shapes mounted to this node
	%rowCount = ShapeEdMountWindow-->mountList.rowCount();

	for ( %i = 1; %i < %rowCount; %i++ ) {
		%text = ShapeEdMountWindow-->mountList.getRowText( %i );

		if ( getField( %text, 1 ) $= %oldName ) {
			%text = setField( %text, 1, %newName );
			ShapeEdMountWindow-->mountList.setRowById( ShapeEdMountWindow-->mountList.getRowId( %i ), %text );
		}
	}

	// Update list of mountable nodes
	ShapeEdMountWindow-->mountNode.clearEntry( ShapeEdMountWindow-->mountNode.findText( %oldName ) );

	if ( ShapeEdMountWindow.isMountableNode( %newName ) ) {
		ShapeEdMountWindow-->mountNode.add( %newName );
		ShapeEdMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = ShapeEdNodeTreeView.findItemByName( %oldName );
	ShapeEdNodeTreeView.editItem( %id, %newName, 0 );

	if ( ShapeEdNodeTreeView.getSelectedItem() == %id )
		ShapeEdNodes-->nodeName.setText( %newName );

	// --- DETAILS TAB ---
	%id = ShapeEdDetails-->objectNode.findText( %oldName );

	if ( %id != -1 ) {
		ShapeEdDetails-->objectNode.clearEntry( %id );
		ShapeEdDetails-->objectNode.add( %newName, %id );
		ShapeEdDetails-->objectNode.sortID();

		if ( ShapeEdDetails-->objectNode.getText() $= %oldName )
			ShapeEdDetails-->objectNode.setText( %newName );
	}
}

// Update the GUI in response to a node's parent being changed
function ShapeEdPropWindow::update_onNodeParentChanged( %this, %nodeName ) {
	// --- MISC ---
	ShapeEdShapeView.updateNodeTransforms();
	// --- NODES TAB ---
	%isSelected = 0;
	%id = ShapeEdNodeTreeView.findItemByName( %nodeName );

	if ( %id > 0 ) {
		%isSelected = ( ShapeEdNodeTreeView.getSelectedItem() == %id );
		ShapeEdNodeTreeView.removeItem( %id );
	}

	ShapeEdNodeTreeView.addNodeTree( %nodeName );

	if ( %isSelected )
		ShapeEdNodeTreeView.selectItem( ShapeEdNodeTreeView.findItemByName( %nodeName ) );
}

function ShapeEdPropWindow::update_onNodeTransformChanged( %this, %nodeName ) {
	// Default to the selected node if none is specified
	if ( %nodeName $= "" ) {
		%id = ShapeEdNodeTreeView.getSelectedItem();

		if ( %id > 0 )
			%nodeName = ShapeEdNodeTreeView.getItemText( %id );
		else
			return;
	}

	// --- MISC ---
	ShapeEdShapeView.updateNodeTransforms();

	if ( ShapeEdNodes-->objectTransform.getValue() )
		GlobalGizmoProfile.setFieldValue(alignment, Object);
	else
		GlobalGizmoProfile.setFieldValue(alignment, World);

	// --- NODES TAB ---
	// Update the node transform fields if necessary
	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( ( %id > 0 ) && ( ShapeEdNodeTreeView.getItemText( %id ) $= %nodeName ) ) {
		%isWorld = ShapeEdNodes-->worldTransform.getValue();
		%transform = ShapeEditor.shape.getNodeTransform( %nodeName, %isWorld );
		ShapeEdNodes-->nodePosition.setText( getWords( %transform, 0, 2 ) );
		ShapeEdNodes-->nodeRotation.setText( getWords( %transform, 3, 6 ) );
	}
}

function ShapeEdNodeTreeView::onClearSelection( %this ) {
	ShapeEdPropWindow.update_onNodeSelectionChanged( -1 );
}

function ShapeEdNodeTreeView::onSelect( %this, %id ) {
	// Update the node name and transform controls
	ShapeEdPropWindow.update_onNodeSelectionChanged( %id );

	// Update orbit position if orbiting the selected node
	if ( ShapeEdShapeView.orbitNode ) {
		%name = %this.getItemText( %id );
		%transform = ShapeEditor.shape.getNodeTransform( %name, 1 );
		ShapeEdShapeView.setOrbitPos( getWords( %transform, 0, 2 ) );
	}
}

function ShapeEdShapeView::onNodeSelected( %this, %index ) {
	ShapeEdNodeTreeView.clearSelection();

	if ( %index > 0 ) {
		%name = ShapeEditor.shape.getNodeName( %index );
		%id = ShapeEdNodeTreeView.findItemByName( %name );

		if ( %id > 0 )
			ShapeEdNodeTreeView.selectItem( %id );
	}
}

function ShapeEdNodes::onAddNode( %this, %name ) {
	// Add a new node, using the currently selected node as the initial parent
	if ( %name $= "" )
		%name = ShapeEditor.getUniqueName( "node", "myNode" );

	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( %id <= 0 )
		%parent = "";
	else
		%parent = ShapeEdNodeTreeView.getItemText( %id );

	ShapeEditor.doAddNode( %name, %parent, "0 0 0 0 0 1 0" );
}

function ShapeEdNodes::onDeleteNode( %this ) {
	// Remove the node and all its children from the shape
	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = ShapeEdNodeTreeView.getItemText( %id );
		ShapeEditor.doRemoveShapeData( "Node", %name );
	}
}

// Determine the index of a node in the tree relative to its parent
function ShapeEdNodeTreeView::getChildIndexByName( %this, %name ) {
	%id = %this.findItemByName( %name );
	%parentId = %this.getParent( %id );
	%childId = %this.getChild( %parentId );

	if ( %childId <= 0 )
		return 0;   // bad!

	%index = 0;

	while ( %childId != %id ) {
		%childId = %this.getNextSibling( %childId );
		%index++;
	}

	return %index;
}

// Add a node and its children to the node tree view
function ShapeEdNodeTreeView::addNodeTree( %this, %nodeName ) {
	// Abort if already added => something dodgy has happened and we'd end up
	// recursing indefinitely
	if ( %this.findItemByName( %nodeName ) ) {
		error( "Recursion error in ShapeEdNodeTreeView::addNodeTree" );
		return 0;
	}

	// Find parent and add me to it
	%parentName = ShapeEditor.shape.getNodeParentName( %nodeName );

	if ( %parentName $= "" )
		%parentName = "<root>";

	%parentId = %this.findItemByName( %parentName );
	%id = %this.insertItem( %parentId, %nodeName, 0, "" );
	// Add children
	%count = ShapeEditor.shape.getNodeChildCount( %nodeName );

	for ( %i = 0; %i < %count; %i++ )
		%this.addNodeTree( ShapeEditor.shape.getNodeChildName( %nodeName, %i ) );

	return %id;
}

function ShapeEdNodes::onEditName( %this ) {
	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%oldName = ShapeEdNodeTreeView.getItemText( %id );
		%newName = %this-->nodeName.getText();

		if ( %newName !$= "" )
			ShapeEditor.doRenameNode( %oldName, %newName );
	}
}

function ShapeEdNodeParentMenu::onSelect( %this, %id, %text ) {
	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = ShapeEdNodeTreeView.getItemText( %id );
		ShapeEditor.doSetNodeParent( %name, %text );
	}
}

function ShapeEdNodes::onEditTransform( %this ) {
	%id = ShapeEdNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = ShapeEdNodeTreeView.getItemText( %id );
		// Get the node transform from the gui
		%pos = %this-->nodePosition.getText();
		%rot = %this-->nodeRotation.getText();
		%txfm = %pos SPC %rot;
		%isWorld = ShapeEdNodes-->worldTransform.getValue();

		// Do a quick sanity check to avoid setting wildly invalid transforms
		for ( %i = 0; %i < 7; %i++ ) {  // "x y z aa.x aa.y aa.z aa.angle"
			if ( getWord( %txfm, %i ) $= "" )
				return;
		}

		ShapeEditor.doEditNodeTransform( %name, %txfm, %isWorld, -1 );
	}
}

function ShapeEdShapeView::onEditNodeTransform( %this, %node, %txfm, %gizmoID ) {
	ShapeEditor.doEditNodeTransform( %node, %txfm, 1, %gizmoID );
}
