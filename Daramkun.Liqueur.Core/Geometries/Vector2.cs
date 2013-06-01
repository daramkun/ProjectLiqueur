﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Liqueur.Geometries
{
	public struct Vector2 : IComparer<Vector2>, ICollision<Vector2>, ICollision<Circle>, ICollision<Rectangle>
	{
		public static readonly Vector2 Zero = new Vector2 ( 0 );

		public float X, Y;

		//public float X { get { return x; } set { x = value; } }
		//public float Y { get { return y; } set { y = value; } }

		public Vector2 ( float value )
		{
			X = Y = value;
		}

		public Vector2 ( float x, float y )
		{
			this.X = x;
			this.Y = y;
		}

		public float Length { get { return ( float ) Math.Sqrt ( X * X + Y * Y ); } }

		public static Vector2 operator + ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( v1.X + v2.X, v1.Y + v2.Y );
		}

		public static Vector2 operator - ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( v1.X - v2.X, v1.Y - v2.Y );
		}

		public static Vector2 operator - ( Vector2 v )
		{
			return new Vector2 ( -v.X, -v.Y );
		}

		public static Vector2 operator * ( Vector2 v1, float v2 )
		{
			return new Vector2 ( v1.X * v2, v1.Y * v2 );
		}

		public static Vector2 operator * ( Vector2 v1, Vector2 v2 )
		{
			return Cross ( v1, v2 );
		}

		public static Vector2 operator * ( float v1, Vector2 v2 )
		{
			return new Vector2 ( v2.X * v1, v2.Y * v1 );
		}

		public static Vector2 operator / ( Vector2 v1, float v2 )
		{
			return new Vector2 ( v1.X / v2, v1.Y / v2 );
		}

		public static Vector2 operator / ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( v1.X / v2.X, v1.Y / v2.Y );
		}

		public Vector2 Normalize ()
		{
			float length = Length;
			X /= length;
			Y /= length;
			return this;
		}

		public static float Dot ( Vector2 v1, Vector2 v2 )
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public static Vector2 Cross ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( v1.X * v2.Y, v1.Y * v2.X );
		}

		public static Vector2 Normalize ( Vector2 value )
		{
			return value.Normalize ();
		}

		public static float Distance ( Vector2 v1, Vector2 v2 )
		{
			return ( float ) Math.Sqrt ( Math.Pow ( v2.X - v1.X, 2 ) + Math.Pow ( v2.Y - v1.Y, 2 ) );
		}

		public int Compare ( Vector2 x, Vector2 y )
		{
			float xl = x.Length;
			float yl = y.Length;
			if ( xl < yl )
				return 1;
			else if ( xl > yl )
				return -1;
			else
				return 0;
		}

		public override bool Equals ( object obj )
		{
			if ( !( obj is Vector2 ) ) return false;
			return Length == ( ( Vector2 ) obj ).Length;
		}

		public static bool operator == ( Vector2 v1, Vector2 v2 )
		{
			return v1.Equals ( v2 );
		}

		public static bool operator != ( Vector2 v1, Vector2 v2 )
		{
			return !v1.Equals ( v2 );
		}

		public override int GetHashCode ()
		{
			return ( int ) ( Length );
		}

		public override string ToString ()
		{
			return String.Format ( "{{X:{0}, Y:{1}}}", X, Y );
		}

		public bool IsCollisionTo ( Vector2 obj )
		{
			return ( obj.X == X && obj.Y == Y );
		}

		public bool IsCollisionTo ( Circle obj )
		{
			return obj.IsCollisionTo ( this );
		}

		public bool IsCollisionTo ( Rectangle obj )
		{
			return obj.IsCollisionTo ( this );
		}
	}	
}
