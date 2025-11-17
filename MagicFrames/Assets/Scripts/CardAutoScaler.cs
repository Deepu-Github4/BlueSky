using UnityEngine;

public class CardAutoScaler : MonoBehaviour
{
    public RectTransform icon;

    [Range(0.5f, 1.2f)]
    public float iconScale = 1.0f;   

    public float minIconSize = 60f; 

    void LateUpdate()
    {
        float cardSize = ((RectTransform)transform).rect.width;

        float targetSize = cardSize * iconScale;

        targetSize = Mathf.Max(targetSize, minIconSize);

        icon.sizeDelta = new Vector2(targetSize, targetSize);
    }
}
