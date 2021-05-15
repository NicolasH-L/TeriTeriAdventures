using System;
using UnityEngine;

namespace NpcDialog
{
    [Serializable]
    public class Dialog
    {
        private const int MinLines = 3;
        private const int MaxLines = 10;

        public string name;

        [TextArea(MinLines, MaxLines)] public string[] sentences;
    }
}