//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================

function SceneEditorTools::setMeshRootFolder( %this,%folder ) {
	devLog("SceneEditorTools::setMeshRootFolder:",%folder);
	%this.meshRootFolder = %folder;
	%this-->meshRootFolder.setText(%folder);
}
// SceneEditorTools.validateMeshRootFolder($ThisControl);
function SceneEditorTools::validateMeshRootFolder( %this,%ctrl ) {
	devLog("SceneEditorTools::setMeshRootFolder:",%ctrl.getValue());
}
function SceneEditorTools::getMeshRootFolder( %this ) {
	getFolderName("","SceneEditorTools.setMeshRootFolder","art/");
}