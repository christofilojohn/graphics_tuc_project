using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public class Heart : MonoBehaviour, IBooster
    {
        public void Consume()
        {
            // Destroy the booster Heart
            Destroy(gameObject);
        }
    }
}