//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Add new asset group
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::openCreateNewAssetDlg( %this ) {	
	SceneEditorDialogs.showDlg("AssetManager");
	%this.resetNewAssetFields();
	SEP_AssetManagerBook.selectPage(1);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::resetNewAssetFields( %this ) {	
	SEP_AssetManager_NewAssetPage-->newAssetFolder.setText("[Select the root asset folder]");
	SEP_AssetManager_NewAssetPage-->newAssetName.setText("[Use root folder name]");
	SEP_AssetManager_NewAssetPage-->newCategory.setText("[Type a new category to add the asset group]");
	
	%catMenu = SEP_AssetManager_NewAssetPage-->CategoryMenu;
	%id = 0;
	%catMenu.add("New categorie",%id);
	foreach$(%cat in SEP_AssetManager.assetCategories){
		%catMenu.add(%cat,%id++);
	}
	%catMenu.setText("Choose categorie");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::createNewAssetGroup( %this ) {	
	%folder = SEP_AssetManager_NewAssetPage-->newAssetFolder.getText();
	%name = SEP_AssetManager_NewAssetPage-->newAssetName.getText();
	%cat =  SEP_AssetManager_NewAssetPage-->CategoryMenu.getText();
	%newCat = SEP_AssetManager_NewAssetPage-->newCategory.getText();
	if (strFind(%newCat,"[") || %newCat $= "" )
		%category = %cat;
	else
	 	%category = %newCat;
	 	
	SceneEditorPlugin.addAssetFolder(%folder,%name,%category);
	devLog("Create new asset group name:",%name,"Folder:",%folder,"Cat",%category);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::cancelNewAsset( %this ) {	
	%this.resetNewAssetFields();
	SceneEditorDialogs.hideDlg("AssetManager");
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::selectRootFolder( %this,%field ) {	
	getFolderName("*.*|*.*", "SEP_AssetManager.applyRootFolder", %current);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::applyRootFolder( %this,%file ) {
	SEP_AssetManager_NewAssetPage-->newAssetFolder.setText(%file);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::validateNewCategory( %this,%category ) {
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::validateNewAssetName( %this,%name ) {
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AssetManager::validateNewAssetFolder( %this,%name ) {
	
}
//------------------------------------------------------------------------------