using UnityEngine;

namespace IekoLibrary
{
    public struct Color255
    {
        public int r;
        public int g;
        public int b;
        public int a;

        public Color255(int r, int g, int b, int a)
        {
            this.r = Mathf.Clamp(r, 0, 255);
            this.g = Mathf.Clamp(g, 0, 255);
            this.b = Mathf.Clamp(b, 0, 255);
            this.a = Mathf.Clamp(a, 0, 255);
        }

        public readonly Color ToColor()
        {
            return new Color(r, g, b, a) / 255f;
        }

    }
}