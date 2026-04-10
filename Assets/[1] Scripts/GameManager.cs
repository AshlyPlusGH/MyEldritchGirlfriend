using System;
using NaughtyAttributes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Runs any code that is GameState related and holds commonly used methods not relating to any one system.
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents); }}

    [Space(10)]

    #region STATIC RUN DATA
        [SerializeField] private static int dayCount = 1;

        //Completion Data
        [SerializeField, NaughtyAttributes.ReadOnly] private static CompletionData completionData = new();
    #endregion

    #region INSTANCE REFERENCES
        [SerializeField] private GameObject parent2D;
        [SerializeField] private GameObject parent3D;
        [SerializeField] private GameObject parentEndScreen;
        [SerializeField] private Image background;
    #endregion

    [Space(10)]

    #region UNITY EVENTS
        [SerializeField] private UnityEvent onNightStarts;
        [SerializeField] private UnityEvent onNightEnds;
        [SerializeField] private UnityEvent onNightOneStarts;
        [SerializeField] private UnityEvent onNightTwoStarts;
        [SerializeField] private UnityEvent onNightThreeStarts;
        [SerializeField] private UnityEvent onNightOneEnds;
        [SerializeField] private UnityEvent onNightTwoEnds;
        [SerializeField] private UnityEvent onNightThreeEnds;
    #endregion

    private static GameManager instance;

    void Awake(){ Setup(); }
    public void Setup()
    {
        if (instance != null){ Destroy(gameObject); return; }

        instance = this;
    }

    #region COMPLETION CALLS
        public static void AddCollectable(ENUM_CollectableTypes type)
        {
            switch (type)
            {
                case ENUM_CollectableTypes.Flower:
                        if ((completionData.flowersCollected + 1) > RULES.totalFlowers)
                        {
                            Debug.Log("More Flowers collected than expected total! Disregarding last collected...");
                            return; 
                        }
                    completionData.flowersCollected ++;
                    break;
            }
        }
        public static void SurvivedNight()
        {
            completionData.nightsUninjured++;
        }

        public static void ManInRedKilled()
        {
            completionData.menInRedKilled++;
        }

        public static void AddLovePoint()
        {
            completionData.lovePoints++;
        }
    #endregion

    #region GAME FUNCTION CALLS
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
            instance.onNightEnds.Invoke();
            switch (dayCount)
            {
                case 0:
                    Debug.Log("Day Zero does not exist!");
                    SwitchTo2D();
                    break;
                case 1:
                    instance.onNightOneEnds.Invoke();
                    SwitchTo2D();
                    break;
                case 2:
                    instance.onNightTwoEnds.Invoke();
                    SwitchTo2D();
                    break;
                case 3:
                    instance.onNightThreeEnds.Invoke();
                    TriggerEndScreen();
                    break;
            }
            dayCount++;
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
        public static void TriggerEndScreen()
        {
            //Trigger end screen stuff
            instance.parentEndScreen.SetActive(true);
            SwitchTo2D();
        }
    #endregion

    #region GETTERS AND SETTERS
        public static float GetCompletionRate()
        {
            float completionRate = 0.0f;

            completionRate += RULES.survivedNightsCompletionWeight * completionData.nightsUninjured;
            completionRate += RULES.menInRedCompletionWeight * completionData.menInRedKilled;
            completionRate += RULES.lovePointCompletionWeight * completionData.lovePoints;

            //Flower Bonus
            completionRate += RULES.flowerCompletionWeight * completionData.flowersCollected;

            return completionRate;
        }
    #endregion

    [Button] public void DEBUG_SwitchTo2D()
    {
        SwitchTo2D();
    }
    [Button] public void DEBUG_SwitchTo3D()
    {
        SwitchTo3D();
    }
    [Button] public void DEBUG_EndNight()
    {
        EndNight();
    }
    [Button] public void DEBUG_EndDay()
    {
        EndDay();
    }
    [Button] public void DEBUG_AddFlower()
    {
        AddCollectable(ENUM_CollectableTypes.Flower);
        Log("Total Flowers: " + completionData.flowersCollected + "/" + RULES.totalFlowers);
    }
    [Button] public void DEBUG_AddManInRedKill()
    {
        ManInRedKilled();
        Log("Total Kills: " + completionData.menInRedKilled + "/" + RULES.totalMenInRed);
    }
    [Button] public void DEBUG_AddNightSurvived()
    {
        SurvivedNight();
        Log("Total Nights Survived: " + completionData.nightsUninjured + "/" + RULES.totalNights);
    }
    [Button] public void DEBUG_AddLovePoint()
    {
        AddLovePoint();
        Log("Total Love Points: " + completionData.lovePoints + "/" + RULES.totalLovePoints);
    }
    [Button] public void DEBUG_TriggerEndScreen()
    {
        TriggerEndScreen();
    }
}