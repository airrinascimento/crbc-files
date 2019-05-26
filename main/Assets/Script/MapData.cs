using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MapData Ver:v0.0.1_2019052601a
Powered by Fung Kwok Wai "EXGARY"

    Release Note
    v0.0.1_2019052601a
*/


[System.Serializable]
public class BPMMetreDataItem//set music related data
{
    public float BPM;
    public float division;//base on quarter note四分音符, for example, 4/4->division=4,  7/8->division=3.5
    public int bar;//BPM及節拍轉換時機,以小節計算
}

[System.Serializable]
public class NotesItem//set map data
{
    public int bar;//Note所在小節
    public float beat;//小節內第幾拍, base on quarter note四分音符
    public int notePos;//出現位置1~8
    public float holdLength;//Hold長度, 以四分音符計算, Tap為0
    public float noteJudgmentTime=0;//判定時間
}

public class MapData : MonoBehaviour
{
    //Score related value
    //分數=scoreIndex/(totalTapIndex+totalHoldIndex)*滿分   (向下取整)
    /*
        分數表
        perfect Tap=    +3 scoreIndex
        good Tap=       +2 scoreIndex
        perfect Hold=   +1 scoreIndex per 八分音符
    */
    //int totalTapIndex=0;
    //int totalHoldIndex;

    //Get BPM and Metre
    public List<BPMMetreDataItem> BPMMetreData;

    //Get Map Data
    public List<NotesItem> notes;





    //更新時間
    public void NoteSpawnTimingCal()
    {
        Debug.Log("Num of Notes ="+notes.Count);

        for (int i=0; i<notes.Count; ++i)//每個notes        
        {
            //Debug.Log();
            if (BPMMetreData.Count != 1)//如果BPM有變速
            {
                for(int j = 0; j < BPMMetreData.Count; ++j)//每個BPM轉換時機
                {
                    //變速時間+補充時間=實際判定時間
                    if (notes[i].bar < BPMMetreData[j].bar)
                    {
                        notes[i].noteJudgmentTime += (BPMMetreData[j].BPM / 60 * BPMMetreData[j].division * (BPMMetreData[j + 1].bar - BPMMetreData[j].bar));//
                    }
                    else//
                    {
                        notes[i].noteJudgmentTime += (BPMMetreData[j].BPM / 60 * ((notes[i].bar - BPMMetreData[j].bar)* BPMMetreData[j].division + notes[i].beat));//
                        break;//
                    }
                }
            }
            else//無BPM變速
            {
                notes[i].noteJudgmentTime += (BPMMetreData[0].BPM / 60 * ((notes[i].bar-1) * BPMMetreData[0].division + notes[i].beat));//
            }
        }
    }

    //public static void NoteSpawn(float timer,float hiSpeed)
    //{

    //}

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
}
