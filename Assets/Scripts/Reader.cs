using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TalkData // 대사 구조 선언
{
    public string CharacterName; // 대사를 하는 캐릭터 이름
    public string Dialogue; // 대사 내용
}

public class Reader
{
    public static TalkData[] Read(string filename) // 대사 파일을 대사 구조 배열로 반환하는 정적 함수
    {
        string path = Application.streamingAssetsPath + "/" + filename;

        List<TalkData> list = new List<TalkData>(); // 반환할 리스트 파일 생성

        string[] lines = File.ReadAllLines(path); // 줄마다 나눠서 배열로 저장

        for (int i = 0; i < lines.Length; i++) // 줄마다 반복
        {
            string[] splits = lines[i].Split('|'); // '|' 문자로 나눠서 배열로 저장

            TalkData talk = new TalkData(); // 대사 구조 파일 생성
            talk.CharacterName = splits[0].Trim(); // n번째 줄 첫 번째 문자열 -> 캐릭터 이름
            talk.Dialogue = splits[1].Trim(); // n번째 줄 두 번째 문자열 -> 대사 내용

            list.Add(talk); // 대사 리스트에 대사 구조 파일을 복사
            // 배열이 아닌 리스트를 사용하는 하나씩 추가하면서 크기가 계속 변하기 때문
        }

        return list.ToArray(); // 리스트를 배열로 변환하여 반환
        // 배열이 index 번호로 접근하기 유용하고 메모리를 적게 차지하기 때문
    }
}
