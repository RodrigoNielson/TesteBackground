using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptsUteis;

public class BackgroundScript : MonoBehaviour
{
    public GameObject chaoPrefab1;
    public GameObject[] chaoPrefabs;
    public int max = 10;

    //public Camera camera;

    private GameObject[] objetosChao;

    private int indiceObjAtual = 0;

    private float objAtualPos;

    private GameObject bola;
    private float numeroMagico = 7.138808f;

    private float px = 0;
    private float py = 0;

    // Start is called before the first frame update
    void Start()
    {
        bola = this.gameObject;


        Debug.Log(Camera.main.fieldOfView);


        objetosChao = new GameObject[max];

        for (int i = 0; i < max; i++)
        {
            var instanciar = Random.Range(0, 4);
            objetosChao[i] = Instantiate(chaoPrefabs[instanciar], Vector2.zero, Quaternion.identity);
        }

        Inicializa();
    }

    private void Inicializa()
    {
        Vector2[] transforms = new Vector2[max];

        var indiceObjAtualInicializa = 0;
        float pxI = 0;
        float pyI = 0;

        for (int i = 0; i < max -1; i++)
        {
            var objAtual = objetosChao[i];
            if (i + 1 > max)
            {
                break;
            }
            var indiceProximo = i + 1;

            // TODO: fazer ajuste no menor Y, nao pode usar ele, tem que usar o ULTIMO ponto Y

            var colliderObjAtual = objetosChao[indiceObjAtualInicializa].GetComponent<EdgeCollider2D>();
            var colliderObjProx = objetosChao[indiceProximo].GetComponent<EdgeCollider2D>();

            pxI = colliderObjAtual.MaiorX() + (objetosChao[indiceProximo].transform.position.x - colliderObjProx.MenorX());

            var menorYObjetoAtual = colliderObjAtual.MenorY();
            var maiorYProximoObjetoSemTransformPositionY = colliderObjProx.MaiorY();

            pyI = menorYObjetoAtual - maiorYProximoObjetoSemTransformPositionY;
            var pxpyAtual = transforms[indiceObjAtualInicializa];

            pxI = pxpyAtual.x + pxI;
            pyI = pxpyAtual.y + pyI;

            transforms[indiceProximo] = new Vector2(pxI, pyI);

            objetosChao[indiceProximo].transform.position = new Vector2(pxI, pyI);

            indiceObjAtualInicializa++;
            if (indiceObjAtualInicializa == max)
                indiceObjAtualInicializa = 0;
        }

        objAtualPos = transforms.LastOrDefault().x;
    }

    private bool vai = true;



    private void FixedUpdate()
    {
        var boundXCamera = Camera.main.transform.position.x + numeroMagico;

        if (boundXCamera >= objAtualPos)
        {

            if (vai)
            {
                //RecalculaPxPy();
                SpawnaNovo();

                vai = true;
            }
        }
    }

    private void SpawnaNovo()
    {
        var colliderObjAtual = objetosChao[indiceObjAtual].GetComponent<EdgeCollider2D>();
        var indiceProximoObjeto = indiceObjAtual == max - 1 ? 0 : indiceObjAtual + 1;
        var colliderObjProx = objetosChao[indiceProximoObjeto].GetComponent<EdgeCollider2D>();

        px = colliderObjAtual.MaiorX() + (objetosChao[indiceProximoObjeto].transform.position.x - colliderObjProx.MenorX());

        var menorYObjetoAtual = colliderObjAtual.MenorY();
        var maiorYProximoObjetoSemTransformPositionY = colliderObjProx.MaiorY() - objetosChao[indiceProximoObjeto].transform.position.y;

        py = menorYObjetoAtual - maiorYProximoObjetoSemTransformPositionY;

        objAtualPos = colliderObjAtual.MaiorX();

        var indiceProximo = indiceObjAtual == max - 1 ? 0 : indiceObjAtual + 1;

        objetosChao[indiceProximo].transform.position = new Vector2(px, py);

        indiceObjAtual++;
        if (indiceObjAtual == max)
            indiceObjAtual = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

