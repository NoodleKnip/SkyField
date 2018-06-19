using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LayerManager : MonoBehaviour
{
	public class Layer
	{
		public Layer(int newId)
		{
			this.id = newId;
		}

		public int id;

		public GameObject gameObject;
		public RectTransform uiElem;
	}

	public Material masterMaterial;
	public GameObject masterAsset;
	public Texture testTexture;

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

	public void AddLayer()
	{
		Layer newLayer = new Layer(nextLayerSuffix);
		layers.Add(newLayer);

		string newLayerName = newLayerPrefix + " " + newLayer.id;

		// Create the physical object
		newLayer.gameObject = Instantiate(masterAsset, new Vector3(0, 0, 0), Camera.main.transform.rotation, layerParent);
		newLayer.gameObject.name = newLayerPrefix + " " + newLayer.id;

		// Change the meshes' texture
		Renderer meshRenderer = newLayer.gameObject.GetComponentInChildren<Renderer>();
		meshRenderer.material.mainTexture = testTexture;
		meshRenderer.material.renderQueue = newLayer.id;

		// New UI element
		newLayer.uiElem = Instantiate(layerUIElem, layersList.transform);
		newLayer.uiElem.name = newLayerPrefix + " " + newLayer.id;

		LayerUIEntry layerUIEntry = newLayer.uiElem.GetComponent<LayerUIEntry>();
		if(layerUIEntry)
		{
			layerUIEntry.SetLayer(newLayer);
			layerUIEntry.SetText(newLayerPrefix + " " + newLayer.id);
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
}
