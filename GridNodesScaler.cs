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
            AlignTree(rootNode);
        }

        public void SetDefaultScaleForTree(GridNode? rootNode = null)
        {
            SetDefaultScaleForNode(!verticalCompletion, rootNode);
            rootNode.width = rootNode.height = 0;
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


        Dictionary<GridNode, AlignAlgoritmData> algoritmData;
        public void AlignTree(GridNode? rootNode = null)
        {
            algoritmData = new();
            algoritmData[rootNode] = new AlignAlgoritmData(childsNumber: rootNode.childs.Count);

            bool completionTypeOfRootNode = !verticalCompletion;
            while (algoritmData[rootNode].alignmentDone == false)
            {
                ProcessNode(completionTypeOfRootNode, rootNode);
            }
        }

        public void ProcessNode(bool verticalCompletion, GridNode? node = null)
        {
            if (node.isLeaf && !algoritmData.ContainsKey(node))
            {
                algoritmData[node.parent].AddValue(verticalCompletion ? node.width : node.height);
                algoritmData[node] = new AlignAlgoritmData(childsNumber: -1);
            }
            else if (!node.isLeaf)
            {
                if (!algoritmData.ContainsKey(node))
                {
                    algoritmData[node] = new AlignAlgoritmData(childsNumber: node.childs.Count);
                }
                else if (algoritmData[node].alignmentDone)
                {
                    return;
                }
                else if (algoritmData[node].isMeanLengthCounted)
                {
                    if (verticalCompletion)
                        node.height = algoritmData[node].meanLength;
                    else
                        node.width = algoritmData[node].meanLength;

                    float nodeLength = 0;
                    AlignNode(verticalCompletion, ref nodeLength, node);

                    if (verticalCompletion)
                        node.width = nodeLength;
                    else
                        node.height = nodeLength;


                    algoritmData[node].alignmentDone = true;

                    if (node.parent != null)
                        algoritmData[node.parent].AddValue(verticalCompletion ? node.width : node.height);

                    return;
                }

                foreach (var child in node.childs)
                {
                    ProcessNode(!verticalCompletion, child);
                }
            }
        }

        public void AlignNode(bool verticalCompletion, ref float nodeLength, GridNode? node = null)
        {
            foreach (var child in node.childs)
            {
                if (verticalCompletion)
                {
                    child.width *= (node.height / child.height);
                    child.height = node.height;
                    nodeLength += child.width;
                }
                else
                {
                    child.height *= (node.width / child.width);
                    child.width = node.width;
                    nodeLength += child.height;
                }
                if (!child.isLeaf)
                    AlignNode(!verticalCompletion, ref nodeLength, child);
            }
        }
    }

    internal class AlignAlgoritmData
    {
        public bool alignmentDone = false;
        public float meanLength { get { return (sum / childsNumber); } }
        float sum = 0;
        
        public bool isMeanLengthCounted
        {
            get
            {
                return childsNumber == lengthesAccounted;
            }
        }

        public int childsNumber;
        public int lengthesAccounted = 0;

        public AlignAlgoritmData(int childsNumber)
        {
            this.childsNumber = childsNumber;
        }

        public void AddValue(float value)
        {
            sum += value;
            lengthesAccounted ++;
        }
    }
}
