using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace CraigLib
{
    public class CompressionHelper : ICompressionHelper
    {
        public static string Zip(string stringToZip)
        {
            var bytes = Encoding.UTF8.GetBytes(stringToZip);
            var memoryStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(memoryStream);
            var entry = new ZipEntry("ZippedFile");
            zipOutputStream.PutNextEntry(entry);
            zipOutputStream.SetLevel(9);
            zipOutputStream.Write(bytes, 0, bytes.Length);
            zipOutputStream.Finish();
            zipOutputStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Unzip(string stringToUnzip)
        {
            var memoryStream1 = new MemoryStream(Convert.FromBase64String(stringToUnzip));
            var memoryStream2 = new MemoryStream();
            var zipInputStream = new ZipInputStream(memoryStream1);
            zipInputStream.GetNextEntry();
            var buffer = new byte[2048];
            while (true)
            {
                var count = zipInputStream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                    memoryStream2.Write(buffer, 0, count);
                else
                    break;
            }
            return Encoding.UTF8.GetString(memoryStream2.ToArray());
        }

        public static void UnzipFile(string zipfile, string dst)
        {
            new FastZip().ExtractZip(zipfile, dst, FastZip.Overwrite.Always, null, "", "", true);
        }

        public static string Unzip(Stream streamToUnzip)
        {
            var memoryStream = new MemoryStream();
            var zipInputStream = new ZipInputStream(streamToUnzip);
            zipInputStream.GetNextEntry();
            var buffer = new byte[2048];
            while (true)
            {
                var count = zipInputStream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                    memoryStream.Write(buffer, 0, count);
                else
                    break;
            }
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static Stream GetCompressedStream(Stream stream, string encoding)
        {
            if (encoding == "gzip")
                return new GZipInputStream(stream);
            if (encoding == "deflate")
                return new InflaterInputStream(stream);
            return null;
        }

        public static MemoryStream UncompressGZ(Stream data)
        {
            var memoryStream = new MemoryStream();
            data.Position = 0L;
            var gzipStream = new GZipStream(data, CompressionMode.Decompress, true);
            var buffer = new byte[2048];
            int count;
            while ((count = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                memoryStream.Write(buffer, 0, count);
            gzipStream.Close();
            return memoryStream;
        }

        public static string UncompressGZToString(Stream data)
        {
            var memoryStream = UncompressGZ(data);
            memoryStream.Position = 0L;
            var numArray = new byte[(int)memoryStream.Length];
            memoryStream.Read(numArray, 0, numArray.Length);
            return Encoding.ASCII.GetString(numArray, 0, numArray.Length);
        }

        public static void ZipDirectory(string dir, string fileName)
        {
            new FastZip
            {
                CreateEmptyDirectories = true
            }.CreateZip(fileName, dir, true, "");
        }
    }
}
