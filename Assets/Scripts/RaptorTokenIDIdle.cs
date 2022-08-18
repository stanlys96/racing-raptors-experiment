using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaptorTokenIDIdle : MonoBehaviour
{
    public Text tokenIdText;

    public void SetTokenIdText(string value)
    {
        tokenIdText.text = value;
    }
}
