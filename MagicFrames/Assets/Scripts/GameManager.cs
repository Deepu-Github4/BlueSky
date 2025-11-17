using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float totalSpacingX = spacingX * (columns - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float availableWidth = areaWidth - totalSpacingX;
        float availableHeight = areaHeight - totalSpacingY;

        float newWidth = availableWidth / columns;
        float newHeight = availableHeight / rows;

        float aspect = 140f / 190f;

        if (newWidth / newHeight > aspect)
            newWidth = newHeight * aspect;
        else
            newHeight = newWidth / aspect;

        grid.cellSize = new Vector2(newWidth, newHeight);
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
        }
        else
        {
            comboCount = 0;
            StartCoroutine(FlipBackAfterDelay(cardA, cardB));
        }

        revealedCards.Clear();
        CheckGameOver();
    }

    private System.Collections.IEnumerator FlipBackAfterDelay(Card a, Card b)
    {
        yield return new WaitForSeconds(0.6f);
        a.FlipBack();
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

        LevelManager.Instance?.NextLevel();
    }

    public void RestartLevel()
    {
        GenerateBoard();
    }
}
