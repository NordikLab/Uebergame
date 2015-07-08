//==============================================================================
// GameLab -> GuiDragAndDrop Helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Drag and Drop Activation Functions
//==============================================================================

//==============================================================================
// startDragAndDropCtrl - Simplified version of dragAndDropCtrl();
//------------------------------------------------------------------------------
/// Start dragging a specified control with a defaultCallback in case specific
/// onControlDropped haven't been called. If not set, the default onControlDropped
/// will be used.
/// @sourceCtrl The control use as drag source
/// @defaultCallback Function to call when default onControlDropped is called
/// @dropType Variable used to validate the dropped ON control
function startDragAndDropCtrl( %sourceCtrl,%defaultCallback,%dropType ){
   if (!isObject(%sourceCtrl)){
      warnLog("Trying to drop something that is not an object:",%sourceCtrl);
      return;
   }  
   // First we construct a new temporary swatch button that becomes the payload for our
   // drag operation and give it the properties of the swatch button we want to copy.
   %payload = %sourceCtrl.deepClone();
   %payload.position = "0 0";
   %payload.setName("");
   %payload.dragSourceControl = %sourceCtrl; // Remember where the drag originated from so that we don't copy a color swatch onto itself.
   %payload.dropType = %dropType;
   %payload.failOnDefault = true;
%payload.defaultCallback = %defaultCallback;
   // Calculate the offset of the GuiDragAndDropControl from the mouse cursor.  Here we center
   // it on the cursor.

   %xOffset = getWord( %payload.extent, 0 ) / 2;
   %yOffset = getWord( %payload.extent, 1 ) / 2;

   // Compute the initial position of the GuiDragAndDrop control on the cavas based on the current
   // mouse cursor position.

   %cursorPos = Canvas.getCursorPos();
   %xPos = getWord( %cursorpos, 0 ) - %xOffset;
   %yPos = getWord( %cursorpos, 1 ) - %yOffset;

   // Create the drag control.
   %dragCtrl = new GuiDragAndDropControl()
   {
      canSaveDynamicFields    = "0";
      Profile                 = "ToolsGuiSolidDefaultProfile";
      HorizSizing             = "right";
      VertSizing              = "bottom";
      Position                = %xPos SPC %yPos;
      extent                  = %payload.extent;
      MinExtent               = "4 4";
      canSave                 = "1";
      Visible                 = "1";
      hovertime               = "1000";

      // Let the GuiDragAndDropControl delete itself on mouse-up.  When the drag is aborted,
      // this not only deletes the drag control but also our payload.
      deleteOnMouseUp         = true;

      // To differentiate drags, use the namespace hierarchy to classify them.
      // This will allow a color swatch drag to tell itself apart from a file drag, for example.
      class                   = "DropControlClass_"@%dropType;
      dropType = %dropType;
      
   };

   // Add the temporary color swatch to the drag control as the payload.
   %dragCtrl.add( %payload );

   // Start drag by adding the drag control to the canvas and then calling startDragging().

   Canvas.getContent().add( %dragCtrl );
   %dragCtrl.startDragging( %xOffset, %yOffset );
}

//==============================================================================
// SimObject / SimGroup Helpers
//==============================================================================
/// Drag a ctrl and drop it in container with field set to dropType only
/// @dropType The control will only be dropped in ctrl with dropType field
/// @failCallback Function to call when drop succeed (default to DragFailed)
function dragAndDropCtrl( %sourceCtrl,%dropType,%failCallback,%useDefault ){
   if (!isObject(%sourceCtrl)){
      warnLog("Trying to drop something that is not an object:",%sourceCtrl);
      return;
   }
   if (%failCallback $= "")
      %failCallback = "DragFailed";
   // First we construct a new temporary swatch button that becomes the payload for our
   // drag operation and give it the properties of the swatch button we want to copy.
   %payload = %sourceCtrl.deepClone();
   %payload.position = "0 0";
   %payload.setName("");
   %payload.dragSourceControl = %sourceCtrl; // Remember where the drag originated from so that we don't copy a color swatch onto itself.
   %payload.failCallback = %failCallback;
   
   %payload.useDefaultCallbacks = %useDefault;

   // Calculate the offset of the GuiDragAndDropControl from the mouse cursor.  Here we center
   // it on the cursor.

   %xOffset = getWord( %payload.extent, 0 ) / 2;
   %yOffset = getWord( %payload.extent, 1 ) / 2;

   // Compute the initial position of the GuiDragAndDrop control on the cavas based on the current
   // mouse cursor position.

   %cursorPos = Canvas.getCursorPos();
   %xPos = getWord( %cursorpos, 0 ) - %xOffset;
   %yPos = getWord( %cursorpos, 1 ) - %yOffset;

   // Create the drag control.
   %dragCtrl = new GuiDragAndDropControl()
   {
      canSaveDynamicFields    = "0";
      Profile                 = "ToolsGuiSolidDefaultProfile";
      HorizSizing             = "right";
      VertSizing              = "bottom";
      Position                = %xPos SPC %yPos;
      extent                  = %payload.extent;
      MinExtent               = "4 4";
      canSave                 = "1";
      Visible                 = "1";
      hovertime               = "1000";

      // Let the GuiDragAndDropControl delete itself on mouse-up.  When the drag is aborted,
      // this not only deletes the drag control but also our payload.
      deleteOnMouseUp         = true;

      // To differentiate drags, use the namespace hierarchy to classify them.
      // This will allow a color swatch drag to tell itself apart from a file drag, for example.
      class                   = "DropControlClass_"@%dropType;
      dropType = %dropType;
      
   };

   // Add the temporary color swatch to the drag control as the payload.
   %dragCtrl.add( %payload );

   // Start drag by adding the drag control to the canvas and then calling startDragging().

   Canvas.getContent().add( %dragCtrl );
   %dragCtrl.startDragging( %xOffset, %yOffset );
}


//==============================================================================
// Empty Methods to overwrite
//==============================================================================

//==============================================================================
// Initialize the client-side scripts
function GuiControl::onControlDragEnter(%this, %control,%dropPoint) {  
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::onControlDragExit(%this, %control, %dropPoint) {  
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::onControlDragged(%this, %control, %dropPoint) {  
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::onControlDropped(%this, %control, %dropPoint) {  
	
	if (%control.defaultCallback !$= ""){		
		warnLog("Default onControlDropped called but switched to specified callback:",%control.defaultCallback);
		eval(%control.defaultCallback@"(%this,%control,%dropPoint);");
		 //show(%control.dragSourceControl);
		return;
	}
   //Nothing to do with invalid dropped control
   %srcCtrl = %control.dragSourceControl;  
  
   if (!isObject(%control))
      return;
   if (%control.dropType $= ""){
      //No droptype, just call the success callback
      if (%srcCtrl.isMethod("DragSuccess"))
         eval(%srcCtrl@".DragSuccess(%this,%dropPoint);");
      else
         warnLog("Dragged control dropped in valid container but no Success callback found. Nothing has been done");
      
      return;
   }
   if (%this.dropType $= %control.dropType){
      devLog(%control.getName(),"dropped to valid container:",%this.getName());
      if (%control.isMethod("DragSuccess"))
         eval(%control@".DragSuccess(%this,%dropPoint);");
      else
         warnLog("Dragged control dropped in valid container but no Success callback found. Nothing has been done");
      
      return;
   }     
   
   if (%control.isMethod(%control.failCallback))
       eval(%control@"."@%control.failCallback@"(%this,%dropPoint);");
   else
   	eval(%control.failCallback);
   //Make sure the source is visible in case it was hiden during dragging   
   show(%control.dragSourceControl);
}
//------------------------------------------------------------------------------
