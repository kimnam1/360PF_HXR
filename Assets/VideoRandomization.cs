using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class VideoRandomization : MonoBehaviour
{
    [SerializeField] private VideoClip v1 = null;
    [SerializeField] private VideoClip v2 = null;
    [SerializeField] private VideoClip v3 = null;
    [SerializeField] private VideoClip v4 = null;
    [SerializeField] private VideoClip v5 = null;
    [SerializeField] private VideoClip v6 = null;
    [SerializeField] private VideoClip v7 = null;
    [SerializeField] private VideoClip v8 = null;
    [SerializeField] private VideoClip v9 = null;
    [SerializeField] private VideoClip v10 = null;
    [SerializeField] private VideoClip v11 = null;
    [SerializeField] private VideoClip v12 = null;
    [SerializeField] private VideoClip v13 = null;
    [SerializeField] private VideoClip v14 = null;
    [SerializeField] private VideoClip v15 = null;
    [SerializeField] private VideoClip v16 = null;
    [SerializeField] private VideoClip v17 = null;
    [SerializeField] private VideoClip v18 = null;
    [SerializeField] private VideoClip v19 = null;
    [SerializeField] private VideoClip v20 = null;
    [SerializeField] private VideoClip v21 = null;
    [SerializeField] private VideoClip v22 = null;
    [SerializeField] private VideoClip v23 = null;
    [SerializeField] private VideoClip v24 = null;

    // 2차원 배열
    private VideoClip[,] VideoArray = new VideoClip[4, 6];
    public VideoClip[,] ReorderedClips = null;
    public VideoClip[] FinalClips = null;

    // 영상 4개 * 총 7가지 경우의 수 * 영상 2개 묶음 = 56개
    public static GameObject[] videos = new GameObject[56];
    public GameObject VideoPlayer;

    public List<List<string>> DataList = new List<List<string>>();
    public static List<List<int>> Combination = new List<List<int>>();
    private List<int> OneList = new List<int>();
    private List<List<int>> CombinationList = new List<List<int>>();

    private string csvDirectoryName = "Survey";
    public string csvFileName;
    public static string SceneFullName = null;

    // Start is called before the first frame update
    private void Awake()
    {
        VideoArray[0, 0] = v1;
        VideoArray[0, 1] = v2;
        VideoArray[0, 2] = v3;
        VideoArray[0, 3] = v4;
        VideoArray[0, 4] = v5;
        VideoArray[0, 5] = v6;
        VideoArray[1, 0] = v7;
        VideoArray[1, 1] = v8;
        VideoArray[1, 2] = v9;
        VideoArray[1, 3] = v10;
        VideoArray[1, 4] = v11;
        VideoArray[1, 5] = v12;
        VideoArray[2, 0] = v13;
        VideoArray[2, 1] = v14;
        VideoArray[2, 2] = v15;
        VideoArray[2, 3] = v16;
        VideoArray[2, 4] = v17;
        VideoArray[2, 5] = v18;
        VideoArray[3, 0] = v19;
        VideoArray[3, 1] = v20;
        VideoArray[3, 2] = v21;
        VideoArray[3, 3] = v22;
        VideoArray[3, 4] = v23;
        VideoArray[3, 5] = v24;

        for (int i = 0; i < 4; i++) // 영상 4개 4번 랜덤화
        {
            // 파일 경로 설정 (Resources 폴더 내의 파일은 상대 경로로 접근 가능) Resources 파일은 Unity 3D Assets 내에 존재함.
            string filePath = "DataCombination";

            // CSV 파일을 읽어서 List<List<string>>에 저장하는 메서드 호출
            List<List<string>> DataList = loadCSV(filePath);

            // string List 변수 int List 변수로 변환 & int List 조합 생성
            List<List<int>> Combination = SelectedValues(StringToInt(DataList));

            // List<int>로 변환
            OneList = FlattenList(Combination);

            // List<List<Int>>로 합치기
            CombinationList.Add(OneList);
        }

        // CombinationLIst 정보 저장 CSV
        SceneFullName = SceneManager.GetActiveScene().name;
        csvFileName = "Result" + SceneFullName + ".csv";
        CreateCSV();

        // 받아온 List<List<int>>로 영상 순서 배열
        ReorderedClips = ReorderedArray(CombinationList, VideoArray);

        // VideoClip[]으로 변경
        FinalClips = ReorderedClips.Cast<VideoClip>().ToArray();

        // VideoClip[] Shuffle
        FinalClips = ShuffleVideoClips(FinalClips); // to do

        List<GameObject> children = GetChildrenList(VideoPlayer);

        // 무작위로 재배열된 영상 순서에 알맞게 video 내 영상 재배열
        for (int i = 0; i < FinalClips.Length; i++)
        {
            videos[i] = children[i];
        }
        for (int video_num = 0; video_num < videos.Length; video_num++)
        {
            videos[video_num].GetComponent<VideoPlayer>().clip = FinalClips[video_num];
        }
    }

    public static List<GameObject> GetChildrenList(GameObject parent)
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }

        return childrenList;
    }

    public static List<List<int>> StringToInt(List<List<string>> Strings) // String to Int 
    {
        List<List<int>> Result = new List<List<int>>();
        foreach (List<string> strList in Strings)
        {
            List<int> intList = new List<int>();

            foreach (string str in strList)
            {
                int num = int.Parse(str);
                intList.Add(num);
            }
            Result.Add(intList);
        }
        return Result;
    }

    public static List<List<int>> SelectedValues(List<List<int>> Integers) // 조합 내 배열 무작위
    {
        ShuffleList(Integers);
        List<List<int>> OrderList = new List<List<int>>();
        System.Random random = new System.Random();

        foreach (var listInt in Integers)
        {
            // 0 or 1 random
            int randomIdx = random.Next(2);

            // 선택된 index에 따라서 값 선택
            int selectedValue1 = listInt[randomIdx * 2];
            int selectedValue2 = listInt[randomIdx * 2 + 1];

            List<int> selectedValues = new List<int> { selectedValue1, selectedValue2 };

            OrderList.Add(selectedValues);
        }

        return OrderList;
    }

    public static void ShuffleList(List<List<int>> lst) // 영상 조합 무작위
    {
        int n = lst.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + UnityEngine.Random.Range(0, n - i);
            List<int> temp = lst[r];
            lst[r] = lst[i];
            lst[i] = temp;
        }
    }

    public static List<int> FlattenList(List<List<int>> lsts) // 영상 조합 하나의 리스토
    {
        List<int> result = new List<int>();

        foreach (var sublist in lsts)
        {
            result.AddRange(sublist);
        }

        return result;
    }

    public static VideoClip[,] ReorderedArray(List<List<int>> orderList, VideoClip[,] videoClips) // List<List<int>> orderList 내에 있는 순서로 VideoClip[,] vidoeClips 내 비디오 순서 재배열
    {
        VideoClip[,] result = new VideoClip[4, 14];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                int newIndex = orderList[i][j];
                result[i, j] = videoClips[i, newIndex];
            }
        }
        return result;
    }

    public VideoClip[] ShuffleVideoClips(VideoClip[] videoClips) // VideoClip[] videoClips 내 1차원 비열 비디오 2개 씩 묶어서 순서 재배열(랜덤)
    {
        List<VideoClip[]> paired = new List<VideoClip[]>();

        for(int i = 0; i < videoClips.Length; i += 2)
        {
            if (i+1 < videoClips.Length)
            {
                VideoClip[] pair = new VideoClip[2];
                pair[0] = videoClips[i];
                pair[1] = videoClips[i + 1];
                paired.Add(pair);
            }
        }

        // paired 랜덤하게 섞기
        System.Random rng = new System.Random();
        paired = paired.OrderBy(pair => rng.Next()).ToList(); // 이걸 잘 모르겠네... 랜덤을 왜 이렇게 하지 -남일

        // 재배열
        List<VideoClip> shuffled = new List<VideoClip>();

        foreach(var pair in paired)
        {
            shuffled.Add(pair[0]);
            shuffled.Add(pair[1]);
        }

        // 배열로 변환해서 리턴
        return shuffled.ToArray();
    }

    public static List<List<string>> loadCSV(string fileName)
    {
        // CSV 파일을 한 줄씩 읽기위해서 TextAsset 사용???
        TextAsset CSVFile = Resources.Load<TextAsset>(fileName);
        StringReader reader = new StringReader(CSVFile.text);

        List<List<string>> lsts = new List<List<string>>();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            // CSV 파일의 각 라인을 ','으로 분리하여 문자열 배열로 저장
            string[] values = line.Split(',');

            List<string> lineList = new List<string>(values);

            // List<List<string>>에 각 줄의 List<string> 추가
            lsts.Add(lineList);
        }

        // 리소스 해제
        Resources.UnloadAsset(CSVFile);

        return lsts;
    }

    string GetDirectoryPath()
    {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + csvFileName;
    }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    void VerifyFile()
    {
        string file = GetFilePath();

        if (!File.Exists(file))
        {
            Debug.Log("Fail to make file");
        }
    }

    public void CreateCSV()
    {
        VerifyDirectory();
        VerifyFile();

        using(StreamWriter sw = File.CreateText(GetFilePath()))
        {
            for (int i = 0; i < CombinationList.Count; i++)
            {
                List<int> sampleList = CombinationList[i];

                foreach (int numb in sampleList)
                {
                    sw.Write(numb);
                    sw.Write(",");
                }

                sw.Write("\r\n");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
