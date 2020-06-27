using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class BackGroundScript2 : MonoBehaviour
{
    public GameObject[] chaoBase;
    public GameObject[] prefabsChao;

    private Vector2 ultimoPonto;

    private GameObject chao;

    // Start is called before the first frame update
    void Start()
    {
        chao = Instantiate(chaoBase[0], Vector2.zero, Quaternion.identity);

        var ssc = chao.GetComponent<SpriteShapeController>();

        var chaoPrefab = prefabsChao[0];

        // TODO: isso aqui ajusta os pontos iniciais, vamos deixar aqui por enquanto, nao sei se vai precisar
        //foreach (var chao in prefabsChao)
        //{
        //    var sscChao = chao.GetComponent<SpriteShapeController>();
        //    var pInicial = sscChao.spline.GetPosition(0);

        //    if (pInicial.x == 0)
        //        continue;

        //    if (pInicial.x < 0)
        //    {
        //        var ajuste = 0 - pInicial.x;

        //        ssc.spline.SetPosition(0, pInicial + new Vector3(ajuste, 0));

        //        for (int i = 1; i < sscChao.spline.GetPointCount(); i++)
        //        {
        //            var p = sscChao.spline.GetPosition(i);
        //            ssc.spline.SetPosition(i, p + new Vector3(ajuste, 0));
        //        }
        //    }
        //    else
        //    {
        //        var ajuste = pInicial.x - 0;

        //        ssc.spline.SetPosition(0, pInicial - new Vector3(ajuste, 0));

        //        for (int i = 1; i < sscChao.spline.GetPointCount(); i++)
        //        {
        //            var p = sscChao.spline.GetPosition(i);
        //            ssc.spline.SetPosition(i, p - new Vector3(ajuste, 0));
        //        }
        //    }
        //}

        var sscPrefab = chaoPrefab.GetComponent<SpriteShapeController>();

        var pontos = sscPrefab.spline.GetPointCount();
        string ret = "";
        for (var i = 0; i < pontos; i++)
        {
            ret += sscPrefab.spline.GetPosition(i).ToString();
            ret += sscPrefab.spline.GetLeftTangent(i).ToString();
            ret += sscPrefab.spline.GetRightTangent(i).ToString();
            ret += "\r\n";

        }

        var indice = ssc.spline.GetPointCount() - 1;

        var ultimaPosicaoSpline = ssc.spline.GetPosition(indice);

        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);

        ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);

        SuavizarPrimeiroPontoAdicionado(ssc, indice, Vector2.zero);

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);

            var tangenteEsquerda = sscPrefab.spline.GetLeftTangent(i);
            var tangenteDireita = sscPrefab.spline.GetRightTangent(i);

            ultimoPonto = posicaoComPontoAtual;

            var posicaoInserir = ssc.spline.GetPointCount();

            ssc.spline.InsertPointAt(posicaoInserir, posicaoComPontoAtual);
            ssc.spline.SetTangentMode(posicaoInserir, ShapeTangentMode.Continuous);
            ssc.spline.SetLeftTangent(posicaoInserir, tangenteEsquerda);
            ssc.spline.SetRightTangent(posicaoInserir, tangenteDireita);
        }

        ssc.spline.isOpenEnded = false;
        var primeiroPonto = (Vector2)ssc.spline.GetPosition(0);
        var ultimoPontoA = (Vector2)ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        ssc.spline.InsertPointAt(0, primeiroPonto - new Vector2(0, 30));
        ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), ultimoPontoA - new Vector2(0, 30));
    }

    private void FixedUpdate()
    {
        var cameraTransform = Camera.main.transform.position;
        var boundXCamera = cameraTransform.x + (Camera.main.sensorSize.x * 2);
        if (boundXCamera > ultimoPonto.x)
        {
            SpawnaChao();
            RemovePontos(cameraTransform.x);
        }
    }

    private void RemovePontos(float boundXCamera)
    {
        var camerazx = boundXCamera - Camera.main.sensorSize.x;
        var ssc = chao.GetComponent<SpriteShapeController>();

        for (int i = 1; i < ssc.spline.GetPointCount(); i++)
        {
            if (ssc.spline.GetPosition(i).x < camerazx)
            {
                ssc.spline.RemovePointAt(i);
                i--;
            }
            else
            {
                break;
            }
        }

        var primeiroPonto = (Vector2)ssc.spline.GetPosition(1);
        ssc.spline.SetTangentMode(1, ShapeTangentMode.Broken);
        ssc.spline.SetPosition(0, primeiroPonto - new Vector2(0, 20));
    }

    private void SpawnaChao()
    {
        var ssc = chao.GetComponent<SpriteShapeController>();

        var indicePrefab = Random.Range(0, 4);
        var chaoPrefab = prefabsChao[indicePrefab];

        var sscPrefab = chaoPrefab.GetComponent<SpriteShapeController>();

        ssc.spline.RemovePointAt(ssc.spline.GetPointCount() - 1); // remove ultimo

        var pontos = sscPrefab.spline.GetPointCount();

        var indice = ssc.spline.GetPointCount() - 1;

        var ultimaPosicaoSpline = ssc.spline.GetPosition(indice);


        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);

        var indiceTangenteInicial = ssc.spline.GetPointCount() - 1;

        ssc.spline.SetTangentMode(indiceTangenteInicial, ShapeTangentMode.Continuous);
        SuavizarPrimeiroPontoAdicionado(ssc, indice, sscPrefab.spline.GetPosition(1));

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);

            var tangenteEsquerda = sscPrefab.spline.GetLeftTangent(i);
            var tangenteDireita = sscPrefab.spline.GetRightTangent(i);

            ultimoPonto = posicaoComPontoAtual;

            var posicaoInserir = ssc.spline.GetPointCount();

            ssc.spline.InsertPointAt(posicaoInserir, posicaoComPontoAtual);
            ssc.spline.SetTangentMode(posicaoInserir, ShapeTangentMode.Continuous);
            ssc.spline.SetLeftTangent(posicaoInserir, tangenteEsquerda);
            ssc.spline.SetRightTangent(posicaoInserir, tangenteDireita);
        }

        var ultimoPontoA = (Vector2)ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), ultimoPontoA - new Vector2(0, 30));
        ssc.BakeMesh().Complete();
        // INFO: então, colocando esse bakemesh, nao precisa ficar focado na camera, mas comeca a dar uns erros kKkk kKkk
        // INFO: 27-06-2020 então, assim funciona com o .COmplete() HEHE
    }

    /// <summary>
    /// "Suaviza" um ponto adicionado. Nesses casos, esse ponto é o ultimo, assim ele não tem tangente esquerda nem direita.
    /// Assim não fica parecendo que foram juntados dois sprite shape.
    /// </summary>
    private void SuavizarPrimeiroPontoAdicionado(SpriteShapeController ssc, int indice, Vector2 prox)
    {
        // TODO: No momento, estou fazendo um ponto médio entre esse ponto e o anterior, e adicionando como tangente. Tem algumas falhas, tem que ver pra ficar mais "SUAVE".;
        // TODO: 27-06-2020 o ideal é pegar a posicao real do que sera o proximo ponto e usar o Lerp em cima dele para definir a tangente da direita
        var resLep = Vector2.Lerp(ssc.spline.GetPosition(indice), ssc.spline.GetPosition(indice - 1), 0.5f);
        var diff = resLep - (Vector2)ssc.spline.GetPosition(indice);
        ssc.spline.SetLeftTangent(ssc.spline.GetPointCount() - 1, diff);

        Debug.Log($"Diff {diff.x} prox {prox.x}");
        if (prox != Vector2.zero && prox.x != 0 && Math.Abs(diff.x) > Math.Abs(prox.x))
        {
            if (diff.x < 0)
            {
                diff = new Vector2(-Math.Abs(prox.x) , diff.y);
            }
            else
            {
                diff = new Vector2(Math.Abs(prox.x) , diff.y);

            }
        }

        ssc.spline.SetRightTangent(ssc.spline.GetPointCount() - 1, -diff);
    }
}
