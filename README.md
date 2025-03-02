# MultiSkyLine
Multiplayer mod for City Skyline


# Installation

First, locate the mod's directory. To do so, open the `Run` utility (windows + R), and type `%localappdata%`.
Then, go to `Colossal Order\Cities_Skylines\Addons\Mods`.
In this directory, create a `MSL` directory (if it does not yet exist). Then copy all the built dlls to the `Colossal Order\Cities_Skylines\Addons\Mods\MSL` directory.

# Development setup

You will need to have the game installed locally to be able to work on the project and build it.

You will then need to locate the game's installation directory, e.g. `C:\steamapps\common\Cities_Skylines\`. 
For the remainder of this setup, we will refer to this path as `/path/to/cs_data/`.

To be able to build the project, you will need to import a few dlls in your project.

## Rider (JetBrain):

1. Right-click on your project name, under `Add` click on `Reference`.
2. Then, click on the `Add from...` button at the bottom.
3. Navigate to `/path/to/cs_data/Cities_Data/Managed`, and select all the `.dll` files.
4. Once imported, your IDE should be ready.


## Visual Studio (Microsoft):
TODO