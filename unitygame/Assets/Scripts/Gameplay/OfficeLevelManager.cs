using Messages;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Gameplay
{
    public class OfficeLevelManager : MonoBehaviour
    {
        [SerializeField]
        public ScenePicker[] Levels;
        string _currentScenePath;
        
        private void Awake()
        {
            _currentScenePath = Levels[0].scenePath;
            EventBus.Subscribe<CompanyLevelChanged>(CompanyLevelChanged);
            EventBus.Subscribe<CompanyChanged>(OnCompanyChanged);
        }
        private void Start()
        {
            SceneManager.LoadScene(_currentScenePath, LoadSceneMode.Additive);
        }

        private void OnCompanyChanged(CompanyChanged obj)
        {
            // new company (can be null on bankruptcy)
            var company = obj.Content;
        }

        private void CompanyLevelChanged(CompanyLevelChanged obj)
        {
            // newLevel is int int range [2 - 4] (inclusive) 
            var newLevel = obj.Content;

            var sceneIndex = newLevel - 2;
            if (sceneIndex >= 0 && sceneIndex < Levels.Length)
            {
                SceneManager.UnloadSceneAsync(_currentScenePath);
                _currentScenePath = Levels[newLevel - 2].scenePath;
                SceneManager.LoadScene(_currentScenePath, LoadSceneMode.Additive);    
            }
            else
            {
                Debug.LogWarning($"Cannot load scene for company level: {newLevel}. Out of range");
            }
        }
    }
}