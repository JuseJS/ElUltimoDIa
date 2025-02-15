using UnityEngine;

public class TrashBinKeySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] trashBins;
    [SerializeField] private Key keyToSpawn;
    
    private void Start()
    {
        if (trashBins == null || trashBins.Length == 0)
        {
            Debug.LogError("No hay cubos de basura asignados al TrashBinKeySpawner"); // prueba
            return;
        }

        // Asegurarse de que todos los cubos tienen el componente SearchableObject
        foreach (GameObject bin in trashBins)
        {
            if (!bin.TryGetComponent<SearchableObject>(out var searchable))
            {
                searchable = bin.AddComponent<SearchableObject>();
            }
            searchable.Initialize(false, null);
        }

        // Seleccionar un cubo aleatorio y asignarle la llave
        int randomBinIndex = Random.Range(0, trashBins.Length);
        GameObject selectedBin = trashBins[randomBinIndex];
        
        // Configurar el cubo seleccionado para contener la llave
        SearchableObject selectedSearchable = selectedBin.GetComponent<SearchableObject>();
        selectedSearchable.Initialize(true, keyToSpawn);
        
        Debug.Log($"Llave generada en el cubo {randomBinIndex}");
    }
}