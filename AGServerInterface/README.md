AGServerInterface
=================

 

This is the interface for the game reader engines.

if you are developing a game reader then please ensure that it implements this
interface.

 

Interface elements
------------------

 

### Attributes

**Name** This is the short name of the game

**DisplayName** This is the long name of the game

**ProcessNames** This is an array of process names for the game

 

 

### Methods

**Start** This method is called by the server when a game is detected. This
method should start reading data.

**Stop** This method is called by the server when a running game disappears,
i.e. the user closes it.

 

### Events

**GameEvent** This is NOT currently implemented but will allow for game readers
to fire events back to the core server.

 

 

 

 
