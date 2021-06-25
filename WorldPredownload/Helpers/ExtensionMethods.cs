using System;
using Transmtn.DTO.Notifications;
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
            Delegates.GetLoadUserDelegate(pageUserInfo, apiUser);
        }
    }
}