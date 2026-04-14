using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject qtePanel;  // Are we having simple UI stuff? or something else idk
    public Image progressBar;   // not sure if the fillAmount is usable but that is what i saw on API
    public TMPro.TextMeshProUGUI promptText;     // is it just text or a image for the button mash?

    [Header("Tuning")]
    public float requiredMashCount = 50f;
    public float decayRate = 2f;
    public KeyCode mashKey = KeyCode.Space;
    public float mashIncrement = 1.5f;

    float mashProgress;
    bool qteActive;
    GirlfriendAI activeAI;

    [SerializeField] private UnityEvent unityEventOnQTEStart;
    [SerializeField] private UnityEvent unityEventOnQTEEndSuccessful;
    [SerializeField] private UnityEvent unityEventOnQTEEndFailed;

    public void StartQTE(GirlfriendAI ai)
    {
        activeAI = ai;
        mashProgress = requiredMashCount / 2f;
        qteActive = true;
        qtePanel.SetActive(true);
        RefreshUI();

        unityEventOnQTEStart.Invoke();
    }

    void Update()
    {
        if (!qteActive) return;

        if (Input.GetKeyDown(mashKey)) mashProgress += mashIncrement; //this is the legacy system for now it is okay

        mashProgress -= decayRate * Time.deltaTime;
        mashProgress = Mathf.Clamp(mashProgress, 0f, requiredMashCount);

        RefreshUI();

        if (mashProgress >= requiredMashCount) EndQTE(true);
        else if (mashProgress <= 0f) EndQTE(false);
    }

    void EndQTE(bool escaped)
    {
        qteActive = false;
        qtePanel.SetActive(false);

        if (escaped){
            activeAI.OnPlayerEscapedQTE();// ------------------------------------------------what do we do when we pass QTE? (currently nextwaypoint with patrol)
            unityEventOnQTEEndSuccessful.Invoke();
        }
        else {
            //Debug.Log("you get got"); // -----------------------------------------------------------what do we do when we fail QTE?
            unityEventOnQTEEndFailed.Invoke();
        }
    }

    void RefreshUI() // ---------------------------------------------------------------------depens on what type of UI we are having!
    {
        if (progressBar)
        {
            progressBar.fillAmount = mashProgress / requiredMashCount;
            //progressBar.color = Color.Lerp(Color.red, Color.green, mashProgress / requiredMashCount); //maybe we can do this for the bar if the bar is white
        }
        
        if (promptText)
        {
            promptText.text = $"MASH<br>{mashKey}!";
            promptText.color = Color.Lerp(Color.red, Color.green, mashProgress / requiredMashCount); // idk if this would make sense
            promptText.fontSize = Mathf.Lerp(45f, 50f, mashProgress / requiredMashCount);
        }

    }
}