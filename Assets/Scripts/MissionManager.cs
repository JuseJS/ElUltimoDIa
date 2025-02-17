using UnityEngine;
using System;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public event Action<Mission> OnMissionStarted;
    public event Action<Mission> OnMissionCompleted;

    [SerializeField] private MissionData gameMissions;
    [SerializeField] private AudioClip missionCompletedSound;
    private AudioSource audioSource;

    public Mission CurrentMission { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        AudioListener listener = gameObject.GetComponent<AudioListener>();
        if (listener != null)
        {
            Destroy(listener);
        }
    }

    public void InitializeMissions()
    {
        StartMission(gameMissions.findMainKeyMission);
    }

    public void StartMission(Mission mission)
    {
        if (mission == null) return;

        CurrentMission = mission;
        OnMissionStarted?.Invoke(CurrentMission);
        UIManager.Instance.UpdateMission(CurrentMission.description);
    }

    public void CompleteMission()
    {
        if (CurrentMission == null) return;

        OnMissionCompleted?.Invoke(CurrentMission);

        // Reproducir sonido de misión completada
        if (audioSource != null && missionCompletedSound != null)
        {
            audioSource.PlayOneShot(missionCompletedSound);
        }

        if (CurrentMission.nextMission != null)
        {
            StartMission(CurrentMission.nextMission);
        }
    }
}