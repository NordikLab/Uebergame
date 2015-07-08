//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------
singleton GuiControlProfile (ForestEditorProfile) {
	canKeyFocus = true;
	category = "Editor";
};
function initializeForestEditor() {

	echo(" % - Initializing Forest Editor");
	//$FEP_BrushSet = newSimSet("FEP_BrushSet");
	execFEP(true);
	//Add the different editor GUIs to the LabEditor
	Lab.addPluginEditor("ForestEditor",ForestEditorGui);
	Lab.addPluginGui("ForestEditor",   ForestEditorTools);
	Lab.addPluginToolbar("ForestEditor",ForestEditToolbar);
	Lab.addPluginPalette("ForestEditor",   ForestEditorPalette);
	Lab.addPluginDlg("ForestEditor",   ForestEditorDialogs);
	Lab.createPlugin("ForestEditor");
	ForestEditorPlugin.editorGui = ForestEditorGui;
	ForestEditorPalleteWindow.position = getWord($pref::Video::mode, 0) - 209  SPC getWord(EditorGuiToolbar.extent, 1)-1;
	new SimSet(ForestTools) {
		new ForestBrushTool(ForestToolBrush) {
			internalName = "BrushTool";
			toolTip = "Paint Tool";
			buttonImage = "tlab/forest/images/brushTool";
		};
		new ForestSelectionTool(ForestToolSelection) {
			internalName = "SelectionTool";
			toolTip = "Selection Tool";
			buttonImage = "tlab/forest/images/selectionTool";
		};
	};
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "ForestEditorSelectModeBtn.performClick();", "" ); // Select
	%map.bindCmd( keyboard, "2", "ForestEditorMoveModeBtn.performClick();", "" );   // Move
	%map.bindCmd( keyboard, "3", "ForestEditorRotateModeBtn.performClick();", "" ); // Rotate
	%map.bindCmd( keyboard, "4", "ForestEditorScaleModeBtn.performClick();", "" );  // Scale
	%map.bindCmd( keyboard, "5", "ForestEditorPaintModeBtn.performClick();", "" );  // Paint
	%map.bindCmd( keyboard, "6", "ForestEditorEraseModeBtn.performClick();", "" );  // Erase
	%map.bindCmd( keyboard, "7", "ForestEditorEraseSelectedModeBtn.performClick();", "" );  // EraseSelected
	//%map.bindCmd( keyboard, "backspace", "ForestEditorGui.onDeleteKey();", "" );
	//%map.bindCmd( keyboard, "delete", "ForestEditorGui.onDeleteKey();", "" );
	ForestEditorPlugin.map = %map;
}
function execFEP(%loadGui) {
	if (%loadGui) {
		exec( "./gui/forestEditorGui.gui" );
		exec( "./gui/ForestEditorTools.gui" );
		exec( "./gui/forestEditToolbar.gui" );
		exec( "./gui/forestEditorPalette.gui" );
		exec( "tlab/ForestEditor/gui/forestEditorDialogs.gui" );
	}

	// Load Client Scripts.
	exec( "tlab/ForestEditor/forestEditorGui.cs" );
	exec( "./tools.cs" );
	exec( "tlab/ForestEditor/ForestEditorPlugin.cs" );
	exec( "tlab/ForestEditor/ForestEditorSave.cs" );
	exec( "tlab/ForestEditor/ForestEditorScript.cs" );
	execPattern("tlab/ForestEditor/scripts/*.cs");
	execPattern("tlab/ForestEditor/dialogs/*.cs");
}
function destroyForestEditor() {
}

// NOTE: debugging helper.
function reinitForest() {
	exec( "./main.cs" );
	exec( "./forestEditorGui.cs" );
	exec( "./tools.cs" );
}


