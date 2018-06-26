using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using SFB;


public enum CubemapRenderLayout
{
	SeparateFaces,
	UnrealEngine4
};


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
		CaptureAndSaveCubemap();
	}

	public void CaptureAndSaveCubemap(CubemapRenderLayout saveLayout = CubemapRenderLayout.UnrealEngine4)
	{
		// Show the save dialog
		string savePath = StandaloneFileBrowser.SaveFilePanel("Save Cubemap", "", "New Cubemap", "png");
		if(savePath == "")
			return;

		// Sanitise the file path
		savePath = Path.ChangeExtension(savePath, "png");

		Cubemap cubemap = CaptureCubemap();

		if(saveLayout == CubemapRenderLayout.SeparateFaces)
		{
			// Prep a texture buffer before hand, so we don't need to make a new one each loop,
			// we just reuse this one.
			var textureBuffer = new Texture2D(cubemap.width, cubemap.height, TextureFormat.RGB24, false);

			CubemapFace[] faces = new CubemapFace[]
			{
				CubemapFace.PositiveX, CubemapFace.NegativeX,
				CubemapFace.PositiveY, CubemapFace.NegativeY,
				CubemapFace.PositiveZ, CubemapFace.NegativeZ
			};

			foreach(CubemapFace face in faces)
			{
				textureBuffer.SetPixels(cubemap.GetPixels(face));
				File.WriteAllBytes(savePath + "/" + face.ToString() + ".png", textureBuffer.EncodeToPNG());
			}
		}
		else if(saveLayout == CubemapRenderLayout.UnrealEngine4)
		{
			Texture2D finalRender = CubemapToUE4Format(cubemap);
			File.WriteAllBytes(savePath + ".png", finalRender.EncodeToPNG());
		}
		
	}

	public Cubemap CaptureCubemap(int resolution = 1024)
	{
		// Construct a new cubemap object
		Cubemap cubemap = new Cubemap(resolution, TextureFormat.RGB24, false);

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
		cubemapRendererCamera.RenderToCubemap(cubemap);
		Destroy(cubemapRootObject);

		return cubemap;
	}

	public Texture2D CubemapToUE4Format(Cubemap cubemap)
	{
		// Construct a long (6 times the width of the cubemap resolution) texture that
		// will fit all faces at once. Like this:
		// 
		//    F1  F2  F3  ..
		//   +---+---+---+---+---+---+
		//   |   |   |   |   |   |   |
		//   +---+---+---+---+---+---+
		//   
		// See https://docs.unrealengine.com/en-US/Engine/Content/Types/Textures/Cubemaps/CreatingCubemaps
		// for more details on how the UE4 cubemap should be generated.
		Texture2D textureBuffer = new Texture2D(cubemap.width * 6, cubemap.height);

		CubemapFace[] faces = new CubemapFace[]
		{
			CubemapFace.PositiveX, CubemapFace.NegativeX,
			CubemapFace.PositiveY, CubemapFace.NegativeY,
			CubemapFace.PositiveZ, CubemapFace.NegativeZ
		};

		int cursorPosX = 0;
		foreach(CubemapFace face in faces)
		{
			textureBuffer.SetPixels(cursorPosX, 0, cubemap.width, cubemap.height, cubemap.GetPixels(face));
			cursorPosX += cubemap.width;
		}

		return textureBuffer;
	}
}
