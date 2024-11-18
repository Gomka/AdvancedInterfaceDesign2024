using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.GraphicsBuffer;

public class TrainingGraph : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int sphereSize = 20;
    [SerializeField] private Program trainingProgram;

    private void Awake()
    {
        ShowGraph();
    }

    private void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(sphereSize, sphereSize);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
    }

    private void ShowGraph()
    {
        CreateCircle(new Vector2(0, 0));

        int currDuration = 0;
        float target = 0;

        foreach (Section section in trainingProgram.sections) 
        {
            currDuration += section.duration;
            target = (float) currDuration / (float) trainingProgram.totalDuration;

            CreateCircle(new Vector2(target * graphContainer.rect.width, section.bpm * 2));
        }
    }
}
