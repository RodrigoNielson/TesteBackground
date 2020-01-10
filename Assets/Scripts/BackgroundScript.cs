using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptsUteis;
using UnityEngine.U2D;

public class BackgroundScript : MonoBehaviour
{
    public GameObject[] chaoPrefabs;

    private GameObject[] objetosChao;
    private int qtdChao;
    private int indiceObjAtual = 0;
    private float maiorXUltimoChao;
    private float raioCamera;
    private float px = 0;
    private float py = 0;

    // Start is called before the first frame update
    void Start()
    {
        objetosChao = new GameObject[chaoPrefabs.Length];

        qtdChao = objetosChao.Length;
        for (int i = 0; i < qtdChao; i++)
        {
            var instanciar = Random.Range(0, chaoPrefabs.Length);
            objetosChao[i] = Instantiate(chaoPrefabs[instanciar], Vector2.zero, Quaternion.identity);


            var ssc = objetosChao[i].GetComponent<SpriteShapeController>();
            ssc.spline.isOpenEnded = false;
            var p0 = ssc.spline.GetPosition(0);
            var pl = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
            //ssc.spline.InsertPointAt(0, new Vector2(p0.x, p0.y - 0.1f)); // gambi
            //ssc.spline.SetTangentMode(0, ShapeTangentMode.Continuous);
            ssc.spline.InsertPointAt(0, new Vector2(p0.x, p0.y - 100));
            ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), new Vector2(pl.x, p0.y - 100));
            ssc.RefreshSpriteShape();

        }

        PreencherBoundCamera();
        InicializarChao();
    }

    private void PreencherBoundCamera()
    {
        raioCamera = objetosChao.Max((obj1) =>
        {
            var colliderObj1 = obj1.GetComponent<EdgeCollider2D>();
            return colliderObj1.MenorX();
        });
    }

    private void InicializarChao()
    {
        Vector2[] transforms = new Vector2[qtdChao];

        var indiceObjAtualInicializa = 0;

        float pxI;
        float pyI;

        for (int i = 0; i < qtdChao - 1; i++)
        {
            if (i + 1 > qtdChao)
                break;

            var indiceProximo = i + 1;

            var colliderObjAtual = objetosChao[indiceObjAtualInicializa].GetComponent<EdgeCollider2D>();
            var colliderObjProx = objetosChao[indiceProximo].GetComponent<EdgeCollider2D>();

            pxI = colliderObjAtual.MaiorX() + (objetosChao[indiceProximo].transform.position.x - colliderObjProx.MenorX());

            var menorYObjetoAtual = colliderObjAtual.points.LastOrDefault().y;

            var maiorYProximoObjetoSemTransformPositionY = colliderObjProx.points.FirstOrDefault().y;

            pyI = menorYObjetoAtual - maiorYProximoObjetoSemTransformPositionY;
            var pxpyAtual = transforms[indiceObjAtualInicializa];

            pxI = pxpyAtual.x + pxI;
            pyI = pxpyAtual.y + pyI;

            transforms[indiceProximo] = new Vector2(pxI, pyI);

            objetosChao[indiceProximo].transform.position = new Vector2(pxI, pyI);

            indiceObjAtualInicializa++;
            if (indiceObjAtualInicializa == qtdChao)
                indiceObjAtualInicializa = 0;
        }

        maiorXUltimoChao = transforms.LastOrDefault().x;
    }

    private void FixedUpdate()
    {
        var boundXCamera = Camera.main.transform.position.x + raioCamera;

        if (DeveReposicionarChao(boundXCamera))
            ReposicionarChao();
    }

    private bool DeveReposicionarChao(float boundXCamera) => boundXCamera >= maiorXUltimoChao;

    private void ReposicionarChao()
    {
        var colliderObjAtual = objetosChao[indiceObjAtual].GetComponent<EdgeCollider2D>();
        var indiceProximoObjeto = indiceObjAtual == qtdChao - 1 ? 0 : indiceObjAtual + 1;
        var colliderObjProx = objetosChao[indiceProximoObjeto].GetComponent<EdgeCollider2D>();

        px = colliderObjAtual.MaiorX() + (objetosChao[indiceProximoObjeto].transform.position.x - colliderObjProx.MenorX());

        //var ultimoYObjAtual = colliderObjAtual.points.LastOrDefault().y;

        //var primeiroYProximoObj = colliderObjProx.points.FirstOrDefault().y;

        //var ultimoYObjAtual = colliderObjAtual.points.ElementAt(colliderObjAtual.pointCount-2).y;
        
        var ssc = objetosChao[indiceObjAtual].GetComponent<SpriteShapeController>();
        var ultimoYObjAtual = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 2).y;

        //var primeiroYProximoObj = colliderObjProx.points.ElementAt(1).y;
        var ssc2  = objetosChao[indiceProximoObjeto].GetComponent<SpriteShapeController>();
        var primeiroYProximoObj = ssc2.spline.GetPosition(1).y;

        //var p1 = ssc.spline.GetPosition(1);

        //ssc2.spline.SetPosition(1, p1);
        //ssc2.RefreshSpriteShape();
        ////ssc2.BakeCollider();
        //ssc2.BakeMesh();

        var primeiroYProximoObjComTransformAtual = primeiroYProximoObj - objetosChao[indiceObjAtual].transform.position.y;

        py = ultimoYObjAtual - primeiroYProximoObjComTransformAtual;

        //maiorXUltimoChao = colliderObjAtual.MaiorX();
        maiorXUltimoChao = objetosChao[indiceObjAtual].transform.position.x;

        var indiceProximo = indiceObjAtual == qtdChao - 1 ? 0 : indiceObjAtual + 1;

        objetosChao[indiceProximo].transform.position = new Vector2(px, py);

        indiceObjAtual++;
        if (indiceObjAtual == qtdChao)
            indiceObjAtual = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

