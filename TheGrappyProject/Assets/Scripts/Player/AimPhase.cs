using Unity.Mathematics;
using UnityEngine;
public class AimPhase : MonoBehaviour {
   public Transform aimArrowHolder;
   public LineRenderer lineRenderer;
   public Transform aimPoint;
   public Gradient aimMatchGradient;
   private Gradient _initialGradient;
   private float _grapLength;
   private LayerMask _grapPointsMask;
   private float _speed;
   private RaycastHit2D[] hit = new RaycastHit2D[1];
   private Transform _grapTransform;
    private Vector3 _grapPoint;

   //should be called once to setup variables 
   public void Setup (float grapLength, LayerMask grapPointsMask, float speed) {
      _grapLength = grapLength;
      _grapPointsMask = grapPointsMask;
      _speed = speed;
      _initialGradient = lineRenderer.colorGradient;
   }
   //should be called every time when switching to Aim phase
   public void Switch () {
      if (aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> () == null) {
         Debug.LogError ("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
      } else
         aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> ().enabled = true;
      if (_grapTransform != null)
         aimPoint.GetComponent<SpriteRenderer> ().enabled = true;
      lineRenderer.enabled = true;
   }
   public void End () {
      if (aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> () == null) {
         Debug.LogError ("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
      } else
         aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;

      aimPoint.GetComponent<SpriteRenderer> ().enabled = false;
      lineRenderer.enabled = false;

   }
   private void OnDrawGizmos () {
      Debug.DrawLine (transform.position, hit[0].point);

   }
   private void DrawAim (Vector3 dir) {
      Vector3 dirToHitPoint = new Vector3 (hit[0].point.x - transform.position.x,
         hit[0].point.y - transform.position.y, 0);
      float angleToHit = Mathf.Atan2 (dirToHitPoint.y, dirToHitPoint.x) * Mathf.Rad2Deg;

      if (math.dot (dirToHitPoint.normalized, dir.normalized) > 0.9f) {
         lineRenderer.colorGradient = aimMatchGradient;
      } else {
         lineRenderer.colorGradient = _initialGradient;
      }
      lineRenderer.SetPosition (0, transform.position);
      lineRenderer.SetPosition (1, transform.position + (dir * _grapLength / 2));

      aimArrowHolder.rotation = Quaternion.Euler (0, 0, angleToHit - 90);
   }
   public Vector3 Run (Vector2 aimDelta, Vector2 grapPos) {
      float playerAngle = Mathf.Atan2 (transform.right.y, transform.right.x) * Mathf.Rad2Deg;
      float deltaAngle = Mathf.Atan2 (aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
      float angle = deltaAngle + playerAngle;
      //shoot rayCast in direction of joystick
      Vector2 dir = new Vector2 (math.cos (math.radians (angle)), math.sin (math.radians (angle)));
      Debug.DrawLine (transform.position, dir);
      int hitCount = Physics2D.RaycastNonAlloc (
         transform.position, dir, hit, _grapLength, _grapPointsMask);

      //change grap posiotion only if found new target
      if (hitCount != 0) {
         _grapPoint = hit[0].point;
         _grapTransform = hit[0].transform;
         aimPoint.GetComponent<SpriteRenderer> ().enabled = true;

      }

      transform.position += transform.up * _speed * Time.deltaTime;
      DrawAim (dir);
      if (_grapTransform == null) {
         return Vector3.zero;
      }
      SetAimPoint (_grapPoint);
        return _grapPoint;
   }

   private void SetAimPoint (Vector3 point) {
      aimPoint.position = point;
   }

}