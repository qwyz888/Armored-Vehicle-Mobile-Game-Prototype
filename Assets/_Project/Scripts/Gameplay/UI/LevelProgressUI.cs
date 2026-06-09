using UnityEngine;
using UnityEngine.UI;
using Gameplay.Car;
using TMPro;
using VContainer;

public class LevelProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private int levelIndex = 0;

    [Header("UI")]
    [SerializeField] private Slider progressSlider; 
    [SerializeField] private TextMeshProUGUI levelLabelTMP;

    private VehicleController _vehicle;
    private float _startZ;
    private float _levelLength = 1f;
    private string _levelName = "";

    [Inject]
    public void Construct(VehicleController vehicle)
    {
        _vehicle = vehicle;
    }

    private void Start()
    {
        ApplyConfig();

        if (_vehicle != null)
            _startZ = _vehicle.transform.position.z;
    }

    private void Update()
    {
        if (_vehicle == null || progressSlider == null) return;

        float traveled = Mathf.Abs(_vehicle.transform.position.z - _startZ);

        if (_levelLength <= 0f) _levelLength = 1f;

        progressSlider.minValue = 0f;
        progressSlider.maxValue = _levelLength;
        progressSlider.value = Mathf.Clamp(traveled, 0f, _levelLength);
    }

    private void ApplyConfig()
    {
        if (levelConfig != null)
        {
            var data = levelConfig.Get(levelIndex);
            if (data != null)
            {
                _levelLength = data.Length;
                _levelName = string.IsNullOrEmpty(data.LevelName) ? $"Level {levelIndex + 1}" : data.LevelName;
            }
        }

        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (levelLabelTMP != null)
            levelLabelTMP.text = _levelName;
    }

    public void Initialize(LevelConfig cfg, int index, VehicleController veh)
    {
        levelConfig = cfg;
        levelIndex = index;
        _vehicle = veh;

        ApplyConfig();

        if (_vehicle != null)
            _startZ = _vehicle.transform.position.z;
    }

    public void ResetStart()
    {
        if (_vehicle != null)
            _startZ = _vehicle.transform.position.z;
    }
}

