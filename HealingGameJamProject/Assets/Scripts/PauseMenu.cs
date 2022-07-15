using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite play;
    SpriteRenderer spriteRenderer;
    public bool paused = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        paused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        spriteRenderer.sprite = play;
        paused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        spriteRenderer.sprite = pause;
        paused = false;
    }
}
