using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager:MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}