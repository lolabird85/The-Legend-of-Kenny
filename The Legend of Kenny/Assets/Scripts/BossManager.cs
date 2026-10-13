using UnityEngine;

public class BossManager : MonoBehaviour
{
    
    [SerializeField] private Transform _crownParent;
    public GameObject CrownFab;
    public GameObject KingKen;
    public Transform KenParent;
    [SerializeField] private GameObject _gate;

    void Update()
    {
        if (BossBehavior.BossHealth <= 0)
        {
            Instantiate(CrownFab, Vector3.zero, Quaternion.identity, _crownParent);
            BossBehavior.BossHealth = 1000;
        }
        if (KenBehavior.Crowned)
        {
            Instantiate(KingKen, Vector3.zero, Quaternion.identity, KenParent);
            KenBehavior.Crowned = false;
            Destroy(_gate);
            
        }
    }
}
