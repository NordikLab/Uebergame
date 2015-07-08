//==============================================================================
// TorqueLab -> Drag And Drop GUIs
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EditorTreeView::onDefineIcons( %this ) {
	%icons = "tlab/gui/icons/treeview_assets/default:" @
				"tlab/gui/icons/treeview_assets/folderclosed:" @
				"tlab/gui/icons/treeview_assets/groupclosed:" @
				"tlab/gui/icons/treeview_assets/folderopen:" @
				"tlab/gui/icons/treeview_assets/groupopen:" @
				"tlab/gui/icons/treeview_assets/hidden:" @
				"tlab/gui/icons/treeview_assets/shll_icon_passworded_hi:" @
				"tlab/gui/icons/treeview_assets/shll_icon_passworded:" @
				"tlab/gui/icons/treeview_assets/default";
	%this.buildIconTable(%icons);
}

function GuiTreeViewCtrl::handleRenameObject( %this, %name, %obj ) {
	%inspector = GuiInspector::findByObject( %obj );

	if( isObject( %inspector ) ) {
		%field = ( %this.renameInternal ) ? "internalName" : "name";
		%inspector.setObjectField( %field, %name );
		return true;
	}

	return false;
}
