using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public class Meat : MonoBehaviour,IBooster
    {
        public void Consume()
        {
            // Destroy the booster Meat
            Destroy(gameObject);
        }
    }
}
