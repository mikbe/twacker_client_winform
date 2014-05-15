# Twacker  
## A text-to-speech chat reader for Twitch

### Instructions

1. Start the program.
2. Enter the user name you want to log in to twitch as. Preferably this should be a second account you setup.
3. Enter the channel you want to watch. This should be your main channel.
4. Now the Twitch authorization login will appear. Login.
5. If all went well you'll be logged into the IRC server and chat will be read to you.

#### Buttons

**Config**  
Let's you change the channel or user.  

**Erase all settings**  
Exactly what it sounds like.  
It also forces a logout from Twitch just to be safe.  
This is really just for testing: if you change your  
user name it forces a log out as well.  

### To Do

This is a VERY basic app right now and still needs quite a few things.  

* Make it look like not-poo.
* Volume control.
* Refactor out coupling with settings and Speech.
* Create modular plug-in system. (you can add your own modules)
* Follower visual alert for apps like OBS (basically output a file for OBS to read)
* Add more settings so you don't have to edit config files to change stuff.
* Extract speech text to settings file/config menu.
* Configurable timers to remind people to "hit that follow button."
* Chat overlay for games - output the chat directly onto your game.
* Multiple channels? This could get very confusing but if you had different voices...
* Allow selection of installed voices with preview.

### Bugs

* Sometimes it just won't restart. I think that's because a thread is left dangling. Need to find and fix that.

## License

<a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/"><img alt="Creative Commons License" style="border-width:0" src="http://i.creativecommons.org/l/by-sa/4.0/88x31.png" /></a><br /><span xmlns:dct="http://purl.org/dc/terms/" property="dct:title">Twacker Client</span> by <a xmlns:cc="http://creativecommons.org/ns#" href="http://mikebethany.com" property="cc:attributionName" rel="cc:attributionURL">Mike Bethany</a> is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/">Creative Commons Attribution-ShareAlike 4.0 International License</a>.<br />Based on a work at <a xmlns:dct="http://purl.org/dc/terms/" href="https://github.com/mikbe/twacker_client_winform" rel="dct:source">https://github.com/mikbe/twacker_client_winform</a>.