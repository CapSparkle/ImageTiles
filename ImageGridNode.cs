using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTiles
{
    internal class ImageGridNode
    {
        public int imageUid;
        public float height, width;
        public List<ImageGridNode> childs { get; } = new();
        public bool isLeaf { get { return ((childs == null) || (childs.Count == 0)); } }
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
        public ImageGridNode(int imageUid = -1)
        {
            this.imageUid = imageUid;
        }

        public ImageGridNode AddLeaf(int imageUid)
        {
            childs.Add(new ImageGridNode(imageUid));
            return this;
        }

        public ImageGridNode AddBranch()
        {
            var newNode = new ImageGridNode();
            childs.Add(newNode);
            return newNode;
        }
    }
}
