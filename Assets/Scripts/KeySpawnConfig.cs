using UnityEngine;

[CreateAssetMenu(fileName = "New Key Spawn Data", menuName = "Game/Key Spawn Data")]
public class KeySpawnData : ScriptableObject
{
    public string areaId;
    public Key keyToSpawn;
    [TextArea(2,4)]
    public string searchFailMessage = "No has encontrado nada aquí.";
    public string searchSuccessMessage = "¡Has encontrado una llave!";
}