using UnityEngine;
using System.Collections;

namespace TDGP
{
    public class DeathAnim : MonoBehaviour
    {

        void Start()
        {
            StartCoroutine(KillTime());
        }

        IEnumerator KillTime()
        {
            yield return new WaitForSeconds(0.35f);

            Destroy(gameObject);
        }
    }
}
