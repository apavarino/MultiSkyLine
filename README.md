# MultiSkyLine
Multiplayer mod for City Skyline


# Installation

First, locate the mod's directory. To do so, open the `Run` utility (windows + R), and type `%localappdata%`.
Then, go to `Colossal Order\Cities_Skylines\Addons\Mods`.
In this directory, create a `MSL` directory (if it does not yet exist). Then copy all the built dlls to the `Colossal Order\Cities_Skylines\Addons\Mods\MSL` directory.

# Development setup

You will need to have the game installed locally to be able to work on the project and build it.

You will then need to locate the game's installation directory, e.g. `C:\steamapps\common\Cities_Skylines\`. We will refer to this path as `/path/to/cs`.

To be able to build the project, you need to create a new environment variable called `CitiesSkylinesGameDir`. Its value needs to be the `/path/to/cs`