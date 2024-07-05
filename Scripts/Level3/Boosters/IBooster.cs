using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public interface IBooster
    {
        //public void OnTriggerEnter(Collider other);
        public void Consume();
    }
}

