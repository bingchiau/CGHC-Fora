using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    public string NextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.teleport);
            StartCoroutine(DelayLoadScene());
        }
    }

    private IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(NextScene);
    }
}
