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
                new Vector3((width / 2) - 200, (height / 2) + 105, 200), // 0 top left
                new Vector3((width / 2) - 200, (height / 2) - 105, 200), // 1 bottom left
                new Vector3((width / 2), (height / 2) + 100, 5), // 2 top middle
                new Vector3((width / 2), (height / 2) - 100, 5), // 3 bottom middle
                new Vector3((width / 2) + 200, (height / 2) + 105, 200), // 4 top right
                new Vector3((width / 2) + 200, (height / 2) - 105, 200),  // 5 bottom right
            };
            int[] faces =
            {
                1, 0, 3, //left bottom
                3, 0, 2, //left top
                3, 2, 4, //right top
                3, 4, 5,  //right bottom

            };

            Texture2D RayTracingResult = new Texture2D(width, height);

            Vector3 origin = new Vector3(width / 2 , height / 2, -400);
            Vector3 direction;
            float t;
            Vector3 barycentricCoordinate;

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
            Vector3[] vertices =
            {
                new Vector3((width / 2) - 200, (height / 2) + 100, 200), // 0 top left
                new Vector3((width / 2) - 200, (height / 2) - 100, 200), // 1 bottom left
                new Vector3((width / 2), (height / 2) + 100, 100), // 2 top middle
                new Vector3((width / 2), (height / 2) - 100, 100), // 3 bottom middle
                new Vector3((width / 2) + 200, (height / 2) + 100, 200), // 4 top right
                new Vector3((width / 2) + 200, (height / 2) - 100, 200),  // 5 bottom right
            };
            int[] faces =
            {
                1, 0, 3, //left bottom
                3, 0, 2, //left top
                3, 2, 4, //right top
                3, 4, 5,  //right bottom

            };
            Texture2D RayTracingResult = new Texture2D(width, height);

            Vector3 origin = new Vector3(width / 2, height / 2, -400);
            Vector3 direction;
            float t;
            int once = 1;
            Vector3 barycentricCoordinate;

            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    direction = new Vector3(x, y, 0) - origin;
                    direction.Normalize();
                    for (int i = 0; i < 12; i++)
                    {
                        Vector3 a = vertices[faces[i]];
                        Vector3 b = vertices[faces[++i]];
                        Vector3 c = vertices[faces[++i]];
                        if (IntersectTriangle(origin, direction, a, b, c, out t, out barycentricCoordinate))
                        {

                            if(i < 4) //bottom left triangle
                            {
                                a.x = 0; a.y = 0;
                                b.x = 0; b.y = 1;
                                c.x = 1; c.y = 0;
                            }
                            else if(i < 7) //top left triangle
                            {
                                a.x = 1; a.y = 0;
                                b.x = 0; b.y = 1;
                                c.x = 1; c.y = 1;
                            }
                            else if(i < 10) //top right triangle
                            {
                                a.x = 1; a.y = 0;
                                b.x = 1; b.y = 1;
                                c.x = 0; c.y = 1;
                            }
                            else //bottom right triangle
                            {
                                a.x = 1; a.y = 0;
                                b.x = 0; b.y = 1;
                                c.x = 0; c.y = 0;
                            }

                            float u = barycentricCoordinate.x * a.x + barycentricCoordinate.y * b.x + barycentricCoordinate.z * c.x;
                            float v = barycentricCoordinate.x * a.y + barycentricCoordinate.y * b.y + barycentricCoordinate.z * c.y;
                            u *= inputTexture.width;
                            v *= inputTexture.height;

                            //debug prints
                            if (once == 1)
                            {
                                Debug.Log(inputTexture.width);
                                Debug.Log(barycentricCoordinate);
                                Debug.Log(u);
                                Debug.Log(v);
                                once = 0;
                            }

                            RayTracingResult.SetPixel(x, y, inputTexture.GetPixel((int)u, (int)v));
                            i = 12;
                        }
                    }
                }
            }
            return RayTracingResult;

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
            // code written with help from https://www.scratchapixel.com/lessons/3d-basic-rendering/ray-tracing-rendering-a-triangle/ray-triangle-intersection-geometric-solution

            // plane norm
            Vector3 Normal = Vector3.Cross(vA - vB, vA - vC);
            float denom = Vector3.Dot(Normal, Normal);

            float raydotnorm = Vector3.Dot(Normal, direction);
            float d = Vector3.Dot(Normal, vA);

            t = (d - Vector3.Dot(Normal, origin)) / raydotnorm;

            Vector3 ray = origin + t * direction;
            Vector3 temp;

            // temp value to allow return false
            barycentricCoordinate = new Vector3(0,0,0);

            if (t < 0 || Mathf.Abs(raydotnorm) < float.Epsilon) return false;

            // exceed edge vavb?
            Vector3 edge1 = vB - vA;
            Vector3 bary1 = ray - vA;
            temp = Vector3.Cross(edge1, bary1);
            if(Vector3.Dot(Normal, temp) < 0)
            {
                return false;
            }

            //exceed edge vbvc
            Vector3 edge2 = vC - vB;
            bary1 = ray - vB;
            temp = Vector3.Cross(edge2, bary1);
            float u = Vector3.Dot(Normal, temp);
            if (u < 0)
            {
                return false;
            }

            u = u / denom;

            //exceed edge vavc
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