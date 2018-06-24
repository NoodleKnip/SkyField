using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Layer : MonoBehaviour
{
	public enum LayerType
	{
		SphericalMappedImage,
		PlanarMappedImage,
		GeneratedContent
	};

	public int id;
	public LayerType layerType = LayerType.SphericalMappedImage;

	public GameObject ingameObject;
	public RectTransform uiElem;

	public Layer(int newId)
	{
		this.id = newId;
	}
}
