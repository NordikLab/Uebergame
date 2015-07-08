//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function validateDatablockName(%name)
{
   // remove whitespaces at beginning and end
   %name = trim( %name );
   
   // remove numbers at the beginning
   %numbers = "0123456789";   
   while( strlen(%name) > 0 )
   {
      // the first character
      %firstChar = getSubStr( %name, 0, 1 );
      // if the character is a number remove it
      if( strpos( %numbers, %firstChar ) != -1 )
      {
         %name = getSubStr( %name, 1, strlen(%name) -1 );
         %name = ltrim( %name );
      }
      else
         break;
   }
   
   // replace whitespaces with underscores
   %name = strreplace( %name, " ", "_" );
   
   // remove any other invalid characters
   %invalidCharacters = "-+*/%$&§=()[].?\"#,;!~<>|°^{}";
   %name = stripChars( %name, %invalidCharacters );
   
   if( %name $= "" )
      %name = "Unnamed";
   
   return %name;
}


//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================

//==============================================================================
function checkDatablockField(%field ) {
	//Need to run that for invalid array without bracket in code
	%strlen = strlen(%field);
	%l = getSubStr(%field,%strlen-1,1);
	if (%l $= "0" || %l $= "1" || %l $= "2" || %l $= "3" || %l $= "4" || %l $= "5" || %l $= "6" || %l $= "7" ||  %l $= "8" || %l $= "9"  ) {
		%startStr = getSubStr(%field,0,%strlen-1);
		%final = %startStr @ "[" @ %l @ "]";
		%field = %final;
	}
	return %field;
}
//------------------------------------------------------------------------------
//==============================================================================
function dumpDatablocks(  ) {
	foreach( %obj in DatablockGroup ) {
		if( %obj.isMemberOfClass( "CompositeSystemData" ) ) {
			info(%obj.getName(),"ID is:",%obj.getId());
			info(%obj.system1.getName(),"System1 ID is:",%obj.system1.getId());
			info(%obj.system2.getName(),"System2 ID is:",%obj.system2.getId());
		}
	}
}
//------------------------------------------------------------------------------


