using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public GameObject chaoPrefab;
    //public Camera camera;

    private GameObject[] objetosChao;
    private Vector2 objectPoolPosition = new Vector2(0f, 0f);
    private int max = 4;
    private int indiceObjAtual = 0;
    private float objAtualPos;
    private GameObject bola;
    private float numeroMagico = 7.138808f;

    private float px;
    private float py;

    // Start is called before the first frame update
    void Start()
    {
        bola = this.gameObject;


        Debug.Log(Camera.main.fieldOfView);


        objetosChao = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            objetosChao[i] = Instantiate(chaoPrefab, objectPoolPosition, Quaternion.identity);
        }

        // seta posicao inicial dos chaos
        objetosChao[0].transform.position = new Vector2(0, 0);
        Inicializa();
    }

    private void Inicializa()
    {
        var colliderObj0 = objetosChao[indiceObjAtual].GetComponent<EdgeCollider2D>();

        var ultimoPontoObj0 = colliderObj0.points.LastOrDefault();
        var boundsObj01 = colliderObj0.bounds;

        var tamxObj1 = objetosChao[indiceObjAtual + 1].GetComponent<EdgeCollider2D>().bounds.extents.x;
        var tamyObj1 = objetosChao[indiceObjAtual + 1].GetComponent<EdgeCollider2D>().bounds.extents.y;

        px = ultimoPontoObj0.x + -boundsObj01.center.x + tamxObj1;
        py = ultimoPontoObj0.y - boundsObj01.center.y - tamyObj1;


        objetosChao[1].transform.position = new Vector2(px, py);

        objAtualPos = objetosChao[indiceObjAtual].GetComponent<EdgeCollider2D>().points.LastOrDefault().x;

        indiceObjAtual++;
        if (indiceObjAtual == 4)
            indiceObjAtual = 0;
    }

    private bool vai = true;

    private void FixedUpdate()
    {
        var boundXCamera = Camera.main.transform.position.x + numeroMagico;

        if (boundXCamera >= objAtualPos)
        {

            if (vai)
            {
                SpawnaNovo();

                vai = true;
            }
        }
    }

    private void SpawnaNovo()
    {
        var indiceProximo = indiceObjAtual == 3 ? 0 : indiceObjAtual + 1;

        var posAtual = objetosChao[indiceObjAtual].transform.position;

        objetosChao[indiceProximo].transform.position = new Vector2(posAtual.x + px, py + posAtual.y);

        objAtualPos = objetosChao[indiceObjAtual].GetComponent<EdgeCollider2D>().points.LastOrDefault().x + posAtual.x;

        indiceObjAtual++;
        if (indiceObjAtual == 4)
            indiceObjAtual = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
