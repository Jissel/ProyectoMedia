using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics : MonoBehaviour {

	public float[] valores;
	public Color[] colores;
	public Image grafPrefab;
	int cantSociedades;
	ControllerBoard tablero;
	SociedadesController socContr;
	List<Color> coloresComp;
	

	// Use this for initialization
	void Start () {
		valores = new float[3];
		colores = new Color[3];
		colores[0] = new Color32(255,117,117,255);
		colores[1] = new Color32(255,246,143,255);
        colores[2] = new Color32(93, 173, 226 ,255);

		cantSociedades = GameObject.Find("Code").GetComponent<SociedadesController>().cantSociedades;
		tablero = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
		socContr = GameObject.Find("Code").GetComponent<SociedadesController>();
		coloresComp = new List<Color>();
		ColoresComponentes();
    }


	public void Graficar(){
		//grafica las estadisticas en el panel
        float total = 0f;
		float zRotation = 0f;


		//sacar el total de componentes por sociedad
		for(int i = 0; i < socContr.cantSociedades; i++){
			int iterador = i;
			total += tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == iterador).Count;
		}

		if(total > 0){
			for(int i = 0; i < socContr.cantSociedades; i++){
				Image grafNuevo = Instantiate (grafPrefab) as Image;
				grafNuevo.transform.SetParent(transform, false);
				grafNuevo.color = socContr.SociedadDict[i].GetColor();
				grafNuevo.fillAmount = Convert.ToSingle(tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == i).Count) / total;
				GameObject.Find("cantGranja" + i).GetComponent<Text>().text ="<b>"+ GameObject.Find("cantGranja" + i).GetComponent<Text>().text  +"</b>  ("+ (grafNuevo.fillAmount*100).ToString("0.0") +"%)";
				//print(grafNuevo.fillAmount);
				grafNuevo.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,zRotation));
				zRotation -= grafNuevo.fillAmount * 360f;
			}
		}
	}//fGraficar

	public void GraficarSociedad(int tipoSociedad){
		//grafica las estadisticas en el panel
        float total = 0f;
		float zRotation = 0f;
		List<Piece> ListaCompSociedad = tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == tipoSociedad);


		GameObject.Find("TituloStats").GetComponent<Text>().text = "Estadísticas "+socContr.SociedadDict[tipoSociedad].GetName();


		//sacar el total de componentes por sociedad
		for(int i = 0; i < ListaCompSociedad.Count; i++){
			int iterador = i;
			total += ListaCompSociedad[i].totalComp;
		}

		int cantComponentes = 9;

		if(total > 0){
			for(int i = 0; i < cantComponentes; i++){
				GameObject.Find("leyendaComp"+i).GetComponent<Image>().color = coloresComp[i];
				GameObject.Find("tComp"+i).GetComponent<Text>().text = getIDbyNumber(i);
				if(ListaCompSociedad.Find(pc => pc.getIDNumber() == i)){
					Image grafNuevo = Instantiate (grafPrefab) as Image;
					grafNuevo.transform.SetParent(transform, false);
					grafNuevo.color = coloresComp[i];
					Piece p = ListaCompSociedad.Find(pc => pc.getIDNumber() == i);
					print(p.getIDPiece());
					grafNuevo.fillAmount = Convert.ToSingle(p.totalComp) / total;
					grafNuevo.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,zRotation));
					zRotation -= grafNuevo.fillAmount * 360f;
					GameObject.Find("cantComponente"+i).GetComponent<Text>().text ="<b>"+ p.totalComp.ToString() +"</b>  ("+ (grafNuevo.fillAmount*100).ToString("0.0") +"%)";
				}else{
					GameObject.Find("cantComponente"+i).GetComponent<Text>().text = "0";
				}			
				//print(grafNuevo.fillAmount);
				
			}
		}
	}//fGraficarSociedad

	public void DestroyGraph(){
		for(int i = 0; i < GameObject.Find("Grafico").transform.childCount; i++){
			GameObject g = GameObject.Find("Grafico").transform.GetChild(i).gameObject;
			Destroy(g);
		}
	}//fDestroyGraph
	private void ColoresComponentes(){
		coloresComp.Add(new Color(0.9882353f,0.3607843f,0.3960784f));
		coloresComp.Add(new Color(0.658f,0.901f,1f));
		coloresComp.Add(new Color(0f,1f, 0.364f));
		coloresComp.Add(new Color(0.05882353f,0.7254902f,0.6941177f));
		coloresComp.Add(new Color(0.6470588f,0.3686275f,0.9176471f));
		coloresComp.Add(new Color(0.960f,0.274f, 0.984f));
		coloresComp.Add(new Color(0.372f,0.152f,0.803f));
		coloresComp.Add(new Color(0.980f, 0.694f,0.627f));
		coloresComp.Add(new Color(1f,0.949f,0f));
	}
	 public string getIDbyNumber(int num){
        string index = "";
        switch(num){
            case 0:
                index= "Huevo Fertil";
            break;
            case 1:
                index= "Cria y Levante";
            break;
            case 2:
                index= "Planta ABA";
            break;
            case 3:
                index = "Incubadora";
            break;
            case 4:
                index = "Ponedoras";
            break;
            case 5:
                index = "Engorde";
            break;
            case 6:
                index = "Soporte";
            break;
            case 7:
                index = "Mantenimiento";
            break;
            case 8:
                index = "Beneficio";
            break;
        }
        return index;
    }
}
