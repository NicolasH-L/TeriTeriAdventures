using System;
using UnityEngine;

[Serializable]
public class Dialog
{
    private const int MinLines = 3;
    private const int MaxLines = 10;

    public string name;

    [TextArea(MinLines, MaxLines)] public string[] sentences;
}