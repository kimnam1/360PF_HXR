using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Material skyboxMaterial_2K;
    [SerializeField] private Material skyboxMaterial_4K;
    [SerializeField] private Material skyboxMaterial_8K;
    [SerializeField] private RenderTexture RenderTexture;
    [SerializeField] private RenderTexture RenderTexture_2K;
    [SerializeField] private RenderTexture RenderTexture_4K;
    [SerializeField] private RenderTexture RenderTexture_8K;

    [SerializeField] private GameObject[] Players;
    private GameObject player;
    private float playing_time = 0.0f;
    private int VideoNumber = 0;
    public GameObject[] Survey;
    private uint VideoWidth = 0;
    private uint VideoHeight = 0;
    public static bool PlayAccept = false;

    private void OnEnable()
    {

        Transform parentTransform = transform;

        //Players �迭�� ũ�⸦ �� ������Ʈ�� �ڽ� ������ �����մϴ�.
        Players = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            //Players �迭�� �ش� �ε����� �ڽ� ������Ʈ�� �Ҵ��մϴ�.
            Players[i] = childGameObject;
        }
        player = Players[VideoNumber];
        GetVideoSource(player);
        if (skyboxMaterial == true && RenderTexture == true)
        {
            RenderSettings.skybox = skyboxMaterial;
            player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;
            //player.GetComponent<VideoPlayer>().playOnAwake = false;
            player.GetComponent<VideoPlayer>().Play();
            PlayAccept = true;
        }
    }
    private void FixedUpdate()
    {
        Transform parentTransform = transform;
        if (player.GetComponent<VideoPlayer>().isPlaying == true)
        {
            playing_time += Time.deltaTime;

            if (playing_time >= 10.0f) //30�ʺ��� ��ų� ª�� ���� ����, 5�ʿ��� 10�� ���� �� �ʿ��� ���� ����.
            {
                if (VideoNumber < 14)
                {
                    VideoNumber++; //0 -  14 -> ���� �������� ���� ���� ���� -> ���� �� ����Ǹ� ���� �ʿ�
                    Debug.Log(VideoNumber);
                }
                if (VideoNumber % 2 == 0) //�� �� ���� ��û��, �������� ���� -> ���� �� ����Ǹ� ���� �ʿ�
                {
                    PlayAccept = false;
                    Debug.Log(PlayAccept);
                }
                player.GetComponent<VideoPlayer>().Pause();
                player.SetActive(false);
                SliderController.SurveyFinish = false;
            }
        }
        if (player.GetComponent<VideoPlayer>().isPlaying == false)
        {
            if (PlayAccept == true)
            {
                SliderController.SurveyFinish = false;
                if (SliderController.FinalEnd == false && SliderController.SurveyFinish == false) //���� ���� ��, ���� ���� ���
                {
                    player = Players[VideoNumber];
                    GetVideoSource(player);
                    RenderSettings.skybox = skyboxMaterial;
                    player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;
                    player.SetActive(true);
                    player.GetComponent<VideoPlayer>().playOnAwake = false;
                    playing_time = 0.0f;
                    player.GetComponent<VideoPlayer>().Play();
                }
            }
            else if (PlayAccept == false && VideoNumber % 2 == 0)
            {
                Survey[(VideoNumber / 2) - 1].SetActive(true); //���� �� ����Ǹ� ���� �ʿ�
            }
        }
        if (SliderController.SurveyFinish == true) //���� ���� ��, ���� ���� ���� PlayAccept ��ȯ
        {
            Survey[(VideoNumber / 2) - 1].SetActive(false); //���� �� ����Ǹ� ���� �ʿ�
            SliderController.SaveTrigger = false;
            StartCoroutine(DelayTime(0.5f));
            PlayAccept = true;
        }
    }
    public static IEnumerator DelayTime(float delay) //delay�ð���ŭ ���߿� ����
    {
        yield return new WaitForSeconds(delay);
        SliderController.SurveyFinish = false;
    }
    public static List<GameObject> GetChildrenList(GameObject parent) //Children ���� �޾ƿ���
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }

        return childrenList;
    }
    void GetVideoSource(GameObject Video) //���� �ػ󵵿� ���� Material�� RenderTexture ����
    {
        VideoPlayer videoplayer = Video.GetComponent<VideoPlayer>();

        if (videoplayer != null && videoplayer.clip != null)
        {
            VideoWidth = videoplayer.width;
            VideoHeight = videoplayer.height;
            if (VideoWidth == 2560 && VideoHeight == 1440)
            {
                skyboxMaterial = skyboxMaterial_2K;
                RenderTexture = RenderTexture_2K;
            }
            if (VideoWidth == 3840 && VideoHeight == 2160)
            {
                skyboxMaterial = skyboxMaterial_4K;
                RenderTexture = RenderTexture_4K;
            }
            if (VideoWidth == 7680 && VideoHeight == 3840)
            {
                skyboxMaterial = skyboxMaterial_8K;
                RenderTexture = RenderTexture_8K;
            }
        }
    }
}