using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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

        var ultimaPosicaoSpline = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);

        ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);

            ultimoPonto = posicaoComPontoAtual;

            ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), posicaoComPontoAtual);
            ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);
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

            //if (boundXCamera > 500)
            //{
            //    var ssc = chao.GetComponent<SpriteShapeController>();
            //    Camera.main.transform.position = Vector2.zero;
            //    var cameraVector2 = (Vector2)cameraTransform;
            //    for (int i = 0; i < ssc.spline.GetPointCount(); i++)
            //    {
            //        ssc.spline.SetPosition(i, ssc.spline.GetPosition(i) - cameraTransform);
            //    }
            //}
        }

        //if (up)
        //{
        //    up = false;
        //    SpawnaChao();
        //}
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
        ssc.spline.SetPosition(0, primeiroPonto - new Vector2(30, 20));
    }

    private void SpawnaChao()
    {
        var ssc = chao.GetComponent<SpriteShapeController>();

        var chaoPrefab = prefabsChao[0];

        var sscPrefab = chaoPrefab.GetComponent<SpriteShapeController>();

        ssc.spline.RemovePointAt(ssc.spline.GetPointCount() - 1); // remove ultimo

        var pontos = sscPrefab.spline.GetPointCount();

        var ultimaPosicaoSpline = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        var posicao = sscPrefab.spline.GetPosition(0);
        ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, -ultimaPosicaoSpline.y + posicao.y);

        ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);

        for (int i = 1; i < pontos; i++)
        {
            posicao = sscPrefab.spline.GetPosition(i);

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, -ultimaPosicaoSpline.y + posicao.y);

            ultimoPonto = posicaoComPontoAtual;

            ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), posicaoComPontoAtual);
            ssc.spline.SetTangentMode(ssc.spline.GetPointCount() - 1, ShapeTangentMode.Continuous);
        }

        var ultimoPontoA = (Vector2)ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), ultimoPontoA - new Vector2(0, 30));
    }
}
