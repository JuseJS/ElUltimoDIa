using UnityEngine;

[CreateAssetMenu(fileName = "GameMissions", menuName = "Missions/Game Missions")]
public class MissionData : ScriptableObject
{
    [Header("Misiones Principales")]
    public Mission findMainKeyMission;
    public Mission enterSchoolMission;
    public Mission talkToSecretaryMission;
    public Mission findClassroomKeyMission;
    public Mission accessComputerMission;
    public Mission submitWorkMission;
    public Mission returnKeykMission;
}