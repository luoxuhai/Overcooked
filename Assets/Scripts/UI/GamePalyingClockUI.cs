using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GamePalyingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    void Update() {
        timerImage.fillAmount = GameManager.Instance.GetPlayingTimerNormalized();
    }
}
