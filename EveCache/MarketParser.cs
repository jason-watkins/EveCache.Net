#region License
///<license>
/// EveCache.Net - EVE Cache File Reader Library
/// Copyright (C) 2011 Jason Watkins
/// 
/// Based on libevecache
/// Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
/// http://dev.eve-central.com/libevecache/
/// http://gitorious.org/libevecache
/// 
/// This library is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public
/// License as published by the Free Software Foundation; either
/// version 2 of the License, or (at your option) any later version.
/// 
/// This library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
/// General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public
/// License along with this library; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
///</license>
#endregion

namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class MarketParser
	{
		#region Fields
		private MarketList _List;
		private SNode _Stream;
		private bool _Valid;
		#endregion Fields

		#region Properties
		public virtual MarketList List { get { return _List; } private set { _List = value; } }
		protected virtual SNode Stream { get { return _Stream; } set { _Stream = value; } }
		private bool Valid { get { return _Valid; } set { _Valid = value; } }
		#endregion Properties

		#region Constructors
		public MarketParser(SNode stream)
		{
			Stream = stream;
			Valid = false;
		}

		public MarketParser(string fileName)
		{
			try
			{ 
				InitWithFile(fileName); 
			}
			catch (ParseException)
			{ 
				return; 
			}
		}
		#endregion Constructors

		#region Methods
		public void Parse();

		private void InitWithFile(string fileName)
		{
			//CacheFile cf = new CacheFile(fileName);
			//if (!cf.ReadFile())
			//    throw new ParseException("Can't open file " + fileName);

			//CacheFile_Iterator i = cf.Begin();
			//Parser parser = new Parser(i);
			//parser.Parse();
			//SNode snode = parser.Streams[0];
			//Stream = snode;
			//Parse();
			//Stream = null;
			//Valid = true;
		}

        private void Parse(SNode nest);

		private void parseDbRow(List<SNode> node)
		{
			MarketOrder order;

			for (int i = 0; i < node.Count; i++)
			{
				SNode value = node[i];
			}
		}
//void MarketParser::parseDbRow(const SNode* node)
//{
//    MarketOrder order;

//    std::vector<SNode*>::const_iterator i = node->members().begin();

//    for (; i != node->members().end(); ++i)
//    {


//        SNode* value = *i;
//        ++i;
//        SMarker* key = dynamic_cast<SMarker*>(*i);
//        SIdent* ident = dynamic_cast<SIdent*>(*i);

//        int typeKey = -1;

//        if (key != 0) {
//            typeKey = key->id();
//        } else {
//            if (ident->value() == "issueDate") {
//                typeKey = 131;
//            } else {
//                std::cerr << "Can't parse - giving up" << std::endl;
//                break;
//            }
//        }



//        SInt* intV = dynamic_cast<SInt*>(value);
//        SLongLong* longV = dynamic_cast<SLongLong*>(value);
//        SReal* realV = dynamic_cast<SReal*>(value);

//        int sintV = 0;
//        long long slongV = 0;
//        double srealV = 0.0;

//        if (longV != NULL)
//            slongV = longV->value();

//        if (intV != NULL)
//            sintV = intV->value();

//        if (realV != NULL)
//            srealV = realV->value();


//        switch(typeKey) {
//        case 139:
//            order.setPrice(slongV);
//            break;
//        case 161:
//            order.setVolRemaining(srealV);
//            break;
//        case 131:
//            order.setIssued(slongV);
//            break;
//        case 138:
//            order.setOrderID(slongV);
//            break;
//        case 160:
//            order.setVolEntered(sintV);
//            break;
//        case 137:
//            order.setMinVolume(sintV);
//            break;
//        case 155:
//            order.setStationID(sintV);
//            break;
//        case 141:
//            order.setRegionID(sintV);
//            _list.setRegion(sintV);
//            break;
//        case 150:
//            order.setSolarSystemID(sintV);
//            break;
//        case 41:
//            order.setJumps(sintV);
//            break;
//        case 74:
//            order.setType(sintV);
//            _list.setType(sintV);
//            break;
//        case 140:
//            order.setRange(sintV);
//            break;
//        case 126:
//            order.setDuration(sintV);
//            break;
//        case 116:
//            if (sintV)
//                order.setBid(true);
//            else
//                order.setBid(false);
//            break;
//        default:
//            std::cout << "Unknown key ID:" << (int)key->id() <<  " r: " << srealV << " l: " << slongV << " i: " << sintV << std::endl;
//            break;

//        }
//    }
//    _list.addOrder(order);


//}
		#endregion Methods
	}
}
