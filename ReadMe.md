Redistribution of the .Net 4.7.2 Harmony V2 modding library for Kerbal Space Program

Available at : https://github.com/HarmonyKSP/HarmonyKSP

Original distribution : https://github.com/pardeike/Harmony

Licence : MIT

### KSP PLAYERS :
This is a library used by other KSP mods. It doesn't provide any functionality in-game, but is required by other mods.
To install it, copy the `000_Harmony` folder into your `GameData` folder.

### KSP MODDERS :
If you are using Harmony, or planning to use Harmony for your modding needs, please redirect your players to this community maintained version instead of bundling the orginal to avoid version conflicts.

Alternatively, you can also re-bundle the `GameData\000_Harmony` folder in your mod distribution, but be sure to always bundle the latest version.

That distribution is also available on [CKAN](https://github.com/KSP-CKAN/NetKAN/blob/master/NetKAN/Harmony2.netkan) under the `Harmony2` identifer, use the following `depends` in your `*.netkan` metadata file to add the dependency :
```
"depends": [ { "name": "Harmony2" } ],
```
Or ask (nicely) the CKAN people to sort it for you : [CKAN Discord](https://discord.gg/Mb4nXQD) 
