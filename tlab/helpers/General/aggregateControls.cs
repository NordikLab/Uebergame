//==============================================================================
// Boost! -> Main functions used on game launch
// Copyright NordikLab Studio, 2013
//==============================================================================
$HelpersAggregateSkipInternal = "field fieldTitle";
function GuiControl::updateFriends( %this) { 
	%isAggregated = strFind(%this.getParent().class,"Aggregate"); 	  
   if (%isAggregated){		
      %this.getParent().updateFromChild(%this);   
   }   
}
function GuiControl::canAggregate( %this) { 
	%skipObj = strFind($HelpersAggregateSkipInternal,%this.internalName);         
	if(%this.internalName !$= "" && !%skipObj)
		return true;
	
	return false;				
}
//------------------------------------------------------------------------------
// An Aggregate Control is a plain GuiControl that contains other controls,
// which all share a single job or represent a single value.
//------------------------------------------------------------------------------

// AggregateControl.setValue( ) propagates the value to any control that has an
// internal name.
function AggregateControl::setValue(%this, %val, %child) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if( %obj == %child )
			continue;			
		
		if(%obj.canAggregate())		
			%obj.setValueSafe(%val);
	}
	
}

// AggregateControl.getValue() uses the value of the first control that has an
// internal name, if it has not cached a value via .setValue
function AggregateControl::getValue(%this) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.canAggregate()) {
			//error("obj = " @ %obj.getId() @ ", " @ %obj.getName() @ ", " @ %obj.internalName );
			//error(" value = " @ %obj.getValue());
			return %obj.getValue();
		}
	}
}

// AggregateControl.updateFromChild( ) is called by child controls to propagate
// a new value, and to trigger the onAction() callback.
function AggregateControl::updateFromChild(%this, %child) {
	%val = %child.getValue();
	if(%val == mCeil(%val)) {
		%val = mCeil(%val);
	} else {
		if ( %val <= -100) {
			%val = mCeil(%val);
		} else if ( %val <= -10) {
			%val = mFloatLength(%val, 1);
		} else if ( %val < 0) {
			%val = mFloatLength(%val, 2);
		} else if ( %val >= 1000) {
			%val = mCeil(%val);
		} else if ( %val >= 100) {
			%val = mFloatLength(%val, 1);
		} else if ( %val >= 10) {
			%val = mFloatLength(%val, 2);
		} else if ( %val > 0) {
			%val = mFloatLength(%val, 3);
		}
	}
	%this.setValue(%val, %child);
	%this.onAction();
}

// default onAction stub, here only to prevent console spam warnings.
function AggregateControl::onAction(%this) {
}

// call a method on all children that have an internalName and that implement the method.
function AggregateControl::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.canAggregate() && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//==============================================================================
// AggregateBox 
//------------------------------------------------------------------------------
// AggregateBox is same as AggregateControl except there's no value validation
//==============================================================================

//==============================================================================
// SetValue-> propagates the value to any control that has an internal name.
function AggregateBox::setValue(%this, %val, %child) {
	
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		
		if( %obj == %child )
			continue;
      
      //Skip the object if internalName is in Skip Internal list
     if(%obj.canAggregate())
			%obj.setValueSafe(%val);
	}
	
	if (%this.aggregateCtrls !$=""){
		foreach$(%ctrl in %this.aggregateCtrls){
			if (!isObject(%ctrl))
				continue;
			foreach(%obj in %ctrl){
				 //Skip the object if internalName is in Skip Internal list
     			if(%obj.canAggregate())
					%obj.setValueSafe(%val);
			}
		}
	}
				
		
}
//------------------------------------------------------------------------------
//==============================================================================
// getValue-> uses the value of the first control that has an internal name, if it has not cached a value via .setValue
function AggregateBox::getValue(%this) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.canAggregate()) {
			//error("obj = " @ %obj.getId() @ ", " @ %obj.getName() @ ", " @ %obj.internalName );
			//error(" value = " @ %obj.getValue());
			return %obj.getValue();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// updateFromChild-> called by child controls to propagate a new value, and to trigger the onAction() callback.
function AggregateBox::updateFromChild(%this, %child) {
	%val = %child.getValue();	
	
	%this.setValue(%val, %child);
	%this.onAction();	
}
//------------------------------------------------------------------------------
// onAction-> here only to prevent console spam warnings.
function AggregateBox::onAction(%this) {

}
//------------------------------------------------------------------------------
// callMethod-> call a method on all children that have an internalName and that implement the method.
function AggregateBox::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.canAggregate() && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// AggregateVar
//------------------------------------------------------------------------------
// AggregateVar use the control variable value instead of direct control value
//==============================================================================

//==============================================================================
// SetValue-> propagates the value to any control that has an internal name.
function AggregateVar::setValue(%this, %val, %child) {

	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);

		if( %obj == %child )
			continue;

		//Skip the object if internalName is in Skip Internal list      
		if(%obj.canAggregate()) 
			%obj.setValueSafe(%val);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// updateFromChild-> called by child controls to propagate a new value, and to trigger the onAction() callback.
function AggregateVar::updateFromChild(%this, %child) {

	eval("%val = "@%child.variable@";");

	%this.setValue(%val, %child);
	%this.onAction();
}
//------------------------------------------------------------------------------
// onAction-> here only to prevent console spam warnings.
function AggregateVar::onAction(%this) {

}
//------------------------------------------------------------------------------
// callMethod-> call a method on all children that have an internalName and that implement the method.
function AggregateVar::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.canAggregate() && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// GuiControlSpecific Aggregate Functions
//==============================================================================

function DropDownTextEditCtrl::updateFromChild( %this, %ctrl ) {
	if( %ctrl.internalName $= "PopUpMenu" ) {
		%this->TextEdit.setText( %ctrl.getText() );
	} else if ( %ctrl.internalName $= "TextEdit" ) {
		%popup = %this->PopupMenu;
		%popup.changeTextById( %popup.getSelected(), %ctrl.getText() );
		%this.onRenameItem();
	}
}

