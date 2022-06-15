using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilSpiderUI : MonoBehaviour
{
    private bool _letGo;
    private Rigidbody2D _rb;

    [SerializeField] float _lilSpiderSpeed = 8;

    [SerializeField] private Vector3[] angle1;
    [SerializeField] private Vector3[] angle2;
    [SerializeField] private Vector3[] angle3;
    [SerializeField] private Vector3[] angle4;

    [SerializeField] private Transform[] _spawningPositions_Rotation = new Transform[] { };
    [SerializeField] private Vector3[][] _angles;
    
    private Dictionary<Transform, Vector3[]> spawningPositions = 
        new Dictionary<Transform, Vector3[]>();

    private void Awake()
    {
        _angles = new Vector3[][] 
        {
            angle1,
            angle2,
            angle3,
            angle4
        };

        int i = 0;
        foreach (Vector3[] quat in _angles)
        {
            spawningPositions.Add(_spawningPositions_Rotation[i], quat);
            i++;
        }

        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SpiderRunCoroutine());
    }

    private void Update()
    {
        if (_letGo)
            this.gameObject.transform.position += -transform.up * _lilSpiderSpeed * Time.deltaTime;
    }

    private IEnumerator SpiderRunCoroutine() {
        float intervalBetweenAction;
        Vector3[] angles;

        while (true)
        {
            Transform startPos =
                _spawningPositions_Rotation[Random.Range(0, _spawningPositions_Rotation.Length)];
            _rb.transform.position = startPos.localPosition;
            angles = spawningPositions[startPos];
            foreach (var item in angles)
            {
                Debug.Log(item);
            }
            this.gameObject.transform.eulerAngles =
                angles[Random.Range(0, angles.Length)];

            Debug.Log($"Start position {startPos.position}, quaternion {_rb.transform.eulerAngles}");

            intervalBetweenAction = Random.Range(4, 7);
            yield return new WaitForSeconds(intervalBetweenAction);

            _letGo = true;
        }
    }
}