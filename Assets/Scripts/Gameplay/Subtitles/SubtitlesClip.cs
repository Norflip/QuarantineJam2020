using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubtitleRow
{
    public string textLine = "";
    public float leftOffset = 0.0f;
    public float rightOffset = 0.0f;
    public float playTime = 0.0f;
    
    public bool showCompleteRow = false;

    public float TotalPlayTime => leftOffset + playTime + rightOffset;
}

[CreateAssetMenu]
public class SubtitlesClip : ScriptableObject
{
    public string key = "";
    public float leftOffset = 0.0f;
    public float rightOffset = 0.0f;
    public List<SubtitleRow> rows = new List<SubtitleRow>();

    public float CalculateCompletePlayTime ()
    {
        float sum = leftOffset + rightOffset;
        for (int i = 0; i < rows.Count; i++)
            sum += rows[i].TotalPlayTime;
        
        return sum;
    }
}
