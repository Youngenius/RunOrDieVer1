using System;
using System.Linq;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    [SerializeField] private ChunkPlacer chunkPlacer;
    private EnemyBehaviour _spider;
    private Bush[] bushes;
    private Mushroom mushroom;
    private Pond pond;
    private DrowningAnimation _drowningVisualEffect;

    [SerializeField] private ParticleSystem _bigDust;
    [SerializeField] private ParticleSystem _smallDust;
    private ParticleSystem _dustIsPlaying;

    private void Start()
    {
        _spider = GameObject.Find("Spider").GetComponent<EnemyBehaviour>();
        _drowningVisualEffect = (DrowningAnimation)FindInactiveObjectOfType(typeof(DrowningAnimation));
        mushroom = (Mushroom)FindInactiveObjectOfType(typeof(Mushroom));
        pond = (Pond)FindInactiveObjectOfType(typeof(Pond));
        bushes = GameObject.FindObjectsOfType<Bush>(true).Where(eff => !eff.gameObject.activeInHierarchy).ToArray();

        foreach (var bush in bushes)
        {
            bush.GetComponent<Bush>().OnSpiderDetainedEvent += Bush_OnSpiderDetainedEvent;
        }

        _dustIsPlaying = _smallDust;

        chunkPlacer.OnPrecipiceCreateEvent += EnemyBehaviour_OnSpiderCreateEvent;
        //bushes.OnSpiderDetainedEvent += Bush_OnSpiderDetainedEvent;
        mushroom.OnMushroomPickedEvent += Mushroom_OnMushroomPickedEvent;
        mushroom.OnMushroomEatenEvent += Mushroom_OnMushroomEatenEvent;
        pond.OnDrawningEvent += Pond_OnDrawningEvent;
    }

    private void Pond_OnDrawningEvent(object sender, EventArgs e)
    {
        _spider.StopSpider();

        _dustIsPlaying.Stop();
        _drowningVisualEffect.gameObject.SetActive(true);
        _drowningVisualEffect._animator.SetTrigger("Drown");
    }

    private void Mushroom_OnMushroomEatenEvent(object sender, EventArgs e)
    {
        _bigDust.Stop();
        _smallDust.Play();
        _dustIsPlaying = _smallDust;
    }

    private void Mushroom_OnMushroomPickedEvent(object sender, EventArgs e)
    {
        _bigDust.gameObject.SetActive(true);
        _bigDust.Play();
        _smallDust.Stop();

        _dustIsPlaying = _bigDust;
    }

    private void Bush_OnSpiderDetainedEvent(object sender, System.EventArgs e)
    {
        EnemyBehaviour enemyBehaviour = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();

        Debug.Log(EnemyBehaviour.IsSpiderDetained);
        if (EnemyBehaviour.IsSpiderDetained)
        {
            enemyBehaviour.SpawnSpider();
            EnemyBehaviour.IsSpiderDetained = false;
        }
        else if (enemyBehaviour.IsOutOfCamRange)
        {
            enemyBehaviour._speed = 12;
        }
    }

    private void EnemyBehaviour_OnSpiderCreateEvent(object sender, System.EventArgs e)
    {
        EnemyBehaviour.pointForSpyderToStop = GameObject.Find("PointToStop").transform;
    }

    private System.Object FindInactiveObjectOfType( Type effectType)
    {
        System.Object[] effects = new System.Object[] {}; 

        if (effectType.Equals(typeof(Mushroom)))
            effects = GameObject.FindObjectsOfType<Mushroom>(true).Where(eff => !eff.gameObject.activeInHierarchy).ToArray();

        else if (effectType.Equals(typeof(Bush)))
            effects = GameObject.FindObjectsOfType<Bush>(true).Where(eff => !eff.gameObject.activeInHierarchy).ToArray();
        
        else if (effectType.Equals(typeof(Pond)))
            effects = GameObject.FindObjectsOfType<Pond>(true).Where(eff => !eff.gameObject.activeInHierarchy).ToArray();

        else if (effectType.Equals(typeof(DrowningAnimation)))
            effects = GameObject.FindObjectsOfType<DrowningAnimation>(true).Where(eff => !eff.gameObject.activeInHierarchy).ToArray();
        return effects[0];
    }
}
