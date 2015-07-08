//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$SEP_AmbientBook_PageId = 0;
$SEP_PrecipitationBook_PageId = 0;
$SEP_ScatterSkyBook_PageId = 0;
$SEP_CloudsBook_PageId = 0;
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorDialogs::onActivated( %this ) {		
	if (SceneEditorDialogs.selectedPage $= "")
		SceneEditorDialogs.selectedPage = "0";

}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initDialog( %this ) {
	logd("SEP_AmbientManager::initDialog(%this)");

	if (!isObject(SEP_AmbientManager_PM))
		new PersistenceManager(SEP_AmbientManager_PM);
		
	SEP_AmbientBook.selectPage($SEP_AmbientBook_PageId);

	SEP_ScatterSkyManager.buildParams();
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onActivated( %this ) {
	logd("SEP_AmbientManager::onActivated(%this)");
	SEP_AmbientBook.selectPage($SEP_AmbientBook_PageId);
	SEP_ScatterSkyBook.selectPage($SEP_ScatterSkyBook_PageId);
	SEP_PrecipitationBook.selectPage($SEP_PrecipitationBook_PageId);
	SEP_CloudsBook.selectPage($SEP_CloudsBook_PageId);
	SEP_AmbientManager.buildBasicCloudsParams();
	SEP_AmbientManager.initBasicCloudsData();
	SEP_AmbientManager.buildCloudLayerParams();
	SEP_AmbientManager.initCloudLayerData();
	SEP_ScatterSkyManager.initData();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onWake( %this ) {
	SEP_PrecipitationManager.initData();
	SEP_PrecipitationBook.selectPage(0);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onSleep( %this ) {
	logd("SEP_AmbientManager::onSleep( %this )");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientBook::onTabSelected( %this,%text,%index ) {
	logd("SEP_AmbientManager::onTabSelected( %this,%text,%index )");
	
	$SEP_AmbientBook_PageId = %index;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_CloudsBook::onTabSelected( %this,%text,%index ) {	
	
	$SEP_CloudsBook_PageId = %index;
}
//------------------------------------------------------------------------------