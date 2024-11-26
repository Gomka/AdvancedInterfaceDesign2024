using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.GraphicsBuffer;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int sphereSize = 20;
    [SerializeField] private Program trainingProgram;

    private void Awake()
    {
        // Get program
        // DontDestroyOnLoad object que contenga program??
        // Initialize UI with program info

        ShowGraph();

        // get a song 
        // Initialize song UI
        // Start training & song
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(sphereSize, sphereSize);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;

        return gameObject;
    }

    private void ShowGraph()
    {
        int currDuration = 0;
        float target = 0;

        GameObject lastCircle = null;

        foreach (Section section in trainingProgram.sections) 
        {
            GameObject circleGO = CreateCircle(new Vector2(target * (graphContainer.rect.width), section.bpm * 3));

            if (lastCircle != null)
            {
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circleGO.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircle = circleGO;

            currDuration += section.duration;
            target = (float) currDuration / (float) trainingProgram.totalDuration;

            circleGO = CreateCircle(new Vector2(target * (graphContainer.rect.width), section.bpm * 3));

            if (lastCircle != null) 
            {
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circleGO.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircle = circleGO;
        }
    }

    private void CreateDotConnection(Vector2 dotPostitionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        Vector2 dir = (dotPositionB - dotPostitionA).normalized;
        float distance = Vector2.Distance(dotPostitionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPostitionA + dir * distance * .5f;

        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));

    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

}
