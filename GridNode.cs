using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTiles
{
    internal class GridNode
    {
        public int imageUid;
        public double height, width;

        public void ScaleByHeight(double newHeight)
        {
            width = (width / height) * newHeight;
            height = newHeight;
        }

        public void ScaleByWidth(double newWidth)
        {
            height = (height / width) * newWidth;
            width = newWidth;
        }

        //public double aspectRatio { get; private set;}
        //public double aspectRatioFlipped { get; private set; }

        public GridNode parent;
        public List<GridNode> childs { get; } = new();
        public bool isLeaf { get { return imageUid > 0; } }
        public bool noBranchesInChilds
        {
            get
            {
                if (!isLeaf)
                    foreach (var child in childs)
                        if (!child.isLeaf)
                            return false;

                return true;
            }
        }

        public double GetAspectRatio()
        {
            return height / width;
        }

        public double GetAspectRatioFlipped()
        {
            return width / height;
        }

        public GridNode(int imageUid = -1, GridNode parent = null)
        {
            this.imageUid = imageUid;
            this.parent = parent;
        }

        public GridNode AddLeaf(int imageUid)
        {
            if (this.isLeaf)
            {
                throw new Exception("Cannot add child to leaf");
            }
            childs.Add(new GridNode(imageUid, this));
            return this;
        }

        public GridNode AddBranch()
        {
            var newNode = new GridNode(parent:this);
            childs.Add(newNode);
            return newNode;
        }

        public int GetDepth()
        {
            if (isLeaf)
            {
                return 0;
            }

            int maxDepth = 0;
            foreach (var child in childs)
            {
                int childDepth = child.GetDepth();
                maxDepth = Math.Max(maxDepth, childDepth);
            }

            return maxDepth + 1;
        }
    }

    internal struct Padding
    {
        public int Up = 0,
            Bottom = 0,
            Left = 0,
            Right = 0;

        public int verticalSum
        {
            get 
            {
                return Up + Bottom;
            }
        }

        public int horizontalSum
        {
            get
            {
                return Left + Right;
            }
        }

        public Padding(int up = 0, int down = 0, int left = 0, int right = 0)
        {
            Up = up;
            Bottom = down;
            Left = left;
            Right = right;
        }
    }
}
