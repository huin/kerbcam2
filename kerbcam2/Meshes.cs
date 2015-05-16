using System;
using System.Collections.Generic;
using UnityEngine;

namespace kerbcam2 {
    class MeshBuilder {
        // Common vertex positions given by AddVertex. These are indexed by
        // other fields.
        private readonly List<Vector3> commonVertices = new List<Vector3>();
        // Each sub-list is for the corresponding entry in commonVertices, and
        // contains the indices of its copies in vertNormIndices.
        private readonly List<List<int>> copiedVertices = new List<List<int>>();
        // Each int is an index into commonVertices.
        private readonly List<int> vertNormIndices = new List<int>();
        // Each int[] contains the indices of the vertices/normals for a single
        // flat face within vertNormIndices (each face having potentially
        // multiple triangles).
        private readonly List<int[]> faces = new List<int[]>();
        // Each int is an index into vertNormIndices.
        private readonly List<int> triangles = new List<int>();

        public int AddVertex(float x, float y, float z) {
            commonVertices.Add(new Vector3(x, y, z));
            copiedVertices.Add(new List<int>());
            return commonVertices.Count - 1;
        }

        private int VertexCopy(int commonIndex) {
            vertNormIndices.Add(commonIndex);
            int copyId = vertNormIndices.Count - 1;
            copiedVertices[commonIndex].Add(copyId);
            return copyId;
        }

        public void AddFlatFace(params int[] vertices) {
            if (vertices.Length < 3) {
                throw new ArgumentException("Must have a length of 3 or more", "vertices");
            }
            // Create the vertex copies for the face.
            var face = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) {
                face[i] = VertexCopy(vertices[i]);
            }
            faces.Add(face);
            for (int v2 = 1; v2 < face.Length - 2; v2++) {
                triangles.Add(face[0]);
                triangles.Add(face[v2]);
                triangles.Add(face[v2 + 1]);
            }
        }

        public BuiltMesh Build() {
            var mesh = new Mesh();
            var copiedVerticesArr = new int[copiedVertices.Count][];
            for (int i = 0; i < copiedVertices.Count; i++) {
                copiedVerticesArr[i] = copiedVertices[i].ToArray();
            }
            var facesArr = new int[faces.Count][];
            for (int i = 0; i < faces.Count; i++) {
                facesArr[i] = (int[])faces[i].Clone();
            }
            mesh.vertices = new Vector3[vertNormIndices.Count];
            mesh.normals = new Vector3[vertNormIndices.Count];
            mesh.triangles = triangles.ToArray();
            var bm = new BuiltMesh(mesh, copiedVerticesArr, facesArr);
            using (var mm = bm.MutateMesh()) {
                for (int id = 0; id < commonVertices.Count; id++) {
                    mm.SetVertex(id, commonVertices[id]);
                }
            }
            return bm;
        }
    }

    class BuiltMesh {
        private readonly Mesh mesh;
        private readonly int[][] copiedVertices;
        private readonly int[][] faces;

        public BuiltMesh(Mesh mesh, int[][] copiedVertices, int[][] faces) {
            this.mesh = mesh;
            this.copiedVertices = copiedVertices;
            this.faces = faces;
        }

        public Mesh Mesh {
            get { return mesh; }
        }

        public MeshMutator MutateMesh() {
            return new MeshMutator(this);
        }

        public class MeshMutator : IDisposable {
            private BuiltMesh bm;
            private Vector3[] vertices;

            public MeshMutator(BuiltMesh bm) {
                this.bm = bm;
                this.vertices = bm.mesh.vertices;
            }

            public void SetVertex(int commonId, Vector3 v) {
                foreach (int i in bm.copiedVertices[commonId]) {
                    this.vertices[i] = v;
                }
            }

            public void SetVertex(int commonId, float x, float y, float z) {
                SetVertex(commonId, new Vector3(x, y, z));
            }

            void IDisposable.Dispose() {
                Vector3[] normals = bm.mesh.normals;
                // Recalculate normals.
                foreach (int[] face in bm.faces) {
                    Vector3 v1 = vertices[face[0]];
                    Vector3 v2 = vertices[face[1]];
                    Vector3 v3 = vertices[face[2]];
                    Vector3 normal = Vector3.Cross(v3 - v1, v1 - v2).normalized;
                    foreach (int i in face) {
                        normals[i] = normal;
                    }
                }
                bm.mesh.vertices = vertices;
                bm.mesh.normals = normals;
                bm.mesh.RecalculateBounds();
            }
        }
    }

    class SquareFrustrum {
        private readonly BuiltMesh builtMesh;
        // Backface dimensions.
        private Vector2 bfsize;
        private Vector2 ffsize;
        private float depth;

        public SquareFrustrum() {
            // TODO: Use better sizes.
            bfsize = new Vector2(1.33f, 1f);
            ffsize = bfsize * 2f;
            depth = 1f;

            var builder = new MeshBuilder();

            // Front face vertices:
            var FFTL = builder.AddVertex(-ffsize.x / 2, ffsize.y / 2, depth);
            var FFTR = builder.AddVertex(ffsize.x / 2, ffsize.y / 2, depth);
            var FFBL = builder.AddVertex(-ffsize.x / 2, -ffsize.y / 2, depth);
            var FFBR = builder.AddVertex(ffsize.x / 2, -ffsize.y / 2, depth);
            // Back face vertices:
            var BFTL = builder.AddVertex(-bfsize.x / 2, bfsize.y / 2, 0);
            var BFTR = builder.AddVertex(bfsize.x / 2, bfsize.y / 2, 0);
            var BFBL = builder.AddVertex(-bfsize.x / 2, -bfsize.y / 2, 0);
            var BFBR = builder.AddVertex(bfsize.x / 2, -bfsize.y / 2, 0);

            builder.AddFlatFace(FFTL, FFTR, BFTR, BFTL); // Top face.
            builder.AddFlatFace(FFBL, BFBL, BFBR, FFBR); // Bottom face.
            builder.AddFlatFace(FFTL, BFTL, BFBL, FFBR); // Left face.
            builder.AddFlatFace(FFTR, FFBR, BFBR, BFTR); // Right face.

            builtMesh = builder.Build();
        }

        public Mesh Mesh {
            get { return builtMesh.Mesh; }
        }
    }
}
