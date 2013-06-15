﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Liqueur.Geometries;
using Daramkun.Liqueur.Inputs;
using Daramkun.Liqueur.Inputs.States;

namespace Daramkun.Liqueur.Platforms
{
	public interface IWindow
	{
		string Title { get; set; }
		bool IsCursorVisible { get; set; }
		bool IsResizable { get; set; }
		object Icon { get; set; }

		object Handle { get; }

		void DoEvents ();
		void FailFast ( string message, Exception exception );
	}
}