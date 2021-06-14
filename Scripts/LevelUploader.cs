using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using UnityEngine.UI;
 
 
public class LevelUploader : MonoBehaviour
{
	void Start(){
		/* GameObject.Find("UploadBtn").GetComponent<Button>().onClick.AddListener(()=>{
            StartUpload();
            Debug.Log("Upload");
        }); */
	}
    public void StartUpload(string fileName)
    {
        StartCoroutine(UploadLevel(fileName));
    }
   
    IEnumerator UploadLevel(string fileName)  
    {
        fileName = fileName + ".xplora2";
        StreamReader entrada = new StreamReader(Application.persistentDataPath+"\\MapasGuardados\\" + GestionBD.single.nombreUsuario + "\\" + fileName);
       
        //convertir el archivo a bytes para subirlo
        byte[] levelData =Encoding.UTF8.GetBytes(entrada.ReadToEnd());
       
        WWWForm form = new WWWForm();
       
        form.AddField("action", "level upload");
 
        form.AddField("file","file");
 
        form.AddBinaryData ( "file", levelData, fileName,"text/xml");
 
        print("binary data added ");
        //php en el servidor que controla la subida
    
        WWW w = new WWW("http://xplora2-0.000webhostapp.com/LevelUpload.php?uss=" + GestionBD.single.nombreUsuario, form);
        
        yield return w;
        print(w.text);

        print("after yield w");
        if (w.error != null)
        {
            print("error");
            print ( w.error );    
        }
        else
        {
            //this part validates the upload, by waiting 5 seconds then trying to retrieve it from the web
            if(w.uploadProgress == 1 || w.isDone)
            {
                yield return new WaitForSeconds(5);
                //ruta de la carpeta raiz donde se guardan los archivos
                WWW w2 = new WWW("http://xplora2-0.000webhostapp.com/MapasGuardados/" + GestionBD.single.nombreUsuario + "/" + fileName);
                yield return w2;
                if(w2.error != null)
                {
                    print("error 2");
                    print ( w2.error );  
                }
                else
                {
                    //then if the retrieval was successful, validate its content to ensure the level file integrity is intact
                    if(w2.text != null ||  w2.text != "")
                    {
                        print ( "Level File " + fileName + " Contents are: \n\n" + w2.text);
                        /* if(w2.text.Contains("<level>") && w2.text.Contains("</level>"))
                        {
                            //and finally announce that everything went well
                            print ( "Level File " + fileName + " Contents are: \n\n" + w2.text);
                            print ( "Finished Uploading Level " + fileName);
                        }
                        else
                        {
                            print ( "Level File " + fileName + " is Invalid");
                        } */
                    }
                    else
                    {
                        print ( "Level File " + fileName + " is Empty");
                    }
                }
            }      
        }
    }
       
}