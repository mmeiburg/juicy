using UnityEngine;
using UnityEngine.UI;

namespace TinyTools.Juicy
{
    [AddComponentMenu("")]
    public class JuicyScreenFlasher : MonoBehaviour
    {
        private static JuicyScreenFlasher instance;

        public Image Image { get; private set; }

        private void CreateImage()
        {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Image = gameObject.AddComponent<Image>();
            Image.color = new Color(0,0,0,0);
        }

        public static JuicyScreenFlasher Instance {
            get {
                if (instance != null) {
                    return instance;
                }

                GameObject obj = new GameObject(nameof(JuicyScreenFlasher));
                instance = obj.AddComponent<JuicyScreenFlasher>();
                instance.CreateImage();
                
                return instance;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}