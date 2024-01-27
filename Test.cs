using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTiles
{
    internal class Test
    {
        public ImagesStore imagesStore { get; set; }
        public GridNode rootNode;
        public Padding padding;
        public int mainWidth;
        public bool verticalFilling;

        public Test(ImagesStore imagesStore, GridNode rootNode, Padding padding, int mainWidth, bool verticalFilling)
        {
            this.imagesStore = imagesStore;
            this.rootNode = rootNode;
            this.padding = padding;
            this.mainWidth = mainWidth;
            this.verticalFilling = verticalFilling;
            SetDefaultScaleForTree(rootNode);
        }

        public void SetDefaultScaleForTree(GridNode? rootNode = null)
        {
            SetDefaultScaleForNode(!verticalFilling, rootNode);
        }

        void SetDefaultScaleForNode(bool verticalFilling, GridNode? node = null)
        {
            if (node.isLeaf)
            {
                var imgWH = imagesStore.GetWidthAndHeightOfImage(node.imageUid);
                node.width = imgWH.Item1 / 20;
                node.height = imgWH.Item2 / 20;
            }
            else
            {
                bool verticalFillingOfChild = !verticalFilling;
                foreach (var child in node.childs)
                {
                    SetDefaultScaleForNode(verticalFillingOfChild, child);
                }

                node.width = node.childs.Sum(child => child.width);
                node.height = node.childs.Sum(child => child.height);

                if (!verticalFilling)
                {
                    node.width /= node.childs.Count();
                }
                else
                {
                    node.height /= node.childs.Count();
                }
            }
        }

        public async Task ScaleChange(GridNode node, Action redraw)
        {
            float scaleFactor = 1;
            while (true) 
            {
                redraw();
                scaleFactor += 0.2f;
                //scaleFactor = 1
                //    f + (scaleFactor % 1.8f);
                ScaleNode(false, scaleFactor, node);
                await Task.Delay(1500);
            }
        }

        public void ScaleNode(bool verticalFilling, float scaleFactor, GridNode node)
        {
            if (verticalFilling)
            {

            }
            else
            {
                double nodeAspectRatio = node.height / node.width;
                double targetWidthWithPadding = (node.width + node.childs.Count * padding.horizontalSum) * scaleFactor;
                double targetHeight =
                    (Math.Clamp((targetWidthWithPadding - node.childs.Count * padding.horizontalSum), 1, double.MaxValue)
                    * nodeAspectRatio);

                node.height = targetHeight;
                node.width = targetHeight * (1 / nodeAspectRatio);

                foreach(var child in node.childs)
                {
                    child.width = node.height * (child.width / child.height);
                    child.height = node.height;
                }
            }
        }
    }
}
