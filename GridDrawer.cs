﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageTiles
{
    internal class GridDrawer
    {
        private readonly ImagesStore imagesStore;

        int startX, startY;
        bool verticalFilling = false;

        Padding padding; 

        public GridDrawer(ImagesStore imagesStore,
            int startX,
            int startY, 
            bool verticalFilling
            )
        {
            this.imagesStore = imagesStore;
            this.startX = startX;
            this.startY = startY;
            this.verticalFilling = verticalFilling;
        }

        public void DrawImageGrid(SKCanvas canvas, GridNode rootNode, Padding padding)
        {
            this.padding = padding;
            float currentX = startX,
                currentY = startY;

            DrawImageNode(canvas, rootNode, ref currentX, ref currentY, !verticalFilling);    
        }

        void DrawImageNode(SKCanvas canvas, GridNode node, ref float currentX, ref float currentY, bool verticalFilling)
        {
            if (node.isLeaf) 
            {
                DrawImage(canvas, node, ref currentX, ref currentY, verticalFilling);
            }
            else
            {

                float newX = currentX,
                    newY = currentY;

                if (verticalFilling)
                    currentY += (node.height + padding.verticalSum);
                else
                    currentX += (node.width + padding.horizontalSum);

                verticalFilling = !verticalFilling;
                foreach (var child in node.childs)
                {
                    DrawImageNode(canvas, child, ref newX, ref newY, verticalFilling);
                }
            }
        }

        void DrawImage(SKCanvas canvas, GridNode node, ref float currentX, ref float currentY, bool verticalFilling)
        {
            int imgUidToDraw = node.imageUid;
            if (node.imageUid <= 0)
                imgUidToDraw = 1;
            //add img filler

            var image = imagesStore.GetImage(imgUidToDraw);
            if (image != null)
            {
                canvas.DrawImage(image, new SKRect(
                    currentX + padding.Left, 
                    currentY + padding.Up, 
                    currentX + padding.Left + node.width, 
                    currentY + padding.Up + node.height));

                image.Dispose();
            }

            

            if (verticalFilling)
            {
                currentY += (node.height + padding.horizontalSum);
            }
            else
                currentX += (node.width + padding.verticalSum);
        }

    }
}
