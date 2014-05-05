﻿using System;
using OpenTK;

namespace GraphicsEngine.Sphere
{
    public class Sphere
    {
    public static Vertex[] CalculateVertices2(float radius, float height, byte segments, byte rings)
        {
            var data = new Vertex[segments * rings];
 
            int i = 0;
 
            for (double y = 0; y < rings; y++)
            {
                double phi = (y / (rings - 1)) * Math.PI; //was /2 
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;
 
                    Vector3 v = new Vector3()
                    {
                        X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        Y = (float)(height * Math.Cos(phi)),
                        Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
                    };
                    Vector3 n = Vector3.Normalize(v);
                    Vector2 uv = new Vector2()
                    {
                        X = (float)(x / (segments - 1)),
                        Y = (float)(y / (rings - 1))
                    };
                    // Using data[i++] causes i to be incremented multiple times in Mono 2.2 (bug #479506).
                    data[i] = new Vertex() { Position = v, Normal = n, TexCoord = uv };
                    i++;
                }
 
            }
 
            return data;
        }
 
        public static ushort[] CalculateElements(float radius, float height, byte segments, byte rings)
        {
            var num_vertices = segments * rings;
            var data = new ushort[num_vertices * 6];
 
            ushort i = 0;
 
            for (byte y = 0; y < rings - 1; y++)
            {
                for (byte x = 0; x < segments - 1; x++)
                {
                    data[i++] = (ushort)((y + 0) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x + 1);
 
                    data[i++] = (ushort)((y + 1) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x);
                }
            }
 
            // Verify that we don't access any vertices out of bounds:
            foreach (int index in data)
                if (index >= segments * rings)
                    throw new IndexOutOfRangeException();
 
            return data;
        }
  }
 
  public class Vertex
    { // mimic InterleavedArrayFormat.T2fN3fV3f
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Position;
    }

  //Vertex[] SphereVertices = Sphere.Sphere.CalculateVertices2(0.5f, 0.5f, 100, 100);
  //ushort[] SphereElements = Sphere.Sphere.CalculateElements(0.5f, 0.5f, 100, 100);

  //GL.Begin(BeginMode.Triangles);
  //foreach (var element in SphereElements)
  //{
  //    GL.Color4(1.5f, 0.0f, 1.0f, 0.50f);
  //    var vertex = SphereVertices[element];
  //    GL.TexCoord2(vertex.TexCoord);
  //    GL.Normal3(vertex.Normal);
  //    GL.Vertex3(vertex.Position);
  //}
  //GL.End();
}