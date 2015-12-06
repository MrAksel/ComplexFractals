using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalRenderers
{
    public struct SuperSampling
    {
        public int minx, maxx;
        public int miny, maxy;

        public List<Offset> offsets;

        public bool advanced;
        public string name;

        public static readonly SuperSampling OneByOne = new SuperSampling(0, 0, 0, 0, "One by one");
        public static readonly SuperSampling ThreeByThree = new SuperSampling(-1, -1, 1, 1, "Three by three");
        public static readonly SuperSampling FiveByFive = new SuperSampling(-2, -2, 2, 2, "Five by five");
        public static readonly SuperSampling ThreeByThreeCross = new SuperSampling(new Offset[]
        {
            new Offset(0, -1),
            new Offset(-1, 0),
            new Offset(0, 0),
            new Offset(1, 0),
            new Offset(0, 1)
        }.ToList(), "Three by three cross");
        public static readonly SuperSampling FiveByFiveCross = new SuperSampling(new Offset[]
        {
            new Offset(0, -2),
            new Offset(0, -1),
            new Offset(-2, 0),
            new Offset(-1, 0),
            new Offset(0, 0),
            new Offset(+1, 0),
            new Offset(+2, 0),
            new Offset(0, +1),
            new Offset(0, +2),
        }.ToList(), "Five by five cross");

        public SuperSampling(int minx, int miny, int maxx, int maxy)
        {
            this.minx = minx;
            this.miny = miny;
            this.maxx = maxx;
            this.maxy = maxy;
            this.offsets = null;
            this.advanced = false;
            name = string.Format("{0}x{1}", Math.Abs(maxx - minx), Math.Abs(maxy - miny));
        }

        public SuperSampling(int minx, int miny, int maxx, int maxy, string name)
        {
            this.minx = minx;
            this.miny = miny;
            this.maxx = maxx;
            this.maxy = maxy;
            this.offsets = null;
            this.advanced = false;
            this.name = name;
        }

        public SuperSampling(List<Offset> offsets)
        {
            minx = maxx = 0;
            miny = maxy = 0;
            this.offsets = offsets;
            this.advanced = true;

            name = string.Format("Custom");
        }

        public SuperSampling(List<Offset> offsets, string name)
        {
            minx = maxx = 0;
            miny = maxy = 0;
            this.offsets = offsets;
            this.advanced = true;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public struct Offset
    {
        public int x;
        public int y;

        public Offset(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
