using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public Text firstPlaceTokenId;
    public Text secondPlaceTokenId;
    public Text thirdPlaceTokenId;
    public Text fourthPlaceTokenId;
    public Text fifthPlaceTokenId;
    public Text sixthPlaceTokenId;
    public Text fighter1TokenId;
    public Text fighter2TokenId;
    public Text fightWinnerTokenId;
    public Text injuredRaptorTokenId;

    private void Start()
    {
        SetAllTokenIds();
    }

    public void SetAllTokenIds()
    {
        int found = 0;
        for (int i = 0; i < APICall.instance.top3.Length; i++)
        {
            var item = APICall.instance.top3[i];
            if (!APICall.instance.fighters.Contains(item))
            {
                found++;
                if (found == 1)
                {
                    firstPlaceTokenId.text = item.ToString();
                }
                if (found == 2)
                {
                    secondPlaceTokenId.text = item.ToString();
                }
                if (found == 3)
                {
                    thirdPlaceTokenId.text = item.ToString();
                }
                if (found == 4)
                {
                    fourthPlaceTokenId.text = item.ToString();
                }
                if (found == 5)
                {
                    fifthPlaceTokenId.text = item.ToString();
                }
                if (found == 6)
                {
                    sixthPlaceTokenId.text = item.ToString();
                }
            }
        }
        for (int i = 0; i < APICall.instance.theRestRacer.Length; i++)
        {
            var item = APICall.instance.theRestRacer[i];
            if (!APICall.instance.fighters.Contains(item))
            {
                found++;
                if (found == 1)
                {
                    firstPlaceTokenId.text = item.ToString();
                }
                if (found == 2)
                {
                    secondPlaceTokenId.text = item.ToString();
                }
                if (found == 3)
                {
                    thirdPlaceTokenId.text = item.ToString();
                }
                if (found == 4)
                {
                    fourthPlaceTokenId.text = item.ToString();
                }
                if (found == 5)
                {
                    fifthPlaceTokenId.text = item.ToString();
                }
                if (found == 6)
                {
                    sixthPlaceTokenId.text = item.ToString();
                }
            }
        }
        for (int i = 0; i < APICall.instance.fighters.Length; i++)
        {
            var item = APICall.instance.fighters[i];
            if (i == 0)
            {
                fighter1TokenId.text = item.ToString();
            }
            if (i == 1)
            {
                fighter2TokenId.text = item.ToString();
            }
        }
        fightWinnerTokenId.text = APICall.instance.fightWinner.ToString();
        injuredRaptorTokenId.text = APICall.instance.injuredRaptor.ToString();
    }
}
