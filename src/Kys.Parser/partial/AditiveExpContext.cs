﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kys.Parser;
partial class KysParser
{
	partial class AditiveExpContext
	{
		bool? _pot;

		public bool Pot
		{
			get
			{
				if (_pot.HasValue) return _pot.Value;
				_pot = ADITIVEText.Equals("+");
				return _pot.Value;
			}
		}
	}
}