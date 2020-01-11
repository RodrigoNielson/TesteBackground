using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BackGroundScript2 : MonoBehaviour
{
    public GameObject[] chaoBase;
    public GameObject[] prefabsChao;

    // Start is called before the first frame update
    void Start()
    {
        var chao = Instantiate(chaoBase[0], Vector2.zero, Quaternion.identity);

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

            

            var posicaoComPontoAtual = new Vector2(ultimaPosicaoSpline.x + posicao.x, - ultimaPosicaoSpline.y + posicao.y);

            //ultimaPosicaoSpline = new Vector2(ultimaPosicaoSpline.x - posicao.x, - ultimaPosicaoSpline.y + posicao.y);

            ssc.spline.InsertPointAt(ssc.spline.GetPointCount(), posicaoComPontoAtual);
            ssc.spline.SetTangentMode(ssc.spline.GetPointCount()-1, ShapeTangentMode.Continuous);
        }

        //ssc.spline.InsertPointAt(ssc.spline.GetPointCount() - 1, new Vector2(10, 5));
        //ssc.spline.InsertPointAt(ssc.spline.GetPointCount() - 1, new Vector2(15, 5));
        //ssc.spline.InsertPointAt(ssc.spline.GetPointCount() - 1, new Vector2(20, 5));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
