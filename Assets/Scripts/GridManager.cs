using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _xDim;
    [SerializeField] private int _yDim;
    [SerializeField] private Bean[] _beanPrefabs;
    [SerializeField] private Cell _gridPrefab;
    private Cell[,] _spawnedGrid;
    [SerializeField] private List<Bean> _currentMatches = new List<Bean>();
    private bool _gridHasMatches = false;
    private bool _isCheckingMatches = false;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            if (_isCheckingMatches)
            {
                CheckMatches();
                if (_currentMatches.Count > 0)
                {
                    _gridHasMatches = true;
                    if (_gridHasMatches)
                    {
                        StartCoroutine(DeleteMatchedBeans());
                    }
                }
            }



        }
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(_xDim, _yDim);
    }


    public IEnumerator SpawnGridAndBeans()
    {
        _spawnedGrid = new Cell[_xDim, _yDim];
        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Cell newCell = Instantiate(_gridPrefab, new Vector2(x, y), Quaternion.identity);
                _spawnedGrid[x, y] = newCell;
                _spawnedGrid[x, y].transform.parent = transform;
                Bean newBean = Instantiate(_beanPrefabs[Random.Range(0, _beanPrefabs.Length)], new Vector2(x, y), Quaternion.identity);
                newBean.transform.parent = newCell.transform;
                newCell.GetComponent<Cell>().SetForThisCellBean(newBean);
                yield return null;
            }
        }
        _isCheckingMatches = true;
        GameManager.Instance.ChangeGameState(GameManager.GameState.Play);
        UIManager.Instance.ActivatePlayPanelButtons(true);

    }

    private void CheckMatches()
    {

        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Bean currentBean = _spawnedGrid[x, y].GetComponentInChildren<Bean>();
                if (currentBean != null)
                {
                    if (x > 0 && x < _xDim - 1)
                    {

                        Bean rightBean = _spawnedGrid[x + 1, y].GetComponentInChildren<Bean>();
                        Bean leftBean = _spawnedGrid[x - 1, y].GetComponentInChildren<Bean>();
                        if (rightBean != null && leftBean != null)
                        {
                            if (rightBean.tag == currentBean.tag && leftBean.tag == currentBean.tag)
                            {

                                if (!_currentMatches.Contains(rightBean))
                                {
                                    _currentMatches.Add(rightBean);
                                    rightBean.MakeBeanMatchedOrNot(true);
                                }
                                if (!_currentMatches.Contains(leftBean))
                                {
                                    _currentMatches.Add(leftBean);
                                    leftBean.MakeBeanMatchedOrNot(true);
                                }
                                if (!_currentMatches.Contains(currentBean))
                                {
                                    _currentMatches.Add(currentBean);
                                    currentBean.MakeBeanMatchedOrNot(true);
                                }


                            }
                        }
                    }
                    if (y > 0 && y < _yDim - 1)
                    {
                        Bean upBean = _spawnedGrid[x, y + 1].GetComponentInChildren<Bean>();
                        Bean downBean = _spawnedGrid[x, y - 1].GetComponentInChildren<Bean>();
                        if (upBean != null && downBean != null)
                        {
                            if (upBean.tag == currentBean.tag && downBean.tag == currentBean.tag)
                            {
                                if (!_currentMatches.Contains(upBean))
                                {
                                    _currentMatches.Add(upBean);
                                    upBean.MakeBeanMatchedOrNot(true);
                                }
                                if (!_currentMatches.Contains(downBean))
                                {
                                    _currentMatches.Add(downBean);
                                    downBean.MakeBeanMatchedOrNot(true);
                                }
                                if (!_currentMatches.Contains(currentBean))
                                {
                                    _currentMatches.Add(currentBean);
                                    currentBean.MakeBeanMatchedOrNot(true);
                                }


                            }
                        }
                    }

                }
            }
        }


    }

    private IEnumerator DeleteMatchedBeans()
    {
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < _spawnedGrid.GetLength(0); i++)
        {
            for (int j = 0; j < _spawnedGrid.GetLength(1); j++)
            {
                if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
                {
                    Bean beanToDelete = _spawnedGrid[i, j].GetComponentInChildren<Bean>();
                    if (beanToDelete != null)
                    {
                        if (beanToDelete.IsMatched)
                        {
                            beanToDelete.MakeBeanMatchedOrNot(false);
                            Destroy(beanToDelete.gameObject);
                        }

                    }
                }
                else
                    break;

            }
        }
        _gridHasMatches = false;
        _currentMatches.Clear();
        StartCoroutine(ChangeAllBeansPositions());

    }

    private IEnumerator ChangeAllBeansPositions()
    {
        yield return new WaitForSeconds(0.2f);
        int newBeanPosition = 0;

        for (int i = 0; i < _spawnedGrid.GetLength(0); i++)
        {
            for (int j = 0; j < _spawnedGrid.GetLength(1); j++)
            {
                Bean currentCellBean = null;

                if (_spawnedGrid[i, j] != null && _spawnedGrid[i, j].transform.childCount == 0)
                    newBeanPosition++;

                else if (_spawnedGrid[i, j] != null && _spawnedGrid[i, j].transform.childCount > 0 && newBeanPosition > 0)
                {
                    currentCellBean = _spawnedGrid[i, j].GetComponentInChildren<Bean>();
                    Cell newCellParentForBean = _spawnedGrid[i, j - newBeanPosition];
                    currentCellBean.transform.parent = newCellParentForBean.transform;
                    newCellParentForBean.SetForThisCellBean(currentCellBean);
                    currentCellBean.transform.DOLocalMove(Vector2.zero, 0.2f);
                }
            }
            newBeanPosition = 0;
        }

        yield return new WaitForSeconds(0.3f);
        RecreateGrid();
    }

    private void RecreateGrid()
    {
        for (int i = 0; i < _spawnedGrid.GetLength(0); i++)
        {
            for (int j = 0; j < _spawnedGrid.GetLength(1); j++)
            {
                Cell currentCell = _spawnedGrid[i, j];
                if (currentCell != null)
                {
                    if (currentCell.transform.childCount == 0)
                    {
                        Bean newBean = Instantiate(_beanPrefabs[Random.Range(0, _beanPrefabs.Length)], new Vector2(i, j), Quaternion.identity);
                        newBean.transform.parent = currentCell.transform;
                        currentCell.SetForThisCellBean(newBean);
                    }
                }


            }
        }

    }

    public IEnumerator ChangeBeansPositions(Bean firstBean, Bean secondBean)
    {
        Cell firstBeanParent = firstBean.GetComponentInParent<Cell>();
        Cell secondBeanParent = secondBean.GetComponentInParent<Cell>();
        firstBean.transform.parent = secondBeanParent.transform;
        secondBean.transform.parent = firstBeanParent.transform;
        firstBean.transform.DOLocalMove(Vector2.zero, 0.2f);
        secondBean.transform.DOLocalMove(Vector2.zero, 0.2f);
        yield return new WaitForSeconds(0.2f);
        if (_gridHasMatches == false)
        {
            firstBean.transform.parent = firstBeanParent.transform;
            secondBean.transform.parent = secondBeanParent.transform;
            firstBean.transform.DOLocalMove(Vector2.zero, 0.2f);
            secondBean.transform.DOLocalMove(Vector2.zero, 0.2f);
        }



    }

    public void DeleteGrid()
    {
        for (int x = 0; x < _spawnedGrid.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedGrid.GetLength(1); y++)
            {
                Destroy(_spawnedGrid[x, y].gameObject);
            }
        }
    }





}
