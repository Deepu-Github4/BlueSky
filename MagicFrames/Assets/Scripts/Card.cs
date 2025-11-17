using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardID;

    [Header("Images")]
    public Image backImage;     
    public Image frontImage;    

    [Header("Sprites")]
    public Sprite frontSprite;   
    public Sprite backSprite;    

    private bool isFlipped = false;
    private bool isMatched = false;
    private bool isAnimating = false;
    public bool initialized = false;

    private void Start()
    {
        backImage.sprite = backSprite;
        frontImage.sprite = frontSprite;

        if (initialized) return;

        backImage.gameObject.SetActive(true);
        frontImage.gameObject.SetActive(false);
    }

    public void OnCardClicked()
    {
        if (isMatched || isFlipped || isAnimating)
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.cardClick);
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

        isFlipped = !isFlipped;
        backImage.gameObject.SetActive(!isFlipped);
        frontImage.gameObject.SetActive(isFlipped);

        for (float t = 0; t < 1; t += Time.deltaTime * 8)
        {
            transform.localScale = new Vector3(t, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
        isAnimating = false;
    }

    public void FlipBack()
    {
        if (isMatched || !isFlipped || isAnimating)
            return;

        StartCoroutine(FlipCard());
    }

    public void SetMatched()
    {
        isMatched = true;
        Color c = frontImage.color;
        c.a = 0.5f;
        frontImage.color = c;
    }

    public bool IsMatched()
    {
        return isMatched;
    }

    public void FlipFaceUpInstant()
    {
        isFlipped = true;
        backImage.gameObject.SetActive(false);
        frontImage.gameObject.SetActive(true);
    }

    public void FlipFaceDownInstant()
    {
        isFlipped = false;
        backImage.gameObject.SetActive(true);
        frontImage.gameObject.SetActive(false);
    }
}
