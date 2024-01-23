using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTiles
{
    internal class GridNode
    {
        public int imageUid;
        public int height, width;
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
        public GridNode(int imageUid = -1)
        {
            this.imageUid = imageUid;
        }

        public GridNode AddLeaf(int imageUid)
        {
            if (this.isLeaf)
            {
                throw new Exception("Cannot add child to leaf");
            }
            childs.Add(new GridNode(imageUid));
            return this;
        }

        public GridNode AddBranch()
        {
            var newNode = new GridNode();
            childs.Add(newNode);
            return newNode;
        }
    }

    internal class ImageGridNodeState
    {
        public bool Flag { get; set; }
        // Другие свойства состояния
    }
}
