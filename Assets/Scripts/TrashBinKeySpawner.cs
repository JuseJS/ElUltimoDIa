using UnityEngine;

public class TrashBinKeySpawner : MonoBehaviour
{
    public GameObject[] trashBins; // Array con los 3 cubos de basura
    public GameObject keyPrefab;   // Prefab de la llave
    
    private void Start()
    {
        SpawnKeyRandomly();
    }

    private void SpawnKeyRandomly()
    {
        if (trashBins.Length == 0) return;

        // Seleccionar un cubo de basura aleatorio
        int randomBinIndex = Random.Range(0, trashBins.Length);
        GameObject selectedBin = trashBins[randomBinIndex];

        // Instanciar la llave en el cubo seleccionado
        Vector3 spawnPosition = selectedBin.transform.position;
        Instantiate(keyPrefab, spawnPosition, Quaternion.identity, selectedBin.transform);
    }
}