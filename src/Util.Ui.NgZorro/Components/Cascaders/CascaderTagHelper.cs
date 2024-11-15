﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Ui.NgZorro.Components.Base;
using Util.Ui.NgZorro.Components.Cascaders.Renders;
using Util.Ui.NgZorro.Components.Selects.Helpers;
using Util.Ui.NgZorro.Enums;
using Util.Ui.Renders;

namespace Util.Ui.NgZorro.Components.Cascaders; 

/// <summary>
/// 级联选择,生成的标签为&lt;nz-cascader&gt;&lt;/nz-cascader&gt;
/// </summary>
[HtmlTargetElement( "util-cascader" )]
public class CascaderTagHelper : FormControlTagHelperBase {
    /// <summary>
    /// 配置
    /// </summary>
    private Config _config;
    /// <summary>
    /// [nzAllowClear],是否允许清除,类型: boolean, 默认值: true
    /// </summary>
    public string AllowClear { get; set; }
    /// <summary>
    /// [nzAutoFocus],是否自动聚焦，当存在输入框时, 类型: boolean, 默认值: false
    /// </summary>
    public string AutoFocus { get; set; }
    /// <summary>
    /// [nzChangeOn],点击父级菜单项时，可通过该函数判断是否允许值的变化,类型: (option: any, index: number) => boolean
    /// </summary>
    public string ChangeOn { get; set; }
    /// <summary>
    /// [nzChangeOnSelect],点选每级菜单项时,是否值都会变化, 类型: boolean, 默认值: false
    /// </summary>
    public string ChangeOnSelect { get; set; }
    /// <summary>
    /// nzColumnClassName,自定义浮层列类名
    /// </summary>
    public string ColumnClassName { get; set; }
    /// <summary>
    /// [nzColumnClassName],自定义浮层列类名
    /// </summary>
    public string BindColumnClassName { get; set; }
    /// <summary>
    /// [nzDisabled],是否禁用, 默认值: false
    /// </summary>
    public string Disabled { get; set; }
    /// <summary>
    /// nzExpandIcon,自定义次级菜单展开图标
    /// </summary>
    public AntDesignIcon ExpandIcon { get; set; }
    /// <summary>
    /// [nzExpandIcon],自定义次级菜单展开图标, 类型: string|TemplateRef&lt;void>
    /// </summary>
    public string BindExpandIcon { get; set; }
    /// <summary>
    /// nzExpandTrigger,次级菜单的展开方式, 类型: 'click'|'hover',默认值: 'click'
    /// </summary>
    public CascaderExpandTrigger ExpandTrigger { get; set; }
    /// <summary>
    /// [nzExpandTrigger],次级菜单的展开方式, 类型: 'click'|'hover',默认值: 'click'
    /// </summary>
    public string BindExpandTrigger { get; set; }
    /// <summary>
    /// nzTriggerAction,触发操作, 类型: 'click'|'hover',默认值: 'click'
    /// </summary>
    public CascaderTriggerAction TriggerAction { get; set; }
    /// <summary>
    /// [nzTriggerAction],触发操作, 类型: 'click'|'hover',默认值: 'click'
    /// </summary>
    public string BindTriggerAction { get; set; }
    /// <summary>
    /// nzLabelProperty,选项显示文本的属性名, 默认值: 'label'
    /// </summary>
    public string LabelProperty { get; set; }
    /// <summary>
    /// [nzLabelProperty],选项显示文本的属性名, 默认值: 'label'
    /// </summary>
    public string BindLabelProperty { get; set; }
    /// <summary>
    /// nzValueProperty,选项实际值的属性名,默认值: 'value'
    /// </summary>
    public string ValueProperty { get; set; }
    /// <summary>
    /// [nzValueProperty],选项实际值的属性名,默认值: 'value'
    /// </summary>
    public string BindValueProperty { get; set; }
    /// <summary>
    /// [nzLabelRender],选择后展示的渲染模板, 类型: TemplateRef&lt;any>
    /// </summary>
    public string LabelRender { get; set; }
    /// <summary>
    /// [nzLoadData],动态加载选项函数,如果提供了ngModel初始值，且未提供nzOptions值，则会立即触发动态加载,函数定义: (option: any, index?: index) => PromiseLike&lt;any&gt;
    /// </summary>
    public string LoadData { get; set; }
    /// <summary>
    /// nzMenuClassName,自定义浮层类名
    /// </summary>
    public string MenuClassName { get; set; }
    /// <summary>
    /// [nzMenuClassName],自定义浮层类名
    /// </summary>
    public string BindMenuClassName { get; set; }
    /// <summary>
    /// [nzMenuStyle],自定义浮层样式
    /// </summary>
    public string MenuStyle { get; set; }
    /// <summary>
    /// nzNotFoundContent,下拉列表为空时显示的内容, 类型: string | TemplateRef&lt;void>
    /// </summary>
    public string NotFoundContent { get; set; }
    /// <summary>
    /// [nzNotFoundContent],下拉列表为空时显示的内容, 类型: string | TemplateRef&lt;void>
    /// </summary>
    public string BindNotFoundContent { get; set; }
    /// <summary>
    /// [nzOptionRender],选项的渲染模板, 类型: TemplateRef&lt;{ $implicit: NzCascaderOption, index: number }>
    /// </summary>
    public string OptionRender { get; set; }
    /// <summary>
    /// [nzOptions],可选项数据源, 类型: object[]
    /// </summary>
    public string Options { get; set; }
    /// <summary>
    /// nzPlaceHolder,输入框占位文本, 默认值: '请选择'
    /// </summary>
    public string Placeholder { get; set; }
    /// <summary>
    /// [nzPlaceHolder],输入框占位文本, 默认值: '请选择'
    /// </summary>
    public string BindPlaceholder { get; set; }
    /// <summary>
    /// [nzShowArrow],是否显示箭头,类型: boolean, 默认值: true
    /// </summary>
    public string ShowArrow { get; set; }
    /// <summary>
    /// [nzShowInput],是否显示输入框,默认值: true
    /// </summary>
    public string ShowInput { get; set; }
    /// <summary>
    /// [nzShowSearch],是否支持搜索，默认情况下对 label 进行全匹配搜索，不能和 [nzLoadData] 同时使用, 类型: boolean | NzShowSearchOptions ,默认值: false
    /// </summary>
    public string ShowSearch { get; set; }
    /// <summary>
    /// nzSize,输入框尺寸，可选值: 'large'|'small'|'default', 默认值: 'default'
    /// </summary>
    public InputSize Size { get; set; }
    /// <summary>
    /// [nzSize],输入框尺寸，可选值: 'large'|'small'|'default', 默认值: 'default'
    /// </summary>
    public string BindSize { get; set; }
    /// <summary>
    /// nzSuffixIcon,选择框后缀图标
    /// </summary>
    public AntDesignIcon SuffixIcon { get; set; }
    /// <summary>
    /// [nzSuffixIcon],选择框后缀图标, 类型: string | TemplateRef&lt;void>
    /// </summary>
    public string BindSuffixIcon { get; set; }
    /// <summary>
    /// nzStatus,校验状态, 可选值: 'error' | 'warning'
    /// </summary>
    public FormControlStatus Status { get; set; }
    /// <summary>
    /// [nzStatus],校验状态, 可选值: 'error' | 'warning'
    /// </summary>
    public string BindStatus { get; set; }
    /// <summary>
    /// [nzBackdrop],浮层是否应带有背景板,类型: boolean, 默认值: false
    /// </summary>
    public string Backdrop { get; set; }
    /// <summary>
    /// (nzClear),清除事件,清除值时触发, 类型: EventEmitter&lt;void>
    /// </summary>
    public string OnClear { get; set; }
    /// <summary>
    /// (nzVisibleChange),显示状态变化事件,菜单浮层显示/隐藏时触发, 类型: EventEmitter&lt;boolean>
    /// </summary>
    public string OnVisibleChange { get; set; }
    /// <summary>
    /// (nzSelectionChange),选中状态变化事件,值发生变化时触发, 类型: EventEmitter&lt;NzCascaderOption[]>
    /// </summary>
    public string OnSelectionChange { get; set; }

    /// <inheritdoc />
    protected override void ProcessBefore( TagHelperContext context, TagHelperOutput output ) {
        _config = new Config( context, output );
        var service = new SelectService( _config );
        service.DisableAutoNzFor();
        service.Init();
    }

    /// <inheritdoc />
    protected override IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        _config.Content = content;
        return new CascaderRender( _config );
    }
}