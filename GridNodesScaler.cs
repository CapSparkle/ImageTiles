using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageTiles
{
    internal class GridNodesScaler
    {
        public ImagesStore imagesStore { get; set; }
        public GridNode rootNode;
        public Padding padding;
        public int mainLength;
        public bool verticalCompletion;

        public GridNodesScaler(ImagesStore imagesStore, GridNode rootNode, Padding padding, int mainLength, bool verticalCompletion)
        {
            this.imagesStore = imagesStore;
            this.rootNode = rootNode;
            this.padding = padding;
            this.mainLength = mainLength;
            this.verticalCompletion = verticalCompletion;
            SetDefaultScaleForTree(rootNode);
        }

        public void SetDefaultScaleForTree(GridNode? node = null)
        {
            SetDefaultScaleForNode(!verticalCompletion, node);
            node.width = node.height = 0;
        }

        void SetDefaultScaleForNode(bool verticalCompletion, GridNode? node = null)
        {
            if (node.isLeaf)
            {
                var imgWH = imagesStore.GetWidthAndHeightOfImage(node.imageUid);
                node.width = imgWH.Item1 / 20;
                node.height = imgWH.Item2 / 20;
            }
            else
            {
                bool verticalCompletionOfChild = !verticalCompletion;
                foreach (var child in node.childs)
                {
                    SetDefaultScaleForNode(verticalCompletionOfChild, child);
                }

                if (!verticalCompletion)
                {
                    node.width = node.childs.Max(child => child.width);
                    node.height = 0;
                }
                else
                {
                    node.height = node.childs.Max(child => child.height);
                    node.width = 0;
                }
            }
        }
        
        public void AlignGrid(GridNode? node = null)
        {
            if (node.isLeaf)
            {
                var imgWH = imagesStore.GetWidthAndHeightOfImage(node.imageUid);
                node.width = imgWH.Item1 / 20;
                node.height = imgWH.Item2 / 20;
            }
            else
            {
                bool verticalCompletionOfChild = !verticalCompletion;
                foreach (var child in node.childs)
                {
                    SetDefaultScaleForNode(verticalCompletionOfChild, child);
                }

                if (!verticalCompletion)
                {
                    node.width = node.childs.Max(child => child.width);
                    node.height = 0;
                }
                else
                {
                    node.height = node.childs.Max(child => child.height);
                    node.width = 0;
                }
            }
        }

    }
}
