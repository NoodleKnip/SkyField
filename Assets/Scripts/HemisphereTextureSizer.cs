using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemisphereTextureSizer : MonoBehaviour
{
	public Vector2 textureSize = new Vector2(1f, 1f);
	

	// Update is called once per frame
	void Update()
	{
		Vector2 invertedSize = new Vector2(5f / textureSize.x - 0.5f, 5f / textureSize.y - 0.5f);

		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTextureScale = invertedSize;
		renderer.material.mainTextureOffset = invertedSize * new Vector2(-0.5f, -0.5f) + new Vector2(0.5f, 0.5f);
	}
}
