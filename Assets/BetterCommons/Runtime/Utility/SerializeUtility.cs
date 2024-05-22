using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Better.Commons.Runtime.Extensions;

namespace Better.Commons.Runtime.Utility
{
    public static class SerializeUtility
    {
        /// <summary>
        /// Async version of <see cref="Serialize"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task<byte[]> SerializeAsync(object obj)
        {
            if (obj == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(obj));
                return null;
            }

            return await Task.Factory.StartNew(() => Serialize(obj));
        }

        /// <summary>
        /// Serialize type to bytes with using <see cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(obj));
                return null;
            }

            var binFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                binFormatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Async version of <see cref="Deserialize{T}"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> DeserializeAsync<T>(byte[] bytes) where T : class
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var stream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                await stream.WriteAsync(bytes, 0, bytes.Length);
                stream.Position = 0;

                return binFormatter.Deserialize(stream) as T;
            }
        }

        /// <summary>
        /// Deserialize bytes to type with using <see cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes) where T : class
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var stream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();

                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                return binFormatter.Deserialize(stream) as T;
            }
        }

        public static byte[] CompressGZip(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        public static byte[] DecompressGZip(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compressing bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressGZipAsync(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    await gzipStream.WriteAsync(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Decompressing bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> DecompressGZipAsync(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    await gzipStream.WriteAsync(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compressing bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CompressDeflate(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Decompressing bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] DecompressDeflate(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var input = new MemoryStream(bytes))
            {
                using (var output = new MemoryStream())
                {
                    using (var stream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        stream.CopyTo(output);
                    }

                    return output.ToArray();
                }
            }
        }

        /// <summary>
        /// Async version of <see cref="CompressDeflate"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressDeflateAsync(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Async version of <see cref="DecompressDeflate"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> DecompressDeflateAsync(byte[] bytes)
        {
            if (bytes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(bytes));
                return null;
            }

            var input = new MemoryStream(bytes);
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(input, CompressionMode.Decompress))
                {
                    await stream.CopyToAsync(output);
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static string Compress(string uncompressedString)
        {
            if (uncompressedString.IsNullOrEmpty())
            {
                DebugUtility.LogException<ArgumentException>(nameof(uncompressedString));
                return string.Empty;
            }

            var uncompressed = Encoding.UTF8.GetBytes(uncompressedString);
            var compressed = CompressDeflate(uncompressed);
            return Convert.ToBase64String(compressed);
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(string compressedString)
        {
            if (compressedString.IsNullOrEmpty())
            {
                DebugUtility.LogException<ArgumentException>(nameof(compressedString));
                return string.Empty;
            }

            var compressed = Convert.FromBase64String(compressedString);
            var uncompressed = DecompressDeflate(compressed);
            return Encoding.UTF8.GetString(uncompressed);
        }

        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static async Task<string> CompressAsync(string uncompressedString)
        {
            if (uncompressedString.IsNullOrEmpty())
            {
                DebugUtility.LogException<ArgumentException>(nameof(uncompressedString));
                return string.Empty;
            }

            var uncompressed = Encoding.UTF8.GetBytes(uncompressedString);
            var compressed = await CompressDeflateAsync(uncompressed);
            return Convert.ToBase64String(compressed);
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static async Task<string> DecompressAsync(string compressedString)
        {
            if (compressedString.IsNullOrEmpty())
            {
                DebugUtility.LogException<ArgumentException>(nameof(compressedString));
                return string.Empty;
            }

            var compressed = Convert.FromBase64String(compressedString);
            var uncompressed = await DecompressDeflateAsync(compressed);
            return Encoding.UTF8.GetString(uncompressed);
        }
    }
}