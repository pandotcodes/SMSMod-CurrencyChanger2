using UnityEngine;

namespace CurrencyChanger2.GoofyAhhCustomComponents
{
    public class TransformDefiner : MonoBehaviour
    {
        public Vector3 position;
        public Vector3 localPosition;
        public Vector3 localScale;
        public Vector3 eulerAngles;

        public void Update()
        {
            transform.position = position;
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            transform.eulerAngles = eulerAngles;
        }
        public static void AddToGameObject(GameObject go, Vector3 position, Vector3 localPosition, Vector3 localScale, Vector3 eulerAngles)
        {
            var td = go.AddComponent<TransformDefiner>();
            td.position = position;
            td.localPosition = localPosition;
            td.localScale = localScale;
            td.eulerAngles = eulerAngles;
        }
    }
}
