package xyz.peke2.unitycamerarolltest;
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;
import android.os.Bundle;
import android.util.Log;

import android.database.Cursor;
import android.os.Bundle;
import android.content.ContentResolver;
import android.content.Intent;
import android.provider.MediaStore;
import android.net.Uri;

import java.io.InputStream;
import java.io.Console;
import java.io.File;

public class ImageSelector extends UnityPlayerActivity {
	static final int REQUEST_IMAGE_GET = 1;

	protected void onCreate(Bundle savedInstanceState) {
		// UnityPlayerActivity.onCreate() を呼び出す
		super.onCreate(savedInstanceState);
		// logcat にデバッグメッセージをプリントする
		// Log.d("OverrideActivity", "onCreate called!");
	}

	static public void selectImage() {
		UnityPlayer.currentActivity.runOnUiThread(new Runnable(){
			@Override
			public void run(){
				// Intent intent = new Intent(Intent.ACTION_OPEN_DOCUMENT);
				Intent intent = new Intent(Intent.ACTION_GET_CONTENT);

				// int flags = intent.getFlags();
				// System.out.println( String.format("mylog:flags=%d", flags));

				intent.setType("image/*");
				intent.addCategory(Intent.CATEGORY_OPENABLE);
				UnityPlayer.currentActivity.startActivityForResult(intent, REQUEST_IMAGE_GET);
			}
		});
	}

	//	オブジェクトを渡す手段が見当たらないので、ここに保持してUnity側から参照させる
	// public Uri selectedUri;
	private byte[] loadedData;

	public byte[] getLoadedData(){
		return loadedData;
	}

	public void clearLoadedData(){
		loadedData = null;
	}


	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent intent){
        if(requestCode!=REQUEST_IMAGE_GET || resultCode!= RESULT_OK) return;
        Uri uri = intent.getData();

        String[] columns = new String[]{MediaStore.Images.Media.SIZE};
        Cursor csr = getContentResolver().query(uri, columns, null,null,null);

        if( csr.moveToFirst() ){
            int sizeIndex = csr.getColumnIndex(MediaStore.Images.Media.SIZE);
			int fileSize = csr.getInt(sizeIndex);

			try{
				InputStream input = getContentResolver().openInputStream(uri);
				loadedData = new byte[fileSize];
				input.read(loadedData,0, fileSize);
				input.close();
				System.out.println("mylog:Load completed");
			}
			catch (Exception e){
				System.out.println("mylog:" + e.getMessage());
			}

		}
        csr.close();

		//	Unity側で起動しているオブジェクトにメッセージを送る
		UnityPlayer.UnitySendMessage("ImageSelector", "OnSelected", "");
	}
}
