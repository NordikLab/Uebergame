//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//------------------------------------------------------------------------------
//Create Lab Editor Core Objects
exec("tlab/EditorLab/initLabEditor.cs");
exec("tlab/EditorLab/commonSettings.cs");

//------------------------------------------------------------------------------
//Load GameLab system (In-Game Editor)
function tlabExecCore( %loadGui ) {
	if(%loadGui) {
		
		exec("tlab/EditorLab/gui/core/EditorLoadingGui.gui");
		exec("tlab/EditorLab/gui/core/simViewDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/colorPicker.ed.gui");
		exec("tlab/EditorLab/gui/core/scriptEditorDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/uvEditor.ed.gui");
	}

	exec("tlab/EditorLab/gui/core/fileDialogBase.ed.cs");
	exec("tlab/EditorLab/gui/core/openFileDialog.ed.cs");
	exec("tlab/EditorLab/gui/core/saveFileDialog.ed.cs");
	exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.cs");
}
tlabExecCore($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecCore");


//------------------------------------------------------------------------------
//Load the Editor Menubar Scripts

function tlabExecMenubar( %loadGui ) {
	exec("tlab/EditorLab/menubar/manageMenu.cs");
	exec("tlab/EditorLab/menubar/defineMenus.cs");
	exec("tlab/EditorLab/menubar/menuHandlers.cs");
	exec("tlab/EditorLab/menubar/labstyle/menubarScript.cs");
	exec("tlab/EditorLab/menubar/labstyle/buildWorldMenu.cs");
	exec("tlab/EditorLab/menubar/native/buildNativeMenu.cs");
	exec("tlab/EditorLab/menubar/native/lightingMenu.cs");
}
tlabExecMenubar($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecMenubar");
//------------------------------------------------------------------------------
//Load the Editor Main Scripts
function tlabExecScripts( %loadGui ) {
	execPattern("tlab/EditorLab/scripts/*.cs");
}
tlabExecScripts($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecScripts");
//------------------------------------------------------------------------------
//Load the Editor Main Gui
function tlabExecHelpers(%loadGui  ) {
	execPattern("tlab/EditorLab/helpers/*.cs");
}
tlabExecHelpers($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecHelpers");
//------------------------------------------------------------------------------
//Load the Editor Main Gui

function tlabExecScene(%loadGui ) {
	execPattern("tlab/EditorLab/scene/*.cs");
}
tlabExecScene($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecScene");
//------------------------------------------------------------------------------
//Load the LabGui (Cleaned EditorGui files)
function tlabExecEditor(%loadGui ) {
	if (%loadGui) {
		exec("tlab/EditorLab/editor/gui/EditorGui.gui");
		exec("tlab/EditorLab/gui/cursors.cs");
		
	}

	exec("tlab/EditorLab/editor/EditorOpen.cs");
	exec("tlab/EditorLab/editor/EditorClose.cs");
	exec("tlab/EditorLab/editor/EditorScript.cs");
	exec("tlab/EditorLab/editor/manageGui.cs");
	exec("tlab/EditorLab/editor/generalFunctions.cs");
	execPattern("tlab/EditorLab/editor/worldEditor/*.cs");
	execPattern("tlab/EditorLab/editor/features/*.cs");
}
tlabExecEditor($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecEditor");
//------------------------------------------------------------------------------
//Load the Tools scripts (Toolbar and special functions)
function tlabExecToolbar(%loadGui ) {
	execPattern("tlab/EditorLab/toolbar/*.cs");
}
tlabExecToolbar($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecToolbar");

//------------------------------------------------------------------------------
//Load the LabGui Ctrl (Cleaned EditorGui files)
function tlabExecGui(%loadGui ) {
	if (%loadGui) {
		exec("tlab/EditorLab/gui/CtrlCameraSpeedDropdown.gui");
		exec("tlab/EditorLab/gui/CtrlSnapSizeSlider.gui");
		exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.gui");
		exec("tlab/EditorLab/gui/DlgManageSFXParameters.gui" );
		exec("tlab/EditorLab/gui/LabWidgetsGui.gui");
		exec("tlab/EditorLab/gui/DlgAddFMODProject.gui");
		exec("tlab/EditorLab/gui/DlgEditorChooseLevel.gui");
		exec("tlab/EditorLab/gui/DlgGenericPrompt.gui");
		exec("tlab/EditorLab/gui/DlgObjectBuilder.gui");
		exec("tlab/EditorLab/gui/DlgTimeAdjust.gui");
		exec( "tlab/EditorLab/gui/Settings/LabMissionSettingsDlg.gui" );
		exec("tlab/EditorLab/gui/MaterialSelector/MaterialSelectorDlg.gui");
	}

	exec("tlab/EditorLab/gui/messageBoxes/ToolsMsgBox.cs");
	exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
	exec("tlab/EditorLab/gui/DlgManageSFXParameters.cs" );
	exec("tlab/EditorLab/gui/DlgAddFMODProject.cs");
	exec("tlab/EditorLab/gui/DlgEditorChooseLevel.cs");
	exec( "tlab/EditorLab/gui/Settings/LabMissionSettingsDlg.cs" );
	execPattern("tlab/EditorLab/gui/MaterialSelector/*.cs");
}
tlabExecGui($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecGui");
//------------------------------------------------------------------------------
//Old Settings Dialog for temporary references
function tlabExecDialogs(%loadGui ) {
	if (%loadGui) {
		exec("tlab/EditorLab/gui/dialogs/ESelectObjects.gui");
		exec("tlab/EditorLab/gui/dialogs/EManageBookmarks.gui");
		exec("tlab/EditorLab/gui/dialogs/ESceneManager.gui");
		exec("tlab/EditorLab/gui/dialogs/ColladaImportDlg.gui");
		exec("tlab/EditorLab/gui/dialogs/ColladaImportProgress.gui");
		exec("tlab/EditorLab/gui/dialogs/GameLabGui.gui");
		execPattern("tlab/EditorLab/gui/tools/*.gui");
	}

	exec("tlab/EditorLab/gui/dialogs/EObjectSelection.cs");
	exec("tlab/EditorLab/gui/dialogs/ESelectObjects.cs");
	exec("tlab/EditorLab/gui/dialogs/EManageBookmarks.cs");
	exec("tlab/EditorLab/gui/dialogs/ESceneManager.cs");
	exec("tlab/EditorLab/gui/commonDialogs.cs");
	exec("tlab/EditorLab/gui/dialogs/ColladaImportDlg.cs");
	exec("tlab/EditorLab/gui/dialogs/GameLabGui.cs");
	execPattern("tlab/EditorLab/gui/tools/*.cs");
}
tlabExecDialogs($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecDialogs");
function execTools( ) {
	execPattern("tlab/EditorLab/gui/tools/*.cs");
}

//------------------------------------------------------------------------------
// TorqueLab Params scripts
function tlabExecParams(%loadGui ) {
	if (%loadGui)
		exec("tlab/EditorLab/params/LabParamsDlg.gui");

	exec("tlab/EditorLab/params/LabParamsDlg.cs");
	exec("tlab/EditorLab/params/initParams.cs");
	exec("tlab/EditorLab/params/paramsSystem.cs");
	exec("tlab/EditorLab/params/configObject.cs");
	exec("tlab/EditorLab/params/configSystem.cs");
}
tlabExecParams($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecParams");

//------------------------------------------------------------------------------
// TorqueLab common scrits
function tlabExecCommon(%loadGui ) {
	exec("tlab/EditorLab/common/physicSystem.cs");
	exec("tlab/EditorLab/common/creatorHelpers.cs");
	exec("tlab/EditorLab/common/defineFields.cs");
	exec("tlab/EditorLab/common/areaEditor.cs");
	exec("tlab/EditorLab/common/specialRender.cs");
	exec("tlab/EditorLab/common/undoManager.cs");
	exec("tlab/EditorLab/common/missionHelpers.cs");
}
tlabExecCommon($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecCommon");

//------------------------------------------------------------------------------
// TorqueLab Plugins Scripts
function tlabExecPlugin(%loadGui ) {
	execPattern("tlab/EditorLab/plugin/*.cs");
}
tlabExecPlugin($LabExecGui);
%execAll = strAddWord(%execAll,"tlabExecPlugin");
$TLabExecAllList = %execAll;
function tlabExec( ) {
	foreach$(%func in $TLabExecAllList) {
		devLog("Calling exec function:",%func);
		eval(%func@"();");
	}
}
