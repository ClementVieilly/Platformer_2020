using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar = null;
    public Slider LoadingBar => _loadingBar;
}
