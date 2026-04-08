using UnityEngine;
using Yarn.Unity;

public class UI_DialogueStarter : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents); }}

    [Space(10)]

    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string startNode = "Intro";

    private void OnEnable()
    {
        Log("Starting Dialogue!");
        
        _ = dialogueRunner.StartDialogue(startNode);
    }
}
