using System.Collections;
using System.Collections.Generic;
using System.IO;
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

	public void CaptureCubemap(string outputPath = null, string outputName = "New Cubemap", int resolution = 1024)
	{
		if(outputPath == null)
			outputPath = Path.GetFullPath(Application.dataPath + "/Cubemaps/..");
		string fullDestPath = Path.Combine(outputPath, outputName);
		Directory.CreateDirectory(fullDestPath);
		Debug.Log(fullDestPath);

		// Construct a new cubemap object
		Cubemap newCubemap = new Cubemap(resolution, TextureFormat.RGB24, false);

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

		// Take snapshot and delete the object from the scene
		cubemapRendererCamera.RenderToCubemap(newCubemap);
		Destroy(cubemapRootObject);

		// Make a new texture to prepare for pixel dump
		var tex = new Texture2D(newCubemap.width, newCubemap.height, TextureFormat.RGB24, false);
		CubemapFace[] faces = new CubemapFace[]
		{
			CubemapFace.PositiveX, CubemapFace.NegativeX,
			CubemapFace.PositiveY, CubemapFace.NegativeY,
			CubemapFace.PositiveZ, CubemapFace.NegativeZ
		};

		foreach(CubemapFace face in faces)
		{
			tex.SetPixels(newCubemap.GetPixels(face));
			File.WriteAllBytes(fullDestPath + "/" + face.ToString() + ".png", tex.EncodeToPNG());
		}
	}
}
