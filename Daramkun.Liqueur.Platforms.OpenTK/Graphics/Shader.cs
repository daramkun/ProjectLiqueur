﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Liqueur.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Liqueur.Graphics
{
	class Shader : IShader
	{
		int shaderId;

		public ShaderType ShaderType { get; private set; }

		private Shader ( IGraphicsDevice graphicsDevice, Graphics.ShaderType shaderType )
		{
			ShaderType = shaderType;
			shaderId = GL.CreateShader ( ConvertShaderType ( shaderType ) );
		}

		public Shader ( IGraphicsDevice graphicsDevice, Stream stream, Graphics.ShaderType shaderType )
			: this ( graphicsDevice, shaderType )
		{
			byte [] shaderCode = new byte [ stream.Length ];
			stream.Read ( shaderCode, 0, shaderCode.Length );
			SetShaderAndCompile ( Encoding.UTF8.GetString ( shaderCode ) );
		}

		public Shader ( IGraphicsDevice graphicsDevice, string shaderCode, Graphics.ShaderType shaderType )
			: this ( graphicsDevice, shaderType )
		{
			SetShaderAndCompile ( shaderCode );
		}

		private void SetShaderAndCompile ( string shaderCode )
		{
			GL.ShaderSource ( shaderId, shaderCode );
			GL.CompileShader ( shaderId );

			int compileState;
			GL.GetShader ( shaderId, ShaderParameter.CompileStatus, out compileState );
			if ( compileState == 0 )
				throw new ShaderCompilationException ( string.Format ( "Compile failed: [Shader Type: {0}], {1}", ShaderType, shaderCode ) );
		}

		~Shader ()
		{
			Dispose ( false );
		}

		protected virtual void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				GL.DeleteShader ( shaderId );
				shaderId = 0;
			}
		}

		public void Dispose ()
		{
			Dispose ( true );
			GC.SuppressFinalize ( this );
		}

		public void Attach ( IEffect effect )
		{
			GL.AttachShader ( ( effect as Effect ).programId, shaderId );
		}

		public void Detach ( IEffect effect )
		{
			GL.DetachShader ( ( effect as Effect ).programId, shaderId );
		}

		private OpenTK.Graphics.OpenGL.ShaderType ConvertShaderType ( Graphics.ShaderType shaderType )
		{
			switch ( shaderType )
			{
				case Graphics.ShaderType.VertexShader: return OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
				case Graphics.ShaderType.PixelShader: return OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
				default: return ( OpenTK.Graphics.OpenGL.ShaderType ) ( -1 );
			}
		}
	}
}