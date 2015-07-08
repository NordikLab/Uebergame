//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initMenubar(%this) {
	if (!$Cfg_UseCoreMenubar) {
		exec("tlab/EditorLab/menubar/labstyle/menubarScript.cs");
		exec("tlab/EditorLab/menubar/labstyle/buildWorldMenu.cs");
		Lab.initLabMenuData(true);

		if (isObject(Lab.menuBar))
			Lab.menuBar.removeFromCanvas();

		return;
	}

	Lab.clearMenus();

	if(!isObject(%this.menuBar))
		%this.buildMenus();

	%this.attachMenus();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setNativeMenuSystem(%this,%useNative) {
	if ($Cfg_UseCoreMenubar == %useNative)
		return;

	$Cfg_UseCoreMenubar = %useNative;
	%this.initMenubar();
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function Lab::checkMenuItem(%this,%menu,%firstPos,%lastPos,%checkPos) {
	if ($Cfg_UseCoreMenubar)
		eval("Lab."@%menu@"Menu.checkRadioItem( "@%firstpos@", "@%lastPos@", "@%checkPos@" );");
}
function Lab::checkFindItem(%this,%findMenu,%firstPos,%lastPos,%checkPos) {
	if ($Cfg_UseCoreMenubar) {
		%menu = Lab.findMenu( %findMenu );
		eval("Lab."@%menu@"Menu.checkRadioItem( "@%firstpos@", "@%lastPos@", "@%checkPos@" );");
	}
}
function Lab::setMenuDefaultState(%this,%menu) {
	if ($Cfg_UseCoreMenubar) {
		%menu.setupDefaultState();
	}
}

//==============================================================================
//Editor Initialization callbacks
//==============================================================================
//==============================================================================
function Lab::insertDynamicMenu(%this,%menu) {
	if ($Cfg_UseCoreMenubar)
		Lab.menuBar.insert( %menu, EditorGui.menuBar.dynamicItemInsertPos );
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeDynamicMenu(%this,%menu) {
	if ($Cfg_UseCoreMenubar)
		Lab.menuBar.remove( %menu );
}
//------------------------------------------------------------------------------
function Lab::attachMenus(%this) {
	if ($Cfg_UseCoreMenubar)
		%this.menuBar.attachToCanvas(Canvas, 0);
}


function Lab::detachMenus(%this) {
	if ($Cfg_UseCoreMenubar)
		%this.menuBar.removeFromCanvas();
}

function Lab::setMenuDefaultState(%this) {
	if(! isObject(%this.menuBar) || !$Cfg_UseCoreMenubar)
		return 0;

	for(%i = 0; %i < %this.menuBar.getCount(); %i++) {
		%menu = %this.menuBar.getObject(%i);
		%menu.setupDefaultState();
	}

	%this.worldMenu.setupDefaultState();
}

function Lab::updateUndoMenu(%this) {
	if ($Cfg_UseCoreMenubar) {
		%editMenu =  Lab.menuBar-->EditMenu;
		%undoName = %this.getNextUndoName();
		%redoName = %this.getNextRedoName();
		%editMenu.setItemName( 0, "Undo " @ %undoName );
		%editMenu.setItemName( 1, "Redo " @ %redoName );
		%editMenu.enableItem( 0, %undoName !$= "" );
		%editMenu.enableItem( 1, %redoName !$= "" );
	}
}

