using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public class BadMeat : MonoBehaviour,IBooster
    {
        public void Consume()
        {
            // Destroy the booster Meat
            Destroy(gameObject);
        }
    }
}
