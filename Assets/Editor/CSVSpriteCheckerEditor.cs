using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVSpriteCheckerResources : Editor
{
   // This points to a CSV file in Assets/CSV/imageData.csv
      private const string csvFilePath = "Assets/CSV/imageData.csv";
  
      [MenuItem("Tools/Check CSV Sprites (Resources)")]
      public static void CheckSpritesFromCSV()
      {
          // 1. Check if CSV file exists
          if (!File.Exists(csvFilePath))
          {
              Debug.LogWarning("CSV file not found at: " + csvFilePath);
              return;
          }
  
          // 2. Read each line from the CSV
          string[] csvLines = File.ReadAllLines(csvFilePath);
  
          // 3. For every line (resource path), attempt to load a Sprite
          foreach (string line in csvLines)
          {
              string resourcePath = line.Trim();
  
              // Skip blank lines
              if (string.IsNullOrEmpty(resourcePath))
              {
                  Debug.Log("Empty line in CSV. Skipping...");
                  continue;
              }
  
              // The user specifically needs to check "Backgrounds/ChengWing2".
              // But we’ll still handle multiple lines if present.
              Sprite sprite = Resources.Load<Sprite>($"Backgrounds/ChengWing2/{resourcePath}");
  
              if (sprite == null)
              {
                  Debug.Log("Sprite NOT found for path: " + resourcePath);
              }
              else
              {
                  Debug.Log("Sprite loaded successfully for path: " + resourcePath);
              }
          }
  
          Debug.Log("CSV sprite check complete." + csvLines.Length);
      }
}