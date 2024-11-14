﻿using Util.Ui.Builders;
using Util.Ui.Extensions;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Radios.Builders;
using Util.Ui.NgZorro.Components.Radios.Configs;

namespace Util.Ui.NgZorro.Components.Radios.Renders;

/// <summary>
/// 单选框渲染器
/// </summary>
public class RadioRender : FormControlRenderBase {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 单选框组合共享配置
    /// </summary>
    private readonly RadioGroupShareConfig _shareConfig;
    /// <summary>
    /// 指令名称
    /// </summary>
    private readonly string _directiveName;

    /// <summary>
    /// 初始化单选框渲染器
    /// </summary>
    /// <param name="config">配置</param>
    /// <param name="directiveName">指令名称</param>
    public RadioRender( Config config, string directiveName ) : base( config ) {
        _config = config;
        _shareConfig = GetShareConfig();
        _directiveName = directiveName;
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
        var groupBuilder = GetRadioGroupBuilder();
        if ( _shareConfig.IsRadioExtend && _shareConfig.IsAutoCreateRadioGroup == true ) {
            groupBuilder.AppendContent( $"@for(item of {_shareConfig.ExtendId}.options;track item.value )" );
            groupBuilder.AppendContent( "{" );
        }
        groupBuilder.AppendContent( GetRadioBuilder() );
        if ( _shareConfig.IsRadioExtend && _shareConfig.IsAutoCreateRadioGroup == true ) {
            groupBuilder.AppendContent( "}" );
        }
        formControlBuilder.AppendContent( groupBuilder );
    }

    /// <summary>
    /// 获取单选框组合标签生成器
    /// </summary>
    private TagBuilder GetRadioGroupBuilder() {
        if ( _shareConfig.IsAutoCreateRadioGroup != true )
            return new EmptyContainerTagBuilder();
        var groupBuilder = new RadioGroupBuilder( _config );
        return groupBuilder.SelectExtend().AutoLoad()
            .Data().Url().QueryParam().Sort()
            .Name().NgModel().OnModelChange()
            .Required().RequiredMessage()
            .ValidationExtend().OnLoad();
    }

    /// <summary>
    /// 获取单选框标签生成器
    /// </summary>
    private TagBuilder GetRadioBuilder() {
        var builder = new RadioBuilder( _config );
        builder.Attribute( _directiveName );
        builder.Config();
        if ( _config.Content.IsEmpty() == false )
            builder.SetContent( _config.Content );
        if ( _shareConfig.IsRadioExtend )
            builder.Extend();
        return builder;
    }

    /// <inheritdoc />
    public override IHtmlContent Clone() {
        return new RadioRender( _config.Copy(), _directiveName );
    }
}