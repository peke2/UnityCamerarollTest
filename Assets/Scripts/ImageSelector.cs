using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.IO;

public class ImageSelector : MonoBehaviour
{
	public delegate void LoadedCallback(Texture2D tex);
	public LoadedCallback OnLoaded {get; set;}

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
		// Debug.Log(path);
		// path = path.Replace("/storage/emulated/0", "/storage/self/primary");
		
		// using( AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment") )
        // using(AndroidJavaObject joPublicDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jcEnvironment.GetStatic<string>( /*"DIRECTORY_PICTURES"*/"DIRECTORY_DCIM" ) ) )
        // {
		// 	var externalPath = joPublicDir.Call<string>("toString");
		// 	Debug.Log($"ExternalPath[{externalPath}]");
		// }

		// StartCoroutine("LoadImage", "jar:file://" + path);
		// StartCoroutine("LoadImage", path);
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		// AndroidJavaObject uri = jo.Get<AndroidJavaObject>("selectedUri");
		// AndroidJavaObject contentResolver = jo.Call<AndroidJavaObject>("getContentResolver");
        // AndroidJavaObject inputStream = contentResolver.Call<AndroidJavaObject>("openInputStream", uri);
		// int fileSize = int.Parse(path);
		// byte[] bytes = new byte[fileSize];
		// int loadedByte = inputStream.Call<int>("read", bytes);
		// inputStream.Call("close");

		// string uriStr = uri.Call<string>("toString");
		// Debug.Log($"uri:{uriStr}");
		// Debug.Log($"Loaded file size={fileSize} / result size={loadedByte}");

		byte[] bytes = jo.Call<byte[]>("getLoadedData");

		Debug.Log($"{bytes[0]}, {bytes[1]}, {bytes[2]}, {bytes[3]}");

		var tex = new Texture2D(1,1);
		tex.LoadImage(bytes);

		if( OnLoaded != null ){
			OnLoaded(tex);
		}

		jo.Call("clearLoadedData");

	}


	IEnumerator LoadImage(string path)
	{
		// if(!System.IO.File.Exists(path)){
		// 	Debug.Log($"File no exists[{path}]");
		// 	yield break;
		// }
		// Debug.Log($"asset path=[{Application.streamingAssetsPath}]");

		// using( UnityWebRequest req = UnityWebRequestTexture.GetTexture(path) ){
		// 	yield return req.SendWebRequest();
		// 	if( !req.isNetworkError && !req.isHttpError ){
		// 		Debug.Log("読み込み完了");
				
		// 		Texture tex;
		// 		while( (tex = DownloadHandlerTexture.GetContent(req)) == null ){
		// 			yield return new WaitForEndOfFrame();
		// 		}

		// 		Debug.Log("テクスチャ取得完了");

		// 		LoadedTexture = tex;
		// 		var obj = GameObject.Find("ImageBoard");
		// 		var mat = obj.GetComponent<Material>();
		// 		mat.SetTexture("_MainTex", LoadedTexture);
		// 	}
		// 	else{
		// 		Debug.Log($"ファイル読み込み失敗[{path}][{req.error}]");
		// 	}
		// }

		if( System.IO.File.Exists(path) ){
			Debug.Log($"ファイル有り[{path}]");
		}
		else{
			Debug.Log($"ファイル無し[{path}]");
		}

		// string checkPath;
		// using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		// {
		// 	using (AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity"))
		// 	{
		// 		checkPath = currentActivity.Call<AndroidJavaObject>("getFilesDir").Call<string>("getCanonicalPath");
		// 	}
		// }
		// Debug.Log($"チェックパス[{checkPath}]");
		
		// var bytes = File.ReadAllBytes(path);
		byte[] bytes;
		using(var fs = File.OpenRead(path)){
			using(var reader = new BinaryReader(fs)){
				bytes = reader.ReadBytes((int)fs.Length);
			}			
		}

		Debug.Log($"filesize = [{bytes.Length}]");
		
		var tex = new Texture2D(1,1);
		tex.LoadImage(bytes);
		// var obj = GameObject.Find("ImageBoard");

		// var renderer = obj.GetComponent<Renderer>();
		// renderer.material.mainTexture = tex;
		if( OnLoaded != null ){
			OnLoaded(tex);
		}

		yield break;
	}
}
