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
            return VRCUiManager.field_Private_Static_VRCUiManager_0.field_Internal_Dictionary_2_String_VRCUiPage_0
                .Count > 0;
        }
    }
}
