using UnityEngine;
using Yarn.Unity;

public class UI_DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string startNode = "Intro";
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        _ = dialogueRunner.StartDialogue(startNode);
    }

    private void OnDisable()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }

    private void OnDialogueComplete()
    {
        gameManager.SwitchTo3D();
    }
}
