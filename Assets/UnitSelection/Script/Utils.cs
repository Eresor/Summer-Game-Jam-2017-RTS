using UnityEngine;

public static class Utils{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture{
        get{
            if(_whiteTexture == null){
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

	static Bounds bounds = new Bounds();

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2){
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;
        //min.z = 0.0f;
        //max.z = 1.0f;

        bounds.SetMinMax(min, max);

        return bounds;
    }

}