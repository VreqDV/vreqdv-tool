# VReqDV: Model Based VR Scene Design Generation and Versioning Tool

## Steps to Reproduce: Bowling Alley Scene

1. Download and unzip the source code from the GitHub repository - https://github.com/VreqDV/vreqdv-tool
2. Open Unity Hub on your local machine (version 2022.3.9f1 or later) and create a new project. Select VR template.
Note: If you do not have a VR headset to deploy the scene, you can select Unity 3D Core template instead of VR.
3. In the Unity Editor, under the Project tab, locate the Assets folder. Place the src folder from the downloaded and unzipped source code here.
4. The src folder contains VReqST specifications for the Bowling Alley scene. These can be modified according to the user’s requirements. Sample specifications can also be found [here](https://vreqdv.github.io/vreqdv-tool/#vreqst-specifications-of-bowling-alley-scene).
5. Save the project.
6. At the top of the Unity Editor, locate and click on the menu Window in the toolbar. Select VReqDV. VReqDV tab is opened. (Refer Figure Below) You can see the specifications appearing at the left in the window, labeled version 1. On selecting 'Display Mock-up', the mock-up scene for version 1 is generated and displayed on the scene editor window.

![image](https://github.com/user-attachments/assets/bdf46bcb-f59b-422b-93d5-ef0532ddce01)


## VReqST specifications of Bowling Alley Scene

### Scene Properties - scene.json

```
{
   "_scenename":"Bowlling Alley",
   "_sid":"45189",
   "_slabel":"This is a interactive game",
      "_playarea":{
      "#pid":"p1",
      "#length":"50",
      "#breadth":"50",
      "#height":"50",
	  "#comment":"Values in feets",
	  "#x_scenecenter":0,
      "#y_scenecenter":5,
      "#z_scenecenter":0
	  },   
   "_camera":
  { 
     "IsSceneObject":true,
     "trackingorigin":"floor"
   },
   "_initialcamerapos":{
      "#x_initialcamerapos":"0",
      "#y_initialcamerapos":"5",
      "#z_initialcamerapos":"0"
   },
   "_viewport":{
      "#x_viewport":"0",
      "#y_viewport":"0",
      "#w_viewport":"1",
	  "#h_viewport":"1"
	 },
	"_clippingplane":{
	"near_cp":0.01,
	"far_cp":1000
	}, 
   "_horizon":true,
   "_dof":6,
   "_skybox":1,
   "_controllers":
	{
		"type":"hand", "raycast":true, "raydistance":10, "raythinkness":1, "raycolor":"red", "raytype":"curve"
		},
   "_gravity":{
     "value":10
   },
   "_interaction":true,
   "_nestedscene":{
      "#value":false,
      "#scenecount":0,
      "#sid_order":0
   },
   "_audio":true,
   "_timeline":true,
   "_Opttxt1":"null",
   "@context_mock":"_blank",
   "usertype":[
   {
	 "type": "single",
	 "uplayarea":{
	 "#length_uplayarea":20,
     "#breadth_uplayarea":20,
	 "#height_uplayarea":50
	},
	"initialupos":{
      "#x_initialupos":-10,
      "#y_initialupos":0,
      "#z_initialupos":0
		},
		"uplayareacenter":{
	  "#x_uplayareacenter":-10,
      "#y_uplayareacenter":10,
      "#z_uplayareacenter":0
	  }
   },
	  {
	 "type": "multi",
	 "uplayarea":{
	 "#length_uplayarea":25,
     "#breadth_uplayarea":25,
	 "#height_uplayarea":50
	},
	"initialupos":{
      "#x_initialupos":-10,
      "#y_initialupos":0,
      "#z_initialupos":0
		},
		"uplayareacenter":{
	  "#x_uplayareacenter":-10,
      "#y_uplayareacenter":10,
      "#z_uplayareacenter":0
	  }
	  }
	]
}
```

### Object Properties - article.json

```
{
	"articles": [
		{
			"_objectname": "Ball",
			"_sid": "_gameball",
			"_slabel": "This is used to roll on pins",
			"_IsHidden": 0,
			"_enumcount": 1,
			"_Is3DObject": 1,
			"HasChild": 0,
			"shape": "sphere",
			"dimension": {
				"dradii": 5,
				"dvolumn": "null",
				"dlength": "null",
				"dbreadth": "null",
				"dheigth": "null"
			},
			"IsText": false,
			"IsText3D": false,
			"lighting": {
				"CastShadow": "1",
				"ReceiveShadow": "1",
				"ContributeGlobalIlumination": "1"
			},
			"IsIlluminate": true,
			"Transform_initialpos": {
				"x": "0",
				"y": "0.5",
				"z": "0"
			},
			"Transform_initialrotation": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_objectscale": {
				"x": "1",
				"y": "1",
				"z": "1"
			},
			"repeattransfrom": {
				"distfactorx": "null",
				"distfactory": "null",
				"distfactorz": "null"
			},
			"Interaction": {
				"XRGrabInteractable": "1",
				"XRInteractionMaskLayer": "nothing/everything/custom",
				"TrackPosition": "1",
				"TrackRotation": "1",
				"Throw_Detach": "1",
				"forcegravity": "1",
				"velocity": "3",
				"angularvelocity": "2"
			},
			"Smoothing": "1",
			"Smoothing_duration": "0.5",
			"attachtransform": {
				"rotate_x": "null",
				"rotate_y": "null",
				"rotate_z": "null",
				"pos_x": "null",
				"pos_y": "null",
				"pos_z": "null"
			},
			"XRRigidObject": {
				"value": "1",
				"mass": "20",
				"dragfriction": "5",
				"angulardrag": "4",
				"Isgravityenable": "true",
				"IsKinematic": "1",
				"CanInterpolate": "0",
				"CollisionPolling": "discreet/continous"
			},
			"aud_hasaudio": "1",
			"aud_type": "static/dynamic",
			"aud_src": "/path",
			"aud_volume": "10",
			"aud_PlayInloop": "0",
			"aud_IsSurround": "1",
			"aud_Dopplerlevel": "0.9",
			"aud_spread": "null",
			"aud_mindist": "null",
			"aud_maxdist": "null",
			"_Opttxt1": "null",
			"@context_img_source": "blank"
		},
		{
			"_objectname": "Plane",
			"_sid": "_pitch",
			"_slabel": "This is where the ball rolls",
			"_IsHidden": 0,
			"_enumcount": 1,
			"_Is3DObject": 1,
			"HasChild": 0,
			"shape": "cube",
			"dimension": {
				"dradii": 5,
				"dvolumn": "null",
				"dlength": "null",
				"dbreadth": "null",
				"dheigth": "null"
			},
			"IsText": false,
			"IsText3D": false,
			"lighting": {
				"CastShadow": "1",
				"ReceiveShadow": "1",
				"ContributeGlobalIlumination": "1"
			},
			"IsIlluminate": true,
			"Transform_initialpos": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_initialrotation": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_objectscale": {
				"x": "20",
				"y": "0.1",
				"z": "5"
			},
			"repeattransfrom": {
				"distfactorx": "null",
				"distfactory": "null",
				"distfactorz": "null"
			},
			"Interaction": {
				"XRGrabInteractable": "1",
				"XRInteractionMaskLayer": "nothing/everything/custom",
				"TrackPosition": "1",
				"TrackRotation": "1",
				"Throw_Detach": "1",
				"forcegravity": "1",
				"velocity": "3",
				"angularvelocity": "2"
			},
			"Smoothing": "1",
			"Smoothing_duration": "0.5",
			"attachtransform": {
				"rotate_x": "null",
				"rotate_y": "null",
				"rotate_z": "null",
				"pos_x": "null",
				"pos_y": "null",
				"pos_z": "null"
			},
			"XRRigidObject": {
				"value": "1",
				"mass": "20",
				"dragfriction": "5",
				"angulardrag": "4",
				"Isgravityenable": "true",
				"IsKinematic": "1",
				"CanInterpolate": "0",
				"CollisionPolling": "discreet/continous"
			},
			"aud_hasaudio": "1",
			"aud_type": "static/dynamic",
			"aud_src": "/path",
			"aud_volume": "10",
			"aud_PlayInloop": "0",
			"aud_IsSurround": "1",
			"aud_Dopplerlevel": "0.9",
			"aud_spread": "null",
			"aud_mindist": "null",
			"aud_maxdist": "null",
			"_Opttxt1": "null",
			"@context_img_source": "blank"
		},
		{
			"_objectname": "Pin_1",
			"_sid": "_pinsetter5",
			"_slabel": "This is pin",
			"_IsHidden": 0,
			"_enumcount": 1,
			"_Is3DObject": 1,
			"HasChild": 0,
			"shape": "sphere",
			"dimension": {
				"dradii": 5,
				"dvolumn": "null",
				"dlength": "null",
				"dbreadth": "null",
				"dheigth": "null"
			},
			"IsText": false,
			"IsText3D": false,
			"lighting": {
				"CastShadow": "1",
				"ReceiveShadow": "1",
				"ContributeGlobalIlumination": "1"
			},
			"IsIlluminate": true,
			"Transform_initialpos": {
				"x": "5",
				"y": "0.5",
				"z": "0"
			},
			"Transform_initialrotation": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_objectscale": {
				"x": "0.2",
				"y": "1",
				"z": "0.2"
			},
			"repeattransfrom": {
				"distfactorx": "null",
				"distfactory": "null",
				"distfactorz": "null"
			},
			"Interaction": {
				"XRGrabInteractable": "1",
				"XRInteractionMaskLayer": "nothing/everything/custom",
				"TrackPosition": "1",
				"TrackRotation": "1",
				"Throw_Detach": "1",
				"forcegravity": "1",
				"velocity": "3",
				"angularvelocity": "2"
			},
			"Smoothing": "1",
			"Smoothing_duration": "0.5",
			"attachtransform": {
				"rotate_x": "null",
				"rotate_y": "null",
				"rotate_z": "null",
				"pos_x": "null",
				"pos_y": "null",
				"pos_z": "null"
			},
			"XRRigidObject": {
				"value": "1",
				"mass": "20",
				"dragfriction": "5",
				"angulardrag": "4",
				"Isgravityenable": "true",
				"IsKinematic": "1",
				"CanInterpolate": "0",
				"CollisionPolling": "discreet/continous"
			},
			"aud_hasaudio": "1",
			"aud_type": "static/dynamic",
			"aud_src": "/path",
			"aud_volume": "10",
			"aud_PlayInloop": "0",
			"aud_IsSurround": "1",
			"aud_Dopplerlevel": "0.9",
			"aud_spread": "null",
			"aud_mindist": "null",
			"aud_maxdist": "null",
			"_Opttxt1": "null",
			"@context_img_source": "blank"
		},
		{
			"_objectname": "Pin_2",
			"_sid": "_pinsetter5",
			"_slabel": "This is pin",
			"_IsHidden": 0,
			"_enumcount": 1,
			"_Is3DObject": 1,
			"HasChild": 0,
			"shape": "sphere",
			"dimension": {
				"dradii": 5,
				"dvolumn": "null",
				"dlength": "null",
				"dbreadth": "null",
				"dheigth": "null"
			},
			"IsText": false,
			"IsText3D": false,
			"lighting": {
				"CastShadow": "1",
				"ReceiveShadow": "1",
				"ContributeGlobalIlumination": "1"
			},
			"IsIlluminate": true,
			"Transform_initialpos": {
				"x": "5.5",
				"y": "0.5",
				"z": "1"
			},
			"Transform_initialrotation": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_objectscale": {
				"x": "0.2",
				"y": "1",
				"z": "0.2"
			},
			"repeattransfrom": {
				"distfactorx": "null",
				"distfactory": "null",
				"distfactorz": "null"
			},
			"Interaction": {
				"XRGrabInteractable": "1",
				"XRInteractionMaskLayer": "nothing/everything/custom",
				"TrackPosition": "1",
				"TrackRotation": "1",
				"Throw_Detach": "1",
				"forcegravity": "1",
				"velocity": "3",
				"angularvelocity": "2"
			},
			"Smoothing": "1",
			"Smoothing_duration": "0.5",
			"attachtransform": {
				"rotate_x": "null",
				"rotate_y": "null",
				"rotate_z": "null",
				"pos_x": "null",
				"pos_y": "null",
				"pos_z": "null"
			},
			"XRRigidObject": {
				"value": "1",
				"mass": "20",
				"dragfriction": "5",
				"angulardrag": "4",
				"Isgravityenable": "true",
				"IsKinematic": "1",
				"CanInterpolate": "0",
				"CollisionPolling": "discreet/continous"
			},
			"aud_hasaudio": "1",
			"aud_type": "static/dynamic",
			"aud_src": "/path",
			"aud_volume": "10",
			"aud_PlayInloop": "0",
			"aud_IsSurround": "1",
			"aud_Dopplerlevel": "0.9",
			"aud_spread": "null",
			"aud_mindist": "null",
			"aud_maxdist": "null",
			"_Opttxt1": "null",
			"@context_img_source": "blank"
		},
		{
			"_objectname": "Pin_3",
			"_sid": "_pinsetter5",
			"_slabel": "This is pin",
			"_IsHidden": 0,
			"_enumcount": 1,
			"_Is3DObject": 1,
			"HasChild": 0,
			"shape": "sphere",
			"dimension": {
				"dradii": 5,
				"dvolumn": "null",
				"dlength": "null",
				"dbreadth": "null",
				"dheigth": "null"
			},
			"IsText": false,
			"IsText3D": false,
			"lighting": {
				"CastShadow": "1",
				"ReceiveShadow": "1",
				"ContributeGlobalIlumination": "1"
			},
			"IsIlluminate": true,
			"Transform_initialpos": {
				"x": "5.5",
				"y": "0.5",
				"z": "-1"
			},
			"Transform_initialrotation": {
				"x": "0",
				"y": "0",
				"z": "0"
			},
			"Transform_objectscale": {
				"x": "0.2",
				"y": "1",
				"z": "0.2"
			},
			"repeattransfrom": {
				"distfactorx": "null",
				"distfactory": "null",
				"distfactorz": "null"
			},
			"Interaction": {
				"XRGrabInteractable": "1",
				"XRInteractionMaskLayer": "nothing/everything/custom",
				"TrackPosition": "1",
				"TrackRotation": "1",
				"Throw_Detach": "1",
				"forcegravity": "1",
				"velocity": "3",
				"angularvelocity": "2"
			},
			"Smoothing": "1",
			"Smoothing_duration": "0.5",
			"attachtransform": {
				"rotate_x": "null",
				"rotate_y": "null",
				"rotate_z": "null",
				"pos_x": "null",
				"pos_y": "null",
				"pos_z": "null"
			},
			"XRRigidObject": {
				"value": "1",
				"mass": "20",
				"dragfriction": "5",
				"angulardrag": "4",
				"Isgravityenable": "true",
				"IsKinematic": "1",
				"CanInterpolate": "0",
				"CollisionPolling": "discreet/continous"
			},
			"aud_hasaudio": "1",
			"aud_type": "static/dynamic",
			"aud_src": "/path",
			"aud_volume": "10",
			"aud_PlayInloop": "0",
			"aud_IsSurround": "1",
			"aud_Dopplerlevel": "0.9",
			"aud_spread": "null",
			"aud_mindist": "null",
			"aud_maxdist": "null",
			"_Opttxt1": "null",
			"@context_img_source": "blank"
		}
	]
}
```

### Action Responses - action-response.json

```
{
    "ObjAction": [
        {
            "actresid": "ball_roll_on_click",
            "trigger_event": {
                "sourceObj": "Ball",
                "IsCollision": "false",
                "action": "input",
                "force": {},
                "disappear": "none",
                "inputType": "click",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Ball",
                "IsCollision": "false",
                "response": "force",
                "force": {
                    "direction": "forward",
                    "strength": "10",
                    "type": "impulse"
                },
                "disappear": "none",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "ball_pin",
            "trigger_event": {
                "sourceObj": "Ball",
                "IsCollision": "true",
                "action": "none",
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_1",
                "IsCollision": "true",
                "response": "none",
                "force": {},
                "disappear": "none",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "ball_pin2",
            "trigger_event": {
                "sourceObj": "Ball",
                "IsCollision": "true",
                "action": "none",
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_2",
                "IsCollision": "true",
                "response": "none",
                "force": {},
                "disappear": "none",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "ball_pin3",
            "trigger_event": {
                "sourceObj": "Ball",
                "IsCollision": "true",
                "action": "none",
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_3",
                "IsCollision": "true",
                "response": "none",
                "force": {},
                "disappear": "none",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "pin_fall_disappear",
            "trigger_event": {
                "sourceObj": "Pin_1",
                "IsCollision": "false",
                "action": "change",
                "change_property_by": {
                    "Transform_initialrotation": {
                        "x": "90",
                        "y": "90",
                        "z": "90"
                    }
                },
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_1",
                "IsCollision": "false",
                "response": "disappear",
                "force": {},
                "disappear": "target",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "pin_fall_disappear2",
            "trigger_event": {
                "sourceObj": "Pin_2",
                "IsCollision": "false",
                "action": "change",
                "change_property_by": {
                    "Transform_initialrotation": {
                        "x": "90",
                        "y": "90",
                        "z": "90"
                    }
                },
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_2",
                "IsCollision": "false",
                "response": "disappear",
                "force": {},
                "disappear": "target",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        },
        {
            "actresid": "pin_fall_disappear3",
            "trigger_event": {
                "sourceObj": "Pin_3",
                "IsCollision": "false",
                "action": "change",
                "change_property_by": {
                    "Transform_initialrotation": {
                        "x": "90",
                        "y": "90",
                        "z": "90"
                    }
                },
                "force": {},
                "disappear": "none",
                "inputType": "none",
                "repeatactionfor": ""
            },
            "response_event": {
                "targetObj": "Pin_3",
                "IsCollision": "false",
                "response": "disappear",
                "force": {},
                "disappear": "target",
                "outputType": "none",
                "repeatactionfor": ""
            },
            "comment": "<freetext>",
            "Syncronous": "true"
        }
    ]
}
```

### Timeline - timeline.json

```
```

