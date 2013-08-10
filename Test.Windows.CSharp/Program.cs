﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Daramkun.Liqueur;
using Daramkun.Liqueur.Audio;
using Daramkun.Liqueur.Common;
using Daramkun.Liqueur.Contents;
using Daramkun.Liqueur.Contents.Decoder.Audios;
using Daramkun.Liqueur.Contents.Decoder.Images;
using Daramkun.Liqueur.Graphics;
using Daramkun.Liqueur.Logging;
using Daramkun.Liqueur.Mathematics;
using Daramkun.Liqueur.Mathematics.Transforms;
using Daramkun.Liqueur.Nodes;
using Daramkun.Liqueur.Platforms;

namespace Test.Windows.CSharp
{
	static class Program
	{
		[StructLayout ( LayoutKind.Sequential )]
		public struct Vertex
		{
			Vector2 position;
			Vector2 texture;

			public Vertex ( Vector2 position, Vector2 texture )
			{
				this.position = position;
				this.texture = texture;
			}
		}

		class InternalScene : Node
		{
			IShader vertexShader, fragShader;
			IEffect effect;
			IVertexBuffer<Vertex> vertexBuffer;
			IIndexBuffer indexBuffer;
			ITexture2D texture;
			IRenderBuffer renderBuffer;

			public override void Intro ( params object [] args )
			{
				vertexShader = LiqueurSystem.GraphicsDevice.CreateShader ( @"#version 150
layout(location = 0) in vec4 a_position;
layout(location = 1) in vec2 a_texcoord;

out vec2 v_texcoord;

void main () {
	gl_Position = a_position;
	v_texcoord = a_texcoord;
}
					", ShaderType.VertexShader );
				fragShader = LiqueurSystem.GraphicsDevice.CreateShader ( @"#version 150
in vec2 v_texcoord;

uniform sampler2D texture;

void main () {
	gl_FragColor = texture ( texture, v_texcoord.st );
}
					", ShaderType.FragmentShader );
				effect = LiqueurSystem.GraphicsDevice.CreateEffect ( vertexShader, fragShader );

				vertexBuffer = LiqueurSystem.GraphicsDevice.CreateVertexBuffer<Vertex> ( FlexibleVertexFormat.PositionXY | FlexibleVertexFormat.Diffuse, new Vertex []
				{
					new Vertex ( new Vector2 ( -0.5f, +0.5f ), new Vector2 ( 0, 1 ) ),
					new Vertex ( new Vector2 ( +0.5f, -0.5f ), new Vector2 ( 1, 0 ) ),
					new Vertex ( new Vector2 ( -0.5f, -0.5f ), new Vector2 ( 0, 0 ) ),
					new Vertex ( new Vector2 ( +0.5f, +0.5f ), new Vector2 ( 1, 1 ) ),
				} );
				indexBuffer = LiqueurSystem.GraphicsDevice.CreateIndexBuffer ( new int [] { 0, 1, 2, 0, 1, 3, } );

				using ( FileStream fs = new FileStream ( "temp.png", FileMode.Open ) )
					texture = LiqueurSystem.GraphicsDevice.CreateTexture2D ( new PngDecoder ().Decode ( fs ) );

				renderBuffer = LiqueurSystem.GraphicsDevice.CreateRenderBuffer ( 800, 600 );
				base.Intro ( args );
			}

			public override void Draw ( GameTime gameTime )
			{
				LiqueurSystem.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, new Color ( 0.2f, 0.5f, 0.4f, 1.0f ) );

				LiqueurSystem.GraphicsDevice.RenderTarget = renderBuffer;
				effect.Dispatch ( ( IEffect ef ) =>
				{
					effect.SetTextures ( new TextureArgument () { Uniform = "texture", Texture = texture } );
					LiqueurSystem.GraphicsDevice.Draw<Vertex> ( PrimitiveType.TriangleList, vertexBuffer, indexBuffer );
				} );

				LiqueurSystem.GraphicsDevice.RenderTarget = null;
				effect.Dispatch ( ( IEffect ef ) =>
				{
					effect.SetTextures ( new TextureArgument () { Uniform = "texture", Texture = renderBuffer } );
					LiqueurSystem.GraphicsDevice.Draw<Vertex> ( PrimitiveType.TriangleList, vertexBuffer, indexBuffer );
				} );
				base.Draw ( gameTime );
			}

			public override void Outro ()
			{
				renderBuffer.Dispose ();
				texture.Dispose ();
				indexBuffer.Dispose ();
				vertexBuffer.Dispose ();
				effect.Dispose ();
				fragShader.Dispose ();
				vertexShader.Dispose ();
				base.Outro ();
			}
		}

		[STAThread]
		static void Main ()
		{
			Logger.AddDefaultLogWriter ();
			LiqueurSystem.Run ( new Launcher (), new InternalScene () );
		}
	}
}