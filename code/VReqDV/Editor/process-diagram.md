``` mermaid 
flowchart TB

    %% ======================
    %% Specification Layer
    %% ======================
    subgraph S["Specification Layer (Model)"]
        A1["article.json<br/>(objects, states, assets)"]
        A2["behaviors.json<br/>(coordination logic)"]
        A3["algorithms.json<br/>(algorithm contracts)"]
    end

    %% ======================
    %% Parsing Layer
    %% ======================
    subgraph P["Parsing & Validation Layer"]
        B1["Article Parser"]
        B2["Behavior Parser"]
        B3["Algorithm Parser"]
    end

    %% ======================
    %% Intermediate Representation
    %% ======================
    subgraph IR["Intermediate Representation (IR)"]
        C1["Article IR<br/>(objects, states)"]
        C2["Behavior IR<br/>(rules, conditions, triggers)"]
        C3["Algorithm IR<br/>(signatures only)"]
    end

    %% ======================
    %% Code Generation
    %% ======================
    subgraph G["Code Generation Layer"]
        D1["Template Family 4<br/>Structural Infrastructure"]
        D2["Template Family 1<br/>Polling (OnCondition)"]
        D3["Template Family 2<br/>Subscription (OnStateChange)"]
        D4["Template Family 3<br/>Timed (optional)"]
    end

    %% ======================
    %% Generated Code
    %% ======================
    subgraph C["Generated Unity Code"]
        E1["State Enums & Storage"]
        E2["Registries & Initializers"]
        E3["Coordination Scripts"]
    end

    %% ======================
    %% Runtime
    %% ======================
    subgraph R["Unity Runtime Execution"]
        F1["Generated Coordination Logic"]
        F2["User Algorithm Implementations"]
        F3["State Change Events"]
    end

    %% ======================
    %% Flow connections
    %% ======================
    A1 --> B1
    A2 --> B2
    A3 --> B3

    B1 --> C1
    B2 --> C2
    B3 --> C3

    C1 --> D1
    C2 --> D2
    C2 --> D3
    C2 --> D4
    C3 --> D2
    C3 --> D3

    D1 --> E1
    D1 --> E2
    D2 --> E3
    D3 --> E3
    D4 --> E3

    E1 --> F3
    E2 --> F1
    E3 --> F1
    F1 --> F2
    F2 --> F3
