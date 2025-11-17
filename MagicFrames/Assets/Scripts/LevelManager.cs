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
        int totalCards = GetTotalCardsForLevel(currentLevel);

        int r, c;
        CalculateGrid(totalCards, out r, out c);

        GameManager.Instance.rows = r;
        GameManager.Instance.columns = c;

        GameManager.Instance.GenerateBoard();
    }

    public void NextLevel()
    {
        StartCoroutine(LevelTransitionRoutine());
    }

    private IEnumerator LevelTransitionRoutine()
    {
        yield return TransitionManager.Instance.FadeOut(0.4f);

        currentLevel++;
        SaveSystem.SaveProgress(currentLevel, GameManager.Instance.score);

        UIManager.Instance.UpdateLevel(currentLevel);
        LoadLevel();

        yield return TransitionManager.Instance.FadeIn(0.4f);
    }

    public void CalculateGrid(int totalCards, out int rows, out int cols)
    {
        rows = Mathf.CeilToInt(Mathf.Sqrt(totalCards));
        cols = Mathf.CeilToInt((float)totalCards / rows);

        while (rows * cols != totalCards)
        {
            rows++;
            cols = Mathf.CeilToInt((float)totalCards / rows);
        }
    }

    private int GetTotalCardsForLevel(int level)
    {
        return 4 + (level - 1) * 4;
    }

}
