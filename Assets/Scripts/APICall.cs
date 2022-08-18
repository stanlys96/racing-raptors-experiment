using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Runtime.InteropServices;

[System.Serializable]
public class Data
{
    public int id;
    public string sender;
    public string receiver;
    public string value;

    public static Data CreateFromJson(string jsonString) {
        return JsonUtility.FromJson<Data>(jsonString);
    }
}

[System.Serializable]
public class Data2
{
    public int[] queue;

    public static Data2 CreateFromJson(string jsonString) {
        return JsonUtility.FromJson<Data2>(jsonString);
    }
}

[System.Serializable]
public class DataFightWinner
{
    public int id;
    public int raptor_fight_winner;

    public static DataFightWinner CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataFightWinner>(jsonString);
    }
}

[System.Serializable]
public class DataFighters
{
    public int id;
    public int[] raptor_fighters;

    public static DataFighters CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataFighters>(jsonString);
    }
}

[System.Serializable]
public class DataTop3
{
    public int id;
    public int[] raptor_top_3;

    public static DataTop3 CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataTop3>(jsonString);
    }
}

[System.Serializable]
public class DataRaceWinner
{
    public int id;
    public int qp_winner;

    public static DataRaceWinner CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataRaceWinner>(jsonString);
    }
}

[System.Serializable]
public class RaptorJsonData
{
    [JsonProperty("Token ID")]
    public string TokenID;
    public string description;
    public string image;
    public string external_url;
    public string name;
    public List<Attribute> attributes;

    public static RaptorJsonData CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<RaptorJsonData>(jsonString);
    }
}

[System.Serializable]
public class Attribute
{
    public string trait_type;
    public string value;
}

public static class ArrayExt
{
    public static T[] GetRow<T>(this T[,] array, int row)
    {
        if (!typeof(T).IsPrimitive)
            throw new InvalidOperationException("Not supported for managed types.");

        if (array == null)
            throw new ArgumentNullException("array");

        int cols = array.GetUpperBound(1) + 1;
        T[] result = new T[cols];

        int size;

        if (typeof(T) == typeof(bool))
            size = 1;
        else if (typeof(T) == typeof(char))
            size = 2;
        else
            size = Marshal.SizeOf<T>();

        Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

        return result;
    }
}


public class APICall : MonoBehaviour
{
    public static APICall instance;
    InputField outputArea;
    Data data;
    int[] result = new int[8];
    [SerializeField] GameObject raptor0 = null;
    [SerializeField] GameObject raptor1 = null;
    [SerializeField] GameObject raptor2 = null;
    [SerializeField] GameObject raptor3 = null;
    [SerializeField] GameObject raptor4 = null;
    [SerializeField] GameObject raptor5 = null;
    [SerializeField] GameObject raptor6 = null;
    [SerializeField] GameObject raptor7 = null;
    [SerializeField] GameObject raptor8 = null;
    [SerializeField] GameObject raptor9 = null;
    [SerializeField] RuntimeAnimatorController runtimeControllerRaptor0 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite1 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite2 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite3 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite4 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite5 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite6 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite7 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite8 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite9 = null;
    GameObject spawnPoints = null;
    public GameObject thisSceneCanvas;
    public int[] raptorsInPlay = new int[8];
    public int injuredRaptor;
    public int fightWinner = 34;
    public int[] fighters = new int[2];
    public int[] top3 = new int[3];
    public int[] theRestRacer = new int[5];
    public int quickPlayWinner;
    public int competitiveWinner;
    public int deathRaceWinner;
    public int ripRaptor;
    public bool hasStarted = false;
    public int[] currentQueue = new int[8];
    private GameObject[] currentGameObjectRaptors = new GameObject[8];
    public Dictionary<int, RuntimeAnimatorController> tokenIdToSprite = new Dictionary<int, RuntimeAnimatorController>();
    public int[] mockTokenId = { 10, 2, 3, 8, 5, 6, 25, 34 };
    public string[] mockBodyType = { "Scaled", "Jigsaw", "Rhodnite", "Bolt", "Stripe", "Undead", "Marble", "Flame" };

    public double GetAverage(int[] arr)
    {
        double average = 0.0;
        for (int i = 0; i < arr.Length; i++)
        {
            average += arr[i];
        }
        return average / arr.Length;
    }

    public int GetWeight(double[] arr, int position, int index)
    {
        int below = 0;
        int above = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (i == index)
            {
                continue;
            }
            else if (arr[i] < arr[index])
            {
                below += 1;
            }
            else if (arr[i] > arr[index])
            {
                above += 1;
            }
        }

        if (position < arr.Length - below)
        {
            return 1; //faster speed
        }
        else
        {
            return -1; //slower speed
        }
    }

    public GameObject GetRaptorGameObject(string value)
    {
        switch (value)
        {
            case "Bolt":
                return raptor0;
            case "Scaled":
                return raptor1;
            case "Undead":
                return raptor2;
            case "Flame":
                return raptor3;
            case "Marble":
                return raptor2;
            case "Blue":
                return raptor5;
            case "Slimy":
                return raptor6;
            case "Jigsaw":
                return raptor7;
            case "Rhodnite":
                return raptor8;
            case "Stripe":
                return raptor9;
            default:
                return raptor0;
        }
    }

    public RuntimeAnimatorController GetRaptorSprite(string value)
    {
        switch (value)
        {
            case "Bolt":
                return runtimeControllerRaptor0;
            case "Scaled":
                return raptorSprite1;
            case "Undead":
                return raptorSprite2;
            case "Flame":
                return raptorSprite3;
            case "Marble":
                return raptorSprite2;
            case "Blue":
                return raptorSprite5;
            case "Slimy":
                return raptorSprite6;
            case "Jigsaw":
                return raptorSprite7;
            case "Rhodnite":
                return raptorSprite8;
            case "Stripe":
                return raptorSprite9;
            default:
                return runtimeControllerRaptor0;
        }
    }

    void Awake()
    {
        instance = this;
        spawnPoints = GameObject.FindWithTag("SpawnPoint");
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        System.Random random = new System.Random();
        return (float)(random.NextDouble() * ((double)maximum - (double)minimum) + (double)minimum);
    }

    float[] NextFloat(float randNumber)
    {
        float total = randNumber;
        System.Random rnd = new System.Random();
        float[] result = new float[6];
        for (int i = 0; i < 6; i++)
        {
            float minimum = total / (6 - i) * 0.6f;
            float maximum = total / (6 - i) * 1.3f;
            result[i] = GetRandomNumber(minimum, maximum);
            total -= result[i];
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        float[] res = NextFloat(4.8f);
        foreach (var item in res)
        {
            print(item);
        }
        DontDestroyOnLoad(gameObject);
    }

    void GetData()
    {
        StartCoroutine(GetData_Coroutine());
    }

    IEnumerator GetData_Coroutine()
    {
        while (!hasStarted)
        {
            currentQueue = mockTokenId;
            int index = 0;
            int count = 0;
            foreach (int item in currentQueue)
            {
                if (item == 0)
                {
                    if (spawnPoints.transform.GetChild(index).gameObject.transform.childCount > 0)
                    {
                        Destroy(spawnPoints.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject);
                    }
                }
                else
                {
                    count++;
                    currentGameObjectRaptors[index] = GetRaptorGameObject(mockBodyType[index]);
                    tokenIdToSprite[item] = GetRaptorSprite(mockBodyType[index]);
                    raptorsInPlay[index] = item;
                    index++;
                }
            }
            int spawnPointIndex = 0;
            foreach (GameObject raptorGameObject in currentGameObjectRaptors)
            {
                if (currentQueue[spawnPointIndex] != 0)
                {
                    if (spawnPoints != null)
                    {
                        if (raptorGameObject != null)
                        {
                            if (spawnPoints.transform.GetChild(spawnPointIndex).gameObject.transform.childCount > 0)
                            {
                                Destroy(spawnPoints.transform.GetChild(spawnPointIndex).gameObject.transform.GetChild(0).gameObject);
                            }

                            GameObject raptorObject = Instantiate(raptorGameObject, spawnPoints.transform.GetChild(spawnPointIndex).transform);
                            raptorObject.GetComponent<RaptorTokenIDIdle>().SetTokenIdText(mockTokenId[spawnPointIndex].ToString());
                            spawnPointIndex++;
                        }
                    }
                }
            }
            if (count == 8)
            {
                fighters[0] = mockTokenId[7];
                fighters[1] = mockTokenId[3];
                top3[0] = mockTokenId[0];
                top3[1] = mockTokenId[1];
                top3[2] = mockTokenId[2];
                int[] raptorsInPlayTemp = raptorsInPlay;
                int restRacerIndex = 0;
                foreach (int raptor in raptorsInPlayTemp)
                {
                    if (!top3.Contains(raptor))
                    {
                        theRestRacer[restRacerIndex] = raptor;
                        restRacerIndex++;
                    }
                }
                quickPlayWinner = top3[0];
                hasStarted = true;
                yield return new WaitForSeconds(5f);
                Fader fader = FindObjectOfType<Fader>();
                DontDestroyOnLoad(gameObject);
                yield return fader.FadeOut(1f);
                thisSceneCanvas.gameObject.SetActive(false);
                yield return SceneManager.LoadSceneAsync(1);
                yield return new WaitForSeconds(0.5f);
                yield return fader.FadeIn(1f);
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}