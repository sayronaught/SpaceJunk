using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Security.Cryptography;

public class PreLobby : MonoBehaviour
{

    public TMP_Text debugText;
    public TMP_InputField usernameAttempt;
    public TMP_InputField passwordAttempt;

    public async void TestGet()
    {
        var url = "http://trollbyte.io/SpaceJunk/api.php?apiVersion=One";

        var httpClient = new HappyHttpClient(new JsonSerializationOption());
        var result = await httpClient.Get<ApiV1>(url);
        Debug.Log(result.debug);
        debugText.text = result.debug;
    }

    public async void TryLogin()
    {
        /*
        var url = "http://trollbyte.io/SpaceJunk/api.php?apiVersion=One&trylogin=";
        url += usernameAttempt.text+"&trypassword="+passwordAttempt.text;
        */

        HMACSHA256 hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes("sjPass"));

        byte[] hashValue = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordAttempt.text));

        var url = "http://trollbyte.io/SpaceJunk/api.php?apiVersion=One";
        
        var httpClient = new HappyHttpClient(new JsonSerializationOption());
        var postData = new List<HappyHttpClient.postdata>();
        postData.Add(new HappyHttpClient.postdata().create("trylogin", usernameAttempt.text));
        postData.Add(new HappyHttpClient.postdata().create("trypassword", System.Text.Encoding.UTF8.GetString(hashValue)));
        var result = await httpClient.Post<ApiV1>(url,postData.ToArray());
        Debug.Log(result.debug);
        debugText.text = result.debug;
    }

    // Start is called before the first frame update
    void Start()
    {
        TestGet();
        //Task task = getWebDataAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
