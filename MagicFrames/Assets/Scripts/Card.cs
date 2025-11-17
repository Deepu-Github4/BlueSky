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

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = backSprite; // Start with back side
    }

    public void OnCardClicked()
    {
        if (isMatched || isFlipped)
            return;

        StartCoroutine(FlipCard());
        GameManager.Instance.OnCardRevealed(this);
    }

    private IEnumerator FlipCard()
    {
        // Flip out
        for (float t = 0; t < 1; t += Time.deltaTime * 8)
        {
            transform.localScale = new Vector3(1 - t, 1, 1);
            yield return null;
        }

        // Change image
        image.sprite = isFlipped ? backSprite : frontSprite;
        isFlipped = !isFlipped;

        // Flip in
        for (float t = 0; t < 1; t += Time.deltaTime * 8)
        {
            transform.localScale = new Vector3(t, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void FlipBack()
    {
        if (!isMatched && isFlipped)
            StartCoroutine(FlipCard());
    }
}
