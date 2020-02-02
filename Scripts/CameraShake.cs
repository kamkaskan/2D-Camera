using System.Collections;
using UnityEngine;
using UnityEditor;

namespace kamkaskan.camera
{
    [CustomEditor(typeof(CameraShake))]
        public class ColliderCreatorEditor : Editor {
        override public void  OnInspectorGUI () {
            CameraShake cameraShake = (CameraShake)target;
            if(GUILayout.Button("Add 0.5f Shake")) 
                cameraShake.AddShake(0.5f); 
            DrawDefaultInspector();
        }
    }
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float shakeSpeed = 10f;
        [SerializeField]
        private float maxRotate = 15f;
        [SerializeField]
        private float maxOffset = 0.5f;
        [SerializeField]
        [Range(0f,1f)]
        private float shakePower = 0f;
        [SerializeField]
        private float decreasePower = 0.75f;
        [SerializeField]
        private Transform shakeObject;

        IEnumerator shakingCoroutine;
        // Start is called before the first frame update
        public void AddShake(float value)
        {
            shakePower += value;
            if (shakePower > 1f) shakePower = 1f;

            if (shakingCoroutine == null)
            {
                shakingCoroutine = Shaking();
                StartCoroutine(shakingCoroutine);
            }
        }

        public void SetShake(float value)
        {
            shakePower = value;

            if (shakePower > 0f  && shakingCoroutine == null)
            {
                shakingCoroutine = Shaking();
                StartCoroutine(shakingCoroutine);
            }
        }
        void Shake(float power)
        {
            //Rotate
            float rotateAmount = maxRotate * power * GetPerlin(0, shakeSpeed * Time.time);
            shakeObject.localRotation = Quaternion.Euler(0, 0, rotateAmount);  

            //Move
            float xAmount = maxOffset * power * GetPerlin(0.33f, shakeSpeed * Time.time);
            float yAmount = maxOffset * power * GetPerlin(0.66f, shakeSpeed * Time.time);
            shakeObject.localPosition = new Vector3(xAmount, yAmount, 0);        
        }

        float GetPerlin(float offset, float time)
        {
            return 2f * (Mathf.PerlinNoise(offset, time) - 0.5f);
        }

        IEnumerator Shaking()
        {
            while (shakePower > 0)
            {
                Shake(shakePower * shakePower);
                yield return null;
                shakePower -= decreasePower * Time.deltaTime;
            }
            shakePower = 0f;
            shakingCoroutine = null;
        }
    }
}