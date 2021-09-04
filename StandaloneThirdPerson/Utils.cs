using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using UnityEngine.Networking;
using VRC.Core;

namespace StandaloneThirdPerson
{
    public static class Utils
    {
        public static bool IsBigMenuOpen()
        {
            return VRCUiManager.field_Private_Static_VRCUiManager_0.field_Internal_Dictionary_2_String_VRCUiPage_0.Count > 0;
        }
        
        
        // Credits to Psychloor for this: https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/Utilities.cs#L76
        internal static IEnumerator CheckWorld()
        {
            var worldId = RoomManager.field_Internal_Static_ApiWorld_0.id;

            // Check if black/whitelisted from EmmVRC - thanks Emilia and the rest of EmmVRC Staff
            var uwr = UnityWebRequest.Get($"https://dl.emmvrc.com/riskyfuncs.php?worldid=   {worldId}");
            uwr.SendWebRequest();
            while (!uwr.isDone)
                yield return new WaitForEndOfFrame();
            var result = uwr.m_DownloadHandler.text?.Trim().ToLower();
            uwr.Dispose();
            if (!string.IsNullOrWhiteSpace(result))
                switch (result)
                {
                    case "allowed":
                        Main.Allowed = true;
                        yield break;

                    case "denied":
                        Main.Allowed = false;
                        yield break;
                }


            // no result from server or they're currently down
            // Check tags then. should also be in cache as it just got downloaded
            API.Fetch<ApiWorld>(
                worldId,
                new Action<ApiContainer>(
                    container =>
                    {
                        ApiWorld apiWorld;
                        if ((apiWorld = container.Model.TryCast<ApiWorld>()) != null)
                        {
                            foreach (var worldTag in apiWorld.tags)
                                if (worldTag.IndexOf("game", StringComparison.OrdinalIgnoreCase) != -1
                                    || worldTag.IndexOf("club", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    Main.Allowed = false;
                                    return;
                                }

                            Main.Allowed = true;
                        }
                        else
                        {
                            MelonLogger.Error("Failed to cast ApiModel to ApiWorld");
                        }
                    }),
                disableCache: false);
        }
    }
}