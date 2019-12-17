Game AI - Assignment 6


Alberto Mejia, Jessica Lichter


** Some notes **

Panda BTS:
	
	HarleySimulator.BT
	
	Harley.BT 

Scripts:
	
	HarleySimulator.cs
	
	Harley.cs
	
	

	HarleySimulator handles input from the user, constantly updates time, and constantly updates levels.
Most of the trees handle one input (like a tree for "F" to feed, a tree for "W" to go walk, etc). As each
minute passes, harley's levels (bladder,hunger,etc) decrease by a certain amount. At the top of the 
HarleySimulator script, where it says NOTE, you can tinker with different values for  the rate of how fast 
individual levels of Harley decrease. The tinkerRate will modify the rate that all levels decrease. HarleySimulator
also has knowledge of harley's levels and the owner's current state; an input can receive a different response depending on 
what Harley/ the owner is currently doing. For example, if you input "F" to fill Harley's bowl, but you are at work,
the game will output that this is not possible. As another example, if you input "K" to play with Harley, but she is starving,
the game will output that she is too hungry to play.

	
	Harley (the script and tree) constantly checks our doggie's levels, displays these levels, responds to needs, 
and contains behavior trees for responding to user input. These behavior trees are called in the HarleySimulator.BT 
within the trees that handle input. Harley is always checking levels and displaying them, but will fallback on responding
to what she needs most in order of priority at the same time. For example, if Harley is starving, she won't care about
anything else (won't bark to play fetch, want to go out, etc). 

	

	Hope you enjoy the game. It can use improvements but it was a lot of effort, a great learning experience, and fun to build! 
