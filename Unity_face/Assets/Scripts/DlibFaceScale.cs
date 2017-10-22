using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlibFaceScale : MonoBehaviour {

    public DlibFaceLandmarkDetectorSample.FaceTrackCoords face;
	public float speed = 0.5f;
	public float inputXmin = 0f;
	public float inputXmax = 1f;
	public float inputYmin = 0f;
	public float inputYmax = 1f;
	public float outputXmin = 0f;
	public float outputXmax = 1f;
	public float outputYmin = 0f;
	public float outputYmax = 1f;

	private float zPos = 0f;

	private void Start() {
		zPos = transform.position.z;
	}

	void Update() {
        Debug.Log(face.trackCoords.y);
		float x = remap(face.trackCoords.x, inputXmin, inputXmax, outputXmin, outputXmax);
		float y = remap(face.trackCoords.y, inputYmin, inputYmax, outputYmin, outputYmax);
		transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, zPos), speed);
	}

	float remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

}
