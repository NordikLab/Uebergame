//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//---------------------------------------------------------------------------------------------
// dumpKeybindings
// Saves of all keybindings.
//---------------------------------------------------------------------------------------------
function dumpKeybindings()
{
   // Loop through all the binds.
   for (%i = 0; %i < $keybindCount; %i++)
   {
      devLog("Bind",$keybindMap[%i]);
      // If we haven't dealt with this map yet...
      if (isObject($keybindMap[%i]))
      {
         // Save and delete.
         $keybindMap[%i].save("core/dump/bind.cs", %i == 0 ? false : true);
         $keybindMap[%i].delete();
      }
   }
}

function loadKeybindings()
{
   $keybindCount = 0;
   // Load up the active projects keybinds.
   if(isFunction("setupKeybinds"))
      setupKeybinds();
}

function dumpAllBinds()
{
  foreach$(%map in $BindMapList){
		eval(%map@"Map.save( \"core/dump/allBinds.cs\",%append );");
		%append = true;
  }
  extractBindsFromFile("core/dump/allBinds.cs");
}

function extractBindsFromFile(%file)
{
   /*
   if (isObject(hudMap)) hudMap.delete();
new ActionMap(hudMap);
hudMap.bind(keyboard, "ctrl h", hideHUDs);
hudMap.bind(keyboard, "alt p", doScreenShotHudless);
hudMap.bind(keyboard, "f2", showPlayerList);
*/
   $ExtractedBinds = newArrayObject("ExtractedBinds");
  %fileRead = new FileObject();
	%result = %fileRead.OpenForRead(%file);
	
	while( !%fileRead.isEOF() ) {
	   %line = %fileRead.readLine();
	   %id++;
	   if (strstr(%line,".bind") $= "-1")
	      continue;
      %type = "bind";
      %noDot = strreplace(%line,"."," ");
      if (strstr(%line,".bindCmd") !$= "-1")
         %type = "bindCmd";
      if (strstr(%line,".bindObj") !$= "-1")
         %type = "bindObj";
         
      %map =  firstWord(%noDot); 
      if ( %mapId[%map] $= "")
         %mapList = trim( %mapList SPC %map);
    
              
       %mapId[%map]++; 
       %mid =   %mapId[%map]; 
      %bracketStart = strpos(%noDot,"(");	  
	    %inText = getSubStr(%noDot,%bracketStart+1);
	    %inText = trim(%inText) @ "***";
	    %inText = strreplace(%inText,");***","");
	     %inText = strreplace(%inText,"\"","");
	     
	    %inTabbed = strReplace(%inText,",","\t");
	     devLog("Map:",%map,"Type:",%type);
      if (%type $= "bindCmd"){
        %device = getField(%inTabbed,0);
        %action = getField(%inTabbed,1);
        %cmd = getField(%inTabbed,2);
        %cmdBreak = getField(%inTabbed,3);
          //devLog("Device",getField(%inTabbed,0),"Action=",getField(%inTabbed,1),"MakeCmd=",getField(%inTabbed,2),"BreakCmd=",getField(%inTabbed,3));
      }
	   else if (getFieldCount(%inTabbed) >= 5){
	       %device = getField(%inTabbed,0);
        %action = getField(%inTabbed,1);
        %cmd = getField(%inTabbed,4);
        %flag = getField(%inTabbed,2);
        %deadzone = getField(%inTabbed,3);
	       devLog("Device",getField(%inTabbed,0),"Action=",getField(%inTabbed,1),"Flag=",getField(%inTabbed,2),"DeadZone=",getField(%inTabbed,3),"Cmd=",getField(%inTabbed,4));
	    }
	    else {
	        %device = getField(%inTabbed,0);
        %action = getField(%inTabbed,1);
        %cmd = getField(%inTabbed,2);
	        devLog("Device",getField(%inTabbed,0),"Action=",getField(%inTabbed,1),"Cmd=",getField(%inTabbed,2));
	    
	    }
      $ExtractedBinds[%map,%mid] = %device TAB %action TAB %cmd TAB %cmdBreak TAB %deadzone TAB %flag;
	     devLog("InTextp:",%line);
	    devLog("Map:",%map,"Device",getField(%inTabbed,0),"Action=",getField(%inTabbed,1),"Command=",getField(%inTabbed,2));
	    //gamepad0, "btn_y", "getout 
	   $ExtractedBindsCount[%map] = %mid; 
	}
	%fileRead.close();
	%fileRead.delete();
	
	foreach$(%map in %mapList)
	   devLog($ExtractedBindsCount[%map]," binds extracted from map:", %map);
   
  
	   
   $ExtractedBindsMaps = %mapList;
	
}
function dumpAllExtractedMapData()
{
    foreach$(%map in $ExtractedBindsMaps)
	   dumpExtractedMapData(%map);
}
function dumpExtractedMapData(%map)
{
   for(%i=1;%i<=$ExtractedBindsCount[%map];%i++)
      devLog(%map,"--> Bind #"@%i,$ExtractedBinds[%map,%i]);
}