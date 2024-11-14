﻿using Util.Ui.Angular.Builders;
using Util.Ui.Angular.Configs;
using Util.Ui.Angular.Extensions;
using Util.Ui.NgZorro.Components.Forms.Configs;
using Util.Ui.NgZorro.Configs;
using Util.Ui.NgZorro.Enums;

namespace Util.Ui.NgZorro.Components.Forms.Builders;

/// <summary>
/// 表单标签生成器
/// </summary>
public class FormBuilder : AngularTagBuilder {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 表单共享配置
    /// </summary>
    private readonly FormShareConfig _shareConfig;

    /// <summary>
    /// 初始化表单标签生成器
    /// </summary>
    /// <param name="config">配置</param>
    public FormBuilder( Config config ) : base( config, "form" ) {
        base.Attribute( "nz-form" );
        _config = config;
        _shareConfig = _config.GetValueFromItems<FormShareConfig>();
    }

    /// <summary>
    /// 配置布局方式
    /// </summary>
    public FormBuilder Layout() {
        AttributeIfNotEmpty( "nzLayout", _config.GetValue<FormLayout?>( UiConst.Layout )?.Description() );
        AttributeIfNotEmpty( "[nzLayout]", _config.GetValue( AngularConst.BindLayout ) );
        return this;
    }

    /// <summary>
    /// 配置自动提示
    /// </summary>
    public FormBuilder AutoTips() {
        AttributeIfNotEmpty( "[nzAutoTips]", _config.GetValue( UiConst.AutoTips ) );
        return this;
    }

    /// <summary>
    /// 配置禁用自动提示
    /// </summary>
    public FormBuilder DisableAutoTips() {
        AttributeIfNotEmpty( "[nzDisableAutoTips]", _config.GetValue( UiConst.DisableAutoTips ) );
        return this;
    }

    /// <summary>
    /// 配置不显示标签冒号
    /// </summary>
    public FormBuilder NoColon() {
        AttributeIfNotEmpty( "[nzNoColon]", _config.GetValue( UiConst.NoColon ) );
        return this;
    }

    /// <summary>
    /// 配置标签文本是否换行
    /// </summary>
    public FormBuilder LabelWrap() {
        AttributeIfNotEmpty( "[nzLabelWrap]", _config.GetValue( UiConst.LabelWrap ) );
        return this;
    }

    /// <summary>
    /// 配置标签文本对齐方式
    /// </summary>
    public FormBuilder LabelAlign() {
        AttributeIfNotEmpty( "nzLabelAlign", _config.GetValue<LabelAlign?>( UiConst.LabelAlign )?.Description() );
        AttributeIfNotEmpty( "[nzLabelAlign]", _config.GetValue( AngularConst.BindLabelAlign ) );
        return this;
    }

    /// <summary>
    /// 配置标签提示图标
    /// </summary>
    public FormBuilder TooltipIcon() {
        AttributeIfNotEmpty( "nzTooltipIcon", _config.GetValue<AntDesignIcon?>( UiConst.TooltipIcon )?.Description() );
        AttributeIfNotEmpty( "[nzTooltipIcon]", _config.GetValue( AngularConst.BindTooltipIcon ) );
        return this;
    }

    /// <summary>
    /// 配置自动完成
    /// </summary>
    public FormBuilder AutoComplete() {
        var isAutoComplete = _config.GetValue<bool?>( UiConst.Autocomplete );
        if ( isAutoComplete == null )
            return this;
        if ( isAutoComplete == true ) {
            Attribute( "autocomplete", "on" );
            return this;
        }
        Attribute( "autocomplete", "off" );
        return this;
    }

    /// <summary>
    /// 配置表单组
    /// </summary>
    public FormBuilder FormGroup() {
        AttributeIfNotEmpty( "[formGroup]", GetFormGroup() );
        return this;
    }

    /// <summary>
    /// 获取表单组
    /// </summary>
    private string GetFormGroup() {
        return _config.GetValue( UiConst.FormGroup );
    }

    /// <summary>
    /// 配置是否不验证表单
    /// </summary>
    public FormBuilder Novalidate() {
        AttributeIf( "novalidate", _config.GetValue<bool?>( UiConst.Novalidate ) == true );
        return this;
    }

    /// <summary>
    /// 配置事件
    /// </summary>
    public FormBuilder Events() {
        AttributeIfNotEmpty( "(ngSubmit)", _config.GetValue( UiConst.OnSubmit ) );
        return this;
    }

    /// <summary>
    /// 配置
    /// </summary>
    public override void Config() {
        base.Config();
        Layout().AutoTips().DisableAutoTips().NoColon().LabelWrap().LabelAlign()
            .TooltipIcon().AutoComplete().FormGroup().Novalidate()
            .Events();
        AutocompleteOff();
    }

    /// <summary>
    /// 配置标识
    /// </summary>
    protected override void ConfigId( Config config ) {
        this.RawId( _config );
        if ( Util.Helpers.Environment.IsTest && _config.Contains( UiConst.Id ) == false )
            return;
        if ( _shareConfig.IsSearch.SafeValue() && _config.Contains( UiConst.Id ) == false )
            return;
        if ( GetFormGroup().IsEmpty() == false ) {
            AttributeIf( $"#{_shareConfig.FormId}", _shareConfig.FormId.IsEmpty() == false );
            return;
        }
        Attribute( $"#{_shareConfig.FormId}", "ngForm" );
    }

    /// <summary>
    /// 配置全局设置 autocomplete="off"
    /// </summary>
    protected void AutocompleteOff() {
        var options = NgZorroOptionsService.GetOptions();
        if ( options.EnableAutocompleteOff )
            Attribute( "autocomplete", "off" );
    }
}