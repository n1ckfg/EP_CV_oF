#pragma warning disable 0219
using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DlibFaceLandmarkDetector
{
	/// <summary>
	/// Face landmark detector.
	/// </summary>
	public class FaceLandmarkDetector: DisposableDlibObject
	{
		
		
		protected override void Dispose (bool disposing)
		{
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			try {
				
				if (disposing) {
				}
				
				if (IsEnabledDispose) {
					if (nativeObj != IntPtr.Zero)
						DlibFaceLandmarkDetector_Dispose (nativeObj);
					nativeObj = IntPtr.Zero;
				}
				
			} finally {
				base.Dispose (disposing);
			}
			
			#else
			
			#endif
		}

		/**
		* Initializes FaceLandmarkDetector using default frontal face detector.
		* 
		* ObjectDetector is initialized in such a code.
		*   frontal_face_detector face_detector;
		*   face_detector = get_frontal_face_detector();
		* 
		* ShapePredictor is initialized in such a code.
		*   shape_predictor sp;
		*   deserialize(shape_predictor_filename) >> sp;
		* 
		* 
		* @param shapePredictorFilePath
		*/
		public FaceLandmarkDetector (string shapePredictorFilePath)
		{
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			nativeObj = DlibFaceLandmarkDetector_Init ();


				if(!DlibFaceLandmarkDetector_LoadObjectDetector(nativeObj, null)){
//					Debug.LogError ("Failed to load " + objectDetectorFilename);
				}

				if(!DlibFaceLandmarkDetector_LoadShapePredictor (nativeObj, shapePredictorFilePath)){
					Debug.LogError ("Failed to load " + shapePredictorFilePath);
				}
			
			return;
			#else
			
			#endif
		}

		/**
		* Initializes FaceLandmarkDetector.
		* 
		* ObjectDetector is initialized in such a code.
		*   if(object_detector_filename != null){
		*     object_detector<scan_fhog_pyramid<pyramid_down<6>>> simple_detector;
		*     deserialize(object_detector_filename) >> simple_detector;
		*   }else{
		*     frontal_face_detector face_detector;
		*     face_detector = get_frontal_face_detector();
		*   }
		* 
		* ShapePredictor is initialized in such a code.
		*   shape_predictor sp;
		*   deserialize(shape_predictor_filename) >> sp;
		* 
		* @param objectDetectorFilePath
		* @param shapePredictorFilePath
		*/
		public FaceLandmarkDetector (string objectDetectorFilePath, string shapePredictorFilePath)
		{
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5

			nativeObj = DlibFaceLandmarkDetector_Init ();

				if(!DlibFaceLandmarkDetector_LoadObjectDetector(nativeObj, objectDetectorFilePath)){
					Debug.LogError ("Failed to load " + objectDetectorFilePath);
				}

				if(!DlibFaceLandmarkDetector_LoadShapePredictor (nativeObj, shapePredictorFilePath)){
					Debug.LogError ("Failed to load " + shapePredictorFilePath);
				}

			
			return;
			#else
			
			#endif
		}

		/**
		* Set Image from Texture2D
		* 
		* @param texture2D
		*/
		public void SetImage (Texture2D texture2D)
		{
			if (texture2D == null)
				throw new ArgumentNullException ("texture2D == null");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5

			#if UNITY_5 && !UNITY_5_0 

			if (texture2D.format == TextureFormat.RGBA32) {

				SetImage<byte> (texture2D.GetRawTextureData (), texture2D.width, texture2D.height, 4, true);

				return;
			}
			if (texture2D.format == TextureFormat.RGB24) {
				
				SetImage<byte> (texture2D.GetRawTextureData (), texture2D.width, texture2D.height, 3, true);
				
				return;
			}
			if (texture2D.format == TextureFormat.Alpha8) {
				
				SetImage<byte> (texture2D.GetRawTextureData (), texture2D.width, texture2D.height, 1, true);
				
				return;
			}
			#endif

			Color32[] colors = texture2D.GetPixels32 ();
			
			GCHandle colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);

			DlibFaceLandmarkDetector_SetImage (nativeObj, colorsHandle.AddrOfPinnedObject (), texture2D.width, texture2D.height, 4, true);

			colorsHandle.Free ();

			#else
			return;
			#endif
			
		}

		/**
		* Set Image from WebCamTexture
		* 
		* @param webCamTexture
		*/
		public void SetImage (WebCamTexture webCamTexture)
		{
			SetImage (webCamTexture, null);
		}

		/**
		* Set Image from WebCamTexture
		* 
		* @param webCamTexture
		* @param bufferColors Optional array to receive pixel data.
		* You can optionally pass in an array of Color32s to use in colors to avoid allocating new memory each frame.
		* The array needs to be initialized to a length matching width * height of the texture.(http://docs.unity3d.com/ScriptReference/WebCamTexture.GetPixels32.html)
		*/
		public void SetImage (WebCamTexture webCamTexture, Color32[] bufferColors)
		{
			
			if (webCamTexture == null)
				throw new ArgumentNullException ("webCamTexture == null");
			ThrowIfDisposed ();
			
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			GCHandle colorsHandle;
			if (bufferColors == null) {
				
				Color32[] colors = webCamTexture.GetPixels32 ();
				
				colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
			} else {
				webCamTexture.GetPixels32 (bufferColors);
				
				colorsHandle = GCHandle.Alloc (bufferColors, GCHandleType.Pinned);
			}
			
			DlibFaceLandmarkDetector_SetImage (nativeObj, colorsHandle.AddrOfPinnedObject (), webCamTexture.width, webCamTexture.height, 4, true);
			
			colorsHandle.Free ();
			
			#else
			return;
			#endif
			
		}

		/**
		* Set Image from IntPtr
		* 
		* @param intPtr
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		*/
		public void SetImage (IntPtr intPtr, int width, int height, int bytesPerPixel)
		{
			SetImage (intPtr, width, height, bytesPerPixel, false);
		}

		/**
		* Set Image from IntPtr
		* 
		* @param intPtr
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param flip flip vertical
		*/
		public void SetImage (IntPtr intPtr, int width, int height, int bytesPerPixel, bool flip)
		{
			if (intPtr == IntPtr.Zero)
				throw new ArgumentNullException ("intPtr == IntPtr.Zero");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			DlibFaceLandmarkDetector_SetImage (nativeObj, intPtr, width, height, bytesPerPixel, flip);
			
			#else
			return;
			#endif
			
		}

		/**
		* Set Image from IList<T>
		* 
		* @param array
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		*/
		public void SetImage<T> (IList<T> array, int width, int height, int bytesPerPixel)
		{
			SetImage<T> (array, width, height, bytesPerPixel, false);
		}

		/**
		* Set Image from IList<T>
		* 
		* @param array
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param flip flip vertical
		*/
		public void SetImage<T> (IList<T> array, int width, int height, int bytesPerPixel, bool flip)
		{
			
			if (array == null)
				throw new ArgumentNullException ("array == null");
			ThrowIfDisposed ();
			
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			GCHandle arrayHandle = GCHandle.Alloc (array, GCHandleType.Pinned);

			DlibFaceLandmarkDetector_SetImage (nativeObj, arrayHandle.AddrOfPinnedObject (), width, height, bytesPerPixel, flip);
			
			arrayHandle.Free ();
			
			#else
			return;
			#endif
			
		}


		public class RectDetection
		{
			public Rect rect;
			public double detection_confidence;
			public long weight_index;
			
			public RectDetection ()
			{
				rect = new Rect();
				detection_confidence = 0.0;
				weight_index = 0;
			}
		}

		/**
		* Detect Objects
		* 
		* @return List<Rect> detected list of object's rect.
		*/
		public List<Rect> Detect ()
		{
			return Detect (0.0);
		}

		/**
		* Detect Objects
		* 
		* @return List<Rect> detected list of object's rect.
		*/
		public List<Rect> Detect (double adjust_threshold)
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5

			List<Rect> rects = new List<Rect> ();

			int detectCount = DlibFaceLandmarkDetector_Detect (nativeObj, adjust_threshold);
			if (detectCount > 0) {

				double[] result = new double[detectCount * 6];
				DlibFaceLandmarkDetector_GetDetectResult (nativeObj, result);

				for (int i = 0; i < detectCount; i++) {

//					Debug.Log ("face : 0"+ result[i*6 + 0] + " 1 "+ result[i*6 + 1] + " 2 "+ result[i*6 + 2] + " 3 "+ result[i*6 + 3]);
					rects.Add (new Rect ((float)result [i * 6 + 0], (float)result [i * 6 + 1], (float)result [i * 6 + 2], (float)result [i * 6 + 3]));
				}
			}

			return rects;
			
			#else
			return null;
			#endif
		}

		/**
		* Detect Objects
		* 
		* @return List<RectDetection> detected list of object's RectDetection.
		*/
		public List<RectDetection> DetectRectDetection ()
		{
			return DetectRectDetection (0.0);
		}
		
		/**
		* Detect Objects
		* 
		* @return List<RectDetection> detected list of object's RectDetection.
		*/
		public List<RectDetection> DetectRectDetection (double adjust_threshold)
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			List<RectDetection> rectDetections = new List<RectDetection> ();
			
			int detectCount = DlibFaceLandmarkDetector_Detect (nativeObj, adjust_threshold);
			if (detectCount > 0) {
				
				double[] result = new double[detectCount * 6];
				DlibFaceLandmarkDetector_GetDetectResult (nativeObj, result);
				
				for (int i = 0; i < detectCount; i++) {
					RectDetection rectDetection = new RectDetection();
					rectDetection.rect = new Rect ((float)result [i * 6 + 0], (float)result [i * 6 + 1], (float)result [i * 6 + 2], (float)result [i * 6 + 3]);
					rectDetection.detection_confidence = result [i * 6 + 4];
					rectDetection.weight_index = (long)result [i * 6 + 5];

					rectDetections.Add (rectDetection);
				}
			}
			
			return rectDetections;
			
			#else
			return null;
			#endif
		}

		/**
		* Detect Objects
		* 
		* @return double[] detected object's data.[left_0, top_0, width_0, height_0, detection_confidence_0, weight_index_0, left_1, top_1, width_1, height_1, detection_confidence_1, weight_index_1, ...]
		*/
		public double[] DetectArray ()
		{
			return DetectArray(0.0);
		}

		/**
		* Detect Objects
		* 
		* @return double[] detected object's data.[left_0, top_0, width_0, height_0, detection_confidence_0, weight_index_0, left_1, top_1, width_1, height_1, detection_confidence_1, weight_index_1, ...]
		*/
		public double[] DetectArray (double adjust_threshold)
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			int detectCount = DlibFaceLandmarkDetector_Detect (nativeObj, adjust_threshold);
			if (detectCount > 0) {
				
				double[] result = new double[detectCount * 6];
				DlibFaceLandmarkDetector_GetDetectResult (nativeObj, result);

				return result;
			}
			
			return null;
			
			#else
			return null;
			#endif
		}

		/**
		* Detect Object Landmark
		* 
		* @param left
		* @param top
		* @param width
		* @param height
		* @return List<Vector2> detected Vector2 list of object landmark.
		*/
		public List<Vector2> DetectLandmark (double left, double top, double width, double height)
		{
			return DetectLandmark (new Rect ((float)left, (float)top, (float)width, (float)height));
		}

		/**
		* Detect Object Landmark
		* 
		* @param rect
		* @return List<Vector2> detected Vector2 list of object landmark.
		*/
		public List<Vector2> DetectLandmark (Rect rect)
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			List<Vector2> points = new List<Vector2> ();
			
			int detectCount = DlibFaceLandmarkDetector_DetectLandmark (nativeObj, rect.xMin, rect.yMin, rect.width, rect.height);
			if (detectCount > 0) {
				
				double[] result = new double[detectCount * 2];
				DlibFaceLandmarkDetector_GetDetectLandmarkResult (nativeObj, result);
				
				for (int i = 0; i < detectCount; i++) {
					
					points.Add (new Vector2 ((float)result [i * 2 + 0], (float)result [i * 2 + 1]));
				}
			}
			
			return points;
			
			#else
			return null;
			#endif
		}

		/**
		* Detect Object Landmark
		* 
		* @param left
		* @param top
		* @param width
		* @param height
		* @return double[] detected object landmark data.[x_0, y_0, x_1, y_1, ...]
		*/
		public double[] DetectLandmarkArray (int left, int top, int width, int height)
		{
			return DetectLandmarkArray (new Rect (left, top, width, height));
		}

		/**
		* Detect Object Landmark
		* 
		* @param rect
		* @return double[] detected object landmark data.[x_0, y_0, x_1, y_1, ...]
		*/
		public double[] DetectLandmarkArray (Rect rect)
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			int detectCount = DlibFaceLandmarkDetector_DetectLandmark (nativeObj, (int)rect.xMin, (int)rect.yMin, (int)rect.width, (int)rect.height);
			if (detectCount > 0) {
				
				double[] result = new double[detectCount * 2];
				DlibFaceLandmarkDetector_GetDetectLandmarkResult (nativeObj, result);
				
				return result;
			}
			
			return null;
			
			#else
			return null;
			#endif
		}

		/**
		* Whether all of the object parts point is contained in the object rectangle?
		* 
		* @return bool
		*/
		public bool IsAllPartsInRect ()
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			bool flag = DlibFaceLandmarkDetector_IsAllPartsInRect (nativeObj);
			
			return flag;
			
			#else
			return false;
			#endif
		}

		/**
		* Get ShapePredictorNumParts
		* 
		* @return long
		*/
		public long GetShapePredictorNumParts ()
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			long numParts = DlibFaceLandmarkDetector_ShapePredictorNumParts (nativeObj);
			
			return numParts;
			
			#else
			return -1;
			#endif
		}

		/**
		* Get ShapePredictorNumFeatures
		* 
		* @return long
		*/
		public long GetShapePredictorNumFeatures ()
		{
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			long numFeatures = DlibFaceLandmarkDetector_ShapePredictorNumFeatures (nativeObj);
			
			return numFeatures;
			
			#else
			return -1;
			#endif
		}

		/**
		* Draw Detect Result
		* 
		* @param texture2D
		* @param r
		* @param g
		* @param b
		* @param a
		* @param thickness
		*/
		public void DrawDetectResult (Texture2D texture2D, int r, int g, int b, int a, int thickness)
		{

			if (texture2D == null)
				throw new ArgumentNullException ("texture2D == null");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5

			Color32[] colors = null;
			byte[] data = null;
			GCHandle colorsHandle;
			int bytesPerPixel = 4;
			#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
			if (texture2D.format == TextureFormat.RGBA32) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 4;
			}else if (texture2D.format == TextureFormat.RGB24) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 3;
			}else if (texture2D.format == TextureFormat.Alpha8) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 1;
			} else {
				colors = texture2D.GetPixels32 ();
				colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
				bytesPerPixel = 4;
			}
#else
			colors = texture2D.GetPixels32 ();
			colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
			bytesPerPixel = 4;
#endif

			DlibFaceLandmarkDetector_DrawDetectResult (nativeObj, colorsHandle.AddrOfPinnedObject (), texture2D.width, texture2D.height, bytesPerPixel, true, r, g, b, a, thickness);

			#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
			if (texture2D.format == TextureFormat.RGBA32 || texture2D.format == TextureFormat.RGB24 || texture2D.format == TextureFormat.Alpha8) {
				texture2D.LoadRawTextureData (colorsHandle.AddrOfPinnedObject (), texture2D.width * texture2D.height * bytesPerPixel);
			}else {
				texture2D.SetPixels32 (colors);
			}
#else
			texture2D.SetPixels32 (colors);
#endif
			texture2D.Apply ();

			colorsHandle.Free ();
			
			#else
			return;
			#endif
		}

		/**
		* Draw Detect Result
		* 
		* @param intPtr
		* @param width
		* @param height
		* @bytePerPixel 1 , 3 or 4
		* @param r
		* @param g
		* @param b
		* @param a
		* @param thickness
		*/
		public void DrawDetectResult (IntPtr intPtr, int width, int height, int bytesPerPixel, int r, int g, int b, int a, int thickness)
		{
			DrawDetectResult (intPtr, width, height, bytesPerPixel, false, r, g, b, a, thickness);
		}

		/**
		* Draw Detect Result
		* 
		* @param intPtr
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param flip flip vertical
		* @param r
		* @param g
		* @param b
		* @param a
		* @param thickness
		*/
		public void DrawDetectResult (IntPtr intPtr, int width, int height, int bytesPerPixel, bool flip, int r, int g, int b, int a, int thickness)
		{
			
			if (intPtr == IntPtr.Zero)
				throw new ArgumentNullException ("intPtr == IntPtr.Zero");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			DlibFaceLandmarkDetector_DrawDetectResult (nativeObj, intPtr, width, height, bytesPerPixel, flip, r, g, b, a, thickness);
			
			#else
			return;
			#endif
		}

		/**
		* Draw Detect Result
		* 
		* @param array
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param r
		* @param g
		* @param b
		* @param a
		* @param thickness
		*/
		public void DrawDetectResult<T> (IList<T> array, int width, int height, int bytesPerPixel, int r, int g, int b, int a, int thickness)
		{
			DrawDetectResult<T> (array, width, height, bytesPerPixel, false, r, g, r, a, thickness);
		}

		/**
		* Draw Detect Result
		* 
		* @param array
		* @param width
		* @param height
		* @param bytePerPixel 1 , 3 or 4
		* @param flip flip vertical
		* @param r
		* @param g
		* @param b
		* @param a
		* @param thickness
		*/
		public void DrawDetectResult<T> (IList<T> array, int width, int height, int bytesPerPixel, bool flip, int r, int g, int b, int a, int thickness)
		{
			
			if (array == null)
				throw new ArgumentNullException ("array == null");
			ThrowIfDisposed ();
			
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			GCHandle arrayHandle = GCHandle.Alloc (array, GCHandleType.Pinned);
			
			DlibFaceLandmarkDetector_DrawDetectResult (nativeObj, arrayHandle.AddrOfPinnedObject (), width, height, bytesPerPixel, flip, r, g, b, a, thickness);
			
			arrayHandle.Free ();
			
			#else
			return;
			#endif
			
		}

		/**
		* Draw Detect Landmark Result
		* 
		* @param texture2D
		* @param r
		* @param g
		* @param b
		* @param a
		*/
		public void DrawDetectLandmarkResult (Texture2D texture2D, int r, int g, int b, int a)
		{
			
			if (texture2D == null)
				throw new ArgumentNullException ("texture2D == null");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5

			Color32[] colors = null;
			byte[] data = null;
			GCHandle colorsHandle;
			int bytesPerPixel = 4;
			#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
			if (texture2D.format == TextureFormat.RGBA32) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 4;
			}else if (texture2D.format == TextureFormat.RGB24) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 3;
			}else if (texture2D.format == TextureFormat.Alpha8) {
				data = texture2D.GetRawTextureData ();
				colorsHandle = GCHandle.Alloc (data, GCHandleType.Pinned);
				bytesPerPixel = 1;
			} else {
				colors = texture2D.GetPixels32 ();
				colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
				bytesPerPixel = 4;
			}
			#else
			colors = texture2D.GetPixels32 ();
			colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
			bytesPerPixel = 4;
			#endif
			
			DlibFaceLandmarkDetector_DrawDetectLandmarkResult (nativeObj, colorsHandle.AddrOfPinnedObject (), texture2D.width, texture2D.height, bytesPerPixel, true, r, g, b, a);

			#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
			if (texture2D.format == TextureFormat.RGBA32 || texture2D.format == TextureFormat.RGB24 || texture2D.format == TextureFormat.Alpha8) {
				texture2D.LoadRawTextureData (colorsHandle.AddrOfPinnedObject (), texture2D.width * texture2D.height * bytesPerPixel);
			} else {
				texture2D.SetPixels32 (colors);
			}
			#else
			texture2D.SetPixels32 (colors);
			#endif
			texture2D.Apply ();

			colorsHandle.Free ();
			
			#else
			return;
			#endif
		}

		/**
		* Draw Detect Landmark Result
		* 
		* @param intPtr
		* @param width
		* @param height
		* @param bytesPerPixel 1 ,3 or 4
		* @param r
		* @param g
		* @param b
		* @param a
		*/
		public void DrawDetectLandmarkResult (IntPtr intPtr, int width, int height, int bytesPerPixel, int r, int g, int b, int a)
		{
			DrawDetectLandmarkResult (intPtr, width, height, bytesPerPixel, false, r, g, b, a);
		}

		/**
		* Draw Detect Landmark Result
		* 
		* @param intPtr
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param flip flip vertical
		* @param r
		* @param g
		* @param b
		* @param a
		*/
		public void DrawDetectLandmarkResult (IntPtr intPtr, int width, int height, int bytesPerPixel, bool flip, int r, int g, int b, int a)
		{
			
			if (intPtr == IntPtr.Zero)
				throw new ArgumentNullException ("intPtr == IntPtr.Zero");
			ThrowIfDisposed ();
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			
			DlibFaceLandmarkDetector_DrawDetectLandmarkResult (nativeObj, intPtr, width, height, bytesPerPixel, flip, r, g, b, a);
			
			#else
			return;
			#endif
		}

		/**
		* Draw Detect Landmark Result
		* 
		* @param array
		* @param width
		* @param height
		* @param bytesPerPixel 1 , 3 or 4
		* @param r
		* @param g
		* @param b
		* @param a
		*/
		public void DrawDetectLandmarkResult<T> (IList<T> array, int width, int height, int bytesPerPixel, int r, int g, int b, int a)
		{
			DrawDetectLandmarkResult<T> (array, width, height, bytesPerPixel, false, r, g, b, a);
		}

		/**
		* Draw Detect Landmark Result
		* 
		* @param array
		* @param width
		* @param height
		* @bytesPerPixel 1 , 3 or 4
		* @flip flip vertical
		* @param r
		* @param g
		* @param b
		* @param a
		*/
		public void DrawDetectLandmarkResult<T> (IList<T> array, int width, int height, int bytesPerPixel, bool flip, int r, int g, int b, int a)
		{
			
			if (array == null)
				throw new ArgumentNullException ("array == null");
			ThrowIfDisposed ();
			
			
			#if UNITY_PRO_LICENSE || ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR) || UNITY_5
			GCHandle arrayHandle = GCHandle.Alloc (array, GCHandleType.Pinned);
			
			DlibFaceLandmarkDetector_DrawDetectLandmarkResult (nativeObj, arrayHandle.AddrOfPinnedObject (), width, height, bytesPerPixel, flip, r, g, b, a);
			
			arrayHandle.Free ();
			
			#else
			return;
			#endif
			
		}

		
		#if UNITY_IOS && !UNITY_EDITOR
		const string LIBNAME = "__Internal";
#else
		const string LIBNAME = "dlibfacelandmarkdetector";
		#endif

		[DllImport (LIBNAME)]
		private static extern IntPtr DlibFaceLandmarkDetector_Init ();

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_Dispose (IntPtr nativeObj);

		[DllImport (LIBNAME)]
		private static extern bool DlibFaceLandmarkDetector_LoadObjectDetector (IntPtr self, string objectDetectorFilename);

		[DllImport (LIBNAME)]
		private static extern bool DlibFaceLandmarkDetector_LoadShapePredictor (IntPtr self, string shapePredictorFilename);

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_SetImage (IntPtr self, IntPtr byteArray, int texWidth, int texHeight, int bytesPerPixel, bool flip);

		[DllImport (LIBNAME)]
		private static extern int DlibFaceLandmarkDetector_Detect (IntPtr self, double adjust_threshold);

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_GetDetectResult (IntPtr self, double[] result);

		[DllImport (LIBNAME)]
		private static extern int DlibFaceLandmarkDetector_DetectLandmark (IntPtr self, double left, double top, double right, double bottom);

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_GetDetectLandmarkResult (IntPtr self, double[] result);

		[DllImport (LIBNAME)]
		private static extern bool DlibFaceLandmarkDetector_IsAllPartsInRect (IntPtr self);

		[DllImport (LIBNAME)]
		private static extern long DlibFaceLandmarkDetector_ShapePredictorNumParts (IntPtr self);

		[DllImport (LIBNAME)]
		private static extern long DlibFaceLandmarkDetector_ShapePredictorNumFeatures (IntPtr self);

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_DrawDetectResult (IntPtr self, IntPtr byteArray, int texWidth, int texHeight, int bytesPerPixel, bool flip, int r, int g, int b, int a, int thickness);

		[DllImport (LIBNAME)]
		private static extern void DlibFaceLandmarkDetector_DrawDetectLandmarkResult (IntPtr self, IntPtr byteArray, int texWidth, int texHeight, int bytesPerPixel, bool flip, int r, int g, int b, int a);
	}
}

