using UnityEngine;
using Yarn.Unity;

public class YARN_DIALOGUE_TriggerEffectsOnNodeCompleted : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private soDATA_OnDialogueCompletedEffectTable data;

    void Awake(){ Setup(); }
    public void Setup(){  }

    private void OnEnable()
    {
        RegisterListeners();
    }
    private void RegisterListeners(){ dialogueRunner.onNodeComplete.AddListener(OnNodeCompleted); }
    private void OnDisable()
    {
        UnregisterListeners();
    }
    private void UnregisterListeners(){ dialogueRunner.onNodeComplete.AddListener(OnNodeCompleted); }

    private void OnNodeCompleted(string completedNodeName)
    {
        if (!data.STAT_data.ContainsKey(completedNodeName)){ Log("No Node found with that name!"); return; }
        data.STAT_data[completedNodeName].Apply();
    }
}