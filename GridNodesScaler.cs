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

        //padding data

        public GridNodesScaler(ImagesStore imagesStore, GridNode rootNode)
        {
            this.imagesStore = imagesStore;
            this.rootNode = rootNode;
            SetDefaultScaleForNode(rootNode);
        }



        public void SetDefaultScaleForNode(GridNode? node = null)
        {
            if (node.isLeaf)
            {
                var imgWH = imagesStore.GetWidthAndHeightOfImage(node.imageUid);
                node.width = imgWH.Item1 /10;
                node.height = imgWH.Item2 / 10;
            }
            else
            {
                foreach (var child in node.childs)
                {
                    SetDefaultScaleForNode(child);
                }
            }
        }
    }
}
