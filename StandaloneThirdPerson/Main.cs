using System;
using System.Collections;
using HarmonyLib;
using MelonLoader;
using UIExpansionKit.API;
using UnityEngine;
using Main = StandaloneThirdPerson.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(Main), "StandaloneThirdPerson", "1.0.0", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]

namespace StandaloneThirdPerson
{
    internal partial class Main : MelonMod
    {
        private static CameraMode currentMode = CameraMode.Normal;

        public override void OnApplicationStart()
        {
	        ModSettings.RegisterSettings();
	        ModSettings.LoadSettings();
	        MelonCoroutines.Start(WaitForUIInit());
        }

        private static IEnumerator WaitForUIInit()
        {
	        while (VRCUiManager.prop_VRCUiManager_0 == null)
		        yield return new WaitForEndOfFrame();
			OnUIInit();
        }
        
        private static Camera thirdPersonCamera;
        private static Camera vrcCamera;
        private static bool initialised;
        internal static bool Allowed;

        private static void OnUIInit()
        {
	        var temp = GameObject.Find("Camera (eye)");

	        if (temp == null)
	        {
		        MelonLogger.Error("Couldn't find camera... mod won't run...");
		        return;
	        }
	        
	        vrcCamera = temp.GetComponent<Camera>();
	        var originalCameraTransform = vrcCamera.transform;
	        thirdPersonCamera = new GameObject("Standalone ThirdPerson Camera").AddComponent<Camera>();
	        thirdPersonCamera.fieldOfView = 80f;
	        thirdPersonCamera.nearClipPlane = 0.01f;
	        thirdPersonCamera.enabled = false;
	        thirdPersonCamera.transform.parent = originalCameraTransform.parent;

	        GameObject.Find("UserInterface/QuickMenu/MicControls").AddComponent<QMEnableDisableListener>();
	        
	        initialised = true;
        }

        private void RepositionCamera(bool isBehind)
        {
	        var vrcCameraTransform = vrcCamera.transform;
	        var thirdPersonCameraTransform = thirdPersonCamera.transform;
	        thirdPersonCameraTransform.position = vrcCameraTransform.position +  (isBehind ? -vrcCameraTransform.forward : vrcCameraTransform.forward);
	        thirdPersonCameraTransform.LookAt(vrcCameraTransform);
	        thirdPersonCameraTransform.position += thirdPersonCameraTransform.forward * +0.25f; // Reverse + = In  && - = Out
        }

        public static void UpdateCameraSettings()
        {
	        if (thirdPersonCamera == null) return;
	        thirdPersonCamera.fieldOfView = ModSettings.FOV;
	        thirdPersonCamera.nearClipPlane = ModSettings.NearClipPlane;
        }
        public override void OnUpdate()
        {
	        
	        if (!Allowed || Utils.IsBigMenuOpen() || QMEnableDisableListener.Enabled)
		        return;
	        

	        if (initialised)
	        {
		        if (Input.GetKeyDown(ModSettings.KeyBind))
		        {
			        currentMode++;
			        if (currentMode > CameraMode.InFront) currentMode = CameraMode.Normal;
			        if (currentMode != CameraMode.Normal)
			        {
				        RepositionCamera(currentMode == CameraMode.Behind);
				        thirdPersonCamera.enabled = true;
			        }
			        else
			        {
				        thirdPersonCamera.enabled = false;
			        }
		        }

		        if (currentMode != CameraMode.Normal)
		        {
			        if (Input.GetKeyDown(KeyCode.Escape))
			        {
				        currentMode = CameraMode.Normal;
				        thirdPersonCamera.enabled = false;
			        }
			        thirdPersonCamera.transform.position += (thirdPersonCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel"));
		        }
	        }
        }
        
        public override void OnPreferencesLoaded()
        {
	        ModSettings.LoadSettings();
        }

        public override void OnPreferencesSaved()
        {
	        ModSettings.LoadSettings();
        }

        [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
        internal class OnJoinedRoomPatch
        {
	        private static void Prefix()
	        {
		        currentMode = CameraMode.Normal;
		        Allowed = false;
		        MelonCoroutines.Start(Utils.CheckWorld());
	        }
        }
    }
}