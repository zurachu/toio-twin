using UnityEngine;
using toio.Simulator;

public class ToioMatCoordService
{
    private static ToioMatCoordService instance;

    private Stage stage;
    private Mat mat;

    public static ToioMatCoordService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ToioMatCoordService();
                instance.stage = Object.FindObjectOfType<Stage>();
                instance.mat = Object.FindObjectOfType<Mat>();
            }

            return instance;
        }
    }

    public Vector3 MatCoord2UnityCoord(Vector2 matCoord)
    {
        return mat.MatCoord2UnityCoord(matCoord.x, matCoord.y) - stage.transform.position;
    }

    public Vector2Int UnityCoord2MatCoord(Vector3 unityCoord)
    {
        return mat.UnityCoord2MatCoord(unityCoord + stage.transform.position);
    }

    public Vector2Int ClampWithinMatRect(Vector2Int matCoord, int margin)
    {
        var min = new Vector2Int(mat.xMin + margin, mat.yMin + margin);
        var max = new Vector2Int(mat.xMax - margin, mat.yMax - margin);
        matCoord.Clamp(min, max);
        return matCoord;
    }
}
