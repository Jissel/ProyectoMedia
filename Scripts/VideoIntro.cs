using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VideoIntro : MonoBehaviour {

	string URL;
	public Text progreso;
	bool streamReady;
	AudioSource sonido;
	public VideoSource video;
	public VideoPlayer videoPlayer;
	public RawImage image;
	public GameObject btnPlay;
	public bool cargado;
	bool play;
	public VideoClip videoGuardado;
	public GameObject loading;
	public Sprite imgPlay;
	public Sprite imgPause;
	public Sprite imgMute;
	public Sprite imgVol;
	public Sprite imgVolMed;
	public Sprite imgRestart;
	Sprite spAnt;
	public GameObject btnVol;
	bool mute;
	float volAnt;
	bool mid;
	public GameObject controles;
	Animator controlAnim;
	GameObject playScreen;
	bool cargando;
	bool final;

	// Use this for initialization
	void Start () {
		streamReady = false;
		//progreso.text = "";
		image = gameObject.GetComponent<RawImage>();
		cargado = false;
		play = false;
		mute = false;
		mid = false;
		cargando = false;
		final = false;

		URL = Application.dataPath + "/Resources/Videos/intro.mp4";

		//listener();
		GameObject.Find("BtnSaltarIntro").GetComponent<Button>().onClick.AddListener(()=>{
			print("Cerrar video");
			Destroy(this.gameObject);
			Destroy(GameObject.Find("cargando(Clone)"));
			Destroy(GameObject.Find("BtnSaltarIntro"));
		});
		StartCoroutine(StartStream(@URL));
	}

	public void SetURL(string url){
		//asignar el video a reproducir
		this.URL = url;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space") && !streamReady){
			StartCoroutine(StartStream(@URL));
		}
		/* if(cargado && videoPlayer.frame == 0){
			btnPlay.GetComponent<Image>().sprite = imgPlay;
		} */

		if(cargado){ //sprites volumen
			if(sonido.volume == 0f){
				btnVol.GetComponent<Image>().sprite = imgMute;
				mute = true;
			}else if(mute && (sonido.volume != 0)){
				mute = false;
				btnVol.GetComponent<Image>().sprite = imgVol;
			}else
			if(sonido.volume < 0.5 && !mid){
				mid = true;
				btnVol.GetComponent<Image>().sprite = imgVolMed;
			}else if(sonido.volume > 0.5 && mid){
				mid = false;
				btnVol.GetComponent<Image>().sprite = imgVol;
			}
		}
	}

	void listener(){
		GameObject.Find("BtnSaltarIntro").GetComponent<Button>().onClick.AddListener(()=>{
			print("Cerrar video");
			Destroy(this.gameObject);
		});
		//Play/Pausa
		btnPlay.GetComponent<Button>().onClick.AddListener(()=>{
			if(!play && cargado){ //play
				videoPlayer.Play();
				sonido.Play();
				play = true;
				btnPlay.GetComponent<Image>().sprite = imgPause;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 0;
				final = false;
				//controlAnim.SetTrigger("hide");
				Debug.Log("caso 1");
			}else if(play && cargado){ //pause
				videoPlayer.Pause();
				sonido.Pause();
				play = false;
				btnPlay.GetComponent<Image>().sprite = imgPlay;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 1;
				Debug.Log("caso 2");
			}else if(!cargado && !cargando){ //carga
				btnPlay.GetComponent<Image>().sprite = imgPause;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 0;
				//controlAnim.SetTrigger("hide");
				//Application.runInBackground = true;
				StartCoroutine(StartStream(@URL));
			}
		});
		//Volumen
		btnVol.GetComponent<Button>().onClick.AddListener(()=>{
			if(cargado){
				if(!mute){
					//Si se le da al boton de volumen, se desactiva el audio, se cambia el icono, se baja el slider
					//se guarda el volumen que tenia
					volAnt = sonido.volume;
					spAnt = btnVol.GetComponent<Image>().sprite;
					sonido.volume = 0f;
					GameObject.Find("Slider").GetComponent<Slider>().value = 0f;
					btnVol.GetComponent<Image>().sprite = imgMute;
					mute = true;
				}else{
					//esta en mute y se coloca en volumen
					sonido.volume = volAnt;
					btnVol.GetComponent<Image>().sprite = spAnt;
					GameObject.Find("Slider").GetComponent<Slider>().value = volAnt;
					btnVol.GetComponent<Image>().sprite = imgVol;
					mute = false;
				}
			}
		});
		
		//Play pausa en pantalla
		playScreen.GetComponent<Button>().onClick.AddListener(()=>{
			if(!play && cargado){ //play
				videoPlayer.Play();
				sonido.Play();
				play = true;
				btnPlay.GetComponent<Image>().sprite = imgPause;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 0;
				controlAnim.SetTrigger("hide");
				//controlAnim.SetTrigger("hide");
			}else if(play && cargado){ //pause
				//StartCoroutine(WaitScreen());
				videoPlayer.Pause();
				sonido.Pause();
				play = false;
				btnPlay.GetComponent<Image>().sprite = imgPlay;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 1;
				controlAnim.SetTrigger("show");
			}else if(!cargado && !cargando){ //carga
				btnPlay.GetComponent<Image>().sprite = imgPause;
				GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 0;
				controlAnim.SetTrigger("hide");
				StartCoroutine(StartStream(@URL));
			}
		});
	}//flistener

	IEnumerator StartStream(string url){
		//WWW videoStreamer = new WWW (url);
		videoPlayer = gameObject.GetComponent<VideoPlayer>();
		sonido = gameObject.GetComponent<AudioSource>();

		//inicializar
		videoPlayer.playOnAwake = false;
		sonido.playOnAwake = false;
		sonido.Pause();
		/* videoPlayer.source = VideoSource.Url;
		videoPlayer.url = url; */
		videoPlayer.source = VideoSource.VideoClip;

		//audio
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//audio del video a audiosource
		videoPlayer.controlledAudioTrackCount = 1;
		videoPlayer.EnableAudioTrack(0,true);
		videoPlayer.SetTargetAudioSource(0, sonido);

		//guardado
		videoPlayer.clip = videoGuardado;

		//prepara el video
		videoPlayer.Prepare();

		//espera
		GameObject load = Instantiate(loading) as GameObject;
		load.transform.SetParent(GameObject.Find("Canvas").transform,false);
		while(!videoPlayer.isPrepared){
			cargando = true;
			Debug.Log("cargando...");
			yield return new WaitForSeconds(5);
			//break;
		}
		Destroy(load);
		Debug.Log("ready");
		cargado = true;

		//imagen
		image.texture = videoPlayer.texture;

		//play
		videoPlayer.Play();
		sonido.Play();
		Debug.Log("Play");
		play = true;

		while(videoPlayer.isPlaying){
			//Debug.Log("VideoTime: "+ Mathf.FloorToInt((float)videoPlayer.time));
			//Debug.Log((int) videoPlayer.time);
			//GameObject.Find("Tiempo").GetComponent<Text>().text = ((int) videoPlayer.time).ToString();
			yield return 0;
		}
		if(videoPlayer.frame == (long)videoPlayer.frameCount){
			Debug.Log("final");
			Destroy(this.gameObject);
            Destroy(GameObject.Find("BtnSaltarIntro"));
            /* 	btnPlay.GetComponent<Image>().sprite = imgRestart;
                GameObject.Find("ImgPlayScreen").GetComponent<CanvasGroup>().alpha = 1;
                controlAnim.SetTrigger("show"); */
            play = false;
		}
	}//fStartStream

	IEnumerator WaitScreen(){
		int clicks = 1;

		//tengo que validar cuando se presiona por primera vez y cuando por segunda, cuando sale el icono de pausa y que se
		//muestren los controles cuando se le hace pausa

		WaitForSeconds espera = new WaitForSeconds(3);
		while(clicks == 1){
			playScreen.GetComponent<Button>().onClick.AddListener(()=>{
				clicks += 1;
			});
			yield return espera;
		}
		//Debug.Log("Sale while "+clicks);
		if(clicks == 1){
			//oculta el panel de nuevo
			Debug.Log("1click");
		}else if(clicks == 2){
			//coloca en pausa
			Debug.Log("2click");
		}
	}//fWaitScreen

	public void SetVolume(float volume){
		if(cargado){
			sonido.volume = volume;
		}
	}//fSetVolume
}