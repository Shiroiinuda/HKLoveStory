#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ChapterDownload : EditorWindow
{
    private static readonly string configPath = "Assets/Config/ChapterConfig.json";
    private static readonly string saveDataPath = "Assets/SaveData/ChapterSaveData.asset";
    private string googleSheetID = "";
    private string googleSheetGID = "0";
    private string savePath = "Assets/Chapters/";

    void OnEnable()
    {
        LoadConfiguration();
    }

    [MenuItem("CSV/Detail/Chapter Download")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChapterDownload));
    }

    [MenuItem("CSV/Chapter Quick Download")]
    public static void QuickLoad()
    {
        ConfigData config = LoadConfigurationStatic();
        if (config != null)
        {
            string csvURL = $"https://docs.google.com/spreadsheets/d/{config.googleSheetID}/export?format=csv&id={config.googleSheetID}&gid={config.googleSheetGID}";
            var downloader = ScriptableObject.CreateInstance<ChapterDownload>();
            downloader.DownloadCsvAsync(csvURL, $"{config.savePath}/ChapterData.csv");
        }
    }

    [MenuItem("CSV/Reset All Chapters")]
    public static void ResetAllChapters()
    {
        SaveChapterSO saveData = AssetDatabase.LoadAssetAtPath<SaveChapterSO>(saveDataPath);
        if (saveData == null)
        {
            Debug.LogError($"<color=#FF0000>Error:</color> SaveChapterSO not found at {saveDataPath}");
            return;
        }

        if (saveData.chapters == null)
        {
            Debug.LogError("<color=#FF0000>Error:</color> No chapters found in SaveChapterSO");
            return;
        }

        for (int i = 0; i < saveData.chapters.Count; i++)
        {
            if (saveData.chapters[i].chapter != null) // Only reset non-null chapters
            {
                ChapterSave chapterSave = saveData.chapters[i];
                chapterSave.played = false;
                chapterSave.favorability = new npcFavorability();
                saveData.chapters[i] = chapterSave;
            }
        }

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssets();
        Debug.Log($"<color=#00FF00>Success:</color> Reset {saveData.chapters.Count} chapters");
    }

    private static ConfigData LoadConfigurationStatic()
    {
        if (File.Exists(configPath))
        {
            string jsonText = File.ReadAllText(configPath);
            return JsonUtility.FromJson<ConfigData>(jsonText);
        }
        else
        {
            Debug.LogError("Configuration file not found.");
            return null;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV Settings", EditorStyles.boldLabel);
        googleSheetID = EditorGUILayout.TextField("Google Sheet ID", googleSheetID);
        googleSheetGID = EditorGUILayout.TextField("Sheet GID", googleSheetGID);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Download and Save CSV"))
        {
            SaveConfiguration();
            string csvURL = $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&id={googleSheetID}&gid={googleSheetGID}";
            DownloadCsvAsync(csvURL, $"{savePath}/ChapterData.csv");
        }
    }

    async void DownloadCsvAsync(string url, string path)
    {
        try
        {
            string csvData = await DownloadCSV(url);
            csvData = csvData.Trim();
            File.WriteAllText(path, csvData);
            Debug.Log($"<color=#00FF00>Success: </color> CSV saved to <color=#00FFFF>{path}</color>");
            ProcessCsvToScriptableObject(csvData);
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"<color=#FF0000>Error downloading CSV:</color> {e.Message}");
        }
    }

    void ProcessCsvToScriptableObject(string csvData)
    {
        string[] lines = csvData.Split('\n');
        List<ChapterSave> allChapters = new List<ChapterSave>();
        
        // First, create all main chapters and their sub-chapters
        for (int i = 1; i < lines.Length; i++) // skip header
        {
            string[] splitData = lines[i].Split(',');
            if (splitData.Length < 3) continue;

            string chapterName = splitData[0].Trim();
            string chapterBookMark = splitData[1].Trim();
            string previousChapter1 = splitData[2].Trim();

            // Skip if it's a sub-chapter or random chapter
            if (chapterName.Contains("_") || chapterName.StartsWith("Random")) continue;

            // Create main chapter
            ChapterSO mainChapter = CreateChapter(chapterName, chapterBookMark, previousChapter1);
            allChapters.Add(new ChapterSave
            {
                chapter = mainChapter,
                played = false,
                favorability = new npcFavorability()
            });

            // Create sub-chapters (if they exist in CSV)
            for (int j = 1; j < lines.Length; j++)
            {
                string[] subData = lines[j].Split(',');
                if (subData.Length < 3) continue;

                string subChapterName = subData[0].Trim();
                if (subChapterName.StartsWith(chapterName + "_"))
                {
                    ChapterSO subChapter = CreateChapter(subChapterName, subData[1].Trim(), chapterName);
                    allChapters.Add(new ChapterSave
                    {
                        chapter = subChapter,
                        played = false,
                        favorability = new npcFavorability()
                    });
                }
            }

            // Add null entries for empty chapters
            for (int j = 1; j <= 3; j++)
            {
                allChapters.Add(new ChapterSave
                {
                    chapter = null,
                    played = false,
                    favorability = new npcFavorability()
                });
            }
        }

        // Create or update SaveChapterSO
        SaveChapterSO saveData = AssetDatabase.LoadAssetAtPath<SaveChapterSO>(saveDataPath);
        if (saveData == null)
        {
            saveData = ScriptableObject.CreateInstance<SaveChapterSO>();
            AssetDatabase.CreateAsset(saveData, saveDataPath);
        }

        saveData.chapters = allChapters;
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"<color=#00FF00>Saved:</color> {allChapters.Count} Chapters including null entries");
    }

    private ChapterSO CreateChapter(string chapterName, string chapterBookMark, string previousChapter1)
    {
        string subfolder = chapterName.StartsWith("Random") ? "Random" : "Chapter";
        string assetPath = $"{savePath}/{subfolder}/{chapterName}.asset";

        ChapterSO chapterSO = AssetDatabase.LoadAssetAtPath<ChapterSO>(assetPath);
        bool newCreate = chapterSO == null;
        if (newCreate)
        {
            chapterSO = ScriptableObject.CreateInstance<ChapterSO>();
        }

        chapterSO.chapterName = chapterName;
        chapterSO.chapterBookMark = chapterBookMark;
        chapterSO.previousChapter1 = previousChapter1;

        if (newCreate)
            AssetDatabase.CreateAsset(chapterSO, assetPath);
        else
            EditorUtility.SetDirty(chapterSO);

        return chapterSO;
    }

    private Task<string> DownloadCSV(string url)
    {
        var tcs = new TaskCompletionSource<string>();
        UnityWebRequest www = UnityWebRequest.Get(url);
        var asyncOp = www.SendWebRequest();
        asyncOp.completed += _ =>
        {
            if (www.result != UnityWebRequest.Result.Success)
                tcs.SetException(new System.Exception(www.error));
            else
                tcs.SetResult(www.downloadHandler.text);
        };
        return tcs.Task;
    }

    void LoadConfiguration()
    {
        if (File.Exists(configPath))
        {
            string jsonText = File.ReadAllText(configPath);
            ConfigData config = JsonUtility.FromJson<ConfigData>(jsonText);
            googleSheetGID = config.googleSheetGID;
            googleSheetID = config.googleSheetID;
            savePath = config.savePath;
        }
    }

    void SaveConfiguration()
    {
        ConfigData config = new ConfigData
        {
            googleSheetGID = googleSheetGID,
            googleSheetID = googleSheetID,
            savePath = savePath
        };
        string jsonText = JsonUtility.ToJson(config, true);
        File.WriteAllText(configPath, jsonText);
        AssetDatabase.Refresh();
    }

    private class ConfigData
    {
        public string googleSheetID = "";
        public string googleSheetGID = "0";
        public string savePath = "Assets/Chapters/";
    }
}
#endif
