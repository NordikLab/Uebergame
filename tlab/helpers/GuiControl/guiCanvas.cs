//==============================================================================
// Helpers Lab -> UI - Canvas helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//! getRealCursorPos() > Get the Cursor position inside the engine window
//+ Canvas.getCursorPos() return the cursor position on the user screen, with this function
//+ you will get the cursor position inside Torque3D window.
//+ Usage: %realPos = getRealCursorPos();
function getRealCursorPos() {
	%cursorPos = Canvas.getCursorPos();
	%windowPos = Canvas.getWindowPosition();
	%realPos = getWords(VectorSub(%cursorPos,%windowPos),0,1);
	
	return %realPos;  
}
//------------------------------------------------------------------------------