using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform boardContainer;
    public GameObject cardPrefab;
    public Sprite[] cardFronts;
    public Sprite cardBack;

    [Header("Board Settings")]
    public int rows = 2;
    public int columns = 2;

    private List<Card> revealedCards = new List<Card>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
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

        AdjustGridSize();
    }

    private void AdjustGridSize()
    {
        GridLayoutGroup grid = boardContainer.GetComponent<GridLayoutGroup>();

        RectTransform rt = boardContainer.GetComponent<RectTransform>();

        float width = rt.rect.width;
        float height = rt.rect.height;

        float cellWidth = width / columns - grid.spacing.x;
        float cellHeight = height / rows - grid.spacing.y;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(cellSize, cellSize);
    }

    public void OnCardRevealed(Card card)
    {
        revealedCards.Add(card);

        if (revealedCards.Count == 2)
        {
            CheckMatch();
        }
    }

    private void CheckMatch()
    {
        Card cardA = revealedCards[0];
        Card cardB = revealedCards[1];

        if (cardA.cardID == cardB.cardID)
        {
            cardA.SetMatched();
            cardB.SetMatched();
        }
        else
        {
            StartCoroutine(FlipBackAfterDelay(cardA, cardB));
        }
        revealedCards.Clear();
    }

    private System.Collections.IEnumerator FlipBackAfterDelay(Card a, Card b)
    {
        yield return new WaitForSeconds(0.6f);

        a.FlipBack();
        b.FlipBack();
    }
}
