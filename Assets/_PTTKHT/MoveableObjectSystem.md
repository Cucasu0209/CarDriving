```mermaid

classDiagram
    MoveableObject<|-- Player
    MoveableObject<|-- CarBot
    MoveableObject<|-- CharacterBot



    class MoveableObject{
        + TraceData Trace
    }

    class Player{
        + void Run()
        + void Stop()
    }

    class CarBot{

    }

    class CharacterBot{

    }
