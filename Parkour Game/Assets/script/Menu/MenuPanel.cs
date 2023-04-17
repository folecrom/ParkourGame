using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private bool state;

    private Canvas canvas;
    private CanvasGroup group;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void UpdateState() {
        canvas.enabled = state;
    }
    public void ChangeState() {
        state = !state;
        UpdateState();
    }

    public void ChangeState(bool _state) {
        state = _state;
        UpdateState();
    }
    #region Getter
    public PanelType GetPanelType() {return type;}
    #endregion
}
