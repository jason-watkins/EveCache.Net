#region License
/* EveCache.Net - C# EVE Cache File Reader Library
 * Copyright (C) 2011 Jason Watkins <jason@blacksunsystems.net>
 *
 * Based on libevecache
 * Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
 * http://dev.eve-central.com/libevecache/
 * http://gitorious.org/libevecache
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#endregion

namespace EveCache
{
	public enum EStreamCode : byte
	{
        EStreamStart = 0x7e,
        ENone = 0x01, // Python None type
        EString = 0x2, // Another type of string, also ids
        ELong = 0x3, // 64 bit value?
        EInteger = 0x04, // 4 byte, little endian?
        EShort = 0x05, // 2 byte
        EByte = 0x6, // cfReader think
        ENeg1Integer = 0x07, // int == -1
        E0Integer = 0x08, // int  == 0
        E1Integer = 0x09, // int == 1
        EReal = 0x0a, // floating point, 64 bits, assume matches machine double type
        E0Real = 0x0b,
        E0String = 0xe, // 0 length string
        EString3 = 0xf, // really? another? single character string
        EString4 = 0x10,
        EMarker = 0x11, // A one byte identifier code
        EUnicodeString = 0x12,
        EIdent = 0x13, // an identifier string
        ETuple = 0x14, // a tuple of N objects
        ETuple2 = 0x15, // a tuple of N objects, variant 2
        EDict = 0x16, // N objects, consisting of key object and value object
        EObject = 0x17, // Object type ?
        ESharedObj = 0x1b, // shared object reference
        EChecksum = 0x1c,
        EBoolTrue = 0x1f,
        EBoolFalse = 0x20,
        EObject22 = 0x22, // a database header field of some variety
        EObject23 = 0x23, // another datatype containing ECompressedRows/DBRows
        E0Tuple = 0x24, // a tuple of 0 objects
        E1Tuple = 0x25, // a tuple of 1 objects
        E0Tuple2 = 0x26,
        E1Tuple2 = 0x27, // a tuple of 1 objects, variant 2
        EEmptyString = 0x28, // empty
        EUnicodeString2 = 0x29,
        ECompressedRow = 0x2a, // the datatype from hell, a RLEish compressed row
        ESubstream = 0x2b, // substream - length bytes followed by 0x7e
        E2Tuple = 0x2c, // a tuple of 2 objects
        EString2 = 0x2e, // stringtastic
        ESizedInt = 0x2f, // when you can't decide ahead of time how long to make the integer...
    }
}
