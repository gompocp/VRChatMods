using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace WorldPredownload.UI
{
    public static class HudIcon
    {
        private static AssetBundle iconsAssetBundle;
        private static Sprite hudIcon;
        private static Image heavy;
        private static Image fade;

        public static void Setup()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WorldPredownload.gompowpd"))
            using (var tempStream = new MemoryStream((int) stream.Length))
            {
                stream.CopyTo(tempStream);

                iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            hudIcon = iconsAssetBundle.LoadAsset_Internal("Assets/DownloadIcon.png", Il2CppType.Of<Sprite>())
                .Cast<Sprite>();
            hudIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            var main = CreateImage("DownloadStatusProgress");
            var secondary = CreateImage("DownloadStatusTransparent");
            secondary.color = new Color(1, 1, 1, 0.4f);
            main.type = Image.Type.Filled;
            main.fillMethod = Image.FillMethod.Horizontal;
            main.fillOrigin = (int) Image.OriginHorizontal.Left;
            main.fillAmount = 0f;
            heavy = main;
            fade = secondary;
            Disable();
        }

        private static Image CreateImage(string name)
        {
            var hudRoot = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud");
            var requestedParent = hudRoot.transform.Find("NotificationDotParent");
            var indicator = Object
                .Instantiate(hudRoot.transform.Find("NotificationDotParent/NotificationDot").gameObject,
                    requestedParent, false).Cast<GameObject>();
            indicator.name = "NotifyDot-" + name;
            indicator.SetActive(true);
            indicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(-145, 110);
            var image = indicator.GetComponent<Image>();
            image.sprite = hudIcon;
            image.enabled = true;
            return image;
        }

        public static void Enable()
        {
            heavy.gameObject.SetActive(true);
            fade.gameObject.SetActive(true);
        }

        public static void Disable()
        {
            heavy.gameObject.SetActive(false);
            fade.gameObject.SetActive(false);
        }

        public static void Update(float f)
        {
            heavy.fillAmount = f;
        }
    }
}