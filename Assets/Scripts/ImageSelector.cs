using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour
{
	public Texture LoadedTexture {get; private set;}
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnClick()
	{
		Debug.Log("pushed");
		var imgSel = new AndroidJavaObject("xyz.peke2.unitycamerarolltest.ImageSelector");
		imgSel.CallStatic("selectImage");
	}

	public void OnSelected(string path)
	{
		Debug.Log(path);
		// StartCoroutine("LoadImage", path.Replace("content:", "file:"));
		// path = path.Replace("/storage/emulated/0", "/storage/self/primary");
		
		using( AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment") )
        using(AndroidJavaObject joPublicDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jcEnvironment.GetStatic<string>( /*"DIRECTORY_PICTURES"*/"DIRECTORY_DCIM" ) ) )
        {
			var externalPath = joPublicDir.Call<string>("toString");
			Debug.Log($"ExternalPath[{externalPath}]");
		}

		// StartCoroutine("LoadImage", "jar:file://" + path);
		StartCoroutine("LoadImage", path);
	}


	IEnumerator LoadImage(string path)
	{
		// if(!System.IO.File.Exists(path)){
		// 	Debug.Log($"File no exists[{path}]");
		// 	yield break;
		// }
		// Debug.Log($"asset path=[{Application.streamingAssetsPath}]");

		using( UnityWebRequest req = UnityWebRequestTexture.GetTexture(path) ){
			yield return req.SendWebRequest();
			if( !req.isNetworkError && !req.isHttpError ){
				Debug.Log("読み込み完了");
				
				Texture tex;
				while( (tex = DownloadHandlerTexture.GetContent(req)) == null ){
					yield return new WaitForEndOfFrame();
				}

				Debug.Log("テクスチャ取得完了");

				LoadedTexture = tex;
				var obj = GameObject.Find("ImageBoard");
				var mat = obj.GetComponent<Material>();
				mat.SetTexture("_MainTex", LoadedTexture);
			}
			else{
				Debug.Log($"ファイル読み込み失敗[{path}][{req.error}]");
			}
		}
	}
}
