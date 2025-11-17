using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Board")]
    public Transform boardContainer;
    public GameObject cardPrefab;
    public Sprite[] cardFronts;
    public Sprite cardBack;

    [Header("Game Settings")]
    public int rows = 2;
    public int columns = 2;

    private List<Card> revealedCards = new List<Card>();
    public int score = 0;
    private int comboCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {

        StopAllCoroutines();

        foreach (Transform child in boardContainer)
            Destroy(child.gameObject);

        int totalCards = rows * columns;
        List<int> cardIDs = new List<int>();

        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        for (int i = 0; i < cardIDs.Count; i++)
        {
            int rand = Random.Range(i, cardIDs.Count);
            (cardIDs[i], cardIDs[rand]) = (cardIDs[rand], cardIDs[i]);
        }

        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject obj = Instantiate(cardPrefab, boardContainer);
            Card card = obj.GetComponent<Card>();

            card.cardID = cardIDs[i];
            card.frontSprite = cardFronts[cardIDs[i]];
            card.backSprite = cardBack;
        }

        GridLayoutGroup grid = boardContainer.GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        AdjustGridSize();
    }

    private void AdjustGridSize()
    {
        GridLayoutGroup grid = boardContainer.GetComponent<GridLayoutGroup>();
        RectTransform rt = boardContainer.GetComponent<RectTransform>();

        float areaWidth = rt.rect.width;
        float areaHeight = rt.rect.height;

        int totalCards = rows * columns;

        float targetCardSize = 120f; 

        if (totalCards >= 24)
            targetCardSize = 80f;  

        float requiredWidth = (targetCardSize * columns) + (targetCardSize * (columns - 1));
        float requiredHeight = (targetCardSize * rows) + (targetCardSize * (rows - 1));

        float scale = 1f;

        if (requiredWidth > areaWidth || requiredHeight > areaHeight)
        {
            float widthScale = areaWidth / requiredWidth;
            float heightScale = areaHeight / requiredHeight;
            scale = Mathf.Min(widthScale, heightScale);
        }

        float finalSize = targetCardSize * scale;

        float spacing = finalSize;

        grid.cellSize = new Vector2(finalSize, finalSize);
        grid.spacing = new Vector2(spacing, spacing);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
    }


    public void OnCardRevealed(Card card)
    {
        if (revealedCards.Count < 2)
            revealedCards.Add(card);

        if (revealedCards.Count == 2)
            CheckMatch();
    }

    private void CheckMatch()
    {
        Card cardA = revealedCards[0];
        Card cardB = revealedCards[1];

        if (cardA.cardID == cardB.cardID)
        {
            cardA.SetMatched();
            cardB.SetMatched();

            comboCount++;
            AddScore(10 + comboCount * 2);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.correctMatch);
        }
        else
        {
            comboCount = 0;
            StartCoroutine(FlipBackAfterDelay(cardA, cardB));
            AudioManager.Instance.PlaySFX(AudioManager.Instance.wrongMatch);
        }

        revealedCards.Clear();
        CheckGameOver();
    }

    private IEnumerator FlipBackAfterDelay(Card a, Card b)
    {
        yield return new WaitForSeconds(0.6f);

        if (a != null && a.gameObject != null && !a.IsMatched())
            a.FlipBack();

        if (b != null && b.gameObject != null && !b.IsMatched())
            b.FlipBack();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }

    private void CheckGameOver()
    {
        foreach (Transform child in boardContainer)
        {
            Card card = child.GetComponent<Card>();
            if (card != null && !card.IsMatched())
                return;
        }

        AudioManager.Instance.PlaySFX(AudioManager.Instance.levelComplete);
        LevelManager.Instance?.NextLevel();
    }
    public void RestartButton()
    {
        LevelManager.Instance.RestartGame();
    }
    public void HomeButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }

    public IEnumerator InitialPreview(float delay)
    {
        foreach (Transform t in boardContainer)
            t.GetComponent<Card>().FlipFaceUpInstant();

        yield return new WaitForSeconds(delay);

        foreach (Transform t in boardContainer)
            t.GetComponent<Card>().FlipFaceDownInstant();
    }
}
