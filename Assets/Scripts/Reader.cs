using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TalkData // ��� ���� ����
{
    public string CharacterName; // ��縦 �ϴ� ĳ���� �̸�
    public string Dialogue; // ��� ����
}

public class Reader
{
    public static TalkData[] Read(string filename) // ��� ������ ��� ���� �迭�� ��ȯ�ϴ� ���� �Լ�
    {
        string path = Application.streamingAssetsPath + "/" + filename;

        List<TalkData> list = new List<TalkData>(); // ��ȯ�� ����Ʈ ���� ����

        string[] lines = File.ReadAllLines(path); // �ٸ��� ������ �迭�� ����

        for (int i = 0; i < lines.Length; i++) // �ٸ��� �ݺ�
        {
            string[] splits = lines[i].Split('|'); // '|' ���ڷ� ������ �迭�� ����

            TalkData talk = new TalkData(); // ��� ���� ���� ����
            talk.CharacterName = splits[0].Trim(); // n��° �� ù ��° ���ڿ� -> ĳ���� �̸�
            talk.Dialogue = splits[1].Trim(); // n��° �� �� ��° ���ڿ� -> ��� ����

            list.Add(talk); // ��� ����Ʈ�� ��� ���� ������ ����
            // �迭�� �ƴ� ����Ʈ�� ����ϴ� �ϳ��� �߰��ϸ鼭 ũ�Ⱑ ��� ���ϱ� ����
        }

        return list.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� ��ȯ
        // �迭�� index ��ȣ�� �����ϱ� �����ϰ� �޸𸮸� ���� �����ϱ� ����
    }
}
