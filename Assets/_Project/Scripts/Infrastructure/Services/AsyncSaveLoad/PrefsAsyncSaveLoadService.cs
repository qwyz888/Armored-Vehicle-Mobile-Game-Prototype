using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.AsyncJson.Core;
using Infrastructure.Services.AsyncSaveLoad.Core;
using UnityEngine;

namespace Infrastructure.Services.AsyncSaveLoad
{
    public class PrefsAsyncSaveLoadService : IAsyncSaveLoadService
    {
        private readonly IAsyncJsonService _jsonService;

        public PrefsAsyncSaveLoadService(IAsyncJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        public async UniTask SaveAsync<T>(string key, T data)
        {
            string jsonData = await _jsonService.SerializeAsync(data);

            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
            Debug.Log($"[PrefsAsyncSaveLoadService] Saved key={key} jsonLength={jsonData?.Length}");
        }

        public async UniTask<T> LoadAsync<T>(string key, T defaultValue = default)
        {
            if (HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);

                try
                {
                    T instance = await _jsonService.DeserializeAsync<T>(jsonData);

                    Debug.Log($"[PrefsAsyncSaveLoadService] Loaded key={key} jsonLength={jsonData?.Length}");
                    if (instance == null)
                        return defaultValue;

                    return instance;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public UniTask DeleteAsync(string key)
        {
            PlayerPrefs.DeleteKey(key);
            return UniTask.CompletedTask;
        }
    }
}