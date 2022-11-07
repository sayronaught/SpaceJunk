using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class PreLobby : MonoBehaviour
{

    public TMP_Text debugText;

    public async void TestGet()
    {
        var url = "http://trollbyte.io/SpaceJunk/api.php?apiVersion=One";

        var httpClient = new HappyHttpClient(new JsonSerializationOption());
        var result = await httpClient.Get<ApiV1>(url);
        Debug.Log(result.greeting);
        debugText.text = result.greeting;
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
