using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	public Text continueText;
	public float blinkTime=0f;
	private bool blink;
	public Text scoreText;
	private float timeElapsed=0f;
	private float bestTime = 0f;
	private bool bestBeatTime;
	private GameObject floor;
	private Spawner spawner;
	public TimeManager timeManager;
	public GameObject playerPrefab;
	private bool gameStarted;
	private GameObject player;
	void Awake()
	{
		floor = GameObject.Find ("Foreground");
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner> ();
		timeManager = GetComponent<TimeManager> ();
	}
	// Use this for initialization
	void Start () {
		var floorHeight = floor.transform.localScale.y;

		var pos = floor.transform.position;
		pos.x = 0;
		pos.y=-((Screen.height/PixelPerfectCamera.pixelToUnits)/2)+(floorHeight/2);
			floor.transform.position=pos;
			spawner.active=false;
		Time.timeScale = 0;
		continueText.text = "PRESS ANY KEY TO START";
		bestTime = PlayerPrefs.GetFloat ("BestTime");
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameStarted && Time.timeScale == 0) {
			if (Input.anyKeyDown) {
				Debug.Log ("click!");
				timeManager.ManipulateTime (1, 1f);
				ResetGame ();
			}
		}
		if (!gameStarted) {
			blinkTime++;
			if (blinkTime % 40 == 0) {
				blink = !blink;
			}
			continueText.canvasRenderer.SetAlpha (blink ? 0 : 1);
			//var textColor = bestBeatTime ? Color.white: Color.yellow;
			scoreText.color=!bestBeatTime ? Color.white: Color.yellow;

			scoreText.text = "TIME: " + FormatTime (timeElapsed) +"\nBEST: " + FormatTime (bestTime); 
		} else {
			scoreText.text = "TIME: " + FormatTime (timeElapsed); 
			timeElapsed += Time.deltaTime;
		}
	}
	void OnPlayerKilled()
	{
		spawner.active = false;
		Debug.Log ("on killed");
		var playerDestroyScript = player.GetComponent<DestroyOffScreen> ();
		playerDestroyScript.DestroyCallback -= OnPlayerKilled;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;


		timeManager.ManipulateTime (0, 5.5f);
		gameStarted = false;
		continueText.text = "PRESS ANY KEY TO RESTART";

		if (timeElapsed > bestTime) {
			bestTime = timeElapsed;
			PlayerPrefs.SetFloat ("BestTime", bestTime);
			bestBeatTime = true;
		}

	}
	void ResetGame()
	{
		spawner.active = true;

		player=GameObjectUtil.Instantiate(playerPrefab,new Vector3(0,(Screen.height/PixelPerfectCamera.pixelToUnits)/2+100,0));
		var playerDestroyScript = player.GetComponent<DestroyOffScreen> ();
		playerDestroyScript.DestroyCallback += OnPlayerKilled;
		gameStarted = true;
		continueText.canvasRenderer.SetAlpha (0);
		timeElapsed = 0f;
		bestBeatTime = false;
	}
	string FormatTime(float value)
	{
		TimeSpan t = TimeSpan.FromSeconds (value);

		return string.Format ("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}
}
