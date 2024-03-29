using System;
using System.IO;

namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public static class IO
	{
		public static void createdir(string path)
		{
			Directory.CreateDirectory(path);
		}

		public static void deletedir(string path, bool recursive)
		{
			Directory.Delete(path, recursive);
		}

		public static bool direxists(string path)
		{
			return Directory.Exists(path);
		}

		public static void movedir(string sourceDirName, string destDirName)
		{
			Directory.Move(sourceDirName, destDirName);
		}

		public static string parentdir(string path)
		{
			return Directory.GetParent(path)!.FullName;
		}

		public static void copyfile(string sourceDirName, string destDirName)
		{
			File.Copy(sourceDirName, destDirName);
		}

		public static void deletefile(string path)
		{
			File.Delete(path);
		}

		public static bool fileexists(string path)
		{
			return File.Exists(path);
		}

		public static void movefile(string sourceDirName, string destDirName)
		{
			File.Move(sourceDirName, destDirName);
		}

		public static string readtextfile(string path)
		{
			return File.OpenText(path).ReadToEnd();
		}

		public static bool savetextfile(string text, string path)
		{
			try
			{
				FileStream stream = File.OpenWrite(path);
				using StreamWriter streamWriter = new StreamWriter(stream);
				streamWriter.Write(text);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
