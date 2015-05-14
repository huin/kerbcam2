using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace kerbcam2 {
    /// <summary>
    /// Raw manual editor for Vector3d.
    /// </summary>
    class Vector3dField {
        readonly CheckedField<double> x, y, z;

        public Vector3dField(Vector3d initialValue) {
            x = new CheckedField<double>(initialValue.x, ValueParser.doubleParser);
            y = new CheckedField<double>(initialValue.y, ValueParser.doubleParser);
            z = new CheckedField<double>(initialValue.z, ValueParser.doubleParser);
        }

        public void DrawUI(ref Vector3d curValue) {
            var w = GUILayout.MaxWidth(60);
            using (GU.Horizontal()) {
                x.DrawUI(ref curValue.x, w);
                y.DrawUI(ref curValue.y, w);
                z.DrawUI(ref curValue.z, w);
                GUILayout.FlexibleSpace();
            }
        }
    }
}
