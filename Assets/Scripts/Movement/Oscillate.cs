using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [SerializeField] float speed, amplitude;
    [SerializeField] bool _randomInitPos = false;

    Vector3 posInitial;
    Vector3 addPosLocal = new Vector3(0, 0, 0);
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        posInitial = transform.position;
        if(_randomInitPos)
        {
            time = Random.Range(0f, 55f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vibrar();
    }

    private void Vibrar()
    {
        addPosLocal.y = Mathf.Sin(time * speed) * amplitude;
        time += Time.deltaTime;
        transform.position = addPosLocal + posInitial;
    }
}
