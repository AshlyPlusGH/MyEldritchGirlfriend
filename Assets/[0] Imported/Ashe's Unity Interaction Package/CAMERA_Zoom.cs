using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Ash {
    public class CAMERA_Zoom : MonoBehaviour
    {
        [SerializeField] private bool debug;
            private void Log(string contents){ if (debug){ Debug.Log(contents); }}

        [Space(10)]
        
        [SerializeField] private int zoomMax = 128;
        [SerializeField] private int zoomMin = 25;

        [Space(10)]

        [SerializeField] private int zoomSpeed = 2;

        [Space(10)]

        [SerializeField] private ENUM_CAMERA_Type cameraType = ENUM_CAMERA_Type.Standard;

        [Space(10)]

        [SerializeField] private UnityEvent unityEventOnScroll;

        void Update(){ Scroll(); }
        void Scroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll == 0){ return; }

            switch (cameraType)
            {
                case ENUM_CAMERA_Type.Standard:
                    StandardScroll(scroll);
                    break;
            }

            unityEventOnScroll.Invoke();
        }
            void StandardScroll(float scroll)
            {
                if (scroll < 0f)
                {
                    // Scroll up
                    gameObject.GetComponent<Camera>().orthographicSize += zoomSpeed;
                }
                else if (scroll > 0f)
                {
                    // Scroll down
                    if (gameObject.GetComponent<Camera>().orthographicSize - zoomSpeed > 1){
                        gameObject.GetComponent<Camera>().orthographicSize -= zoomSpeed;
                    }
                }
            }
    }

    enum ENUM_CAMERA_Type
    {
        Standard
    }
}
