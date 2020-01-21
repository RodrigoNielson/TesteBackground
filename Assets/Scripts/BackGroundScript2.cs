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

        var sscPrefab = chaoPrefab.GetComponent<SpriteShapeController>();

        var pontos = sscPrefab.spline.GetPointCount();
        string ret = "";
        for (var i  = 0; i < pontos; i++)
        {
            ret += sscPrefab.spline.GetPosition(i).ToString();
            ret += sscPrefab.spline.GetLeftTangent(i).ToString();
            ret += sscPrefab.spline.GetRightTangent(i).ToString();
            ret += "\r\n";

        }

        var indice = ssc.spline.GetPointCount() - 1;

        var ultimaPosicaoSpline = ssc.spline.GetPosition(indice);
        //var ultimaTangeteEsquerda = ssc.spline.GetLeftTangent(indice);
        //var ultimaTangeteDireita = ssc.spline.GetRightTangent(indice);

        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);
        //ultimaTangeteEsquerda = new Vector2(ultimaTangeteEsquerda.x - posicao.x, -ultimaTangeteEsquerda.y + posicao.y);
        //ultimaTangeteDireita = new Vector2(ultimaTangeteDireita.x - posicao.x, -ultimaTangeteDireita.y + posicao.y);

        ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var diferencaTangenteEsquerda = sscPrefab.spline.GetLeftTangent(i) - posicao;
            var diferencaTangenteDireita = posicao- sscPrefab.spline.GetRightTangent(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);
            var tangenteEsquerdaComPontoAtual = new Vector2(ultimaPosicaoSpline.x + diferencaTangenteEsquerda.x, -ultimaPosicaoSpline.y + posicao.y);
            var tangenteDireitaComPontoAtual = new Vector2(ultimaPosicaoSpline.x + diferencaTangenteDireita.x, -ultimaPosicaoSpline.y + posicao.y);

            ultimoPonto = posicaoComPontoAtual;

            var posicaoInserir = ssc.spline.GetPointCount();

            ssc.spline.InsertPointAt(posicaoInserir, posicaoComPontoAtual);
            ssc.spline.SetTangentMode(posicaoInserir, ShapeTangentMode.Continuous);
            ssc.spline.SetLeftTangent(posicaoInserir, tangenteEsquerdaComPontoAtual);
            ssc.spline.SetRightTangent(posicaoInserir, tangenteDireitaComPontoAtual);
        }

        ssc.spline.isOpenEnded = false;
        var primeiroPonto = (Vector2)ssc.spline.GetPosition(0);
        var ultimoPontoA = (Vector2)ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        ssc.spline.InsertPointAt(0, primeiroPonto - new Vector2(0, 30));
        ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), ultimoPontoA - new Vector2(0, 30));
    }

    bool up = true;

    private void FixedUpdate()
    {
        var cameraTransform = Camera.main.transform.position;
        var boundXCamera = cameraTransform.x + (Camera.main.sensorSize.x / 2);
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

        //var asiodh = Random.Range(0, 2);
        var chaoPrefab = prefabsChao[0];

        var sscPrefab = chaoPrefab.GetComponent<SpriteShapeController>();

        ssc.spline.RemovePointAt(ssc.spline.GetPointCount() - 1); // remove ultimo

        var pontos = sscPrefab.spline.GetPointCount();

        var indice = ssc.spline.GetPointCount() - 1;

        var ultimaPosicaoSpline = ssc.spline.GetPosition(indice);
        var ultimaTangeteEsquerda = ssc.spline.GetLeftTangent(indice);
        var ultimaTangeteDireita = ssc.spline.GetRightTangent(indice);

        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);
        ultimaTangeteEsquerda = new Vector2(ultimaTangeteEsquerda.x - posicao.x, -ultimaTangeteEsquerda.y + posicao.y);
        ultimaTangeteDireita = new Vector2(ultimaTangeteDireita.x - posicao.x, -ultimaTangeteDireita.y + posicao.y);

        ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);
            var tangenteEsquerdaComPontoAtual = new Vector2(ultimaTangeteEsquerda.x + posicao.x, -ultimaTangeteEsquerda.y + posicao.y);
            var tangenteDireitaComPontoAtual = new Vector2(ultimaTangeteDireita.x + posicao.x, -ultimaTangeteDireita.y + posicao.y);

            ultimoPonto = posicaoComPontoAtual;

            ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), posicaoComPontoAtual);
            ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);
            //ssc.spline.SetLeftTangent(ssc.spline.GetPointCount(), tangenteEsquerdaComPontoAtual);
            //ssc.spline.SetRightTangent(ssc.spline.GetPointCount(), tangenteDireitaComPontoAtual);
        }

        var ultimoPontoA = (Vector2)ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), ultimoPontoA - new Vector2(0, 30));
    }
}
