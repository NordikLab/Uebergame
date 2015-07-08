//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//------------------------------------------------------------------------------
//Create Lab Editor Core Objects
exec("tlab/core/initLabEditor.cs");
exec("tlab/core/commonSettings.cs");

//------------------------------------------------------------------------------
//Load GameLab system (In-Game Editor)
function tlabExecCore( %loadGui ) {
	if(%loadGui) {
		execPattern("tlab/core/*.gui");			
	}

	execPattern("tlab/core/plugin/*.cs");
	execPattern("tlab/core/helpers/*.cs");
	execPattern("tlab/core/scripts/*.cs");
	execPattern("tlab/core/common/*.cs");
	execPattern("tlab/core/params/*.cs");

	
	
	
	exec("tlab/core/classBase/popupMenu.cs");
	exec("tlab/core/classBase/guiInspector.cs");
	exec("tlab/core/classBase/guiTreeViewCtrl.cs");
	exec("tlab/core/classBase/guiSwatchButtonCtrl.cs");
}
tlabExecCore(!$LabGuiExeced);
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
tlabExecMenubar(!$LabGuiExeced);
%execAll = strAddWord(%execAll,"tlabExecMenubar");
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//Load the Editor Main Gui

function tlabExecScene(%loadGui ) {
	execPattern("tlab/EditorLab/scene/*.cs");
}
tlabExecScene(!$LabGuiExeced);
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
tlabExecEditor(!$LabGuiExeced);
%execAll = strAddWord(%execAll,"tlabExecEditor");
//------------------------------------------------------------------------------
//Load the Tools scripts (Toolbar and special functions)
function tlabExecToolbar(%loadGui ) {
	execPattern("tlab/EditorLab/toolbar/*.cs");
}
tlabExecToolbar(!$LabGuiExeced);
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
		
		exec("tlab/EditorLab/gui/core/EditorLoadingGui.gui");
		exec("tlab/EditorLab/gui/core/simViewDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/colorPicker.ed.gui");
		exec("tlab/EditorLab/gui/core/scriptEditorDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/uvEditor.ed.gui");
		exec("tlab/EditorLab/gui/GameLabGui.gui");
	}

	exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
	exec("tlab/EditorLab/gui/DlgManageSFXParameters.cs" );
	exec("tlab/EditorLab/gui/DlgAddFMODProject.cs");
	exec("tlab/EditorLab/gui/DlgEditorChooseLevel.cs");
	exec( "tlab/EditorLab/gui/Settings/LabMissionSettingsDlg.cs" );
	execPattern("tlab/EditorLab/gui/MaterialSelector/*.cs");
	
	exec("tlab/EditorLab/gui/core/fileDialogBase.ed.cs");
	exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.cs");
	exec("tlab/EditorLab/gui/GameLabGui.cs");
}
tlabExecGui(!$LabGuiExeced);
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
		execPattern("tlab/EditorLab/gui/tools/*.gui");
		execPattern("tlab/EditorLab/gui/debugTools/*.gui");
	}

	exec("tlab/EditorLab/gui/dialogs/EObjectSelection.cs");
	exec("tlab/EditorLab/gui/dialogs/ESelectObjects.cs");
	exec("tlab/EditorLab/gui/dialogs/EManageBookmarks.cs");
	exec("tlab/EditorLab/gui/dialogs/ESceneManager.cs");
	exec("tlab/EditorLab/gui/commonDialogs.cs");
	exec("tlab/EditorLab/gui/dialogs/ColladaImportDlg.cs");
	execPattern("tlab/EditorLab/gui/tools/*.cs");
	execPattern("tlab/EditorLab/gui/debugTools/*.cs");
}
tlabExecDialogs(!$LabGuiExeced);
%execAll = strAddWord(%execAll,"tlabExecDialogs");
function execTools( ) {
	execPattern("tlab/EditorLab/gui/tools/*.cs");
}




//------------------------------------------------------------------------------

$TLabExecAllList = %execAll;
function tlabExec( ) {
	foreach$(%func in $TLabExecAllList) {		
		eval(%func@"();");
	}
	info("All core TorqueLab scripts executed.");
}
