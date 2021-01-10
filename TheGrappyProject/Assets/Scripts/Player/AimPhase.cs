using Unity.Mathematics;
using UnityEngine;
public class AimPhase : MonoBehaviour {

   public Transform aimArrowHolder;
   public Transform aimPoint;
   private Linker linker;
   private float _grapLength;
   private float _circleCastRadius;
   private LayerMask _grapPointsMask;
   private float _speed;
   private RaycastHit2D[] hit = new RaycastHit2D[1];
   private Transform _grapPoint;

   //should be called once to setup variables 
   public void Setup (float grapLength, float circleCastRadius, LayerMask grapPointsMask, float speed) {
      _grapLength = grapLength;
      _circleCastRadius = circleCastRadius;
      _grapPointsMask = grapPointsMask;
      _speed = speed;
   }
   //should be called every time when switching to Aim phase
   public void Switch () {
      if (aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> () == null) {
         Debug.LogError ("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
      } else
         aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> ().enabled = true;

      aimPoint.GetComponent<SpriteRenderer> ().enabled = true;
   }
   public void End () {
      if (aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> () == null) {
         Debug.LogError ("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
      } else
         aimArrowHolder.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;

      aimPoint.GetComponent<SpriteRenderer> ().enabled = false;

   }
   private void OnDrawGizmos () {
      Gizmos.DrawWireSphere (hit[0].centroid, _circleCastRadius);
      Debug.DrawLine (transform.position, hit[0].point);

   }
   public Transform Run (Vector2 aimDelta, Vector2 grapPos) {
      float playerAngle = Mathf.Atan2 (transform.right.y, transform.right.x) * Mathf.Rad2Deg;
      //Debug.Log ("playerAngle: " + playerAngle);
      float deltaAngle = Mathf.Atan2 (aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
      // Debug.Log ("deltaAngle: " + deltaAngle);
      float angle = deltaAngle + playerAngle;
      Debug.Log (angle);
      //rotate aimArrow to the joystick direction
      aimArrowHolder.rotation = Quaternion.Lerp (aimArrowHolder.rotation,
         Quaternion.Euler (0, 0, angle - 90), 0.5f);
      //shoot circleCast in direction of joystick
      //Vector2 dir = new Vector2(aimDelta.x + transform.right.x, aimDelta.y + transform.right.y);
      Vector2 dir = new Vector2 (math.cos (math.radians (angle)), math.sin (math.radians (angle)));
      Debug.Log (dir);
      Debug.DrawLine (transform.position, dir);
      int hitCount = Physics2D.CircleCastNonAlloc (
         transform.position, _circleCastRadius, dir, hit, _grapLength, _grapPointsMask);

      //change grap posiotion only if found new target
      if (hitCount != 0) {
         _grapPoint = hit[0].transform;
      }

      transform.position += transform.up * _speed * Time.deltaTime;
      if (_grapPoint == null) {
         return null;
      }
      SetGrapPoint (_grapPoint);
      return _grapPoint;
   }

   private void SetGrapPoint (Transform point) {
      aimPoint.position = point.position;
   }

}