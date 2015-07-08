//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabGameMap.bindCmd(keyboard, "ctrl 0", "GameLabGui.toggleMe();");
$LabGameMap.bindCmd(keyboard, "ctrl i", "GameLabGui.toggleCursor();");
$LabGameMap.bindCmd(keyboard, "ctrl m", "Lab.toggleGameDlg(\"SceneEditorDialogs\",\"AmbientManager\");");

//==============================================================================
function GameLabGui::onWake( %this ) {	
	%menu = GameLabGui-->dialogMenu;
	if (isObject(Lab.currentGameDlg))
		%menu.setSelected(Lab.currentGameDlg.getId(),false);
	else
		%menu.setSelected(0,false);
	
}

//------------------------------------------------------------------------------
//==============================================================================
function GameLabGui::onSleep( %this ) {	
	hideCursor();
	$LabGameMap.push();
	//Canvas.schedule(300,"hideCursor");
	//Canvas.schedule(300,"cursorOff");
}

//------------------------------------------------------------------------------
	

//==============================================================================
function GameLabGui::toggleMe( %this ) {
	toggleDlg(GameLabGui);
	//if (%this.isVisible())
	//	GameLabGui.schedule(300,"toggleCursor",true);
}

//------------------------------------------------------------------------------

//==============================================================================
function GameLabGuiDialogMenu::onSelect( %this,%id,%text ) {	
	if (%id == 0){
		GameLabGui.reset();
		return;
	}
	GameLabGui.showDlg(%id);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// GameLabGui -> Show Dialogs in game
//==============================================================================

//==============================================================================
function GameLabGui::getDlgObject( %this,%dlg,%child ) {
	%dlgCtrl = %dlg;
	if (%child !$= "" && isObject(%dlg)){
		%dlgCtrl = %dlg.findObjectByInternalName(%child);
		
		//If not to default parent try with GameLabGui
		if (!isObject(%dlgCtrl))
			%dlgCtrl = %this.findObjectByInternalName(%child);
	}
	return %dlgCtrl;
}
//------------------------------------------------------------------------------
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function Lab::toggleGameDlg(%this,%dlg,%child) {
	%dlgCtrl = GameLabGui.getDlgObject(%dlg,%child );		
	if (!isObject(%dlgCtrl))
		return;	
	
	
	if (Lab.currentGameDlg $= %dlgCtrl)
		GameLabGui.hideDlg(%dlg,%child);
	else	
		GameLabGui.showDlg(%dlg,%child);
	return;
	if (isObject(Lab.currentGameDlg)){
		Lab.currentGameDlg.editorParent.add(Lab.currentGameDlg);
		
	}
	if (%dlgCtrl.isMethod("onActivated"))
		%dlgCtrl.onActivated();
	%dlgCtrl.editorParent = %dlgCtrl.parentGroup;
	
	%pluginName = %dlgCtrl.editorParent.pluginObj.displayName;
	GameLabGui-->dialogTitle.text = %pluginName SPC "\c1->\c2" SPC %dlgCtrl.internalName;
	GameLabGui.add(%dlgCtrl);
	Lab.currentGameDlg = %dlgCtrl;
	%dlgCtrl.visible = 1;
	pushDlg(GameLabGui);
	//GameLabGui.schedule(300,"toggleCursor",true);
}
//------------------------------------------------------------------------------


//==============================================================================
function GameLabGui::showDlg( %this,%dlg,%child ) {	
	%dlgCtrl = %this.getDlgObject(%dlg,%child );	
	//If not to default parent try with GameLabGui
	if (!isObject(%dlgCtrl)){
		warnLog("Trying to show invalid GameLab Dialog:",%dlg,%child);
		return;
	}	
	
	if (isObject(Lab.currentGameDlg)){
		Lab.currentGameDlg.editorParent.add(Lab.currentGameDlg);		
	}
	
	if (%dlgCtrl.isMethod("onActivated"))
		%dlgCtrl.onActivated();
	%dlgCtrl.editorParent = %dlgCtrl.parentGroup;
	
	%pluginName = %dlgCtrl.editorParent.pluginObj.displayName;
	GameLabGui-->dialogTitle.text = %pluginName SPC "\c1->\c2" SPC %dlgCtrl.internalName;
	GameLabGui.add(%dlgCtrl);
	Lab.currentGameDlg = %dlgCtrl;
	%dlgCtrl.visible = 1;
	
	pushDlg(GameLabGui);
	//GameLabGui.schedule(300,"toggleCursor",true);
GameLabGui.clearChildResponder();
}
//------------------------------------------------------------------------------
//==============================================================================
function GameLabGui::hideDlg( %this,%dlg,%child ) {
	%dlgCtrl.editorParent.add(%dlgCtrl);
		%dlgCtrl.visible = 0;
		popDlg(GameLabGui);
		Lab.currentGameDlg = "";
	
}
//------------------------------------------------------------------------------
//==============================================================================
function GameLabGui::closeAll( %this ) {
	Lab.currentGameDlg = "";
	foreach(%ctrl in 	%this){
		if (%ctrl.internalName $= "infoContainer")
			continue;
		if (isObject(%ctrl.editorParent)){
			%ctrl.editorParent.add(%ctrl);
		}
		hide(%ctrl);
	}
	Canvas.cursorOff();
	popDlg(%this);
}
//------------------------------------------------------------------------------


//==============================================================================
function GameLabGui::toggleCursor( %this,%show ) {
	%button = %this-->toggleCursorButton;
	%button.text = "Toggle cursor (ctrl + i)";	
	%button.active = true;

	if (Canvas.isCursorOn() && !%show){		
		hideCursor();		
	}
	else{
		showCursor();	
	}
	
	if (!Canvas.isCursorOn()){
		%this.clearChildResponder();		
	}
		
	
	$HudCtrl.makeFirstResponder(!Canvas.isCursorOn());
	GameLabGui.makeFirstResponder(Canvas.isCursorOn());
}
//------------------------------------------------------------------------------

//==============================================================================
function GameLabGui::setNoCursor( %this,%noCursor ) {
	GameLabGui.noCursor = %noCursor;

}
//------------------------------------------------------------------------------
//==============================================================================
function GameLabGui::reset( %this ) {
	//Make sure all Dlg in GameLabGui have been returned to editor
	foreach(%ctrl in GameLabGui){
		if (%ctrl.internalName $= "infoContainer")
			continue;
		if (isObject(%ctrl.editorParent)){
			%ctrl.editorParent.add(%ctrl);
		}
		hide(%ctrl);
	}
	Lab.currentGameDlg = "";

}