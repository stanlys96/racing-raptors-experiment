using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartInstantiate : MonoBehaviour
{
    public GameObject raptorUIToInstantiate;
    public GameObject canvas;

    private void Start()
    {
        StartCoroutine(InstantiateRaptorUI());
    }

    IEnumerator InstantiateRaptorUI()
    {
        while (true)
        {
            foreach (var item in APICall.instance.mockTokenId)
            {
                if (item != 0)
                {
                    GameObject ui = Instantiate(raptorUIToInstantiate, transform.position, transform.rotation);
                    ui.transform.SetParent(canvas.transform, false);
                    ui.GetComponent<MovingUI>().SetTokenIdText(item.ToString());
                    yield return new WaitForSeconds(4f);
                }
            }
        }
    }
}
