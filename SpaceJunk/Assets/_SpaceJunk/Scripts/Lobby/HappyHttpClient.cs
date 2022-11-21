using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HappyHttpClient
{
    private readonly ISerializationOption _serializationOption;

    public HappyHttpClient(ISerializationOption serializationOption)
    {
        _serializationOption = serializationOption;
    }
    [Serializable]
    public class postdata
    {
        public string key;
        public string value;
        public postdata create(string newkey,string newvalue)
        {
            key = newkey;
            value = newvalue;
            return this;
        }
    }

    //public List<postdata>

    public async Task<TResultType> Get<TResultType>(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);

            //www.SetRequestHeader("Content-Type", _serializationOption.ContentType);
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            www.SetRequestHeader("User-Agent", "runscope/0.1");

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = _serializationOption.Deserialize<TResultType>(www.downloadHandler.text);

            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }
    public async Task<TResultType> Post<TResultType>(string url, postdata[] data)
    {
        try
        {
            WWWForm form = new WWWForm();
            if ( data.Length > 0)
            {
                foreach (postdata dataset in data)
                {
                    form.AddField(dataset.key, dataset.value);
                }
            }
            //form.AddField("username", "aaa");

            using var www = UnityWebRequest.Post(url, form);

            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            www.SetRequestHeader("User-Agent", "runscope/0.1");

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = _serializationOption.Deserialize<TResultType>(www.downloadHandler.text);

            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }
}