using UnityEngine;

public static class SaveSystem
{
    private const string KEY_LEVEL = "saved_level";
    private const string KEY_SCORE = "saved_score";

    public static void SaveProgress(int level, int score)
    {
        PlayerPrefs.SetInt(KEY_LEVEL, level);
        PlayerPrefs.SetInt(KEY_SCORE, score);
        PlayerPrefs.Save();
    }

    public static int LoadLevel()
    {
        return PlayerPrefs.GetInt(KEY_LEVEL, 1);
    }

    public static int LoadScore()
    {
        return PlayerPrefs.GetInt(KEY_SCORE, 0);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(KEY_LEVEL);
        PlayerPrefs.DeleteKey(KEY_SCORE);
    }
}
