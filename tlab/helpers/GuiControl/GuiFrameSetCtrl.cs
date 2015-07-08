//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// String Manipulation Helpers
//==============================================================================
//==============================================================================
// Set a new rows value and refresh the frame set
function GuiFrameSetCtrl::setRows(%this,%rows,%fitAll){
	%this.rows = %rows;
	%this.updateSizes();
	
}

//==============================================================================
// Set a new rows value and refresh the frame set
function GuiFrameSetCtrl::hideCtrl(%this,%ctrl){
	hide(%ctrl);
	%this.pushToBack(%ctrl);
}
//==============================================================================
// Set a new rows value and refresh the frame set
function GuiFrameSetCtrl::showCtrl(%this,%ctrl,%index){
	
	show(%ctrl);	
	
	%currentIndexHolder = %this.getObject(%index);
	
	if (%currentIndexHolder $= %ctrl){
		devLog(%ctrl.getName()," is already at index:",%index);
		return;
	}
	
	
	%this.reorderChild(%ctrl, %currentIndexHolder);	
}	
//==============================================================================
// References
//==============================================================================
/*

void GuiFrameSetCtrl::addColumn	(		 ) 	
Add a new column.

void GuiFrameSetCtrl::addRow	(		 ) 	
Add a new row.

void GuiFrameSetCtrl::frameBorder	(	int 	index,
string 	state = "dynamic"	 
)			
Override the borderEnable setting for this frame.

Parameters:
index 	Index of the frame to modify
state 	New borderEnable state: "on", "off" or "dynamic"
void GuiFrameSetCtrl::frameMinExtent	(	int 	index,
int 	width,
int 	height	 
)			
Set the minimum width and height for the frame. It will not be possible for the user to resize the frame smaller than this.

Parameters:
index 	Index of the frame to modify
width 	Minimum width in pixels
height 	Minimum height in pixels
void GuiFrameSetCtrl::frameMovable	(	int 	index,
string 	state = "dynamic"	 
)			
Override the borderMovable setting for this frame.

Parameters:
index 	Index of the frame to modify
state 	New borderEnable state: "on", "off" or "dynamic"
void GuiFrameSetCtrl::framePadding	(	int 	index,
RectSpacingI 	padding	 
)			
Set the padding for this frame. Padding introduces blank space on the inside edge of the frame.

Parameters:
index 	Index of the frame to modify
padding 	Frame top, bottom, left, and right padding
int GuiFrameSetCtrl::getColumnCount	(		 ) 	
Get the number of columns.

Returns:
The number of columns
int GuiFrameSetCtrl::getColumnOffset	(	int 	index	 ) 	
Get the horizontal offset of a column.

Parameters:
index 	Index of the column to query
Returns:
Column offset in pixels
RectSpacingI GuiFrameSetCtrl::getFramePadding	(	int 	index	 ) 	
Get the padding for this frame.

Parameters:
index 	Index of the frame to query
int GuiFrameSetCtrl::getRowCount	(		 ) 	
Get the number of rows.

Returns:
The number of rows
int GuiFrameSetCtrl::getRowOffset	(	int 	index	 ) 	
Get the vertical offset of a row.

Parameters:
index 	Index of the row to query
Returns:
Row offset in pixels
void GuiFrameSetCtrl::removeColumn	(		 ) 	
Remove the last (rightmost) column.

void GuiFrameSetCtrl::removeRow	(		 ) 	
Remove the last (bottom) row.

void GuiFrameSetCtrl::setColumnOffset	(	int 	index,
int 	offset	 
)			
Set the horizontal offset of a column.

Note that column offsets must always be in increasing order, and therefore this offset must be between the offsets of the colunns either side.

Parameters:
index 	Index of the column to modify
offset 	New column offset
void GuiFrameSetCtrl::setRowOffset	(	int 	index,
int 	offset	 
)			
Set the vertical offset of a row.

Note that row offsets must always be in increasing order, and therefore this offset must be between the offsets of the rows either side.

Parameters:
index 	Index of the row to modify
offset 	New row offset
void GuiFrameSetCtrl::updateSizes	(		 ) 	
Recalculates child control sizes.

Member Data Documentation

bool GuiFrameSetCtrl::autoBalance
If true, row and column offsets are automatically scaled to match the new extents when the control is resized.

ColorI GuiFrameSetCtrl::borderColor
Color of interior borders between cells.

GuiFrameState GuiFrameSetCtrl::borderEnable
Controls whether frame borders are enabled.

Frames use this value unless overridden for that frame using ctrl.frameBorder(index)

GuiFrameState GuiFrameSetCtrl::borderMovable
Controls whether borders can be dynamically repositioned with the mouse by the user.

Frames use this value unless overridden for that frame using ctrl.frameMovable(index)

int GuiFrameSetCtrl::borderWidth
Width of interior borders between cells in pixels.

intList GuiFrameSetCtrl::columns
A vector of column offsets (determines the width of each column).

int GuiFrameSetCtrl::fudgeFactor
Offset for row and column dividers in pixels.

intList GuiFrameSetCtrl::rows
A vector of row offsets (determines the height of each row).
*/