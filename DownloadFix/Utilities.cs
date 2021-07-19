using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DownloadFix
{
    public static class Utilities
    {
        public static void UnblockUnPackQueue()
        {
            //AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.field_Private_Nullable_1_UniTask_0.Value.Forget();
            //AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.field_Private_Boolean_0 = false; //Yes this literally fixes it
        }
        
        public static void DeselectClickedButton(GameObject button)
        {
            if (EventSystem.current.currentSelectedGameObject == button)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}