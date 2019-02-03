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
    public class SphereGenerator
    {
        string name;
        float radius;
        int[] center;

        public SphereGenerator()
        {
            //you can define the sphere center and radius in the constructor.
            this.name = "SphereGenerator";
            this.radius = 300f;
        }

        public Texture2D GenSphere(int width, int height)
        {
            /*
            implement ray-sphere intersection and render a sphere with ambient, diffuse and specular lighting.
            int width - width of the returned texture
            int height - height of the return texture
            return:
                Texture2D - Texture2D object which contains the rendered result
            */

            Texture2D RayTracingResult = new Texture2D(width, height);

            Vector3 sphere_center = new Vector3 (width / 2, height / 2, 500);
            Vector3 origin = new Vector3( width / 2 , height / 2, -500);
            Vector3 light_src = new Vector3(width-300, height+300, 475);
            Vector3 direction;
            Vector3 intersectNormal;
            float t;
            
            //lighting factors
            float ambient = 0.05f;
            float diffuse = 0.6f;
            float specular = 0.99f;
            float shininess = 16f;
            


            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    direction = new Vector3(x, y, 0) - origin;
                    direction.Normalize();
                    if(IntersectSphere(origin, direction, sphere_center, this.radius, out t, out intersectNormal))
                    {
                        Color sColor = new Color(0f, 0f, 0.25f);
                        Vector3 light_dir = intersectNormal - light_src;
                        light_dir.Normalize();

                        // cos(theta) = (u dot v) / (||u|| * ||v||)
                        float lightdotray = Vector3.Dot(intersectNormal, light_dir);
                        float raylength = Vector3.Magnitude(intersectNormal);
                        float lightlength = Vector3.Magnitude(light_dir);
                        float cosTheta = lightdotray / (raylength * lightlength);
                        // diffuse light = cosTheta * diffuse factor;
                        float d_light = cosTheta * diffuse;

                        // phong specular
                        Vector3 view_dir = origin;
                        view_dir.Normalize();
                        Vector3 reflect_dir = Vector3.Reflect(-light_dir, intersectNormal);
                        reflect_dir.Normalize();
                        float spec_angle = Math.Max(Vector3.Dot(reflect_dir, view_dir), 0f);
                        float s_light = (float)Math.Pow(spec_angle, shininess / 4f) * specular;

                        sColor.r += ambient + d_light + s_light;
                        sColor.b += ambient + d_light + s_light;
                        sColor.g += ambient + d_light + s_light;
                        RayTracingResult.SetPixel(x, y, sColor);
                    }
                }
            }
            return RayTracingResult;
            throw new NotImplementedException();
        }
        private bool IntersectSphere(Vector3 origin,
                                        Vector3 direction,
                                        Vector3 sphereCenter,
                                        float sphereRadius,
                                        out float t,
                                        out Vector3 intersectNormal)
        {
            /*
            Vector3 origin - origin point of the ray
            Vector3 direction - the direction of the ray
            Vector3 sphereCenter - center of target sphere
            float sphereRadius - radius of target sphere
            out float t - distance the ray travelled to hit a point
            out Vector3 intersectNormal - normal of the hit point
            return:
                bool - indicating hit or not
            */

            //tca  = L dot D
            float L = Vector3.Magnitude(origin - sphereCenter);
            float tca = Vector3.Dot(origin - sphereCenter, direction);
            float d = (float)Math.Sqrt(((L * L) - (tca * tca)));
            float thc = (float)Math.Sqrt(((radius * radius) - (d * d)));
            t = tca - thc;

            intersectNormal = (origin + t * direction);
            intersectNormal.Normalize();

            if(d > radius || d < 0)
            {
                return false;
            }

            return true;

            throw new NotImplementedException();
        }
    }
}