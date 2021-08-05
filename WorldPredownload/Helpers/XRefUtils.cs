using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;

namespace WorldPredownload.Helpers
{
    public static class XRefUtils
    {
        public static bool CheckXrefStrings(MethodBase m, List<string> keywords)
        {
            try
            {
                foreach (var keyword in keywords)
                    if (!XrefScanner.XrefScan(m).Any(
                        instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance
                            .ReadAsObject().ToString()
                            .Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                        return false;
                return true;
            }
            catch
            {
            }

            return false;
        }

        public static bool XRefScanFor(this MethodBase methodBase, string searchTerm)
        {
            return XrefScanner.XrefScan(methodBase).Any(
                xref => xref.Type == XrefType.Global && xref.ReadAsObject()?.ToString()
                    .IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool CheckXrefNoStrings(MethodBase m)
        {
            try
            {
                foreach (var instance in XrefScanner.XrefScan(m))
                {
                    if (instance.Type != XrefType.Global || instance.ReadAsObject() == null) continue;
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                MelonLogger.Msg("For loop failed:" + e);
            }

            return false;
        }

        public static void ScanMethod(MethodInfo m)
        {
            MelonLogger.Msg($"Scanning: {m.FullDescription()}");
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found String: \"{instance.ReadAsObject().ToString()}\"");
                        }
                        catch
                        {
                        }
                    else if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found Method: {instance.TryResolve().FullDescription()}");
                        }
                        catch
                        {
                        }
                }
                catch
                {
                }

            foreach (var instance in XrefScanner.UsedBy(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found Used By Method: {instance.TryResolve().FullDescription()}");
                        }
                        catch
                        {
                        }
                }
                catch
                {
                }
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
    }
}