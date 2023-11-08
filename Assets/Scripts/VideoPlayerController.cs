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

        //Players 배열의 크기를 빈 오브젝트의 자식 개수로 조정합니다.
        Players = new GameObject[parentTransform.childCount];

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            //Players 배열의 해당 인덱스에 자식 오브젝트를 할당합니다.
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

            if (playing_time >= 10.0f) //30초보다 길거나 짧은 영상 존재, 5초에서 10초 정도 텀 필요할 수도 있음.
            {
                if (VideoNumber < 14)
                {
                    VideoNumber++; //0 -  14 -> 비디오 순번으로 설문 문항 실행 -> 비디오 수 변경되면 수정 필요
                    Debug.Log(VideoNumber);
                }
                if (VideoNumber % 2 == 0) //두 개 영상 시청후, 설문조사 실행 -> 비디오 수 변경되면 수정 필요
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
                if (SliderController.FinalEnd == false && SliderController.SurveyFinish == false) //설문 종료 후, 다음 영상 재생
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
                Survey[(VideoNumber / 2) - 1].SetActive(true); //비디오 수 변경되면 수정 필요
            }
        }
        if (SliderController.SurveyFinish == true) //설문 종료 후, 이전 영상 끄고 PlayAccept 변환
        {
            Survey[(VideoNumber / 2) - 1].SetActive(false); //비디오 수 변경되면 수정 필요
            SliderController.SaveTrigger = false;
            StartCoroutine(DelayTime(0.5f));
            PlayAccept = true;
        }
    }
    public static IEnumerator DelayTime(float delay) //delay시간만큼 나중에 실행
    {
        yield return new WaitForSeconds(delay);
        SliderController.SurveyFinish = false;
    }
    public static List<GameObject> GetChildrenList(GameObject parent) //Children 정보 받아오기
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }

        return childrenList;
    }
    void GetVideoSource(GameObject Video) //영상 해상도에 따라 Material과 RenderTexture 설정
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
