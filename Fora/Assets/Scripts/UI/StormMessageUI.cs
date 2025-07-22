using System.Collections;
using UnityEngine;
using TMPro;

public class StormMessageUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI stormText;

    [Header("Message Settings")]
    [SerializeField] private string message = "The storm is approaching.";
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float displayDurationAfterTyping = 2f; // how long to keep it visible

    public void ShowStormMessage()
    {
        stormText.text = "";
        stormText.gameObject.SetActive(true);
        StartCoroutine(TypeMessage());
    }

    private IEnumerator TypeMessage()
    {
        for (int i = 0; i < message.Length; i++)
        {
            stormText.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait a bit after message is fully shown
        yield return new WaitForSeconds(displayDurationAfterTyping);

        stormText.gameObject.SetActive(false);
    }
}
