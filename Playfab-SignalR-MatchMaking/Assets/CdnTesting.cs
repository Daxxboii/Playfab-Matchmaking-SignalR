using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CdnTesting : MonoBehaviour
{
    public string Key;
    [ReadOnly] public string Url;

    [ButtonMethod]
    public void GenerateUrl()
    {
        Url = Utilities.GetUrlFromKey(config.CDN,Key);
    }

    [ButtonMethod]
    public void Validate()
    {
        if (Utilities.ValidateUri(Url))
        {
            Debug.Log("200");
        }
        else
        {
            Debug.Log("DoesNotExist");
        }
    }
}
