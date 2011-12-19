namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	public class Parser
	{
		#region Fields
		private CacheFile_Iterator _Iter;
		private uint _ShareCount;
		private uint _ShareCursor;
		SNode[][] _ShareObj;
		uint[] _ShareMap;
		private List<SNode> _Streams;
		#endregion Fields

		#region Properties
		private CacheFile_Iterator Iter { get { return _Iter; } set { _Iter = value; } }
		private uint ShareCount { get { return _ShareCount; } set { _ShareCount = value; } }
		private uint ShareCursor { get { return _ShareCursor; } set { _ShareCursor = value; } }
		private SNode[][] ShareObj { get { return _ShareObj; } set { _ShareObj = value; } }
		private uint[] ShareMap { get { return _ShareMap; } set { _ShareMap = value; } }
		public List<SNode> Streams { get { return _Streams; } private set { _Streams = value; } }
		#endregion Properties

		#region Events
		#endregion Events

		#region Constructors
		public Parser(CacheFile_Iterator iter)
		{
			Iter = iter;
			ShareCount = 0;
			ShareCursor = 0;
			ShareObj = null;
			ShareMap = null;
		}
		#endregion Constructors

		#region Static Methods
		public static void rle_unpack(byte[] in_buf, int in_length, List<byte> buffer)
		{
			buffer.Clear();
			if (in_length == 0)
				return;

			int i = 0;
			while (i < in_buf.Length)
			{
				Packer_Opcap opcap = new Packer_Opcap(in_buf[i++]);
				if (opcap.tzero)
				{
					for (int count = opcap.tlen + 1; count > 0; count--)
						buffer.Add(0);
				}
				else
				{
					for (int count = 8 - opcap.tlen; count > 0; count--)
						buffer.Add(in_buf[i++]);
				}

				if (opcap.bzero)
				{
					for (int count = opcap.blen + 1; count > 0; count--)
						buffer.Add(0);
				}
				else
				{
					for (int count = 8 - opcap.blen; count > 0; count--)
						buffer.Add(in_buf[i++]);
				}
			}
		}
		#endregion Static Methods
		#region Methods
		protected SNode GetDBRow();

		public int GetLength();

		public void Parse();

		protected void Parse(SNode node, int limit);

		protected SNode ParseOne()
		{
			byte check;
			byte isshared = 0;
			SNode thisobj = null;
			SDBRow lastDbRow = null;

			try
			{
				byte type = Iter.ReadChar();
				check = (byte)(type & 0x3f);
				isshared = (byte)(type & 0x40);
			}
			catch (EndOfFileException)
			{
				return null;
			}

			switch ((EStreamCode)check)
			{
				case EStreamCode.EStreamStart:
					break;
				case EStreamCode.ENone:
					thisobj = new SNone();
					break;
				case EStreamCode.EString:
					thisobj = new SString(Iter.ReadString(Iter.ReadChar()));
					break;
				case EStreamCode.ELong:
					thisobj = new SLong(Iter.ReadLong());
					break;
				case EStreamCode.EInteger:
					thisobj = new SInt(Iter.ReadInt());
					break;
				case EStreamCode.EShort:
					thisobj = new SInt(Iter.ReadShort());
					break;
				case EStreamCode.EByte:
					thisobj = new SInt(Iter.ReadChar());
					break;
				case EStreamCode.ENeg1Integer:
					thisobj = new SInt(-1);
					break;
				case EStreamCode.E0Integer:
					thisobj = new SInt(0);
					break;
				case EStreamCode.E1Integer:
					thisobj = new SInt(1);
					break;
				case EStreamCode.EReal:
					thisobj = new SReal(Iter.ReadDouble());
					break;
				case EStreamCode.E0Real:
					thisobj = new SReal(0);
					break;
				case EStreamCode.E0String:
					thisobj = new SString(null);
					break;
				case EStreamCode.EString3:
					thisobj = new SString(Iter.ReadString(1));
					break;
				case EStreamCode.EString4:
					goto case EStreamCode.EString;
				case EStreamCode.EMarker:
					thisobj = new SMarker((byte)GetLength());
					break;
				case EStreamCode.EUnicodeString:
					goto case EStreamCode.EString;
				case EStreamCode.EIdent:
					thisobj = new SIdent(Iter.ReadString(GetLength()));
					break;
				case EStreamCode.ETuple:
					{
						int length = GetLength();
						thisobj = new STuple((uint)length);
						Parse(thisobj, length);
						break;
					}
				case EStreamCode.ETuple2:
					goto case EStreamCode.ETuple;
				case EStreamCode.EDict:
					{
						int len = (GetLength() * 2);
						SDict dict = new SDict((uint)len);
						thisobj = dict;
						Parse(dict, len);
						break;
					}
				case EStreamCode.EObject:
					thisobj = new SObject();
					Parse(thisobj, 2);
					break;
				case EStreamCode.ESharedObj:
					thisobj = ShareGet(GetLength());
					break;
				case EStreamCode.EChecksum:
					thisobj = new SString("checksum");
					Iter.ReadInt();
					break;
				case EStreamCode.EBoolTrue:
					thisobj = new SInt(1);
					break;
				case EStreamCode.EBoolFalse:
					thisobj = new SInt(0);
					break;
				case EStreamCode.EObject22:
					{
						SObject obj = new SObject();
						thisobj = obj;
						Parse(thisobj, 1);

						string oclass = obj.Name;
						if (oclass == "dbutil.RowList")
						{
							SNode row;
							while ((row = ParseOne()) != null)
								obj.AddMember(row);
						}
						break;
					}
				case EStreamCode.EObject23:
					goto case EStreamCode.EObject22;
				case EStreamCode.E0Tuple:
					thisobj = new STuple(0);
					break;
				case EStreamCode.E1Tuple:
					thisobj = new STuple(1);
					Parse(thisobj, 1);
					break;
				case EStreamCode.E0Tuple2:
					goto case EStreamCode.E0Tuple;
				case EStreamCode.E1Tuple2:
					goto case EStreamCode.E1Tuple;
				case EStreamCode.EEmptyString:
					thisobj = new SString(string.Empty);
					break;
				case EStreamCode.EUnicodeString2:
					/* Single unicode character */
					thisobj = new SString(Iter.ReadString(2));
					break;
				case EStreamCode.ECompressedRow:
					thisobj = GetDBRow();
					break;
				case EStreamCode.ESubstream:
					{
						int len = GetLength();
						CacheFile_Iterator IterSub = new CacheFile_Iterator(Iter);
						IterSub.Limit = len;
						SSubstream ss = new SSubstream(len);
						thisobj = ss;
						Parser sp = new Parser(IterSub);
						sp.Parse();
						for (int i = 0; i < sp.Streams.Count; i++)
							ss.AddMember(sp.Streams[i].Clone());

						Iter.Seek(IterSub.Position);
						break;
					}
				case EStreamCode.E2Tuple:
					thisobj = new STuple(2);
					Parse(thisobj, 2);
					break;
				case EStreamCode.EString2:
					goto case EStreamCode.EString;
				case EStreamCode.ESizedInt:
					switch (Iter.ReadChar())
					{
						case 8:
							thisobj = new SLong(Iter.ReadLong());
							break;
						case 4:
							thisobj = new SInt(Iter.ReadInt());
							break;
						case 3:
							// The following seems more correct than the forumla used.
							// int value = (Iter.Char() << 16) + (Iter.ReadChar());
							thisobj = new SInt((Iter.ReadChar()) + (Iter.ReadChar() << 16));
							break;
						case 2:
							thisobj = new SInt(Iter.ReadShort());
							break;
					}
					break;
				case (EStreamCode)0x2d:
					if (Iter.ReadChar() != (byte)0x2d)
						throw new ParseException("Didn't encounter a double 0x2d where one was expected at " + (Iter.Position - 2));
					else if (lastDbRow != null)
						lastDbRow.IsLast = true;
					return null;
				case 0:
					break;
				default:
					throw new ParseException("Can't identify type " + String.Format("{0:0x2}", check) +
											" at position " + String.Format("{0:0x2}", Iter.Position) + " limit " + Iter.Limit);

			}

			if (thisobj == null)
				throw new ParseException("no thisobj in parseone");

			if (isshared != 0)
			{
				if (thisobj == null)
					throw new ParseException("shared flag but no obj");
				ShareAdd(thisobj);
			}

			return thisobj;
		}

		protected void ShareAdd(SNode obj);

		protected SNode ShareGet(int id);

		protected uint ShareInit();

		protected void SharSkip();
		#endregion Methods
	}

	public struct Packer_Opcap
	{
		public byte tlen;
		public bool tzero;
		public byte blen;
		public bool bzero;

		public Packer_Opcap(byte b)
		{
			tlen = (byte)((byte)(b << 5) >> 5);
			tzero = (byte)((byte)(b << 4) >> 7) == 1;
			blen = (byte)((byte)(b << 1) >> 5);
			bzero = (byte)(b >> 7) == 1;
		}
	}
}
