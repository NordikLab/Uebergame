//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
function AssetsLab::onWake( %this ) {
	if ($SEP_AssetTypeActive $= "")
		$SEP_AssetTypeActive = "0";

	AssetTypeStack.getObject(0).performClick();
	%this.contentCtrl = AssetIconArray;
}
//------------------------------------------------------------------------------
//==============================================================================
function AssetsLab::beginGroup( %this, %group ) {
	%this.currentGroup = %group;
}
//------------------------------------------------------------------------------
//==============================================================================
function AssetsLab::endGroup( %this, %group ) {
	%this.currentGroup = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function AssetsLab::getCreateObjectPosition() {
	%focusPoint = LocalClientConnection.getControlObject().getLookAtPoint();

	if( %focusPoint $= "" )
		return "0 0 0";
	else
		return getWord( %focusPoint, 1 ) SPC getWord( %focusPoint, 2 ) SPC getWord( %focusPoint, 3 );
}
//------------------------------------------------------------------------------
//==============================================================================


function AssetsLab::navigateDown( %this, %folder ) {
	if ( %this.address $= "" )
		%address = %folder;
	else
		%address = %this.address SPC %folder;

	// Because this is called from an IconButton::onClick command
	// we have to wait a tick before actually calling navigate, else
	// we would delete the button out from under itself.
	%this.schedule( 1, "navigate", %address );
}
//------------------------------------------------------------------------------
//==============================================================================
function AssetsLab::navigateUp( %this ) {
	%count = getWordCount( %this.address );

	if ( %count == 0 )
		return;

	if ( %count == 1 )
		%address = "";
	else
		%address = getWords( %this.address, 0, %count - 2 );

	%this.navigate( %address );
}
//------------------------------------------------------------------------------
//==============================================================================
function AssetsLab::setListView( %this, %noupdate ) {
	//AssetIconArray.clear();
	//AssetIconArray.setVisible( false );
	AssetIconArray.setVisible( true );
	%this.contentCtrl = AssetIconArray;
	%this.isList = true;

	if ( %noupdate == true )
		%this.navigate( %this.address );
}
//------------------------------------------------------------------------------
//==============================================================================
//function SEP_AssetPage::setIconView( %this )
//{
//echo( "setIconView" );
//
//AssetIconStack.clear();
//AssetIconStack.setVisible( false );
//
//AssetIconArray.setVisible( true );
//%this.contentCtrl = AssetIconArray;
//%this.isList = false;
//
//%this.navigate( %this.address );
//}

//------------------------------------------------------------------------------
//==============================================================================
function AssetPopupMenu::onSelect( %this, %id, %text ) {
	%split = strreplace( %text, "/", " " );
	SEP_AssetPage.navigate( %split );
}
//------------------------------------------------------------------------------
