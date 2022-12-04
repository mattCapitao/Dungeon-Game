using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrap : MonoBehaviour
{ 
    [SerializeField] Trap _trapPrefab;
    [SerializeField] int _trapsPlaced;
    [SerializeField] int _playerMaxTraps = 20;
    [SerializeField] int _maxTrapRange = 6;
    [SerializeField] float _trapTimeOut = 30;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && _trapsPlaced < _playerMaxTraps)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                float distance = Vector3.Distance(transform.position, hitInfo.collider.transform.position);
                Debug.Log("Distance to target " + distance);
                if (hitInfo.collider.GetComponentInChildren<Trap>() != null || distance > _maxTrapRange)
                    return;
                SpawnTrap(hitInfo);
            }
        }
    }

    private void SpawnTrap(RaycastHit hitInfo)
    {
        _trapsPlaced++;
        var placementPoint = hitInfo.collider.transform.position + new Vector3(0, .5f, 0);
        Trap trap = Instantiate(_trapPrefab, placementPoint, Quaternion.identity);
        trap.transform.SetParent(hitInfo.collider.transform);
        StartCoroutine(TrapTimeOut(trap));
    }

    IEnumerator TrapTimeOut(Trap trap)
    {
        yield return new WaitForSeconds(_trapTimeOut);
        _trapsPlaced--;
        Destroy(trap.gameObject);
    }
}