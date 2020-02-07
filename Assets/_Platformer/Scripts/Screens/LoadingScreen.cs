using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    public Slider LoadingBar => _loadingBar;
}
