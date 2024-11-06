namespace AllIn1SpringsToolkit
{
	public struct Vector2Bool: IVectorBool
	{
		public static readonly Vector2Bool AllTrue = new Vector2Bool(true, true);
		public static readonly Vector2Bool AllFalse = new Vector2Bool(false, false);

		public bool x;
		public bool y;

		public bool this[int index]
		{
			get
			{
				bool res = false;
				switch (index)
				{
					case 0:
						res = x;
						break;
					case 1:
						res = y;
						break;
				}

				return res;
			}
			set
			{
				switch (index)
				{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
				}
			}
		}

		public Vector2Bool(bool x, bool y)
		{
			this.x = x;
			this.y = y;
		}

		public int GetSize()
		{
			return 2;
		}

		public void SetValue(int index, bool value)
		{
			this[index] = value;
		}
	}
}