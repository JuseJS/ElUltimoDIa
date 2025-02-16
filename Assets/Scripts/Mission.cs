using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    public string missionId;
    [TextArea(2, 4)]
    public string description;
    public Mission nextMission;
}