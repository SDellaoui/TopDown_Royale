using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CollectibleSpawnCurveController : MonoBehaviourPunCallbacks, IPunObservable
{
    //Authorize spawn on network
    bool authorizeSpawn = false;
    // object 
    private float speed = 2f;
    private float count = 0f;

    //Bezier curve
    private Vector3[] point = new Vector3[3];
    private Vector2 startPoint;

    private Vector2 endPoint;
    private Vector3 distancePoint;
    private Vector2 direction;
    private Vector2 perpendicularDirection;

    private float directionAngle;
    private float curveFactor;

    //Bounce
    private int bounceCount = 0;
    private int nBounce = 3;

    //Shadow
    public GameObject shadow;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if (!PhotonNetwork.IsMasterClient)
            return;
        startPoint = transform.position;
        nBounce = Random.Range(1, 3);
        InitCurve();
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceCount >= nBounce || !authorizeSpawn)
            return;   
        MakeParabola();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
        if(stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(endPoint.x);
            stream.SendNext(endPoint.y);
            stream.SendNext(nBounce);
            stream.SendNext(authorizeSpawn);
        }
        else if(stream.IsReading)
        {
            float endX = (float)stream.ReceiveNext();
            float endY = (float)stream.ReceiveNext();
            nBounce = (int)stream.ReceiveNext();
            endPoint = new Vector2(endX, endY);
            authorizeSpawn = (bool)stream.ReceiveNext();
        }
    }

    public void InitCurve()
    {
        float endX = Random.Range(-1f, 1f);
        float endY = Random.Range(-1f, 1f);

        //Init Curve coordinates
        endPoint = transform.position + new Vector3(endX, endY).normalized;
        endPoint += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0f));

        photonView.RPC("RPCSetCurve", RpcTarget.All,startPoint, endPoint, nBounce);
    }
    [PunRPC]
    void RPCSetCurve(Vector2 _startPoint, Vector2 _endPoint, int _nBounce)
    {
        startPoint = _startPoint;
        endPoint = _endPoint;
        nBounce = _nBounce;
        SetCurve();
    }
    public void SetCurve()
    { 
        direction = endPoint - startPoint;

        //Calculate angle direction from startpoint to end point
        directionAngle = -Vector2.SignedAngle(direction, transform.right);
        perpendicularDirection = (Vector2)(Quaternion.Euler(0, 0, directionAngle + 90) * Vector2.right);

        //Reduce the curve factor if sin(angle) gets closer to 1 or -1
        curveFactor = Mathf.Clamp(1 - Mathf.Abs(Mathf.Sin(AngleToRad(directionAngle))), 0f, 0.7f);

        //invert the curve factor when cos(angle) < 0
        if (Mathf.Cos(AngleToRad(directionAngle)) < 0)
            curveFactor = -curveFactor;

        authorizeSpawn = true;

        CreateCurve();
        
    }
    void CreateCurve()
    {
        //Set the midpoint from mid distance start and end, and to end depending on the sin(angle)
        Vector2 midPoint = Vector2.Lerp(startPoint, endPoint, Mathf.Clamp(curveFactor, 0.5f, 1f));
        midPoint += perpendicularDirection * curveFactor;

        //Create bezier curve points.

        //point = new Vector3[3];
        point[0] = startPoint;
        point[1] = midPoint;
        point[2] = endPoint;
        
        count = 0f;
    }
    
    void RemakeCurve()
    {
        //iterate the number of bounces
        bounceCount += 1;
        //set the new points coordinates. The curve will be half the previous one.
        startPoint = endPoint;
        direction /= 2;
        endPoint = startPoint + direction;
        curveFactor /= 2;

        //add a little speed to the next bounce.
        speed += 1f;

        //Recalculate curve
        CreateCurve();
    }
    
    void MakeParabola()
    {
        if (count < 1.0f)
        {
            count += 1.0f *speed *  Time.deltaTime;

            //Debug.Log(point.Length);
            Vector3 m1 = Vector3.Lerp(point[0], point[1], count);
            Vector3 m2 = Vector3.Lerp(point[1], point[2], count);
            transform.position = Vector3.Lerp(m1, m2, count);

            //Shadow Lerp
            shadow.transform.position = Vector3.Lerp(shadow.transform.position, point[2], count);
        }
        //if the object reaches the end point, call a bounce.
        if (endPoint == new Vector2(transform.position.x, transform.position.y))
            RemakeCurve();
    }

    float AngleToRad(float angle)
    {
        return Mathf.PI / 180 * angle;
    }

    
}
