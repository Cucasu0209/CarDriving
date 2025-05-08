```mermaid

classDiagram
    MapData<--TraceData
    TraceData<--ObstacleData
    TraceData<--LevelData
    ObstacleData<--LevelData

    class MapData{
        + int MapIndex;
        + string MapName;
        + string MapDescription;

        + List~Vector3~ Intersections;
        + List~List~Vector3~~ Connections;
    }
    
    class TraceData{
        + MapData Map;
        + List~int~ TracePoints;
        + LoopType Type; 
    }
    
    class ObstacleData{
        + int MapIndex;
        + TraceData Trace;
        + ObstacleType Type;
    }

    class LevelData{
        + int MapIndex;
        + TraceData PlayerTrace;
        + Vector3 PickupPoint;
        + List~List~Vector3~~ Traps;
        + List~ObstacleData~ Obstacle;
    }

