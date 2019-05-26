using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
NoteController Ver:v0.0.1_2019052601a
Powered by Fung Kwok Wai "EXGARY"

    Release Note
    v0.0.1_2019052601a
*/

public class NoteController : MonoBehaviour
{

    MapData mapData;
    //TimeControl
    public bool play;
    float playTime = -4;//4sec perpare time
    /*
    [Space(10)]
    //BPMContol
    int currentMetreCount=0;
    float currentMetre = 0;
    int BPMMetrePointer = 0;
    */
    [Space(10)]
    //NoteSpawn
    public float hiSpeed = 2;
    int spawnPointer;
    int[] notePointer;//index = button position,tapPointer[pos]
    GameObject[,] notes;//notes[pos,tapPointer]
    float preSpawnTime = 10;//出現時機delta=判定時間-出現時間

    [Space(10)]

    //Button
    public GameObject[] button;
    bool[] judgementAvailable;
    int[] judgementPointer;
    float[,] buttonJudgementTime;//buttonJudgementTime[pos,tapPointer]

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        mapData = GetComponent<MapData>();

        mapData.NoteSpawnTimingCal();
        
        play = true;
        
        
        Initialization();
        InputJudgementTime();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        TimeCount();
        NoteSpawn();
        //NoteMove();
        
    }

    void Initialization()
    {
        spawnPointer = 0;

        buttonJudgementTime= new float[button.Length,mapData.notes.Count];
        notes = new GameObject[button.Length, mapData.notes.Count];

        notePointer = new int[button.Length];
        judgementAvailable = new bool[button.Length];
        judgementPointer = new int[button.Length];
        for (int i=0;i< button.Length; i++)
        {
            notePointer[i] =0;
            judgementAvailable[i] = false;
            judgementPointer[i] = 0;
        }



    }

    //遊玩時間更新
    void TimeCount()
    {
        if (play)
            playTime+= Time.deltaTime;
    }

    void InputJudgementTime()
    {
        for (int i = 0; i < mapData.notes.Count; i++)
        {
            buttonJudgementTime[mapData.notes[i].notePos, notePointer[mapData.notes[spawnPointer].notePos]] = mapData.notes[i].noteJudgmentTime;
            notePointer[mapData.notes[spawnPointer].notePos]++;
            spawnPointer++;
        }
        spawnPointer = 0;
        for (int i = 0; i < button.Length; i++)
        {
            notePointer[i] = 0;
        }
        //Debug.Log(buttonJudgementTime[0,1]);
    }

    void NoteSpawn()
    {
        //Debug.Log(mapData.notes[spawnPointer].noteJudgmentTime - preSpawnTime);
        if (spawnPointer < mapData.notes.Count)
        {


            if (mapData.notes[spawnPointer].noteJudgmentTime - preSpawnTime<=playTime)
            {
                Debug.Log("note spawned" + spawnPointer);
                notes[mapData.notes[spawnPointer].notePos, notePointer[mapData.notes[spawnPointer].notePos]] = ObjectPooler.SharedInstance.GetPooledObject("TapNote");
                notes[mapData.notes[spawnPointer].notePos, notePointer[mapData.notes[spawnPointer].notePos]].SetActive(true);
                notePointer[mapData.notes[spawnPointer].notePos]++;
                spawnPointer++;

                
            }
        }
    }

    void NoteMove()
    {

        for(int i=0; i < button.Length; i++)//i is pos pointer
        {
            if (judgementPointer[i]<notePointer[i])
            {
                for (int k= judgementPointer[i];k<= notePointer[i];k++)//k is 出現時機delta=判定時間-遊玩時間pointer
                {
                    switch (i)
                    {
                        case 1:
                            notes[i, judgementPointer[i] + k].transform.position = button[i].transform.position + new Vector3(0, (buttonJudgementTime[i, judgementPointer[i]+k]-playTime)*hiSpeed, 0);
                            break;
                        case 2:
                            notes[i, judgementPointer[i] + k].transform.position = button[i].transform.position + new Vector3((buttonJudgementTime[i, judgementPointer[i] + k] - playTime) * hiSpeed, 0, 0);
                            break;
                        case 3:
                            notes[i, judgementPointer[i] + k].transform.position = button[i].transform.position + new Vector3(0, -(buttonJudgementTime[i, judgementPointer[i] + k] - playTime) * hiSpeed, 0);
                            break;
                        case 4:
                            notes[i, judgementPointer[i] + k].transform.position = button[i].transform.position + new Vector3(-(buttonJudgementTime[i, judgementPointer[i] + k] - playTime) * hiSpeed, 0, 0);
                            break;

                    }
                }
            }
        }
    }

}
