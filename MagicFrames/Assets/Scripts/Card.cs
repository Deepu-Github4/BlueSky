using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardID; // Unique ID for matching pairs

    [Header("Images")]
    public Sprite frontSprite;
    public Sprite backSprite;

    private Image image;
    private bool isFlipped = false;
    private bool isMatched = false;
    private bool isAnimating = false;


    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = backSprite; // Start with back side
    }

    public void OnCardClicked()
    {
        if (isMatched || isFlipped || isAnimating)
            return;

        StartCoroutine(FlipCard());
        GameManager.Instance.OnCardRevealed(this);
    }


    private IEnumerator FlipCard()
    {
        isAnimating = true;

        for (float t = 0; t < 1; t += Time.deltaTime * 8)
        {
            transform.localScale = new Vector3(1 - t, 1, 1);
            yield return null;
        }

        image.sprite = isFlipped ? backSprite : frontSprite;
        isFlipped = !isFlipped;

        for (float t = 0; t < 1; t += Time.deltaTime * 8)
        {
            transform.localScale = new Vector3(t, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;

        isAnimating = false;
    }


    public void SetMatched()
    {
        isMatched = true;
    }

    public void FlipBack()
    {
        if (isMatched || isAnimating || !isFlipped)
            return;

        StartCoroutine(FlipCard());
    }

    public bool IsMatched()
    {
        return isMatched;
    }
}
