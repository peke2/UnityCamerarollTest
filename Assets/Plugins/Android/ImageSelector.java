package xyz.peke2.unitycamerarolltest;
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;
import android.os.Bundle;
import android.util.Log;

import android.database.Cursor;
import android.os.Bundle;
import android.content.ContentResolver;
import android.content.Intent;
import android.provider.DocumentsContract;
import android.provider.MediaStore;
import android.widget.Button;
import android.view.View;
import android.net.Uri;

import java.io.Console;

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

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent intent){
		if(requestCode!=REQUEST_IMAGE_GET || resultCode!= RESULT_OK) return;
		Uri uri = intent.getData();

		// System.out.println("mylog:"+uri.toString());

		String id = DocumentsContract.getDocumentId(uri);
        String[] ids = id.split(":");
        String sid = ids[ids.length-1];

		ContentResolver cres = getContentResolver();
		// cres.takePersistableUriPermission(uri, Intent.FLAG_GRANT_READ_URI_PERMISSION);
        Cursor csr = cres.query(
            MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
            new String[]{MediaStore.Images.Media.DATA},
            "_id=?",
            new String[]{sid},
			null
        );

		String filePath = "";

        if( csr.moveToFirst() ){
            filePath = csr.getString(0);
        }
        csr.close();

		//System.out.println(String.format("mylog:画像パス[%s]", uri.toString()));
		//	Unity側で起動しているオブジェクトにメッセージを送る
		UnityPlayer.UnitySendMessage("ImageSelector", "OnSelected", filePath);
	}
}
