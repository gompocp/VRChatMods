using System;
using MelonLoader;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace WorldPredownload.Cache
{
    public class CacheManager
    {
        private static HashSet<string> directories = new (); 

        public static void UpdateDirectories()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            directories.Clear();
            foreach (var entry in Directory.EnumerateDirectories(GetCache().path)) 
            {
                directories.Add(new DirectoryInfo(entry).Name);
            }
            timer.Stop();
            MelonLogger.Msg($"Finished getting { directories.Count } cache entries in { timer.ElapsedMilliseconds }ms");
        }

        public static void AddDirectory(string hash) => directories.Add(hash);


        public static bool HasDownloadedWorld(string id)
        {
            return false; //Dead Method for now in case I want to rework the invite checking mechanism
        }

        public static bool HasDownloadedWorld(string id, int version)
        {
            string hash = ComputeAssetHash(id);
            if (directories.Contains(hash))
            {
                if (HasVersion(hash, version))
                    return true;
                return false;
            }
            return false;
        }

        public static string ComputeAssetHash(string id)
        {
            return Utilities.ByteArrayToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(id))).ToUpper().Substring(0, 16);
        }

        private static UnityEngine.Cache GetCache() => Utilities.GetAssetBundleDownloadManager().field_Private_Cache_0;

        private static bool HasVersion(string hash, int version)
        {
            foreach(DirectoryInfo directoryInfo in new DirectoryInfo(Path.Combine(GetCache().path, hash)).GetDirectories())
            {
                if (directoryInfo.Name.EndsWith(ComputeVersionString(version))) return true;
            }
            return false;
        }

        private static string ComputeVersionString(int version) //Int to Little Endian Hex String
        {
            byte[] bytes = BitConverter.GetBytes(version);
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        public static bool WorldFileExists(string id)
        {
            DirectoryInfo expectedLocation = new DirectoryInfo(Path.Combine(GetCache().path, ComputeAssetHash(id)));
            if (!Directory.Exists(expectedLocation.FullName)) return false;
            foreach (var file in expectedLocation.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (file.Name.Contains("__data")) return true;
            }
            return false;
        }
    }
}
