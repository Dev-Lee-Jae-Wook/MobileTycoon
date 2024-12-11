using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EverythingStore.Save
{
	public static class SaveSystem
	{
		public static async Task SaveData<T>(T data, string fileName) where T : SaveData
		{
			string saveData = JsonUtility.ToJson(data);
			string filePath = GetSaveDataPath(fileName);

			using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (StreamWriter writer = new StreamWriter(stream ,Encoding.UTF8))
				{
					Debug.Log(saveData);
					await writer.WriteLineAsync(saveData);
				}
			}

			Debug.Log($"Save {fileName}");
		}

		public static T LoadData<T>(string fileName) where T : SaveData
		{
			string filePath = GetSaveDataPath(fileName);
			string data;

			using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					data = reader.ReadToEnd();
				}
			}

			return JsonUtility.FromJson<T>(data);
		}

		public static bool HasSaveData(string fileName)
		{
			return File.Exists(GetSaveDataPath(fileName));
		}

		private static string GetSaveDataPath(string fileName)
		{
			string path = $"{Application.persistentDataPath}/{fileName}.json";

			Debug.Log($"File Load : {path}");
			return path;
		}

	}
}