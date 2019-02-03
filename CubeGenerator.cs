/* 
UVic CSC 305, 2019 Spring
Assignment 01
Name: Matthew Belford  
UVic ID: V00874211
This is skeleton code we provided.
Feel free to add any member variables or functions that you need.
Feel free to modify the pre-defined function header or constructor if you need.
Please fill your name and uvic id.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment01
{
    public class CubeGenerator
    {
        string name;


        public CubeGenerator()
        {
            //you can define your cube vertices and indices in the constructor.
            name = "CubeGenerator";
            
        }

        public Texture2D GenBarycentricVis(int width, int height)
        {
            /*
            implement ray-triangle intersection and 
            visualize the barycentric coordinate on each of the triangles of a cube, 
            with Red, Green and Blue for each coordinate.
            int width - width of the returned texture
            int height - height of the return texture
            return:
                Texture2D - Texture2D object which contains the rendered result
            */
            Vector3[] vertices =
            {
                new Vector3((width / 2) - 400, (height / 2) + 100, 400), // 0 top left
                new Vector3((width / 2) - 400, (height / 2) - 100, 400), // 1 bottom left
                new Vector3((width / 2) - 200, (height / 2) + 100, 200), // 2 top middle
                new Vector3((width / 2) - 200, (height / 2) - 100, 200), // 3 bottom middle
                new Vector3((width / 2), (height / 2) + 100, 400), // 4 top right
                new Vector3((width / 2), (height / 2) - 100, 400)  // 5 bottom right
            };
            int[] faces =
            {
                1, 0, 3, //left bottom
                3, 0, 2, //left top
                3, 2, 4, //right top
                5, 3, 4  //right bottom

            };

            Texture2D RayTracingResult = new Texture2D(width, height);

            Vector3 origin = new Vector3(width / 2 , height / 2, -500);
            Vector3 direction;
            float t;
            Vector3 barycentricCoordinate;

            for(int i = 0; i < 12; i++)
            {
                Debug.Log(vertices[faces[i]]);
                Debug.Log(vertices[faces[++i]]);
                Debug.Log(vertices[faces[++i]]);
            }

            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    direction = new Vector3(x, y, 0) - origin;
                    direction.Normalize();
                    for(int i = 0; i < 12; i++)
                    {
                        if(IntersectTriangle(origin, direction, vertices[faces[i]], vertices[faces[++i]], vertices[faces[++i]], out t, out barycentricCoordinate))
                        {
                            Color bColor = new Color(barycentricCoordinate.x, barycentricCoordinate.y, barycentricCoordinate.z);
                            RayTracingResult.SetPixel(x, y, bColor);
                            i = 12;
                        }
                    }
                }
            }


            return RayTracingResult;

            throw new NotImplementedException();
        }

        public Texture2D GenUVMapping(int width, int height, Texture2D inputTexture)
        {
            /*
            implement UV mapping with the calculated barycentric coordinate in the previous step, 
            and visualize a texture image on each face of the cube.
            (choose any texture you like)
            we have declared textureOnCube as a public variable,
            you can attach texture to it from Unity.
            you can define your cube vertices and indices in this function.
            int width - width of the returned texture
            int height - height of the return texture
            Texture2D inputTexture - the texture you need to sample from
            return:
                Texture2D - Texture2D object which contains the rendered result
            */
            throw new NotImplementedException();
        }

        private bool IntersectTriangle(Vector3 origin,
                                        Vector3 direction,
                                        Vector3 vA,
                                        Vector3 vB,
                                        Vector3 vC,
                                        out float t,
                                        out Vector3 barycentricCoordinate)
        {
            /*
            Vector3 origin - origin point of the ray
            Vector3 direction - the direction of the ray
            vA, vB, vC - 3 vertices of the target triangle
            out float t - distance the ray travelled to hit a point
            out Vector3 barycentricCoordinate - you should know what this is
            return:
                bool - indicating hit or not
            */
            //code written with help from https://www.scratchapixel.com/lessons/3d-basic-rendering/ray-tracing-rendering-a-triangle/ray-triangle-intersection-geometric-solution

            // plane norm
            Vector3 vAvB = vB - vA;
            Vector3 vAvC = vC - vA;
            Vector3 Normal = Vector3.Cross(vAvB, vAvC);
            float area = Vector3.Magnitude(Normal);
            float denom = Vector3.Dot(Normal, Normal);

            float raydotnorm = Vector3.Dot(Normal, direction);
            float d = Vector3.Dot(Normal, vA);

            t = (Vector3.Dot(Normal, origin) + d) / raydotnorm;

            Vector3 ray = origin + t * direction;
            Vector3 temp;

            // temp value to allow return false
            barycentricCoordinate = ray;

            if (t < 0 || Mathf.Abs(raydotnorm) < float.Epsilon) return false;

            // exceed edge vavb?
            Vector3 edge1 = vB - vA;
            Vector3 bary1 = ray - vA;
            temp = Vector3.Cross(edge1, bary1);
            if(Vector3.Dot(Normal, temp) < 0)
            {
                return false;
            }

            //exceed edge vavc
            Vector3 edge2 = vC - vB;
            bary1 = ray - vB;
            temp = Vector3.Cross(edge2, bary1);
            float u = Vector3.Dot(Normal, temp);
            if (u < 0)
            {
                return false;
            }

            u = u / denom;

            //exceed edge vbvc
            Vector3 edge3 = vA - vC;
            bary1 = ray - vC;
            temp = Vector3.Cross(edge3, bary1);
            float v = Vector3.Dot(Normal, temp);
            if(v < 0)
            {
                return false;
            }

            v = v / denom;

            barycentricCoordinate = new Vector3(u, v, 1 - u - v);

            return true;
            throw new NotImplementedException();
        }
    }
}