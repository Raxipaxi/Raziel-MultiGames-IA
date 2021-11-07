// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( Ps1GraphicsPPSRenderer ), PostProcessEvent.AfterStack, "Ps1Graphics", true )]
public sealed class Ps1GraphicsPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "PixelX" )]
	public FloatParameter _PixelX = new FloatParameter { value = 256f };
	[Tooltip( "PixelY" )]
	public FloatParameter _PixelY = new FloatParameter { value = 144f };
	[Tooltip( "EffectIntensity" )]
	[Range(0,1)]public FloatParameter _EffectIntensity = new FloatParameter { value = 0f };
}

public sealed class Ps1GraphicsPPSRenderer : PostProcessEffectRenderer<Ps1GraphicsPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Ps1Graphics" ) );
		sheet.properties.SetFloat( "_PixelX", settings._PixelX );
		sheet.properties.SetFloat( "_PixelY", settings._PixelY );
		sheet.properties.SetFloat( "_EffectIntensity", settings._EffectIntensity );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
