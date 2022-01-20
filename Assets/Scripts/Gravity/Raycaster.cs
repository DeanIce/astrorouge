using System;
using UnityEngine;

namespace Gravity
{
    public class Raycaster
    {


        public static Vector3 DoRayCast(GameObject o, int rows, int cols, float spacing)
        {
            Vector3 origin = o.transform.position;
            float width = spacing * cols;
            float height = spacing * rows;
            var corner = origin;
            corner.x -= width / 2;
            corner.z -= height / 2;

            int hits = 0;
            Vector3 sum = Vector3.zero;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var point = new Vector3(row * spacing, 0,col * spacing) + corner;
                    var end = point - o.transform.up;
                    Debug.DrawLine(point,end,Color.magenta);
                    
                    if (Physics.Raycast(o.transform.position, -o.transform.up, out var hit))
                    {
                        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
                        hits++;
                        sum += hit.normal;
                    }
                }
            }

            // if (Physics.Raycast(o.transform.position, -o.transform.up, out var hit))
            // {
            //     surfaceNormal = hit.normal;
            //     Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
            // }

            var result = Vector3.zero;
            if (hits > 0)
            {
                result = sum / hits;
            }
            else
            {
                Console.WriteLine("reeeeee");
            }

            return result;
        }
    }
}