using UnityEngine;

namespace Version_15
{
    public static class UserAlgorithms
    {
        public static bool IsBallClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var obj = GameObject.Find("Ball");
                    if (obj != null && hit.collider.gameObject == obj)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void RollBall()
        {
            GameObject obj = GameObject.Find("Ball");
            Debug.Log($"[UserAlgorithms] Rolling Ball: {obj.name}");
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(new Vector3(10f, 0f, 0f), ForceMode.Impulse); // Adjust force as needed
            }
            BallStateAPI.SetRolling(obj);
        }

        public static bool HasBallReachedEnd()
        {
            GameObject obj = GameObject.Find("Ball");
            return obj.transform.position.x > 7.0f; 
        }

        public static void StopAndResetBall()
        {
            GameObject obj = GameObject.Find("Ball");
            Debug.Log($"[UserAlgorithms] Stopping Ball: {obj.name}");
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // Stop physics
            }
            obj.transform.position = new Vector3(0, 0.5f, 0); 
            obj.transform.rotation = Quaternion.identity;
            BallStateAPI.SetReady(obj);
        }
        
        public static bool IsPinFallen(GameObject obj)
        {
            float angle = Vector3.Angle(obj.transform.up, Vector3.up);
            if (angle > 45.0f)
            {
                return true;
            }
            return false;
        }

        public static void SetPinFallen(GameObject obj)
        {
            Debug.Log($"[UserAlgorithms] Pin Fallen: {obj.name}");
            // Add scoring logic here
            if (obj.name == "Pin_1") Pin_1StateAPI.SetFallen(obj);
            else if (obj.name == "Pin_2") Pin_2StateAPI.SetFallen(obj);
            else if (obj.name == "Pin_3") Pin_3StateAPI.SetFallen(obj);
        }

        public static void ResetBallAndPins()
        {
            Debug.Log("[UserAlgorithms] Resetting Game (State + Transform)...");

            // Reset Ball
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                ball.transform.position = new Vector3(0.73f, 0.5f, 0);
                ball.transform.rotation = Quaternion.identity;
                
                var rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true; 
                }
                
                BallStateAPI.SetReady(ball);
            }

            // Reset Pins
            string[] pinNames = { "Pin_1", "Pin_2", "Pin_3" };
            foreach (var pinName in pinNames)
            {
                GameObject pin = GameObject.Find(pinName);
                if (pin != null)
                {
                    pin.transform.rotation = Quaternion.identity;
                    
                    var rb = pin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    if (pinName == "Pin_1") Pin_1StateAPI.SetStanding(pin);
                    else if (pinName == "Pin_2") Pin_2StateAPI.SetStanding(pin);
                    else if (pinName == "Pin_3") Pin_3StateAPI.SetStanding(pin);
                }
            }
        }
    }
}
