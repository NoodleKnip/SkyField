using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using SFB;


public class LayerManager : MonoBehaviour
{
	public Material masterMaterial;
	public GameObject masterAsset;

	[Header("Layers")]
	public Transform layerParent;
	public VerticalLayoutGroup layersList;
	public RectTransform layerUIElem;
	public string newLayerPrefix = "Layer";

	private List<Layer> layers;
	private int nextLayerSuffix = 1;

	public static LayerManager instance = null;


	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != null)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		layers = new List<Layer>();
	}

	void Start()
	{
		// Check to make sure the layer UI element prefab has the expected
		// script attached to it.
		if(!layerUIElem)
			Debug.LogError("No layer UI element assigned");
		else if(!layerUIElem.GetComponent<LayerEntry>())
			Debug.LogError("Ensure that the assigned layer UI element has the LayerEntry script attached");

		if(!layersList)
			Debug.LogError("No layer list assigned");
	}

	public void CreateNewLayer()
	{
		string[] paths = ShowMediaDialog("Import Image", "Images", new string[]{"png", "jpg", "jpeg"}, true);
		foreach(string path in paths)
		{
			Debug.Log(path);
			AddLayerFromImagePath(path);
		}
	}

	public string[] ShowMediaDialog(string dialogBoxTitle, string filterName, string[] filters, bool multiSelect = false)
	{
		var extensions = new[] {
			new ExtensionFilter(filterName, filters)
		};

		return StandaloneFileBrowser.OpenFilePanel(dialogBoxTitle, "", extensions, multiSelect);
	}

	public void AddLayerFromImagePath(string path)
	{
		Layer newLayer = new Layer(nextLayerSuffix);
		layers.Add(newLayer);

		string newLayerName = newLayerPrefix + " " + newLayer.id;

		// Create the 3D object
		newLayer.ingameObject = Instantiate(masterAsset, new Vector3(0, 0, 0), Camera.main.transform.rotation, layerParent);
		newLayer.ingameObject.name = newLayerPrefix + " " + newLayer.id;

		// Change the meshes' texture
		Texture2D textureFromPath = DecodeImageFromPath(path);
		Renderer meshRenderer = newLayer.ingameObject.GetComponentInChildren<Renderer>();
		meshRenderer.material.mainTexture = textureFromPath;
		meshRenderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
		meshRenderer.material.renderQueue = newLayer.id;

		// New UI element
		newLayer.uiElem = Instantiate(layerUIElem, layersList.transform);
		newLayer.uiElem.name = newLayerName;

        LayerEntry layerEntry = newLayer.uiElem.GetComponent<LayerEntry>();
		if(layerEntry)
		{
            layerEntry.SetLayer(newLayer);
            layerEntry.SetText(newLayerName);
			layerEntry.SetPreview(textureFromPath);
		}

		nextLayerSuffix++;
	}

	public void DeleteLayer(Layer layer)
	{
		Destroy(layer.gameObject);
		Destroy(layer.uiElem.gameObject);

		layers.Remove(layer);
	}

	public List<Layer> GetAllLayers()
	{
		return layers;
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
