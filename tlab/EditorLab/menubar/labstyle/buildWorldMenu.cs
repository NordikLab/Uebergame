//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::buildMenu(%this) {
	Lab.clearMenus();
	%menuId = 0;

	while($LabMenu[%menuId] !$= "") {
		Lab.addMenu( $LabMenu[%menuId],%menuId);
		%menuItemId = 0;

		while($LabMenuItem[%menuId,%menuItemId] !$= "") {
			%item = $LabMenuItem[%menuId,%menuItemId];
			Lab.addMenuItem( %menuId, getField(%item,0),%menuItemId,getField(%item,1),-1);
			%subMenuItemId = 0;

			while($LabMenuSubMenuItem[%menuId,%menuItemId,%subMenuItemId] !$= "") {
				%subitem = $LabMenuSubMenuItem[%menuId,%menuItemId,%subMenuItemId] ;
				Lab.addSubmenuItem(%menuId,%menuItemId,getField(%subitem,0),%subMenuItemId,getField(%subitem,1),-1);
				%subMenuItemId++;
			}

			%menuItemId++;
		}

		%menuId++;
	}
}
//------------------------------------------------------------------------------