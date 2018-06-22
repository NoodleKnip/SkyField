using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using SFB;


public class MediaManager : MonoBehaviour
{
	public RectTransform mediaAssetTemplate;
	public RectTransform grid;

	static MediaManager instance = null;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if (instance != null)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void ShowMediaDialog()
	{
		var extensions = new[] {
			new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
		};
		string[] paths = StandaloneFileBrowser.OpenFilePanel("Import Media", "", extensions, true);

		foreach(string p in paths)
		{
			Texture2D texture = DecodeImageFromPath(p);
			if(texture)
			{
				RectTransform newTemplateCopy = Instantiate(mediaAssetTemplate);

				newTemplateCopy.SetParent(grid);

				RawImage image = newTemplateCopy.GetChild(0).GetComponent<RawImage>();
				image.texture = texture;

				// Do ratio calculations
				// Normalized Width or Height = Lowest side / highest side
				// Normalize X or Y           = 0.5 - Normalized Width or Height / 2
				Rect imageRect = image.uvRect;

				if(texture.height > texture.width)
				{
					imageRect.height = (float)texture.width / (float)texture.height;
					imageRect.y = 0.5f - imageRect.height / 2f;
				}
				else if(texture.width > texture.height)
				{
					imageRect.width = (float)texture.height / (float)texture.width;
					imageRect.x = 0.5f - imageRect.width / 2f;
				}

				image.uvRect = imageRect;
			}
			else
			{
				Debug.Log("The image file does not exist (" + p + ")");
			}
		}
	}

	Texture2D DecodeImageFromPath(string path)
	{
		Texture2D texture = null;
		byte[] buffer;

		if(File.Exists(path))
		{
			buffer = File.ReadAllBytes(path);
			texture = new Texture2D(0, 0);
			texture.LoadImage(buffer);
			texture.wrapMode = TextureWrapMode.Clamp;
		}

		return texture;
	}
}
