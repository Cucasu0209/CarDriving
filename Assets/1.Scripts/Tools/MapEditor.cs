
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MapAdjusting))]
public class MapEditor : Editor
{
    void OnSceneGUI()
    {
        MapAdjusting map = (MapAdjusting)target;

        if (map.Level != null)
        {

            Undo.RecordObject(map, "Move Point");

            #region PlayerTrace

            for (int i = 0; i < map.Level.PlayerTrace.GetIntersectionList().Count; i++)
            {
                // Hiển thị điểm trong Scene và cho phép kéo
                Vector3 worldPos = map.transform.TransformPoint(map.Level.PlayerTrace.GetIntersectionList()[i]);
                Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                // Nếu điểm thay đổi vị trí thì cập nhật lại list
                if (newWorldPos != worldPos)
                {
                    map.Level.PlayerTrace.GetIntersectionList()[i] = map.transform.InverseTransformPoint(newWorldPos);
                    EditorUtility.SetDirty(map);
                    EditorUtility.SetDirty(map.Level.PlayerTrace);
                    AssetDatabase.SaveAssets();
                }

                // Tên hiển thị của knot
                Handles.Label(worldPos + Vector3.up * 0.1f, $"Player {i}", EditorStyles.boldLabel);
            }

            // Vẽ đường nối các điểm
            Handles.color = Color.cyan;
            for (int i = 0; i < map.Level.PlayerTrace.GetIntersectionList().Count - 1; i++)
            {
                Vector3 p1 = map.transform.TransformPoint(map.Level.PlayerTrace.GetIntersectionList()[i]);
                Vector3 p2 = map.transform.TransformPoint(map.Level.PlayerTrace.GetIntersectionList()[i + 1]);
                Handles.DrawLine(p1, p2);
            }

            #endregion

            #region Obstacle

            for (int j = 0; j < map.Level.Obstacles.Count; j++)
            {
                for (int i = 0; i < map.Level.Obstacles[j].Trace.GetIntersectionList().Count; i++)
                {
                    // Hiển thị điểm trong Scene và cho phép kéo
                    Vector3 worldPos = map.transform.TransformPoint(map.Level.Obstacles[j].Trace.GetIntersectionList()[i]);
                    Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                    // Nếu điểm thay đổi vị trí thì cập nhật lại list
                    if (newWorldPos != worldPos)
                    {
                        map.Level.Obstacles[j].Trace.GetIntersectionList()[i] = map.transform.InverseTransformPoint(newWorldPos);
                        EditorUtility.SetDirty(map);
                        EditorUtility.SetDirty(map.Level.Obstacles[j].Trace);
                        AssetDatabase.SaveAssets();

                    }

                    // Tên hiển thị của knot
                    if (map.Level.Obstacles[j].Type == ObstacleType.Character)
                        Handles.Label(worldPos + Vector3.up * 0.1f, $"Human_{j+1}_{i}", EditorStyles.boldLabel);
                    else
                        Handles.Label(worldPos + Vector3.up * 0.1f, $"Car_{j + 1}_{i}", EditorStyles.boldLabel);

                }

                // Vẽ đường nối các điểm
                if (map.Level.Obstacles[j].Type == ObstacleType.Character)
                    Handles.color = Color.green;
                else
                    Handles.color = Color.red;

                for (int i = 0; i < map.Level.Obstacles[j].Trace.GetIntersectionList().Count - 1; i++)
                {
                    Vector3 p1 = map.transform.TransformPoint(map.Level.Obstacles[j].Trace.GetIntersectionList()[i]);
                    Vector3 p2 = map.transform.TransformPoint(map.Level.Obstacles[j].Trace.GetIntersectionList()[i + 1]);
                    Handles.DrawLine(p1, p2);
                }
            }

            #endregion

            #region Points
            //-----------------------------------------
            //PICKUP POINT
            Vector3 worldPos1 = map.transform.TransformPoint(map.Level.PickupPoint);
            Vector3 newWorldPos1 = Handles.PositionHandle(worldPos1, Quaternion.identity);

            // Nếu điểm thay đổi vị trí thì cập nhật lại list
            if (newWorldPos1 != worldPos1)
            {
                map.Level.PickupPoint = map.transform.InverseTransformPoint(newWorldPos1);
                EditorUtility.SetDirty(map);
                EditorUtility.SetDirty(map.Level);
                AssetDatabase.SaveAssets();

            }

            // Tên hiển thị của knot
            Handles.Label(worldPos1 + Vector3.up * 0.1f, $"Pickup Point", EditorStyles.boldLabel);
            //------------------------------------------


            //-----------------------------------------
            //START CUSTOMER POINT
            Vector3 worldPos2 = map.transform.TransformPoint(map.Level.CustomerStartPoint);
            Vector3 newWorldPos2 = Handles.PositionHandle(worldPos2, Quaternion.identity);

            // Nếu điểm thay đổi vị trí thì cập nhật lại list
            if (newWorldPos2 != worldPos2)
            {
                map.Level.CustomerStartPoint = map.transform.InverseTransformPoint(newWorldPos2);
                EditorUtility.SetDirty(map);
                EditorUtility.SetDirty(map.Level);
                AssetDatabase.SaveAssets();
            }

            // Tên hiển thị của knot
            Handles.Label(worldPos2 + Vector3.up * 0.1f, $"Start Customer", EditorStyles.boldLabel);
            //------------------------------------------

            //-----------------------------------------
            //END CUSTOMER POINT
            Vector3 worldPos3 = map.transform.TransformPoint(map.Level.FinalCustomerPoint);
            Vector3 newWorldPos3 = Handles.PositionHandle(worldPos3, Quaternion.identity);

            // Nếu điểm thay đổi vị trí thì cập nhật lại list
            if (newWorldPos3 != worldPos3)
            {
                map.Level.FinalCustomerPoint = map.transform.InverseTransformPoint(newWorldPos3);
                EditorUtility.SetDirty(map);
                EditorUtility.SetDirty(map.Level);
                AssetDatabase.SaveAssets();
            }

            // Tên hiển thị của knot
            Handles.Label(worldPos3 + Vector3.up * 0.1f, $"End Customer", EditorStyles.boldLabel);
            //------------------------------------------
            #endregion

            #region Safe Points
            for (int i = 0; i < map.Level.SafePoints.Count; i++)
            {
                // Hiển thị điểm trong Scene và cho phép kéo
                Vector3 worldPos = map.transform.TransformPoint(map.Level.SafePoints[i]);
                Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                // Nếu điểm thay đổi vị trí thì cập nhật lại list
                if (newWorldPos != worldPos)
                {
                    map.Level.SafePoints[i] = map.transform.InverseTransformPoint(newWorldPos);
                    EditorUtility.SetDirty(map);
                    EditorUtility.SetDirty(map.Level);
                    AssetDatabase.SaveAssets();
                }

                // Tên hiển thị của knot
                Handles.Label(worldPos + Vector3.up * 0.1f, $"Safe point {i}", EditorStyles.boldLabel);
            }

            #endregion
        }
    }

}
#endif