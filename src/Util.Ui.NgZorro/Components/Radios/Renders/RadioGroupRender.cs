﻿using Util.Ui.Builders;
using Util.Ui.Extensions;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Radios.Builders;
using Util.Ui.NgZorro.Components.Radios.Configs;

namespace Util.Ui.NgZorro.Components.Radios.Renders;

/// <summary>
/// 单选框组合渲染器
/// </summary>
public class RadioGroupRender : FormControlRenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 单选框组合共享配置
    /// </summary>
    private readonly RadioGroupShareConfig _shareConfig;

    /// <summary>
    /// 初始化单选框组合渲染器
    /// </summary>
    /// <param name="config">配置</param>
    public RadioGroupRender( Config config ) : base( config ) {
        _config = config;
        _shareConfig = GetShareConfig();
    }

    /// <summary>
    /// 获取单选框组合共享配置
    /// </summary>
    private RadioGroupShareConfig GetShareConfig() {
        return _config.GetValueFromItems<RadioGroupShareConfig>() ?? new RadioGroupShareConfig();
    }

    /// <summary>
    /// 添加表单控件
    /// </summary>
    protected override void AppendControl( TagBuilder formControlBuilder ) {
        var builder = new RadioGroupBuilder( _config );
        builder.Config();
        if ( _shareConfig.IsRadioExtend && _shareConfig.IsAutoCreateRadioGroup != true ) {
            builder.AppendContent( $"@for(item of {_shareConfig.ExtendId}.options;track item.value )" );
            builder.AppendContent( "{" );
        }
        _config.Content.AppendTo( builder );
        if ( _shareConfig.IsRadioGroupExtend )
            builder.SelectExtend();
        builder.AppendContent( GetRadioBuilder() );
        if ( _shareConfig.IsRadioExtend && _shareConfig.IsAutoCreateRadioGroup != true ) {
            builder.AppendContent( "}" );
        }
        formControlBuilder.AppendContent( builder );
    }

    /// <summary>
    /// 获取单选框标签生成器
    /// </summary>
    private TagBuilder GetRadioBuilder() {
        if ( _shareConfig.IsAutoCreateRadio != true )
            return new EmptyContainerTagBuilder();
        var builder = new RadioBuilder( _config );
        builder.Attribute( "nz-radio" );
        if ( _shareConfig.IsRadioExtend )
            builder.Extend();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new RadioGroupRender( _config.Copy() );
    }
}