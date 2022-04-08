using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject mainMenu;
    private AsyncOperation loadingOperation;
    private bool startLoading = false;

    void Start() 
    {
    }
    void Update()
    {
        if (progressBar.gameObject.activeSelf && startLoading)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        }
    }

    public void NextScene()
    {
        mainMenu.SetActive(false);
        progressBar.gameObject.SetActive(true);
        StartCoroutine(WaitSeconds(3.0f));
        startLoading = true;
        loadingOperation = SceneManager.LoadSceneAsync(1);
    }
    IEnumerator WaitSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
