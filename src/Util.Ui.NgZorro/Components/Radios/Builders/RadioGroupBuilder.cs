﻿using Util.Ui.Angular.Configs;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Radios.Configs;
using Util.Ui.NgZorro.Components.Selects.Builders;
using Util.Ui.NgZorro.Enums;

namespace Util.Ui.NgZorro.Components.Radios.Builders; 

/// <summary>
/// 单选框组合标签生成器
/// </summary>
public class RadioGroupBuilder : FormControlBuilderBase<RadioGroupBuilder> {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 单选框组合共享配置
    /// </summary>
    private readonly RadioGroupShareConfig _shareConfig;

    /// <summary>
    /// 初始化单选框组合标签生成器
    /// </summary>
    /// <param name="config">配置</param>
    public RadioGroupBuilder( Config config ) : base( config, "nz-radio-group" ) {
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
    /// 配置所有单选按钮名称
    /// </summary>
    public override RadioGroupBuilder Name() {
        AttributeIfNotEmpty( "nzName", _config.GetValue( UiConst.Name ) );
        AttributeIfNotEmpty( "[nzName]", _config.GetValue( AngularConst.BindName ) );
        AttributeIfNotEmpty( "name", _config.GetValue( UiConst.Name ) );
        AttributeIfNotEmpty( "[name]", _config.GetValue( AngularConst.BindName ) );
        return this;
    }
        
    /// <summary>
    /// 配置禁用
    /// </summary>
    public RadioGroupBuilder Disabled() {
        AttributeIfNotEmpty( "[nzDisabled]", _config.GetValue( UiConst.Disabled ) );
        return this;
    }

    /// <summary>
    /// 配置尺寸
    /// </summary>
    public RadioGroupBuilder Size() {
        AttributeIfNotEmpty( "nzSize", _config.GetValue<ButtonSize?>( UiConst.Size )?.Description() );
        AttributeIfNotEmpty( "[nzSize]", _config.GetValue( AngularConst.BindSize ) );
        return this;
    }

    /// <summary>
    /// 配置风格
    /// </summary>
    public RadioGroupBuilder ButtonStyle() {
        AttributeIfNotEmpty( "nzButtonStyle", _config.GetValue<RadioStyle?>( UiConst.ButtonStyle )?.Description() );
        AttributeIfNotEmpty( "[nzButtonStyle]", _config.GetValue( AngularConst.BindButtonStyle ) );
        return this;
    }
    
    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        base.ConfigBase( _config );
        ConfigForm().Name().Disabled().Size().ButtonStyle()
            .Data().Url().AutoLoad().QueryParam().Sort()
            .SpaceItem().OnLoad();
    }

    /// <summary>
    /// 数据源扩展
    /// </summary>
    public RadioGroupBuilder Data() {
        AttributeIfNotEmpty( "[data]", _config.GetValue( UiConst.Data ) );
        return this;
    }

    /// <summary>
    /// Api地址扩展
    /// </summary>
    public RadioGroupBuilder Url() {
        AttributeIfNotEmpty( "url", _config.GetValue( UiConst.Url ) );
        AttributeIfNotEmpty( "[url]", _config.GetValue( AngularConst.BindUrl ) );
        return this;
    }

    /// <summary>
    /// 选择框扩展
    /// </summary>
    public RadioGroupBuilder SelectExtend() {
        Attribute( $"#{_shareConfig.ExtendId}", "xSelectExtend" );
        Attribute( "x-select-extend" );
        return this;
    }

    /// <summary>
    /// 配置自动加载
    /// </summary>
    public RadioGroupBuilder AutoLoad() {
        AttributeIfNotEmpty( "[autoLoad]", _config.GetBoolValue( UiConst.AutoLoad ) );
        return this;
    }

    /// <summary>
    /// 配置查询参数
    /// </summary>
    public RadioGroupBuilder QueryParam() {
        AttributeIfNotEmpty( "[(queryParam)]", _config.GetValue( UiConst.QueryParam ) );
        return this;
    }

    /// <summary>
    /// 配置排序条件
    /// </summary>
    public RadioGroupBuilder Sort() {
        AttributeIfNotEmpty( "order", _config.GetValue( UiConst.Sort ) );
        AttributeIfNotEmpty( "[order]", _config.GetValue( AngularConst.BindSort ) );
        return this;
    }

    /// <summary>
    /// 配置加载完成事件
    /// </summary>
    public RadioGroupBuilder OnLoad() {
        AttributeIfNotEmpty( "(onLoad)", _config.GetValue( UiConst.OnLoad ) );
        return this;
    }
}