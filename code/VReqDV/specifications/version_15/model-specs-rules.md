# JSON Model Specification Documentation

This document explains how to write `article.json` and `behavior.json` files to define objects and behaviors in the application.

## 1. article.json

The `article.json` file defines the GameObjects to be instantiated or managed in the scene. It contains a list of `articles`.

### Structure
```json
{
  "articles": [
    {
      "_objectname": "ObjectName",
      ... properties ...
    }
  ]
}
```

### Fields

| Field | Type | Description |
| :--- | :--- | :--- |
| `_objectname` | `string` | **Required.** The unique name of the GameObject. |
| `source` | `string` | The name of another article to inherit properties from. If defined, this object copies all fields from the source, which can then be overridden. |
| `shape` | `string` | The primitive shape or type. Valid values: `"Cube"`, `"Sphere"`, `"Capsule"`, `"Cylinder"`, `"Plane"`, `"Quad"`, `"empty"`. |
| `Transform_initialpos` | `object` | Position coordinates `{ "x": "0.0", "y": "0.0", "z": "0.0" }`. |
| `Transform_initialrotation` | `object` | Rotation Euler angles `{ "x": "0.0", "y": "0.0", "z": "0.0" }`. |
| `Transform_objectscale` | `object` | Scale `{ "x": "1.0", "y": "1.0", "z": "1.0" }`. |
| `states` | `string[]` | A list of state names (e.g., `["ready", "rolling"]`). These generate a state machine enum for the object. |
| `context_img_source` | `string` | (Optional) Path to a prefab asset (e.g., `Assets/Prefabs/MyObject.prefab`). If provided, this prefab is instantiated instead of a primitive shape. |
| `HasChild` | `int` | Set to `1` to indicate this object is a group parent. Set to `0` otherwise. |
| `Children` | `string[]` | If `HasChild` is `1`, this list contains names of *existing* objects to group under this parent. The system will calculate the center and reparent them. |
| `XRRigidObject` | `object` | Configuration for the Rigidbody component. See below. |

### XRRigidObject Properties

| Field | Type | Description |
| :--- | :--- | :--- |
| `value` | `string` | "1" to add a Rigidbody, "0" to skip. |
| `mass` | `string` | Mass of the object (float). |
| `dragfriction` | `string` | Linear drag (float). |
| `angulardrag` | `string` | Angular drag (float). |
| `Isgravityenable` | `string` | "true" or "false". |
| `IsKinematic` | `string` | "true" or "false". |
| `CollisionPolling` | `string` | Collision detection mode. <br>Values: `"discrete"`, `"continuous"`, `"continuous-dynamic"`, `"continuous-speculative"`. |
| `CanInterpolate` | `string` | Interpolation mode. <br>`"0"`: None<br>`"1"`: Interpolate<br>`"2"`: Extrapolate |

### Inheritance Logic
If `source` is specified:
1. The system finds the source article.
2. It copies `shape`, `Transform_*`, `XRRigidObject`, `states`, `context_img_source`, and child settings from the source.
3. Any fields explicitly defined in the current article override the copied values.

---

## 2. behavior.json

The `behavior.json` file defines the logic rules, event triggers, and actions for the objects.

### Structure
```json
{
  "behaviors": [
    {
      "id": "BehaviorID",
      "event": "OnCondition",
      ...
    }
  ]
}
```

### Fields

| Field | Type | Description |
| :--- | :--- | :--- |
| `id` | `string` | **Required.** Unique Identifier for this behavior. used as the generated class name. |
| `event` | `string` | The trigger type.<br>`"OnCondition"`: Checks `precondition` every frame (Update loop).<br>`"OnStateChange"`: Triggered when `source`'s state changes. |
| `source` | `string` | The name of the object this behavior belongs to (or listens to). |
| `precondition` | `ConditionNode` | Logic tree that must evaluate to `true` for the action to run. |
| `action` | `ActionNode` | The action to execute if the event and precondition are met. |
| `postcondition` | `ConditionNode` | (Optional) Defines the expected state after the action. (Currently for documentation/verification). |

### ConditionNode
A condition node can check states, run algorithms, or combine other occurrences.

*   **Logical Operators:**
    *   `"all": [ ... ]` - valid if **ALL** child nodes are true (AND).
    *   `"any": [ ... ]` - valid if **ANY** child node is true (OR).

*   **State/Value Checks:**
    *   `"equals": ["Left", "Right"]` - Checks equality.
    *   **Special Syntax:** If "Left" ends in `.state` (e.g., `"Ball.state"`), it checks the runtime state machine. "Right" should be the state name (e.g., `"rolling"`).

*   **Algorithms:**
    *   `"runAlgorithm": "MethodName"` - Calls a static boolean method in `UserAlgorithms` class.
    *   `"params": { "key": "value" }` - Arguments for the method. If value is `"obj"`, it is often used loosely to refer to the object name.

### ActionNode
Defines what happens.

*   `"runAlgorithm": "MethodName"` - Calls a static void method in `UserAlgorithms` class.
*   `"params": { ... }` - Arguments passed to the method.

### Example Behavior
```json
{
    "id": "PinFall",
    "event": "OnCondition",
    "source": "Pin_1",
    "precondition": {
        "all": [
            { "equals": ["Pin_1.state", "standing"] },
            { "runAlgorithm": "IsPinFallen", "params": { "obj": "Pin_1" } }
        ]
    },
    "action": {
        "runAlgorithm": "SetPinFallen",
        "params": { "obj": "Pin_1" }
    }
}
```
This checks if `Pin_1` is "standing" AND `IsPinFallen` returns true. If so, it runs `SetPinFallen`.

### Inheritance in Behaviors
If `source` in `article.json` establishes an inheritance (e.g., `Pin_2` sources `Pin_1`), the behavior system can expand rules.
*   If a rule references `Pin_1` as source, and `Pin_2` inherits from `Pin_1`, the system generates a **copy** of the rule for `Pin_2`.
*   It replaces all occurrences of "Pin_1" with "Pin_2" in the `id`, `source`, `precondition`, `action`, and `postcondition`.
