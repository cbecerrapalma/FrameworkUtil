﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Util.Ui.Angular.Builders;
using Util.Ui.Angular.Configs;
using Util.Ui.Helpers;
using Util.Ui.NgZorro.Enums;
using Util.Ui.NgZorro.Extensions;

namespace Util.Ui.NgZorro.Components.Images.Builders;

/// <summary>
/// 图片标签生成器
/// </summary>
public class ImageBuilder : AngularTagBuilder {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;

    /// <summary>
    /// 初始化图片标签生成器
    /// </summary>
    public ImageBuilder( Config config ) : base( config,"img", TagRenderMode.SelfClosing ) {
        _config = config;
        base.Attribute( "nz-image" );
    }

    /// <summary>
    /// 配置图片地址
    /// </summary>
    public ImageBuilder Src() {
        AttributeIfNotEmpty( "nzSrc", _config.GetValue( UiConst.Src ) );
        AttributeIfNotEmpty( "[nzSrc]", _config.GetValue( AngularConst.BindSrc ) );
        return this;
    }

    /// <summary>
    /// 配置加载失败容错地址
    /// </summary>
    public ImageBuilder Fallback() {
        AttributeIfNotEmpty( "nzFallback", _config.GetValue( UiConst.Fallback ) );
        AttributeIfNotEmpty( "[nzFallback]", _config.GetValue( AngularConst.BindFallback ) );
        return this;
    }

    /// <summary>
    /// 配置加载失败容错地址
    /// </summary>
    public ImageBuilder Placeholder() {
        AttributeIfNotEmpty( "nzPlaceholder", _config.GetValue( UiConst.Placeholder ) );
        AttributeIfNotEmpty( "[nzPlaceholder]", _config.GetValue( AngularConst.BindPlaceholder ) );
        return this;
    }

    /// <summary>
    /// 配置是否禁止预览
    /// </summary>
    public ImageBuilder DisablePreview() {
        AttributeIfNotEmpty( "[nzDisablePreview]", _config.GetValue( UiConst.DisablePreview ) );
        return this;
    }

    /// <summary>
    /// 配置导航时是否关闭预览
    /// </summary>
    public ImageBuilder CloseOnNavigation() {
        AttributeIfNotEmpty( "[nzCloseOnNavigation]", _config.GetValue( UiConst.CloseOnNavigation ) );
        return this;
    }

    /// <summary>
    /// 配置文字方向
    /// </summary>
    public ImageBuilder Direction() {
        AttributeIfNotEmpty( "nzDirection", _config.GetValue<Direction?>( UiConst.Direction )?.Description() );
        AttributeIfNotEmpty( "[nzDirection]", _config.GetValue( AngularConst.BindDirection ) );
        return this;
    }

    /// <summary>
    /// 配置宽度
    /// </summary>
    public ImageBuilder Width() {
        var width = SizeHelper.GetValue( _config.GetValue( UiConst.Width ) );
        AttributeIfNotEmpty( "width", width );
        AttributeIfNotEmpty( "[width]", _config.GetValue( AngularConst.BindWidth ) );
        return this;
    }

    /// <summary>
    /// 配置高度
    /// </summary>
    public ImageBuilder Height() {
        var height = SizeHelper.GetValue( _config.GetValue( UiConst.Height ));
        AttributeIfNotEmpty( "height", height );
        AttributeIfNotEmpty( "[height]", _config.GetValue( AngularConst.BindHeight ) );
        return this;
    }

    /// <summary>
    /// 配置文本描述
    /// </summary>
    public ImageBuilder Alt() {
        AttributeIfNotEmpty( "alt", _config.GetValue( UiConst.Alt ) );
        AttributeIfNotEmpty( "[alt]", _config.GetValue( AngularConst.BindAlt ) );
        return this;
    }

    /// <summary>
    /// 配置缩放的每步倍数
    /// </summary>
    public ImageBuilder ScaleStep() {
        AttributeIfNotEmpty( "[nzScaleStep]", _config.GetValue( UiConst.ScaleStep ) );
        return this;
    }

    /// <summary>
    /// 配置事件
    /// </summary>
    public ImageBuilder Events() {
        AttributeIfNotEmpty( "(load)", _config.GetValue( UiConst.OnLoad ) );
        return this;
    }

    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        base.Config();
        Src().Fallback().Placeholder().DisablePreview().CloseOnNavigation().Direction()
            .Width().Height().Alt().ScaleStep().Events()
            .SpaceItem( _config );
    }
}