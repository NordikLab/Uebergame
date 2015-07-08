//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
$AssetsLab_CategoryList = "Map models" TAB "Trees" TAB "Plants/Flowers" TAB "Grass";
$AssetsLab_PathList = "art/models/" TAB "art/models/" TAB "art/shapes/";
$AssetsLab_DisplayList = "List" TAB "Small icons" TAB "Large icons";
//==============================================================================
//SEP_AssetPage.setMeshFolderDepth($ThisControl);
function SceneEditorPlugin::initAssets( %this ) {
	if (!isObject(AssetsLab))
		$AssetsLab = newScriptObject("AssetsLab");
	
	%this.getAssetFolders();
	//Fill the menus
	AssetCategoryMenu.clear();
	AssetCategoryMenu.add("All",0);

	for(%i=0; %i<getFieldCount($AssetsLab_CategoryList); %i++) {
		%item = getField($AssetsLab_CategoryList,%i);
		AssetCategoryMenu.add(%item,%i+1);
	}

	//Fill the menus
	AssetDisplayMenu.clear();

	for(%i=0; %i<getFieldCount($AssetsLab_DisplayList); %i++) {
		%item = getField($AssetsLab_DisplayList,%i);
		AssetDisplayMenu.add(%item,%i);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//SEP_AssetPage.setMeshFolderDepth($ThisControl);
function AssetsLab::onMenuSelect( %this,%menu ) {
	%menuType = %menu.internalName;

	switch$(%menuType) {
	case "SearchPath":
		%path = %menu.getText();		
		AssetsLab.navigateAssets(getField(%path,1));

	case "Category":
		%category = %menu.getText();
		%folders = SceneEditorPlugin.getAssetsInCategory(%category);		

	case "Display":
		%displayMode = %menu.getText();		
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneEditorPlugin.getAssetFolders();
function SceneEditorPlugin::getAssetFolders( %this ) {
	//Fill the menus
	AssetPathMenu.clear();
	AssetPathMenu.add("All",0);
		AssetCategoryMenu.clear();
	AssetCategoryMenu.add("All",0);
	AssetCategoryMenu.currentId = 0;
	SEP_AssetManager.assetCategories = "";
	%next = true;
	%i = 0;
	while(%next){
		%data = %this.getCfg("AssetData"@%i);		
		if (%data $= ""){
			%next = false;
			SceneEditorPlugin.nextAssetId = %i;
			continue;
		}
		%folder = getField(%data,0);
		%name = getField(%data,1);
		%cat = getField(%data,2);		
		%this.updateCategoryList(%cat);
		AssetPathMenu.add(%name TAB %folder,%i+1);
		%i++;		
	}
	AssetPathMenu.setSelected(0);
	AssetCategoryMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneEditorPlugin.addAssetFolder("Test/Folder","New Asset 1","Rocks");
function SceneEditorPlugin::addAssetFolder( %this,%folder,%name,%cat ) {
	%id = %this.nextAssetId;
	%this.setCfg("AssetData"@%id,%folder @"\t"@%name@"\t"@%cat);
	%this.getAssetFolders();
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneEditorPlugin.addAssetFolder("Test/Folder","New Asset 1","Rocks");
function SceneEditorPlugin::updateCategoryList( %this,%cat ) {
	%isInMenu = AssetCategoryMenu.findText(%cat);
	if (%isInMenu > 0)
		return;
	
	AssetCategoryMenu.add(%cat, AssetCategoryMenu.currentId++);
	SEP_AssetManager.assetCategories = strAddField(SEP_AssetManager.assetCategories,%cat);
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneEditorPlugin.getAssetFolders();
function SceneEditorPlugin::getAssetsInCategory( %this,%findCat ) {
	//Fill the menus
	%folderList = "";	
	%next = true;
	%i = 0;
	while(%next){
		%data = %this.getCfg("AssetData"@%i);		
		if (%data $= ""){
			%next = false;
			SceneEditorPlugin.nextAssetId = %i;
			continue;
		}
		%folder = getField(%data,0);
		%name = getField(%data,1);
		%cat = getField(%data,2);
		if (%cat $= %findCat)
			%folderList = strAddField(%folderList,%folder);
		
		%i++;		
	}
	
	return %folderList;
}
//------------------------------------------------------------------------------