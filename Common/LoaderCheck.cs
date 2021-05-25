using System;
using System.Reflection;
using Harmony;
using MelonLoader;

namespace gompoCommon
{
    [HarmonyShield]
    internal static class LoaderCheck
    {
        private static readonly string currentAssemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";

        //Credit to knah: https://github.com/knah/VRCMods/blob/master/UIExpansionKit/LoaderIntegrityCheck.cs
        public static void CheckForRainbows()
        {
            try
            {
                var assembly = Assembly.Load(ReadResource("_dummy_.dll"));
                RainbowsFound();
                Console.ReadLine();
            }
            catch (BadImageFormatException ex)
            {
            }

            try
            {
                var assembly = Assembly.Load(ReadResource("_dummy2_.dll"));
            }
            catch (BadImageFormatException ex)
            {
                MelonLogger.Error(ex.ToString());
                RainbowsFound();
                Console.ReadLine();
            }

            try
            {
                var harmony = HarmonyInstance.Create(Guid.NewGuid().ToString());
                harmony.Patch(AccessTools.Method(typeof(LoaderCheck), nameof(PatchTest)), new HarmonyMethod(typeof(LoaderCheck), nameof(ReturnFalse)));

                PatchTest();
                RainbowsFound();
                Console.ReadLine();
            }
            catch (BadImageFormatException ex)
            {
            }
        }

        private static bool ReturnFalse()
        {
            return false;
        }

        public static void PatchTest()
        {
            throw new BadImageFormatException();
        }

        private static void RainbowsFound()
        {
            MelonLogger.Error("===================================================================");
            MelonLogger.Error($"Message from: {currentAssemblyName}");
            MelonLogger.Error("You're using MelonLoader with important security features missing.");
            MelonLogger.Error("This exposes you to additional risks from certain malicious actors,");
            MelonLogger.Error("including account theft, account bans, and other unwanted consequences");
            MelonLogger.Error("If this is not what you want, download the official installer from");
            MelonLogger.Error("https://github.com/LavaGang/MelonLoader/releases");
            MelonLogger.Error("then close this console, and reinstall MelonLoader using it.");
            MelonLogger.Error("If you want to accept those risks, press Enter to continue");
            MelonLogger.Error("===================================================================");
        }

        private static byte[] ReadResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            byte[] buffer;
            using (var stream = assembly.GetManifestResourceStream($"{currentAssemblyName}.{fileName}"))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}