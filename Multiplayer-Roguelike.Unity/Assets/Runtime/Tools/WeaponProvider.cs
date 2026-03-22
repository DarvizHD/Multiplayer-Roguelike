using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Tools
{
    public class WeaponProvider : MonoBehaviour
    {
        public int Current
        {
            set
            {
                for (var i = 0; i < _weapons.Count; i++)
                {
                    _weapons[i].SetActive(i == value);
                }
            }
        }

        [SerializeField] private List<GameObject> _weapons;
    }
}
