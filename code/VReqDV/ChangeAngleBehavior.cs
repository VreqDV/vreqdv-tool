// using UnityEngine;

// [CreateAssetMenu(fileName = "NewAngleChangeBehavior", menuName = "Behaviors/AngleChange")]
// public class AngleChangeBehavior : ResponseTemplate
// {
//     public Vector3 newAngle;  // Target rotation
//     public float duration = 1.0f; // Duration of rotation in seconds

//     public override void Execute(GameObject obj)
//     {
//         Rotator rotator = obj.GetComponent<Rotator>();
//         if (rotator == null)
//         {
//             rotator = obj.AddComponent<Rotator>(); // Attach the script dynamically if not present
//         }
//         rotator.RotateTo(Quaternion.Euler(newAngle), duration);
//     }
// }
