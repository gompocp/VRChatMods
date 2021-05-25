using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;

namespace WorldPredownload.Cache
{
    public class CacheManager
    {
        private static readonly HashSet<string> directories = new();

        public static void UpdateDirectories()
        {
            var timer = new Stopwatch();
            timer.Start();
            directories.Clear();
            foreach (var entry in Directory.EnumerateDirectories(GetCache().path))
                directories.Add(new DirectoryInfo(entry).Name);
            timer.Stop();
            MelonLogger.Msg($"Finished getting {directories.Count} cache entries in {timer.ElapsedMilliseconds}ms");
        }

        public static void AddDirectory(string hash)
        {
            directories.Add(hash);
        }


        public static bool HasDownloadedWorld(string id)
        {
            return false; //Dead Method for now in case I want to rework the invite checking mechanism
        }

        public static bool HasDownloadedWorld(string id, int version)
        {
            var hash = ComputeAssetHash(id);
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
            return Utilities.ByteArrayToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(id))).ToUpper()
                .Substring(0, 16);
        }

        private static UnityEngine.Cache GetCache()
        {
            return Utilities.GetAssetBundleDownloadManager().field_Private_Cache_0;
        }

        private static bool HasVersion(string hash, int version)
        {
            foreach (var directoryInfo in new DirectoryInfo(Path.Combine(GetCache().path, hash)).GetDirectories())
                if (directoryInfo.Name.EndsWith(ComputeVersionString(version)))
                    return true;
            return false;
        }

        private static string ComputeVersionString(int version) //Int to Little Endian Hex String
        {
            var bytes = BitConverter.GetBytes(version);
            var result = "";
            foreach (var b in bytes) result += b.ToString("x2");
            return result;
        }

        public static bool WorldFileExists(string id)
        {
            var expectedLocation = new DirectoryInfo(Path.Combine(GetCache().path, ComputeAssetHash(id)));
            if (!Directory.Exists(expectedLocation.FullName)) return false;
            foreach (var file in expectedLocation.GetFiles("*.*", SearchOption.AllDirectories))
                if (file.Name.Contains("__data"))
                    return true;
            return false;
        }
    }
}