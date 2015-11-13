NWNDotNet
=========
Started by Baaleos : July 2014

Intent is to create a .Net sandbox environment that can be initiated from within NWNScript, but then freely able to interact with NWN Resources without interfering with 
the stack in a negative way.


Proposed method is to hook MainLoop, and then have MainLoop monitor a queue of actions pending solving. 
When the loop runs, it will loop through each action, and execute each one before continuing on with the next interation of the MainLoop.

This will preserve the stack of anything before or after the iteration of the MainLoop
testing