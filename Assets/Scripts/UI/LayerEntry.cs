using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LayerEntry : MonoBehaviour
{
	public InputField textElement;
	public Button deleteButton;
	public RawImage preview;

	private Layer layer;

	void Start()
	{
		deleteButton.onClick.AddListener(OnDeleteClick);
	}

	public void SetLayer(Layer newLayer)
	{
		layer = newLayer;
	}

	public void SetText(string newText)
	{
		textElement.text = newText;
	}

    public void SetPreview(Texture2D texture)
    {
		preview.texture = texture;

		// Do ratio calculations
		// Normalized Width or Height = Lowest side / highest side
		// Normalize X or Y           = 0.5 - Normalized Width or Height / 2
		Rect imageRect = preview.uvRect;

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

		preview.uvRect = imageRect;
	}

	private void OnDeleteClick()
	{
		LayerManager.instance.DeleteLayer(layer);
	}
}
