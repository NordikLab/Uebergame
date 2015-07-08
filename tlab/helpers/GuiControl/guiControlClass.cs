//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Called each time a GuiControl is loaded
function GuiControl::onAdd	(%this) {
    //Check if want to assign to a guiGroup
    if (%this.guiGroup !$= "" ) 
      addCtrlToGuiGroup(%this);   
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function GuiControl::fitIntoParents( %this,%mode ) {	
	
	
   %parent = %this.parentGroup;
	%pos = "0 0";
	%extent = %parent.extent;
	if (%mode $= "" || %mode $= "width"){
		%this.position.x = 0;
		%this.extent.x = %extent.x;
	}
	if (%mode $= "" || %mode $= "height"){
		%this.position.y = 0;
		%this.extent.y = %extent.y;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function GuiControl::clearChildResponder( %this ) {		
	%responder = %this.getFirstResponder();	
	if (isObject(%responder))
		%responder.clearFirstResponder();
}
//------------------------------------------------------------------------------

//==============================================================================
// Update the GuiControl data field depending of the Class (use by Aggregate system)
//==============================================================================
//==============================================================================
// Get the type of a GuiControl depending of it class
function GuiControl::getType( %this ) {

	if (!isObject(%this)) {
		return "";
	}

	%class = %this.getClassName();
	if (%class $= "GuiTextEditSliderBitmapCtrl" || %class $= "GuiTextEditSliderCtrl"
		              || %class $= "GuiSliderCtrl")
			%type = "Value";

	if (%class $= "GuiCheckBoxCtrl")
			%type = "Checkbox";

	if (%class $= "GuiColorPickerCtrl")
			%type = "Color";

	if (%class $= "GuiPopUpMenuCtrl" || %class $= "GuiTextEditCtrl" || %class $= "GuiTextCtrl")
			%type = "Text";
	return %type;
}
//------------------------------------------------------------------------------
//==============================================================================
//! Check if a substring is present in a string
//+ This function will search for a string inside another string. (String != Word)
//+ If the string is found, the function will return true, if not, it return false
//> %string: The source string to search into
//> %searchFor: The string to search for in source string
//> Usage: %found = strFind("Torque3D is the best","que3D"); //Will return true
//==============================================================================
// Set the value for a GuiControl depending of it's type
function GuiControl::setTypeValue( %this,%value,%updateFriends ) {

	%type = %this.getType();

	switch$(%type) {
	case "Value":
		%this.setValue(%value);

	case "Checkbox":
		%this.setStateOn(%value);
	case "Color":
	   if (%this.isIntColor)
	     %this.BaseColor = ColorIntToFloat(%value);
		else
		   %this.BaseColor = %value;
		%this.updateColor();
	case "Text":
		%this.setText(%value);
	default:
		%this.setValue(%value);
	}
	if (%updateFriends)
		%this.updateFriends();

}
//------------------------------------------------------------------------------
//==============================================================================
// Get the value of a GuiControl depending of it type
function GuiControl::getTypeValue( %this ) {
	%type = %this.getType();

	switch$(%type) {
	case "Value":
		%value = %this.getValue();
	case "Text":
		%value = %this.getText();

	case "Checkbox":
		%value = %this.isStateOn();
	case "Color":
		%value = %this.BaseColor ;

	}
	return %value;
}
//------------------------------------------------------------------------------

//==============================================================================
//! Align control with parent
//+ Set a GuiControl to be aligned with parent top, bottom, left or right
//+ Optional: You can set a margin to be added to the new position
//+ Usage: GuiControl.AlignCtrlToParent("top",10); //The Y position will be set to 10

//==============================================================================
function GuiControl::AlignCtrlToParent(%this,%direction,%margin) {
      if (%margin $= "")
         %margin = 0;
     
      %parent = %this.parentGroup;
     if (!isObject(%parent)){
         info(%this.getName()," have no parent to be forced inside");
       
         return;      
      } 
      %posX = %this.getPosition().x;
       %posY = %this.getPosition().y;
      switch$(%direction){
         case "right": //Set max right of parent        
            %posX = %parent.getExtent().x - %this.getExtent().x - %margin;
         case "left": //Set max left of parent        
            %posX= %margin;
         case "top": //Set max left of parent        
            %posY = %margin;
          case "bottom": //Set max right of parent         
          %posY = %parent.getExtent().y - %this.getExtent().y -%margin;          
          
           
         case "centerX": //Set max right of parent    
            %diff = %parent.getExtent().x - %this.getExtent().x;
            %posX = (%parent.getExtent().x - %this.getExtent().x)/2;   
              
      }   
    %this.setPosition(%posX,%posY);
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiControl::setSizeRatio(%this,%ratio,%axis) {
 
   %wasExtent = %this.extent;
 
   
  
   if (%axis $= "x" || %axis $= ""){   
      %newX = %this.defaultExtent.x * %ratio;
      %newX = mCeil(%newX);
      %this.extent.x = %newX;
   }
 
   if (%axis $= "y" || %axis $= ""){   
      %newY = %this.defaultExtent.y * %ratio;   
      %newY = mCeil(%newY);
       %this.extent.y = %newY;
   }
   
   devLog(%this.getId(),"Resized to:",%this.extent,"Was",%wasExtent,"Default",%this.defaultExtent);
   
   
   //Now update the align 
   if (%this.horizSizing $= "Center")
      %this.AlignCtrlToParent("CenterX");
   
}
//------------------------------------------------------------------------------
//==============================================================================
// Drag & Drop
//==============================================================================

//==============================================================================
// Collapse all stacks
function GuiControl::getRealPosition(%this) {   
   %parent = %this.parentGroup;
   %pos = %this.position;
   while(isObject(%parent)){
     %pos.x += %parent.position.x;
      %pos.y += %parent.position.y;
	   %parent = %parent.parentGroup;
   }
   return %pos;
}
//----------------------------------------------------------------------------


//==============================================================================
// Collapse all stacks
function GuiControl::forceInsideCtrl(%this,%ctrl) {   
   %maxX = %ctrl.extent.x - %this.extent.x;
   %maxY = %ctrl.extent.y - %this.extent.y;
   
   if (%maxX < 0 || %maxY < 0){
      warnLog("The control doesn't fit into the target because it's bigger! Source:",%this.extent,"Target:",%ctrl.extent);
      return;
   }  
   %this.position.x = mClamp(%this.position.x,0,%maxX);
   %this.position.y = mClamp(%this.position.y,0,%maxY);  
}
//----------------------------------------------------------------------------

//==============================================================================
// SetValue Safe Functions
//==============================================================================
function GuiControl::setValueSafe(%this, %val) {
	%cmd = %this.command;
	%alt = %this.altCommand;
	%this.command = "";
	%this.altCommand = "";
	%this.setValue(%val);
	%this.command = %cmd;
	%this.altCommand = %alt;
}

function GuiControl::shareValueSafe(%this, %dest) {
	%dest.setValueSafe(%this.getValue());
}

function GuiControl::shareValueSafeDelay(%this, %dest, %delayMs) {
	%this.schedule(%delayMs, "shareValueSafe", %dest);
}



//==============================================================================
// Initialize the client-side scripts
function GuiControl::collapseChilds(%this, %state) {
   devLog("GuiControl::collapseChilds callOnChildrenNoRecurse",%this);  
	%this.callOnChildrenNoRecurse("collapse", %state);
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::expandChilds(%this, %state) {
	foreach(%ctrl in %this) {
		%ctrl.expanded = %state;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::syncObj(%this, %obj,%field,%skipAggregate) {
   %value = %this.getTypeValue();
   if (isObject(%obj) && %field !$= ""){
      %obj.setFieldValue(%field,%value);
      devLog("SyncObj",%obj.getName(),"Field",%field,"Value",%obj.getFieldValue(%field));
   }
   if (!%skipAggregate)
      %this.updateFriends();
      
   
	
}
//------------------------------------------------------------------------------