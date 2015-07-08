//==============================================================================
// HelperLab -> Scripted GuiGroup system
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to update multiple GUIs sharing same GuiGroup at same time
//==============================================================================

//==============================================================================
// Add a GuiControl to it's GuiGroup SimSet
function addCtrlToGuiGroup(%ctrl) {
    //logd("addCtrlToGuiGroup(%ctrl)",%ctrl);
    
    //Leave if invalid object
    if (!isObject(%ctrl)) return;

    %group = $GuiGroup_[%ctrl.guiGroup];
    if (!isObject(%group)) {
        %group = newSimSet("GuiGroup_"@%ctrl.guiGroup,$Group_GuiGroup);
    }
    else if (%group.getObjectIndex(%ctrl) >= 0){
       //warnLog("Object:",%ctrl.getName(),"is already in the group:",%ctrl.guiGroup);       
       return;
    }
    %group.add(%ctrl);

}
//------------------------------------------------------------------------------
//==============================================================================
// Add a GuiControl to it's GuiGroup SimSet
function getGuiGroupList(%groupType) {
    //logd("addCtrlToGuiGroup(%ctrl)",%ctrl);
    %group = $GuiGroup_[%groupType];
    if (!isObject(%group)) {
        //warnlog("Try to call a GuiGroup action on invalid group:",%groupType);
        echo("Try to call a get ctrl list for invalid GuiGroup:" SPC %groupType);
        return "";
    }
   
    foreach(%ctrl in %group) {
      %list = strAddWord(%list,%ctrl.getId());  
    }
    devLog(%groupType,"guiGroup Ctrl list:",%list);
    return %list;
}
//------------------------------------------------------------------------------
//==============================================================================
// Call an action on all GuiControl in a GuiGroup
function doGuiGroupAction(%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10) {
    //loge("doGuiGroupAction(%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10)",%groupType,%act1,%act2,%act3,%act4,%act5,%act6,%act7,%act8,%act9,%act10);
    %group = $GuiGroup_[%groupType];
    if (!isObject(%group)) {
        //warnlog("Try to call a GuiGroup action on invalid group:",%groupType);
        echo("Try to call a GuiGroup action on invalid group:" SPC %groupType);
        return;
    }
    foreach(%ctrl in %group) {
        %id = 1;
        while (%act[%id] !$="") {
        		//devLog(%groupType,"Calling group action:","%ctrl."@%act[%id]@";");
        		if (!strFind(%act[%id],";")){
        			//devLog("Old guigroup action format detected with no ;. ; added to the end",%act[%id]);
        			%act[%id] = %act[%id]@";";
        		}
            eval("%ctrl."@%act[%id]);
            %id++;
        }
    }
    return true;
}
//------------------------------------------------------------------------------
