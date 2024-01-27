using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ImageTiles
{
    internal class GridNodesScaler
    {
        public ImagesStore imagesStore { get; set; }
        public GridNode rootNode;
        public Padding padding;
        public int mainWidth;
        public bool verticalFilling;

        public GridNodesScaler(ImagesStore imagesStore, GridNode rootNode, Padding padding, int mainWidth, bool verticalFilling)
        {
            this.imagesStore = imagesStore;
            this.rootNode = rootNode;
            this.padding = padding;
            this.mainWidth = mainWidth;
            this.verticalFilling = verticalFilling;
            SetDefaultScaleForTree(rootNode);
            AlignTree(rootNode);
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

        public void AlignTree(GridNode rootNode)
        {
            bool fillingTypeOfRootNode = !verticalFilling;

            double targetWidth =
                Math.Clamp(
                    mainWidth
                    - rootNode.childs.Count
                    * padding.horizontalSum,
                    0.1,
                    double.MaxValue);


            rootNode.ScaleByWidth(targetWidth);

            int treeDepth = rootNode.GetDepth();
            treeDepth++;
            for (int i = 0; i < treeDepth; i++)
                AlignNode(fillingTypeOfRootNode, rootNode);
        }

        private void AddPadding(GridNode node, Padding padding)
        {
            node.width -= padding.horizontalSum;
            node.height -= padding.verticalSum;

            foreach (var child in node.childs)
            {
                AddPadding(child, padding);
            }
        }


        public void AlignNode(bool verticalFilling, GridNode? node = null)
        {
            double branchLength = 0;
            foreach (var child in node.childs)
            {
                

                if (!child.isLeaf)
                    AlignNode(!verticalFilling, child);

                if (verticalFilling)
                {
                    double targetHeight =
                        Math.Clamp(
                            node.height
                            + padding.verticalSum
                            - (child.isLeaf ? 1 : child.childs.Count) * padding.verticalSum,
                            0.1,
                            double.MaxValue);

                    child.ScaleByHeight(targetHeight);

                    branchLength += child.width;
                }
                else
                {
                    double targetWidth =
                        Math.Clamp(
                            node.width
                            + padding.horizontalSum
                            - (child.isLeaf ? 1 : child.childs.Count) * padding.horizontalSum,
                            0.1,
                            double.MaxValue);

                    child.ScaleByWidth(targetWidth);

                    branchLength += child.height;
                }
            }

            if (verticalFilling)
                node.width = branchLength;
            else
                node.height = branchLength;



            if (verticalFilling && (node.width != node.childs.Sum(child => child.width)))
            {
                var w = node.width;
                var cSW = node.childs.Sum(child => child.width);
            }
            if (!verticalFilling && (node.height != node.childs.Sum(child => child.height)))
            {
                var h = node.height;
                var cSH = node.childs.Sum(child => child.height);
            }
        }
    }
}
