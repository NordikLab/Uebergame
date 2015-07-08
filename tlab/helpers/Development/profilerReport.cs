//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profiler dumping system
//==============================================================================
//==============================================================================
//Record profile while profile bind is pressed
function doProfile(%val) {
	if (%val) {
		// key down -- start profile	
		note("Starting profile session...");
		profilerReset();
		profilerEnable(true);
	} else {
		// key up -- finish off profile
		note("Ending profile session...");
		%file = "data/profiler/quickLog_" @ getSimTime() @ ".txt";
		profilerDumpToFile(%file);
		%file = "data/profiler/"@$Cfg::Game::SessionNumber@"/prof_" @$Cfg::Game::SessionNumber@"_"@ getRealTime() @ ".txt";
		profilerDumpToFile(%file);
		profilerEnable(false);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Record profile until it's set to off
function toggleProfile(%val) {
	if (%val) {
		if($ProfilerActive) {
			$ProfilerActive = false;
			// key up -- finish off profile
			note("Ending a long profile session...");
			%file = "data/profiler/fullLog_" @ getSimTime() @ ".txt";
			profilerDumpToFile(%file);
			%file = "data/profiler/"@$Cfg::Game::SessionNumber@"/profExtent_" @$Cfg::Game::SessionNumber@"_"@ getRealTime() @ ".txt";
			profilerDumpToFile(%file);
			profilerEnable(false);
		} else {
			$ProfilerActive = true;
			// key down -- start profile
			note("Starting a long profile session...");
			profilerReset();
			profilerEnable(true);
		}
	}
}
//------------------------------------------------------------------------------



//==============================================================================
// DISPLAY DEVELOPMENT INFORMATIONS HELPERS
//------------------------------------------------------------------------------



