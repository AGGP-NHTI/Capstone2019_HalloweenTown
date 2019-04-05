using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TeleportScript : MonoBehaviour {

    public GameObject tpLocationIndicator;
    public GameObject objectToTeleport;
    public string targetLayer;
    public float objectHeight = 1.0f;
    public float objectRadius = 0.3f;
    public float maxFloorSlope = 30.0f;

    public float parabolaMaxMagnitude = 50.0f;
    public float parabolaStepSize = 0.1f;
    public float maxArcLength = 20.0f;
    public bool showParabola = false;
    public float parabolaTimeToRampUp = 2.0f;

    protected float _parabolaMagnitude;
    protected LineRenderer _lr;
    protected List<Vector3> _parabolaPoints = new List<Vector3>();

    protected Vector3 _lastHitPosition;
    protected bool _validTeleportLocation = false;
    protected bool _coroutineActive = false;

    // Use this for initialization
    void Start()
    {
        _lr = gameObject.GetComponent<LineRenderer>();
        tpLocationIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showParabola)
        {
            SetTrajectoryPoints();
        }
        else
        {
            _lr.positionCount = 0;
        }

        if (showParabola && _validTeleportLocation)
        {
            tpLocationIndicator.transform.position = _lastHitPosition;
            tpLocationIndicator.SetActive(true);
        }
        else
        {
            tpLocationIndicator.SetActive(false);
        }
    }

    void SetTrajectoryPoints()
    {
        _parabolaPoints.Clear();
        Vector3 point = transform.position;
        Vector3 pointVelocity = transform.forward * _parabolaMagnitude;

        _parabolaPoints.Add(point);

        RaycastHit hit = new RaycastHit();
        Ray parabolaArcRay;
        bool parabolaCollided = false;

        for (float distanceArced = 0.0f; distanceArced < maxArcLength && !parabolaCollided; distanceArced += parabolaStepSize)
        {
            parabolaArcRay = new Ray(point, pointVelocity);
            //Debug.DrawRay(parabolaArcRay.origin, parabolaArcRay.direction);
            if (Physics.Raycast(parabolaArcRay, out hit, parabolaStepSize))
            {
                parabolaCollided = true;
                _parabolaPoints.Add(hit.point);
            }
            else
            {
                point += pointVelocity.normalized * parabolaStepSize;
                _parabolaPoints.Add(point);
            }

            pointVelocity += Physics.gravity * parabolaStepSize;
        }

        if (parabolaCollided)
        {
            CheckAndUpdateTeleportLocation(hit);
        }
        else
        {
            _validTeleportLocation = false;
        }

        _lr.positionCount = _parabolaPoints.Count;
        _lr.SetPositions(_parabolaPoints.ToArray());
    }

    protected virtual void CheckAndUpdateTeleportLocation(RaycastHit hit)
    {
        if (hit.transform.gameObject.layer != LayerMask.NameToLayer(targetLayer))
        {
            _validTeleportLocation = false;
            return;
        }

        if (Vector3.Angle(hit.normal, Vector3.up) > maxFloorSlope)
        {
            _validTeleportLocation = false;
            return;
        }

        if (Physics.CheckCapsule(hit.point + Vector3.up * (objectRadius + 0.01f), hit.point + (Vector3.up * (objectHeight - objectRadius)), objectRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            _validTeleportLocation = false;
            return;
        }

        _lastHitPosition = hit.point;
        _validTeleportLocation = true;
    }

    public virtual void OnActivate()
    {
        showParabola = true;
        if (!_coroutineActive)
        {
            StartCoroutine(RampUpParabola());
        }
    }

    public virtual void OnDeactivate()
    {
        showParabola = false;
        

        if (_validTeleportLocation && objectToTeleport)
        {
            objectToTeleport.transform.position = _lastHitPosition + (Vector3.up * objectHeight);
        }

        _validTeleportLocation = false;
    }

    protected virtual IEnumerator RampUpParabola()
    {
        _coroutineActive = true;
        _parabolaMagnitude = 0.0f;

        for (float timer = 0.0f; (timer < parabolaTimeToRampUp) && showParabola; timer += Time.deltaTime)
        {
            float rampPercent = timer / parabolaTimeToRampUp;
            _parabolaMagnitude = Mathf.Lerp(0.0f, parabolaMaxMagnitude, rampPercent);
            yield return null;
        }
        if(showParabola)
        {
            _parabolaMagnitude = parabolaMaxMagnitude;
        }
        _coroutineActive = false;
    }

    private void OnDisable()
    {
        OnDeactivate();
    }
}
