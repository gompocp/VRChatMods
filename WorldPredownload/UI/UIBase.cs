using UnityEngine;

namespace WorldPredownload.UI
{
    internal interface UIButtonBase
    {
        public GameObject Button { get; set; }
        public void UpdateText();
    }
}