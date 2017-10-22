using UnityEngine;
using System.Collections;

public class ShiftUV : MonoBehaviour {

	public Transform target;
	public Renderer ren;
	public float uvScaler = 1f;
	public float posScaler = 1f;
	public float rotScaler = 1f;

	private Vector3 lastPos = Vector3.zero;
	private Vector3 deltaPos = Vector3.zero;
	private Vector3 deltaRot = Vector3.zero;

    void Update() {
        if (target != null) {
            Vector3 d = target.position - lastPos;
            deltaPos = d * posScaler;
            deltaRot = d * rotScaler;

            ren.material.SetVector("_Ctl", new Vector3(target.position.x * uvScaler, target.position.y * uvScaler, 0f));

            transform.Rotate(deltaRot.y, 0f, -deltaRot.x);
            transform.Translate(-deltaPos.x, 0f, deltaPos.y);

            lastPos = target.position;
        }
	}

}
