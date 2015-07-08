//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
/*
 LabMenu.addMenu("MyFirstMenu",0);
   LabMenu.addMenuItem(0,"MyFirstItem",0,"Ctrl numpad0",-1);
   LabMenu.addSubmenuItem("MyFirstMenu","MyFirstItem","MyFirstSubItem",0,"",-1);
   */

//==============================================================================
function Lab::addMenuBind(%this,%bind,%command) {
	EditorMap.bindCmd(keyboard, %bind, %command, "");
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addMenu(%this,%group,%id) {
	if (%id $= "") %id = 1;

	LabMenu.addMenu(%group,%id);
	LabMenu.menuList = trim(LabMenu.menuList SPC %group);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::addMenuItem(%this,%menu,%menuItemText,%menuItemId,%accelerator,%checkGroup) {
	/*Adds a menu item to the specified menu. The menu argument can be either the text of a menu or its id.

	Parameters:
	%menu 	Menu name or menu Id to add the new item to.
	%menuItemText 	Text for the new menu item.
	%menuItemId 	Id for the new menu item.
	%accelerator 	Accelerator key for the new menu item.
	%checkGroup 	Check group to include this menu item in.*/
	$LabMenuCallback[%menu,%menuItemId,%menuItemText] = getField($LabMenuItem[%menu,%menuItemId],2);

	if (%accelerator !$="") {
		%callBack =  getField($LabMenuItem[%menu,%menuItemId],2);
		%this.addMenuBind(%accelerator,%callBack);
	}

	LabMenu.addMenuItem(%menu,%menuItemText,%menuItemId,%accelerator,%checkGroup);
	LabMenu.menuItemList[%menu] = trim(LabMenu.menuItemList[%menu] SPC %menuItemText);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::addSubmenuItem(%this,%menuTarget,%menuItem,%submenuItemText,%submenuItemId,%accelerator,%checkGroup) {
	/*Adds a menu item to the specified menu. The menu argument can be either the text of a menu or its id.

	   Parameters:
	   %menuTarget 	Menu to affect a submenu in
	   %menuItem 	Menu item to affect
	   %submenuItemText 	Text to show for the new submenu
	   %submenuItemId 	Id for the new submenu
	   %accelerator 	Accelerator key for the new submenu
	   %checkGroup 	Which check group the new submenu should be in, or -1 for none.
	   LabMenu.setMenuItemSubmenuState(1,1,true);
	    LabMenu.addSubmenuItem(1,1,"Sub",0,"",-1);
	*/
	$LabMenuCallback[%menuTarget,%submenuItemId,%submenuItemText] = getField($LabMenuSubMenuItem[%menuTarget,%menuItem,%submenuItemId],2);

	if (%accelerator !$="") {
		%callBack =  getField( $LabMenuSubMenuItem[%menuTarget,%menuItem,%submenuItemId],2);
		%this.addMenuBind(%accelerator,%callBack);
	}

	LabMenu.addSubmenuItem(%menuTarget,%menuItem,%submenuItemText,%submenuItemId,%accelerator,%checkGroup);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::ClearMenus(%this) {
	LabMenu.clearMenus("","");
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::ClearMenusItems(%this,%menu) {
	LabMenu.ClearMenusItems(%menu);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::removeMenu(%this,%menu) {
	LabMenu.removeMenu(%menu);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeMenuItem(%this,%menu,%item) {
	LabMenu.removeMenuItem(%menu,%item);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setCheckmarkBitmapIndex(%this,%index) {
	LabMenu.setCheckmarkBitmapIndex(%index);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuBitmapIndex(%this,%menuTarget,%bitmapindex,%bitmaponly,%drawborder) {
	/* Sets the bitmap index for the menu and toggles rendering only the bitmap.

	Parameters:
	%menuTarget 	Menu to affect
	%bitmapindex 	Bitmap index to set for the menu
	%bitmaponly 	If true, only the bitmap will be rendered
	%drawborder 	If true, a border will be drawn around the menu.*/
	LabMenu.setMenuBitmapIndex(%menuTarget,%bitmapindex,%bitmaponly,%drawborder);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemBitmap(%this,%menuTarget,%menuItem,%bitmapIndex) {
	/* Sets the specified menu item bitmap index in the bitmap array. Setting the item's index to -1 will remove any bitmap.

	Parameters:
	%menuTarget 	Menu to affect the menuItem in
	%menuItem 	Menu item to affect
	%bitmapIndex 	Bitmap index to set the menu item to
	*/
	LabMenu.setMenuItemBitmap(%menuTarget,%menuItem,%bitmapIndex);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuItemChecked(%this,%menuTarget,%menuItem,%checked) {
	/* Sets the menu item bitmap to a check mark, which by default is the first element in the bitmap array (although this may be changed with setCheckmarkBitmapIndex()). Any other menu items in the menu with the same check group become unchecked if they are checked.

	Parameters:
	%menuTarget 	Menu to work in
	%menuItem 	Menu item to affect
	%checked 	Whether we are setting it to checked or not

	*/
	LabMenu.setMenuItemBitmap(%menuTarget,%menuItem,%checked);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuItemChecked(%this,%menuTarget,%menuItemTarget,%enabled) {
	/* sets the menu item to enabled or disabled based on the enable parameter. The specified menu and menu item can either be text or ids.

	Detailed description

	Parameters:
	%menuTarget 	Menu to work in
	%menuItemTarget 	The menu item inside of the menu to enable or disable
	%enabled 	Boolean enable / disable value.

	*/
	LabMenu.setMenuItemChecked(%menuTarget,%menuItemTarget,%enabled);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemSubmenuState(%this,%menuTarget,%menuItem,%isSubmenu) {
	/* Sets the given menu item to be a submenu.

	Parameters:
	%menuTarget 	Menu to affect a submenu in
	%menuItem 	Menu item to affect
	%isSubmenu 	Whether or not the menuItem will become a subMenu or not
	LabMenu.setMenuItemSubmenuState(0,2,true);
	*/
	LabMenu.setMenuItemSubmenuState(%menuTarget,%menuItem,%isSubmenu);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemText(%this,%menuTarget,%menuItem,%newMenuItemText) {
	/* Sets the text of the specified menu item to the new string.

	Parameters:
	%menuTarget 	Menu to affect
	%menuItem 	Menu item in the menu to change the text at
	%newMenuItemText 	New menu text

	*/
	LabMenu.setMenuItemText(%menuTarget,%menuItem,%newMenuItemText);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuItemVisible(%this,%menuTarget,%menuItem,%isVisible) {
	/* Brief Description.

	Detailed description

	Parameters:
	menuTarget 	Menu to affect the menu item in
	menuItem 	Menu item to affect
	%isVisible 	Visible state to set the menu item to.

	*/
	LabMenu.setMenuItemVisible(%menuTarget,%menuItem,%isVisible);
}
//------------------------------------------------------------------------------


//==============================================================================
function Lab::setMenuMargins(%this,%horizontalMargin,%verticalMargin,%bitmapToTextSpacing) {
	/*
	Sets the menu rendering margins: horizontal, vertical, bitmap spacing.

	Detailed description

	Parameters:
	%horizontalMargin 	Number of pixels on the left and right side of a menu's text.
	%verticalMargin 	Number of pixels on the top and bottom of a menu's text.
	%bitmapToTextSpacing 	Number of pixels between a menu's bitmap and text.

	*/
	LabMenu.setMenuMargins(%horizontalMargin,%verticalMargin,%bitmapToTextSpacing);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setMenuText(%this,%menuTarget,%newMenuText) {
	/* Sets the text of the specified menu to the new string.

	Parameters:
	menuTarget 	Menu to affect
	%newMenuText 	New menu text


	*/
	LabMenu.setMenuText(%menuTarget,%newMenuText);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setMenuVisible(%this,%menuTarget,%visible) {
	/* Sets the whether or not to display the specified menu.

	Parameters:
	menuTarget 	Menu item to affect
	%visible 	Whether the menu item will be visible or not



	*/
	LabMenu.setMenuVisible(%menuTarget,%visible);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setSubmenuItemChecked(%this,%menuTarget,%menuItem,%submenuItemText,%checked) {
	/* Sets the menu item bitmap to a check mark, which by default is the first element in the bitmap array (although this may be changed with setCheckmarkBitmapIndex()). Any other menu items in the menu with the same check group become unchecked if they are checked.

	Parameters:
	%menuTarget 	Menu to affect a submenu in
	%menuItem 	Menu item to affect
	%submenuItemText 	Text to show for submenu
	%checked 	Whether or not this submenu item will be checked.



	*/
	LabMenu.setSubmenuItemChecked(%menuTarget,%menuItem,%submenuItemText,%checked);
}
//------------------------------------------------------------------------------



//==============================================================================
// LabMenu Callbacks
//==============================================================================

//==============================================================================
function LabMenu::onMenuItemSelect(%this,%menuId,%menuText,%menuItemId,%menuItemText) {
	/* Called whenever an item in a menu is selected.

	Parameters:
	%menuId 	Index id of the menu which contains the selected menu item
	%menuText 	Text of the menu which contains the selected menu item
	%menuItemId 	Index id of the selected menu item
	%menuItemText 	Text of the selected menu item

	*/

		
	%callBack =  $LabMenuCallback[%menuId,%menuItemId,%menuItemText];
	eval(%callBack);
}
//------------------------------------------------------------------------------

//==============================================================================
function LabMenu::onMenuSelect(%this,%menuId,%menuText) {
	/* Called whenever a menu is selected.

	Parameters:
	menuId 	Index id of the clicked menu
	menuText 	Text of the clicked menu

	*/
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMenu::onMouseInMenu(%this,%isInMenu) {
	/* Called whenever the mouse enters, or persists is in the menu.

	Parameters:
	isInMenu 	True if the mouse has entered the menu, otherwise is false.
	Note:
	To receive this callback, call setProcessTicks(true) on the menu bar.

	*/
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMenu::onSubmenuSelect(%this,%submenuId,%submenuText) {
	/* Called whenever a submenu is selected.

	Parameters:
	submenuId 	Id of the selected submenu
	submenuText 	Text of the selected submenu


	*/
}
//------------------------------------------------------------------------------