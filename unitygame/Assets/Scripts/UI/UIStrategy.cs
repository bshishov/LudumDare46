using UnityEngine;

namespace UI
{
    public class UIStrategy : MonoBehaviour
    {
        public UICanvasGroupFader CanvasGroupFader;
        public UIProgressBar Marketing;
        public UIProgressBar Technology;
        public UIProgressBar Relevance;

        public void Set(float marketing, float technology, float relevance)
        {
            if(Marketing != null)
                Marketing.SetTarget(marketing);
            
            if(Technology != null)
                Technology.SetTarget(technology);
            
            if(Relevance != null)
                Relevance.SetTarget(relevance);
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