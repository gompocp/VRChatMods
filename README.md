# Disclaimer

Modding the VRChat client is against VRChat's Terms of Service. Therefore use these mods at your own risk. I am not responsible for if you get banned or any other punishment by using these mods!<br>

## Mods
<ol>
  <li><a href="#actionmenuapi">ActionMenuApi</a></li>
  <li><a href="#actionmenuutils">ActionMenuUtils</a></li>
  <li><a href="#downloadfix">DownloadFix</a></li>
  <li><a href="#worldpredownload">WorldPredownload</a></li>
</ol>

## Canny Posts



I highly recommend checking out the canny posts linked below and upvoting if you'd like VRChat to implement some mods as actual vanilla features
* [World Predownloading/Preloading](https://feedback.vrchat.com/feature-requests/p/preload-worlds)
* [Avatar/Loading Screen 100% Bug](https://feedback.vrchat.com/bug-reports/p/vrchat-loading-screen-100-stuck-bug)
* [Another Avatar/Loading Screen 100% Bug](https://feedback.vrchat.com/bug-reports/p/still-consistently-stuck-in-loading-screen-at-100-if-avatars-dont-finish-downloa)
* [Menu Respawn Issue ActionMenuUtils addresses](https://feedback.vrchat.com/feature-requests/p/respawnhub-hotkey)
* [Useable menu while falling](https://feedback.vrchat.com/feature-requests/p/useable-menu-while-moving-or-falling)
* [Not currently a mod feature but priority for friend's avatars first](https://feedback.vrchat.com/feature-requests/p/friends-first-model-priority-loading)


## Building

To Build simply
1. Clone repo using git
2. Copy `*.dlls` from `MelonLoader\Managed` to `Libs` folder
3. Copy in `MelonLoader.dll`,`ActionMenuApi.dll` and `UIExpansionKit.dll` too
4. Build Solution

## ActionMenuApi

This mod doesn't do anything on it's own. <br>
It provides an easy way for modders to add integration with the action menu. <br>
It supports the use of the <br>
* Radial Puppet
* Four Axis Puppet
* Button
* Toggle Button
* Sub Menus

Additionally allows mods to add their menus to a dedicated section on the action menu to prevent clutter. <br>
More information [here](https://github.com/gompocp/ActionMenuApi)

## ActionMenuUtils 
- Lets you respawn using the action menu<br>
- Lets you go home for when respawning wont save you such as in broken worlds with no floor<br>
- Additionally lets you reset avatar or rejoin instance<br>
- Shows an example of how you might use my action menu api thingy :) more info available [here](https://github.com/gompocp/ActionMenuApi/) <br>


### Credits<br>
- [Knah](https://github.com/knah/) for the assetbundle loading mechanism from https://github.com/knah/VRCMods,<br>
  for suggesting to add a button for go home and just help in general :)
- [Ben](https://github.com/BenjaminZehowlt/) for xref scanning mechanism from https://github.com/BenjaminZehowlt/DynamicBonesSafety
- SOS emoji from https://openmoji.org/ full credit to them for it


## DownloadFix

Hi, <br>
The devs are very much aware of this bug. According to some "secret intel" they think they know how to fix it so hopefully it should come soon™ and when that day comes if I catch a single person saying "oh wow about time they fixed it blah blah blah we had a mod that fixed this forever blah blah devs stealing code" I _will_ personally bonk you on the head. My fix is crap to say the least and useless to them and I can guarantee you that their eventual fix wont = my "fix". So be grateful towards VRChat rather than hateful when it comes out eventually. <br> Thanks!  <br> gompo :)

- Yeah its literally a one line fix for the loading bug  <br>
- Adds a button to the UIX settings page and to the world loading screen <br>
- Don't really know what else to say about it so yeah <br>

## WorldPredownload<br>
- Credit to [Psychloor](https://github.com/Psychloor/AdvancedInvites/blob/master/AdvancedInvites/InviteHandler.cs) for method to convert worldID to apiWorld instance + much much more, and some of the way stuff is structured is also inspired by his utilities file
- Credit to [Ben](https://github.com/BenjaminZehowlt/DynamicBonesSafety) for xref scanning mechanism and hud icon idea
- Credit to [Knah](https://github.com/knah/) for [Enable Listener](https://github.com/knah/VRCMods/blob/master/UIExpansionKit/Components/EnableDisableListener.cs) and the [hud icon method](https://github.com/knah/VRCMods/blob/master/JoinNotifier/JoinNotifierMod.cs#L120) that I changed to use here along with the [assetbundle loading mechanism](https://github.com/knah/VRCMods/blob/master/JoinNotifier/JoinNotifierMod.cs#L61)
- fwenny for helping with some testing 
- You can hit preload on an invite, on a world page or on a friend user page
- You can see your download status bottom right of quick menu 
- Currently you can only download/preload one world at a time 
- If you go to another world while downloading, it'll cancel the download 
- Performance may degrade for split second on world enter due to you having a large cache size 


Repo structure from [Knah's VRCMods Repo](https://github.com/knah/VRCMods/)
