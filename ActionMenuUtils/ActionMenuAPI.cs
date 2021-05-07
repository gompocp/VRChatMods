using System;
using System.Collections.Generic;
using System.Linq;
//using ActionMenu = ActionMenu;
//using PedalOption = PedalOption;
//using ActionMenuOpener = ActionMenuOpener;
//using ActionMenuDriver = ActionMenuDriver;
using PedalOptionTriggerEvent = PedalOption.MulticastDelegateNPublicSealedBoUnique; //Will this change?, ¯\_(ツ)_/¯
using ActionMenuPage = ActionMenu.ObjectNPublicAcTeAcStGaUnique;  //Will this change?, ¯\_(ツ)_/¯x2
using UnityEngine;
using Harmony;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;
using MelonLoader;
using UnhollowerRuntimeLib;

//Made by gompo#6956 you are free to use this provided credit is given and 
//your mod is licensed under the same license as the one in this repository 
//for more complicated situations please dm me on discord
//Credits:  
//-Function XRefCheck here adapted to use string lists rather than just strings from Ben's 
//  (Ben 🐾#3621) Dynamic Bone Safety Mod, link: https://github.com/BenjaminZehowlt/DynamicBonesSafety/blob/master/DynamicBonesSafetyMod.cs

namespace ActionMenuUtils
{
    internal class ActionMenuAPI
    {
        public static List<PedalStruct> configPagePre = new List<PedalStruct>(); //I'm lazy so I made these two public for my mod
        public static List<PedalStruct> configPagePost = new List<PedalStruct>();
        private static List<PedalStruct> emojisPagePre = new List<PedalStruct>();
        private static List<PedalStruct> emojisPagePost = new List<PedalStruct>();
        private static List<PedalStruct> expressionPagePre = new List<PedalStruct>();
        private static List<PedalStruct> expressionPagePost = new List<PedalStruct>();
        private static List<PedalStruct> sdk2ExpressionPagePre = new List<PedalStruct>();
        private static List<PedalStruct> sdk2ExpressionPagePost = new List<PedalStruct>();
        private static List<PedalStruct> mainPagePre = new List<PedalStruct>();
        private static List<PedalStruct> mainPagePost = new List<PedalStruct>();
        private static List<PedalStruct> menuOpacityPagePre = new List<PedalStruct>();
        private static List<PedalStruct> menuOpacityPagePost = new List<PedalStruct>();
        private static List<PedalStruct> menuSizePagePre = new List<PedalStruct>();
        private static List<PedalStruct> menuSizePagePost = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesPagePre = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesPagePost = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesOpacityPagePre = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesOpacityPagePost = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesVisibilityPagePre = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesVisibilityPagePost = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesSizePagePre = new List<PedalStruct>();
        private static List<PedalStruct> nameplatesSizePagePost = new List<PedalStruct>();
        private static List<PedalStruct> optionsPagePre = new List<PedalStruct>();
        private static List<PedalStruct> optionsPagePost = new List<PedalStruct>();

        private static List<string> openConfigPageKeywords = new List<string>(new string[] { "Menu Size", "Menu Opacity" });
        private static List<string> openMainPageKeyWords = new List<string>(new string[] { "Options", "Emojis" });
        private static List<string> openMenuOpacityPageKeyWords = new List<string>(new string[] { "{0}%" });
        private static List<string> openEmojisPageKeyWords = new List<string>(new string[] { " ", "_" });
        private static List<string> openExpressionMenuKeyWords = new List<string>(new string[] { "Reset Avatar" });
        private static List<string> openOptionsPageKeyWords = new List<string>(new string[] { "Config" }); //"Nameplates" and "Close Menu" are ones as well but that update hasnt dropped yet
        private static List<string> openSDK2ExpressionPageKeyWords = new List<string>(new string[] { "EMOTE{0}" });

        // This isnt out yet on the stable branch sooo no idea if these will work properly when it drops
        private static List<string> openNameplatesOpacityPageKeyWords = new List<string>(new string[] { "100%", "80%", "60%", "40%", "20%", "0%" });
        private static List<string> openNameplatesPageKeyWords = new List<string>(new string[] { "Visibility", "Size", "Opacity" });
        private static List<string> openNameplatesVisibilityPageKeyWords = new List<string>(new string[] { "Nameplates Shown", "Icons Only", "Nameplates Hidden" });
        private static List<string> openNameplatesSizePageKeyWords = new List<string>(new string[] { "Large", "Medium", "Normal", "Small", "Tiny" });
        

        private static List<string> openMenuSizePageKeyWords = new List<string>(new string[] { "XXXXXXXXX" }); // No strings found :( Unusable for now. Scanning for methods doesnt help either as there are other functions that yield similar results


        



        /// <summary>
        /// Creates a new instance of ActionMenuApi and patches all required methods if found
        /// </summary>
        public ActionMenuAPI()
        {
             /*
            foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openMainPageKeyWords)))
            {
                MelonLogger.Log("Found Main Page: " + methodBase.Name);
                harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenMainPagePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenMainPagePost))));
                break;
            }
            
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openConfigPageKeywords)))
           {
               MelonLogger.Log("Found Config Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenConfigPagePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenConfigPagePost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openMenuOpacityPageKeyWords)))
           {
               MelonLogger.Log("Found Menu Opacity Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenMenuOpacityPagePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenMenuOpacityPagePost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openEmojisPageKeyWords)))
           {
               MelonLogger.Log("Found Emojis Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenEmojisPagePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenEmojisPagePost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openExpressionMenuKeyWords)))
           {
               MelonLogger.Log("Found Expression Menu Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenExpressionMenuPre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenExpressionMenuPost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openNameplatesOpacityPageKeyWords)))
           {
               MelonLogger.Log("Found Nameplates Opacity Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesOpacityPre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesOpacityPost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openNameplatesPageKeyWords)))
           {
               MelonLogger.Log("Found Namesplates Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesPagePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesPagePost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openNameplatesVisibilityPageKeyWords)))
           {
               MelonLogger.Log("Found Namesplates Visibility Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesVisibilityPre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesVisibilityPost))));
               break;
           }
           foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openNameplatesSizePageKeyWords)))
           {
               MelonLogger.Log("Found Namesplates Size Page:" + methodBase.Name);
               harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesSizePre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenNameplatesSizePost))));
               break;
           }     
           */
            foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openOptionsPageKeyWords)))
            {

                MelonLogger.Msg("Found Options Page:" + methodBase.Name);
                Main.HarmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuAPI).GetMethod(nameof(OpenOptionsPre))), new HarmonyMethod(typeof(ActionMenuAPI).GetMethod(nameof(OpenOptionsPost))));
                break;
            }
            /*
            foreach (MethodBase methodBase in typeof(ActionMenu).GetMethods().Where(m => m.Name.StartsWith("Method") && checkXref(m, openSDK2ExpressionPageKeyWords)))
            {
                MelonLogger.Log("Found SDK2 Expression Page:" + methodBase.Name);
                harmonyInstance.Patch(methodBase, new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenSDK2ExpressionPre))), new HarmonyMethod(typeof(ActionMenuApi).GetMethod(nameof(OpenSDK2ExpressionPost))));
                break;
            }*/

 
        }



        /// <summary>
        /// Adds a custom Pedal to an already existing menu in vrchat. Options are in enum ActionMenuPageType
        /// </summary>
        /// <param name="pageType">The page type you want to add your pedal to</param>
        /// <param name="triggerEvent">Called on pedal click</param>
        /// <param name="text">Pedal Text</param>
        /// <param name="icon">Pedal Icon</param>
        /// <param name="insertion">Whether to place your pedal before or after vrchat places its own</param>
        public PedalStruct AddPedalToExistingMenu(ActionMenuPageType pageType, System.Action triggerEvent, string text = "Button Text", Texture2D icon = null, Insertion insertion = Insertion.Post)
        {
            PedalStruct customPedal = new PedalStruct(text, icon, triggerEvent);
            switch (pageType)
            {
                case ActionMenuPageType.Config:
                    if (insertion == Insertion.Pre) configPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) configPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.Emojis:
                    if (insertion == Insertion.Pre) emojisPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) emojisPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.Expression:
                    if (insertion == Insertion.Pre) expressionPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) expressionPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.Main:
                    if (insertion == Insertion.Pre) mainPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) mainPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.MenuOpacity:
                    if (insertion == Insertion.Pre) menuOpacityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) menuOpacityPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.MenuSize:
                    if (insertion == Insertion.Pre) menuSizePagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) menuSizePagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.Nameplates:
                    if (insertion == Insertion.Pre) nameplatesPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) nameplatesPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.NameplatesOpacity:
                    if (insertion == Insertion.Pre) nameplatesOpacityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) nameplatesOpacityPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.NameplatesSize:
                    if (insertion == Insertion.Pre) nameplatesSizePagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) nameplatesSizePagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.NameplatesVisibilty:
                    if (insertion == Insertion.Pre) nameplatesVisibilityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) nameplatesVisibilityPagePost.Add(customPedal);
                    break;
                case ActionMenuPageType.Options:
                    if (insertion == Insertion.Pre) optionsPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) optionsPagePost.Add(customPedal);
                    break;
            }
            return customPedal;
        }

        /// <summary>
        /// call this in a triggerevent for an already existing pedal to open a new submenu
        /// Add pedals to this in the onStart param using AddPedalToCustomMenu()
        /// </summary>
        /// <param name="openFunc">Called on submenu open whats the difference... I dunno it exists tho</param>
        /// <param name="closeFunc">Called on submenu close</param>
        /// <param name="icon">Pedal Icon</param>
        /// <param name="text">title</param>
        public ActionMenuPage CreateSubMenu(System.Action openFunc, System.Action closeFunc = null, Texture2D icon = null, string text = null)
        {
            if (GetActionMenuOpener() == null) return null;

            return GetActionMenuOpener().field_Public_ActionMenu_0.PushPage(openFunc, closeFunc, icon, text);
        }

        /// <summary>
        /// If you create a custom Submenu you can use this to add your pedals to it
        /// </summary>
        /// <param name="triggerEvent">Called on pedal click</param>
        /// <param name="text">Pedal Text</param>
        /// <param name="icon">Pedal Icon</param>
        public PedalOption AddPedalToCustomMenu(System.Action triggerEvent, string text = "Button Text", Texture2D icon = null)
        {
            ActionMenuOpener actionMenuOpener = GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            PedalOption pedalOption = actionMenuOpener.field_Public_ActionMenu_0.AddOption();
            pedalOption.setText(text); //    VVV
            pedalOption.setIcon(icon); //Pretty self explanatory
            pedalOption.field_Public_MulticastDelegateNPublicSealedBoUnique_0 = DelegateSupport.ConvertDelegate<PedalOptionTriggerEvent>(triggerEvent);
           
            return pedalOption;
        }

        private static ActionMenuOpener GetActionMenuOpener()
        {
            //MonoBehaviourPublicObBoSiObObObUnique
            if (!ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.isOpen() && ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.isOpen())
            {
                return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1;
            }
            else if (ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.isOpen() && !ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.isOpen())
            {
                return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0;
            }
            else return null;
            /*
            else if (ActionMenuDriver._instance.openerL.isOpen() && ActionMenuDriver._instance.openerR.isOpen())
            {
                return null; //Which one to return ¯\_(ツ)_/¯ Mystery till I figure something smart out
            }
            */
        }

        public static void OpenConfigPagePre(ActionMenu __instance)
        {
            AddPedalsInList(configPagePre, __instance);
        }
        public static void OpenConfigPagePost(ActionMenu __instance)
        {
            AddPedalsInList(configPagePost, __instance);
        }
        public static void OpenMainPagePre(ActionMenu __instance)
        {
            AddPedalsInList(mainPagePre, __instance);
        }
        public static void OpenMainPagePost(ActionMenu __instance)
        {
            AddPedalsInList(mainPagePost, __instance);
        }
        public static void OpenMenuOpacityPagePre(ActionMenu __instance)
        {
            AddPedalsInList(menuOpacityPagePre, __instance);
        }
        public static void OpenMenuOpacityPagePost(ActionMenu __instance)
        {
            AddPedalsInList(menuOpacityPagePost, __instance);
        }
        public static void OpenEmojisPagePre(ActionMenu __instance)
        {
            AddPedalsInList(emojisPagePre, __instance);
        }
        public static void OpenEmojisPagePost(ActionMenu __instance)
        {
            AddPedalsInList(emojisPagePost, __instance);
        }
        public static void OpenExpressionMenuPre(ActionMenu __instance)
        {
            AddPedalsInList(expressionPagePre, __instance);
        }
        public static void OpenExpressionMenuPost(ActionMenu __instance)
        {
            AddPedalsInList(expressionPagePost, __instance);
        }
        public static void OpenNameplatesOpacityPre(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesOpacityPagePre, __instance);
        }
        public static void OpenNameplatesOpacityPost(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesOpacityPagePost, __instance);
        }
        public static void OpenNameplatesPagePre(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesPagePre, __instance);
        }
        public static void OpenNameplatesPagePost(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesPagePost, __instance);
        }
        public static void OpenNameplatesVisibilityPre(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesVisibilityPagePre, __instance);
        }
        public static void OpenNameplatesVisibilityPost(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesVisibilityPagePost, __instance);
        }
        public static void OpenNameplatesSizePre(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesSizePagePre, __instance);
        }
        public static void OpenNameplatesSizePost(ActionMenu __instance)
        {
            AddPedalsInList(nameplatesSizePagePost, __instance);
        }
        public static void OpenOptionsPre(ActionMenu __instance)
        {
            AddPedalsInList(optionsPagePre, __instance);
        }
        public static void OpenOptionsPost(ActionMenu __instance)
        {
            AddPedalsInList(optionsPagePost, __instance);
        }
        public static void OpenSDK2ExpressionPre(ActionMenu __instance)
        {
            AddPedalsInList(sdk2ExpressionPagePre, __instance);
        }
        public static void OpenSDK2ExpressionPost(ActionMenu __instance)
        {
            AddPedalsInList(sdk2ExpressionPagePost, __instance);
        }
        public static void OpenMenuSizePre(ActionMenu __instance)
        {
            AddPedalsInList(menuSizePagePre, __instance);
        }
        public static void OpenMenuSizePost(ActionMenu __instance)
        {
            AddPedalsInList(menuSizePagePost, __instance);
        }

        private static void AddPedalsInList(List<PedalStruct> pedalStructs, ActionMenu instance)
        {
            foreach (PedalStruct pedalStruct in pedalStructs)
            {
                PedalOption pedalOption = instance.AddOption();
                pedalOption.setText(pedalStruct.text); //    VVV
                pedalOption.setIcon(pedalStruct.icon); //Pretty self explanatory
                pedalOption.field_Public_MulticastDelegateNPublicSealedBoUnique_0 = DelegateSupport.ConvertDelegate<PedalOptionTriggerEvent>(pedalStruct.triggerEvent);
            }
        }

        // Function here adapted to use string lists rather than just strings from Ben's (Ben 🐾#3621) Dynamic Bone Safety Mod, link: https://github.com/BenjaminZehowlt/DynamicBonesSafety/blob/master/DynamicBonesSafetyMod.cs
        private bool checkXref(MethodBase m, List<string> keywords)
        {
            try
            { 
                foreach (string keyword in keywords)
                {
                    
                    if (!XrefScanner.XrefScan(m).Any(
                    instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance.ReadAsObject().ToString()
                                   .Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch{ }
            return false;
        }
    
        internal class PedalStruct
        {
            public string text { get; set; }
            public Texture2D icon { get; set; }
            public System.Action triggerEvent { get; set; }
            public PedalStruct(string text, Texture2D icon, System.Action triggerEvent)
            {
                this.text = text;
                this.icon = icon;
                this.triggerEvent = triggerEvent;
            }
        }

    
        internal enum Insertion
        {
            Pre,
            Post
        }


        // Note: anything to do with nameplates and menu size wont work till the new update
        internal enum ActionMenuPageType
        {
            Config,
            Emojis,
            Expression,
            SDK2Expression,
            Main,
            MenuOpacity,
            MenuSize,
            Nameplates,
            NameplatesOpacity,
            NameplatesVisibilty,
            NameplatesSize,
            Options
        }

    }
    internal static class ExtensionMethods
    {
        
        public static PedalOption AddOption(this ActionMenu menu)
        {
            return menu.Method_Private_PedalOption_0();
            //return menu.Method_Private_MonoBehaviourPublicObSiObAcSiBoObStSiBoUnique_0(); //This should be safe for a while unless they add another similar method
        }
        public static string setText(this PedalOption pedal, string text)
        {
            return pedal.prop_String_0 = text; //Likewise... should be safe
        }
        public static string getText(this PedalOption pedal)
        {
            return pedal.prop_String_0; //Likewise... should be safe
        }
        public static ActionMenuPage PushPage(this ActionMenu menu, Il2CppSystem.Action openFunc, Il2CppSystem.Action closeFunc = null, Texture2D icon = null, string text = null)
        {
            return menu.Method_Public_ObjectNPublicAcTeAcStGaUnique_Action_Action_Texture2D_String_0(openFunc, closeFunc, icon, text); //Likewise... should be safe but reflection is pretty easy for it
        }
        public static Texture2D setIcon(this PedalOption pedal, Texture2D icon)
        {
            //PropertyInfo texture = typeof(PedalOption).GetProperties().Where(p => p.PropertyType == typeof(Texture2D)).First(); meh
            return pedal.prop_Texture2D_0 = icon;
        }
        public static Texture2D getIcon(this PedalOption pedal)
        {
            return pedal.prop_Texture2D_0;
        }
        public static bool isOpen(this ActionMenuOpener actionMenuOpener)
        {
            return actionMenuOpener.field_Private_Boolean_0;
        }
    }

}
