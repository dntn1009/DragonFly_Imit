using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonobehaviour<EffectManager>
{
    [SerializeField]
    GameObject m_fxExplosionPrefab;
    GameObjectPool<FxController> m_effectPool;

    public void RemoveEffect(FxController effect)
    {
        effect.gameObject.SetActive(false);
        m_effectPool.Set(effect);
    }
    public void CreateEffect(Vector3 pos)
    {
        var effect = m_effectPool.Get();
        effect.transform.position = pos;
        effect.gameObject.SetActive(true);
    }
    protected override void OnStart()
    {
        m_effectPool = new GameObjectPool<FxController>(5, () =>
        {
            var obj = Instantiate(m_fxExplosionPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            var effect = obj.GetComponent<FxController>();
            return effect;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
