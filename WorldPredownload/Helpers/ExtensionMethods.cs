using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using Transmtn.DTO.Notifications;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode

namespace WorldPredownload.Helpers
{
    public static class ExtensionMethods
    {
        private static LoadUserDelegate loadUserDelegate;


        private static LoadUserDelegate GetLoadUserDelegate
        {
            get
            {
                //Build 1088 menu.Method_Private_Void_ObjectNPublicAcTeAcStGaUnique_0()
                if (loadUserDelegate != null) return loadUserDelegate;
                var loadUserMethod = typeof(PageUserInfo).GetMethods().Where(
                        m =>
                            m.Name.StartsWith("Method_Public_Void_APIUser_PDM_")
                    )
                    .OrderBy(m => m.GetCustomAttribute<CallerCountAttribute>().Count)
                    .Last();
                loadUserDelegate = (LoadUserDelegate) Delegate.CreateDelegate(
                    typeof(LoadUserDelegate),
                    null,
                    loadUserMethod);
                return loadUserDelegate;
            }
        }

        public static void SetAction(this Button button, Action action)
        {
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(action);
        }

        public static void SetButtonAction(this GameObject gameObject, Action action)
        {
            gameObject.GetComponent<Button>().SetAction(action);
        }

        public static void SetButtonActionInChildren(this GameObject gameObject, Action action)
        {
            gameObject.GetComponentInChildren<Button>().SetAction(action);
        }

        public static Text GetTextComponentInChildren(this GameObject gameObject)
        {
            return gameObject.GetComponentInChildren<Text>();
        }

        public static Text GetTextComponentInChildren(this Button button)
        {
            return button.GetComponentInChildren<Text>();
        }

        public static RectTransform GetRectTrans(this Button button)
        {
            return button.GetComponent<RectTransform>();
        }

        public static RectTransform GetRectTrans(this GameObject gameObject)
        {
            return gameObject.GetComponent<RectTransform>();
        }

        public static void SetAnchoredPos(this RectTransform transform, Vector2 pos)
        {
            transform.anchoredPosition = pos;
        }

        public static void SetName(this GameObject gameObject, string name)
        {
            gameObject.name = name;
        }

        public static void SetText(this GameObject gameObject, string text)
        {
            gameObject.GetTextComponentInChildren().text = text;
        }

        public static void SetText(this Button button, string text)
        {
            button.GetTextComponentInChildren().text = text;
        }

        public static string GetWorldID(this Notification notification)
        {
            return notification.details["worldId"].ToString().Split(':')[0];
        }

        public static string GetInstanceIDWithTags(this Notification notification)
        {
            return notification.details["worldId"].ToString().Split(':')[1];
        }

        public static string GetInstanceIDWithTags(this ApiWorld apiWorld)
        {
            return apiWorld.id.Split(':')[1];
        }

        public static void LoadUser(this PageUserInfo pageUserInfo, APIUser apiUser)
        {
            GetLoadUserDelegate(pageUserInfo, apiUser);
        }

        public static bool HasStringLiterals(this MethodInfo m)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null) return true;
                }
                catch
                {
                }

            return false;
        }

        public static bool CheckStringsCount(this MethodInfo m, int count)
        {
            var total = 0;
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null) total++;
                }
                catch
                {
                }

            return total == count;
        }

        public static bool HasMethodCallWithName(this MethodInfo m, string txt)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (instance.TryResolve().Name.Contains(txt)) return true;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return false;
        }

        public static bool SameClassMethodCallCount(this MethodInfo m, int calls)
        {
            var count = 0;
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (m.DeclaringType == instance.TryResolve().DeclaringType) count++;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return count == calls;
        }

        public static bool HasMethodWithDeclaringType(this MethodInfo m, Type declaringType)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (declaringType == instance.TryResolve().DeclaringType) return true;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return false;
        }

        private delegate void LoadUserDelegate(PageUserInfo pageUserInfo, APIUser apiUser);
    }
}