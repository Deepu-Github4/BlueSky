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

    public void LoadLevel(int level)
    {
        currentLevel = level;

        switch (level)
        {
            case 1:
                GameManager.Instance.rows = 2;
                GameManager.Instance.columns = 2;
                break;

            case 2:
                GameManager.Instance.rows = 2;
                GameManager.Instance.columns = 3;
                break;

            case 3:
                GameManager.Instance.rows = 3;
                GameManager.Instance.columns = 4;
                break;

            case 4:
                GameManager.Instance.rows = 4;
                GameManager.Instance.columns = 4;
                break;

            case 5:
                GameManager.Instance.rows = 5;
                GameManager.Instance.columns = 6;
                break;

            default:
                GameManager.Instance.rows = 6;
                GameManager.Instance.columns = 6;
                break;
        }

        GameManager.Instance.GenerateBoard();
        UIManager.Instance.UpdateLevel(level);
    }

    public void NextLevel()
    {
        Debug.Log("Check");
        LoadLevel(currentLevel + 1);
    }
}
