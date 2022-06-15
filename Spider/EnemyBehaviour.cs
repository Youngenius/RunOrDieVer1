using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float _speed;
    public static Transform pointForSpyderToStop;
    private bool isOutOfCamRange;

    private float _distanceToPrecipice = 4.5f;
    public static bool IsSpiderDetained = false;


    public static EnemyBehaviour SpiderInstance;
    public bool IsOutOfCamRange => isOutOfCamRange;

    private void Awake()
    {
        SpiderInstance = this.gameObject.GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        this.gameObject.transform.Translate(Vector2.right * _speed * Time.deltaTime);

        if (pointForSpyderToStop != null)
            if (pointForSpyderToStop.position.x - this.gameObject.transform.position.x <= _distanceToPrecipice)
            {
                StopSpider();
            }
    }

    private void OnBecameInvisible()
    {
        isOutOfCamRange = true;;
    }

    private IEnumerator OnBecameVisible()
    {
        float distance = 6;

        yield return new WaitUntil(() =>
            PlayerBehaviour.PlayerInstance.position.x - this.gameObject.transform.position.x <= distance);

        isOutOfCamRange = false;
        _speed = 10;
    }

    public void SpawnSpider()
    {
        float distanceToStayOutsideCam = 10f;
        pointForSpyderToStop = null;
        Debug.Log("SPAWNED");

        Vector2 playerPosition = PlayerBehaviour.PlayerInstance.position;

        this.gameObject.transform.position =
            new Vector2(playerPosition.x - distanceToStayOutsideCam,
                        playerPosition.y);
        _speed = 12;
    }
    
    public void StopSpider()
    {
        _speed = 0;
        IsSpiderDetained = true;
    }
}
