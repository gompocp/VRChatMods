using System;
using MelonLoader;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace StandaloneThirdPerson
{
    [RegisterTypeInIl2Cpp]
    public class QMEnableDisableListener : MonoBehaviour
    {

        public static bool Enabled;
        
        public QMEnableDisableListener(IntPtr obj0) : base(obj0)
        {
        }

        private void OnEnable()
        {
            Enabled = true;
        }

        private void OnDisable()
        {
            Enabled = false;
        }
    }
}