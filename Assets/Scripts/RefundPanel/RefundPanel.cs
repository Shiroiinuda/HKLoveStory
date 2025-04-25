using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RefundPanel : MonoBehaviour
{
    public RefundCSVReader refundCSVReader;
    public GameControl gameControl;
    public CurrencyManager currencyManager;

    public TMP_InputField aCodeInput;
    public TMP_InputField bCodeInput;

    [SerializeField] private string aCode;
    [SerializeField] private string bCode;

    private void Start()
    {
        ResetInputs();
    }

    public void ResetInputs()
    {
        aCodeInput.text = "";
        bCodeInput.text = "";
        aCode = "14857t57439580kopipoiywtufeg0=vdifopdsjgjlgkhjfkshjlkfghjsldkghdjkghdljerertkljluaoruhuityipweotupohvbvbdkfahluiheruhtueirqoyhjkfdsgbdfffgtiuiour85738956yuhtjbngGHFGWEUIRYEITU0jghjvbsdldgbehtyworeuiotypowuroikfhdjsgbh730r54P850";
        bCode = "gdfggusiptui3w5=8=6856=579fgjgkzhfjgrtDSGDGeytyuyi738957938457-DGDFGTYHRTuqe3l47=3480iyujrtg5674874sdfgertw3ry23njebhwrkvgbyhkrtfvqiwetguigbrejhgbehrt743605uihgjbdhvfuakfgaeitpoeghjbnjGHHIUJIOjirwioeru893r78uhgfjshnfdm89ry70rfudiohfjhf";
    }

    private void GenerateACode(int jadeAmount)
    {
        List<string> jadeList = GetJadeList(jadeAmount);

        if (jadeList.Count == 0)
            return;

        int randomNumber = Random.Range(0, jadeList.Count);
        string[] lines = jadeList[randomNumber].Split(',');

        if (CheckHasRefund(lines[0].Trim()) == false)
        {
            aCode = lines[0];
            bCode = lines[1];

            aCodeInput.text = aCode;
        }
        else
        {
            ResetInputs();
        }
    }

    private bool CheckHasRefund(string a)
    {
        if (gameControl.refundList.Contains(a))
            return true;
        else
            return false;
    }

    private List<string> GetJadeList(int jadeAmount)
    {
        switch (jadeAmount)
        {
            case 10:
                return refundCSVReader.jadeString_10Jade;
            case 100:
                return refundCSVReader.jadeString_100Jade;
            case 400:
                return refundCSVReader.jadeString_400Jade;
            default:
                return new List<string>();
        }
    }

    private void SubmitBCode(int jadeAmount)
    {
        if (bCodeInput.text.Trim() == bCode.Trim())
        {
            gameControl.currency += jadeAmount;
            currencyManager.ShowCurrency();
            gameControl.refundList.Add(aCode);
            ResetInputs();
        }
    }

    public void GenerateACode_10Jade()
    {
        GenerateACode(10);
    }

    public void SubmitBCode_10Jade()
    {
        SubmitBCode(10);
    }

    public void GenerateACode_100Jade()
    {
        GenerateACode(100);
    }

    public void SubmitBCode_100Jade()
    {
        SubmitBCode(100);
    }

    public void GenerateACode_400Jade()
    {
        GenerateACode(400);
    }

    public void SubmitBCode_400Jade()
    {
        SubmitBCode(400);
    }
}
