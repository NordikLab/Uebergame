//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TerrainMaterialDlg::show( %this, %matIndex, %terrMat, %onApplyCallback ) {
	Canvas.pushDialog( %this );
	%this.matIndex = %matIndex;
	%this.onApplyCallback = %onApplyCallback;
	%matLibTree = %this-->matLibTree;
	%item = %matLibTree.findItemByObjectId( %terrMat );

	if ( %item != -1 ) {
		%matLibTree.selectItem( %item );
		%matLibTree.scrollVisible( %item );
	} else {
		for( %i = 1; %i < %matLibTree.getItemCount(); %i++ ) {
			%terrMat = TerrainMaterialDlg-->matLibTree.getItemValue(%i);

			if( %terrMat.getClassName() $= "TerrainMaterial" ) {
				%matLibTree.selectItem( %i, true );
				%matLibTree.scrollVisible( %i );
				break;
			}
		}
	}
}
//------------------------------------------------------------------------------


//==============================================================================
//Show terrain material information (if no ObjectId, user need to select one)
function TerrainMaterialDlg::showByObjectId( %this, %matObjectId, %onApplyCallback ) {
	Canvas.pushDialog( %this );
	%this.matIndex = -1;
	%this.onApplyCallback = %onApplyCallback;
	%matLibTree = %this-->matLibTree;
	%matLibTree.clearSelection();
	%item = %matLibTree.findItemByObjectId( %matObjectId );

	if ( %item != -1 ) {
		%matLibTree.selectItem( %item );
		%matLibTree.scrollVisible( %item );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::onWake( %this ) {
	if( !isObject( ETerrainMaterialPersistMan ) )
		new PersistenceManager( ETerrainMaterialPersistMan );

	if( !isObject( TerrainMaterialDlgNewGroup ) )
		new SimGroup( TerrainMaterialDlgNewGroup );

	if( !isObject( TerrainMaterialDlgDeleteGroup ) )
		new SimGroup( TerrainMaterialDlgDeleteGroup );

	if (!$TerrainMaterialDlg_Initialized)
		%this.initFiltersData();

	// Snapshot the materials.
	%this.snapshotMaterials();
	// Refresh the material list.
	%this.setFilteredMaterialsSet();
	$TerrainMaterialDlg_Initialized = true;
	//%this.activateMaterialCtrls( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::refreshMaterialTree( %this,%selected ) {
	// Refresh the material list.
	%matLibTree = %this-->matLibTree;
	%matLibTree.clear();
	%matLibTree.open( FilteredTerrainMaterialsSet, false );
	%matLibTree.buildVisibleTree( true );
	%item = %matLibTree.getFirstRootItem();
	%matLibTree.expandItem( %item );
	%this.activateMaterialCtrls( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::onSleep( %this ) {
	if( isObject( TerrainMaterialDlgSnapshot ) )
		TerrainMaterialDlgSnapshot.delete();

	%this-->matLibTree.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function TMD_MaterialTree::onMouseUp( %this ,%itemId,%click) {
	if (%click $= "2")
		TerrainMaterialDlg.dialogApply(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::dialogApply( %this,%closeDlg ) {
	// Move all new materials we have created to the root group.
	%newCount = TerrainMaterialDlgNewGroup.getCount();

	for( %i = 0; %i < %newCount; %i ++ )
		RootGroup.add( TerrainMaterialDlgNewGroup.getObject( %i ) );

	// Finalize deletion of all removed materials.
	%deletedCount = TerrainMaterialDlgDeleteGroup.getCount();

	for( %i = 0; %i < %deletedCount; %i ++ ) {
		%mat = TerrainMaterialDlgDeleteGroup.getObject( %i );
		ETerrainMaterialPersistMan.removeObjectFromFile( %mat );
		%matIndex = ETerrainEditor.getMaterialIndex( %mat.internalName );

		if( %matIndex != -1 ) {
			ETerrainEditor.removeMaterial( %matIndex );
			EPainter.updateLayers();
		}

		%mat.delete();
	}

	// Make sure we save any changes to the current selection.
	%this.saveDirtyMaterial( %this.activeMat );
	// Save all changes.
	ETerrainMaterialPersistMan.saveDirty();
	// Delete the snapshot.
	TerrainMaterialDlgSnapshot.delete();

	// Remove ourselves from the canvas.
	if (%closeDlg)
		Canvas.popDialog( TerrainMaterialDlg );

	call( %this.onApplyCallback, %this.activeMat, %this.matIndex );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::dialogCancel( %this ) {
	// Restore material properties we have changed.
	%this.restoreMaterials();
	// Clear the persistence manager state.
	ETerrainMaterialPersistMan.clearAll();
	// Delete all new object we have created.
	TerrainMaterialDlgNewGroup.clear();
	// Restore materials we have marked for deletion.
	%deletedCount = TerrainMaterialDlgDeleteGroup.getCount();

	for( %i = 0; %i < %deletedCount; %i ++ ) {
		%mat = TerrainMaterialDlgDeleteGroup.getObject( %i );
		%mat.parentGroup = RootGroup;
		TerrainMaterialSet.add( %mat );
	}

	Canvas.popDialog( TerrainMaterialDlg );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialTreeCtrl::onSelect( %this, %item ) {
	TerrainMaterialDlg.setActiveMaterial( %item );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialTreeCtrl::onUnSelect( %this, %item ) {
	TerrainMaterialDlg.saveDirtyMaterial( %item );
	TerrainMaterialDlg.setActiveMaterial( 0 );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::_selectTextureFileDialog( %this, %defaultFileName ) {
	if( $Pref::TerrainEditor::LastPath $= "" )
		$Pref::TerrainEditor::LastPath = "art/terrains/";

	if (TerrainPainterTools.defaultTexturesFolder !$= "")
		%defaultPath = TerrainPainterTools.defaultTexturesFolder;
	else
		%defaultPath = $Pref::TerrainEditor::LastPath;

	%dlg = new OpenFileDialog() {
		Filters        = $TerrainEditor::TextureFileSpec;
		DefaultPath    = $Pref::TerrainEditor::LastPath;
		DefaultFile    = %defaultFileName;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if ( %ret ) {
		$Pref::TerrainEditor::LastPath = filePath( %dlg.FileName );
		%file = %dlg.FileName;
	}

	%dlg.delete();

	if ( !%ret )
		return;

	%file = filePath(%file) @ "/" @ fileBase(%file);
	return %file;
}
//------------------------------------------------------------------------------
