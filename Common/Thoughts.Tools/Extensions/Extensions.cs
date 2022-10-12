using System.Security.Cryptography;
using System.Text;

namespace Thoughts.Tools.Extensions
{
	public static class Extensions
    {
        public static string ToByteString(this byte[] array) => array
           .Aggregate(new StringBuilder(), (S, b) => S.Append(b.ToString("x2")))
           .ToString();

        public static async Task<string?> GetMd5Async(this Stream stream)
        {
            if (stream == Stream.Null) return null;
            using var md5 = MD5.Create();
            var result = await md5.ComputeHashAsync(stream);
            //return BitConverter.ToString(result).Replace("-", String.Empty);
            return result.ToByteString();
        }

        public static async Task<string?> GetMd5Async(this string path)
        {
            await using var fs = File.OpenRead(path);
            return await GetMd5Async(fs);
        }

        public static async Task<string?> GetMd5Async(this byte[] buffer)
        {
            if (buffer.Length <= 0) return null;
            await using var fs = new MemoryStream(buffer);
            return await GetMd5Async(fs);
        }

        public static async Task<string?> GetSha1Async(this Stream stream)
        {
            if (stream == Stream.Null) return null;
            using var sha = SHA1.Create();
            var result = await sha.ComputeHashAsync(stream);
            //return BitConverter.ToString(result).Replace("-", string.Empty);
            return result.ToByteString();
        }

        public static async Task<string?> GetSha1Async(this string path)
        {
            await using var fs = File.OpenRead(path);
            return await GetSha1Async(fs);
        }

        public static async Task<string?> GetSha1Async(this byte[] buffer)
        {
            if (buffer.Length <= 0) return null;
            await using var fs = new MemoryStream(buffer);
            return await GetSha1Async(fs);
        }
    }
}
