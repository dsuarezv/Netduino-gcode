using System;
using System.IO;
using System.Collections;
using System.Globalization;

using Tao.OpenGl;

namespace Druid.Viewer
{
	public class Object3D
	{
        private ArrayList mMeshes = new ArrayList();

        public IList Meshes
        {
            get{ return mMeshes; }
        }

		public Object3D()
		{

		}

        private const string NO_MATERIAL = "NOMATERIAL";
        private const string SINGLE_MATERIAL = "SINGLEMAT";
        private const string MULTI_MATERIAL = "MULTIMAT";
        private const string NO_TEXTURE	= "NOTEXTURE";


        public void LoadFromStream(Stream r)
        {
            TextReader reader = new StreamReader(r);

            try
            {
                NumberFormatInfo usNumberFormat = new NumberFormatInfo();
                usNumberFormat.NumberDecimalSeparator = ".";

                // jump date line
                reader.ReadLine();

                // read number of meshes                        
                int MeshCount = Convert.ToInt32(reader.ReadLine());

                // loop through all meshes
                for (int i = 0; i < MeshCount; ++i)
                {
                    Mesh mesh = new Mesh();

                    // read name, vertex no, text no and triangle no
                    string line;

                    line = reader.ReadLine();

                    string[] param = line.Split(' ');

                    mesh.Name = param[0];
                    mesh.NumVertexes = long.Parse(param[1]);
                    mesh.NumTexCoords = long.Parse(param[2]);
                    mesh.NumTriangles = long.Parse(param[3]);                

                    mesh.Vertexes = new float[3 * mesh.NumVertexes];
                    mesh.Normals = new float[3 * mesh.NumVertexes];
                    mesh.Indexes = new short[3* mesh.NumTriangles];
                
                    if( mesh.NumTexCoords > 0 )
                        mesh.TexCoords = new float[2 * mesh.NumTexCoords];

                    int j;

                    // read all vertex
                    for (j = 0; j < mesh.NumVertexes; ++j)
                    {
                        line = reader.ReadLine();
                        param = line.Split(' ');                    
                        
                        float v1 = float.Parse(param[1], usNumberFormat);
                        mesh.Vertexes[j*3] = v1;
                        mesh.Vertexes[j*3+1] = float.Parse(param[2], usNumberFormat);
                        mesh.Vertexes[j*3+2] = float.Parse(param[3], usNumberFormat);
                    }

                    // read all normals
                    for (j = 0; j < mesh.NumVertexes; ++j)
                    {
                        line = reader.ReadLine();
                        param = line.Split(' ');                    

                        mesh.Normals[j*3] = float.Parse(param[1], usNumberFormat);
                        mesh.Normals[j*3+1] = float.Parse(param[2], usNumberFormat);
                        mesh.Normals[j*3+2] = float.Parse(param[3], usNumberFormat);
                    }

                    // read texture coords
                    if (mesh.NumTexCoords > 0)
                    {
                        for (j = 0; j < mesh.NumTexCoords; ++j)
                        {
                            // fscanf(f,"%d %f %f %f",&num,&x,&y,&z);
                            line = reader.ReadLine();
                            param = line.Split(' ');                    

                            mesh.TexCoords[j*2] = float.Parse(param[1], usNumberFormat);
                            mesh.TexCoords[j*2+1] = float.Parse(param[2], usNumberFormat);
                        }
                    }

                    // read triangles
                    for( j = 0; j < mesh.NumTriangles; ++j)
                    {
                        // fscanf(f,"%d %d %d %d %s\n",&num,&v0,&v1,&v2,szBuffer);
                        line = reader.ReadLine();
                        param = line.Split(' ');                    

                        mesh.Indexes[j*3] = short.Parse(param[1]);
                        mesh.Indexes[j*3+1] = short.Parse(param[2]);
                        mesh.Indexes[j*3+2] = short.Parse(param[3]);
                    }

                    // read materials, can be single or multiple material
                    line = reader.ReadLine();

                    if (line != NO_MATERIAL)
                    {
                        // then we have the name of a material
                        line = reader.ReadLine();
                        if (line == SINGLE_MATERIAL)
                        {
                            // single material, read color
                            // fscanf(f,"%f %f %f",&x,&y,&z);
                            line = reader.ReadLine();
                            param = line.Split(' ');

                            mesh.Color.R = float.Parse(param[0], usNumberFormat);
                            mesh.Color.G = float.Parse(param[1], usNumberFormat);
                            mesh.Color.B = float.Parse(param[2], usNumberFormat);

                            // look for texture
                            line = reader.ReadLine();
                        
                            if (line != NO_TEXTURE) mesh.TextureName = line;
                        }
                        else
                        {
                            // handle multimaterial
                        }
                    }

                    // Add Mesh to object
                    mMeshes.Add(mesh);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        public bool LoadFromFile(string fileName)
        {
            if( ! File.Exists(fileName) )
                return false;

            Stream st = new FileStream(fileName, FileMode.Open);

            LoadFromStream(st);
            
            return true;
        }

        public bool ContainsPoint(Vect3f pos, Vect3f point)
        {
            foreach( Mesh m in mMeshes )
            {
                if( m.GetBoundingBox().ContainsPoint(pos, point) )
                    return true;
            }
            return false;
        }

        public BoundingBox GetBoundingBox(Vect3f pos)
        {
            BoundingBox b = ((Mesh) mMeshes[0]).GetBoundingBox();
            BoundingBox result = new BoundingBox(b.MinPoint + pos, b.MaxPoint + pos);

            return result;
            
        }

        private enum Quadrants  { Right, Left, Middle };

        private void FindCandidatePlanes(
            float origin, float minB, float maxB, 
            ref bool bInside, out Quadrants quadrant, ref float candidatePlane)
        {
            if( origin < minB ) 
            {
                quadrant = Quadrants.Left;
                candidatePlane = minB;
                bInside = false;
            }
            else if (origin > maxB) 
            {
                quadrant = Quadrants.Right;
                candidatePlane = maxB;
                bInside = false;
            }
            else	
            {
                quadrant = Quadrants.Middle;
            }

        }

        private void CalculateDistance(Quadrants quadrant, float dir, float candidatePlane, float origin, ref float maxT)
        {
            
            if( (quadrant != Quadrants.Middle) && (dir !=0.0f) )
                maxT = (candidatePlane - origin ) / dir;
            else
                maxT = -1.0f;

        }

        private bool CheckPlane(int i, int whichPlane, 
            float[] maxT, float origin, float dir, 
            float minB, float maxB, float[] candidatePlane,
            ref float coord)
        {
            if( whichPlane != i ) 
            {
                coord = origin + maxT[whichPlane] * dir;
                if( (coord < minB) || (coord > maxB) )
                    return false;
            } 
            else 
            {
                coord = candidatePlane[i];
            }
            return true;
        }

        public Vect3f IntersectsWithRay(Vect3f origin, Vect3f dir, Vect3f pos)
        {
            Vect3f minB, maxB;
            Vect3f coord;				/* hit point */

        	bool bInside = true;
	        Quadrants quadrantX, quadrantY, quadrantZ;
	        int i;
	        int whichPlane;
	        float[] maxT = new float[3];
	        float[] candidatePlane = new float[3];


            BoundingBox box = GetBoundingBox(pos);
            minB = box.MinPoint;
            maxB = box.MaxPoint;

	        /* Find candidate planes; this loop can be avoided if
   	        rays cast all from the eye(assume perpsective view) */
            FindCandidatePlanes(origin.X, minB.X, maxB.X, ref bInside, out quadrantX, ref candidatePlane[0]);
            FindCandidatePlanes(origin.Y, minB.Y, maxB.Y, ref bInside, out quadrantY, ref candidatePlane[1]);
            FindCandidatePlanes(origin.Z, minB.Z, maxB.Z, ref bInside, out quadrantZ, ref candidatePlane[2]);

	        /* Ray origin inside bounding box */
	        if( bInside)	
            {
		        coord = origin;
		        return coord;
	        }

	        /* Calculate T distances to candidate planes */            
            CalculateDistance(quadrantX, dir.X, candidatePlane[0], origin.X, ref maxT[0]);
            CalculateDistance(quadrantY, dir.Y, candidatePlane[1], origin.Y, ref maxT[1]);
            CalculateDistance(quadrantZ, dir.Z, candidatePlane[2], origin.Z, ref maxT[2]);

	        /* Get largest of the maxT's for final choice of intersection */
	        whichPlane = 0;
	        for( i = 1; i < 3; i++ )
		        if (maxT[whichPlane] < maxT[i])
			        whichPlane = i;

	        /* Check final candidate actually inside box */
	        if( maxT[whichPlane] < 0.0 ) 
                return null;


            coord = new Vect3f(0,0,0);
            if( !CheckPlane(0, whichPlane, maxT, origin.X, dir.X, minB.X, maxB.X, candidatePlane, ref coord.X) )
                return null;

            if( !CheckPlane(1, whichPlane, maxT, origin.Y, dir.Y, minB.Y, maxB.Y, candidatePlane, ref coord.Y) )
                return null;

            if( !CheckPlane(2, whichPlane, maxT, origin.Z, dir.Z, minB.Z, maxB.Z, candidatePlane, ref coord.Z) )
                return null;

            return coord; 
        }

        public void Draw(float alpha, float[] color)
        {
            Draw(this, alpha, color);
        }

        public static void Draw(Object3D obj, float alpha, float[] color)
        {
            if (obj == null)
                return;
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            //Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            Gl.glPushMatrix();
            //Gl.glScalef(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < obj.Meshes.Count; ++i)
            {
                Mesh mesh = (Mesh)obj.Meshes[i];

                if (color == null)
                    Gl.glColor4f(
                        mesh.Color.R / 255.0f,
                        mesh.Color.G / 255.0f,
                        mesh.Color.B / 255.0f,
                        alpha);
                else
                    Gl.glColor4f(
                        color[0],
                        color[1],
                        color[2],
                        alpha);


                float[] mat_difusa = new float[4];
                mat_difusa[0] = ((float)mesh.Color.R) / 255.0f;
                mat_difusa[1] = ((float)mesh.Color.G) / 255.0f;
                mat_difusa[2] = ((float)mesh.Color.B) / 255.0f;
                mat_difusa[3] = 1.0f;

                Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_difusa);
                Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, mesh.Vertexes);
                Gl.glNormalPointer(Gl.GL_FLOAT, 0, mesh.Normals);

                /*    if( mesh.mpTexCoords != null )
                        Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, mesh.mpTexCoords);*/

                Gl.glDrawElements(
                    Gl.GL_TRIANGLES,
                    (int)mesh.NumTriangles * 3,
                    Gl.GL_UNSIGNED_SHORT,
                    mesh.Indexes);
            }
            Gl.glPopMatrix();
        }
	}
    
    public struct ColorRgb
    {
        public float R;
        public float G;
        public float B;

        public ColorRgb(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    public class BoundingBox
    {
        public Vect3f MinPoint;
        public Vect3f MaxPoint;

        public BoundingBox(Vect3f min, Vect3f max)
        {
            MinPoint = min;
            MaxPoint = max;
        }

        public bool ContainsPoint(Vect3f pos, Vect3f point)
        {
            Vect3f min = MinPoint + pos;
            Vect3f max = MaxPoint + pos;

            return 
                (point.X <= max.X) && (point.X >= min.X) &&
                (point.Y <= max.Y) && (point.Y >= min.Y) &&
                (point.Z <= max.Z) && (point.Z >= min.Z);
        }

    }

    public class Mesh
    {
        public float[] Vertexes;
        public float[] Normals;
        public short[] Indexes;
        public float[] TexCoords;
        public long  NumVertexes;
        public long  NumTriangles;
        public long  NumTexCoords;
        public string Name;
        public string TextureName;
        public ColorRgb Color;

        private BoundingBox mBoundingBox = null;

        public BoundingBox GetBoundingBox()
        {
            if (mBoundingBox != null) return mBoundingBox;

            Vect3f min = new Vect3f(float.MaxValue, float.MaxValue, float.MaxValue);
            Vect3f max = new Vect3f(float.MinValue, float.MinValue, float.MinValue);


            Vect3f p = new Vect3f(0,0,0);
            for (int i = 0; i < Vertexes.Length - 3; i += 3)
            {
                p.X = Vertexes[i];
                p.Y = Vertexes[i+1];
                p.Z = Vertexes[i+2];

                if( p.X < min.X )
                    min.X = p.X;
                if( p.Y < min.Y )
                    min.Y = p.Y;
                if( p.Z < min.Z )
                    min.Z = p.Z;
    
                if( p.X > max.X )
                    max.X = p.X;
                if( p.Y > max.Y )
                    max.Y = p.Y;
                if( p.Z > max.Z )
                    max.Z = p.Z;
            }

            mBoundingBox = new BoundingBox(min, max);
            
            return mBoundingBox;
        }
    }
}
