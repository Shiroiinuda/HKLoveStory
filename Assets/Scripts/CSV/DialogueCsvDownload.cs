#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

namespace DialogueCSVLoader
{
    public class DialogueCsvDownload : EditorWindow
    {
        private static readonly string configPath = "Assets/Config/config.json";
        private string prefabName = "DialogueDataPrefab";
        private string prefabSavePath = "Assets/";
        private string googleSheetID = "";
        private string googleSheetGID = "0";
        private string savePath = "Assets/";

        private string CSVtutor =
            "https://docs.google.com/presentation/d/1Pqd_VM9C3PfvEb2Ez2A-BWf4L0SWZZpxG2VToos5Mt0/edit#slide=id.g2c899be256b_0_585";

        void OnEnable()
        {
            LoadConfiguration();

            void LoadConfiguration()
            {
                if (File.Exists(configPath))
                {
                    string jsonText = File.ReadAllText(configPath);
                    ConfigData config = JsonUtility.FromJson<ConfigData>(jsonText);
                    googleSheetGID = config.googleSheetGID;
                    googleSheetID = config.googleSheetID;
                    prefabName = config.prefabName;
                    prefabSavePath = config.prefabSavePath;
                    savePath = config.savePath;
                }
                else
                {
                    Debug.LogError("Configuration file not found.");
                }
            }
        }

        void SaveConfiguration()
        {
            ConfigData config = new ConfigData
            {
                googleSheetGID = googleSheetGID,
                googleSheetID = googleSheetID,
                prefabName = prefabName,
                prefabSavePath = prefabSavePath,
                savePath = savePath
            };
            string jsonText = JsonUtility.ToJson(config, true);
            File.WriteAllText(configPath, jsonText);
            AssetDatabase.Refresh();
        }

        [MenuItem("CSV/Detail/Dialogue Download")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DialogueCsvDownload));
        }

        [MenuItem("CSV/Dialogue Download")]
        public static void QuickLoad()
        {
            ConfigData config = LoadConfigurationStatic();
            if (config != null)
            {
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{config.googleSheetID}/export?format=csv&id={config.googleSheetID}&gid={config.googleSheetGID}";
                var downloader = CreateInstance<DialogueCsvDownload>();
                downloader.DownloadCsvAsync(csvURL, $"{config.savePath}/DialogueData.csv");
            }
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
            GUILayout.Label("Prefab Settings", EditorStyles.boldLabel);
            prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);
            prefabSavePath = EditorGUILayout.TextField("Prefab Save Path", prefabSavePath);
            if (GUILayout.Button("Download and Save CSV"))
            {
                SaveConfiguration();
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&id={googleSheetID}&gid={googleSheetGID}";
                DownloadCsvAsync(csvURL, $"{savePath}/DialogueData.csv");
            }

            if (GUILayout.Button("How to use"))
            {
                Application.OpenURL(CSVtutor);
            }
        }

        async void DownloadCsvAsync(string url, string path)
        {
            try
            {
                string csvData = await DownloadCSV(url);
                csvData = csvData.Trim();
                File.WriteAllText(path, csvData);
                Debug.Log($"<color=#00FF00>Sucess: </color> CSV saved to <color=#00FFFF>{path}</color>");
                ProcessCsvToPrefab(csvData, prefabName);
                AssetDatabase.Refresh();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"<color=#FF0000>Error downloading CSV:</color> {e.Message}");
            }
        }

        void ProcessCsvToPrefab(string csvData, string prefabNames)
        {
            string[] assetPaths = AssetDatabase.FindAssets(prefabNames);
            GameObject prefab = null;
            foreach (string assetPath in assetPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetPath);
                if (Path.GetFileNameWithoutExtension(path) == prefabNames)
                {
                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    break;
                }
            }

            if (prefab == null)
            {
                prefab = new GameObject(prefabNames);
                prefab.AddComponent<DialogueData>(); // Assuming DialogueData is the component you want
                // Save the new GameObject as a prefab
                string prefabPath = prefabSavePath + prefabNames + ".prefab";
                prefab = PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            }

            // Update the prefab with CSV data
            DialogueData dataComponent = prefab.GetComponent<DialogueData>();
            if (dataComponent != null)
            {
                dataComponent.dialogues = new List<Dialogue>();
                string[] lines = csvData.Split('\n');
                Debug.Log($"<color=#ffee00>Loading:</color> {lines.Length} dialogues");
                for (int i = 1; i < lines.Length; i++)
                {
                    dataComponent.dialogues.Add(new Dialogue(lines[i]));
                }

                Debug.Log($"<color=#00FF00>Completed:</color> {lines.Length} dialogues loaded");
            }
            else
            {
                Debug.LogError(prefabNames + " does not have a DialogueData component.");
            }

            EditorUtility.SetDirty(prefab);
            AssetDatabase.SaveAssets();
        }

        private Task<string> DownloadCSV(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("<color=#FF0000> Error:</color> DownloadCSV called with null or empty URL.");
                tcs.SetException(new ArgumentNullException(nameof(url)));
                return tcs.Task;
            }

            UnityWebRequest www = UnityWebRequest.Get(url);
            if (www == null)
            {
                Debug.LogError("<color=#FF0000> Error:</color> Failed to create UnityWebRequest with URL: " + url);
                tcs.SetException(
                    new InvalidOperationException("<color=#FF0000> Error:</color> UnityWebRequest.Get returned null."));
                return tcs.Task;
            }

            var asyncOp = www.SendWebRequest();
            asyncOp.completed += (asyncOperation) =>
            {
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("<color=#FF0000> Error:</color> Web request error: " + www.error);
                    tcs.SetException(new Exception(www.error));
                }
                else
                {
                    tcs.SetResult(www.downloadHandler.text);
                }
            };
            return tcs.Task;
        }

        private class ConfigData
        {
            public string prefabName = "DialogueDataPrefab";
            public string prefabSavePath = "Assets/";
            public string googleSheetID = "";
            public string googleSheetGID = "0";
            public string savePath = "Assets/";
        }
    }

    public class TutorialLink
    {
        // Add a menu item named "Open Link" to the "Tools" menu
        [MenuItem("Tools/Tutorial")]
        private static void OpenLink()
        {
            string url =
                "https://docs.google.com/presentation/d/1Pqd_VM9C3PfvEb2Ez2A-BWf4L0SWZZpxG2VToos5Mt0/edit?usp=sharing";
            Application.OpenURL(url);
        }
    }
}

namespace ItemInventory
{
    public class InventoryDownload : EditorWindow
    {
        private static readonly string configPath = "Assets/Config/Inventoryconfig.json";
        private string googleSheetID = "";
        private string googleSheetGID = "0";
        private string savePath = "Assets/";

        void OnEnable()
        {
            LoadConfiguration();

            void LoadConfiguration()
            {
                if (File.Exists(configPath))
                {
                    string jsonText = File.ReadAllText(configPath);
                    InventConfigData config = JsonUtility.FromJson<InventConfigData>(jsonText);
                    googleSheetGID = config.googleSheetGID;
                    googleSheetID = config.googleSheetID;
                    savePath = config.savePath;
                }
                else
                {
                    Debug.LogError("Configuration file not found.");
                }
            }
        }

        [MenuItem("CSV/Detail/Inventory Download")]
        public static void ShowWindow()
        {
            GetWindow(typeof(InventoryDownload));
        }

        [MenuItem("CSV/Inventory Download")]
        public static void QuickLoad()
        {
            InventConfigData config = LoadConfigurationStatic();
            if (config != null)
            {
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{config.googleSheetID}/export?format=csv&id={config.googleSheetID}&gid={config.googleSheetGID}";
                var downloader = ScriptableObject.CreateInstance<InventoryDownload>();
                downloader.DownloadCsvAsync(csvURL, $"{config.savePath}/InventoryData.csv");
            }
        }

        private static InventConfigData LoadConfigurationStatic()
        {
            if (File.Exists(configPath))
            {
                string jsonText = File.ReadAllText(configPath);
                return JsonUtility.FromJson<InventConfigData>(jsonText);
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
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&id={googleSheetID}&gid={googleSheetGID}";
                DownloadCsvAsync(csvURL, $"{savePath}/InventoryData.csv");
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
            Debug.Log($"<color=#ffee00>Loading:</color> {lines.Length - 1} Items");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] splitData = lines[i].Split(',');
                string itemSaveName = splitData[0].Trim();
                string itemSavePath = splitData[1].Trim();
                string assetPath = $"Assets/Resources/{itemSavePath}/{itemSaveName}.asset";
                //                    Debug.Log($"Assets/Resources/{itemSavePath}/{itemSaveName}.asset");
                InventoryItems inventoryItems = AssetDatabase.LoadAssetAtPath<InventoryItems>(assetPath);
                var newCreate = inventoryItems == null;
                if (newCreate)
                {
                    inventoryItems = ScriptableObject.CreateInstance<InventoryItems>();
                }
                else
                {
                    inventoryItems.itemText.Clear();
                    inventoryItems.itemName = "";
                    inventoryItems.itemDiscription = "";
                    inventoryItems.itemSprite = null;
                    inventoryItems.inventory = false;
                    inventoryItems.collection = false;
                    inventoryItems.needSave = false;
                    inventoryItems.triggerEnd = false;
                    inventoryItems.path = "";
                }

                inventoryItems.keyValue = itemSaveName;
                inventoryItems.itemText = new List<string>();
                inventoryItems.path = splitData[1].Trim();
                inventoryItems.itemName = splitData[2].Trim();
                if (!string.IsNullOrEmpty(splitData[3].Trim()))
                {
                    string[] splitString = (splitData[3].Trim()).Split('&');
                    foreach (string text in splitString)
                    {
                        inventoryItems.itemText.Add(text);
                    }
                }

                inventoryItems.itemDiscription = splitData[4];
                Sprite itemSprite = Resources.Load<Sprite>($"{itemSavePath}/{splitData[5]}");
                Debug.Log(splitData[9] + " " + splitData[0]);
                if (itemSprite != null)
                    inventoryItems.itemSprite = itemSprite;
                if (splitData[6] == "1")
                    inventoryItems.inventory = true;
                else
                    inventoryItems.inventory = false;
                if (splitData[7] == "1")
                    inventoryItems.collection = true;
                else
                    inventoryItems.collection = false;
                if (splitData[8] == "1")
                    inventoryItems.needSave = true;
                else
                    inventoryItems.needSave = false;
                if (splitData[9].Trim() == "1")
                    inventoryItems.triggerEnd = true;
                else
                    inventoryItems.triggerEnd = false;
                Debug.Log(inventoryItems.triggerEnd);
                if (newCreate == true)
                    AssetDatabase.CreateAsset(inventoryItems, assetPath);
                else
                    EditorUtility.SetDirty(inventoryItems);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"<color=#00FF00>Saved:</color> {lines.Length - 1} Items");
        }

        private Task<string> DownloadCSV(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("<color=#FF0000> Error:</color> DownloadCSV called with null or empty URL.");
                tcs.SetException(new ArgumentNullException(nameof(url)));
                return tcs.Task;
            }

            UnityWebRequest www = UnityWebRequest.Get(url);
            if (www == null)
            {
                Debug.LogError("<color=#FF0000> Error:</color> Failed to create UnityWebRequest with URL: " + url);
                tcs.SetException(
                    new InvalidOperationException("<color=#FF0000> Error:</color> UnityWebRequest.Get returned null."));
                return tcs.Task;
            }

            var asyncOp = www.SendWebRequest();
            asyncOp.completed += (asyncOperation) =>
            {
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("<color=#FF0000> Error:</color> Web request error: " + www.error);
                    tcs.SetException(new Exception(www.error));
                }
                else
                {
                    tcs.SetResult(www.downloadHandler.text);
                }
            };
            return tcs.Task;
        }

        void SaveConfiguration()
        {
            InventConfigData config = new InventConfigData
            {
                googleSheetGID = googleSheetGID,
                googleSheetID = googleSheetID,
                savePath = savePath
            };
            string jsonText = JsonUtility.ToJson(config, true);
            File.WriteAllText(configPath, jsonText);
            AssetDatabase.Refresh();
        }

        private class InventConfigData
        {
            public string googleSheetID = "";
            public string googleSheetGID = "0";
            public string savePath = "Assets/";
        }
    }
}

namespace ItemNormal
{
    public class NormalIemDownload : EditorWindow
    {
        private static readonly string configPath = "Assets/Config/NormalItemconfig.json";
        private string googleSheetID = "";
        private string googleSheetGID = "0";
        private string savePath = "Assets/";

        void OnEnable()
        {
            LoadConfiguration();

            void LoadConfiguration()
            {
                if (File.Exists(configPath))
                {
                    string jsonText = File.ReadAllText(configPath);
                    NormalItemConfigData config = JsonUtility.FromJson<NormalItemConfigData>(jsonText);
                    googleSheetGID = config.googleSheetGID;
                    googleSheetID = config.googleSheetID;
                    savePath = config.savePath;
                }
                else
                {
                    Debug.LogError("Configuration file not found.");
                }
            }
        }

        [MenuItem("CSV/Detail/NormalIem Download")]
        public static void ShowWindow()
        {
            GetWindow(typeof(NormalIemDownload));
        }

        [MenuItem("CSV/NormalIem Download")]
        public static void QuickLoad()
        {
            NormalItemConfigData config = LoadConfigurationStatic();
            if (config != null)
            {
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{config.googleSheetID}/export?format=csv&id={config.googleSheetID}&gid={config.googleSheetGID}";
                var downloader = ScriptableObject.CreateInstance<NormalIemDownload>();
                downloader.DownloadCsvAsync(csvURL, $"{config.savePath}/NormalItemData.csv");
            }
        }

        private static NormalItemConfigData LoadConfigurationStatic()
        {
            if (File.Exists(configPath))
            {
                string jsonText = File.ReadAllText(configPath);
                return JsonUtility.FromJson<NormalItemConfigData>(jsonText);
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
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&id={googleSheetID}&gid={googleSheetGID}";
                DownloadCsvAsync(csvURL, $"{savePath}/InventoryData.csv");
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
            Debug.Log($"<color=#ffee00>Loading:</color> {lines.Length - 1} Items");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] splitData = lines[i].Split(',');
                string itemSaveName = splitData[0].Trim();
                string itemSavePath = splitData[1].Trim();
                string assetPath = $"Assets/Resources/{itemSavePath}/{itemSaveName}.asset";
                //                    Debug.Log($"Assets/Resources/{itemSavePath}/{itemSaveName}.asset");
                NormalInvestigationItems normalItems =
                    AssetDatabase.LoadAssetAtPath<NormalInvestigationItems>(assetPath);
                var newCreate = normalItems == null;
                if (newCreate)
                {
                    normalItems = ScriptableObject.CreateInstance<NormalInvestigationItems>();
                }
                else
                {
                    normalItems.itemText.Clear();
                    normalItems.needKey = false;
                    normalItems.key = null;
                    normalItems.delKey = false;
                    normalItems.unlockedText.Clear();
                    normalItems.unlockedSFx = null;
                    normalItems.lockedSFx = null;
                    normalItems.triggerEnd = false;
                    normalItems.enalbleBtnAfterEnd = false;
                    normalItems.jumpTo = 0;
                }

                normalItems.keyValue = itemSaveName;
                normalItems.itemText = new List<string>();
                if (!string.IsNullOrEmpty(splitData[2].Trim()))
                {
                    string[] splitString = (splitData[2].Trim()).Split('&');
                    foreach (string text in splitString)
                    {
                        normalItems.itemText.Add(text);
                    }
                }

                if (splitData[3] == "1")
                    normalItems.needKey = true;
                else
                    normalItems.needKey = false;

                string keyPath = splitData[4].Trim();
                InventoryItems inventoryItem = Resources.Load<InventoryItems>($"{keyPath}/{splitData[5].Trim()}");
                if (inventoryItem != null)
                    normalItems.key = inventoryItem;

                if (splitData[6] == "1")
                    normalItems.delKey = true;
                else
                    normalItems.delKey = false;

                normalItems.unlockedText = new List<string>();
                if (!string.IsNullOrEmpty(splitData[7].Trim()))
                {
                    string[] splitString = (splitData[7].Trim()).Split('&');
                    foreach (string text in splitString)
                    {
                        normalItems.unlockedText.Add(text);
                    }
                }

                AudioClip unlockSfx = Resources.Load<AudioClip>($"Sounds/sfx/{splitData[8].Trim()}");
                if (unlockSfx != null)
                    normalItems.unlockedSFx = unlockSfx;

                AudioClip lockedSfx = Resources.Load<AudioClip>($"Sounds/sfx/{splitData[9].Trim()}");
                if (lockedSfx != null)
                    normalItems.lockedSFx = lockedSfx;

                if (splitData[10].Trim() == "1")
                    normalItems.triggerEnd = true;
                else
                    normalItems.triggerEnd = false;

                if (splitData[11].Trim() == "1")
                    normalItems.enalbleBtnAfterEnd = true;
                else
                    normalItems.enalbleBtnAfterEnd = false;

                normalItems.jumpTo = int.Parse(splitData[12]);

                if (newCreate == true)
                    AssetDatabase.CreateAsset(normalItems, assetPath);
                else
                    EditorUtility.SetDirty(normalItems);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"<color=#00FF00>Saved:</color> {lines.Length - 1} Items");
        }

        private Task<string> DownloadCSV(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("<color=#FF0000> Error:</color> DownloadCSV called with null or empty URL.");
                tcs.SetException(new ArgumentNullException(nameof(url)));
                return tcs.Task;
            }

            UnityWebRequest www = UnityWebRequest.Get(url);
            if (www == null)
            {
                Debug.LogError("<color=#FF0000> Error:</color> Failed to create UnityWebRequest with URL: " + url);
                tcs.SetException(
                    new InvalidOperationException("<color=#FF0000> Error:</color> UnityWebRequest.Get returned null."));
                return tcs.Task;
            }

            var asyncOp = www.SendWebRequest();
            asyncOp.completed += (asyncOperation) =>
            {
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("<color=#FF0000> Error:</color> Web request error: " + www.error);
                    tcs.SetException(new Exception(www.error));
                }
                else
                {
                    tcs.SetResult(www.downloadHandler.text);
                }
            };
            return tcs.Task;
        }

        void SaveConfiguration()
        {
            NormalItemConfigData config = new NormalItemConfigData
            {
                googleSheetGID = googleSheetGID,
                googleSheetID = googleSheetID,
                savePath = savePath
            };
            string jsonText = JsonUtility.ToJson(config, true);
            File.WriteAllText(configPath, jsonText);
            AssetDatabase.Refresh();
        }

        private class NormalItemConfigData
        {
            public string googleSheetID = "";
            public string googleSheetGID = "0";
            public string savePath = "Assets/";
        }
    }
}

namespace CharacterInventory
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Networking;

    public class CharacterDataDownload : EditorWindow
    {
        private static readonly string configPath = "Assets/Config/CharacterConfig.json";
        private string googleSheetID = "";
        private string googleSheetGID = "0";
        private string savePath = "Assets/";

        void OnEnable()
        {
            LoadConfiguration();

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
                else
                {
                    Debug.LogError("Configuration file not found.");
                }
            }
        }

        [MenuItem("CSV/Character/Character Data Download")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CharacterDataDownload));
        }

        [MenuItem("CSV/Character Quick Download")]
        public static void QuickLoad()
        {
            ConfigData config = LoadConfigurationStatic();
            if (config != null)
            {
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{config.googleSheetID}/export?format=csv&id={config.googleSheetID}&gid={config.googleSheetGID}";
                var downloader = ScriptableObject.CreateInstance<CharacterDataDownload>();
                downloader.DownloadCsvAsync(csvURL, $"{config.savePath}/CharacterData.csv");
            }
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
                string csvURL =
                    $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&id={googleSheetID}&gid={googleSheetGID}";
                DownloadCsvAsync(csvURL, $"{savePath}/CharacterData.csv");
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
    Debug.Log($"<color=#ffee00>Processing:</color> {lines.Length - 1} Entries");

    CharacterScriptableObject currentCharacter = null;
    List<Sprite> bodySprites = new List<Sprite>();
    List<Sprite> headSprites = new List<Sprite>();
    List<Sprite> expressionSprites = new List<Sprite>();
    string speakerName = null;

    for (int i = 1; i < lines.Length; i++) // Skip header
    {
        if (string.IsNullOrWhiteSpace(lines[i])) continue;

        string[] splitData = lines[i].Split(',');

        // Check if this row starts a new character (new SpeakerID)
        if (!string.IsNullOrEmpty(splitData[1].Trim())) // SpeakerID is non-empty
        {
            // Save the current character if one exists
            if (currentCharacter != null)
            {
                SaveCharacterData(currentCharacter, bodySprites, headSprites, expressionSprites);
            }

            // Start a new character
            speakerName = splitData[0].Trim();
            int id = int.Parse(splitData[1].Trim());
            currentCharacter = CreateOrLoadCharacterScriptableObject(id);

            // Reset lists
            bodySprites = new List<Sprite>();
            headSprites = new List<Sprite>();
            expressionSprites = new List<Sprite>();
        }

        // Append body sprites if present
        if (!string.IsNullOrEmpty(splitData[2].Trim()))
        {
            string bodyPath = $"CharacterPortraits/{speakerName}/{splitData[2].Trim()}";
            AddSpriteToList(bodySprites, bodyPath);
        }

        // Append head sprites if present
        if (!string.IsNullOrEmpty(splitData[3].Trim()))
        {
            string headPath = $"CharacterPortraits/{speakerName}/{splitData[3].Trim()}";
            AddSpriteToList(headSprites, headPath);
        }

        // Append expression sprites if present
        if (!string.IsNullOrEmpty(splitData[4].Trim()))
        {
            string expressionPath = $"CharacterPortraits/{speakerName}/{splitData[4].Trim()}";
            AddSpriteToList(expressionSprites, expressionPath);
        }
    }

    // Save the last character after finishing the loop
    if (currentCharacter != null)
    {
        SaveCharacterData(currentCharacter, bodySprites, headSprites, expressionSprites);
    }

    AssetDatabase.SaveAssets();
    Debug.Log($"<color=#00FF00>Completed:</color> {lines.Length - 1} Entries");
}

void AddSpriteToList(List<Sprite> spriteList, string path)
{
    Sprite sprite = Resources.Load<Sprite>(path);
    if (sprite != null)
    {
        spriteList.Add(sprite);
    }
    else
    {
        Debug.LogWarning($"<color=#FF8800>Warning:</color> Sprite not found at path {path}");
    }
}

CharacterScriptableObject CreateOrLoadCharacterScriptableObject(int id)
{
    string assetPath = $"{savePath}/Character_{id}.asset";
    CharacterScriptableObject characterData = AssetDatabase.LoadAssetAtPath<CharacterScriptableObject>(assetPath);

    if (characterData == null)
    {
        characterData = ScriptableObject.CreateInstance<CharacterScriptableObject>();
        characterData.id = id;
        AssetDatabase.CreateAsset(characterData, assetPath);
    }

    return characterData;
}

void SaveCharacterData(CharacterScriptableObject characterData, List<Sprite> bodySprites, List<Sprite> headSprites, List<Sprite> expressionSprites)
{
    if (characterData != null)
    {
        characterData.bodySprites = new List<Sprite>(bodySprites);
        characterData.headSprites = new List<Sprite>(headSprites);
        characterData.expressionSprites = new List<Sprite>(expressionSprites);

        EditorUtility.SetDirty(characterData);
    }
}

        private Task<string> DownloadCSV(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            UnityWebRequest www = UnityWebRequest.Get(url);

            var asyncOp = www.SendWebRequest();
            asyncOp.completed += _ =>
            {
                if (www.result != UnityWebRequest.Result.Success)
                {
                    tcs.SetException(new System.Exception(www.error));
                }
                else
                {
                    tcs.SetResult(www.downloadHandler.text);
                }
            };

            return tcs.Task;
        }

        void SaveConfiguration()
        {
            ConfigData config = new ConfigData
            {
                googleSheetID = googleSheetID,
                googleSheetGID = googleSheetGID,
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
            public string savePath = "Assets/";
        }
    }
}

#endif