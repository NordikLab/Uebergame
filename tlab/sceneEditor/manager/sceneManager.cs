//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================



function SceneAddSimGroupButton::onClick( %this ) {
	devLog("SceneAddSimGroupButton::onClick??");
	EWorldEditor.addSimGroup();
}
function SceneAddSimGroupButton::onDefaultClick( %this ) {
	devLog("SceneAddSimGroupButton::onDefaultClick??");
	EWorldEditor.addSimGroup();
}

function SceneAddSimGroupButton::onCtrlClick( %this ) {
	devLog("SceneAddSimGroupButton::onCtrlClick??");
	EWorldEditor.addSimGroup( true );
}