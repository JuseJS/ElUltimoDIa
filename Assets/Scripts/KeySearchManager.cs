using UnityEngine;
using System.Collections.Generic;

public class KeySearchManager : MonoBehaviour
{
    public static KeySearchManager Instance { get; private set; }

    [SerializeField] private KeySpawnData[] searchAreas;
    private Dictionary<string, GameObject> keyLocations = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
            InitializeSearchAreas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSearchAreas()
    {
        Debug.Log($"Inicializando {searchAreas.Length} áreas de búsqueda");

        foreach (var areaData in searchAreas)
        {
            if (areaData == null) continue;

            SearchableArea[] areas = FindObjectsOfType<SearchableArea>();
            List<GameObject> searchableObjects = new List<GameObject>();

            foreach (var area in areas)
            {
                if (area.areaId == areaData.areaId)
                {
                    // Verificar si ya tiene SearchableObject
                    var searchable = area.GetComponent<SearchableObject>();
                    if (searchable == null)
                    {
                        searchable = area.gameObject.AddComponent<SearchableObject>();
                    }
                    searchableObjects.Add(area.gameObject);
                    Debug.Log($"Objeto búscable encontrado: {area.gameObject.name} en área {area.areaId}");
                }
            }

            if (searchableObjects.Count == 0)
            {
                Debug.LogError($"No se encontraron objetos para el área {areaData.areaId}");
                continue;
            }

            // Inicializar todos como no contenedores de llave
            foreach (var obj in searchableObjects)
            {
                var searchable = obj.GetComponent<SearchableObject>();
                searchable.Initialize(false, null, areaData.searchFailMessage, areaData.searchSuccessMessage);
                Debug.Log($"Inicializado objeto {obj.name} sin llave");
            }

            // Seleccionar uno aleatorio para la llave
            int randomIndex = Random.Range(0, searchableObjects.Count);
            GameObject selectedObject = searchableObjects[randomIndex];
            SearchableObject selectedSearchable = selectedObject.GetComponent<SearchableObject>();

            // Verificar que la llave a spawnear existe
            if (areaData.keyToSpawn == null)
            {
                Debug.LogError($"La llave a spawnear es null en el área {areaData.areaId}");
                continue;
            }

            selectedSearchable.Initialize(true, areaData.keyToSpawn, areaData.searchFailMessage, areaData.searchSuccessMessage);
            keyLocations[areaData.keyToSpawn.name] = selectedObject;

            Debug.Log($"Llave {areaData.keyToSpawn.name} generada en el objeto {selectedObject.name}");
        }
    }

    public bool IsKeyFound(string keyName)
    {
        return !keyLocations.ContainsKey(keyName);
    }

    public void RegisterKeyFound(string keyName)
    {
        if (keyLocations.ContainsKey(keyName))
        {
            keyLocations.Remove(keyName);
        }
    }
}