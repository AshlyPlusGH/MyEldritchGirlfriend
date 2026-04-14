using NaughtyAttributes;
using UnityEngine;
using Yarn.Unity;

public class YARN_DIALOGUE_Call : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private DialogueRunner dialogueRunner;

    private static YARN_DIALOGUE_Call instance;

    void Awake(){ Setup(); }
    public void Setup()
    {
        if (instance != null){ return; }

        instance = this;
    }

    public static void StartNode(string name)
    {
        instance.dialogueRunner.StartDialogue(name);
    }
}