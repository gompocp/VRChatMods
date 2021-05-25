using System;
using System.IO;
using System.Reflection;
using BetterVRCKeyboard;
using gompoCommon;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[assembly:MelonGame("VRChat", "VRChat")]
[assembly:MelonInfo(typeof(VRCKeyboardTweaks.Main), "VRCKeyboardTweaks", "1.0.0", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]

namespace VRCKeyboardTweaks
{
    public class Main : MelonMod
    {
        private static AssetBundle iconsAssetBundle = null;
        private static AudioSource clickAudioSource;
        private static GameObject inputPopup;
        public override void VRChat_OnUiManagerInit()
        {
            ModSettings.RegisterSettings();
            inputPopup = GameObject.Find("UserInterface/MenuContent/Popups/InputPopup");
            SetKeyboardScale(ModSettings.KeyboardScale);
            AudioSource audioSource = GameObject.Find("UserInterface/MenuContent/Popups/InputPopup").AddComponent<AudioSource>();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VRCKeyboardTweaks.bettervrckeyboard"))
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);
                iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
            audioSource.clip = iconsAssetBundle.LoadAsset_Internal("Assets/BetterVRCKeyboard/Click1.wav", Il2CppType.Of<AudioClip>()).Cast<AudioClip>();
            audioSource.clip.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            SetClickVolume(ModSettings.ClickVolume);

            GameObject clear = CloneGameObject("UserInterface/MenuContent/Popups/InputPopup/ButtonRight", "UserInterface/MenuContent/Popups/InputPopup");
            clear.name = "ClearButton";
            clear.GetComponentInChildren<Text>().text = "Clear";
            clear.transform.localPosition = new Vector2(450, 160);
            var clearButton = clear.GetComponent<Button>();
            clearButton.onClick = new Button.ButtonClickedEvent();
            clearButton.onClick.AddListener(new Action(() =>
            {
                DeselectClickedButton(clear);
                GameObject.Find("UserInterface/MenuContent/Popups/InputPopup/InputField").GetComponent<InputField>().text = String.Empty;
                audioSource.Play();
                if(ModSettings.UseTweening) DOTweenWrapper.Punch(() => clearButton.gameObject.transform.localScale, x => clearButton.gameObject.transform.localScale = x, new Vector3(0.3f, 0.3f, 0), 0.1f, 2, 0.4f);
                
            }));
            
            foreach (var button in  GameObject.Find("UserInterface/MenuContent/Popups/InputPopup/Keyboard/Keys").GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(new Action(() =>
                {
                    audioSource.Play();
                    if(ModSettings.UseTweening) DOTweenWrapper.Punch(() => button.gameObject.transform.localScale, x => button.gameObject.transform.localScale = x, new Vector3(0.3f, 0.3f, 0), 0.1f, 2, 0.4f);
                }));
            }
        }
        public static GameObject CloneGameObject(string pathToGameObject, string pathToParent)
        {
            return GameObject.Instantiate(GameObject.Find(pathToGameObject).transform, GameObject.Find(pathToParent).transform).gameObject;
        }
        public static void DeselectClickedButton(GameObject button)
        {
            if (EventSystem.current.currentSelectedGameObject == button)
                EventSystem.current.SetSelectedGameObject(null);
        }

        public static void SetClickVolume(float volume)
        {
            if(clickAudioSource != null) 
                clickAudioSource.volume = volume;
        }
        
        public static void SetKeyboardScale(float scale)
        {
            if(inputPopup != null) 
                inputPopup.transform.localScale = new Vector3(scale, scale, scale);
        }

        public override void OnPreferencesSaved()
        {
            ModSettings.LoadSettings();
        }

        public Main()
        {
            LoaderCheck.CheckForRainbows();
        }
    }
}