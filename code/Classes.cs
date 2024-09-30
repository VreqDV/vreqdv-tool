using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneConfig
{
    public string scenename;
    public string sid;
    public string slabel;
    public PlayArea playarea;
    public CameraSettings camera;
    public InitialCameraPos initialcamerapos;
    public Viewport viewport;
    public ClippingPlane clippingplane;
    public bool horizon;
    public int dof;
    public int skybox;
    public Controllers controllers;
    public Gravity gravity;
    public bool interaction;
    public NestedScene nestedscene;
    public bool audio;
    public bool timeline;
    public string Opttxt1;
    public string context_mock;
    public List<UserType> usertype;
}

[System.Serializable]
public class PlayArea
{
    public string pid;
    public float length;
    public float breadth;
    public float height;
    public string comment;
    public float x_scenecenter;
    public float y_scenecenter;
    public float z_scenecenter;
}

[System.Serializable]
public class CameraSettings
{
    public bool IsSceneObject;
    public string trackingorigin;
}

[System.Serializable]
public class InitialCameraPos
{
    public float x_initialcamerapos;
    public float y_initialcamerapos;
    public float z_initialcamerapos;
}

[System.Serializable]
public class Viewport
{
    public float x_viewport;
    public float y_viewport;
    public float w_viewport;
    public float h_viewport;
}

[System.Serializable]
public class ClippingPlane
{
    public float near_cp;
    public float far_cp;
}

[System.Serializable]
public class Controllers
{
    public string type;
    public bool raycast;
    public float raydistance;
    public float raythinkness;
    public string raycolor;
    public string raytype;
}

[System.Serializable]
public class Gravity
{
    public float value;
}

[System.Serializable]
public class NestedScene
{
    public bool value;
    public int scenecount;
    public int sid_order;
}

[System.Serializable]
public class UserType
{
    public string type;
    public UPlayArea uplayarea;
    public InitialUPos initialupos;
    public UPlayAreaCenter uplayareacenter;
}

[System.Serializable]
public class UPlayArea
{
    public float length_uplayarea;
    public float breadth_uplayarea;
    public float height_uplayarea;
}

[System.Serializable]
public class InitialUPos
{
    public float x_initialupos;
    public float y_initialupos;
    public float z_initialupos;
}

[System.Serializable]
public class UPlayAreaCenter
{
    public float x_uplayareacenter;
    public float y_uplayareacenter;
    public float z_uplayareacenter;
}


[System.Serializable]
public class ActionResponseList
{
    public List<ActionResponse> ObjAction;
}

[System.Serializable]
public class TriggerEvent
{
    public string sourceObj;
    public string IsCollision;
    public string action;
    public object change_property_by;
    public object force;
    public string disappear;
    public string inputType;
    public string repeatactionfor;
}

[System.Serializable]
public class ResponseEvent
{
    public string targetObj;
    public string IsCollision;
    public string response;
    public object force;
    public string disappear;
    public string outputType;
    public string repeatactionfor;
}

// [System.Serializable]
// public class Force
// {
//     // Define force properties if needed
// }

[System.Serializable]
public class ActionResponse
{
    public string actresid;
    public TriggerEvent trigger_event;
    public ResponseEvent response_event;
    public string comment;
    public string Syncronous;
}

// public class property_change
// {

// }
public class ArticleList
{
    public List<Article> articles { get; set; }
}

public class Article
{
    public string _objectname { get; set; }
    public string _sid { get; set; }
    public string _slabel { get; set; }
    public int _IsHidden { get; set; }
    public int _enumcount { get; set; }
    public int _Is3DObject { get; set; }
    public int HasChild { get; set; }
    public string shape { get; set; }
    public Dimension dimension { get; set; }
    public bool IsText { get; set; }
    public bool IsText3D { get; set; }
    public Lighting lighting { get; set; }
    public bool IsIlluminate { get; set; }
    public TransformData Transform_initialpos { get; set; }
    public TransformData Transform_initialrotation { get; set; }
    public TransformData Transform_objectscale { get; set; }
    public RepeatTransform repeattransfrom { get; set; }
    public Interaction Interaction { get; set; }
    public string Smoothing { get; set; }
    public string Smoothing_duration { get; set; }
    public AttachTransform attachtransform { get; set; }
    public XRRigidObject XRRigidObject { get; set; }
    public string aud_hasaudio { get; set; }
    public string aud_type { get; set; }
    public string aud_src { get; set; }
    public string aud_volume { get; set; }
    public string aud_PlayInloop { get; set; }
    public string aud_IsSurround { get; set; }
    public string aud_Dopplerlevel { get; set; }
    public string aud_spread { get; set; }
    public string aud_mindist { get; set; }
    public string aud_maxdist { get; set; }
    public string _Opttxt1 { get; set; }
    public string @context_img_source { get; set; }
}

public class Dimension
{
    public float dradii { get; set; }
    public string dvolumn { get; set; }
    public string dlength { get; set; }
    public string dbreadth { get; set; }
    public string dheigth { get; set; }
}

public class Lighting
{
    public string CastShadow { get; set; }
    public string ReceiveShadow { get; set; }
    public string ContributeGlobalIlumination { get; set; }
}

public class TransformData
{
    public string x { get; set; }
    public string y { get; set; }
    public string z { get; set; }
}

public class RepeatTransform
{
    public string distfactorx { get; set; }
    public string distfactory { get; set; }
    public string distfactorz { get; set; }
}

public class Interaction
{
    public string XRGrabInteractable { get; set; }
    public string XRInteractionMaskLayer { get; set; }
    public string TrackPosition { get; set; }
    public string TrackRotation { get; set; }
    public string Throw_Detach { get; set; }
    public string forcegravity { get; set; }
    public string velocity { get; set; }
    public string angularvelocity { get; set; }
}

public class AttachTransform
{
    public string rotate_x { get; set; }
    public string rotate_y { get; set; }
    public string rotate_z { get; set; }
    public string pos_x { get; set; }
    public string pos_y { get; set; }
    public string pos_z { get; set; }
}

public class XRRigidObject
{
    public string value { get; set; }
    public string mass { get; set; }
    public string dragfriction { get; set; }
    public string angulardrag { get; set; }
    public string Isgravityenable { get; set; }
    public string IsKinematic { get; set; }
    public string CanInterpolate { get; set; }
    public string CollisionPolling { get; set; }
}
