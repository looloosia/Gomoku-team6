using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class RecordPopup : BasePopup
{
    [SerializeField]
    private Transform tsParent;
    [SerializeField]
    private GameObject recordPrefab;

    private List<RecordContent> listRecordContent = new List<RecordContent>();

    protected override void Start()
    {
        // 닫기 버튼이 연결 되어있다면 자동으로 Hide() 메소드 연결
        if (closeBtn != null)
            closeBtn.onClick.AddListener(() => Hide());

        Init();
    }
    protected override void Init()
    {
        CreateRecordContent();
    }
    private ReplaySaveData LoadReplayJson()
    {
        string folderPath = Application.dataPath + "/Replay";
        string filePath = folderPath + "/ReplayData_Test.json"; // 예시 파일명

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("저장된 리플레이 파일이 없습니다! 경로를 확인해주세요: " + filePath);
            return default;
        }

        string json = File.ReadAllText(filePath);
        ReplaySaveData loadData = JsonUtility.FromJson<ReplaySaveData>(json);
        List<ReplayFrameData> data = loadData.listReplayFrameData;

        Debug.Log("Json 로드 완료");
        return loadData;
    }
    private void Record()
    {
        ReplaySaveData loadData = LoadReplayJson();
        RecordContent content = CreateRecordContent();
        
    }
    private RecordContent CreateRecordContent()
    {
        GameObject obj = Instantiate(this.recordPrefab, this.tsParent);
        RecordContent content = obj.GetComponent<RecordContent>();
        this.listRecordContent.Add(content);
        content.Init(RecordScene);

        return content;
    }
    private void RecordScene()
    {
        
    }
}
