using System;
using System.Collections.Generic;
using System.Net;
using MelonLoader;
using Newtonsoft.Json;
using Semver;
using Main = UpdateChecker.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(Main), "UpdateChecker", "1.0.0", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonColor(ConsoleColor.Magenta)]

namespace UpdateChecker
{
    public partial class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            // Api fetching code comes from: https://github.com/Slaynash/VRCModUpdater/blob/main/Core/VRCModUpdaterCore.cs
            string apiResponse;
            using var client = new WebClient
            {
                Headers = {["User-Agent"] = "UpdateChecker"}
            };
            try
            {
                apiResponse = client.DownloadString("https://api.vrcmg.com/v0/mods.json");
            }
            catch (WebException e)
            {
                MelonLogger.Error($"Failed to contact api: {e.Message}");
                return;
            }
            
            List<Mod> mods = JsonConvert.DeserializeObject<List<Mod>>(apiResponse);

            if (mods == null || mods.Count == 0)
            {
                MelonLogger.Error("Didn't receive any mods from the api");
                return;
            }
            
            Dictionary<string, ModVersion> workingModsLookUpTable = new Dictionary<string, ModVersion>();
            Dictionary<string, ModVersion> brokenModsLookUpTable = new Dictionary<string, ModVersion>();
            
            foreach (var mod in mods)
            {
                if(mod.versions.Count == 0) continue;
                var modVersion = mod.versions[0];
                try
                {
                    modVersion.SemVersion = SemVersion.Parse(modVersion.modversion);
                }
                catch (ArgumentException)
                {
                    // Ignore. There's a null check that handles this later
                }
                foreach (var alias in mod.aliases)
                {
                    if(modVersion.ApprovalStatus == 2) 
                        brokenModsLookUpTable.Add(alias, modVersion);
                    else if(modVersion.ApprovalStatus == 1)
                        workingModsLookUpTable.Add(alias, modVersion);
                }
            }
            
            foreach (var melon in MelonHandler.Mods)
            {
                try
                {
                    SemVersion semVersion = SemVersion.Parse(melon.Info.Version);
                    
                    if (workingModsLookUpTable.ContainsKey(melon.Info.Name))
                    {
                        var latestVersion = workingModsLookUpTable[melon.Info.Name];
                        
                        if (latestVersion.SemVersion == null)
                            throw new ArgumentException();
                        
                        if (semVersion < latestVersion.SemVersion)
                            MelonLogger.Msg(ConsoleColor.Green,$"Mod {melon.Info.Name} by {melon.Info.Author} is out of date. {melon.Info.Version} --> {latestVersion.modversion}");
                        
                    }
                    else if (brokenModsLookUpTable.ContainsKey(melon.Info.Name))
                        MelonLogger.Msg(ConsoleColor.Yellow,$"Running currently broken mod: {melon.Info.Name} by {melon.Info.Author}");
                    
                    else if (!melon.Info.Name.Equals("UpdateChecker"))
                        MelonLogger.Msg(ConsoleColor.Blue,$"Running unknown mod: {melon.Info.Name} by {melon.Info.Author}");

                }
                catch(ArgumentException)
                {
                    MelonLogger.Msg(ConsoleColor.Red,$"MelonMod {melon.Info.Name} isn't following semver. Skipping...");   
                }
            }
        }
    }
}