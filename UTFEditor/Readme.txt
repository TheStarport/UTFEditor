Freelancer UTF Editor

This is a version of the Freelancer UTF editor. It continues the work of Colin Sanby and Mario 'HCl' Brito.

You should be able to find the latest version of this software at http://the-starport.net.

This program is free software. You may do what you like with it but don't forget to the credit the people who made this possible.

==== Changes in 2.1 ====

* Reads the model materials from the parent and grandparent directories; expands the root node automatically; and shows the model for .3db and .sph files.
* Displays the model scale as a number; removes the shortcut keys from the node's right-click menu, adding accelerator keys to the buttons (and adds a rename button, tweaks the rename dialog). The diff is still based on the original release.
* Allows specification of model center point in model viewer.

==== Changes in 2.0 ====

* Added animation channel editor.
* Added automatic CRC update for VMeshRefs when the VMeshData name is edited
* Added model view that updates when Pris/Rev/Fix data is edited.
* Added VMeshRef editor.
* Added a slighty buggy yaw/pitch/roll rotation interpreter for the fix editor (it's good enough and I'm sick of euler/matrix conversions.)

==== To do ====

* Complete support for all FVFs.
* Improve euler angle conversions.
* Add tga/dds viewer.
* Add hard point viewer/editor
* Fix/improve mouse based center position movement.

==== Credits ==== 

* Adoxa - Fixes and improvements for 2.1
* Cannon - Wrote version 2.0, added the model viewer and other random features.
* FriendlyFire - Added hardpoint support to model viewer and miscellaneous features and bug fixes.
* w0dk4 - Bugfixes and support for normal mapping-enabled models.

Additional thanks to the following people:
* Mario 'HCl' Brito - UTF (CMP) file structures and for releasing the source code to the original utf_edit program.
* Colin Sanby - CMP Exporter, SUR Importer, SurDump.exe and the many structures he managed to deduce.
* Anton and FLModelTool for providing examples and structures.
* LancerSolurus for more SUR structure information.
* Matrix/Euler angle conversions by Martin Baker http://www.euclideanspace.com/
* Thanks to all of the other people who have contributed to the ongoing improvements of Freelancer.

==== Installation ====

This program needs Directx 9 and .NET 3.5 client libraries installed.

==== Build ====

Visual Studio 2008 C# Express
DirectX SDK August 2007
