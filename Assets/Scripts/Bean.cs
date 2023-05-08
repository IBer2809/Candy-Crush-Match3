using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour
{
    public bool IsMatched { get; private set; }
    public bool IsClicked { get; private set; }
    public void MakeBeanMatchedOrNot(bool value) => IsMatched = value;

    public void SetIsClicked(bool value) => IsClicked = value;
    [SerializeField] private RaycastHit2D _firstHittedBean;
    [SerializeField] private RaycastHit2D _secondHittedBean;
    [SerializeField] private Bean _currentBean;
    [SerializeField] private Bean _nextBean;
    [SerializeField] private Vector2 delta;
    private LayerMask _layerMask = 3;


    private void OnMouseDown()
    {
        _firstHittedBean = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (_firstHittedBean.collider.gameObject.TryGetComponent(out Bean firstBean))
        {
            _currentBean = _firstHittedBean.collider.gameObject.GetComponent<Bean>();
        }
    }

    private void OnMouseUp()
    {
        _secondHittedBean = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (_secondHittedBean.collider.gameObject.TryGetComponent(out Bean secondBean))
        {

            _nextBean = _secondHittedBean.collider.gameObject.GetComponent<Bean>();
            delta = _secondHittedBean.collider.gameObject.transform.position - _firstHittedBean.collider.gameObject.transform.position;
            CalculateRightMove(delta);
            delta = Vector2.zero;
            _currentBean = null;
            _nextBean = null;


        }




    }

    private void CalculateRightMove(Vector3 distValue)
    {
        if (distValue.x > 0 && distValue.y > 0 || distValue.x < 0 && distValue.y < 0 || distValue.x < 0 && distValue.y > 0 || distValue.x > 0 && distValue.y < 0)
        {
            return;
        }
        else if (distValue.x > 0 && distValue.x <= 1 || distValue.x < 0 && distValue.x >= -1)
        {
            StartCoroutine(GridManager.Instance.ChangeBeansPositions(_currentBean, _nextBean));
        }
        else if (distValue.y > 0 && distValue.y <= 1 || distValue.y < 0 && distValue.y >= -1)
        {
            StartCoroutine(GridManager.Instance.ChangeBeansPositions(_currentBean, _nextBean));
        }
    }

}
