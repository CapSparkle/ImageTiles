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
                node.width = imgWH.Item1;
                node.height = imgWH.Item2;
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

            float rootNodeAspectRatio = rootNode.GetAspectRatio();
            float targetWidthWithPadding = mainWidth;
            rootNode.width =
                (float)Math.Clamp(
                    targetWidthWithPadding
                    - rootNode.childs.Count
                    * padding.horizontalSum,
                    0.1,
                    double.MaxValue);

            rootNode.height =
                rootNode.width
                * rootNodeAspectRatio;


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

        //public void ProcessNode(bool verticalFilling, GridNode? node = null)
        //{
        //    if (node.isLeaf && !algoritmData.ContainsKey(node))
        //    {
        //        algoritmData[node.parent].AddValue(verticalFilling ? node.width : node.height);
        //        algoritmData[node] = new AlignAlgoritmData(childsNumber: -1);
        //    }
        //    else if (!node.isLeaf)
        //    {
        //        if (!algoritmData.ContainsKey(node))
        //        {
        //            algoritmData[node] = new AlignAlgoritmData(childsNumber: node.childs.Count);
        //        }
        //        else if (algoritmData[node].alignmentDone)
        //        {
        //            return;
        //        }
        //        else if (algoritmData[node].isMeanLengthCounted)
        //        {
        //            if (verticalFilling)
        //                node.height = algoritmData[node].meanLength;
        //            else
        //                node.width = algoritmData[node].meanLength;

                    
        //            AlignNode(verticalFilling, node);

        //            algoritmData[node].alignmentDone = true;

        //            //if (node.parent != null)
        //            //{
        //            //    algoritmData[node.parent].AddValue(
        //            //        verticalFilling ? 
        //            //        (node.width + (node.childs.Count - 1) * padding.horizontalSum) 
        //            //        :
        //            //        (node.height + (node.childs.Count - 1) * padding.verticalSum));
        //            //}
                        

        //            return;
        //        }

        //        foreach (var child in node.childs)
        //        {
        //            ProcessNode(!verticalFilling, child);
        //        }
        //    }
        //}

        public void AlignNode(bool verticalFilling, GridNode? node = null)
        {
            float branchLength = 0;
            foreach (var child in node.childs)
            {
                if (verticalFilling)
                {
                    float childAspectRatioFlipped = child.GetAspectRatioFlipped();
                    float targetHeightWithPadding = node.height + padding.verticalSum;
                    child.height =
                        (float)Math.Clamp(
                            targetHeightWithPadding
                            - (child.isLeaf ? 1 : child.childs.Count)
                            * padding.verticalSum,
                            0.1,
                            double.MaxValue);

                    child.width =
                        child.height
                        * childAspectRatioFlipped;

                    branchLength += child.width;
                }
                else
                {
                    float childAspectRatio = child.GetAspectRatio();
                    float targetWidthWithPadding = node.width + padding.horizontalSum;
                    child.width =
                        (float)Math.Clamp(
                            targetWidthWithPadding
                            - (child.isLeaf ? 1 : child.childs.Count)
                            * padding.horizontalSum,
                            0.1,
                            double.MaxValue);

                    child.height =
                        child.width
                        * childAspectRatio;

                    branchLength += child.height;
                }
                if (!child.isLeaf)
                    AlignNode(!verticalFilling, child);
            }

            if (verticalFilling)
                node.width = branchLength;
            else
                node.height = branchLength;

        }
    }
}
