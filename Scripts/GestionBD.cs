using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GestionBD : MonoBehaviour {

    public InputField txtUsuario;
    public InputField txtContraseña;

    public string nombreUsuario;
    //  public int idUsuario;
    //  public int logUsuario;
    public int scoreUsuario;

    public string archivoUsuario;

    public bool sesionIniciada;

    public static GestionBD single;

    public static bool offline;

    private void Awake()
    {
        if (single == null) {

            single = this;

        } else {
            Destroy(gameObject);
        }
        
        if(Application.internetReachability != NetworkReachability.NotReachable){
            Debug.Log("Conectado");
        }else{
            Debug.Log("No conectado");
            //desabilitar inicio y registro porque no hay conexión a internet
            //mostrar una ventana o algo asi
            GameObject.Find("InputUsuario").GetComponent<InputField>().interactable = false;
            GameObject.Find("InputContraseña").GetComponent<InputField>().interactable = false;
            GameObject.Find("ButtonIniciar").GetComponent<Button>().interactable = false;
            GameObject.Find("ButtonRegistrar").GetComponent<Button>().interactable = false;
        }

        offline = false;
    }


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

    }

    public void IniciarSesion()
    {
        StartCoroutine(Login());
        
    }

    public void RegistrarUsuario()
    {
        StartCoroutine(Registrar());
    }

    //boton offline
    public void ModoOffline(){
        StartCoroutine(CargarEscena());
        
        //SceneManager.LoadScene("tablero");
        offline = true;
    }
    private IEnumerator CargarEscena(){
        //GameObject go = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/cargando")) as GameObject;
        //go.transform.SetParent(GameObject.Find("Canvas").transform,false);
        AsyncOperation carga = SceneManager.LoadSceneAsync("tablero");
        while(!carga.isDone){
            Debug.Log(carga.progress);
            ShowMessage(7);
            yield return null;
        }
        //Destroy(go);
        //instanciar video de intro
        GameObject video = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/IntroVideo")) as GameObject;
        video.transform.SetParent(GameObject.Find("Canvas").transform,false);
        yield break;
    }

/*
    public void Score_Actualizar(int nScore)
    {
        StartCoroutine(ActualizarBD(nScore));
    }
*/
   

    IEnumerator Login()
    {
        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/login.php?uss=" + txtUsuario.text + "&pss=" + txtContraseña.text); 
        yield return (connect);
        Debug.Log(connect);
        if (connect.text == "good") // El usuario existe en la base de datos
        {
            print("El usuario si existe"); 
            StartCoroutine(Datos());

        }
        else if (connect.text == "Ingrese Usuario y Contraseña correctamente")
        {
           // print("Ingrese Usuario y Contraseña correctamente para Iniciar");
            ShowMessage(1);      
                                
        }
        else if (connect.text == "wronguser") // Usuario no está registrado
        {          
            ShowMessage(2);
            
        }
        else if (connect.text == "wrongpass") // Usuario no está registrado
        {        
            ShowMessage(3);

        }
        else // Error en la conexión con la BD
        {
            print("Error en la conexión con la BD");

        }
    }

    IEnumerator Datos()
    {
        //WWW connect = new WWW("http://localhost/test/datos.php?uss=" + txtUsuario.text);
        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/datos.php?uss=" + txtUsuario.text);
        yield return (connect);

              
        if (connect.text == "wrong"){
            print("Usuario incorrecto");

        }else{

            string[] nDatos = connect.text.Split('|');

            if (nDatos.Length != 2)
            {
                print("Error en la conexión con la Datos");

            }else{      
                
                // Revisa los datos de la BD

                nombreUsuario = nDatos[0];
                scoreUsuario = int.Parse(nDatos[1]);
              
                sesionIniciada = true;               

                //SceneManager.LoadScene("tablero");
                StartCoroutine(CargarEscena());

            }

        }

    }

    IEnumerator Registrar()
    {
        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/registro.php?uss=" + txtUsuario.text + "&pss=" + txtContraseña.text);
        // WWW connect = new WWW("http://localhost/test/registro.php?uss=" + txtUsuario.text + "&pss=" + txtContraseña.text);
        yield return (connect);

        if (connect.text == "El usuario ya existe") // El usuario ya existe
        {
            // Debug.LogError("El usuario ya existe");

            ShowMessage(4);     
           
        }else if (connect.text == "Usuario Registrado") // Se registra el usuario
        {
            nombreUsuario = txtUsuario.text;
        //  scoreUsuario = 0;
            sesionIniciada = true;

            ShowMessage(5);

        }else if (connect.text == "Ingrese Usuario y Contraseña correctamente") // Usuario o contraseña vacía
        {
            print("Ingrese Usuario y Contraseña correctamente para Registrar");
            ShowMessage(6);
            
        }else
        {
            Debug.LogError("Error en la conexión con la BD reg");
        }

    }

/*
    IEnumerator ActualizarBD(int nScore)
    {
        WWW connect = new WWW("http://localhost/test/update.php?uss=" + txtUsuario.text + "&nScore=" + nScore.ToString());
        yield return (connect);

        if (connect.text == "wrong")
        {
            Debug.Log("Usuario no existe!");

        }
        else if (connect.text == "Score Actualizado")
        {
            print("Datos actualizados correctamente");        
            scoreUsuario = nScore;
            
        }
        else
        {
            print(connect.text);
            Debug.Log("Error en la conexión con la BD actdat");

        }

    }

*/

    void ShowMessage(int num)
    {
        if(num == 1)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "Ingrese Usuario y Contraseña correctamente para Iniciar";
        }

        if (num == 2)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "Usuario o Contraseña incorrecta";
            //GameObject.Find("TextMessage").GetComponent<Text>().text = "El usuario no está registrado";
        }

        if (num == 3)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "La contraseña es incorrecta";
        }

        if (num == 4)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "El usuario ya existe";
        }
             
        if (num == 5)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "Usuario registrado correctamente";
        }

        if (num == 6)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = "Ingrese Usuario y Contraseña correctamente para Registrar";
        }

        if (num == 7)
        {
            GameObject.Find("TextMessage").GetComponent<Text>().text = " Iniciando Xplora...";
        }

        GameObject.Find("TextMessage").GetComponent<CanvasGroup>().alpha = 1;
        
    }


}
