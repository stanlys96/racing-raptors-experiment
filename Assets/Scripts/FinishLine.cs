using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject leaderboard;
    private int counter = 0;
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Raptor" || collision.tag == "MainPlayer")
        {
            counter++;
            print(counter);
        }
        if (counter == 6)
        {
            yield return new WaitForSeconds(2f);
            leaderboard.SetActive(true);
            yield return new WaitForSeconds(100f);
            yield return SceneManager.LoadSceneAsync(0);
            counter = 0;
        }
    }
}
