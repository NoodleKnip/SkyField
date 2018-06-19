using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubemapManager : MonoBehaviour
{
	public static CubemapManager instance = null;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != null)
			Destroy(gameObject);

		DontDestroyOnLoad(instance);
	}

	public void CaptureCubemapWithDefaults()
	{
		CaptureCubemap();
	}

	public void CaptureCubemap(string outputPath = "cubemap.png", int resolution = 1024)
	{
		// Construct a new cubemap object
		Cubemap newCubemap = new Cubemap(resolution, TextureFormat.RGBA32, false);

		// Setup the root object that will house the camera
		GameObject cubemapRootObject = new GameObject("CubemapRenderer");
		cubemapRootObject.transform.position = new Vector3(0, 0, 0);
		cubemapRootObject.transform.rotation = Quaternion.identity;

		// Setup the camera correctly
		cubemapRootObject.AddComponent<Camera>();
		Camera cubemapRendererCamera = cubemapRootObject.GetComponent<Camera>();
		cubemapRendererCamera.clearFlags = CameraClearFlags.SolidColor;
		cubemapRendererCamera.backgroundColor = new Color(0, 0, 0);
		cubemapRendererCamera.depth = 1; // Set the depth to be higher than a default camera so we don't see flashing

		// Take snapshot
		cubemapRendererCamera.RenderToCubemap(newCubemap);

		// Pull pixel data from the snapshot

		// Save that pixel data to disk

		Destroy(cubemapRootObject);
	}
}
