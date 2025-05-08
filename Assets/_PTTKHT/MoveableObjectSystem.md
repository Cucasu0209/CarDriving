```mermaid

classDiagram
    MoveableObject<|-- Player
    MoveableObject<|-- CarBot
    MoveableObject<|-- CharacterBot



    class MoveableObject{
        + TraceData Trace;

        + virtual void Run()
        + virtual void Stop()
        + virtual void OnHit()

    }

    class Player{
        - Vector3 TargetVelocity
        - float TargetAngle

        + void DrawLine()
        + override void Run() //user Input 
        + override void Stop() //user Input
        + override void OnHit() //Game over
    }

    class CarBot{

    }

    class CharacterBot{

    }
