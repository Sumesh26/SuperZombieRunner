using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {
	public Vector2 speed=Vector2.zero;
	//public Renderer rend;
	private Vector2 offset = Vector2.zero;
	private Material material;
	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;
		//rend = GetComponent<Renderer> ();
		offset=material.GetTextureOffset ("_MainTex");
	}
	
	// Update is called once per frame
	void Update () {
		offset += speed * Time.deltaTime;
		//rend.material.SetTextureOffset ("_MainTex",new Vector2( offset,0));
		material.SetTextureOffset ("_MainTex",offset);
	}
}
