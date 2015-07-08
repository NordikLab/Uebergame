//==============================================================================
// TorqueLab -> Terrain Paint Generator System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Terrain Paint Generator Globals Define
//------------------------------------------------------------------------------
$TPG_CoverageMax = 80;
$TPG_ValidateFields = "heightMin heightMax slopeMin slopeMax coverage";
//------------------------------------------------------------------------------

//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::init(%this) {
	if ( !isObject( TPG_LayerGroup ) ) {
		new SimGroup( TPG_LayerGroup  );
	}

	TPG_StackLayers.clear();
	TPG_LayerFolderMenu.clear();
	TPG_LayerFolderMenu.add("TerrainEditor",0);
	TPG_LayerFolderMenu.add("CurrentLevel",1);
	TPG_LayerFolderMenu.setSelected(0);
	$TerrainPaintGeneratorGui_Initialized = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::exec(%this) {
	exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.cs");
	exec("tlab/terrainEditor/scripts/paintGenerator.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
// Make sure the stack and group have valid object and are synced
function TPG::checkLayersStackGroup(%this) {
	foreach(%pill in TPG_StackLayers) {
		if(isObject(%pill.layerObj))
			%addList = strAddWord(%addList,%pill.layerObj);
		else
			%badPill = strAddWord(%badPill,%pill);
	}

	foreach(%layer in TPG_LayerGroup) {
		%inStack = strFind(%addList,%layer.getId());

		if (!strFind(%addList,%layer.getId()))
			%deleteLayers = strAddWord(%deleteLayers,%layer);
	}

	foreach$(%pill in %badPill)
		%pill.delete();

	foreach$(%layer in %deleteLayers)
		%layer.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Layer Validation Functions
//==============================================================================
function TPG::reportValidationErrors(%this) {
	if ($TPG_LayerValidationErrors !$="")
		LabMsgOk(%layer.internalName SPC "validation failed",$TPG_LayerValidationErrors);

	$TPG_LayerValidationErrors = "";
}
//==============================================================================
// Validate all setting for a single layer
function TPG::validateLayerSettings(%this,%layer,%doPostValidation) {
	%validated = false;
	%layer.failedSettings = "";
	$TPG_LayerValidationErrors = "";
	%this.validateLayerSetting("heightMin",%layer,true);
	%this.validateLayerSetting("heightMax",%layer,true);
	%this.validateLayerSetting("slopeMin",%layer,true);
	%this.validateLayerSetting("slopeMax",%layer,true);
	%this.validateLayerSetting("coverage",%layer,true);
	%this.reportValidationErrors();

	if (%doPostValidation && %layer.failedSettings $= "")
		%validated = %this.postLayerValidation(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a single setting for a layer
function TPG::validateLayerSetting(%this,%setting,%layer,%fullValidation) {
	if (!isObject(%layer)) {
		%error = "Invalid layer submitted for validation. There's no such object:\c2" SPC %layer;
		warnLog(%error,"Setting:",%setting);
		return;
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	}

	%layer.isValidated = false;
	%layer.setFieldValue(%setting,"");
	eval("%ctrl = %layer.pill-->"@%setting@";");

	if (!isObject(%ctrl)) {
		%error = %layer.internalName SPC "layer doesn't have a valid field for:" SPC %setting @ ". Please delete it and report the issue.";
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	} else {
		%value = %ctrl.getValue();

		if (!strIsNumeric(%value)) {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}

		if (%value $= "") {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}
	}

	if (%failed) {
		%layer.failedSettings = strAddWord(%layer.failedSettings,%setting);

		if (!%fullValidation)
			%this.reportValidationErrors();

		return;
	}

	switch$(%setting) {
	case "heightMin":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_MinHeight)
				%value = $TPG_MinHeight;
		}

	case "heightMax":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_MaxHeight)
				%value = $TPG_MaxHeight;
		}

	case "slopeMin":
		if (%value < 0)
			%value = "0";

	case "slopeMax":
		if (%value > 90)
			%value = "90";

	case "coverage":
		%value = mClamp(%value,"0","99");
	}

	%ctrl.setValue(%value);
	%layer.setFieldValue(%setting,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
// Post validate the settings of a layer globally
function TPG::postLayerValidation(%this,%layer) {
	foreach$(%field in $TPG_ValidateFields) {
		if (%layer.getFieldValue(%field) $="") {
			%error = %setting SPC "is not validated";
			%errors = strAddRecord(%errors,%error);
		}
	}

	if (%errors !$="") {
		LabMsgOk("Layer post validation failed",%errors);
		return false;
	}

	if (%layer.getFieldValue("heightMin") > %layer.getFieldValue("heightMax")) {
		%layer.setFieldValue("heightMin",%layer.getFieldValue("heightMax"));
		%report = "Height min was higher than Height max. The height minimum is changed to fit the maximum.";
		%reports = strAddRecord(%reports,%report);
	}

	if (%reports !$="") {
		LabMsgOk("Layer post validation succeed with warnings",%reports);
	}

	%layer.fieldsValidated = true;
	%layer.isValidated = true;
	return true;
}
//------------------------------------------------------------------------------

//==============================================================================
// Terrain Paint Generator - Terrain Paint Generation Functions
//==============================================================================

//==============================================================================
// Generate all the layers in the layer group
function TPG::generateLayerGroup(%this) {
	//First make sure layers data is right
	TPG.checkLayersStackGroup();

	foreach(%layer in TPG_LayerGroup) {
		%layer.inactive = false;

		if ( !%layer.activeCtrl.isStateOn()) {
			%layer.inactive = true;
			devLog("Skipping inactive layer:",%layer.matInternalName);
			continue;
		}

		%layer.fieldsValidated = false;
		%validated = %this.validateLayerSettings(%layer,true);

		if (%layer.failedSettings !$= "") {
			%validationgFailed = true;
			%badLayers = strAddRecord(%badLayers,%layer.internalName SPC "Bad fields:" SPC %layer.failedSettings);
		}
	}

	if (%validationgFailed) {
		if (%badLayers !$="" ) {
			LabMsgOk("Generation aborted!","Some layers have fail the validation. Here's the list:\c2" SPC  %badLayers @ ". Please fix them before attempting generation again.");
		}

		return;
	}

	show(TPG_GenerateProgressWindow);
	TPG_GenerateLayerStack.clear();

	foreach(%layer in TPG_LayerGroup) {
		if (%layer.inactive)
			continue;

		%pill = cloneObject(TPG_GenerateInfoPill,"",%layer.internalName,TPG_GenerateLayerStack);
		%pill-->info.text = %layerId++ @ "-> \c1" @ %layer.internalName;
	}

	if (%layerId < 1) {
		LabMsgOk("No active layers","There's no active layers to generate terrain materials, operation aborted");
		hide(TPG_GenerateProgressWindow);
		return;
	}

	LabMsgYesNo("Early development feature","The terrain pain generator is still in early development and can cause the engine to freeze. "@
					"We recommend you to save your work before proceeding with automated painting. Are you sure you want to start the painting process?","TPG.schedule(1000,\"startGeneration\");");
	//%this.schedule(1000,"startGeneration");
}
//------------------------------------------------------------------------------
//==============================================================================
// Start the generation process now that everything is validated
function TPG::startGeneration(%this) {
	if ($TPG_Generating) return;

	TPG.generationStartTime = $Sim::Time;
	$TPG_Generating = true;
	TPG.generatorSteps = "";

	foreach(%layer in TPG_LayerGroup) {
		if (%layer.inactive)
			continue;

		if ($TPG_StepGeneration)
			TPG.generatorSteps = strAddWord(TPG.generatorSteps,%layer.getId());
		else
			%this.generateLayer(%layer);
	}

	$TPG_Generating = false;
	TPG_GenerateProgressWindow.setVisible(false);

	if ($TPG_StepGeneration)
		%this.doGenerateLayerStep(true);
}
//------------------------------------------------------------------------------

function TPG::doGenerateLayerStep(%this,%start) {
	%layer = getWord(TPG.generatorSteps,0);

	if (%layer $= "")
		return;

	TPG.generatorSteps = removeWord(TPG.generatorSteps,0);
	%this.schedule(500,"generateLayer",%layer,true);
}
//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayer(%this,%layer,%stepMode) {
	ETerrainEditor.paintIndex = %layer.matIndex;
	%heightMin = %layer.getFieldValue("heightMin");
	%heightMax = %layer.getFieldValue("heightMax");
	%slopeMin = %layer.getFieldValue("slopeMin");
	%slopeMax = %layer.getFieldValue("slopeMax");
	%coverage = %layer.getFieldValue("coverage");

	if (!%layer.useTerrainHeight) {
		%heightMin += $Lab_TerrainZ;
		%heightMax += $Lab_TerrainZ;
	}

	if (%stepMode)
		info("Step Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin, %heightMax, %slopeMin, %slopeMax,"Coverage",%coverage);
	else
		info("Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin, %heightMax, %slopeMin, %slopeMax,"Coverage",%coverage);

	ETerrainEditor.autoMaterialLayer(%heightMin, %heightMax, %slopeMin, %slopeMax,%coverage);
	%this.generateLayerCompleted(%layer,%stepMode);
}
//------------------------------------------------------------------------------

//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayerCompleted(%this,%layer,%stepMode) {
	TPG.generationTotalTime = $Sim::Time - TPG.generationStartTime;

	if (%stepMode && getWord(TPG.generatorSteps,0) !$= "")
		LabMsgYesNo(%layer.matInternalName SPC "step completed","Do you want to proceed with next step:" SPC getWord(TPG.generatorSteps,0).matInternalName SPC "?","TPG.doGenerateLayerStep();");
	else
		LabMsgOk("Terrain painting completed","All layers have been successfully painted over the terrain. It took" SPC TPG.generationTotalTime SPC "to complete the process.");
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Saving and Loading layer group
//==============================================================================
$TPG_FilerFilter = "Lab Painter Layers|*.painter.cs";

//==============================================================================
// Save all the layers to a file
function TPG::saveLayerGroup(%this) {
	if (%this.folderBase $= "TerrainEditor")
		TPG.layerPath = "tlab/terrainEditor/painterLayers/default.painter.cs";
	else if (%this.folderBase $= "CurrentLevel")
		TPG.layerPath = MissionGroup.getFilename();

	getSaveFilename($TPG_FilerFilter, "TPG.saveLayerGroupHandler", TPG.layerPath);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for the saved file
function TPG::saveLayerGroupHandler( %this,%filename ) {
	%filename = makeRelativePath( %filename, getMainDotCsDir() );

	if(strStr(%filename, ".") == -1)
		%filename = %filename @ ".painter.cs";

	// Create a file object for writing
	%fileWrite = new FileObject();
	%fileWrite.OpenForWrite(%filename);
	%fileWrite.writeObject(TPG_LayerGroup);
	%fileWrite.close();
	%fileWrite.delete();
	info("Terrain painter layers save to file:",%filename);
}
//------------------------------------------------------------------------------
//==============================================================================
// Load all the layers saved in a file
function TPG::loadLayerGroup(%this) {
	if (%this.folderBase $= "TerrainEditor")
		TPG.layerPath = "tlab/terrainEditor/painterLayers/default.painter.cs";
	else if (%this.folderBase $= "CurrentLevel")
		TPG.layerPath = MissionGroup.getFilename();

	getLoadFilename($TPG_FilerFilter, "TPG.loadLayerGroupHandler",TPG.layerPath);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for file loader
function TPG::loadLayerGroupHandler(%this,%filename) {
	if ( isScriptFile( %filename ) ) {
		TerrainPaintGeneratorGui.deleteLayerGroup();
		TPG_LayerGroup.delete();
		%filename = expandFilename(%filename);
		exec(%filename);
	}

	foreach(%layer in TPG_LayerGroup) {
		%layer.pill = "";
		TerrainPaintGeneratorGui.addLayerPill(%layer,true);
	}

	if (!isObject(TPG_LayerGroup))
		new SimGroup( TPG_LayerGroup );
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the layer group file folder base (Editor or Level)
function TPG::setLayersFolderBase(%this,%menu) {
	TPG.folderBase = %menu.getText();
}
//------------------------------------------------------------------------------