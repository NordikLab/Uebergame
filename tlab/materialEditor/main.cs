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

// Material Editor Written by Dave Calabrese and Travis Vroman of Gaslight Studios

function initializeMaterialEditor() {
	echo(" % - Initializing Material Editor");
	execMEP(true);
	//exec("./gui/profiles.ed.cs");
	Lab.createPlugin("MaterialEditor","Material Editor");
	MaterialEditorPlugin.superClass = "WEditorPlugin";
	Lab.addPluginGui("MaterialEditor",MaterialEditorGui); //Tools renamed to Gui to store stuff
	Lab.addPluginDlg("MaterialEditor",matEd_cubemapEditor);
	Lab.addPluginDlg("MaterialEditor",matEd_addCubemapWindow);
	Lab.addPluginToolbar("MaterialEditor",MaterialEditorToolbar);
}
function execMEP(%loadGui) {
	if (%loadGui) {
		// Load MaterialEditor Guis
		exec("tlab/materialEditor/gui/matEd_cubemapEditor.gui");
		exec("tlab/materialEditor/gui/matEd_addCubemapWindow.gui");
		exec("tlab/materialEditor/gui/matEdNonModalGroup.gui");
		exec("tlab/materialEditor/gui/MaterialEditorToolbar.gui");
		exec("tlab/materialEditor/gui/MaterialEditorTools.gui");
	}

	// Load Client Scripts.
	exec("./scripts/materialEditor.cs");
	exec("./scripts/materialEditorUndo.cs");
	exec("./MaterialEditorPlugin.cs");
	exec("./base/ME_MaterialCore.cs");
	exec("./base/ME_MaterialActive.cs");
	exec("./base/ME_MaterialGui.cs");
	exec("./base/ME_MaterialUpdate.cs");
	exec("./base/ME_MaterialCubemap.cs");
	execPattern("tlab/materialEditor/guiScripts/*.cs");
}
function destroyMaterialEditor() {
}
