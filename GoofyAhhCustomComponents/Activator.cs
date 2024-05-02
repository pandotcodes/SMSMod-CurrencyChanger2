using UnityEngine;

namespace CurrencyChanger2.GoofyAhhCustomComponents
{
    public class Activator : MonoBehaviour
    {
        public void OnDisable() => gameObject.SetActive(true);
    }
}
