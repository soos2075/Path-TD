using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableManager
{



    private TextAsset CSV_LOAD(string fileName)
    {
        TextAsset myData = Resources.Load<TextAsset>($"Data/{fileName}");
        if (myData == null)
        {
            Debug.Log($"{fileName}.csv 테이블 정보를 불러오지 못했습니다");
            return null;
        }
        return myData;
    }


    public class CSV_Data
    {
        public int Counter { get; }
        public string[] Line { get; }
        public string Memo { get; set; }

        public CSV_Data(int counter, string[] data, string memo = null)
        {
            Counter = counter;
            Line = data;
            Memo = memo;
        }
    }

    public CSV_Data CSV_LOAD_Stage (string fileName)
    {
        TextAsset data = CSV_LOAD($"Stage/{fileName}");

        string[] origin = data.text.Split('\n');
        string memo = origin[0];
        int wave = int.Parse(origin[0].Substring(data.text.IndexOf("Wave=") + 5, 2));


        //? 초기설정값 받아오기. 이후 들어오는 Income은 여기서 처리 안함
        string temp = data.text.Substring(data.text.IndexOf("Initialize"));
        string[] tempLine = temp.Split('\n');
        string[] init_data = tempLine[0].Split(',');

        GameManager.Instance.AddCoin(int.Parse(init_data[3]));
        GameManager.Instance.AddPlatform(int.Parse(init_data[4]));
        GameManager.Instance.AddLife(int.Parse(init_data[5]));


        string contents = data.text.Substring(data.text.IndexOf("Wave_01"));
        string[] line = contents.Split('\n');

        CSV_Data csv = new CSV_Data(wave, line, memo);
        return csv;
    }


    public CSV_Data CSV_LOAD_Tower_Status(string fileName)
    {
        TextAsset data = CSV_LOAD($"Tower/{fileName}");

        string[] origin = data.text.Split('\n');
        string memo = origin[0] + "\n" + origin[1];
        int count = int.Parse(origin[0].Substring(data.text.IndexOf("Tower=") + 6, 2));

        string contents = data.text.Substring(data.text.IndexOf("FireTower"));
        string[] line = contents.Split('\n');

        CSV_Data csv = new CSV_Data(count, line, memo);
        return csv;
    }

    public CSV_Data CSV_LOAD_Tower_Info(string fileName)
    {
        TextAsset data = CSV_LOAD($"Tower/{fileName}");

        string[] origin = data.text.Split('\n');
        string memo = origin[0] + "\n" + origin[1];
        int count = int.Parse(origin[0].Substring(data.text.IndexOf("Tower=") + 6, 2));

        string contents = data.text.Substring(data.text.IndexOf("FireTower"));
        string[] line = contents.Split('\n');

        CSV_Data csv = new CSV_Data(count, line, memo);
        return csv;
    }


}
