using UnityEngine;
using System.Collections;

public class ghostAI : MonoBehaviour {
	GameObject player;
	float speed;
	SpriteRenderer SR;
	[HideInInspector] public Animator anim;
	[HideInInspector] public float distance;
	AudioSource audioSrc;
	bool oneStart = false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		speed = Random.Range(0.5f,2f);
		SR = this.gameObject.GetComponent<SpriteRenderer>();
		anim = this.gameObject.GetComponent<Animator>();
		audioSrc = this.gameObject.GetComponent<AudioSource>();
		if(player != null) startInfo();
	}
	
	// Update is called once per frame
	void Update () {
		if(!oneStart){
			player = GameObject.FindGameObjectWithTag("Player");
			if(player != null) startInfo();
		}
		else{
			distance = Vector3.Distance(player.transform.position,this.transform.position);
			//print(distance);
			if(distance < 12){
				moveToPlayer();
				if(distance < 3 && !anim.GetBool("dead")){
					if(!audioSrc.isPlaying)audioSrc.Play();
				}
				else if(audioSrc.isPlaying)audioSrc.Pause();
			}
			if(SR.color.a == 0) anim.SetBool("dead", false);
			else anim.SetBool("dead", true);
		}
	}

	void startInfo(){
		oneStart = true;
		player.GetComponent<PlayerController>().ghosts.Add(this);
		audioSrc.volume = player.GetComponent<PlayerController>().getVolume();
	}

	void moveToPlayer(){
		if(!anim.GetBool("dead")){
			transform.position = Vector3.MoveTowards(this.transform.position,player.transform.position,Time.deltaTime*speed);
		}
	}
}
