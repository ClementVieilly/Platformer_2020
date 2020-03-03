///-----------------------------------------------------------------
/// Author : 
/// Date : 29/02/2020 16:49
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Curve {
	public static class CurveUtils {
	
        /// <summary>
        /// Quadratic Curve
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="ratio">will be clamp 0/1</param>
        /// <returns></returns>
		public static Vector3 GetPoints (Vector3 point0, Vector3 point1, Vector3 point2, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float oneMinusRatio = 1f - ratio;
            return
                oneMinusRatio * oneMinusRatio * point0 +
                2f * oneMinusRatio * ratio * point1 +
                ratio * ratio * point2;
            //return Vector3.Lerp(Vector3.Lerp(point0, point1, ratio), Vector3.Lerp(point1, point2, ratio), ratio);
        }
        public static Vector3 GetFirstDerivate(Vector3 point0, Vector3 point1, Vector3 point2, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float oneMinusRatio = 1f - ratio;
            return
                2f * oneMinusRatio * (point1 - point0) +
                2f * ratio * (point2 - point1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 GetPoints(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float oneMinusRatio = 1f - ratio;
            return
                oneMinusRatio * oneMinusRatio * oneMinusRatio * point0 +
                3f * oneMinusRatio * oneMinusRatio * ratio * point1 +
                3f * oneMinusRatio * ratio * ratio * point2 +
                ratio * ratio * ratio * point3;
        }
        public static Vector3 GetFirstDerivate(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float oneMinusRatio = 1f - ratio;
            return
                3f * oneMinusRatio * oneMinusRatio * (point1 - point0) +
                6f * oneMinusRatio * ratio * (point2 - point1) +
                3f * ratio * ratio * (point3 - point2);
        }
    }
}