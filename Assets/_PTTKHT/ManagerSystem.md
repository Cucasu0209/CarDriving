```mermaid


classDiagram  
  class LevelManager{
      - int MapIndex
      - int LevelIndex


      + void LoadLevel() 
      + void SetupPlayer()
      + void SetupTraps()
      + void SetupObstacles()
  }

  class GameManager{
      + Action OnStartRunning
      
  }

