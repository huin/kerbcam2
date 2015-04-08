using System;
using UnityEngine;

namespace kerbcam2 {
    class CheckedField<T> where T : IEquatable<T> {
        public delegate bool TryParser(string s, out T v);

        private T lastValue;
        private string strValue;
        private TryParser parser;

        public CheckedField(T initialValue, TryParser parser) {
            this.lastValue = initialValue;
            this.strValue = initialValue.ToString();
            this.parser = parser;
        }

        public T DrawUI(T curValue) {
            if (!curValue.Equals(lastValue)) {
                lastValue = curValue;
                strValue = curValue.ToString();
            }

            string label;
            GUIStyle labelStyle;
            GUIStyle fieldStyle;
            T unused;
            if (parser(strValue, out unused)) {
                label = " ";
                labelStyle = GUI.skin.label;
                fieldStyle = GUI.skin.textField;
            } else {
                label = "*";
                labelStyle = Styles.invalidValueLabel;
                fieldStyle = Styles.invalidTextField;
            }

            string newStrValue;
            using (GU.Horizontal()) {
                GUILayout.Label(label, labelStyle);
                newStrValue = GUILayout.TextField(strValue, fieldStyle);
            }

            if (newStrValue != strValue) {
                strValue = newStrValue;
                T newValue;
                if (parser(newStrValue, out newValue)) {
                    lastValue = newValue;
                    return newValue;
                }
            }

            return curValue;
        }
    }
}
