using Sirenix.OdinInspector;
using UnityEngine;

public static class MathUtilities2D
{
    public static void RotateTransformToPointWithSpeed(Transform transformToRotate, Vector2 center, Vector2 point, float speed)
    {
        Vector2 directionToPoint = new Vector2(point.x - center.x,
         point.y - center.y);

        float angleToPoint = Mathf.Atan2(directionToPoint.y, directionToPoint.x) * Mathf.Rad2Deg;
        RotateTransformToAngleWithSpeed(transformToRotate, angleToPoint, speed);
    }

    public static void RotateTransformToAngleWithSpeed(Transform transformToRotate, float angle, float speed)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, angle);

        float angleDelta = Mathf.Abs(Quaternion.Angle(transformToRotate.rotation, newRotation));
        transformToRotate.rotation = Quaternion.RotateTowards(
            transformToRotate.rotation, newRotation, angleDelta * speed);
    }

    public static float GetAngleToAlignDirection(Vector2 direction, bool normalizedTo360 = true)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (normalizedTo360)
        {
            return NormalizeAngleTo360(angle);
        }
        else
        {
            return angle;
        }
    }

    public static float NormalizeAngleTo360(float angle)
    {
        float normalizedAngle = angle;
        while (normalizedAngle < 0)
        {
            normalizedAngle = 360 + normalizedAngle;
        }
        while(normalizedAngle > 360)
        {
            normalizedAngle = normalizedAngle - 360;
        }
        return normalizedAngle;
    }

    public static Vector2 GetNormal(Vector2 direction)
    {
        return Vector3.Cross(direction, Vector3.forward);
    }

    public static Vector2 GetDirectionFromTo(Vector2 from, Vector2 to)
    {
        return to - from;
    }

    public static Vector2 GetPositionToRotatePointAroundOrigin(
        Vector2 point, Vector2 origin, float anglesRad, bool clockwise)
    {
        if (!clockwise)
        {
            anglesRad = -anglesRad;
        }
        Vector2 newPosition;
        newPosition.x = (Mathf.Cos(anglesRad) * (point.x - origin.x))
             - (Mathf.Sin(anglesRad) * (point.y - origin.y)) + origin.x;
        newPosition.y = (Mathf.Sin(anglesRad) * (point.x - origin.x))
            + (Mathf.Cos(anglesRad) * (point.y - origin.y)) + origin.y;
        return newPosition;

    }

    public static float GetRotationTowards(float currentAngle, float targetAngle, float rotationSpeed)
    {
        float anglesDiff = GetAnglesDiff(currentAngle, targetAngle, signed: true);
        float sign = anglesDiff < 0 ? -1 : 1;
        float anglesAddCount = Mathf.Clamp(Mathf.Abs(anglesDiff), 0, rotationSpeed);

        float newAngle = currentAngle - (anglesAddCount * sign);
        newAngle = NormalizeAngleTo360(newAngle);
        return newAngle;
    }

    public static float GetAnglesDiff(float angle1, float angle2, bool signed = false)
    {
        angle1 = NormalizeAngleTo360(angle1);
        angle2 = NormalizeAngleTo360(angle2);

        float diff = Mathf.Abs(angle1 - angle2);

        if(diff > 180)
        {
            if(angle1 < angle2)
            {
                angle1 += 360;
            }
            else
            {
                angle2 += 360;
            }
        }

        diff = angle1 - angle2;
        if (signed)
        {
            return diff;
        }
        else
        {
            return Mathf.Abs(diff);
        }
        

    }
}
