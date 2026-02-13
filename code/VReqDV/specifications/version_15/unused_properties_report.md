# Unused Properties Report

This report identifies properties defined in `article.json` and `behavior.json` (via `Classes.cs`) that are not implemented in the active codebase (`ObjectHandler.cs`, `BehaviorCodeGenerator.cs`, `StateCodeGenerator.cs`, `VReqDV.cs`).

An analysis of `Assets/Editor/actionHandler.cs` was also performed to check for deprecated usage.

## 1. article.json (Article Class)

The following properties are defined in the schema but have varying levels of implementation.

### A. Completely Unused
These fields are **not used anywhere**. They are not read by the scene generator (`ObjectHandler.cs`) nor displayed/edited in the Editor window (`VReqDV.cs`).

*   **dimension** (and all sub-properties: `dradii`, `dvolumn`, `dlength`, `dbreadth`, `dheigth`)
*   **IsText**
*   **IsText3D**
*   **IsIlluminate**
*   **repeattransfrom** (and all sub-properties: `distfactorx`, `distfactory`, `distfactorz`)
*   **Interaction** (and all sub-properties: `XRGrabInteractable`, `XRInteractionMaskLayer`, `TrackPosition`, `TrackRotation`, `Throw_Detach`, `forcegravity`, `velocity`, `angularvelocity`)
*   **Smoothing**
*   **Smoothing_duration**
*   **attachtransform** (and all sub-properties: `rotate_x`... `pos_z`)
*   **Audio Properties** (prefix `aud_`):
    *   `aud_hasaudio`
    *   `aud_type`
    *   `aud_src`
    *   `aud_volume`
    *   `aud_PlayInloop`
    *   `aud_IsSurround`
    *   `aud_Dopplerlevel`
    *   `aud_spread`
    *   `aud_mindist`
    *   `aud_maxdist`
*   **_Opttxt1**

### B. Editor-Only (No Runtime Implementation)
These fields are displayed and editable in the `VReqDV` editor window but are **ignored** by `ObjectHandler.cs`. Setting these values in `article.json` will have **no effect** on the generated GameObjects in the scene.

*   **_sid**
*   **_slabel**
*   **_IsHidden**
*   **_enumcount**
*   **_Is3DObject**
*   **lighting** (and all sub-properties: `CastShadow`, `ReceiveShadow`, `ContributeGlobalIlumination`)

## 2. behavior.json (BehaviorRule Class)

### A. Partially Unused (Parsed but Ignored)
*   **Postcondition**: The `postcondition` field represents a `ConditionNode`. It is parsed by `BehaviorJsonParser` and processed by `BehaviorCodeGenerator` during inheritance expansion (renaming parameters). However, the content of `postcondition` is **never used** in the generated C# behavior scripts. The scripts only check `Precondition` and execute `Action`.

## 3. actionHandler.cs Analysis

The user requested an analysis of fields used in the obsolete `actionHandler.cs`.

*   **Findings**: `ActionHandler.cs` operates exclusively on `action-response.json` (deserializing to `ActionResponseList`). It does **not** consume `article.json` or `behavior.json`.
*   **Conclusion**: There is no overlap between the unused fields in `article.json`/`behavior.json` and the logic in `actionHandler.cs`. The unused fields listed above (like `Interaction`) appear to be vestiges of an unimplemented feature set within the `Article` schema itself, unrelated to the legacy action handler.
