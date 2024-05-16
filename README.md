# Description
This program is designed to move footage created while playing [Content Warning](https://store.steampowered.com/app/2881650/Content_Warning/) into a separate folder on your desktop instead of turning your desktop into a video trash bin. Enjoy your clean space :)

# Usage
#### Script-Mode
By launching the program by double-clicking `LMB`, the program will enumerate all files on the desktop that match the search criteria and move them to the appropriate location.
#### Observer-Mode
By running the program with the argument `-a` or `--auto` you will activate the observer mode and the program will try to catch each video right at the saving stage
#### Information
By launchung program with the argument `-i` or `--info` you will see some information about the operation of the program (currently only the directory for saving footage is shown)
#### Help
Run the program with the argument `-h` or `--help` to get a description of the program and arguments
### Сhanging the save directory
The save directory can be changed if you pass the `-d` or `--dir` parameter to the program and specify the path to the existing folder as the second parameter. The use of environment variables is supported. For example, running the program with the parameters `-d %UserProfile%\Documents`, the new saving directory will be `C:\Users\YourUser\Documents\Content Warning`

# Locations
Scanning directory : `%UserProfile%\Desktop` <br />
Default save location : `%UserProfile%\Desktop`
