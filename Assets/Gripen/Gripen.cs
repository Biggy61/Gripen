using UnityEngine;

public class Gripen : MonoBehaviour
{
    public int hp;
    private int _maxHp = 100;
    void Start()
    {
     hp = _maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAlive()
    {
        return hp > 0;
    }
}
