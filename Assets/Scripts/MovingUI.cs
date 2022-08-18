using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingUI : MonoBehaviour
{
    [SerializeField] Text tokenIdText;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * 55 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "End")
        {
            Destroy(gameObject);
        }
    }

    public void SetTokenIdText(string value)
    {
        tokenIdText.text = value;
    }
}
