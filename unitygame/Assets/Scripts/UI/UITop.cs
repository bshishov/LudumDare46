using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UITop : MonoBehaviour
    {
        public UICanvasGroupFader CanvasGroupFader;
        public GameObject Prefab;

        public Color[] BgrColors;
        public Color[] TextColors;
        public Transform Container;

        private readonly List<UITopEntry> _entries = new List<UITopEntry>();

        public void Setup(int n)
        {
            ClearContainerChildren();
            _entries.Clear();
            for (var i = 0; i < n; i++)
            {
                _entries.Add(CreateEntry());
            }
        }

        private UITopEntry CreateEntry()
        {
            var go = GameObject.Instantiate(Prefab, Container);
            return go.GetComponent<UITopEntry>();
        }

        public void Setup(int index, string name, float balance)
        {
            if (index >= 0 && index < _entries.Count)
                _entries[index].Setup(index + 1, name, balance, 
                    GetColor(BgrColors, index), 
                    GetColor(TextColors, index));
        }

        private Color GetColor(Color[] array, int index)
        {
            if (array == null || array.Length == 0)
                return Color.white;

            return array[Mathf.Clamp(index, 0, array.Length - 1)];
        }

        private void ClearContainerChildren()
        {
            foreach (Transform child in Container)
                Destroy(child.gameObject);
        }
        

        public void Show()
        {
            if(CanvasGroupFader != null)
                CanvasGroupFader.FadeIn();
        }
        
        public void Hide()
        {
            if(CanvasGroupFader != null)
                CanvasGroupFader.FadeOut();
        }
    }
}