using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
public class Utilities : MonoBehaviour
{
    /// <summary>
    /// Appends the key and cdn ip together , providing a path to user 
    /// </summary>
    /// <param name="Cdn_IP">Catch The Ip of CDN through the config file</param>
    /// <param name="Key">path to the File Requested</param>
   public static string GetUrlFromKey(string Cdn_IP,string Key)
    {
        string uri = "http://" + Cdn_IP + "/" + Key;
        Debug.Log("RequestedLink: " + uri);
        return uri;
    }

    /// <summary>
    /// Checks if the given urlis valid 
    /// </summary>
    /// <param name="url">Url to check</param>
    /// <returns></returns>
    public static bool ValidateUri(string url)
    {
        try
        {
            //Creating the HttpWebRequest
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Setting the Request method HEAD, you can also use GET too.
            request.Method = "HEAD";
            //Getting the Web Response.
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //Returns TRUE if the Status code == 200
            response.Close();
            return (true);
        }
        catch
        {
            //Any exception will returns false.
            return false;
        }
    }
}

