namespace Luthetus.Ide.RazorLib.Terminals.Models;

/*
This file might not end up having any code in it.
I'm just writing some notes for myself here.
TODO: Delete this file.

The goal is:
- Execution terminal should contain the output only for
  	the most recent run of the program.
  	- In otherwords, clear the execution terminal everytime the program is ran.
- General terminal should contain the output only for
  	the most recent (x) amount of commands.
  	- Maybe (x) amount of commands could be 10?
- Make sure that any memory used by a terminal command is fully disposed of,
  	/ getting garbage collected.
  	- What I refer to by "getting garbage collected" is that,
  	  	if I hold on to a reference to a terminal command, I do not believe
  	  	it will be garbage collected.
  	  	- So, while garbage collection is not deterministic in regards to when,
  	  	  	I should be able to see whether something NEVER is getting garbage collected,
  	  	  	and then if so, look into why.
- Test Explorer should use the execution background task queue (as it currently does),
  	and capture any output directly into a StringBuilder which is on the terminal command
  	instance (this part it doesn't do at the moment).
- Implement holding Ctrl key to show clickable links in the integrated terminal.
  	- Having the on hover tooltips in the terminal is incredibly obnoxious
  	  	and these should be replaced with the ctrl click.
  	  	- While holding Ctrl, if there is a link, then draw a presentation layer
  	  	  	for an underline under the text to indicate it is clickable.
  	  	  	- An issue with this though, if I am in the main text editor,
  	  	  	  	and hold Ctrl, I don't want to see the presentation layer
  	  	  	  	rendering and disappearing as I hold / let go of Ctrl
  	  	  	  	while in a different editor it would be annoying.
  	  	  	  	- So, maybe only render the presentation layer
  	  	  	  	  	when hovering text which maps to a clickable link.
  	  	  	  	  	
=================================
=================================

- I think the first step towards these goals is to get the test explorer output
  	to be written to a property on the terminal command.
- Then, I can do the execution terminal goal, to only track the most recent run
  	of the program.
- 
*/
