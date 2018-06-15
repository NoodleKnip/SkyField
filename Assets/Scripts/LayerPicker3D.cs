using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LayerPicker3D : MonoBehaviour
{
	private Camera camera;

	void Start()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		// Cast a ray out from the 2D mouse coords into 3D frustrum coords
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{

		}
	}
}
