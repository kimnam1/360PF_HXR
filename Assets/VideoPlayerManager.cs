using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NewBehaviourScript : MonoBehaviour
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
    private int VidNumb = 0;
    public GameObject[] Survey;
    private uint VidWidth = 0;
    private uint VidHeight = 0;
    public static bool PlayAccept = 0;

    private void OnEnable()
    {
        // 해당 스크립트가 들어간 오브젝트의 Transform 정보를 parentTransform에 할당
        Transform parentTransform = transform;

        // Player 배열 크기를 빈 오브젝트의 자식 개수로 조정.
        Players new GameObject[parentTransform.childCount];

        // 빈 오브젝트의 개수만큼 반복
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            // Players 배열의 해당 인덱스에 자식 오브젝트를 할당.
            Players[i] = childGameObject;
        }

        // Players의 VideoNumb에 해당하는 리스트 정보는 player에 할당
        player = Players[VidNumb];

        // GetVidSource 함수 확인
        GetVidSource(player);

        if (skyboxMaterial == true && RenderTexture == true)
        {
            RenderSetting.skybox = skyboxMaterial;
            player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;

            player.GetComponent<VideoPlayer>().playOnAwake = false;
            player.GetCOmponenet<VideoPlayer>().Play();
            PlayAccept = true;
        }
    }

    private void FixedUpdate()
    {
        Transform parentTransform = transform;

        //player가 재생될 때
        if (player.GetComponent<VideoPlayer>().isPlaying == true)
        {
            playing_time += Time.deltaTime;

            if (playing_time >= 10.0f) // 30초보다 길거나 짧은 영상이 있음. 5초에서 10초 정도 텀이 필요할 수도 있음.
            {
                if (VidNumb < 70)
                {
                    VidNumb++; // 0~70 -> 비디오 순번으로 설문 문항 실행. -> 비디오 수 변경되면 수정필요.
                    Debug.Log(VidNumb);
                }

                if (VidNumb % 2 == 0) // 두 개 영상 시청 후, 설문조사 진행 -> 비디오 수 변경되면 수정 필요.
                {
                    PlayAccept = false;
                    // Debug.Log(PlayAccept);
                }

                player.GetComponent<VideoPlayer>().Pause();
                player.SetActive(false);
                NewSliderController.SurveyFinish = false; // 이름 변경 필요.
            }
        }

        // player의 재생이 종료되면 실행
        if (player.GetComponent<VideoPlayer>().isPlaying == false)
        {
            if (PlayAccept == true)
            {
                NewSliderController.SurveyFinish = false;
                if (NewSliderController.FinalEnd == false && NewSliderConroller.SurveyFinishi == false)
                {
                    player = Players[VidNumb];
                    GetVidSource(player);
                    RenderSettings.skybox = skyboxMaterial;
                    player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;
                    player.SetActive(true);
                    player.GetComponent<VideoPlayer>().playOnAwake = false;
                    playing_time = 0.0f
                    player.GetComponent<VideoPlayer>().Play();
                }
            }
            else if (PlayAccept == false && VidNumb % 2 == 0)
            {
                Survey[(VidNumb / 2) - 1].SetActive(true); // 비디오 수 변경되면 수정 필요.
            }
        }
        // 설문 종료 후 이전 영상 끄고 PlayAccep 변환
        if (NewSliderController.SurveyFinish == true)
        {
            Survey[(VidNumb / 2) - 1].SetActive(false); // 비디오 수 변경되면 수정 필요
            NewSliderController.SaveTrigger = false;
            StartCoroutine(DelayTime(0.5f));
            PlayAccept = true;
        }

    }

    public static IEnumerator DelayTime(float delay) // delay 시간만큼 나중에 실행
    {
        yield return new WaitForSeconds(delay);
        NewSliderController.SurveyFinish = false;
    }

    public static List<GameObject> GetChildrenList(GameObject parent) // Children 정보 받아오기
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }
    }

    void GetVidSource(GameObject Vid) // 영상 해상도에 따라서 Material과 RenderTexture 설정
    {
        VideoPlayer videoPlayer = Vid.GetComponent<VideoPlayer>();

        if (videoPlayer != null && videoPlayer.clip != null)
        {
            VidWidth = videoPlayer.width;
            VidHeight = videoPlayer.height;

            if (VidWidth == 2560 && VidHeight == 1440)
            {
                skyboxMaterial = skyboxMaterial_2K;
                RenderTexture = RenderTexture_2K;
            }
            if (VidWidth == 3840 && VidHeight == 2160)
            {
                skyboxMaterial = skyboxMaterial_4K;
                RenderTexture = RenderTexture_2K;
            }
            if (VidWidth == 7680 && VidHeight == 4320)
            {
                skyboxMaterial = skyboxMaterial_8K;
                RenderTexture = RenderTexture_8K;
            }

        }
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
