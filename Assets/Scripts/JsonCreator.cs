using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace ProjectYouMustClickYes
{
    public class JsonCreator : MonoBehaviour
    {
        void Start()
        {
            CreateJsonFile();
        }

        void CreateJsonFile()
        {
            DialogueData data = new DialogueData
            {
                dialogues = new List<string>
                {
                    "게임을 시작하려면 '예' 버튼을 누르세요.",
                    "좋아요! 다음 버튼을 눌러 계속 진행하세요.",
                    "훌륭해요! 몇 번만 더 누르면 됩니다."
                }
            };

            string json = JsonUtility.ToJson(data, true); // JSON을 문자열로 변환 (true = 보기 좋게 포맷)

            string path = Path.Combine(Application.streamingAssetsPath, "dialogues.json");

            File.WriteAllText(path, json); // 파일 저장

            Debug.Log($"JSON 파일이 생성됨: {path}");
        }
    }
}