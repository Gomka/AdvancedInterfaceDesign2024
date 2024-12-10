using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.GraphicsBuffer;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int sphereSize = 20;
    [SerializeField] private Program trainingProgram;
    [SerializeField] private TMP_Text programName, spm, target, timer, distance, songName;
    [SerializeField] private RectTransform playerCircle, progressLine;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioResource [] songs, intenseSongs;
    private StepCounter stepCounter;
    private int count = 0, targetSection = 0;
    private bool musicPaused = true;

    private void Awake()
    {
        stepCounter = StepCounter.Instance;
        // Get program
        // DontDestroyOnLoad object que contenga program??
        
        ShowGraph();
        StartMusic();
        programName.text = trainingProgram.name;
    }

    private void Update()
    {
        UpdateTexts();
        UpdatePlayer();

        if(Time.timeSinceLevelLoad > (float)trainingProgram.totalDuration)
        {
            sceneController.LoadScene(0); // end training
        }
    }

    public void ToggleMusic()
    {
        if (musicPaused)
        {
            StartMusic();
        }
        else
        {
            StopMusic();
        }
    }


    private void StopMusic()
    {
        musicPaused = true;
        musicPlayer.Stop();
    }

    public void StartMusic()
    {
        if (stepCounter.stepsMinute > trainingProgram.sections[targetSection].bpm)
        {
            int rand = Random.Range(0, songs.Length);
            musicPlayer.resource = songs[rand];
            songName.text = songs[rand].name;
            musicPlayer.Play();
        }
        else
        {
            int rand = Random.Range(0, intenseSongs.Length);
            musicPlayer.resource = intenseSongs[rand];
            songName.text = intenseSongs[rand].name;
            musicPlayer.Play();
        }
        musicPaused = false;
    }

    private void UpdatePlayer()
    {
        float xfloat = (Time.timeSinceLevelLoad / (float)trainingProgram.totalDuration) * graphContainer.rect.width;
        float yfloat = stepCounter.stepsMinute * 3;

        playerCircle.anchoredPosition = new Vector2 (xfloat, yfloat);
        progressLine.anchoredPosition = new Vector2(xfloat, 0);
    }

    private void UpdateTexts()
    {
        count = 0;
        targetSection = 0;

        spm.text = "SPM: "+stepCounter.stepsMinute;

        foreach (Section section in trainingProgram.sections)
        {
            count += section.duration;
            if(count > (int)Time.timeSinceLevelLoad)
            {
                break;
            }
            targetSection++;
        }
        target.text = "Target: " + trainingProgram.sections[targetSection].bpm;

        timer.text = ((int)Time.timeSinceLevelLoad) + "s";
        distance.text = stepCounter.distanceWalked + "m";

        if (stepCounter.stepsMinute > trainingProgram.sections[targetSection].bpm)
        {
            spm.color = Color.yellow;
        }
        else
        {
            spm.color = Color.green;
        }
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
