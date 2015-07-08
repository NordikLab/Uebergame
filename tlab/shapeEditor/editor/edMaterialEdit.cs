//==============================================================================
// TorqueLab -> ShapeEditor -> Material Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Edit the selected object material
//==============================================================================

function ShapeEdMaterials::editSelectedMaterial( %this ) {
	if ( isObject( %this.selectedMaterial ) ) {
		// Remove the highlight effect from the selected material, then switch
		// to the Material Editor
		%this.updateSelectedMaterial( false );
		// Create a temporary TSStatic so the MaterialEditor can query the model's
		// materials.
		pushInstantGroup();
		%this.tempShape = new TSStatic() {
			shapeName = ShapeEditor.shape.baseShape;
			collisionType = "None";
		};
		popInstantGroup();
		MaterialEditorGui.currentMaterial = %this.selectedMaterial;
		MaterialEditorGui.currentObject = $Lab::materialEditorList = %this.tempShape;
		Lab.setEditor(MaterialEditorPlugin);

		
		
		show(MEP_CallbackArea);
		MEP_CallbackArea-->callbackButton.text = "Return to ShapeEditor";
		MEP_CallbackArea-->callbackButton.command = "ShapeEdMaterials.editSelectedMaterialEnd();";
		//ShapeEdSelectWindow.setVisible( false );
		//ShapeEdPropWindow.setVisible( false );
		//EditorGui-->MatEdPropertiesWindow.setVisible( true );
		//EditorGui-->MatEdPreviewWindow.setVisible( true );
		//MatEd_phoBreadcrumb.setVisible( true );
		//MatEd_phoBreadcrumb.command = "ShapeEdMaterials.editSelectedMaterialEnd();";
		//advancedTextureMapsRollout.Expanded = false;
		//materialAnimationPropertiesRollout.Expanded = false;
		//materialAdvancedPropertiesRollout.Expanded = false;
		//MaterialEditorGui.open();
		//MaterialEditorGui.setActiveMaterial( %this.selectedMaterial );
		%id = SubMaterialSelector.findText( %this.selectedMapTo );

		if( %id != -1 )
			SubMaterialSelector.setSelected( %id );
			
		Lab.setEditor(MaterialEditorPlugin);
	}
}

function ShapeEdMaterials::editSelectedMaterialEnd( %this, %closeEditor ) {	
	hide(MEP_CallbackArea);
	Lab.setEditor(ShapeEditorPlugin);

	//MaterialEditorGui.quit();
	//EditorGui-->MatEdPropertiesWindow.setVisible( false );
	//EditorGui-->MatEdPreviewWindow.setVisible( false );
	// Delete the temporary TSStatic
	%this.tempShape.delete();

	if( !%closeEditor ) {
		ShapeEdSelectWindow.setVisible( true );
		ShapeEdPropWindow.setVisible( true );
	}
}
//==============================================================================
// ShapeEditor -> Material Editing
//==============================================================================


function ShapeEdMaterials::updateMaterialList( %this ) {
	// --- MATERIALS TAB ---
	ShapeEdMaterialList.clear();
	ShapeEdMaterialList.addRow( -2, "Name" TAB "Mapped" );
	ShapeEdMaterialList.setRowActive( -2, false );
	ShapeEdMaterialList.addRow( -1, "<none>" );
	%count = ShapeEditor.shape.getTargetCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%matName = ShapeEditor.shape.getTargetName( %i );
		%mapped = getMaterialMapping( %matName );

		if ( %mapped $= "" )
			ShapeEdMaterialList.addRow( WarningMaterial.getID(), %matName TAB "unmapped" );
		else
			ShapeEdMaterialList.addRow( %mapped.getID(), %matName TAB %mapped );
	}

	ShapeEdMaterials-->materialListHeader.setExtent( getWord( ShapeEdMaterialList.extent, 0 ) SPC "19" );
}

function ShapeEdMaterials::updateSelectedMaterial( %this, %highlight ) {
	// Remove the highlight effect from the old selection
	if ( isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = %this.savedMap;
		%this.selectedMaterial.reload();
	}

	// Apply the highlight effect to the new selected material
	%this.selectedMapTo = getField( ShapeEdMaterialList.getRowText( ShapeEdMaterialList.getSelectedRow() ), 0 );
	%this.selectedMaterial = ShapeEdMaterialList.getSelectedId();
	%this.savedMap = %this.selectedMaterial.diffuseMap[1];

	if ( %highlight && isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = "tlab/shapeEditor/images/highlight_material";
		%this.selectedMaterial.reload();
	}
}
