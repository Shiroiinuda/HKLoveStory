using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefundCSVReader : MonoBehaviour
{
    public TextAsset csv_10Jade;
    public TextAsset csv_100Jade;
    public TextAsset csv_400Jade;
    public List<string> jadeString_10Jade = new List<string>();
    public List<string> jadeString_100Jade = new List<string>();
    public List<string> jadeString_400Jade = new List<string>();

    private void Start()
    {
        LoadCSV(csv_10Jade, jadeString_10Jade);
        LoadCSV(csv_100Jade, jadeString_100Jade);
        LoadCSV(csv_400Jade, jadeString_400Jade);
    }

    void LoadCSV(TextAsset csv, List<string> rows)
    {
        string[] lines = csv.text.Split('\n');
        rows.AddRange(lines);
    }
}
