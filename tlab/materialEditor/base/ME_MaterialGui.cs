//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Automated editor plugin setting interface
function MaterialEditorGui::initGui(%this,%params ) {
	advancedTextureMapsRollout.Expanded = false;
	materialAnimationPropertiesRollout.Expanded = false;
	materialAdvancedPropertiesRollout.Expanded = false;
	%this-->previewOptions.expanded = false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Select object logic (deciding material/target mode)

function MaterialEditorGui::setMode( %this ) {
	MatEdMaterialMode.setVisible(0);
	MatEdTargetMode.setVisible(0);

	if( isObject(MaterialEditorGui.currentObject) ) {
		MaterialEditorGui.currentMode = "Mesh";
		MatEdTargetMode.setVisible(1);
	} else {
		MaterialEditorGui.currentMode = "Material";
		MatEdMaterialMode.setVisible(1);
		EWorldEditor.clearSelection();
	}
}


//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorGui::updatePreviewObject(%this) {
	%newModel = matEd_quickPreview_Popup.getValue();

	switch$(%newModel) {
	case "sphere":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/spherePreview.dts");
		matEd_previewObjectView.setOrbitDistance(4);

	case "cube":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/cubePreview.dts");
		matEd_previewObjectView.setOrbitDistance(5);

	case "pyramid":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/pyramidPreview.dts");
		matEd_previewObjectView.setOrbitDistance(5);

	case "cylinder":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/cylinderPreview.dts");
		matEd_previewObjectView.setOrbitDistance(4.2);

	case "torus":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/torusPreview.dts");
		matEd_previewObjectView.setOrbitDistance(4.2);

	case "knot":
		matEd_quickPreview_Popup.selected = %newModel;
		matEd_previewObjectView.setModel("tlab/materialEditor/assets/torusknotPreview.dts");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorGui::updateLivePreview(%this,%preview) {
	// When checkbox is selected, preview the material in real time, if not; then don't
	if( %preview )
		MaterialEditorGui.copyMaterials( materialEd_previewMaterial, MaterialEditorGui.currentMaterial );
	else
		MaterialEditorGui.copyMaterials( notDirtyMaterial, MaterialEditorGui.currentMaterial );

	MaterialEditorGui.currentMaterial.flush();
	MaterialEditorGui.currentMaterial.reload();
}
//------------------------------------------------------------------------------
//==============================================================================

function MaterialEditorGui::guiSync( %this, %material ) {
	%this.preventUndo = true;

	//Setup our headers
	if( MaterialEditorGui.currentMode $= "material" ) {
		MatEdMaterialMode-->selMaterialName.setText(MaterialEditorGui.currentMaterial.name);
		MatEdMaterialMode-->selMaterialMapTo.setText(MaterialEditorGui.currentMaterial.mapTo);
	} else {
		if( MaterialEditorGui.currentObject.isMethod("getModelFile") ) {
			%sourcePath = MaterialEditorGui.currentObject.getModelFile();

			if( %sourcePath !$= "" ) {
				MatEdTargetMode-->selMaterialMapTo.ToolTip = %sourcePath;
				%sourceName = fileName(%sourcePath);
				MatEdTargetMode-->selMaterialMapTo.setText(%sourceName);
				MatEdTargetMode-->selMaterialName.setText(MaterialEditorGui.currentMaterial.name);
			}
		} else {
			%info = MaterialEditorGui.currentObject.getClassName();
			MatEdTargetMode-->selMaterialMapTo.ToolTip = %info;
			MatEdTargetMode-->selMaterialMapTo.setText(%info);
			MatEdTargetMode-->selMaterialName.setText(MaterialEditorGui.currentMaterial.name);
		}
	}

	MaterialEditorPropertiesWindow-->alphaRefTextEdit.setText((%material).alphaRef);
	MaterialEditorPropertiesWindow-->alphaRefSlider.setValue((%material).alphaRef);
	MaterialEditorPropertiesWindow-->doubleSidedCheckBox.setValue((%material).doubleSided);
	MaterialEditorPropertiesWindow-->transZWriteCheckBox.setValue((%material).translucentZWrite);
	MaterialEditorPropertiesWindow-->alphaTestCheckBox.setValue((%material).alphaTest);
	MaterialEditorPropertiesWindow-->castShadows.setValue((%material).castShadows);
	MaterialEditorPropertiesWindow-->translucentCheckbox.setValue((%material).translucent);

	switch$((%material).translucentBlendOp) {
	case "None":
		%selectedNum = 0;

	case "Mul":
		%selectedNum = 1;

	case "Add":
		%selectedNum = 2;

	case "AddAlpha":
		%selectedNum = 3;

	case "Sub":
		%selectedNum = 4;

	case "LerpAlpha":
		%selectedNum = 5;
	}

	MaterialEditorPropertiesWindow-->blendingTypePopUp.setSelected(%selectedNum);

	if((%material).cubemap !$= "") {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(1);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(1);
	} else if((%material).dynamiccubemap) {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(2);
	} else if((%material).planarReflection) {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(3);
	} else {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(0);
	}

	MaterialEditorPropertiesWindow-->effectColor0Swatch.color = (%material).effectColor[0];
	MaterialEditorPropertiesWindow-->effectColor1Swatch.color = (%material).effectColor[1];
	MaterialEditorPropertiesWindow-->showFootprintsCheckbox.setValue((%material).showFootprints);
	MaterialEditorPropertiesWindow-->showDustCheckbox.setValue((%material).showDust);
	MaterialEditorGui.updateSoundPopup("Footstep", (%material).footstepSoundId, (%material).customFootstepSound);
	MaterialEditorGui.updateSoundPopup("Impact", (%material).impactSoundId, (%material).customImpactSound);
	//layer specific controls are located here
	%layer = MaterialEditorGui.currentLayer;

	if((%material).diffuseMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->diffuseMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->diffuseMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->diffuseMapNameText.setText( (%material).diffuseMap[%layer] );
		MaterialEditorPropertiesWindow-->diffuseMapDisplayBitmap.setBitmap( (%material).diffuseMap[%layer] );
	}

	if((%material).normalMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->normalMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->normalMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->normalMapNameText.setText( (%material).normalMap[%layer] );
		MaterialEditorPropertiesWindow-->normalMapDisplayBitmap.setBitmap( (%material).normalMap[%layer] );
	}

	if((%material).overlayMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->overlayMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->overlayMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->overlayMapNameText.setText( (%material).overlayMap[%layer] );
		MaterialEditorPropertiesWindow-->overlayMapDisplayBitmap.setBitmap( (%material).overlayMap[%layer] );
	}

	if((%material).detailMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->detailMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->detailMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->detailMapNameText.setText( (%material).detailMap[%layer] );
		MaterialEditorPropertiesWindow-->detailMapDisplayBitmap.setBitmap( (%material).detailMap[%layer] );
	}

	if((%material).detailNormalMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->detailNormalMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->detailNormalMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->detailNormalMapNameText.setText( (%material).detailNormalMap[%layer] );
		MaterialEditorPropertiesWindow-->detailNormalMapDisplayBitmap.setBitmap( (%material).detailNormalMap[%layer] );
	}

	if((%material).lightMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->lightMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->lightMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->lightMapNameText.setText( (%material).lightMap[%layer] );
		MaterialEditorPropertiesWindow-->lightMapDisplayBitmap.setBitmap( (%material).lightMap[%layer] );
	}

	if((%material).toneMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->toneMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->toneMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->toneMapNameText.setText( (%material).toneMap[%layer] );
		MaterialEditorPropertiesWindow-->toneMapDisplayBitmap.setBitmap( (%material).toneMap[%layer] );
	}

	if((%material).specularMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->specMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->specMapDisplayBitmap.setBitmap( "tlab/materialEditor/assets/unknownImage" );
	} else {
		MaterialEditorPropertiesWindow-->specMapNameText.setText( (%material).specularMap[%layer] );
		MaterialEditorPropertiesWindow-->specMapDisplayBitmap.setBitmap( (%material).specularMap[%layer] );
	}

	MaterialEditorPropertiesWindow-->detailScaleTextEdit.setText( getWord((%material).detailScale[%layer], 0) );
	MaterialEditorPropertiesWindow-->detailNormalStrengthTextEdit.setText( getWord((%material).detailNormalMapStrength[%layer], 0) );
	MaterialEditorPropertiesWindow-->colorTintSwatch.color = (%material).diffuseColor[%layer];
	MaterialEditorPropertiesWindow-->specularColorSwatch.color = (%material).specular[%layer];
	MaterialEditorPropertiesWindow-->specularPowerTextEdit.setText((%material).specularPower[%layer]);
	MaterialEditorPropertiesWindow-->specularPowerSlider.setValue((%material).specularPower[%layer]);
	MaterialEditorPropertiesWindow-->specularStrengthTextEdit.setText((%material).specularStrength[%layer]);
	MaterialEditorPropertiesWindow-->specularStrengthSlider.setValue((%material).specularStrength[%layer]);
	MaterialEditorPropertiesWindow-->pixelSpecularCheckbox.setValue((%material).pixelSpecular[%layer]);
	MaterialEditorPropertiesWindow-->glowCheckbox.setValue((%material).glow[%layer]);
	MaterialEditorPropertiesWindow-->emissiveCheckbox.setValue((%material).emissive[%layer]);
	MaterialEditorPropertiesWindow-->parallaxTextEdit.setText((%material).parallaxScale[%layer]);
	MaterialEditorPropertiesWindow-->parallaxSlider.setValue((%material).parallaxScale[%layer]);
	MaterialEditorPropertiesWindow-->useAnisoCheckbox.setValue((%material).useAnisotropic[%layer]);
	MaterialEditorPropertiesWindow-->vertLitCheckbox.setValue((%material).vertLit[%layer]);
	MaterialEditorPropertiesWindow-->vertColorSwatch.color = (%material).vertColor[%layer];
	MaterialEditorPropertiesWindow-->subSurfaceCheckbox.setValue((%material).subSurface[%layer]);
	MaterialEditorPropertiesWindow-->subSurfaceColorSwatch.color = (%material).subSurfaceColor[%layer];
	MaterialEditorPropertiesWindow-->subSurfaceRolloffTextEdit.setText((%material).subSurfaceRolloff[%layer]);
	MaterialEditorPropertiesWindow-->minnaertTextEdit.setText((%material).minnaertConstant[%layer]);
	// Animation properties
	MaterialEditorPropertiesWindow-->RotationAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->ScrollAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->WaveAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->ScaleAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->SequenceAnimation.setValue(0);
	%flags = (%material).getAnimFlags(%layer);
	%wordCount = getWordCount( %flags );

	for(%i = 0; %i != %wordCount; %i++) {
		switch$(getWord( %flags, %i)) {
		case "$rotate":
			MaterialEditorPropertiesWindow-->RotationAnimation.setValue(1);

		case "$scroll":
			MaterialEditorPropertiesWindow-->ScrollAnimation.setValue(1);

		case "$wave":
			MaterialEditorPropertiesWindow-->WaveAnimation.setValue(1);

		case "$scale":
			MaterialEditorPropertiesWindow-->ScaleAnimation.setValue(1);

		case "$sequence":
			MaterialEditorPropertiesWindow-->SequenceAnimation.setValue(1);
		}
	}

	MaterialEditorPropertiesWindow-->RotationTextEditU.setText( getWord((%material).rotPivotOffset[%layer], 0) );
	MaterialEditorPropertiesWindow-->RotationTextEditV.setText( getWord((%material).rotPivotOffset[%layer], 1) );
	MaterialEditorPropertiesWindow-->RotationSpeedTextEdit.setText( (%material).rotSpeed[%layer] );
	MaterialEditorPropertiesWindow-->RotationSliderU.setValue( getWord((%material).rotPivotOffset[%layer], 0) );
	MaterialEditorPropertiesWindow-->RotationSliderV.setValue( getWord((%material).rotPivotOffset[%layer], 1) );
	MaterialEditorPropertiesWindow-->RotationSpeedSlider.setValue( (%material).rotSpeed[%layer] );
	MaterialEditorPropertiesWindow-->RotationCrosshair.setPosition( 45*mAbs(getWord((%material).rotPivotOffset[%layer], 0))-2, 45*mAbs(getWord((%material).rotPivotOffset[%layer], 1))-2 );
	MaterialEditorPropertiesWindow-->ScrollTextEditU.setText( getWord((%material).scrollDir[%layer], 0) );
	MaterialEditorPropertiesWindow-->ScrollTextEditV.setText( getWord((%material).scrollDir[%layer], 1) );
	MaterialEditorPropertiesWindow-->ScrollSpeedTextEdit.setText( (%material).scrollSpeed[%layer] );
	MaterialEditorPropertiesWindow-->ScrollSliderU.setValue( getWord((%material).scrollDir[%layer], 0) );
	MaterialEditorPropertiesWindow-->ScrollSliderV.setValue( getWord((%material).scrollDir[%layer], 1) );
	MaterialEditorPropertiesWindow-->ScrollSpeedSlider.setValue( (%material).scrollSpeed[%layer] );
	MaterialEditorPropertiesWindow-->ScrollCrosshair.setPosition( -(23 * getWord((%material).scrollDir[%layer], 0))+20, -(23 * getWord((%material).scrollDir[%layer], 1))+20);
	%waveType = (%material).waveType[%layer];

	for( %radioButton = 0; %radioButton < MaterialEditorPropertiesWindow-->WaveButtonContainer.getCount(); %radioButton++ ) {
		if( %waveType $= MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).waveType )
			MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).setStateOn(1);
	}

	MaterialEditorPropertiesWindow-->WaveTextEditAmp.setText( (%material).waveAmp[%layer] );
	MaterialEditorPropertiesWindow-->WaveTextEditFreq.setText( (%material).waveFreq[%layer] );
	MaterialEditorPropertiesWindow-->WaveSliderAmp.setValue( (%material).waveAmp[%layer] );
	MaterialEditorPropertiesWindow-->WaveSliderFreq.setValue( (%material).waveFreq[%layer] );
	%numFrames = mRound( 1 / (%material).sequenceSegmentSize[%layer] );
	MaterialEditorPropertiesWindow-->SequenceTextEditFPS.setText( (%material).sequenceFramePerSec[%layer] );
	MaterialEditorPropertiesWindow-->SequenceTextEditSSS.setText( %numFrames );
	MaterialEditorPropertiesWindow-->SequenceSliderFPS.setValue( (%material).sequenceFramePerSec[%layer] );
	MaterialEditorPropertiesWindow-->SequenceSliderSSS.setValue( %numFrames );
	%this.preventUndo = false;
}

//------------------------------------------------------------------------------
//==============================================================================
// Color Picker Helpers - They are all using colorPicker.ed.gui in order to function
// These functions are mainly passed callbacks from getColorI/getColorF callbacks

function MaterialEditorGui::syncGuiColor(%this, %guiCtrl, %propname, %color) {
	%layer = MaterialEditorGui.currentLayer;
	%r = getWord(%color,0);
	%g = getWord(%color,1);
	%b = getWord(%color,2);
	%a = getWord(%color,3);
	%colorSwatch = (%r SPC %g SPC %b SPC %a);
	%color = "\"" @ %r SPC %g SPC %b SPC %a @ "\"";
	%guiCtrl.color = %colorSwatch;
	MaterialEditorGui.updateActiveMaterial(%propName, %color);
}
//------------------------------------------------------------------------------
