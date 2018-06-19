using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LayerUIEntry : MonoBehaviour
{
	public InputField textElement;
	public Button deleteButton;

	private LayerManager.Layer layer;

	void Start()
	{
		deleteButton.onClick.AddListener(OnDeleteClick);
	}

	public void SetLayer(LayerManager.Layer newLayer)
	{
		layer = newLayer;
	}

	public void SetText(string newText)
	{
		textElement.text = newText;
	}

	private void OnDeleteClick()
	{
		LayerManager.instance.DeleteLayer(layer);
	}
}
