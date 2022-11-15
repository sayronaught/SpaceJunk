using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

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

        var url = "http://trollbyte.io/SpaceJunk/api.php?apiVersion=One";
        
        var httpClient = new HappyHttpClient(new JsonSerializationOption());
        var postData = new HappyHttpClient.postdata();

        var result = await httpClient.Get<ApiV1>(url);
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
