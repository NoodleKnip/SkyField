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

	public void AddLayer(string path = null)
	{
		// Get the coords of the camera and add the layer opposite to the camera
		Vector3 cameraDir = Camera.main.transform.position.normalized;

		Layer newLayer = new Layer(nextLayerSuffix);
		layers.Add(newLayer);

		string newLayerName = newLayerPrefix + " " + newLayer.id;

		// Create the physical object
		newLayer.gameObject = Instantiate(masterAsset, new Vector3(0, 0, 0), Quaternion.identity, layerParent);
		newLayer.gameObject.name = "Layer " + newLayer.id;

		// Change the meshes' texture
		Renderer meshRenderer = newLayer.gameObject.GetComponent<Renderer>();
		meshRenderer.material.mainTexture = testTexture;

		// New UI element
		newLayer.uiElem = Instantiate(layerUIElem, layersList.transform);
		newLayer.uiElem.name = "Layer " + newLayer.id;
		newLayer.uiElem.GetComponent<Text>().text = newLayerName;

		nextLayerSuffix++;
	}
}
