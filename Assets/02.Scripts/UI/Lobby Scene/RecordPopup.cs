using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using UnityEditor.U2D.Aseprite;
using System.Linq;

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
        LoadAllReplayJson();
    }
    private void LoadAllReplayJson()
    {
        string folderPath = Application.dataPath + "/Replay";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Replay 폴더가 없습니다! 경로: " + folderPath);
        }

        string[] files = Directory.GetFiles(folderPath, "*.json");

        if (files.Length == 0)
        {
            Debug.Log("저장된 리플레이 파일이 없습니다.");
        }

        GetSortedReplayDataWithOut(files, out List<ReplaySaveData> sortedList);

        foreach (ReplaySaveData data in sortedList)
        {
            CreateRecordContent(data);
        }
    }

    private void GetSortedReplayDataWithOut(string[] files, out List<ReplaySaveData> resultList)
    {
        List<ReplaySaveData> tempList = new List<ReplaySaveData>();

        foreach (string filePath in files)
        {
            string json = File.ReadAllText(filePath);
            ReplaySaveData data = JsonUtility.FromJson<ReplaySaveData>(json);

            tempList.Add(data);
        }

        resultList = tempList.OrderBy(data => data.replayName).ToList();
    }

    private void CreateRecordContent(ReplaySaveData loadData)
    {
        GameObject obj = Instantiate(this.recordPrefab, this.tsParent);
        RecordContent content = obj.GetComponent<RecordContent>();
        this.listRecordContent.Add(content);
        content.SetFrameData(loadData);
        content.Init(RecordScene);
    }
    private void RecordScene(ReplaySaveData data)
    {
        MySceneManager.Instance.LoadSceneWithCallback<ReplayBoard>("Record", (replayBoard) =>
        {
            List<ReplayFrameData> frame = data.listReplayFrameData;
            replayBoard.onLoadReplayData(frame);
        });
    }
}
