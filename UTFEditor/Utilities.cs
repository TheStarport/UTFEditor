using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTFEditor
{
    class Utilities
    {
        public static string GetString(System.Windows.Forms.TreeNode node, int startIndex)
        {
            byte[] data = node.Tag as byte[];
            if (data != null)
                return GetString(data, startIndex);
            return "";
        }

        public static string GetString(System.Windows.Forms.TreeNode node, int startIndex, int maxLength)
        {
            byte[] data = node.Tag as byte[];
            if (data != null)
                return GetString(data, startIndex, maxLength);
            return "";
        }

        public static string GetString(byte[] data, int startIndex)
        {
            return GetString(data, startIndex, data.Length);
        }

        public static string GetString(byte[] data, int startIndex, int maxLength)
        {
            int index = data.ToList().FindIndex(startIndex, maxLength, delegate(byte value) { return value == 0; }) - startIndex;
            if (index < 0 || index > maxLength - 1)
                index = maxLength;
            return System.Text.Encoding.ASCII.GetString(data, startIndex, index).Trim();
        }

        /// <summary>
        /// Look up table for id creation.
        /// </summary>
        private static uint[] createIDTable = null;

        /// <summary>
        /// Function for calculating the Freelancer data nickname hash.
        /// Algorithm from flhash.exe by sherlog@t-online.de (2003-06-11)
        /// </summary>
        /// <param name="nickName">String to hash</param>
        /// <returns>Return the hashcode</returns>
        public static uint CreateID(string nickName)
        {
            const uint FLHASH_POLYNOMIAL = 0xA001;
            const int LOGICAL_BITS = 30;
            const int PHYSICAL_BITS = 32;

            // Build the crc lookup table if it hasn't been created
            if (createIDTable == null)
            {
                createIDTable = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    uint x = i;
                    for (uint bit = 0; bit < 8; bit++)
                        x = ((x & 1) == 1) ? (x >> 1) ^ (FLHASH_POLYNOMIAL << (LOGICAL_BITS - 16)) : x >> 1;
                    createIDTable[i] = x;
                }
                if (2926433351 != CreateID("st01_to_st03_hole")) throw new Exception("Create ID hash algoritm is broken!");
                if (2460445762 != CreateID("st02_to_st01_hole")) throw new Exception("Create ID hash algoritm is broken!");
                if (2263303234 != CreateID("st03_to_st01_hole")) throw new Exception("Create ID hash algoritm is broken!");
                if (2284213505 != CreateID("li05_to_li01")) throw new Exception("Create ID hash algoritm is broken!");
                if (2293678337 != CreateID("li01_to_li05")) throw new Exception("Create ID hash algoritm is broken!");
            }

            byte[] tNickName = Encoding.ASCII.GetBytes(nickName.ToLowerInvariant());

            // Calculate the hash.
            uint hash = 0;
            for (int i = 0; i < tNickName.Length; i++)
                hash = (hash >> 8) ^ createIDTable[(byte)hash ^ tNickName[i]];
            // b0rken because byte swapping is not the same as bit reversing, but 
            // that's just the way it is; two hash bits are shifted out and lost
            hash = (hash >> 24) | ((hash >> 8) & 0x0000FF00) | ((hash << 8) & 0x00FF0000) | (hash << 24);
            hash = (hash >> (PHYSICAL_BITS - LOGICAL_BITS)) | 0x80000000;

            return hash;
        }

	    // CRC32 table for Freelancer
        static UInt32[] flcrc32tbl =
	    {
            0x0, 0x9073096, 0x120E612C, 0x1B0951BA, 
            0xFF6DC419, 0xF66AF48F, 0xED63A535, 0xE46495A3, 
            0xFEDB8832, 0xF7DCB8A4, 0xECD5E91E, 0xE5D2D988, 
            0x01B64C2B, 0x8B17CBD, 0x13B82D07, 0x1ABF1D91, 
            0xFDB71064, 0xF4B020F2, 0xEFB97148, 0xE6BE41DE, 
            0x02DAD47D, 0xBDDE4EB, 0x10D4B551, 0x19D385C7, 
            0x036C9856, 0xA6BA8C0, 0x1162F97A, 0x1865C9EC, 
            0xFC015C4F, 0xF5066CD9, 0xEE0F3D63, 0xE7080DF5, 
            0xFB6E20C8, 0xF269105E, 0xE96041E4, 0xE0677172, 
            0x0403E4D1, 0x0D04D447, 0x160D85FD, 0x1F0AB56B, 
            0x05B5A8FA, 0x0CB2986C, 0x17BBC9D6, 0x1EBCF940, 
            0xFAD86CE3, 0xF3DF5C75, 0xE8D60DCF, 0xE1D13D59, 
            0x06D930AC, 0x0FDE003A, 0x14D75180, 0x1DD06116, 
            0xF9B4F4B5, 0xF0B3C423, 0xEBBA9599, 0xE2BDA50F, 
            0xF802B89E, 0xF1058808, 0xEA0CD9B2, 0xE30BE924, 
            0x076F7C87, 0x0E684C11, 0x15611DAB, 0x1C662D3D, 
            0xF6DC4190, 0xFFDB7106, 0xE4D220BC, 0xEDD5102A, 
            0x09B18589, 0x00B6B51F, 0x1BBFE4A5, 0x12B8D433, 
            0x0807C9A2, 0x0100F934, 0x1A09A88E, 0x130E9818, 
            0xF76A0DBB, 0xFE6D3D2D, 0xE5646C97, 0xEC635C01, 
            0x0B6B51F4, 0x026C6162, 0x196530D8, 0x1062004E, 
            0xF40695ED, 0xFD01A57B, 0xE608F4C1, 0xEF0FC457, 
            0xF5B0D9C6, 0xFCB7E950, 0xE7BEB8EA, 0xEEB9887C, 
            0x0ADD1DDF, 0x03DA2D49, 0x18D37CF3, 0x11D44C65, 
            0x0DB26158, 0x04B551CE, 0x1FBC0074, 0x16BB30E2, 
            0xF2DFA541, 0xFBD895D7, 0xE0D1C46D, 0xE9D6F4FB, 
            0xF369E96A, 0xFA6ED9FC, 0xE1678846, 0xE860B8D0, 
            0x0C042D73, 0x05031DE5, 0x1E0A4C5F, 0x170D7CC9, 
            0xF005713C, 0xF90241AA, 0xE20B1010, 0xEB0C2086, 
            0x0F68B525, 0x066F85B3, 0x1D66D409, 0x1461E49F, 
            0x0EDEF90E, 0x07D9C998, 0x1CD09822, 0x15D7A8B4, 
            0xF1B33D17, 0xF8B40D81, 0xE3BD5C3B, 0xEABA6CAD, 
            0xEDB88320, 0xE4BFB3B6, 0xFFB6E20C, 0xF6B1D29A, 
            0x12D54739, 0x1BD277AF, 0xDB2615, 0x9DC1683, 
            0x13630B12, 0x1A643B84, 0x16D6A3E, 0x86A5AA8, 
            0xEC0ECF0B, 0xE509FF9D, 0xFE00AE27, 0xF7079EB1, 
            0x100F9344, 0x1908A3D2, 0x201F268, 0xB06C2FE, 
            0xEF62575D, 0xE66567CB, 0xFD6C3671, 0xF46B06E7, 
            0xEED41B76, 0xE7D32BE0, 0xFCDA7A5A, 0xF5DD4ACC, 
            0x11B9DF6F, 0x18BEEFF9, 0x3B7BE43, 0xAB08ED5, 
            0x16D6A3E8, 0x1FD1937E, 0x4D8C2C4, 0xDDFF252, 
            0xE9BB67F1, 0xE0BC5767, 0xFBB506DD, 0xF2B2364B, 
            0xE80D2BDA, 0xE10A1B4C, 0xFA034AF6, 0xF3047A60, 
            0x1760EFC3, 0x1E67DF55, 0x56E8EEF, 0xC69BE79, 
            0xEB61B38C, 0xE266831A, 0xF96FD2A0, 0xF068E236, 
            0x140C7795, 0x1D0B4703, 0x60216B9, 0xF05262F, 
            0x15BA3BBE, 0x1CBD0B28, 0x7B45A92, 0xEB36A04, 
            0xEAD7FFA7, 0xE3D0CF31, 0xF8D99E8B, 0xF1DEAE1D, 
            0x1B64C2B0, 0x1263F226, 0x96AA39C, 0x6D930A, 
            0xE40906A9, 0xED0E363F, 0xF6076785, 0xFF005713, 
            0xE5BF4A82, 0xECB87A14, 0xF7B12BAE, 0xFEB61B38, 
            0x1AD28E9B, 0x13D5BE0D, 0x8DCEFB7, 0x1DBDF21, 
            0xE6D3D2D4, 0xEFD4E242, 0xF4DDB3F8, 0xFDDA836E, 
            0x19BE16CD, 0x10B9265B, 0xBB077E1, 0x2B74777, 
            0x18085AE6, 0x110F6A70, 0xA063BCA, 0x3010B5C, 
            0xE7659EFF, 0xEE62AE69, 0xF56BFFD3, 0xFC6CCF45, 
            0xE00AE278, 0xE90DD2EE, 0xF2048354, 0xFB03B3C2, 
            0x1F672661, 0x166016F7, 0xD69474D, 0x46E77DB, 
            0x1ED16A4A, 0x17D65ADC, 0xCDF0B66, 0x5D83BF0, 
            0xE1BCAE53, 0xE8BB9EC5, 0xF3B2CF7F, 0xFAB5FFE9, 
            0x1DBDF21C, 0x14BAC28A, 0xFB39330, 0x6B4A3A6, 
            0xE2D03605, 0xEBD70693, 0xF0DE5729, 0xF9D967BF, 
            0xE3667A2E, 0xEA614AB8, 0xF1681B02, 0xF86F2B94, 
            0x1C0BBE37, 0x150C8EA1, 0xE05DF1B, 0x702EF8D, 
        };

	    /// Get a CRC code for a model name. This type of CRC is used in model files.
        /// Many thanks to Anton's CRCTool source for the algorithm and the CRC table!
        public static uint FLModelCRC(string str)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(str.ToLowerInvariant());

            uint crc = 0xFFFFFFFF;
            for (uint i = 0; i < str.Length; i++)
                crc = ((crc >> 8) & 0x00FFFFFF) ^ flcrc32tbl[(crc ^ (data[i])) & 0xFF];
            crc = crc ^ 0xFFFFFFFF;
            return crc;
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
