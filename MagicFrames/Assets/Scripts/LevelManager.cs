using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentLevel = 1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        currentLevel = SaveSystem.LoadLevel();
        UIManager.Instance.UpdateLevel(currentLevel);

        GameManager.Instance.score = SaveSystem.LoadScore();
        UIManager.Instance.UpdateScore(GameManager.Instance.score);

        LoadLevel();

        yield return TransitionManager.Instance.FadeIn(0.5f);
    }

    public void LoadLevel()
    {
        if (currentLevel == 1)
        {
            GameManager.Instance.rows = 2;
            GameManager.Instance.columns = 2;
        }
        else if (currentLevel == 2)
        {
            GameManager.Instance.rows = 2;
            GameManager.Instance.columns = 3;
        }
        else if (currentLevel == 3)
        {
            GameManager.Instance.rows = 3;
            GameManager.Instance.columns = 4;
        }
        else if (currentLevel == 4)
        {
            GameManager.Instance.rows = 4;
            GameManager.Instance.columns = 4;
        }
        else if (currentLevel == 5)
        {
            GameManager.Instance.rows = 4;
            GameManager.Instance.columns = 5;
        }
        else
        {
            GameManager.Instance.rows = 4 + (currentLevel - 5);
            GameManager.Instance.columns = 4 + (currentLevel - 5);
        }

        GameManager.Instance.GenerateBoard();
    }

public void NextLevel()
    {
        StartCoroutine(LevelTransitionRoutine());
    }

    private IEnumerator LevelTransitionRoutine()
    {
        // Fade Out
        yield return TransitionManager.Instance.FadeOut(0.4f);

        currentLevel++;
        SaveSystem.SaveProgress(currentLevel, GameManager.Instance.score);

        UIManager.Instance.UpdateLevel(currentLevel);
        LoadLevel();

        // Fade In
        yield return TransitionManager.Instance.FadeIn(0.4f);
    }


}
