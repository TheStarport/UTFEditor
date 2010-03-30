using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Structures by Mario Brito from FLModelToolby Anton (Xtreme Team Studios).
namespace UTFEditor
{
    class VWireData
    {
        /// Wire Data Header
        public UInt32 HeaderSize;
        public UInt32 VMeshLibId;
        public UInt16 VertexOffset;
        public UInt16 NoVertices;
        public UInt16 NoRefVertices;
        public UInt16 MaxVertNoPlusOne;
    
        public struct Line
        {
            public UInt16 Point1;
            public UInt16 Point2;
        };

        public List<VWireData.Line> Lines = new List<VWireData.Line>();

        public VWireData(byte[] data)
        {
            int pos = 0;

            // Read the data header.
            HeaderSize = BitConverter.ToUInt32(data, pos); pos += 4;
            VMeshLibId = BitConverter.ToUInt32(data, pos); pos += 4;
            VertexOffset = BitConverter.ToUInt16(data, pos); pos += 2;
            NoVertices = BitConverter.ToUInt16(data, pos); pos += 2;
            NoRefVertices = BitConverter.ToUInt16(data, pos); pos += 2;
            MaxVertNoPlusOne = BitConverter.ToUInt16(data, pos); pos += 2;

            pos = (int)HeaderSize;

            // Read Line data
            int no_lines = NoRefVertices / 2;
            for (int count = 0; count < no_lines; count++)
            {
                Line item = new Line();
                item.Point1 = BitConverter.ToUInt16(data, pos); pos += 2;
                item.Point2 = BitConverter.ToUInt16(data, pos); pos += 2;
                Lines.Add(item);
            }
        }
    }
}
