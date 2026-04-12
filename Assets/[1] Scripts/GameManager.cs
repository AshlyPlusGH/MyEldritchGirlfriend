using System;
using System.Threading;
using NaughtyAttributes;
using TMPro;
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
        public static int dayCount = 1;
        public static float timeSpentInNight;
        public static bool ritualCircleCompleted;
        public static bool machineFixed;

        //Completion Data
        private static CompletionData completionData = new();
    #endregion

    #region INSTANCE REFERENCES
        [SerializeField] private GameObject parent2D;
        [SerializeField] private GameObject parent3D;
        [SerializeField] private GameObject parentEndScreen;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI endScreenSummaryTMP;
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

    public void Setup()
    {
        if (instance != null){ Destroy(gameObject); return; }

        instance = this;
        STATIC_Setup();
    }
    public static void STATIC_Setup()
    {
        dayCount = 1;
        timeSpentInNight = 0;

        ritualCircleCompleted = false;
        machineFixed = false;
        
        completionData = new();

        //For Debug Purposes
        if (instance.parent3D.activeInHierarchy){ StartNight(); }
        else
        {
            YARN_DIALOGUE_Call.StartNode(RULES.dayOneNode);
        }
    }

    #region NIGHT PROGRESS TRACKING
        public void Update()
        {
                if (instance == null){ return; }
            IncrementTimer();

            if (QueryIsNightEnd()){ EndNightSurvived(); }
        }
        private static void IncrementTimer()
        {
                if (instance == null){ return; }
                if (!instance.parent3D.activeInHierarchy){ return; }
            timeSpentInNight += Time.deltaTime;
        }
        private static bool QueryIsNightEnd()
        {
            if (!instance.parent3D.activeInHierarchy){ return false; }
            if (timeSpentInNight < RULES.nightLength){ return false; }
            if (!FindAnyObjectByType<TAG_PLAYER>().GetComponent<FirstPersonController>().enabled){ return false; }

            switch (dayCount)
            {
                case 1: break;
                case 2: if (!ritualCircleCompleted){ return false; } break;
                case 3: if (!ritualCircleCompleted || !machineFixed){ return false; } break;
            }
            
            return true;
        }
    #endregion

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
        public static void IncrementSurvivedNights()
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
        public static void StartDay()
        {
                if (dayCount > 3){ return; }
            switch (dayCount)
            {
                case 0:
                    Debug.Log("Day Zero does not exist!");
                    SwitchTo2D();
                    break;
                case 1:
                    instance.onNightOneEnds.Invoke();
                    SwitchTo2D();
                    YARN_DIALOGUE_Call.StartNode(RULES.dayTwoNode);
                    break;
                case 2:
                    instance.onNightTwoEnds.Invoke();
                    SwitchTo2D();
                    YARN_DIALOGUE_Call.StartNode(RULES.dayThreeNode);
                    break;
                case 3:
                    instance.onNightThreeEnds.Invoke();
                    TriggerEndScreen();
                    break;
            }
            dayCount++;

            instance.parent2D.SetActive(true);
        }
        public static void EndDay()
        {
                if (dayCount > 3){ return; }
            instance.parent2D.SetActive(false);

            StartNight();
        }
        
        public static void StartNight()
        {
                if (dayCount > 3){ return; }
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
            timeSpentInNight = 0;
            ritualCircleCompleted = false;
            machineFixed = false;

            instance.parent3D.SetActive(true);

            instance.onNightStarts.Invoke();
        }
        public static void EndNight()
        {
                if (dayCount > 3){ return; }
            instance.parent3D.SetActive(false);

            instance.onNightEnds.Invoke();

            StartDay();
        }
        public static void EndNightInjured()
        {
            EndNight();
        }
        public static void EndNightSurvived()
        {
            IncrementSurvivedNights();
            EndNight();
        }

        public static void RitualCircleCompleted()
        {
            ritualCircleCompleted = true;
        }
        public static void MachineFixed()
        {
            machineFixed = true;
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
            float finalScore = 
                (completionData.lovePoints * RULES.lovePointCompletionWeight) + 
                (completionData.menInRedKilled * RULES.menInRedCompletionWeight) + 
                (completionData.nightsUninjured * RULES.survivedNightsCompletionWeight) + 
                (completionData.flowersCollected * RULES.flowerCompletionWeight);
            instance.endScreenSummaryTMP.text = (
                "Final Completion: " + finalScore*100 + "%\n" +
                "Gained Love Points: " + completionData.lovePoints + "\n" +
                "Men in Red Kiled: " + completionData.menInRedKilled + "\n" +
                "Nights without Injury: " + completionData.nightsUninjured + "\n" +
                "Flowers Collected: " + completionData.flowersCollected + "\n"
            );
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

    #region DEBUG CALLS
        [Button] public void DEBUG_SwitchTo2D()
        {
            parent3D.SetActive(false);
            parent2D.SetActive(true);
        }
        [Button] public void DEBUG_SwitchTo3D()
        {
            parent2D.SetActive(false);
            parent3D.SetActive(true);
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
            IncrementSurvivedNights();
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
        [Button] public void DEBUG_TriggerGameEnd()
        {
                if (instance.parent2D.activeInHierarchy){ EndDay(); }
            dayCount = 3;

            EndNight();
        }
    #endregion
}