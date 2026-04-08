using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Runs any code that is GameState related and holds commonly used methods not relating to any one system.
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents); }}

    [Space(10)]

    [SerializeField] private GameObject parent2D;
    [SerializeField] private GameObject parent3D;
    [SerializeField] private Image background;
    [SerializeField] private UnityEvent onNightStarts;
    [SerializeField] private UnityEvent onNightEnds;
    [SerializeField] private UnityEvent onNightOneStarts;
    [SerializeField] private UnityEvent onNightTwoStarts;
    [SerializeField] private UnityEvent onNightThreeStarts;
    [SerializeField] private UnityEvent onNightOneEnds;
    [SerializeField] private UnityEvent onNightTwoEnds;
    [SerializeField] private UnityEvent onNightThreeEnds;

    [SerializeField] private static GameManager instance;
    [SerializeField] private static int dayCount = 0;

    void Awake(){ Setup(); }
    public void Setup()
    {
        if (instance != null){ Destroy(gameObject); return; }

        instance = this;
    }

    public static void EndDay()
    {
        instance.parent3D.SetActive(true);

        instance.onNightStarts.Invoke();

        switch (dayCount)
        {
            case 1:
                instance.onNightOneStarts.Invoke();
                break;
            case 2:
                instance.onNightTwoStarts.Invoke();
                break;
            case 3:
                instance.onNightThreeStarts.Invoke();
                break;
        }

        instance.parent2D.SetActive(false);
    }
    public static void EndNight()
    {
        instance.parent2D.SetActive(true);

        instance.onNightEnds.Invoke();

        switch (dayCount)
        {
            case 1:
                instance.onNightOneEnds.Invoke();
                break;
            case 2:
                instance.onNightTwoEnds.Invoke();
                break;
            case 3:
                instance.onNightThreeEnds.Invoke();
                break;
        }

        instance.parent3D.SetActive(false);
    }

    public static void SwitchTo2D()
    {
        instance.parent3D.SetActive(false);
        instance.parent2D.SetActive(true);
    }
    public static void SwitchTo3D()
    {
        instance.parent2D.SetActive(false);
        instance.parent3D.SetActive(true);
    }
    public static void SetBackground(Sprite newBackground)
    {
        instance.background.sprite = newBackground;
    }

    [Button] public void DEBUG_SwitchTo2D()
    {
        SwitchTo2D();
    }
    [Button] public void DEBUG_SwitchTo3D()
    {
        SwitchTo3D();
    }
}