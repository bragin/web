using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    /*
     * Bloom filter for CSS style selection optimisation.
     *
     * Attempting to match CSS rules by querying the client about DOM nodes via
     * the selection callbacks is slow.  To avoid this, clients may pass a node
     * bloom filter to css_get_style.  This bloom filter has bits set according
     * to the node's ancestor element names, class names and id names.
     *
     * Generate the bloom filter by adding calling css_bloom_add_hash() on each
     * ancestor element name, class name and id name for the node.
     *
     * Use the insesnsitive hash value:
     *
     *     lwc_err = lwc_string_caseless_hash_value(str, &hash);
     */

    // libcss/src/select/bloom.h:48
    public class CssBloom
    {
        public uint[] Bloom;

        /* Size of bloom filter as multiple of 32 bits.
         * Has to be 4, 8, or 16.
         * Larger increases optimisation of style selection engine but uses more memory.
         */
        byte BloomSize;
        byte IndexBits;


        public CssBloom(byte bloomSize = 4)
        {
            if (bloomSize != 4 &&
                bloomSize != 8 &&
                bloomSize != 16)
            {
                Debug.Assert(false);
                return;
            }
            BloomSize = bloomSize;

            // Setup index bit mask
            IndexBits = (byte)(BloomSize - 1);

            // Allocate bloom
            Bloom = new uint[BloomSize];
            for (int i =0;i < BloomSize;i++)
            {
                Bloom[i] = 0;
            }
        }

        // Add a hash value to the bloom filter
        // bloom.h:57
        public void AddHash(uint hash)
        {
            byte bit = (byte)(hash & 0x1f); // Top 5 bits
            uint index = (hash >> 5) & IndexBits; // Next N bits

            Bloom[index] |= (uint)(1 << bit);
        }

        // Test whether bloom filter contains given hash value.
        // bloom.h:74
        public bool HasHash(uint hash)
        {
            byte bit = (byte)(hash & 0x1f); // Top 5 bits
            uint index = (hash >> 5) & IndexBits; // Next N bits

            return (Bloom[index] & (uint)(1 << bit)) != 0;
        }

        // Test whether bloom 'a' is a subset of bloom 'b'.
        // bloom.h:92
        public bool InBloom(CssBloom a)
        {
            for (int i = 0;i < BloomSize; i++)
            {
                if ((a.Bloom[i] & Bloom[i]) != a.Bloom[i])
                    return false;
            }

            return true;
        }
    }
}
