using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace DlibFaceLandmarkDetector
{
	public static class Utils
	{

		/**
		* Gets the file path.
		* <p>
		* <br>Set the filename in "StreamingAssets" folder'.
		* 
		* @param filename
		*/
		public static string getFilePath (string filename)
		{
			#if (UNITY_ANDROID) && !UNITY_EDITOR
			string srcPath = Application.streamingAssetsPath + "/" + filename;
			string destPath = Application.persistentDataPath + "/dlibfacelandmarkdetector/" + filename;
			
			//			Debug.Log("Extracting file from: "+ srcPath);
			//			Debug.Log("Extracting to: "+ destPath);
			
			WWW www =new WWW(srcPath);
			while(!www.isDone){;}
			
			//create Directory
			if(!Directory.Exists(Path.GetDirectoryName(destPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(destPath));
			
			bool needCopyFile = false;
			
			if(File.Exists(destPath)){
				//				Debug.Log("src size: "+ www.bytes.Length);
				
				FileInfo fi = new FileInfo(destPath);
				//				Debug.Log("dest size: "+ fi.Length);
				
				if(www.bytes.Length != fi.Length){
					needCopyFile = true;
				}
			}else{
				needCopyFile = true;
			}
			
			if(needCopyFile){
				
				File.WriteAllBytes(destPath, www.bytes);
				
				//			if(File.Exists(destPath)){
				//				Debug.Log("copy success: " + destPath);
				//			}else{
				//				Debug.Log("copy failure: " + destPath);
				//			}
			}
			
			return destPath;
			#else
			return System.IO.Path.Combine (Application.streamingAssetsPath, filename);
			#endif
		}

		/**
		* Sets the debug mode.
		* <p>
		* <br>if true, The error log of the Native side OpenCV will be displayed on the Unity Editor Console.
		* <br>This method is supported in WIN, MAC and LINUX.
		* <br>Please use as follows.
		* <br>Utils.setDebugMode(true);
		* <br>aaa
		* <br>bbb
		* <br>ccc
		* <br>Utils.setDebugMode(false);
		* 
		* @param debugMode
		*/
		public static void setDebugMode (bool debugMode)
		{
			#if (UNITY_PRO_LICENSE || UNITY_5) && (UNITY_STANDALONE || UNITY_EDITOR)
			DlibFaceLandmarkDetector_SetDebugMode (debugMode);

						if (debugMode) {
				DlibFaceLandmarkDetector_SetDebugLogFunc (debugLogFunc);
//				DlibFaceLandmarkDetector_DebugLogTest ();

						} else {
				DlibFaceLandmarkDetector_SetDebugLogFunc (null);
						}
#endif
		}
		
		

		#if (UNITY_PRO_LICENSE || UNITY_5) && (UNITY_STANDALONE || UNITY_EDITOR)

		private delegate void DebugLogDelegate (string str);
		
		private static DebugLogDelegate debugLogFunc = msg => Debug.LogError (msg);

		[DllImport ("dlibfacelandmarkdetector")]
		private static extern void DlibFaceLandmarkDetector_SetDebugMode (bool flag);

		[DllImport ("dlibfacelandmarkdetector")]
		private static extern void DlibFaceLandmarkDetector_SetDebugLogFunc (DebugLogDelegate func);

		[DllImport ("dlibfacelandmarkdetector")]
		private static extern void DlibFaceLandmarkDetector_DebugLogTest ();
		#endif

	}
}
