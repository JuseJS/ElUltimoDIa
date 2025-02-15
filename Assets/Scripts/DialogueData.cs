using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        public string speaker;
        public string text;
        public DialogueAction action;
    }

    public DialogueLine[] lines;
}

public enum DialogueAction
{
    None,
    RemoveMainKey,
    StartClassroomKeyMission,
    StartGoToClassMission,
    CompleteGame
}