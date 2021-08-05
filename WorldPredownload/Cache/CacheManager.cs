using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;
using WorldPredownload.Helpers;

namespace WorldPredownload.Cache
{
    public class CacheManager
    {
        private static readonly HashSet<string> Directories = new();
        private static readonly Stopwatch Stopwatch = new();

        public static void UpdateDirectories()
        {
            Stopwatch.Restart();
            Directories.Clear();
            var files = Directory.GetDirectories(GetCache().path, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
                Directories.Add(Path.GetFileName(files[i]));
            Stopwatch.Stop();
            MelonLogger.Msg($"Finished getting {Directories.Count} cache entries in {Stopwatch.ElapsedMilliseconds}ms");
        }

        public static void AddDirectory(string hash)
        {
            Directories.Add(hash);
        }


        public static bool HasDownloadedWorld(string id)
        {
            return false; //Dead Method for now in case I want to rework the invite checking mechanism
        }

        public static bool HasDownloadedWorld(string id, int version)
        {
            var hash = ComputeAssetHash(id);
            if (Directories.Contains(hash))
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

        public static UnityEngine.Cache GetCache()
        {
            return Utilities.GetAssetBundleDownloadManager().field_Private_Cache_0;
        }

        private static bool HasVersion(string hash, int version)
        {
            if (!Directory.Exists(Path.Combine(GetCache().path, hash))) return false;
            foreach (var directoryInfo in new DirectoryInfo(Path.Combine(GetCache().path, hash)).GetDirectories())
                if (directoryInfo.Name.EndsWith(ComputeVersionString(version)))
                    return true;
            return false;
        }

        public static string ComputeVersionString(int version) //Int to Little Endian Hex String
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

        public static void CreateInfoFileFor(string file)
        {
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), "__info"),
                $"-1\n{((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds()}\n1\n__data\n");
        }
    }
}