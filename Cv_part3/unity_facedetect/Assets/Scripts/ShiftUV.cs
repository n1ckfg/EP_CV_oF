using UnityEngine;
using System.Collections;

public class ShiftUV : MonoBehaviour {

	public Transform target;
	public Renderer ren;
	public float uvScaler = 1f;
	public float rotScaler = 1f;

	[HideInInspector] public Vector3 rect = Vector3.zero;

	private Vector3 lastPos = Vector3.zero;
	private Vector3 deltaPos = Vector3.zero;

	void Update() {
		if (target != null) {
			deltaPos = (target.position - lastPos) * rotScaler;

			ren.material.SetVector("_Ctl", new Vector3(target.position.x * uvScaler, target.position.y * uvScaler, 0f));

			transform.Rotate(deltaPos.y, 0f, -deltaPos.x);

			lastPos = target.position;
		} else {
			deltaPos = (rect - lastPos) * rotScaler;

			ren.material.SetVector("_Ctl", new Vector3(rect.x * uvScaler, rect.y * uvScaler, 0f));

			transform.Rotate(deltaPos.y, 0f, -deltaPos.x);

			lastPos = rect;
		}
	}

}
