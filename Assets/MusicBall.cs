using UnityEngine;
using System.Collections;

public class MusicBall : MonoBehaviour {

	private bool isPlaying =false;
	public GameObject Player;

	public GameObject camera;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		float dist = Vector3.Distance(Player.transform.position, transform.position);
		if (dist <= 2f) 
		{
			gameObject.renderer.material.color = Color.blue;
			camera.GetComponent<Visualize>().start = true;
		}
	}

	void OnTriggerEnter(Collider Other)
	{
		if (!isPlaying) 
		{
			if (Other.collider.tag == "Player")
			{
				//audio.Play ();
				Debug.Log("Touched");


			}
			isPlaying =true;
		}
	}
}
